using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class SteamSlinger : MonoBehaviour
{
	public void UnlockSteamAchievement(STEAM_ACHIEVEMENT TheAchievement)
	{
		Debug.Log("Placeholder Discord Achievement " + TheAchievement.ToString());
	}

	public void ClearAchievements()
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.ResetAllStats(true);
		}
	}

	public void ClearReset()
	{
		this.tutDocs.Clear();
		this.loreDocs.Clear();
		this.zerodayProducts.Clear();
		this.shadowMarketProducts.Clear();
		this.crackedWifiNetworks.Clear();
		this.tutDocs = new Dictionary<int, bool>(8);
		this.loreDocs = new Dictionary<int, bool>(25);
		this.zerodayProducts = new Dictionary<int, bool>(10);
		this.shadowMarketProducts = new Dictionary<int, bool>(10);
		this.crackedWifiNetworks = new Dictionary<int, bool>(50);
	}

	public void PlayerDeclinedStartCall()
	{
		this.ghostingA = true;
	}

	public void PlayerDeclinedProductCall()
	{
		this.ghostingB = true;
		if (this.ghostingA && this.ghostingB)
		{
			this.UnlockSteamAchievement(STEAM_ACHIEVEMENT.GHOSTING);
		}
	}

	public void AddTutDoc(int HashCode)
	{
		this.tutDocs.Add(HashCode, false);
	}

	public void ReadTutDoc(int HashCode)
	{
		if (this.tutDocs.ContainsKey(HashCode))
		{
			this.tutDocs[HashCode] = true;
		}
		bool flag = true;
		foreach (KeyValuePair<int, bool> keyValuePair in this.tutDocs)
		{
			if (!keyValuePair.Value)
			{
				flag = false;
			}
		}
		if (flag)
		{
			this.UnlockSteamAchievement(STEAM_ACHIEVEMENT.KNOWLEDGEISPOWER);
		}
	}

	public void AddLoreDoc(int HashCode)
	{
		this.loreDocs.Add(HashCode, false);
	}

	public void InspectLoreDoc(int HashCode)
	{
		if (this.loreDocs.ContainsKey(HashCode))
		{
			this.loreDocs[HashCode] = true;
		}
		bool flag = true;
		foreach (KeyValuePair<int, bool> keyValuePair in this.loreDocs)
		{
			if (!keyValuePair.Value)
			{
				flag = false;
			}
		}
		if (flag)
		{
			this.UnlockSteamAchievement(STEAM_ACHIEVEMENT.LOREJUNKY);
		}
	}

	public void CheckStalkerURL(string theURL)
	{
		if (theURL == "http://www.twitter.com/thewebpro")
		{
			this.stalkerA = true;
		}
		if (theURL == "http://www.youtube.com/c/ReflectStudios")
		{
			this.stalkerB = true;
		}
		if (this.stalkerA && this.stalkerB)
		{
			this.UnlockSteamAchievement(STEAM_ACHIEVEMENT.STALKER);
		}
	}

	public void PlayerLostZone()
	{
		this.zoneWonCount = 0;
		this.zoneLostCount++;
		if (this.zoneLostCount >= 20)
		{
			this.UnlockSteamAchievement(STEAM_ACHIEVEMENT.BUTTERFINGERS);
		}
	}

	public void PlayerBeatZone()
	{
		this.zoneLostCount = 0;
		this.zoneWonCount++;
		if (this.zoneWonCount >= 20)
		{
			this.UnlockSteamAchievement(STEAM_ACHIEVEMENT.CLICKMASTER);
		}
	}

	public void AddZeroDayProduct(int HashCode)
	{
		this.zerodayProducts.Add(HashCode, false);
	}

	public void ActivateZeroDayProduct(int HashCode)
	{
		if (this.zerodayProducts.ContainsKey(HashCode))
		{
			this.zerodayProducts[HashCode] = true;
		}
		bool flag = true;
		foreach (KeyValuePair<int, bool> keyValuePair in this.zerodayProducts)
		{
			if (!keyValuePair.Value)
			{
				flag = false;
			}
		}
		if (flag)
		{
			this.UnlockSteamAchievement(STEAM_ACHIEVEMENT.DIGITALCONSUMER);
		}
		this.ShoppingSpreeCheck();
	}

	public void AddShadowMarketProduct(int HashCode)
	{
		this.shadowMarketProducts.Add(HashCode, false);
	}

	public void ActivateShadowMarketProduct(int HashCode)
	{
		if (this.shadowMarketProducts.ContainsKey(HashCode))
		{
			this.shadowMarketProducts[HashCode] = true;
		}
		bool flag = true;
		foreach (KeyValuePair<int, bool> keyValuePair in this.shadowMarketProducts)
		{
			if (!keyValuePair.Value)
			{
				flag = false;
			}
		}
		if (flag)
		{
			this.UnlockSteamAchievement(STEAM_ACHIEVEMENT.SHIPPINGHANDLER);
		}
		this.ShoppingSpreeCheck();
	}

	public void ShoppingSpreeCheck()
	{
		bool flag = true;
		foreach (KeyValuePair<int, bool> keyValuePair in this.zerodayProducts)
		{
			if (!keyValuePair.Value)
			{
				flag = false;
			}
		}
		foreach (KeyValuePair<int, bool> keyValuePair2 in this.shadowMarketProducts)
		{
			if (!keyValuePair2.Value)
			{
				flag = false;
			}
		}
		if (flag)
		{
			this.UnlockSteamAchievement(STEAM_ACHIEVEMENT.SHOPPINGSPREE);
		}
	}

	public void AddWifiNetworks(List<WifiNetworkDefinition> TheNetworks)
	{
		for (int i = 0; i < TheNetworks.Count; i++)
		{
			if (TheNetworks[i].networkSecurity != WIFI_SECURITY.NONE && !this.crackedWifiNetworks.ContainsKey(TheNetworks[i].GetHashCode()))
			{
				this.crackedWifiNetworks.Add(TheNetworks[i].GetHashCode(), false);
			}
		}
	}

	public void CrackWifiNetwork(int HashCode)
	{
		if (this.crackedWifiNetworks.ContainsKey(HashCode))
		{
			this.crackedWifiNetworks[HashCode] = true;
		}
		bool flag = true;
		foreach (KeyValuePair<int, bool> keyValuePair in this.crackedWifiNetworks)
		{
			if (!keyValuePair.Value)
			{
				flag = false;
			}
		}
		if (flag)
		{
			this.UnlockSteamAchievement(STEAM_ACHIEVEMENT.INFILTRATOR);
		}
	}

	public void AddPurchasedProduct()
	{
		if (this.myData != null)
		{
			this.myData.ProductPickUpCount = this.myData.ProductPickUpCount + 1;
			DataManager.Save<SteamSlingerData>(this.myData);
		}
	}

	public void CheckForPro()
	{
		if (this.myData != null && this.myData.ProductPickUpCount <= 1)
		{
			this.UnlockSteamAchievement(STEAM_ACHIEVEMENT.THEPROFESSIONAL);
		}
	}

	private void Awake()
	{
		SteamSlinger.Ins = this;
	}

	private void Start()
	{
		this.myData = new SteamSlingerData(221445);
		if (this.myData == null)
		{
			this.myData = new SteamSlingerData(221445);
			this.myData.ProductPickUpCount = 0;
		}
	}

	private void Update()
	{
		SteamAPI.RunCallbacks();
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (!this.requestedCurrentStats)
		{
			if (!SteamManager.Initialized)
			{
				this.requestedCurrentStats = true;
				return;
			}
			bool flag = SteamUserStats.RequestCurrentStats();
			this.requestedCurrentStats = flag;
		}
		if (this.storeStats)
		{
			SteamAPI.RunCallbacks();
			bool flag2 = SteamUserStats.StoreStats();
			this.storeStats = !flag2;
		}
	}

	public static SteamSlinger Ins;

	private Dictionary<STEAM_ACHIEVEMENT, string> achievementLookUp = new Dictionary<STEAM_ACHIEVEMENT, string>
	{
		{
			STEAM_ACHIEVEMENT.INFILTRATOR,
			"WTTG2_INFILTRATOR"
		},
		{
			STEAM_ACHIEVEMENT.SHOPPINGSPREE,
			"WTTG2_SHOPPINGSPREE"
		},
		{
			STEAM_ACHIEVEMENT.DIGITALCONSUMER,
			"WTTG2_DIGITALCONSUMER"
		},
		{
			STEAM_ACHIEVEMENT.SHIPPINGHANDLER,
			"WTTG2_SHIPPINGHANDLER"
		},
		{
			STEAM_ACHIEVEMENT.NIGHTVISION,
			"WTTG2_NIGHTVISION"
		},
		{
			STEAM_ACHIEVEMENT.DOUBLEUP,
			"WTTG2_DOUBLEUP"
		},
		{
			STEAM_ACHIEVEMENT.HOLDTHEDOOR,
			"WTTG2_HOLDTHEDOOR"
		},
		{
			STEAM_ACHIEVEMENT.LOREJUNKY,
			"WTTG2_LOREJUNKY"
		},
		{
			STEAM_ACHIEVEMENT.CLICKMASTER,
			"WTTG2_CLICKMASTER"
		},
		{
			STEAM_ACHIEVEMENT.BUTTERFINGERS,
			"WTTG2_BUTTERFINGERS"
		},
		{
			STEAM_ACHIEVEMENT.GHOSTING,
			"WTTG2_GHOSTING"
		},
		{
			STEAM_ACHIEVEMENT.KNOWLEDGEISPOWER,
			"WTTG2_KNOWLEDGEISPOWER"
		},
		{
			STEAM_ACHIEVEMENT.GOLDFISHMEMORY,
			"WTTG2_GOLDFISHMEMORY"
		},
		{
			STEAM_ACHIEVEMENT.STEELTRAP,
			"WTTG2_STEELTRAP"
		},
		{
			STEAM_ACHIEVEMENT.STACKOVERLOAD,
			"WTTG2_STACKOVERLOAD"
		},
		{
			STEAM_ACHIEVEMENT.POPPERPRO,
			"WTTG2_POPPERPRO"
		},
		{
			STEAM_ACHIEVEMENT.IDONTNODE,
			"WTTG2_IDONTNODE"
		},
		{
			STEAM_ACHIEVEMENT.HOOKUPMASTER,
			"WTTG2_HOOKUPMASTER"
		},
		{
			STEAM_ACHIEVEMENT.WHOSTHATLADY,
			"WTTG2_WHOSTHATLADY"
		},
		{
			STEAM_ACHIEVEMENT.PAIDTOSIT,
			"WTTG2_PAIDTOSIT"
		},
		{
			STEAM_ACHIEVEMENT.THEPROFESSIONAL,
			"WTTG2_THEPROFESSIONAL"
		},
		{
			STEAM_ACHIEVEMENT.PARANOID,
			"WTTG2_PARANOID"
		},
		{
			STEAM_ACHIEVEMENT.STALKER,
			"WTTG2_STALKER"
		},
		{
			STEAM_ACHIEVEMENT.GOOD_GUY_ADAM,
			"WTTG2_GOODGUYADAM"
		},
		{
			STEAM_ACHIEVEMENT.THEONEPERCENT,
			"WTTG2_THEONEPERCENT"
		},
		{
			STEAM_ACHIEVEMENT.DOLLMAKERPET,
			"WTTG2_DOLLMAKERPET"
		},
		{
			STEAM_ACHIEVEMENT.GOD_GAMER,
			"WTTG2_GODGAMER"
		}
	};

	private bool storeStats;

	private bool requestedCurrentStats;

	private bool ghostingA;

	private bool ghostingB;

	private bool stalkerA;

	private bool stalkerB;

	private int zoneWonCount;

	private int zoneLostCount;

	private Dictionary<int, bool> tutDocs = new Dictionary<int, bool>(8);

	private Dictionary<int, bool> loreDocs = new Dictionary<int, bool>(25);

	private Dictionary<int, bool> zerodayProducts = new Dictionary<int, bool>(10);

	private Dictionary<int, bool> shadowMarketProducts = new Dictionary<int, bool>(10);

	private Dictionary<int, bool> crackedWifiNetworks = new Dictionary<int, bool>(50);

	private SteamSlingerData myData;
}
