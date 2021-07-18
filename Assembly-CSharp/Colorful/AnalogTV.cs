using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/analog-tv.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Analog TV")]
	public class AnalogTV : BaseEffect
	{
		protected virtual void Update()
		{
			if (this.AutomaticPhase)
			{
				if (this.Phase > 1000f)
				{
					this.Phase = 10f;
				}
				this.Phase += Time.deltaTime * 0.25f;
			}
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_Params1", new Vector4(this.NoiseIntensity, this.ScanlinesIntensity, (float)this.ScanlinesCount, this.ScanlinesOffset));
			base.Material.SetVector("_Params2", new Vector4(this.Phase, this.Distortion, this.CubicDistortion, this.Scale));
			int num = (!this.VerticalScanlines) ? 0 : 2;
			num += ((!this.ConvertToGrayscale) ? 0 : 1);
			Graphics.Blit(source, destination, base.Material, num);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Analog TV";
		}

		[Tooltip("Automatically animate the Phase value.")]
		public bool AutomaticPhase = true;

		[Tooltip("Current noise phase. Consider this a seed value.")]
		public float Phase = 0.5f;

		[Tooltip("Convert the original render to black & white.")]
		public bool ConvertToGrayscale;

		[Range(0f, 1f)]
		[Tooltip("Noise brightness. Will impact the scanlines visibility.")]
		public float NoiseIntensity = 0.5f;

		[Range(0f, 10f)]
		[Tooltip("Scanline brightness. Depends on the NoiseIntensity value.")]
		public float ScanlinesIntensity = 2f;

		[Range(0f, 4096f)]
		[Tooltip("The number of scanlines to draw.")]
		public int ScanlinesCount = 768;

		[Tooltip("Scanline offset. Gives a cool screen scanning effect when animated.")]
		public float ScanlinesOffset;

		[Tooltip("Uses vertical scanlines.")]
		public bool VerticalScanlines;

		[Range(-2f, 2f)]
		[Tooltip("Spherical distortion factor.")]
		public float Distortion = 0.2f;

		[Range(-2f, 2f)]
		[Tooltip("Cubic distortion factor.")]
		public float CubicDistortion = 0.6f;

		[Range(0.01f, 2f)]
		[Tooltip("Helps avoid screen streching on borders when working with heavy distortions.")]
		public float Scale = 0.8f;
	}
}
