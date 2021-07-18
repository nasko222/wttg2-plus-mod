using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/chromatic-aberration.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Chromatic Aberration")]
	public class ChromaticAberration : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_Refraction", new Vector3(this.RedRefraction, this.GreenRefraction, this.BlueRefraction));
			Graphics.Blit(source, destination, base.Material, (!this.PreserveAlpha) ? 0 : 1);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Chromatic Aberration";
		}

		[Range(0.9f, 1.1f)]
		[Tooltip("Indice of refraction for the red channel.")]
		public float RedRefraction = 1f;

		[Range(0.9f, 1.1f)]
		[Tooltip("Indice of refraction for the green channel.")]
		public float GreenRefraction = 1.005f;

		[Range(0.9f, 1.1f)]
		[Tooltip("Indice of refraction for the blue channel.")]
		public float BlueRefraction = 1.01f;

		[Tooltip("Enable this option if you need the effect to keep the alpha channel from the original render (some effects like Glow will need it). Disable it otherwise for better performances.")]
		public bool PreserveAlpha;
	}
}
