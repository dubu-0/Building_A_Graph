using UnityEngine;
using static BuildingAGraph.FunctionLibrary;

namespace BuildingAGraph
{
	public class Graph : MonoBehaviour
	{
		[SerializeField] private Transform _pointPrefab;
		[SerializeField, Range(1, 10000)] private int _resolution = 1;
		[SerializeField] private FunctionName _function;
		[SerializeField, Range(0f, 10f)] private float _functionDuration = 1f;
		
		private Transform[] _points;
		private float _currentDuration;

		private void Awake()
		{
			BuildGraph();
		}

		private void Update()
		{
			SwitchFunction();
			AnimateGraph();
		}

		private void BuildGraph()
		{
			var step = 2f / _resolution;
			var scale = Vector3.one * step;
			_points = new Transform[_resolution * _resolution];

			for (var i = 0; i < _points.Length; i++)
			{
				var point = _points[i] = Instantiate(_pointPrefab, transform, false);
				point.localScale = scale;
			}
		}

		private void SwitchFunction()
		{
			_currentDuration += Time.deltaTime;

			if (_currentDuration >= _functionDuration)
			{
				_currentDuration -= _functionDuration;
				_function = GetNextFunctionName(_function);
			}
		}

		private void AnimateGraph()
		{
			var time = Time.time;
			var step = 2f / _resolution;
			var v = 0.5f * step - 1;

			for (int i = 0, x = 0, z = 0; i < _points.Length; i++, x++)
			{
				if (x == _resolution)
				{
					x = 0;
					z++;
					v = (z + 0.5f) * step - 1;
				}
				
				var u = (x + 0.5f) * step - 1;

				_points[i].localPosition = GetFunction(_function).Invoke(u, v, time);
			}
		}
	}
}