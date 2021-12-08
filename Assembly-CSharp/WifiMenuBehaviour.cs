using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WifiMenuBehaviour : MonoBehaviour, IPointerExitHandler, IEventSystemHandler
{
	public void refreshNetworks()
	{
		this.buildNetworks(GameManager.ManagerSlinger.WifiManager.GetCurrentWifiNetworks());
	}

	private void buildNetworks(List<WifiNetworkDefinition> Networks)
	{
		this.menuSepObject.GetComponent<RectTransform>().anchoredPosition = this.menuOptOffScreenPOS;
		this.menuDisObject.GetComponent<RectTransform>().anchoredPosition = this.menuOptOffScreenPOS;
		for (int i = 0; i < this.currentOptions.Count; i++)
		{
			this.currentOptions[i].Clear();
			this.wifiMenuOptionObjectPool.Push(this.currentOptions[i]);
		}
		this.currentOptions.Clear();
		float num = this.WifiMenuOption.GetComponent<RectTransform>().sizeDelta.y * (float)Networks.Count + 3f * (float)Networks.Count + 13f;
		float num2 = -3f;
		float num3 = Mathf.Floor(num / 2f);
		WifiNetworkDefinition wifiNetworkDefinition;
		bool currentConnectedNetwork = GameManager.ManagerSlinger.WifiManager.GetCurrentConnectedNetwork(out wifiNetworkDefinition);
		if (currentConnectedNetwork)
		{
			num = num + 6f + 1f + this.WifiMenuDisconnect.GetComponent<RectTransform>().sizeDelta.y;
			num3 = Mathf.Floor(num / 2f);
		}
		this.menuSize.x = base.GetComponent<RectTransform>().sizeDelta.x;
		this.menuSize.y = num;
		this.menuPOS.x = base.GetComponent<RectTransform>().anchoredPosition.x;
		this.menuPOS.y = num;
		base.GetComponent<RectTransform>().sizeDelta = this.menuSize;
		base.GetComponent<RectTransform>().anchoredPosition = this.menuPOS;
		for (int j = 0; j < Networks.Count; j++)
		{
			WifiMenuNetworkOptionObject wifiMenuNetworkOptionObject = this.wifiMenuOptionObjectPool.Pop();
			this.menuOptionPOS.y = num2;
			bool connected = false;
			if (currentConnectedNetwork && wifiNetworkDefinition.networkName == Networks[j].networkName)
			{
				connected = true;
			}
			wifiMenuNetworkOptionObject.Build(connected, Networks[j], this.menuOptionPOS);
			this.currentOptions.Add(wifiMenuNetworkOptionObject);
			num2 = num2 - wifiMenuNetworkOptionObject.GetComponent<RectTransform>().sizeDelta.y - 3f;
		}
		if (currentConnectedNetwork)
		{
			this.menuOptionPOS.y = this.menuOptionPOS.y - this.WifiMenuOption.GetComponent<RectTransform>().sizeDelta.y - 3f;
			this.menuSepObject.GetComponent<RectTransform>().anchoredPosition = this.menuOptionPOS;
			this.menuOptionPOS.y = this.menuOptionPOS.y - 3f - 1f;
			this.menuDisObject.GetComponent<RectTransform>().anchoredPosition = this.menuOptionPOS;
		}
	}

	private void Awake()
	{
		WifiMenuBehaviour.Ins = this;
		this.menuSepObject = UnityEngine.Object.Instantiate<GameObject>(this.WifiMenuSeperator, base.GetComponent<RectTransform>());
		this.menuDisObject = UnityEngine.Object.Instantiate<GameObject>(this.WifiMenuDisconnect, base.GetComponent<RectTransform>());
		this.menuSepObject.GetComponent<RectTransform>().anchoredPosition = this.menuOptOffScreenPOS;
		this.menuDisObject.GetComponent<RectTransform>().anchoredPosition = this.menuOptOffScreenPOS;
		this.wifiMenuOptionObjectPool = new PooledStack<WifiMenuNetworkOptionObject>(delegate()
		{
			WifiMenuNetworkOptionObject component = UnityEngine.Object.Instantiate<GameObject>(this.WifiMenuOption, base.GetComponent<RectTransform>()).GetComponent<WifiMenuNetworkOptionObject>();
			component.SoftBuild();
			return component;
		}, 8);
		GameManager.ManagerSlinger.WifiManager.NewNetworksAvailable += this.buildNetworks;
	}

	private void Start()
	{
		GameManager.ManagerSlinger.WifiManager.WentOnline += this.refreshNetworks;
		GameManager.ManagerSlinger.WifiManager.WentOffline += this.refreshNetworks;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.WifiManager.TriggerWifiMenu();
	}

	private void OnDestroy()
	{
	}

	private const int MENU_OPT_START_POOL = 8;

	private const float OPT_SPACING = 3f;

	private const float OPT_SETX = 10f;

	private const float MENU_BOT = 13f;

	public GameObject WifiMenuOption;

	public GameObject WifiMenuDisconnect;

	public GameObject WifiMenuSeperator;

	private GameObject menuSepObject;

	private GameObject menuDisObject;

	private PooledStack<WifiMenuNetworkOptionObject> wifiMenuOptionObjectPool;

	private List<WifiMenuNetworkOptionObject> currentOptions = new List<WifiMenuNetworkOptionObject>();

	private Vector2 menuOptionPOS = new Vector2(10f, 0f);

	private Vector2 menuPOS = Vector2.zero;

	private Vector2 menuSize = Vector2.zero;

	private Vector2 menuOptOffScreenPOS = new Vector2(0f, 24f);

	public static WifiMenuBehaviour Ins;
}
