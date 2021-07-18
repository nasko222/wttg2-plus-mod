using System;
using System.Collections;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class ForcedCooperation : MonoBehaviour
	{
		public void Comply()
		{
			base.StartCoroutine(this._Comply());
		}

		protected IEnumerator _Comply()
		{
			float t0 = Time.time;
			do
			{
				Vector3 pos = base.transform.InverseTransformPoint(this.whoWillComply.position);
				if (pos.z > 0f)
				{
					pos.z = 0f;
					this.whoWillComply.position = base.transform.TransformPoint(pos);
				}
				yield return null;
			}
			while (Time.time - t0 < this.howLongWillTheyComply);
			yield break;
		}

		public Transform whoWillComply;

		public float howLongWillTheyComply;
	}
}
