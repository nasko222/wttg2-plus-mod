using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/wiggle.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Wiggle")]
	public class Wiggle : BaseEffect
	{
		protected virtual void Update()
		{
			if (this.AutomaticTimer)
			{
				if (this.Timer > 1000f)
				{
					this.Timer -= 1000f;
				}
				this.Timer += this.Speed * Time.deltaTime;
			}
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_Params", new Vector3(this.Frequency, this.Amplitude, this.Timer * ((this.Mode != Wiggle.Algorithm.Complex) ? 1f : 0.1f)));
			Graphics.Blit(source, destination, base.Material, (int)this.Mode);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Wiggle";
		}

		[Tooltip("Animation type. Complex is slower but looks more natural.")]
		public Wiggle.Algorithm Mode = Wiggle.Algorithm.Complex;

		public float Timer;

		[Tooltip("Wave animation speed.")]
		public float Speed = 1f;

		[Tooltip("Wave frequency (higher means more waves).")]
		public float Frequency = 12f;

		[Tooltip("Wave amplitude (higher means bigger waves).")]
		public float Amplitude = 0.01f;

		[Tooltip("Automatically animate this effect at runtime.")]
		public bool AutomaticTimer = true;

		public enum Algorithm
		{
			Simple,
			Complex
		}
	}
}
