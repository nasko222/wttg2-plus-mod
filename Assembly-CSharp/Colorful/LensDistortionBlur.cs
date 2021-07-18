using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/blur-effects/lens-distortion-blur.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Blur Effects/Lens Distortion Blur")]
	public class LensDistortionBlur : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int num = (int)((this.Quality != LensDistortionBlur.QualityPreset.Custom) ? this.Quality : ((LensDistortionBlur.QualityPreset)this.Samples));
			base.Material.SetVector("_Params", new Vector4((float)num, this.Distortion / (float)num, this.CubicDistortion / (float)num, this.Scale));
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/LensDistortionBlur";
		}

		[Tooltip("Quality preset. Higher means better quality but slower processing.")]
		public LensDistortionBlur.QualityPreset Quality = LensDistortionBlur.QualityPreset.Medium;

		[Range(2f, 32f)]
		[Tooltip("Sample count. Higher means better quality but slower processing.")]
		public int Samples = 10;

		[Range(-2f, 2f)]
		[Tooltip("Spherical distortion factor.")]
		public float Distortion = 0.2f;

		[Range(-2f, 2f)]
		[Tooltip("Cubic distortion factor.")]
		public float CubicDistortion = 0.6f;

		[Range(0.01f, 2f)]
		[Tooltip("Helps avoid screen streching on borders when working with heavy distortions.")]
		public float Scale = 0.8f;

		public enum QualityPreset
		{
			Low = 4,
			Medium = 8,
			High = 12,
			Custom
		}
	}
}
