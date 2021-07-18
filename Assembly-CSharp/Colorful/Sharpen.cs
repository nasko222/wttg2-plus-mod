using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/other-effects/sharpen.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Other Effects/Sharpen")]
	public class Sharpen : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Strength == 0f || this.Clamp == 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetVector("_Params", new Vector4(this.Strength, this.Clamp, 1f / (float)source.width, 1f / (float)source.height));
			Graphics.Blit(source, destination, base.Material, (int)this.Mode);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Sharpen";
		}

		[Tooltip("Sharpening algorithm to use.")]
		public Sharpen.Algorithm Mode = Sharpen.Algorithm.TypeB;

		[Range(0f, 5f)]
		[Tooltip("Sharpening Strength.")]
		public float Strength = 0.6f;

		[Range(0f, 1f)]
		[Tooltip("Limits the amount of sharpening a pixel will receive.")]
		public float Clamp = 0.05f;

		public enum Algorithm
		{
			TypeA,
			TypeB
		}
	}
}
