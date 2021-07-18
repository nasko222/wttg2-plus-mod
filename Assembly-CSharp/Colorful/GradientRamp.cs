using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/gradient-ramp.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Gradient Ramp")]
	public class GradientRamp : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.RampTexture == null || this.Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetTexture("_RampTex", this.RampTexture);
			base.Material.SetFloat("_Amount", this.Amount);
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Gradient Ramp";
		}

		[Tooltip("Texture used to remap the pixels luminosity.")]
		public Texture RampTexture;

		[Range(0f, 1f)]
		[Tooltip("Blending factor.")]
		public float Amount = 1f;
	}
}
