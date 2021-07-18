using System;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects
{
	public static class CreateLUT
	{
		public static void fromGradient(int steps, Gradient gradient, ref Texture2D texture)
		{
			if (texture)
			{
				UnityEngine.Object.Destroy(texture);
			}
			texture = new Texture2D(steps, 1);
			texture.SetPixel(0, 0, gradient.Evaluate(0f));
			texture.SetPixel(steps - 1, 0, gradient.Evaluate(1f));
			for (int i = 1; i < steps - 1; i++)
			{
				Color color = gradient.Evaluate((float)i / (float)steps);
				texture.SetPixel(i, 0, color);
			}
			texture.Apply();
		}

		public static void fromAnimationCurve(int steps, AnimationCurve curve, ref Texture2D texture)
		{
			if (texture)
			{
				UnityEngine.Object.Destroy(texture);
			}
			texture = new Texture2D(steps, 1);
			texture.SetPixel(0, 0, new Color(0f, 0f, 0f, curve.Evaluate(0f)));
			texture.SetPixel(steps - 1, 0, new Color(0f, 0f, 0f, curve.Evaluate(1f)));
			for (int i = 1; i < steps - 1; i++)
			{
				float a = curve.Evaluate((float)i / (float)steps);
				texture.SetPixel(i, 0, new Color(0f, 0f, 0f, a));
			}
			texture.Apply();
		}
	}
}
