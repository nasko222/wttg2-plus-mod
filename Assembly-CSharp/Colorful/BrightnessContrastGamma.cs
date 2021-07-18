using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/brightness-contrast-gamma.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Brightness, Contrast, Gamma")]
	public class BrightnessContrastGamma : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Brightness == 0f && this.Contrast == 0f && this.Gamma == 1f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetVector("_BCG", new Vector4((this.Brightness + 100f) * 0.01f, (this.Contrast + 100f) * 0.01f, 1f / this.Gamma));
			base.Material.SetVector("_Coeffs", this.ContrastCoeff);
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Brightness Contrast Gamma";
		}

		[Range(-100f, 100f)]
		[Tooltip("Moving the slider to the right increases tonal values and expands highlights, to the left decreases values and expands shadows.")]
		public float Brightness;

		[Range(-100f, 100f)]
		[Tooltip("Expands or shrinks the overall range of tonal values.")]
		public float Contrast;

		public Vector3 ContrastCoeff = new Vector3(0.5f, 0.5f, 0.5f);

		[Range(0.1f, 9.9f)]
		[Tooltip("Simple power function.")]
		public float Gamma = 1f;
	}
}
