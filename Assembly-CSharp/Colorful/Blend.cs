using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/other-effects/blend.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Other Effects/Blend")]
	public class Blend : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Texture == null || this.Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetTexture("_OverlayTex", this.Texture);
			base.Material.SetFloat("_Amount", this.Amount);
			Graphics.Blit(source, destination, base.Material, (int)this.Mode);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Blend";
		}

		[Tooltip("The Texture2D, RenderTexture or MovieTexture to blend.")]
		public Texture Texture;

		[Range(0f, 1f)]
		[Tooltip("Blending factor.")]
		public float Amount = 1f;

		[Tooltip("Blending mode.")]
		public Blend.BlendingMode Mode;

		public enum BlendingMode
		{
			Darken,
			Multiply,
			ColorBurn,
			LinearBurn,
			DarkerColor,
			Lighten = 6,
			Screen,
			ColorDodge,
			LinearDodge,
			LighterColor,
			Overlay = 12,
			SoftLight,
			HardLight,
			VividLight,
			LinearLight,
			PinLight,
			HardMix,
			Difference = 20,
			Exclusion,
			Subtract,
			Divide
		}
	}
}
