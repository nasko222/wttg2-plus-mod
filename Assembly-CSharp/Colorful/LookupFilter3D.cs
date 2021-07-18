using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/lookup-filter-3d.html")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Colorful FX/Color Correction/Lookup Filter 3D")]
	public class LookupFilter3D : MonoBehaviour
	{
		public Shader Shader2DSafe
		{
			get
			{
				if (this.Shader2D == null)
				{
					this.Shader2D = Shader.Find("Hidden/Colorful/Lookup Filter 2D");
				}
				return this.Shader2D;
			}
		}

		public Shader Shader3DSafe
		{
			get
			{
				if (this.Shader3D == null)
				{
					this.Shader3D = Shader.Find("Hidden/Colorful/Lookup Filter 3D");
				}
				return this.Shader3D;
			}
		}

		public Material Material
		{
			get
			{
				if (this.m_Use2DLut || this.ForceCompatibility)
				{
					if (this.m_Material2D == null)
					{
						this.m_Material2D = new Material(this.Shader2DSafe);
						this.m_Material2D.hideFlags = HideFlags.HideAndDontSave;
					}
					return this.m_Material2D;
				}
				if (this.m_Material3D == null)
				{
					this.m_Material3D = new Material(this.Shader3DSafe);
					this.m_Material3D.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_Material3D;
			}
		}

		protected virtual void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				Debug.LogWarning("Image effects aren't supported on this device");
				base.enabled = false;
				return;
			}
			if (!SystemInfo.supports3DTextures)
			{
				this.m_Use2DLut = true;
			}
			if (!this.Shader2DSafe || !this.Shader2D.isSupported)
			{
				Debug.LogWarning("The shader is null or unsupported on this device");
				base.enabled = false;
				return;
			}
			if (!this.m_Use2DLut && !this.ForceCompatibility && (!this.Shader3DSafe || !this.Shader3D.isSupported))
			{
				this.m_Use2DLut = true;
			}
		}

		protected virtual void OnDisable()
		{
			if (this.m_Material2D)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Material2D);
			}
			if (this.m_Material3D)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Material3D);
			}
			if (this.m_Lut3D)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Lut3D);
			}
			this.m_BaseTextureName = string.Empty;
		}

		protected virtual void Reset()
		{
			this.m_BaseTextureName = string.Empty;
		}

		protected void SetIdentityLut()
		{
			int num = 16;
			Color[] array = new Color[num * num * num];
			float num2 = 1f / (1f * (float)num - 1f);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < num; k++)
					{
						array[i + j * num + k * num * num] = new Color((float)i * 1f * num2, (float)j * 1f * num2, (float)k * 1f * num2, 1f);
					}
				}
			}
			if (this.m_Lut3D)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Lut3D);
			}
			this.m_Lut3D = new Texture3D(num, num, num, TextureFormat.ARGB32, false);
			this.m_Lut3D.hideFlags = HideFlags.HideAndDontSave;
			this.m_Lut3D.SetPixels(array);
			this.m_Lut3D.Apply();
			this.m_BaseTextureName = string.Empty;
		}

		public bool ValidDimensions(Texture2D tex2D)
		{
			return !(tex2D == null) && tex2D.height == Mathf.FloorToInt(Mathf.Sqrt((float)tex2D.width));
		}

		protected void ConvertBaseTexture()
		{
			if (!this.ValidDimensions(this.LookupTexture))
			{
				Debug.LogWarning("The given 2D texture " + this.LookupTexture.name + " cannot be used as a 3D LUT. Pick another texture or adjust dimension to e.g. 256x16.");
				return;
			}
			this.m_BaseTextureName = this.LookupTexture.name;
			int height = this.LookupTexture.height;
			Color[] pixels = this.LookupTexture.GetPixels();
			Color[] array = new Color[pixels.Length];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < height; j++)
				{
					for (int k = 0; k < height; k++)
					{
						int num = height - j - 1;
						array[i + j * height + k * height * height] = pixels[k * height + i + num * height * height];
					}
				}
			}
			if (this.m_Lut3D)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Lut3D);
			}
			this.m_Lut3D = new Texture3D(height, height, height, TextureFormat.ARGB32, false);
			this.m_Lut3D.hideFlags = HideFlags.HideAndDontSave;
			this.m_Lut3D.wrapMode = TextureWrapMode.Clamp;
			this.m_Lut3D.SetPixels(array);
			this.m_Lut3D.Apply();
		}

		public void Apply(Texture source, RenderTexture destination)
		{
			if (source is RenderTexture)
			{
				this.OnRenderImage(source as RenderTexture, destination);
				return;
			}
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
			Graphics.Blit(source, temporary);
			this.OnRenderImage(temporary, destination);
			RenderTexture.ReleaseTemporary(temporary);
		}

		protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.LookupTexture == null || this.Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.m_Use2DLut || this.ForceCompatibility)
			{
				this.RenderLut2D(source, destination);
			}
			else
			{
				this.RenderLut3D(source, destination);
			}
		}

		protected virtual void RenderLut2D(RenderTexture source, RenderTexture destination)
		{
			float num = Mathf.Sqrt((float)this.LookupTexture.width);
			this.Material.SetTexture("_LookupTex", this.LookupTexture);
			this.Material.SetVector("_Params1", new Vector3(1f / (float)this.LookupTexture.width, 1f / (float)this.LookupTexture.height, num - 1f));
			this.Material.SetVector("_Params2", new Vector2(this.Amount, 0f));
			Graphics.Blit(source, destination, this.Material, (!CLib.IsLinearColorSpace()) ? 0 : 1);
		}

		protected virtual void RenderLut3D(RenderTexture source, RenderTexture destination)
		{
			if (this.LookupTexture.name != this.m_BaseTextureName)
			{
				this.ConvertBaseTexture();
			}
			if (this.m_Lut3D == null)
			{
				this.SetIdentityLut();
			}
			this.Material.SetTexture("_LookupTex", this.m_Lut3D);
			float num = (float)this.m_Lut3D.width;
			this.Material.SetVector("_Params", new Vector3((num - 1f) / (1f * num), 1f / (2f * num), this.Amount));
			Graphics.Blit(source, destination, this.Material, (!CLib.IsLinearColorSpace()) ? 0 : 1);
		}

		[Tooltip("The lookup texture to apply. Read the documentation to learn how to create one.")]
		public Texture2D LookupTexture;

		[Range(0f, 1f)]
		[Tooltip("Blending factor.")]
		public float Amount = 1f;

		[Tooltip("The effect will automatically detect the correct shader to use for the device but you can force it to only use the compatibility shader.")]
		public bool ForceCompatibility;

		protected Texture3D m_Lut3D;

		protected string m_BaseTextureName;

		protected bool m_Use2DLut;

		public Shader Shader2D;

		public Shader Shader3D;

		protected Material m_Material2D;

		protected Material m_Material3D;
	}
}
