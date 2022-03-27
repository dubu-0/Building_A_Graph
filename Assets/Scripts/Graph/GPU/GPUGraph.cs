using UnityEngine;
using static BuildingAGraph.Graph.CPU.FunctionLibrary;

namespace BuildingAGraph.Graph.GPU
{
	public class GPUGraph : MonoBehaviour
	{
		private const int FloatSize = 4;
		private const int Vector3Size = 3 * FloatSize;
		
		private static readonly int PositionsID = Shader.PropertyToID("_Positions");
		private static readonly int ResolutionID = Shader.PropertyToID("_Resolution");
		private static readonly int StepID = Shader.PropertyToID("_Step");
		private static readonly int TimeID = Shader.PropertyToID("_Time");

		[SerializeField] private ComputeShader _computeShader;
		[SerializeField] private Material _material;
		[SerializeField] private Mesh _mesh;
		[SerializeField, Range(1, 3000)] private int _resolution = 1;
		[SerializeField] private FunctionName _function;
		[SerializeField, Range(0f, 10f)] private float _functionDuration = 1f;
		[SerializeField, Range(0f, 10f)] private float _transitionDuration = 1f;
		[SerializeField] private TransitionMode _transitionMode;
		
		private float _currentDuration;
		private FunctionName _transitionFunction;
		private bool _transitioning;
		private ComputeBuffer _positionsBuffer;

		private enum TransitionMode { Cycle, Random }

		private void OnEnable()
		{
			_positionsBuffer = new ComputeBuffer(_resolution * _resolution, Vector3Size);
		}

		private void OnDisable()
		{
			_positionsBuffer.Release();
			_positionsBuffer = null;
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

			UpdateFunctionOnGPU();
		}

		private void BuildGraph()
		{
			var step = 2f / _resolution;
			var scale = Vector3.one * step;

		}

		private void UpdateFunctionOnGPU()
		{
			var step = 2f / _resolution;
			
			_computeShader.SetInt(ResolutionID, _resolution);
			_computeShader.SetFloat(StepID, step);
			_computeShader.SetFloat(TimeID, Time.time);
			
			_computeShader.SetBuffer(0, PositionsID, _positionsBuffer);

			var groups = Mathf.CeilToInt(_resolution / 8f);
			_computeShader.Dispatch(0, groups, groups, 1);

			_material.SetBuffer(PositionsID, _positionsBuffer);
			_material.SetFloat(StepID, step);
			
			var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / _resolution));
			Graphics.DrawMeshInstancedProcedural(_mesh, 0, _material, bounds, _positionsBuffer.count);
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
	}
}