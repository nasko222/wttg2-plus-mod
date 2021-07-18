using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/gradient-ramp-dynamic.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Gradient Ramp (Dynamic)")]
	public class GradientRampDynamic : BaseEffect
	{
		protected override void Start()
		{
			base.Start();
			if (this.Ramp != null)
			{
				this.UpdateGradientCache();
			}
		}

		protected virtual void Reset()
		{
			this.Ramp = new Gradient();
			this.Ramp.colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(Color.black, 0f),
				new GradientColorKey(Color.white, 1f)
			};
			this.Ramp.alphaKeys = new GradientAlphaKey[]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 1f)
			};
			this.UpdateGradientCache();
		}

		public void UpdateGradientCache()
		{
			if (this.m_RampTexture == null)
			{
				this.m_RampTexture = new Texture2D(256, 1, TextureFormat.RGB24, false);
				this.m_RampTexture.filterMode = FilterMode.Bilinear;
				this.m_RampTexture.wrapMode = TextureWrapMode.Clamp;
				this.m_RampTexture.hideFlags = HideFlags.HideAndDontSave;
			}
			Color[] array = new Color[256];
			for (int i = 0; i < 256; i++)
			{
				array[i] = this.Ramp.Evaluate((float)i / 255f);
			}
			this.m_RampTexture.SetPixels(array);
			this.m_RampTexture.Apply();
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Ramp == null || this.Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			base.Material.SetTexture("_RampTex", this.m_RampTexture);
			base.Material.SetFloat("_Amount", this.Amount);
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Gradient Ramp";
		}

		[Tooltip("Gradient used to remap the pixels luminosity.")]
		public Gradient Ramp;

		[Range(0f, 1f)]
		[Tooltip("Blending factor.")]
		public float Amount = 1f;

		protected Texture2D m_RampTexture;
	}
}
