using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingAGraph
{
	public class FunctionCollection
	{
		private readonly List<Func<float, float>> _functions = new List<Func<float, float>>();

		private int _current;
		
		public FunctionCollection()
		{
			_functions.Add(Square);
			_functions.Add(Sine);
			_functions.Add(SquareRoot);
			_functions.Add(Cube);
		}

		public Func<float, float> GetNext()
		{
			var next = _functions[_current];
			
			_current++;
			if (_current == _functions.Count) 
				_current = 0;

			return next;
		}
		
		private float Square(float value)
		{
			return value * value;
		}

		private float Sine(float value)
		{
			return Mathf.Sin(value);
		}

		private float SquareRoot(float value)
		{
			return Mathf.Sqrt(value);
		}

		private float Cube(float value)
		{
			return value * value * value;
		}
	}
}