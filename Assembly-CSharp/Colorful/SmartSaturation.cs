using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/smart-saturation.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Smart Saturation")]
	public class SmartSaturation : BaseEffect
	{
		protected Texture2D m_CurveTexture
		{
			get
			{
				if (this._CurveTexture == null)
				{
					this.UpdateCurve();
				}
				return this._CurveTexture;
			}
		}

		protected virtual void Reset()
		{
			this.Curve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.5f, 0f, 0f),
				new Keyframe(1f, 0.5f, 0f, 0f)
			});
		}

		protected virtual void OnEnable()
		{
			if (this.Curve == null)
			{
				this.Reset();
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (this._CurveTexture != null)
			{
				UnityEngine.Object.DestroyImmediate(this._CurveTexture);
			}
		}

		public virtual void UpdateCurve()
		{
			if (this._CurveTexture == null)
			{
				this._CurveTexture = new Texture2D(256, 1, TextureFormat.Alpha8, false);
				this._CurveTexture.name = "Saturation Curve Texture";
				this._CurveTexture.wrapMode = TextureWrapMode.Clamp;
				this._CurveTexture.anisoLevel = 0;
				this._CurveTexture.filterMode = FilterMode.Bilinear;
				this._CurveTexture.hideFlags = HideFlags.DontSave;
			}
			Color[] pixels = this._CurveTexture.GetPixels();
			for (int i = 0; i < 256; i++)
			{
				float num = Mathf.Clamp01(this.Curve.Evaluate((float)i / 255f));
				pixels[i] = new Color(num, num, num, num);
			}
			this._CurveTexture.SetPixels(pixels);
			this._CurveTexture.Apply();
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetTexture("_Curve", this.m_CurveTexture);
			base.Material.SetFloat("_Boost", this.Boost);
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Smart Saturation";
		}

		[Range(0f, 2f)]
		[Tooltip("Saturation boost. Default: 1 (no boost).")]
		public float Boost = 1f;

		public AnimationCurve Curve;

		private Texture2D _CurveTexture;
	}
}
