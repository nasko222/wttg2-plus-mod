using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/other-effects/noise.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Other Effects/Noise")]
	public class Noise : BaseEffect
	{
		protected virtual void Update()
		{
			if (this.Animate)
			{
				if (this.Seed > 1000f)
				{
					this.Seed = 0.5f;
				}
				this.Seed += Time.deltaTime * 0.25f;
			}
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_Params", new Vector3(this.Seed, this.Strength, this.LumContribution));
			int num = (this.Mode != Noise.ColorMode.Monochrome) ? 1 : 0;
			num += ((this.LumContribution <= 0f) ? 0 : 2);
			Graphics.Blit(source, destination, base.Material, num);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Noise";
		}

		[Tooltip("Black & white or colored noise.")]
		public Noise.ColorMode Mode;

		[Tooltip("Automatically increment the seed to animate the noise.")]
		public bool Animate = true;

		[Tooltip("A number used to initialize the noise generator.")]
		public float Seed = 0.5f;

		[Range(0f, 1f)]
		[Tooltip("Strength used to apply the noise. 0 means no noise at all, 1 is full noise.")]
		public float Strength = 0.12f;

		[Range(0f, 1f)]
		[Tooltip("Reduce the noise visibility in luminous areas.")]
		public float LumContribution;

		public enum ColorMode
		{
			Monochrome,
			RGB
		}
	}
}
