using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/other-effects/pixel-matrix.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Other Effects/Pixel Matrix")]
	public class PixelMatrix : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_Params", new Vector4((float)this.Size, Mathf.Floor((float)this.Size / 3f), (float)this.Size - Mathf.Floor((float)this.Size / 3f), this.Brightness));
			Graphics.Blit(source, destination, base.Material, (!this.BlackBorder) ? 0 : 1);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/PixelMatrix";
		}

		[Min(3f)]
		[Tooltip("Tile size. Works best with multiples of 3.")]
		public int Size = 9;

		[Range(0f, 10f)]
		[Tooltip("Tile brightness booster.")]
		public float Brightness = 1.4f;

		[Tooltip("Show / hide black borders on every tile.")]
		public bool BlackBorder = true;
	}
}
