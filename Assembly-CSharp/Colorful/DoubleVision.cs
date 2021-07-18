using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/double-vision.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Double Vision")]
	public class DoubleVision : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Amount <= 0f || this.Displace == Vector2.zero)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetVector("_Displace", new Vector2(this.Displace.x / (float)source.width, this.Displace.y / (float)source.height));
			base.Material.SetFloat("_Amount", this.Amount);
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Double Vision";
		}

		[Tooltip("Diploplia strength.")]
		public Vector2 Displace = new Vector2(0.7f, 0f);

		[Range(0f, 1f)]
		[Tooltip("Blending factor.")]
		public float Amount = 1f;
	}
}
