using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class bringMeToFrontForce : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.bringMeToFrontHolder != null && this.bringMeToFrontHolder.GetComponent<BringWindowToFrontBehaviour>() != null)
		{
			this.bringMeToFrontHolder.GetComponent<BringWindowToFrontBehaviour>().forceTap();
		}
	}

	public GameObject bringMeToFrontHolder;
}
