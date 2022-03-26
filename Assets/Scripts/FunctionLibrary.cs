using static UnityEngine.Mathf;

namespace BuildingAGraph
{
	public static class FunctionLibrary
	{
		public static float Wave(float x, float t)
		{
			return Sin(PI * (x + t));
		}
		
		public static float MultiWave(float x, float t)
		{
			var y = Wave(x, 0.5f * t) + Wave(2 * x, 2 * t) / 2;
			return y / 1.5f;
		}

		public static float Ripple(float x, float t)
		{
			var d = Abs(x);
			var y = Sin(PI * (4f * d - t));
			return y / (1f + 10f * d);
		}
	}
}