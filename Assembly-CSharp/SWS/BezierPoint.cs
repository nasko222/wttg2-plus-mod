using System;
using UnityEngine;

namespace SWS
{
	[Serializable]
	public class BezierPoint
	{
		public Transform wp;

		public Transform[] cp = new Transform[2];
	}
}
