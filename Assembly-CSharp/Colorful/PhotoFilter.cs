using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/photo-filter.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Photo Filter")]
	public class PhotoFilter : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Density <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetColor("_RGB", this.Color);
			base.Material.SetFloat("_Density", this.Density);
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Photo Filter";
		}

		[ColorUsage(false)]
		[Tooltip("Lens filter color.")]
		public Color Color = new Color(1f, 0.5f, 0.2f, 1f);

		[Range(0f, 1f)]
		[Tooltip("Blending factor.")]
		public float Density = 0.35f;
	}
}
