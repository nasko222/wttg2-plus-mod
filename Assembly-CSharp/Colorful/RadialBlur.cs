using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/blur-effects/radial-blur.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Blur Effects/Radial Blur")]
	public class RadialBlur : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Strength <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			int num = (int)((this.Quality != RadialBlur.QualityPreset.Custom) ? this.Quality : ((RadialBlur.QualityPreset)this.Samples));
			base.Material.SetVector("_Center", this.Center);
			base.Material.SetVector("_Params", new Vector4(this.Strength, (float)num, this.Sharpness * 0.01f, this.Darkness * 0.02f));
			Graphics.Blit(source, destination, base.Material, (!this.EnableVignette) ? 0 : 1);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Radial Blur";
		}

		[Range(0f, 1f)]
		[Tooltip("Blur strength.")]
		public float Strength = 0.1f;

		[Range(2f, 32f)]
		[Tooltip("Sample count. Higher means better quality but slower processing.")]
		public int Samples = 10;

		[Tooltip("Focus point.")]
		public Vector2 Center = new Vector2(0.5f, 0.5f);

		[Tooltip("Quality preset. Higher means better quality but slower processing.")]
		public RadialBlur.QualityPreset Quality = RadialBlur.QualityPreset.Medium;

		[Range(-100f, 100f)]
		[Tooltip("Smoothness of the vignette effect.")]
		public float Sharpness = 40f;

		[Range(0f, 100f)]
		[Tooltip("Amount of vignetting on screen.")]
		public float Darkness = 35f;

		[Tooltip("Should the effect be applied like a vignette ?")]
		public bool EnableVignette = true;

		public enum QualityPreset
		{
			Low = 4,
			Medium = 8,
			High = 12,
			Custom
		}
	}
}
