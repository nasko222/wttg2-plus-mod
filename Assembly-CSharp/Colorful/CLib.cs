using System;
using UnityEngine;

namespace Colorful
{
	public static class CLib
	{
		public static float Frac(float f)
		{
			return f - Mathf.Floor(f);
		}

		public static bool IsLinearColorSpace()
		{
			return QualitySettings.activeColorSpace == ColorSpace.Linear;
		}

		public static bool Approximately(float source, float about, float range = 0.0001f)
		{
			return Mathf.Abs(source - about) < range;
		}

		public const float PI_2 = 1.57079637f;

		public const float PI2 = 6.28318548f;
	}
}
