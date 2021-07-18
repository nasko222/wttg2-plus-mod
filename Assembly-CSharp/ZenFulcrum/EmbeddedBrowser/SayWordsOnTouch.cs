using System;
using System.Collections;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class SayWordsOnTouch : MonoBehaviour
	{
		public static int ActiveSpeakers { get; private set; }

		public void OnTriggerEnter(Collider other)
		{
			if (this.triggered)
			{
				return;
			}
			PlayerInventory component = other.GetComponent<PlayerInventory>();
			if (!component)
			{
				return;
			}
			this.triggered = true;
			this.stillTriggered = true;
			SayWordsOnTouch.ActiveSpeakers++;
			base.StartCoroutine(this.SayStuff());
			BoxCollider component2 = base.GetComponent<BoxCollider>();
			if (component2)
			{
				Vector3 size = component2.size;
				size.x += this.extraLeaveRange * 2f;
				size.y += this.extraLeaveRange * 2f;
				size.z += this.extraLeaveRange * 2f;
				component2.size = size;
			}
		}

		private IEnumerator SayStuff()
		{
			int idx = 0;
			while (idx < this.thingsToSay.Length && this.stillTriggered)
			{
				yield return new WaitForSeconds(this.thingsToSay[idx].delay);
				if (!this.stillTriggered)
				{
					break;
				}
				HUDManager.Instance.Say(this.thingsToSay[idx].textHTML, this.thingsToSay[idx].dwellTime);
				idx++;
			}
			SayWordsOnTouch.ActiveSpeakers--;
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		public void OnTriggerExit(Collider other)
		{
			PlayerInventory component = other.GetComponent<PlayerInventory>();
			if (!component)
			{
				return;
			}
			this.stillTriggered = false;
		}

		public SayWordsOnTouch.Verse[] thingsToSay;

		private bool triggered;

		private bool stillTriggered;

		public float extraLeaveRange;

		[Serializable]
		public class Verse
		{
			public float delay;

			[Multiline]
			public string textHTML;

			public float dwellTime = 5f;
		}
	}
}
