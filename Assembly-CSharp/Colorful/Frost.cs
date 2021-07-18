using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/frost.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Frost")]
	public class Frost : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Scale <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetFloat("_Scale", this.Scale);
			if (this.EnableVignette)
			{
				base.Material.SetFloat("_Sharpness", this.Sharpness * 0.01f);
				base.Material.SetFloat("_Darkness", this.Darkness * 0.02f);
			}
			Graphics.Blit(source, destination, base.Material, (!this.EnableVignette) ? 0 : 1);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Frost";
		}

		[Range(0f, 16f)]
		[Tooltip("Frosting strength.")]
		public float Scale = 1.2f;

		[Range(-100f, 100f)]
		[Tooltip("Smoothness of the vignette effect.")]
		public float Sharpness = 40f;

		[Range(0f, 100f)]
		[Tooltip("Amount of vignetting on screen.")]
		public float Darkness = 35f;

		[Tooltip("Should the effect be applied like a vignette ?")]
		public bool EnableVignette = true;
	}
}
