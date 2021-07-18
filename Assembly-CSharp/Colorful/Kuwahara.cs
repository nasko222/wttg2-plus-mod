using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/artistic-effects/kuwahara.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Artistic Effects/Kuwahara")]
	public class Kuwahara : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.Radius = Mathf.Clamp(this.Radius, 1, 6);
			base.Material.SetVector("_PSize", new Vector2(1f / (float)source.width, 1f / (float)source.height));
			Graphics.Blit(source, destination, base.Material, this.Radius - 1);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Kuwahara";
		}

		[Range(1f, 6f)]
		[Tooltip("Larger radius will give a more abstract look but will lower performances.")]
		public int Radius = 3;
	}
}
