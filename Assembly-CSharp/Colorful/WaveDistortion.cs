using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/wave-distortion.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Wave Distortion")]
	public class WaveDistortion : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float num = CLib.Frac(this.Phase);
			if (num == 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetVector("_Params", new Vector4(this.Amplitude, this.Waves, this.ColorGlitch, num));
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Wave Distortion";
		}

		[Range(0f, 1f)]
		[Tooltip("Wave amplitude.")]
		public float Amplitude = 0.6f;

		[Tooltip("Amount of waves.")]
		public float Waves = 5f;

		[Range(0f, 5f)]
		[Tooltip("Amount of color shifting.")]
		public float ColorGlitch = 0.35f;

		[Tooltip("Distortion state. Think of it as a bell curve going from 0 to 1, with 0.5 being the highest point.")]
		public float Phase = 0.35f;
	}
}
