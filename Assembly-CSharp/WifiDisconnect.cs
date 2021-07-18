using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WifiDisconnect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	private void Start()
	{
		this.defaultColor = base.GetComponent<Image>().color;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		base.GetComponent<Image>().color = this.hoverColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		base.GetComponent<Image>().color = this.defaultColor;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.WifiManager.TriggerWifiMenu();
		GameManager.ManagerSlinger.WifiManager.DisconnectFromWifi();
	}

	public Color hoverColor;

	private Color defaultColor;
}
