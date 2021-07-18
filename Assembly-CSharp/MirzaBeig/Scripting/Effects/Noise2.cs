using System;

namespace MirzaBeig.Scripting.Effects
{
	[Serializable]
	public static class Noise2
	{
		private static float smooth(float t)
		{
			return t * t * (3f - 2f * t);
		}

		private static float fade(float t)
		{
			return t * t * t * (t * (t * 6f - 15f) + 10f);
		}

		private static int floor(float x)
		{
			return (x <= 0f) ? ((int)x - 1) : ((int)x);
		}

		private static float lerp(float from, float to, float t)
		{
			return from + t * (to - from);
		}

		private static float grad(int hash, float x, float y, float z)
		{
			switch (hash & 15)
			{
			case 0:
				return x + y;
			case 1:
				return -x + y;
			case 2:
				return x - y;
			case 3:
				return -x - y;
			case 4:
				return x + x;
			case 5:
				return -x + x;
			case 6:
				return x - x;
			case 7:
				return -x - x;
			case 8:
				return y + x;
			case 9:
				return -y + x;
			case 10:
				return y - x;
			case 11:
				return -y - x;
			case 12:
				return y + z;
			case 13:
				return -y + x;
			case 14:
				return y - x;
			case 15:
				return -y - z;
			default:
				return 0f;
			}
		}

