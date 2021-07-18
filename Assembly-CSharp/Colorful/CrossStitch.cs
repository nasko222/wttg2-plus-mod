using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/artistic-effects/cross-stitch.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Artistic Effects/Cross Stitch")]
	public class CrossStitch : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetInt("_StitchSize", this.Size);
			base.Material.SetFloat("_Brightness", this.Brightness);
			int num = (!this.Invert) ? 0 : 1;
			if (this.Pixelize)
			{
				num += 2;
				base.Material.SetFloat("_Scale", (float)source.width / (float)this.Size);
				base.Material.SetFloat("_Ratio", (float)source.width / (float)source.height);
			}
			Graphics.Blit(source, destination, base.Material, num);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Cross Stitch";
		}

		[Range(1f, 128f)]
		[Tooltip("Works best with power of two values.")]
		public int Size = 8;

		[Range(0f, 10f)]
		[Tooltip("Brightness adjustment. Cross-stitching tends to lower the overall brightness, use this to compensate.")]
		public float Brightness = 1.5f;

		[Tooltip("Inverts the cross-stiching pattern.")]
		public bool Invert;

		[Tooltip("Should the original render be pixelized ?")]
		public bool Pixelize = true;
	}
}
