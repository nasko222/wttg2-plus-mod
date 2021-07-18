using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/channel-clamper.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Channel Clamper")]
	public class ChannelClamper : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_RedClamp", this.Red);
			base.Material.SetVector("_GreenClamp", this.Green);
			base.Material.SetVector("_BlueClamp", this.Blue);
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Channel Clamper";
		}

		public Vector2 Red = new Vector2(0f, 1f);

		public Vector2 Green = new Vector2(0f, 1f);

		public Vector2 Blue = new Vector2(0f, 1f);
	}
}
