using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/other-effects/led.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Other Effects/LED")]
	public class Led : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float x = this.Scale;
			if (this.Mode == Led.SizeMode.PixelPerfect)
			{
				x = (float)source.width / this.Scale;
			}
			base.Material.SetVector("_Params", new Vector4(x, (!this.AutomaticRatio) ? this.Ratio : ((float)source.width / (float)source.height), this.Brightness, this.Shape));
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Led";
		}

		[Range(1f, 255f)]
		[Tooltip("Scale of an individual LED. Depends on the Mode used.")]
		public float Scale = 80f;

		[Range(0f, 10f)]
		[Tooltip("LED brightness booster.")]
		public float Brightness = 1f;

		[Range(1f, 3f)]
		[Tooltip("LED shape, from softer to harsher.")]
		public float Shape = 1.5f;

		[Tooltip("Turn this on to automatically compute the aspect ratio needed for squared LED.")]
		public bool AutomaticRatio = true;

		[Tooltip("Custom aspect ratio.")]
		public float Ratio = 1f;

		[Tooltip("Used for the Scale field.")]
		public Led.SizeMode Mode;

		public enum SizeMode
		{
			ResolutionIndependent,
			PixelPerfect
		}
	}
}
