using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BuildingAGraph
{
	public class Graph : MonoBehaviour
	{
		[SerializeField] private Transform _pointPrefab;
		[SerializeField, Range(1, 10000)] private int _pointNumber = 1;
		[SerializeField, Range(1f, 3000f)] private float _graphScale = 1f;

		private bool _rebuildIsNeeded;
		private readonly List<Transform> _points = new List<Transform>();
		private readonly FunctionCollection _functionCollection = new FunctionCollection();

		private Func<float, float> _function;

		private void OnValidate()
		{
			_rebuildIsNeeded = true;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				_function = _functionCollection.GetNext();
				_rebuildIsNeeded = true;
			}

			if (_rebuildIsNeeded) 
				BuildGraph();
		}
		
		private void BuildGraph()
		{
			if (_points.Count > 0) 
				DestroyPoints();

			for (var i = 0; i < _pointNumber; i++)
			{
				var point = (Transform) PrefabUtility.InstantiatePrefab(_pointPrefab, transform);
				
				var newLocalScale = point.localScale * _graphScale * 2 / _pointNumber;
				point.localScale = newLocalScale;

				var x = (i + 0.5f) * newLocalScale.x - _graphScale;
				var y = _function?.Invoke(x) ?? x;

				if (y is float.NaN)
					continue;

				point.position = new Vector3(x, y);
				_points.Add(point);
			}

			_rebuildIsNeeded = false;
		}

		private void DestroyPoints()
		{
			foreach (var point in _points)
				Destroy(point.gameObject);

			_points.Clear();
		}
	}
}