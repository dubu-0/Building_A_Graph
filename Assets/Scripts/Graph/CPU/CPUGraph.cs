using UnityEngine;
using static BuildingAGraph.Graph.CPU.FunctionLibrary;

namespace BuildingAGraph.Graph.CPU
{
	public class CPUGraph : MonoBehaviour
	{
		[SerializeField] private Transform _pointPrefab;
		[SerializeField, Range(10, 175)] private int _resolution = 10;
		[SerializeField] private FunctionName _function;
		[SerializeField, Range(0f, 10f)] private float _functionDuration = 1f;
		[SerializeField, Range(0f, 10f)] private float _transitionDuration = 1f;
		[SerializeField] private TransitionMode _transitionMode;
		
		private Transform[] _points;
		private float _currentDuration;
		private FunctionName _transitionFunction;
		private bool _transitioning;
		
		private enum TransitionMode { Cycle, Random }

		private void Awake()
		{
			BuildGraph();
		}

		private void Update()
		{
			_currentDuration += Time.deltaTime;

			if (_transitioning && _currentDuration >= _transitionDuration)
			{
				_currentDuration -= _transitionDuration;
				_transitioning = false;
			}
			else if (_currentDuration >= _functionDuration)
			{
				_currentDuration -= _functionDuration;
				SwitchFunction();
			}

			if (_transitioning)
				UpdateFunctionTransition();
			else
				UpdateFunction();
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
			_transitioning = true;
			_transitionFunction = _function;
			_function = PickNextFunction();
		}

		private FunctionName PickNextFunction()
		{
			return _transitionMode == TransitionMode.Cycle ?
				GetNextFunctionName(_function) :
				GetRandomFunctionNameOtherThan(_function);
		}

		private void UpdateFunctionTransition()
		{
			var from = GetFunction(_transitionFunction);
			var to = GetFunction(_function);
			var progress = _currentDuration / _transitionDuration;
			
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

				_points[i].localPosition = Morph(u, v, time, from, to, progress);
			}
		}

		private void UpdateFunction()
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