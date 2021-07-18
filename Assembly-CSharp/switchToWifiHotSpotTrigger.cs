using System;
using UnityEngine;

public class switchToWifiHotSpotTrigger : MonoBehaviour
{
	private void placeDongle()
	{
		this.myWifiHotspotObject.GetComponent<WifiHotspotObject>().PlaceDongleHere();
	}

	private void triggerHover()
	{
		this.myWifiHotspotObject.GetComponent<WifiHotspotObject>().ShowPreview();
	}

	private void triggerOffHover()
	{
		this.myWifiHotspotObject.GetComponent<WifiHotspotObject>().HidePreview();
	}

	private void Awake()
	{
		base.GetComponent<InteractionHook>().LeftClickAction += this.placeDongle;
		base.GetComponent<InteractionHook>().RecvAction += this.triggerHover;
		base.GetComponent<InteractionHook>().RecindAction += this.triggerOffHover;
	}

	private void OnDestroy()
	{
		base.GetComponent<InteractionHook>().LeftClickAction -= this.placeDongle;
		base.GetComponent<InteractionHook>().RecvAction -= this.triggerHover;
		base.GetComponent<InteractionHook>().RecindAction -= this.triggerOffHover;
	}

	public GameObject myWifiHotspotObject;
}
