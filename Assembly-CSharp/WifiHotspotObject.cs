using System;
using System.Collections.Generic;
using UnityEngine;

public class WifiHotspotObject : MonoBehaviour
{
	public void ActivateMe()
	{
		this.HighlightBase.enabled = true;
		this.Trigger.SetActive(true);
	}

	public void DeactivateMe()
	{
		this.HighlightBase.enabled = false;
		this.Trigger.SetActive(false);
	}

	public void ShowPreview()
	{
		this.currentDonglePreview.enabled = true;
	}

	public void HidePreview()
	{
		this.currentDonglePreview.enabled = false;
	}

	public void PlaceDongleHere()
	{
		this.HidePreview();
		GameManager.ManagerSlinger.WifiManager.ExitWifiDonglePlacementMode(this);
	}

	public void RefreshPreviewDongle()
	{
		this.currentDonglePreview.enabled = false;
		switch (InventoryManager.WifiDongleLevel)
		{
		case WIFI_DONGLE_LEVEL.LEVEL1:
			this.currentDonglePreview = this.DonglePreviewLevel1;
			break;
		case WIFI_DONGLE_LEVEL.LEVEL2:
			this.currentDonglePreview = this.DonglePreviewLevel2;
			break;
		case WIFI_DONGLE_LEVEL.LEVEL3:
			this.currentDonglePreview = this.DonglePreviewLevel3;
			break;
		default:
			this.currentDonglePreview = this.DonglePreviewLevel1;
			break;
		}
	}

	public int GetWifiNetworkIndex(WifiNetworkDefinition TheNetwork)
	{
		int result = 0;
		for (int i = 0; i < this.myWifiNetworks.Count; i++)
		{
			if (this.myWifiNetworks[i] == TheNetwork)
			{
				result = i;
				i = this.myWifiNetworks.Count;
			}
		}
		return result;
	}

	public WifiNetworkDefinition GetWifiNetworkDefByIndex(int setIndex)
	{
		if (this.myWifiNetworks[setIndex] != null)
		{
			return this.myWifiNetworks[setIndex];
		}
		return null;
	}

	private void prepWifiNetworks()
	{
		List<string> list = new List<string>(GameManager.ManagerSlinger.WifiManager.PasswordList.Keys);
		for (int i = 0; i < this.myWifiNetworks.Count; i++)
		{
			WifiNetworkData wifiNetworkData = DataManager.Load<WifiNetworkData>(this.myWifiNetworks[i].networkName.GetHashCode());
			if (wifiNetworkData == null)
			{
				if (this.myWifiNetworks[i].networkName == "Hidden Network")
				{
					this.myWifiNetworks[i].networkStrength = -1;
					Debug.Log("Fixed array error in Hidden Network");
				}
				if (this.myWifiNetworks[i].networkName == "DonaldsWiFi")
				{
					this.myWifiNetworks[i].networkStrength = 0;
					Debug.Log("Fixed array error in DonaldsWiFi + Buffed DonaldsWiFi");
				}
				if (this.myWifiNetworks[i].networkName == "Big Dave Network" || this.myWifiNetworks[i].networkName == "mycci7471" || this.myWifiNetworks[i].networkName == "tedata")
				{
					this.myWifiNetworks[i].networkStrength = 0;
					Debug.Log("Buffed " + this.myWifiNetworks[i].networkName);
				}
				if (this.myWifiNetworks[i].networkName.ToLower() == "freewifinovirus")
				{
					this.myWifiNetworks[i].affectedByDosDrainer = true;
					Debug.Log("Infected FreeWiFiNoViruses");
				}
				if (ModsManager.EasyModeActive)
				{
					if (this.myWifiNetworks[i].networkMaxInjectionAmount <= 8)
					{
						this.myWifiNetworks[i].networkInjectionCoolOffTime = 1f;
					}
					else if (this.myWifiNetworks[i].networkMaxInjectionAmount > 8 && this.myWifiNetworks[i].networkMaxInjectionAmount <= 38)
					{
						this.myWifiNetworks[i].networkInjectionCoolOffTime = 2f;
					}
					else if (this.myWifiNetworks[i].networkMaxInjectionAmount > 38 && this.myWifiNetworks[i].networkMaxInjectionAmount <= 92)
					{
						this.myWifiNetworks[i].networkInjectionCoolOffTime = 3f;
					}
					else if (this.myWifiNetworks[i].networkMaxInjectionAmount > 92 && this.myWifiNetworks[i].networkMaxInjectionAmount <= 128)
					{
						this.myWifiNetworks[i].networkInjectionCoolOffTime = 4f;
					}
					else if (this.myWifiNetworks[i].networkMaxInjectionAmount > 128)
					{
						this.myWifiNetworks[i].networkInjectionCoolOffTime = 5f;
					}
				}
				wifiNetworkData = new WifiNetworkData(this.myWifiNetworks[i].networkName.GetHashCode());
				wifiNetworkData.NetworkBSSID = MagicSlinger.GenerateRandomHexCode(2, 5, ":");
				if (this.myWifiNetworks[i].networkSecurity > WIFI_SECURITY.NONE)
				{
					int index = UnityEngine.Random.Range(0, list.Count);
					wifiNetworkData.NetworkPassword = GameManager.ManagerSlinger.WifiManager.PasswordList[list[index]];
					list.RemoveAt(index);
				}
				else
				{
					wifiNetworkData.NetworkPassword = string.Empty;
				}
				if (this.myWifiNetworks[i].networkSecurity == WIFI_SECURITY.WEP)
				{
					wifiNetworkData.NetworkOpenPort = (short)UnityEngine.Random.Range((int)this.myWifiNetworks[i].networkRandPortStart, (int)this.myWifiNetworks[i].networkRandPortEnd);
				}
				else
				{
					wifiNetworkData.NetworkOpenPort = 0;
				}
				if (this.myWifiNetworks[i].networkSecurity == WIFI_SECURITY.WPA || this.myWifiNetworks[i].networkSecurity == WIFI_SECURITY.WPA2)
				{
					wifiNetworkData.NetworkInjectionAmount = (short)UnityEngine.Random.Range((int)this.myWifiNetworks[i].networkInjectionRandStart, (int)this.myWifiNetworks[i].networkInjectionRandEnd);
				}
			}
			this.myWifiNetworks[i].networkBSSID = wifiNetworkData.NetworkBSSID;
			this.myWifiNetworks[i].networkPassword = wifiNetworkData.NetworkPassword;
			this.myWifiNetworks[i].networkOpenPort = wifiNetworkData.NetworkOpenPort;
			this.myWifiNetworks[i].networkInjectionAmount = wifiNetworkData.NetworkInjectionAmount;
			DataManager.Save<WifiNetworkData>(wifiNetworkData);
		}
	}

	public void gameLive()
	{
		this.RefreshPreviewDongle();
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
	}

	private void stageMe()
	{
		this.prepWifiNetworks();
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		this.currentDonglePreview = this.DonglePreviewLevel1;
		this.HighlightBase.enabled = false;
		this.Trigger.SetActive(false);
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.TheGameIsLive += this.gameLive;
	}

	public Vector3 DonglePlacedPOS;

	public Vector3 DonglePlacedROT;

	public MeshRenderer DonglePreviewLevel1;

	public MeshRenderer DonglePreviewLevel2;

	public MeshRenderer DonglePreviewLevel3;

	public MeshRenderer HighlightBase;

	public GameObject Trigger;

	public List<WifiNetworkDefinition> myWifiNetworks;

	private MeshRenderer currentDonglePreview;
}