		public static float perlin(float x, float y, float z)
		{
			int num = (x <= 0f) ? ((int)x - 1) : ((int)x);
			int num2 = (y <= 0f) ? ((int)y - 1) : ((int)y);
			int num3 = (z <= 0f) ? ((int)z - 1) : ((int)z);
			float num4 = x - (float)num;
			float num5 = y - (float)num2;
			float num6 = z - (float)num3;
			float num7 = num4 - 1f;
			float num8 = num5 - 1f;
			float num9 = num6 - 1f;
			int num10 = num + 1 & 255;
			int num11 = num2 + 1 & 255;
			int num12 = num3 + 1 & 255;
			num &= 255;
			num2 &= 255;
			num3 &= 255;
			float num13 = num6 * num6 * num6 * (num6 * (num6 * 6f - 15f) + 10f);
			float num14 = num5 * num5 * num5 * (num5 * (num5 * 6f - 15f) + 10f);
			float num15 = num4 * num4 * num4 * (num4 * (num4 * 6f - 15f) + 10f);
			int num16 = (int)Noise2.perm[num + (int)Noise2.perm[num2 + (int)Noise2.perm[num3]]];
			float num17;
			switch (num16 & 15)
			{
			case 0:
				num17 = num4 + num5;
				break;
			case 1:
				num17 = -num4 + num5;
				break;
			case 2:
				num17 = num4 - num5;
				break;
			case 3:
				num17 = -num4 - num5;
				break;
			case 4:
				num17 = num4 + num4;
				break;
			case 5:
				num17 = -num4 + num4;
				break;
			case 6:
				num17 = num4 - num4;
				break;
			case 7:
				num17 = -num4 - num4;
				break;
			case 8:
				num17 = num5 + num4;
				break;
			case 9:
				num17 = -num5 + num4;
				break;
			case 10:
				num17 = num5 - num4;
				break;
			case 11:
				num17 = -num5 - num4;
				break;
			case 12:
				num17 = num5 + num6;
				break;
			case 13:
				num17 = -num5 + num4;
				break;
			case 14:
				num17 = num5 - num4;
				break;
			case 15:
				num17 = -num5 - num6;
				break;
			default:
				num17 = 0f;
				break;
			}
			float num18 = num17;
			num16 = (int)Noise2.perm[num + (int)Noise2.perm[num2 + (int)Noise2.perm[num12]]];
			switch (num16 & 15)
			{
			case 0:
				num17 = num4 + num5;
				break;
			case 1:
				num17 = -num4 + num5;
				break;
			case 2:
				num17 = num4 - num5;
				break;
			case 3:
				num17 = -num4 - num5;
				break;
			case 4:
				num17 = num4 + num4;
				break;
			case 5:
				num17 = -num4 + num4;
				break;
			case 6:
				num17 = num4 - num4;
				break;
			case 7:
				num17 = -num4 - num4;
				break;
			case 8:
				num17 = num5 + num4;
				break;
			case 9:
				num17 = -num5 + num4;
				break;
			case 10:
				num17 = num5 - num4;
				break;
			case 11:
				num17 = -num5 - num4;
				break;
			case 12:
				num17 = num5 + num9;
				break;
			case 13:
				num17 = -num5 + num4;
				break;
			case 14:
				num17 = num5 - num4;
				break;
			case 15:
				num17 = -num5 - num9;
				break;
			default:
				num17 = 0f;
				break;
			}
			float num19 = num17;
			float num20 = num18 + num13 * (num19 - num18);
			num16 = (int)Noise2.perm[num + (int)Noise2.perm[num11 + (int)Noise2.perm[num3]]];
			switch (num16 & 15)
			{
			case 0:
				num17 = num4 + num8;
				break;
			case 1:
				num17 = -num4 + num8;
				break;
			case 2:
				num17 = num4 - num8;
				break;
			case 3:
				num17 = -num4 - num8;
				break;
			case 4:
				num17 = num4 + num4;
				break;
			case 5:
				num17 = -num4 + num4;
				break;
			case 6:
				num17 = num4 - num4;
				break;
			case 7:
				num17 = -num4 - num4;
				break;
			case 8:
				num17 = num8 + num4;
				break;
			case 9:
				num17 = -num8 + num4;
				break;
			case 10:
				num17 = num8 - num4;
				break;
			case 11:
				num17 = -num8 - num4;
				break;
			case 12:
				num17 = num8 + num6;
				break;
			case 13:
				num17 = -num8 + num4;
				break;
			case 14:
				num17 = num8 - num4;
				break;
			case 15:
				num17 = -num8 - num6;
				break;
			default:
				num17 = 0f;
				break;
			}
			num18 = num17;
			num16 = (int)Noise2.perm[num + (int)Noise2.perm[num11 + (int)Noise2.perm[num12]]];
			switch (num16 & 15)
			{
			case 0:
				num17 = num4 + num8;
				break;
			case 1:
				num17 = -num4 + num8;
				break;
			case 2:
				num17 = num4 - num8;
				break;
			case 3:
				num17 = -num4 - num8;
				break;
			case 4:
				num17 = num4 + num4;
				break;
			case 5:
				num17 = -num4 + num4;
				break;
			case 6:
				num17 = num4 - num4;
				break;
			case 7:
				num17 = -num4 - num4;
				break;
			case 8:
				num17 = num8 + num4;
				break;
			case 9:
				num17 = -num8 + num4;
				break;
			case 10:
				num17 = num8 - num4;
				break;
			case 11:
				num17 = -num8 - num4;
				break;
			case 12:
				num17 = num8 + num9;
				break;
			case 13:
				num17 = -num8 + num4;
				break;
			case 14:
				num17 = num8 - num4;
				break;
			case 15:
				num17 = -num8 - num9;
				break;
			default:
				num17 = 0f;
				break;
			}
			num19 = num17;
			float num21 = num18 + num13 * (num19 - num18);
			float num22 = num20 + num14 * (num21 - num20);
			num16 = (int)Noise2.perm[num10 + (int)Noise2.perm[num2 + (int)Noise2.perm[num3]]];
			switch (num16 & 15)
			{
			case 0:
				num17 = num7 + num5;
				break;
			case 1:
				num17 = -num7 + num5;
				break;
			case 2:
				num17 = num7 - num5;
				break;
			case 3:
				num17 = -num7 - num5;
				break;
			case 4:
				num17 = num7 + num7;
				break;
			case 5:
				num17 = -num7 + num7;
				break;
			case 6:
				num17 = num7 - num7;
				break;
			case 7:
				num17 = -num7 - num7;
				break;
			case 8:
				num17 = num5 + num7;
				break;
			case 9:
				num17 = -num5 + num7;
				break;
			case 10:
				num17 = num5 - num7;
				break;
			case 11:
				num17 = -num5 - num7;
				break;
			case 12:
				num17 = num5 + num6;
				break;
			case 13:
				num17 = -num5 + num7;
				break;
			case 14:
				num17 = num5 - num7;
				break;
			case 15:
				num17 = -num5 - num6;
				break;
			default:
				num17 = 0f;
				break;
			}
			num18 = num17;
			num16 = (int)Noise2.perm[num10 + (int)Noise2.perm[num2 + (int)Noise2.perm[num12]]];
			switch (num16 & 15)
			{
			case 0:
				num17 = num7 + num5;
				break;
			case 1:
				num17 = -num7 + num5;
				break;
			case 2:
				num17 = num7 - num5;
				break;
			case 3:
				num17 = -num7 - num5;
				break;
			case 4:
				num17 = num7 + num7;
				break;
			case 5:
				num17 = -num7 + num7;
				break;
			case 6:
				num17 = num7 - num7;
				break;
			case 7:
				num17 = -num7 - num7;
				break;
			case 8:
				num17 = num5 + num7;
				break;
			case 9:
				num17 = -num5 + num7;
				break;
			case 10:
				num17 = num5 - num7;
				break;
			case 11:
				num17 = -num5 - num7;
				break;
			case 12:
				num17 = num5 + num9;
				break;
			case 13:
				num17 = -num5 + num7;
				break;
			case 14:
				num17 = num5 - num7;
				break;
			case 15:
				num17 = -num5 - num9;
				break;
			default:
				num17 = 0f;
				break;
			}
			num19 = num17;
			num20 = num18 + num13 * (num19 - num18);
			num16 = (int)Noise2.perm[num10 + (int)Noise2.perm[num11 + (int)Noise2.perm[num3]]];
			switch (num16 & 15)
			{
			case 0:
				num17 = num7 + num8;
				break;
			case 1:
				num17 = -num7 + num8;
				break;
			case 2:
				num17 = num7 - num8;
				break;
			case 3:
				num17 = -num7 - num8;
				break;
			case 4:
				num17 = num7 + num7;
				break;
			case 5:
				num17 = -num7 + num7;
				break;
			case 6:
				num17 = num7 - num7;
				break;
			case 7:
				num17 = -num7 - num7;
				break;
			case 8:
				num17 = num8 + num7;
				break;
			case 9:
				num17 = -num8 + num7;
				break;
			case 10:
				num17 = num8 - num7;
				break;
			case 11:
				num17 = -num8 - num7;
				break;
			case 12:
				num17 = num8 + num6;
				break;
			case 13:
				num17 = -num8 + num7;
				break;
			case 14:
				num17 = num8 - num7;
				break;
			case 15:
				num17 = -num8 - num6;
				break;
			default:
				num17 = 0f;
				break;
			}
			num18 = num17;
			num16 = (int)Noise2.perm[num10 + (int)Noise2.perm[num11 + (int)Noise2.perm[num12]]];
			switch (num16 & 15)
			{
			case 0:
				num17 = num7 + num8;
				break;
			case 1:
				num17 = -num7 + num8;
				break;
			case 2:
				num17 = num7 - num8;
				break;
			case 3:
				num17 = -num7 - num8;
				break;
			case 4:
				num17 = num7 + num7;
				break;
			case 5:
				num17 = -num7 + num7;
				break;
			case 6:
				num17 = num7 - num7;
				break;
			case 7:
				num17 = -num7 - num7;
				break;
			case 8:
				num17 = num8 + num7;
				break;
			case 9:
				num17 = -num8 + num7;
				break;
			case 10:
				num17 = num8 - num7;
				break;
			case 11:
				num17 = -num8 - num7;
				break;
			case 12:
				num17 = num8 + num9;
				break;
			case 13:
				num17 = -num8 + num7;
				break;
			case 14:
				num17 = num8 - num7;
				break;
			case 15:
				num17 = -num8 - num9;
				break;
			default:
				num17 = 0f;
				break;
			}
			num19 = num17;
			num21 = num18 + num13 * (num19 - num18);
			float num23 = num20 + num14 * (num21 - num20);
			return 0.936f * (num22 + num15 * (num23 - num22));
		}

