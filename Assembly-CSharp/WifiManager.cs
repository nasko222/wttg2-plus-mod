using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class WifiManager : MonoBehaviour
{
	public bool IsOnline
	{
		get
		{
			return this.isOnline;
		}
	}

	public Dictionary<string, string> PasswordList
	{
		get
		{
			return this.passwordList;
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event WifiManager.OnlineOfflineActions WentOnline;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event WifiManager.OnlineOfflineActions WentOffline;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event WifiManager.OnlineWithNetworkActions OnlineWithNetwork;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event WifiManager.NewNetworksActions NewNetworksAvailable;

	public void EnterWifiDonglePlacementMode()
	{
		UIInventoryManager.ShowWifiDongle();
		this.inWifiPlacementMode = true;
		this.DisconnectFromWifi();
		this.ShowWifiHotSpots();
		StateManager.PlayerState = PLAYER_STATE.WIFI_DONGLE_PLACEMENT;
	}

	public void ExitWifiDonglePlacementMode(WifiHotspotObject newWifiHotSpot)
	{
		UIInventoryManager.HideWifiDongle();
		this.inWifiPlacementMode = false;
		this.HideWifiHotSpots();
		this.activeWifiHotSpot = newWifiHotSpot;
		this.theWifiDongle.PlaceDongle(this.activeWifiHotSpot.DonglePlacedPOS, this.activeWifiHotSpot.DonglePlacedROT, true);
		StateManager.PlayerState = PLAYER_STATE.ROAMING;
		for (int i = 0; i < this.wifiHotSpots.Count; i++)
		{
			if (this.wifiHotSpots[i] == newWifiHotSpot)
			{
				this.myData.ActiveWifiHotSpotIndex = i;
				i = this.wifiHotSpots.Count;
			}
		}
		DataManager.Save<WifiManagerData>(this.myData);
		if (this.NewNetworksAvailable != null)
		{
			this.NewNetworksAvailable(this.GetCurrentWifiNetworks());
		}
	}

	public void ShowWifiHotSpots()
	{
		for (int i = 0; i < this.wifiHotSpots.Count; i++)
		{
			this.wifiHotSpots[i].ActivateMe();
		}
	}

	public void HideWifiHotSpots()
	{
		for (int i = 0; i < this.wifiHotSpots.Count; i++)
		{
			this.wifiHotSpots[i].DeactivateMe();
		}
	}

	public void ConnectToWifi(WifiNetworkDefinition wifiNetwork, bool byPassSecuirty = false)
	{
		if (this.isOnline)
		{
			this.DisconnectFromWifi();
		}
		int wifiBarAmount = Mathf.Min((int)wifiNetwork.networkStrength + InventoryManager.GetWifiBoostLevel(), 3);
		if (!wifiNetwork.networkIsOffline)
		{
			if (byPassSecuirty)
			{
				this.isOnline = true;
				this.currentWifiNetwork = wifiNetwork;
				this.changeWifiBars(wifiBarAmount);
				this.myData.CurrentWifiNetworkIndex = this.activeWifiHotSpot.GetWifiNetworkIndex(wifiNetwork);
				this.myData.IsConnected = true;
				DataManager.Save<WifiManagerData>(this.myData);
				if (this.WentOnline != null)
				{
					this.WentOnline();
				}
				if (this.OnlineWithNetwork != null)
				{
					this.OnlineWithNetwork(wifiNetwork);
				}
			}
			else if (wifiNetwork.networkSecurity != WIFI_SECURITY.NONE)
			{
				UIDialogManager.NetworkDialog.Present(wifiNetwork);
			}
			else
			{
				this.isOnline = true;
				this.currentWifiNetwork = wifiNetwork;
				this.changeWifiBars(wifiBarAmount);
				this.myData.CurrentWifiNetworkIndex = this.activeWifiHotSpot.GetWifiNetworkIndex(wifiNetwork);
				this.myData.IsConnected = true;
				DataManager.Save<WifiManagerData>(this.myData);
				if (this.WentOnline != null)
				{
					this.WentOnline();
				}
				if (this.OnlineWithNetwork != null)
				{
					this.OnlineWithNetwork(wifiNetwork);
				}
			}
		}
	}

	public void DisconnectFromWifi()
	{
		if (!this.isOnline)
		{
			return;
		}
		this.isOnline = false;
		this.currentWifiNetwork = null;
		this.changeWifiBars(0);
		this.myData.IsConnected = false;
		DataManager.Save<WifiManagerData>(this.myData);
		if (this.WentOffline != null)
		{
			this.WentOffline();
		}
	}

	public void TakeNetworkOffLine(WifiNetworkDefinition wifiNetwork)
	{
		if (this.currentWifiNetwork == wifiNetwork)
		{
			this.DisconnectFromWifi();
		}
		wifiNetwork.networkIsOffline = true;
		if (this.NewNetworksAvailable != null)
		{
			this.NewNetworksAvailable(this.GetCurrentWifiNetworks());
		}
		GameManager.TimeSlinger.FireTimer<WifiNetworkDefinition>(wifiNetwork.networkCoolOffTime, new Action<WifiNetworkDefinition>(this.PutNetworkBackOnline), wifiNetwork, 0);
	}

	public void PutNetworkBackOnline(WifiNetworkDefinition wifiNetwork)
	{
		wifiNetwork.networkIsOffline = false;
		if (this.NewNetworksAvailable != null)
		{
			this.NewNetworksAvailable(this.GetCurrentWifiNetworks());
		}
	}

	public List<WifiNetworkDefinition> GetAllWifiNetworks()
	{
		List<WifiNetworkDefinition> list = new List<WifiNetworkDefinition>();
		for (int i = 0; i < this.wifiHotSpots.Count; i++)
		{
			for (int j = 0; j < this.wifiHotSpots[i].myWifiNetworks.Count; j++)
			{
				list.Add(this.wifiHotSpots[i].myWifiNetworks[j]);
			}
		}
		return list;
	}

	public List<WifiNetworkDefinition> GetCurrentWifiNetworks()
	{
		List<WifiNetworkDefinition> list = new List<WifiNetworkDefinition>();
		int wifiBoostLevel = InventoryManager.GetWifiBoostLevel();
		for (int i = 0; i < this.activeWifiHotSpot.myWifiNetworks.Count; i++)
		{
			if (RouterBehaviour.Ins.Owned && RouterBehaviour.Ins.RouterIsActive)
			{
				if (!this.activeWifiHotSpot.myWifiNetworks[i].networkIsOffline && (int)this.activeWifiHotSpot.myWifiNetworks[i].networkStrength + wifiBoostLevel > 0)
				{
					list.Add(this.activeWifiHotSpot.myWifiNetworks[i]);
				}
			}
			else if (!this.activeWifiHotSpot.myWifiNetworks[i].networkIsOffline && (int)this.activeWifiHotSpot.myWifiNetworks[i].networkStrength + wifiBoostLevel > 0 && this.activeWifiHotSpot.myWifiNetworks[i].networkName != "TheProgrammingChair")
			{
				list.Add(this.activeWifiHotSpot.myWifiNetworks[i]);
			}
		}
		return list;
	}

	public List<WifiNetworkDefinition> GetSecureNetworks(WIFI_SECURITY SecuirtyType)
	{
		List<WifiNetworkDefinition> list = new List<WifiNetworkDefinition>();
		List<WifiNetworkDefinition> myWifiNetworks = this.activeWifiHotSpot.myWifiNetworks;
		int wifiBoostLevel = InventoryManager.GetWifiBoostLevel();
		for (int i = 0; i < myWifiNetworks.Count; i++)
		{
			if (RouterBehaviour.Ins.Owned && RouterBehaviour.Ins.RouterIsActive)
			{
				if (!myWifiNetworks[i].networkIsOffline && myWifiNetworks[i].networkSecurity == SecuirtyType && (int)myWifiNetworks[i].networkStrength + wifiBoostLevel > 0)
				{
					list.Add(myWifiNetworks[i]);
				}
			}
			else if (!myWifiNetworks[i].networkIsOffline && myWifiNetworks[i].networkSecurity == SecuirtyType && (int)myWifiNetworks[i].networkStrength + wifiBoostLevel > 0 && myWifiNetworks[i].networkName != "TheProgrammingChair")
			{
				list.Add(myWifiNetworks[i]);
			}
		}
		return list;
	}

	public bool GetCurrentConnectedNetwork(out WifiNetworkDefinition currentNetwork)
	{
		currentNetwork = this.currentWifiNetwork;
		return this.isOnline;
	}

	public bool CheckBSSID(string bssidToCheck, out WifiNetworkDefinition targetedWEP)
	{
		targetedWEP = null;
		bool result = false;
		for (int i = 0; i < this.activeWifiHotSpot.myWifiNetworks.Count; i++)
		{
			if (!this.activeWifiHotSpot.myWifiNetworks[i].networkIsOffline && this.activeWifiHotSpot.myWifiNetworks[i].networkBSSID == bssidToCheck)
			{
				result = true;
				targetedWEP = this.activeWifiHotSpot.myWifiNetworks[i];
				i = this.activeWifiHotSpot.myWifiNetworks.Count;
			}
		}
		return result;
	}

	public void TriggerWifiMenu()
	{
		if (!this.wifiMenuAniActive)
		{
			this.wifiMenuAniActive = true;
			if (this.wifiMenuActive)
			{
				this.wifiMenuActive = false;
				this.wifiMenuPOS.x = this.wifiMenu.GetComponent<RectTransform>().anchoredPosition.x;
				this.wifiMenuPOS.y = this.wifiMenu.GetComponent<RectTransform>().sizeDelta.y;
				DOTween.To(() => this.wifiMenu.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
				{
					this.wifiMenu.GetComponent<RectTransform>().anchoredPosition = x;
				}, this.wifiMenuPOS, 0.25f).SetEase(Ease.InQuad).OnComplete(delegate
				{
					this.wifiMenuAniActive = false;
				});
			}
			else
			{
				this.wifiMenuActive = true;
				this.wifiMenuPOS.x = this.wifiMenu.GetComponent<RectTransform>().anchoredPosition.x;
				this.wifiMenuPOS.y = -41f;
				DOTween.To(() => this.wifiMenu.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
				{
					this.wifiMenu.GetComponent<RectTransform>().anchoredPosition = x;
				}, this.wifiMenuPOS, 0.25f).SetEase(Ease.OutQuad).OnComplete(delegate
				{
					this.wifiMenuAniActive = false;
				});
			}
		}
	}

	public float GenereatePageLoadingTime()
	{
		float num = (float)this.currentWifiNetwork.networkPower * 0.2f;
		int num2 = Mathf.Min((int)this.currentWifiNetwork.networkStrength + InventoryManager.GetWifiBoostLevel(), 3);
		num *= 1f - (float)num2 * 20f / 100f;
		num *= 1f - (float)InventoryManager.GetWifiBoostLevel() * 10f / 100f;
		switch (this.currentWifiNetwork.networkSignal)
		{
		case WIFI_SIGNAL_TYPE.W80211B:
			num *= 0.95f;
			break;
		case WIFI_SIGNAL_TYPE.W80211BP:
			num *= 0.9f;
			break;
		case WIFI_SIGNAL_TYPE.W80211G:
			num *= 0.85f;
			break;
		case WIFI_SIGNAL_TYPE.W80211N:
			num *= 0.8f;
			break;
		case WIFI_SIGNAL_TYPE.W80211AC:
			num *= 0.75f;
			break;
		}
		num = Mathf.Max(num, 0.5f);
		return num + UnityEngine.Random.Range(0.25f, 0.75f);
	}

	private void changeWifiBars(int wifiBarAmount)
	{
		this.wifiIcon.GetComponent<Image>().sprite = this.wifiSprites[wifiBarAmount];
	}

	private void productWasPickedUp(ShadowMarketProductDefinition TheProduct)
	{
		HARDWARE_PRODUCTS productID = TheProduct.productID;
		if (productID != HARDWARE_PRODUCTS.WIFI_DONGLE_LEVEL2)
		{
			if (productID == HARDWARE_PRODUCTS.WIFI_DONGLE_LEVEL3)
			{
				InventoryManager.WifiDongleLevel = WIFI_DONGLE_LEVEL.LEVEL3;
				if (this.isOnline)
				{
					int wifiBarAmount = Mathf.Min((int)(this.currentWifiNetwork.networkStrength + 2), 3);
					this.changeWifiBars(wifiBarAmount);
				}
				if (this.NewNetworksAvailable != null)
				{
					this.NewNetworksAvailable(this.GetCurrentWifiNetworks());
				}
				this.theWifiDongle.RefreshActiveWifiDongleLevel();
				for (int i = 0; i < this.wifiHotSpots.Count; i++)
				{
					this.wifiHotSpots[i].RefreshPreviewDongle();
				}
				if (this.myData != null)
				{
					this.myData.OwnedWifiDongleLevel = 2;
					DataManager.Save<WifiManagerData>(this.myData);
				}
			}
		}
		else
		{
			InventoryManager.WifiDongleLevel = WIFI_DONGLE_LEVEL.LEVEL2;
			if (this.isOnline)
			{
				int wifiBarAmount2 = Mathf.Min((int)(this.currentWifiNetwork.networkStrength + 1), 3);
				this.changeWifiBars(wifiBarAmount2);
			}
			if (this.NewNetworksAvailable != null)
			{
				this.NewNetworksAvailable(this.GetCurrentWifiNetworks());
			}
			this.theWifiDongle.RefreshActiveWifiDongleLevel();
			for (int j = 0; j < this.wifiHotSpots.Count; j++)
			{
				this.wifiHotSpots[j].RefreshPreviewDongle();
			}
			if (this.myData != null)
			{
				this.myData.OwnedWifiDongleLevel = 1;
				DataManager.Save<WifiManagerData>(this.myData);
			}
		}
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<WifiManagerData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new WifiManagerData(this.myID);
			this.myData.ActiveWifiHotSpotIndex = 0;
			this.myData.CurrentWifiNetworkIndex = this.wifiHotSpots[0].GetWifiNetworkIndex(this.defaultWifiNetwork);
			this.myData.IsConnected = true;
			this.myData.OwnedWifiDongleLevel = 0;
		}
		InventoryManager.WifiDongleLevel = (WIFI_DONGLE_LEVEL)this.myData.OwnedWifiDongleLevel;
		this.activeWifiHotSpot = this.wifiHotSpots[this.myData.ActiveWifiHotSpotIndex];
		this.theWifiDongle.PlaceDongle(this.activeWifiHotSpot.DonglePlacedPOS, this.activeWifiHotSpot.DonglePlacedROT, false);
		if (this.myData.IsConnected)
		{
			this.currentWifiNetwork = this.activeWifiHotSpot.GetWifiNetworkDefByIndex(this.myData.CurrentWifiNetworkIndex);
			this.ConnectToWifi(this.currentWifiNetwork, true);
		}
		if (this.NewNetworksAvailable != null)
		{
			this.NewNetworksAvailable(this.GetCurrentWifiNetworks());
		}
		SteamSlinger.Ins.AddWifiNetworks(this.GetAllWifiNetworks());
		EnvironmentManager.PowerBehaviour.PowerOffEvent.Event += this.DisconnectFromWifi;
		for (int i = 0; i < this.GetAllWifiNetworks().Count; i++)
		{
			if (!(this.GetAllWifiNetworks()[i].networkName.ToLower() == "freewifinovirus"))
			{
				this.GetAllWifiNetworks()[i].affectedByDosDrainer = false;
			}
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		this.AddNewWiFi();
		this.myID = base.transform.position.GetHashCode();
		this.activeWifiHotSpot = this.wifiHotSpots[0];
		GameManager.ManagerSlinger.WifiManager = this;
		string[] array = this.PList.PasswordList.Split(new string[]
		{
			"\r\n"
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			this.passwordList.Add(MagicSlinger.MD5It(array[i]), array[i]);
		}
		this.wifiIcon = LookUp.DesktopUI.WIFI_ICON;
		this.wifiSprites = LookUp.DesktopUI.WIFI_SPRITES;
		this.wifiMenu = LookUp.DesktopUI.WIFI_MENU;
		GameManager.ManagerSlinger.ProductsManager.ShadowMarketProductWasActivated.Event += this.productWasPickedUp;
		GameManager.StageManager.Stage += this.stageMe;
		this.godSpeed = UnityEngine.Random.Range(0f, 100f);
	}

	private void Start()
	{
		this.dOSDrainer = new DOSDrainer();
	}

	private void OnDestroy()
	{
	}

	public WifiNetworkDefinition getCurrentWiFi()
	{
		return this.currentWifiNetwork;
	}

	public float GenereatePageLoadingTime(TWITCH_NET_SPEED nET_SPEED)
	{
		float num = (float)this.currentWifiNetwork.networkPower * 0.2f;
		int num2 = Mathf.Min((int)this.currentWifiNetwork.networkStrength + InventoryManager.GetWifiBoostLevel(), 3);
		num *= 1f - (float)num2 * 20f / 100f;
		num *= 1f - (float)InventoryManager.GetWifiBoostLevel() * 10f / 100f;
		switch (this.currentWifiNetwork.networkSignal)
		{
		case WIFI_SIGNAL_TYPE.W80211B:
			num *= 0.95f;
			break;
		case WIFI_SIGNAL_TYPE.W80211BP:
			num *= 0.9f;
			break;
		case WIFI_SIGNAL_TYPE.W80211G:
			num *= 0.85f;
			break;
		case WIFI_SIGNAL_TYPE.W80211N:
			num *= 0.8f;
			break;
		case WIFI_SIGNAL_TYPE.W80211AC:
			num *= 0.75f;
			break;
		}
		num = Mathf.Max(num, 0.5f);
		num += UnityEngine.Random.Range(0.25f, 0.75f);
		if (nET_SPEED != TWITCH_NET_SPEED.FAST)
		{
			if (nET_SPEED == TWITCH_NET_SPEED.SLOW)
			{
				num *= 3f;
			}
		}
		else
		{
			num /= 3f;
		}
		return num;
	}

	private void Update()
	{
		if (this.currentWifiNetwork != null && this.currentWifiNetwork.affectedByDosDrainer)
		{
			this.dOSDrainer.tryConsume();
		}
	}

	private void AddNewWiFi()
	{
		WifiNetworkDefinition wifiNetworkDefinition = new WifiNetworkDefinition();
		WifiNetworkDefinition wifiNetworkDefinition2 = new WifiNetworkDefinition();
		WifiNetworkDefinition wifiNetworkDefinition3 = new WifiNetworkDefinition();
		WifiNetworkDefinition wifiNetworkDefinition4 = new WifiNetworkDefinition();
		wifiNetworkDefinition.affectedByDosDrainer = false;
		wifiNetworkDefinition.id = 101;
		wifiNetworkDefinition.networkBSSID = "foobar";
		wifiNetworkDefinition.networkChannel = 6;
		wifiNetworkDefinition.networkCoolOffTime = 0f;
		wifiNetworkDefinition.networkInjectionAmount = 0;
		wifiNetworkDefinition.networkInjectionCoolOffTime = 0f;
		wifiNetworkDefinition.networkInjectionRandEnd = 0;
		wifiNetworkDefinition.networkInjectionRandStart = 0;
		wifiNetworkDefinition.networkIsOffline = false;
		wifiNetworkDefinition.networkMaxInjectionAmount = 0;
		wifiNetworkDefinition.networkName = "JackPott";
		wifiNetworkDefinition.networkOpenPort = 0;
		wifiNetworkDefinition.networkPassword = "foobar";
		wifiNetworkDefinition.networkPower = 55;
		wifiNetworkDefinition.networkRandPortEnd = 0;
		wifiNetworkDefinition.networkRandPortStart = 0;
		wifiNetworkDefinition.networkSecurity = WIFI_SECURITY.NONE;
		wifiNetworkDefinition.networkSignal = WIFI_SIGNAL_TYPE.W80211B;
		wifiNetworkDefinition.networkStrength = -1;
		wifiNetworkDefinition.networkTrackProbability = 0.55f;
		wifiNetworkDefinition.networkTrackRate = 555f;
		wifiNetworkDefinition2.affectedByDosDrainer = false;
		wifiNetworkDefinition2.id = 102;
		wifiNetworkDefinition2.networkBSSID = "foobar";
		wifiNetworkDefinition2.networkChannel = 6;
		wifiNetworkDefinition2.networkCoolOffTime = 83f;
		wifiNetworkDefinition2.networkInjectionAmount = 0;
		wifiNetworkDefinition2.networkInjectionCoolOffTime = 10f;
		wifiNetworkDefinition2.networkInjectionRandEnd = 400;
		wifiNetworkDefinition2.networkInjectionRandStart = 810;
		wifiNetworkDefinition2.networkIsOffline = false;
		wifiNetworkDefinition2.networkMaxInjectionAmount = 42;
		wifiNetworkDefinition2.networkName = "MADP1NG";
		wifiNetworkDefinition2.networkOpenPort = 0;
		wifiNetworkDefinition2.networkPassword = "foobar";
		wifiNetworkDefinition2.networkPower = 7;
		wifiNetworkDefinition2.networkRandPortEnd = 0;
		wifiNetworkDefinition2.networkRandPortStart = 0;
		wifiNetworkDefinition2.networkSecurity = WIFI_SECURITY.WPA;
		wifiNetworkDefinition2.networkSignal = WIFI_SIGNAL_TYPE.W80211N;
		wifiNetworkDefinition2.networkStrength = 0;
		wifiNetworkDefinition2.networkTrackProbability = 0.18f;
		wifiNetworkDefinition2.networkTrackRate = 584f;
		wifiNetworkDefinition3.affectedByDosDrainer = false;
		wifiNetworkDefinition3.id = 103;
		wifiNetworkDefinition3.networkBSSID = "fierce";
		wifiNetworkDefinition3.networkChannel = 12;
		wifiNetworkDefinition3.networkCoolOffTime = 0f;
		wifiNetworkDefinition3.networkInjectionAmount = 0;
		wifiNetworkDefinition3.networkInjectionCoolOffTime = 0f;
		wifiNetworkDefinition3.networkInjectionRandEnd = 0;
		wifiNetworkDefinition3.networkInjectionRandStart = 0;
		wifiNetworkDefinition3.networkIsOffline = false;
		wifiNetworkDefinition3.networkMaxInjectionAmount = 0;
		wifiNetworkDefinition3.networkName = "furrycon";
		wifiNetworkDefinition3.networkOpenPort = 0;
		wifiNetworkDefinition3.networkPassword = "fierce";
		wifiNetworkDefinition3.networkPower = 69;
		wifiNetworkDefinition3.networkRandPortEnd = 180;
		wifiNetworkDefinition3.networkRandPortStart = 420;
		wifiNetworkDefinition3.networkSecurity = WIFI_SECURITY.WEP;
		wifiNetworkDefinition3.networkSignal = WIFI_SIGNAL_TYPE.W80211G;
		wifiNetworkDefinition3.networkStrength = 3;
		wifiNetworkDefinition3.networkTrackProbability = 0.62f;
		wifiNetworkDefinition3.networkTrackRate = 621f;
		wifiNetworkDefinition4.affectedByDosDrainer = false;
		wifiNetworkDefinition4.id = 104;
		wifiNetworkDefinition4.networkBSSID = "amper";
		wifiNetworkDefinition4.networkChannel = 6;
		wifiNetworkDefinition4.networkCoolOffTime = 0f;
		wifiNetworkDefinition4.networkInjectionAmount = 0;
		wifiNetworkDefinition4.networkInjectionCoolOffTime = 0f;
		wifiNetworkDefinition4.networkInjectionRandEnd = 0;
		wifiNetworkDefinition4.networkInjectionRandStart = 0;
		wifiNetworkDefinition4.networkIsOffline = false;
		wifiNetworkDefinition4.networkMaxInjectionAmount = 0;
		wifiNetworkDefinition4.networkName = "TheProgrammingChair";
		wifiNetworkDefinition4.networkOpenPort = 0;
		wifiNetworkDefinition4.networkPassword = "amper";
		wifiNetworkDefinition4.networkPower = 54;
		wifiNetworkDefinition4.networkRandPortEnd = 110;
		wifiNetworkDefinition4.networkRandPortStart = 150;
		wifiNetworkDefinition4.networkSecurity = WIFI_SECURITY.WEP;
		wifiNetworkDefinition4.networkSignal = WIFI_SIGNAL_TYPE.W80211B;
		wifiNetworkDefinition4.networkStrength = 1;
		wifiNetworkDefinition4.networkTrackProbability = 0.16f;
		wifiNetworkDefinition4.networkTrackRate = 1454f;
		this.wifiHotSpots[0].myWifiNetworks.Add(wifiNetworkDefinition);
		this.wifiHotSpots[0].myWifiNetworks.Add(wifiNetworkDefinition2);
		this.wifiHotSpots[1].myWifiNetworks.Add(wifiNetworkDefinition3);
		this.wifiHotSpots[1].myWifiNetworks.Add(wifiNetworkDefinition4);
		this.wifiHotSpots[3].myWifiNetworks.Add(wifiNetworkDefinition4);
	}

	public WifiNetworkDefinition defaultWifiNetwork;

	public PasswordListDefinition PList;

	[SerializeField]
	private List<WifiHotspotObject> wifiHotSpots = new List<WifiHotspotObject>(6);

	[SerializeField]
	private WifiDongleBehaviour theWifiDongle;

	private WifiNetworkDefinition currentWifiNetwork;

	private WifiHotspotObject activeWifiHotSpot;

	private GameObject wifiIcon;

	private GameObject wifiMenu;

	private Dictionary<string, string> passwordList = new Dictionary<string, string>();

	private List<Sprite> wifiSprites = new List<Sprite>();

	private Vector2 wifiMenuPOS = Vector2.zero;

	private bool isOnline;

	private bool inWifiPlacementMode;

	private bool wifiMenuActive;

	private bool wifiMenuAniActive;

	private int myID;

	private WifiManagerData myData;

	private float godSpeed;

	private DOSDrainer dOSDrainer;

	public delegate void OnlineOfflineActions();

	public delegate void OnlineWithNetworkActions(WifiNetworkDefinition TheNetwork);

	public delegate void NewNetworksActions(List<WifiNetworkDefinition> NewNetworks);
}
