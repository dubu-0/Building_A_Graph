using UnityEngine;
using static UnityEngine.Mathf;

namespace BuildingAGraph
{
	public static class FunctionLibrary
	{
		private static readonly Function[] Functions = { Wave, MultiWave, Ripple };

		public delegate Vector3 Function(float u, float v, float t);

		public enum FunctionName { Wave, MultiWave, Ripple }

		public static Function GetFunction(FunctionName functionName)
		{
			return Functions[(int) functionName];
		}
		
		public static Vector3 Wave(float u, float v, float t)
		{
			Vector3 point;
			point.x = u;
			point.y = Sin(PI * (u + v + t));
			point.z = v;
			
			return point;
		}
		
		public static Vector3 MultiWave(float u, float v, float t)
		{
			Vector3 point;
			point.x = u;
			point.y = (1f / 2.5f) * (Sin(PI * (u + 0.5f * t)) + 0.5f * Sin(2f * PI * (v + t)) + Sin(PI * (u + v + 0.25f * t)));
			point.z = v;

			return  point;
		}

		public static Vector3 Ripple(float u, float v, float t)
		{
			var d = Sqrt(u * u + v * v);
			
			Vector3 point;
			point.x = u;
			point.y = Sin(PI * (4f * d - t)) / (1f + 10f * d);
			point.z = v;
			
			return point;
		}
	}
}