using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/letterbox.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Letterbox")]
	public class Letterbox : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float num = (float)source.width;
			float num2 = (float)source.height;
			float num3 = num / num2;
			int pass = 0;
			base.Material.SetColor("_FillColor", this.FillColor);
			float num4;
			if (num3 < this.Aspect)
			{
				num4 = (num2 - num / this.Aspect) * 0.5f / num2;
			}
			else
			{
				if (num3 <= this.Aspect)
				{
					Graphics.Blit(source, destination);
					return;
				}
				num4 = (num - num2 * this.Aspect) * 0.5f / num;
				pass = 1;
			}
			base.Material.SetVector("_Offsets", new Vector2(num4, 1f - num4));
			Graphics.Blit(source, destination, base.Material, pass);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Letterbox";
		}

		[Min(0f)]
		[Tooltip("Crop the screen to the given aspect ratio.")]
		public float Aspect = 2.33333325f;

		[Tooltip("Letter/Pillar box color. Alpha is transparency.")]
		public Color FillColor = Color.black;
	}
}
