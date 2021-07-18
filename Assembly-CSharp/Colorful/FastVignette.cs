using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/fast-vignette.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Fast Vignette")]
	public class FastVignette : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_Params", new Vector4(this.Center.x, this.Center.y, this.Sharpness * 0.01f, this.Darkness * 0.02f));
			base.Material.SetColor("_Color", this.Color);
			Graphics.Blit(source, destination, base.Material, (int)this.Mode);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Fast Vignette";
		}

		[Tooltip("Vignette type.")]
		public FastVignette.ColorMode Mode;

		[ColorUsage(false)]
		[Tooltip("The color to use in the vignette area.")]
		public Color Color = Color.red;

		[Tooltip("Center point.")]
		public Vector2 Center = new Vector2(0.5f, 0.5f);

		[Range(-100f, 100f)]
		[Tooltip("Smoothness of the vignette effect.")]
		public float Sharpness = 10f;

		[Range(0f, 100f)]
		[Tooltip("Amount of vignetting on screen.")]
		public float Darkness = 30f;

		public enum ColorMode
		{
			Classic,
			Desaturate,
			Colored
		}
	}
}
