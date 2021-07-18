using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class ActionTimer : MonoBehaviour
	{
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
			base.StartCoroutine(this.DoThings());
		}

		private IEnumerator DoThings()
		{
			for (int idx = 0; idx < this.thingsToDo.Length; idx++)
			{
				yield return new WaitForSeconds(this.thingsToDo[idx].delay);
				this.thingsToDo[idx].action.Invoke();
			}
			yield break;
		}

		public ActionTimer.TimedAction[] thingsToDo;

		private bool triggered;

		[Serializable]
		public class TimedAction
		{
			public float delay;

			public UnityEvent action;
		}
	}
}
