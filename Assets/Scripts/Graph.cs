using UnityEngine;

namespace BuildingAGraph
{
	public class Graph : MonoBehaviour
	{
		[SerializeField] private Transform _pointPrefab;
		[SerializeField, Range(1, 10000)] private int _pointNumber = 1;
		[SerializeField, Range(0, 2)] private int _function;
		
		private Transform[] _points;

		private void Awake()
		{
			BuildGraph();
		}

		private void Update()
		{
			AnimateGraph();
		}

		private void BuildGraph()
		{
			_points = new Transform[_pointNumber];
			
			for (var i = 0; i < _pointNumber; i++)
			{
				_points[i] = Instantiate(_pointPrefab, transform);
				var newLocalScale = _points[i].localScale * 2 / _pointNumber;
				_points[i].localScale = newLocalScale;
			}
		}
		
		private void AnimateGraph()
		{
			var time = Time.time;

			for (var i = 0; i < _pointNumber; i++)
			{
				var point = _points[i];
				
				var x = (i + 0.5f) * point.localScale.x - 1;
				float y;

				if (_function == 0) 
					y = FunctionLibrary.Wave(x, time);
				else if (_function == 1)
					y = FunctionLibrary.MultiWave(x, time);
				else
					y = FunctionLibrary.Ripple(x, time);


				_points[i].localPosition = new Vector3(x, y);
			}
		}
	}
}