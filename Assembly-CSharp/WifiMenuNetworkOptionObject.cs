using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WifiMenuNetworkOptionObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void Clear()
	{
		this.NetworkName1.GetComponent<Text>().text = string.Empty;
		this.NetworkName2.GetComponent<Text>().text = string.Empty;
		this.NetworkConnected.SetActive(false);
		this.NetworkSecurity.SetActive(false);
		base.GetComponent<RectTransform>().anchoredPosition = this.myPOS;
	}

	public void SoftBuild()
	{
		base.GetComponent<RectTransform>().anchoredPosition = this.myPOS;
	}

	public void Build(bool Connected, WifiNetworkDefinition MyNetwork, Vector2 SetPOS)
	{
		this.NetworkName1.GetComponent<Text>().text = MyNetwork.networkName;
		this.NetworkName1.GetComponent<Text>().text = MyNetwork.networkName;
		if (Connected)
		{
			this.NetworkConnected.SetActive(true);
		}
		if (MyNetwork.networkSecurity > WIFI_SECURITY.NONE)
		{
			this.NetworkSecurity.SetActive(true);
		}
		int index = Mathf.Min((int)MyNetwork.networkStrength + InventoryManager.GetWifiBoostLevel(), 3);
		this.NetworkStrength.GetComponent<Image>().sprite = this.NetworkBarSprites[index];
		this.myWifiNetwork = MyNetwork;
		base.GetComponent<RectTransform>().anchoredPosition = SetPOS;
	}

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
		GameManager.ManagerSlinger.WifiManager.ConnectToWifi(this.myWifiNetwork, false);
	}

	public GameObject NetworkName1;

	public GameObject NetworkName2;

	public GameObject NetworkConnected;

	public GameObject NetworkSecurity;

	public GameObject NetworkStrength;

	public List<Sprite> NetworkBarSprites;

	public Color hoverColor;

	private Color defaultColor;

	private WifiNetworkDefinition myWifiNetwork;

	private Vector2 myPOS = new Vector2(10f, 24f);
}
