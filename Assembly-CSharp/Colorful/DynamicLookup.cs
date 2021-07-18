using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/dynamic-lookup.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Dynamic Lookup")]
	public class DynamicLookup : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetColor("_White", this.White);
			base.Material.SetColor("_Black", this.Black);
			base.Material.SetColor("_Red", this.Red);
			base.Material.SetColor("_Green", this.Green);
			base.Material.SetColor("_Blue", this.Blue);
			base.Material.SetColor("_Yellow", this.Yellow);
			base.Material.SetColor("_Magenta", this.Magenta);
			base.Material.SetColor("_Cyan", this.Cyan);
			base.Material.SetFloat("_Amount", this.Amount);
			Graphics.Blit(source, destination, base.Material, (!CLib.IsLinearColorSpace()) ? 0 : 1);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/DynamicLookup";
		}

		[ColorUsage(false)]
		public Color White = new Color(1f, 1f, 1f);

		[ColorUsage(false)]
		public Color Black = new Color(0f, 0f, 0f);

		[ColorUsage(false)]
		public Color Red = new Color(1f, 0f, 0f);

		[ColorUsage(false)]
		public Color Green = new Color(0f, 1f, 0f);

		[ColorUsage(false)]
		public Color Blue = new Color(0f, 0f, 1f);

		[ColorUsage(false)]
		public Color Yellow = new Color(1f, 1f, 0f);

		[ColorUsage(false)]
		public Color Magenta = new Color(1f, 0f, 1f);

		[ColorUsage(false)]
		public Color Cyan = new Color(0f, 1f, 1f);

		[Range(0f, 1f)]
		[Tooltip("Blending factor.")]
		public float Amount = 1f;
	}
}
