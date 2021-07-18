using System;
using UnityEngine;

namespace Colorful
{
	public sealed class MinAttribute : PropertyAttribute
	{
		public MinAttribute(float min)
		{
			this.Min = min;
		}

		public readonly float Min;
	}
}