		public static float simplex(float x, float y, float z)
		{
			float num = (x + y + z) * Noise2.F3;
			float num2 = x + num;
			float num3 = y + num;
			float num4 = z + num;
			int num5 = (num2 <= 0f) ? ((int)num2 - 1) : ((int)num2);
			int num6 = (num3 <= 0f) ? ((int)num3 - 1) : ((int)num3);
			int num7 = (num4 <= 0f) ? ((int)num4 - 1) : ((int)num4);
			float num8 = (float)(num5 + num6 + num7) * Noise2.G3;
			float num9 = (float)num5 - num8;
			float num10 = (float)num6 - num8;
			float num11 = (float)num7 - num8;
			float num12 = x - num9;
			float num13 = y - num10;
			float num14 = z - num11;
			int num15;
			int num16;
			int num17;
			int num18;
			int num19;
			int num20;
			if (num12 >= num13)
			{
				if (num13 >= num14)
				{
					num15 = 1;
					num16 = 0;
					num17 = 0;
					num18 = 1;
					num19 = 1;
					num20 = 0;
				}
				else if (num12 >= num14)
				{
					num15 = 1;
					num16 = 0;
					num17 = 0;
					num18 = 1;
					num19 = 0;
					num20 = 1;
				}
				else
				{
					num15 = 0;
					num16 = 0;
					num17 = 1;
					num18 = 1;
					num19 = 0;
					num20 = 1;
				}
			}
			else if (num13 < num14)
			{
				num15 = 0;
				num16 = 0;
				num17 = 1;
				num18 = 0;
				num19 = 1;
				num20 = 1;
			}
			else if (num12 < num14)
			{
				num15 = 0;
				num16 = 1;
				num17 = 0;
				num18 = 0;
				num19 = 1;
				num20 = 1;
			}
			else
			{
				num15 = 0;
				num16 = 1;
				num17 = 0;
				num18 = 1;
				num19 = 1;
				num20 = 0;
			}
			float num21 = num12 - (float)num15 + Noise2.G3;
			float num22 = num13 - (float)num16 + Noise2.G3;
			float num23 = num14 - (float)num17 + Noise2.G3;
			float num24 = num12 - (float)num18 + 2f * Noise2.G3;
			float num25 = num13 - (float)num19 + 2f * Noise2.G3;
			float num26 = num14 - (float)num20 + 2f * Noise2.G3;
			float num27 = num12 - 1f + 3f * Noise2.G3;
			float num28 = num13 - 1f + 3f * Noise2.G3;
			float num29 = num14 - 1f + 3f * Noise2.G3;
			int num30 = num5 & 255;
			int num31 = num6 & 255;
			int num32 = num7 & 255;
			float num33 = 0.6f - num12 * num12 - num13 * num13 - num14 * num14;
			float num34;
			if (num33 < 0f)
			{
				num34 = 0f;
			}
			else
			{
				num33 *= num33;
				int num35 = (int)Noise2.perm[num30 + (int)Noise2.perm[num31 + (int)Noise2.perm[num32]]];
				float num36;
				switch (num35 & 15)
				{
				case 0:
					num36 = num12 + num13;
					break;
				case 1:
					num36 = -num12 + num13;
					break;
				case 2:
					num36 = num12 - num13;
					break;
				case 3:
					num36 = -num12 - num13;
					break;
				case 4:
					num36 = num12 + num12;
					break;
				case 5:
					num36 = -num12 + num12;
					break;
				case 6:
					num36 = num12 - num12;
					break;
				case 7:
					num36 = -num12 - num12;
					break;
				case 8:
					num36 = num13 + num12;
					break;
				case 9:
					num36 = -num13 + num12;
					break;
				case 10:
					num36 = num13 - num12;
					break;
				case 11:
					num36 = -num13 - num12;
					break;
				case 12:
					num36 = num13 + num14;
					break;
				case 13:
					num36 = -num13 + num12;
					break;
				case 14:
					num36 = num13 - num12;
					break;
				case 15:
					num36 = -num13 - num14;
					break;
				default:
					num36 = 0f;
					break;
				}
				num34 = num33 * num33 * num36;
			}
			float num37 = 0.6f - num21 * num21 - num22 * num22 - num23 * num23;
			float num38;
			if (num37 < 0f)
			{
				num38 = 0f;
			}
			else
			{
				num37 *= num37;
				int num35 = (int)Noise2.perm[num30 + num15 + (int)Noise2.perm[num31 + num16 + (int)Noise2.perm[num32 + num17]]];
				float num36;
				switch (num35 & 15)
				{
				case 0:
					num36 = num21 + num22;
					break;
				case 1:
					num36 = -num21 + num22;
					break;
				case 2:
					num36 = num21 - num22;
					break;
				case 3:
					num36 = -num21 - num22;
					break;
				case 4:
					num36 = num21 + num21;
					break;
				case 5:
					num36 = -num21 + num21;
					break;
				case 6:
					num36 = num21 - num21;
					break;
				case 7:
					num36 = -num21 - num21;
					break;
				case 8:
					num36 = num22 + num21;
					break;
				case 9:
					num36 = -num22 + num21;
					break;
				case 10:
					num36 = num22 - num21;
					break;
				case 11:
					num36 = -num22 - num21;
					break;
				case 12:
					num36 = num22 + num23;
					break;
				case 13:
					num36 = -num22 + num21;
					break;
				case 14:
					num36 = num22 - num21;
					break;
				case 15:
					num36 = -num22 - num23;
					break;
				default:
					num36 = 0f;
					break;
				}
				num38 = num37 * num37 * num36;
			}
			float num39 = 0.6f - num24 * num24 - num25 * num25 - num26 * num26;
			float num40;
			if (num39 < 0f)
			{
				num40 = 0f;
			}
			else
			{
				num39 *= num39;
				int num35 = (int)Noise2.perm[num30 + num18 + (int)Noise2.perm[num31 + num19 + (int)Noise2.perm[num32 + num20]]];
				float num36;
				switch (num35 & 15)
				{
				case 0:
					num36 = num24 + num25;
					break;
				case 1:
					num36 = -num24 + num25;
					break;
				case 2:
					num36 = num24 - num25;
					break;
				case 3:
					num36 = -num24 - num25;
					break;
				case 4:
					num36 = num24 + num24;
					break;
				case 5:
					num36 = -num24 + num24;
					break;
				case 6:
					num36 = num24 - num24;
					break;
				case 7:
					num36 = -num24 - num24;
					break;
				case 8:
					num36 = num25 + num24;
					break;
				case 9:
					num36 = -num25 + num24;
					break;
				case 10:
					num36 = num25 - num24;
					break;
				case 11:
					num36 = -num25 - num24;
					break;
				case 12:
					num36 = num25 + num26;
					break;
				case 13:
					num36 = -num25 + num24;
					break;
				case 14:
					num36 = num25 - num24;
					break;
				case 15:
					num36 = -num25 - num26;
					break;
				default:
					num36 = 0f;
					break;
				}
				num40 = num39 * num39 * num36;
			}
			float num41 = 0.6f - num27 * num27 - num28 * num28 - num29 * num29;
			float num42;
			if (num41 < 0f)
			{
				num42 = 0f;
			}
			else
			{
				num41 *= num41;
				int num35 = (int)Noise2.perm[num30 + 1 + (int)Noise2.perm[num31 + 1 + (int)Noise2.perm[num32 + 1]]];
				float num36;
				switch (num35 & 15)
				{
				case 0:
					num36 = num27 + num28;
					break;
				case 1:
					num36 = -num27 + num28;
					break;
				case 2:
					num36 = num27 - num28;
					break;
				case 3:
					num36 = -num27 - num28;
					break;
				case 4:
					num36 = num27 + num27;
					break;
				case 5:
					num36 = -num27 + num27;
					break;
				case 6:
					num36 = num27 - num27;
					break;
				case 7:
					num36 = -num27 - num27;
					break;
				case 8:
					num36 = num28 + num27;
					break;
				case 9:
					num36 = -num28 + num27;
					break;
				case 10:
					num36 = num28 - num27;
					break;
				case 11:
					num36 = -num28 - num27;
					break;
				case 12:
					num36 = num28 + num29;
					break;
				case 13:
					num36 = -num28 + num27;
					break;
				case 14:
					num36 = num28 - num27;
					break;
				case 15:
					num36 = -num28 - num29;
					break;
				default:
					num36 = 0f;
					break;
				}
				num42 = num41 * num41 * num36;
			}
			return 32f * (num34 + num38 + num40 + num42);
		}

