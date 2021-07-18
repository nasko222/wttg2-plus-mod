using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/grayscale.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Grayscale")]
	public class Grayscale : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetVector("_Params", new Vector4(this.RedLuminance, this.GreenLuminance, this.BlueLuminance, this.Amount));
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Grayscale";
		}

		[Range(0f, 1f)]
		[Tooltip("Amount of red to contribute to the luminosity.")]
		public float RedLuminance = 0.299f;

		[Range(0f, 1f)]
		[Tooltip("Amount of green to contribute to the luminosity.")]
		public float GreenLuminance = 0.587f;

		[Range(0f, 1f)]
		[Tooltip("Amount of blue to contribute to the luminosity.")]
		public float BlueLuminance = 0.114f;

		[Range(0f, 1f)]
		[Tooltip("Blending factor.")]
		public float Amount = 1f;
	}
}
