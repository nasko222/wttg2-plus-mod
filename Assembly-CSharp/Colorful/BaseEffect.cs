using System;
using UnityEngine;

namespace Colorful
{
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class BaseEffect : MonoBehaviour
	{
		public Shader ShaderSafe
		{
			get
			{
				if (this.Shader == null)
				{
					this.Shader = Shader.Find(this.GetShaderName());
				}
				return this.Shader;
			}
		}

		public Material Material
		{
			get
			{
				if (this.m_Material == null)
				{
					this.m_Material = new Material(this.ShaderSafe);
					this.m_Material.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_Material;
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
			if (!this.ShaderSafe || !this.Shader.isSupported)
			{
				Debug.LogWarning("The shader is null or unsupported on this device");
				base.enabled = false;
			}
		}

		protected virtual void OnDisable()
		{
			if (this.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Material);
			}
			this.m_Material = null;
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
		}

		protected virtual string GetShaderName()
		{
			return "null";
		}

		public Shader Shader;

		protected Material m_Material;
	}
}
