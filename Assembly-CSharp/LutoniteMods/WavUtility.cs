using System;
using System.IO;
using UnityEngine;

namespace LutoniteMods
{
	internal static class WavUtility
	{
		public static AudioClip ToAudioClip(string filePath)
		{
			WavUtility.WAV wav = new WavUtility.WAV(File.ReadAllBytes(Application.dataPath + "/Resources/custom_audio/" + filePath));
			AudioClip audioClip = AudioClip.Create(filePath, wav.SampleCount, 1, wav.Frequency, false);
			audioClip.SetData(wav.LeftChannel, 0);
			return audioClip;
		}

		public class WAV
		{
			private static float bytesToFloat(byte firstByte, byte secondByte)
			{
				return (float)((short)((int)secondByte << 8 | (int)firstByte)) / 32768f;
			}

			private static int bytesToInt(byte[] bytes, int offset = 0)
			{
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					num |= (int)bytes[offset + i] << i * 8;
				}
				return num;
			}

			private static byte[] GetBytes(string filename)
			{
				return File.ReadAllBytes(filename);
			}

			public float[] LeftChannel { get; internal set; }

			public float[] RightChannel { get; internal set; }

			public int ChannelCount { get; internal set; }

			public int SampleCount { get; internal set; }

			public int Frequency { get; internal set; }

			public WAV(string filename) : this(WavUtility.WAV.GetBytes(filename))
			{
			}

			public WAV(byte[] wav)
			{
				this.ChannelCount = (int)wav[22];
				this.Frequency = WavUtility.WAV.bytesToInt(wav, 24);
				int num = 12;
				while (wav[num] != 100 || wav[num + 1] != 97 || wav[num + 2] != 116 || wav[num + 3] != 97)
				{
					num += 4;
					int num2 = (int)wav[num] + (int)wav[num + 1] * 256 + (int)wav[num + 2] * 65536 + (int)wav[num + 3] * 16777216;
					num += 4 + num2;
				}
				num += 8;
				this.SampleCount = (wav.Length - num) / 2;
				if (this.ChannelCount == 2)
				{
					this.SampleCount /= 2;
				}
				this.LeftChannel = new float[this.SampleCount];
				if (this.ChannelCount == 2)
				{
					this.RightChannel = new float[this.SampleCount];
				}
				else
				{
					this.RightChannel = null;
				}
				int num3 = 0;
				int num4 = wav.Length - ((this.RightChannel == null) ? 1 : 3);
				while (num3 < this.SampleCount && num < num4)
				{
					this.LeftChannel[num3] = WavUtility.WAV.bytesToFloat(wav[num], wav[num + 1]);
					num += 2;
					if (this.ChannelCount == 2)
					{
						this.RightChannel[num3] = WavUtility.WAV.bytesToFloat(wav[num], wav[num + 1]);
						num += 2;
					}
					num3++;
				}
			}

			public override string ToString()
			{
				return string.Format("[WAV: LeftChannel={0}, RightChannel={1}, ChannelCount={2}, SampleCount={3}, Frequency={4}]", new object[]
				{
					this.LeftChannel,
					this.RightChannel,
					this.ChannelCount,
					this.SampleCount,
					this.Frequency
				});
			}
		}
	}
}
