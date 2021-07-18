using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/other-effects/convolution-3x3.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Other Effects/Convolution Matrix 3x3")]
	public class Convolution3x3 : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetVector("_PSize", new Vector2(1f / (float)source.width, 1f / (float)source.height));
			base.Material.SetVector("_KernelT", this.KernelTop / this.Divisor);
			base.Material.SetVector("_KernelM", this.KernelMiddle / this.Divisor);
			base.Material.SetVector("_KernelB", this.KernelBottom / this.Divisor);
			base.Material.SetFloat("_Amount", this.Amount);
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Convolution 3x3";
		}

		public Vector3 KernelTop = Vector3.zero;

		public Vector3 KernelMiddle = Vector3.up;

		public Vector3 KernelBottom = Vector3.zero;

		[Tooltip("Used to normalize the kernel.")]
		public float Divisor = 1f;

		[Range(0f, 1f)]
		[Tooltip("Blending factor.")]
		public float Amount = 1f;
	}
}
