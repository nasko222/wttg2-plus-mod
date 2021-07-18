using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/threshold.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Threshold")]
	public class Threshold : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetFloat("_Threshold", this.Value / 255f);
			base.Material.SetFloat("_Range", this.NoiseRange / 255f);
			Graphics.Blit(source, destination, base.Material, (!this.UseNoise) ? 0 : 1);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Threshold";
		}

		[Range(1f, 255f)]
		[Tooltip("Luminosity threshold.")]
		public float Value = 128f;

		[Range(0f, 128f)]
		[Tooltip("Aomunt of randomization.")]
		public float NoiseRange = 24f;

		[Tooltip("Adds some randomization to the threshold value.")]
		public bool UseNoise;
	}
}
