using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/channel-swapper.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Channel Swapper")]
	public class ChannelSwapper : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_Red", ChannelSwapper.m_Channels[(int)this.RedSource]);
			base.Material.SetVector("_Green", ChannelSwapper.m_Channels[(int)this.GreenSource]);
			base.Material.SetVector("_Blue", ChannelSwapper.m_Channels[(int)this.BlueSource]);
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Channel Swapper";
		}

		[Tooltip("Source channel to use for the output red channel.")]
		public ChannelSwapper.Channel RedSource;

		[Tooltip("Source channel to use for the output green channel.")]
		public ChannelSwapper.Channel GreenSource = ChannelSwapper.Channel.Green;

		[Tooltip("Source channel to use for the output blue channel.")]
		public ChannelSwapper.Channel BlueSource = ChannelSwapper.Channel.Blue;

		private static Vector4[] m_Channels = new Vector4[]
		{
			new Vector4(1f, 0f, 0f, 0f),
			new Vector4(0f, 1f, 0f, 0f),
			new Vector4(0f, 0f, 1f, 0f)
		};

		public enum Channel
		{
			Red,
			Green,
			Blue
		}
	}
}
