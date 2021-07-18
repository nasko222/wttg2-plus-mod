using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/white-balance.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/White Balance")]
	public class WhiteBalance : BaseEffect
	{
		protected virtual void Reset()
		{
			this.White = ((!CLib.IsLinearColorSpace()) ? new Color(0.5f, 0.5f, 0.5f) : new Color(0.72974f, 0.72974f, 0.72974f));
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetColor("_White", this.White);
			Graphics.Blit(source, destination, base.Material, (int)this.Mode);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/White Balance";
		}

		[ColorUsage(false)]
		[Tooltip("Reference white point or midtone value.")]
		public Color White = new Color(0.5f, 0.5f, 0.5f);

		[Tooltip("Algorithm used.")]
		public WhiteBalance.BalanceMode Mode = WhiteBalance.BalanceMode.Complex;

		public enum BalanceMode
		{
			Simple,
			Complex
		}
	}
}