		public static float octavePerlin(float x, float y, float z, float frequency, int octaves, float lacunarity, float persistence)
		{
			if (octaves < 2)
			{
				return Noise2.perlin(x * frequency, y * frequency, z * frequency);
			}
			float num = 0f;
			float num2 = 1f;
			float num3 = 0f;
			for (int i = 0; i < octaves; i++)
			{
				num += Noise2.perlin(x * frequency, y * frequency, z * frequency) * num2;
				num3 += num2;
				frequency *= lacunarity;
				num2 *= persistence;
			}
			return num / num3;
		}

		public static float octaveSimplex(float x, float y, float z, float frequency, int octaves, float lacunarity, float persistence)
		{
			if (octaves < 2)
			{
				return Noise2.simplex(x * frequency, y * frequency, z * frequency);
			}
			float num = 0f;
			float num2 = 1f;
			float num3 = 0f;
			for (int i = 0; i < octaves; i++)
			{
				num += Noise2.simplex(x * frequency, y * frequency, z * frequency) * num2;
				num3 += num2;
				frequency *= lacunarity;
				num2 *= persistence;
			}
			return num / num3;
		}

		public static float perlinUnoptimized(float x, float y, float z)
		{
			int num = Noise2.floor(x);
			int num2 = Noise2.floor(y);
			int num3 = Noise2.floor(z);
			float num4 = x - (float)num;
			float num5 = y - (float)num2;
			float num6 = z - (float)num3;
			float x2 = num4 - 1f;
			float y2 = num5 - 1f;
			float z2 = num6 - 1f;
			int num7 = num + 1 & 255;
			int num8 = num2 + 1 & 255;
			int num9 = num3 + 1 & 255;
			num &= 255;
			num2 &= 255;
			num3 &= 255;
			float t = Noise2.fade(num6);
			float t2 = Noise2.fade(num5);
			float t3 = Noise2.fade(num4);
			float from = Noise2.grad((int)Noise2.perm[num + (int)Noise2.perm[num2 + (int)Noise2.perm[num3]]], num4, num5, num6);
			float to = Noise2.grad((int)Noise2.perm[num + (int)Noise2.perm[num2 + (int)Noise2.perm[num9]]], num4, num5, z2);
			float from2 = Noise2.lerp(from, to, t);
			from = Noise2.grad((int)Noise2.perm[num + (int)Noise2.perm[num8 + (int)Noise2.perm[num3]]], num4, y2, num6);
			to = Noise2.grad((int)Noise2.perm[num + (int)Noise2.perm[num8 + (int)Noise2.perm[num9]]], num4, y2, z2);
			float to2 = Noise2.lerp(from, to, t);
			float from3 = Noise2.lerp(from2, to2, t2);
			from = Noise2.grad((int)Noise2.perm[num7 + (int)Noise2.perm[num2 + (int)Noise2.perm[num3]]], x2, num5, num6);
			to = Noise2.grad((int)Noise2.perm[num7 + (int)Noise2.perm[num2 + (int)Noise2.perm[num9]]], x2, num5, z2);
			from2 = Noise2.lerp(from, to, t);
			from = Noise2.grad((int)Noise2.perm[num7 + (int)Noise2.perm[num8 + (int)Noise2.perm[num3]]], x2, y2, num6);
			to = Noise2.grad((int)Noise2.perm[num7 + (int)Noise2.perm[num8 + (int)Noise2.perm[num9]]], x2, y2, z2);
			to2 = Noise2.lerp(from, to, t);
			float to3 = Noise2.lerp(from2, to2, t2);
			return 0.936f * Noise2.lerp(from3, to3, t3);
		}

