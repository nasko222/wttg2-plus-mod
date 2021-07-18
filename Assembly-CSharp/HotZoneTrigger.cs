using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HotZoneTrigger : MonoBehaviour
{
	public void OnTriggerStay(Collider other)
	{
		this.IsHot = true;
	}

	public void OnTriggerExit(Collider other)
	{
		if (this.IsHot)
		{
			this.LeftHotZoneEvent.Execute();
		}
		this.IsHot = false;
	}

	public bool IsHot;

	public CustomEvent LeftHotZoneEvent = new CustomEvent(2);
}
