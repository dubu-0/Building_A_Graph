using UnityEngine;
using TMPro;

namespace BuildingAGraph
{
	public class FrameRateCounter : MonoBehaviour
	{
		private const string FPSFormat = "FPS\n{0:0}\n{1:0}\n{2:0}";
		private const string MSFormat = "MS\n{0:1}\n{1:1}\n{2:1}";
		
		[SerializeField] private TextMeshProUGUI _text;
		[SerializeField, Range(0.1f, 2f)] private float _sampleDuration = 1f;
		[SerializeField] private DisplayMode _displayMode;

		private float _bestDuration = float.MaxValue;
		private float _totalDuration;
		private float _worstDuration = float.MinValue;
		private int _frameCount;
		
		public enum DisplayMode { FPS, MS }
		
		private void Update()
		{
			_frameCount++;
			
			var frameDuration = Time.unscaledDeltaTime;
			_totalDuration += frameDuration;
			
			if (_bestDuration > frameDuration)
				_bestDuration = frameDuration;

			if (_worstDuration < frameDuration)
				_worstDuration = frameDuration;

			if (_totalDuration >= _sampleDuration)
			{
				if (_displayMode == DisplayMode.FPS)
				{
					_text.SetText(FPSFormat, 
						1f / _bestDuration,
						_frameCount / _totalDuration,
						1f / _worstDuration);
				}
				else if (_displayMode == DisplayMode.MS)
				{
					_text.SetText(MSFormat,
						1000f * _bestDuration,
						1000f * _totalDuration / _frameCount,
						1000f * _worstDuration);
				}
				
				_frameCount = 0;
				_bestDuration = float.MaxValue;
				_totalDuration = 0f;
				_worstDuration = float.MinValue;
			}
		}
	}
}