using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/channel-mixer.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Channel Mixer")]
	public class ChannelMixer : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_Red", new Vector4(this.Red.x * 0.01f, this.Green.x * 0.01f, this.Blue.x * 0.01f));
			base.Material.SetVector("_Green", new Vector4(this.Red.y * 0.01f, this.Green.y * 0.01f, this.Blue.y * 0.01f));
			base.Material.SetVector("_Blue", new Vector4(this.Red.z * 0.01f, this.Green.z * 0.01f, this.Blue.z * 0.01f));
			base.Material.SetVector("_Constant", new Vector4(this.Constant.x * 0.01f, this.Constant.y * 0.01f, this.Constant.z * 0.01f));
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Channel Mixer";
		}

		public Vector3 Red = new Vector3(100f, 0f, 0f);

		public Vector3 Green = new Vector3(0f, 100f, 0f);

		public Vector3 Blue = new Vector3(0f, 0f, 100f);

		public Vector3 Constant = new Vector3(0f, 0f, 0f);
	}
}
