using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/negative.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Negative")]
	public class Negative : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetFloat("_Amount", this.Amount);
			Graphics.Blit(source, destination, base.Material, (!CLib.IsLinearColorSpace()) ? 0 : 1);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Negative";
		}

		[Range(0f, 1f)]
		[Tooltip("Blending factor.")]
		public float Amount = 1f;
	}
}
