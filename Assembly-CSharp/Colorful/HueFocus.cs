using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/hue-focus.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Hue Focus")]
	public class HueFocus : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float num = this.Hue / 360f;
			float num2 = this.Range / 180f;
			base.Material.SetVector("_Range", new Vector2(num - num2, num + num2));
			base.Material.SetVector("_Params", new Vector3(num, this.Boost + 1f, this.Amount));
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Hue Focus";
		}

		[Range(0f, 360f)]
		[Tooltip("Center hue.")]
		public float Hue;

		[Range(1f, 180f)]
		[Tooltip("Hue range to focus on.")]
		public float Range = 30f;

		[Range(0f, 1f)]
		[Tooltip("Makes the colored pixels more vibrant.")]
		public float Boost = 0.5f;

		[Range(0f, 1f)]
		[Tooltip("Blending Factor.")]
		public float Amount = 1f;
	}
}
