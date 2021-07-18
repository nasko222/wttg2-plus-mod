using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/s-curve-contrast.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/S-Curve Contrast")]
	public class SCurveContrast : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_Red", new Vector2(this.RedSteepness, this.RedGamma));
			base.Material.SetVector("_Green", new Vector2(this.GreenSteepness, this.GreenGamma));
			base.Material.SetVector("_Blue", new Vector2(this.BlueSteepness, this.BlueGamma));
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/SCurveContrast";
		}

		public float RedSteepness = 1f;

		public float RedGamma = 1f;

		public float GreenSteepness = 1f;

		public float GreenGamma = 1f;

		public float BlueSteepness = 1f;

		public float BlueGamma = 1f;
	}
}
