using UnityEngine;
using static UnityEngine.Mathf;

namespace BuildingAGraph.Graph.CPU
{
	public static class FunctionLibrary
	{
		private static readonly Function[] Functions = { Wave, MultiWave, Ripple, Sphere, Torus };

		public delegate Vector3 Function(float u, float v, float t);

		public enum FunctionName { Wave, MultiWave, Ripple, Sphere, Torus }

		public static Function GetFunction(FunctionName functionName)
		{
			return Functions[(int) functionName];
		}
		
		public static FunctionName GetNextFunctionName(FunctionName functionName)
		{
			return (int) functionName + 1 < Functions.Length ? functionName + 1 : 0;
		}

		public static FunctionName GetRandomFunctionNameOtherThan(FunctionName functionName)
		{
			var choice = (FunctionName) Random.Range(1, Functions.Length);
			return choice == functionName ? 0 : choice;
		}

		public static Vector3 Morph(float u, float v, float t, Function from, Function to, float progress)
		{
			return Vector3.LerpUnclamped(
				from(u, v, t), 
				to(u, v, t), 
				SmoothStep(0f, 1f, progress));
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

		public static Vector3 Sphere(float u, float v, float t)
		{
			var r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t)); 
			var s = r * Cos(0.5f * PI * v);
			
			Vector3 point;
			point.x = s * Sin(PI * u);
			point.y = r * Sin(0.5f * PI * v);
			point.z = s * Cos(PI * u);

			return point;
		}
		
		public static Vector3 Torus(float u, float v, float t)
		{
			var r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
			var r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
			var s = r1 + r2 * Cos(PI * v);
			
			Vector3 point;
			point.x = s * Sin(PI * u);
			point.y = r2 * Sin(0.5f * PI * v);
			point.z = s * Cos(PI * u);

			return point;
		}
	}
}