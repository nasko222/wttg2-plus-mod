using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/rgb-split.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/RGB Split")]
	public class RGBSplit : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Amount == 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetVector("_Params", new Vector3(this.Amount * 0.001f, Mathf.Sin(this.Angle), Mathf.Cos(this.Angle)));
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/RGB Split";
		}

		[Tooltip("RGB shifting amount.")]
		public float Amount;

		[Tooltip("Shift direction in radians.")]
		public float Angle;
	}
}