		public static float simplexUnoptimized(float x, float y, float z)
		{
			float num = (x + y + z) * Noise2.F3;
			float x2 = x + num;
			float x3 = y + num;
			float x4 = z + num;
			int num2 = Noise2.floor(x2);
			int num3 = Noise2.floor(x3);
			int num4 = Noise2.floor(x4);
			float num5 = (float)(num2 + num3 + num4) * Noise2.G3;
			float num6 = (float)num2 - num5;
			float num7 = (float)num3 - num5;
			float num8 = (float)num4 - num5;
			float num9 = x - num6;
			float num10 = y - num7;
			float num11 = z - num8;
			int num12;
			int num13;
			int num14;
			int num15;
			int num16;
			int num17;
			if (num9 >= num10)
			{
				if (num10 >= num11)
				{
					num12 = 1;
					num13 = 0;
					num14 = 0;
					num15 = 1;
					num16 = 1;
					num17 = 0;
				}
				else if (num9 >= num11)
				{
					num12 = 1;
					num13 = 0;
					num14 = 0;
					num15 = 1;
					num16 = 0;
					num17 = 1;
				}
				else
				{
					num12 = 0;
					num13 = 0;
					num14 = 1;
					num15 = 1;
					num16 = 0;
					num17 = 1;
				}
			}
			else if (num10 < num11)
			{
				num12 = 0;
				num13 = 0;
				num14 = 1;
				num15 = 0;
				num16 = 1;
				num17 = 1;
			}
			else if (num9 < num11)
			{
				num12 = 0;
				num13 = 1;
				num14 = 0;
				num15 = 0;
				num16 = 1;
				num17 = 1;
			}
			else
			{
				num12 = 0;
				num13 = 1;
				num14 = 0;
				num15 = 1;
				num16 = 1;
				num17 = 0;
			}
			float num18 = num9 - (float)num12 + Noise2.G3;
			float num19 = num10 - (float)num13 + Noise2.G3;
			float num20 = num11 - (float)num14 + Noise2.G3;
			float num21 = num9 - (float)num15 + 2f * Noise2.G3;
			float num22 = num10 - (float)num16 + 2f * Noise2.G3;
			float num23 = num11 - (float)num17 + 2f * Noise2.G3;
			float num24 = num9 - 1f + 3f * Noise2.G3;
			float num25 = num10 - 1f + 3f * Noise2.G3;
			float num26 = num11 - 1f + 3f * Noise2.G3;
			int num27 = num2 & 255;
			int num28 = num3 & 255;
			int num29 = num4 & 255;
			float num30 = 0.6f - num9 * num9 - num10 * num10 - num11 * num11;
			float num31;
			if (num30 < 0f)
			{
				num31 = 0f;
			}
			else
			{
				num30 *= num30;
				num31 = num30 * num30 * Noise2.grad((int)Noise2.perm[num27 + (int)Noise2.perm[num28 + (int)Noise2.perm[num29]]], num9, num10, num11);
			}
			float num32 = 0.6f - num18 * num18 - num19 * num19 - num20 * num20;
			float num33;
			if (num32 < 0f)
			{
				num33 = 0f;
			}
			else
			{
				num32 *= num32;
				num33 = num32 * num32 * Noise2.grad((int)Noise2.perm[num27 + num12 + (int)Noise2.perm[num28 + num13 + (int)Noise2.perm[num29 + num14]]], num18, num19, num20);
			}
			float num34 = 0.6f - num21 * num21 - num22 * num22 - num23 * num23;
			float num35;
			if (num34 < 0f)
			{
				num35 = 0f;
			}
			else
			{
				num34 *= num34;
				num35 = num34 * num34 * Noise2.grad((int)Noise2.perm[num27 + num15 + (int)Noise2.perm[num28 + num16 + (int)Noise2.perm[num29 + num17]]], num21, num22, num23);
			}
			float num36 = 0.6f - num24 * num24 - num25 * num25 - num26 * num26;
			float num37;
			if (num36 < 0f)
			{
				num37 = 0f;
			}
			else
			{
				num36 *= num36;
				num37 = num36 * num36 * Noise2.grad((int)Noise2.perm[num27 + 1 + (int)Noise2.perm[num28 + 1 + (int)Noise2.perm[num29 + 1]]], num24, num25, num26);
			}
			return 32f * (num31 + num33 + num35 + num37);
		}

