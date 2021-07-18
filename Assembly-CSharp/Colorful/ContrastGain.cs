using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/contrast-gain.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Contrast Gain")]
	public class ContrastGain : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetFloat("_Gain", this.Gain);
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Contrast Gain";
		}

		[Range(0.001f, 2f)]
		[Tooltip("Steepness of the contrast curve. 1 is linear, no contrast change.")]
		public float Gain = 1f;
	}
}
