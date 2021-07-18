using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/artistic-effects/halftone.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Artistic Effects/Halftone")]
	public class Halftone : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_Center", new Vector2(this.Center.x * (float)source.width, this.Center.y * (float)source.height));
			base.Material.SetVector("_Params", new Vector3(this.Scale, this.DotSize, this.Smoothness));
			Matrix4x4 value = default(Matrix4x4);
			value.SetRow(0, this.CMYKRot(this.Angle + 0.2617994f));
			value.SetRow(1, this.CMYKRot(this.Angle + 1.30899692f));
			value.SetRow(2, this.CMYKRot(this.Angle));
			value.SetRow(3, this.CMYKRot(this.Angle + 0.7853982f));
			base.Material.SetMatrix("_MatRot", value);
			Graphics.Blit(source, destination, base.Material, (!this.Desaturate) ? 0 : 1);
		}

		private Vector4 CMYKRot(float angle)
		{
			float num = Mathf.Cos(angle);
			float num2 = Mathf.Sin(angle);
			return new Vector4(num, -num2, num2, num);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Halftone";
		}

		[Min(0f)]
		[Tooltip("Global haltfoning scale.")]
		public float Scale = 12f;

		[Min(0f)]
		[Tooltip("Individual dot size.")]
		public float DotSize = 1.35f;

		[Tooltip("Rotates the dot placement according to the Center point.")]
		public float Angle = 1.2f;

		[Range(0f, 1f)]
		[Tooltip("Dots antialiasing")]
		public float Smoothness = 0.08f;

		[Tooltip("Center point to use for the rotation.")]
		public Vector2 Center = new Vector2(0.5f, 0.5f);

		[Tooltip("Turns the effect black & white.")]
		public bool Desaturate;
	}
}