		private static float F3 = 0.333333343f;

		private static float G3 = 0.166666672f;

		private static byte[] perm = new byte[]
		{
			151,
			160,
			137,
			91,
			90,
			15,
			131,
			13,
			201,
			95,
			96,
			53,
			194,
			233,
			7,
			225,
			140,
			36,
			103,
			30,
			69,
			142,
			8,
			99,
			37,
			240,
			21,
			10,
			23,
			190,
			6,
			148,
			247,
			120,
			234,
			75,
			0,
			26,
			197,
			62,
			94,
			252,
			219,
			203,
			117,
			35,
			11,
			32,
			57,
			177,
			33,
			88,
			237,
			149,
			56,
			87,
			174,
			20,
			125,
			136,
			171,
			168,
			68,
			175,
			74,
			165,
			71,
			134,
			139,
			48,
			27,
			166,
			77,
			146,
			158,
			231,
			83,
			111,
			229,
			122,
			60,
			211,
			133,
			230,
			220,
			105,
			92,
			41,
			55,
			46,
			245,
			40,
			244,
			102,
			143,
			54,
			65,
			25,
			63,
			161,
			1,
			216,
			80,
			73,
			209,
			76,
			132,
			187,
			208,
			89,
			18,
			169,
			200,
			196,
			135,
			130,
			116,
			188,
			159,
			86,
			164,
			100,
			109,
			198,
			173,
			186,
			3,
			64,
			52,
			217,
			226,
			250,
			124,
			123,
			5,
			202,
			38,
			147,
			118,
			126,
			byte.MaxValue,
			82,
			85,
			212,
			207,
			206,
			59,
			227,
			47,
			16,
			58,
			17,
			182,
			189,
			28,
			42,
			223,
			183,
			170,
			213,
			119,
			248,
			152,
			2,
			44,
			154,
			163,
			70,
			221,
			153,
			101,
			155,
			167,
			43,
			172,
			9,
			129,
			22,
			39,
			253,
			19,
			98,
			108,
			110,
			79,
			113,
			224,
			232,
			178,
			185,
			112,
			104,
			218,
			246,
			97,
			228,
			251,
			34,
			242,
			193,
			238,
			210,
			144,
			12,
			191,
			179,
			162,
			241,
			81,
			51,
			145,
			235,
			249,
			14,
			239,
			107,
			49,
			192,
			214,
			31,
			181,
			199,
			106,
			157,
			184,
			84,
			204,
			176,
			115,
			121,
			50,
			45,
			127,
			4,
			150,
			254,
			138,
			236,
			205,
			93,
			222,
			114,
			67,
			29,
			24,
			72,
			243,
			141,
			128,
			195,
			78,
			66,
			215,
			61,
			156,
			180,
			151,
			160,
			137,
			91,
			90,
			15,
			131,
			13,
			201,
			95,
			96,
			53,
			194,
			233,
			7,
			225,
			140,
			36,
			103,
			30,
			69,
			142,
			8,
			99,
			37,
			240,
			21,
			10,
			23,
			190,
			6,
			148,
			247,
			120,
			234,
			75,
			0,
			26,
			197,
			62,
			94,
			252,
			219,
			203,
			117,
			35,
			11,
			32,
			57,
			177,
			33,
			88,
			237,
			149,
			56,
			87,
			174,
			20,
			125,
			136,
			171,
			168,
			68,
			175,
			74,
			165,
			71,
			134,
			139,
			48,
			27,
			166,
			77,
			146,
			158,
			231,
			83,
			111,
			229,
			122,
			60,
			211,
			133,
			230,
			220,
			105,
			92,
			41,
			55,
			46,
			245,
			40,
			244,
			102,
			143,
			54,
			65,
			25,
			63,
			161,
			1,
			216,
			80,
			73,
			209,
			76,
			132,
			187,
			208,
			89,
			18,
			169,
			200,
			196,
			135,
			130,
			116,
			188,
			159,
			86,
			164,
			100,
			109,
			198,
			173,
			186,
			3,
			64,
			52,
			217,
			226,
			250,
			124,
			123,
			5,
			202,
			38,
			147,
			118,
			126,
			byte.MaxValue,
			82,
			85,
			212,
			207,
			206,
			59,
			227,
			47,
			16,
			58,
			17,
			182,
			189,
			28,
			42,
			223,
			183,
			170,
			213,
			119,
			248,
			152,
			2,
			44,
			154,
			163,
			70,
			221,
			153,
			101,
			155,
			167,
			43,
			172,
			9,
			129,
			22,
			39,
			253,
			19,
			98,
			108,
			110,
			79,
			113,
			224,
			232,
			178,
			185,
			112,
			104,
			218,
			246,
			97,
			228,
			251,
			34,
			242,
			193,
			238,
			210,
			144,
			12,
			191,
			179,
			162,
			241,
			81,
			51,
			145,
			235,
			249,
			14,
			239,
			107,
			49,
			192,
			214,
			31,
			181,
			199,
			106,
			157,
			184,
			84,
			204,
			176,
			115,
			121,
			50,
			45,
			127,
			4,
			150,
			254,
			138,
			236,
			205,
			93,
			222,
			114,
			67,
			29,
			24,
			72,
			243,
			141,
			128,
			195,
			78,
			66,
			215,
			61,
			156,
			180
		};
	}
}
