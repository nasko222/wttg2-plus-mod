using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/artistic-effects/dithering.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Artistic Effects/Dithering")]
	public class Dithering : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.m_DitherPattern == null)
			{
				this.m_DitherPattern = Resources.Load<Texture2D>("Misc/DitherPattern");
			}
			base.Material.SetTexture("_Pattern", this.m_DitherPattern);
			base.Material.SetVector("_Params", new Vector4(this.RedLuminance, this.GreenLuminance, this.BlueLuminance, this.Amount));
			int num = (!this.ShowOriginal) ? 0 : 4;
			num += ((!this.ConvertToGrayscale) ? 0 : 2);
			num += ((!CLib.IsLinearColorSpace()) ? 0 : 1);
			Graphics.Blit(source, destination, base.Material, num);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Dithering";
		}

		[Tooltip("Show the original picture under the dithering pass.")]
		public bool ShowOriginal;

		[Tooltip("Convert the original render to black & white.")]
		public bool ConvertToGrayscale;

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

		protected Texture2D m_DitherPattern;
	}
}
