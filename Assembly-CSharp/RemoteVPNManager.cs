using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RemoteVPNManager : MonoBehaviour
{
	public Vector3 CurrentRemoteVPNSpawnLocation
	{
		get
		{
			if (this.currentRemoteVPN != null)
			{
				return this.currentRemoteVPN.SpawnLocation;
			}
			return this.remoteVPNSpawnLocations[0];
		}
	}

	public Vector3 CurrentRemoteVPNSpawnRotation
	{
		get
		{
			if (this.currentRemoteVPN != null)
			{
				return this.currentRemoteVPN.SpawnRotation;
			}
			return this.remoteVPNSpawnRotations[0];
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event RemoteVPNManager.RemoteVPNVoidActions EnteredPlacementMode;

	public void ReturnRemoteVPN()
	{
		if (this.currentRemoteVPN != null)
		{
			StateManager.PlayerState = PLAYER_STATE.ROAMING;
			if (this.RemoteVPNWasReturned != null)
			{
				this.RemoteVPNWasReturned(this.currentRemoteVPN);
			}
			this.currentRemoteVPN.SpawnMe(this.currentRemoteVPN.SpawnLocation, this.currentRemoteVPN.SpawnRotation);
			this.currentRemoteVPN = null;
		}
	}

	private void productWasPickedUp(ShadowMarketProductDefinition TheProduct)
	{
		HARDWARE_PRODUCTS productID = TheProduct.productID;
		if (productID == HARDWARE_PRODUCTS.REMOTE_VPN)
		{
			this.spawnRemoteVPN();
		}
	}

	private void spawnRemoteVPN()
	{
		if (this.currentAvaibleRemoteVPNSpawnLocations.Count > 0 && this.currentAvaibleRemoteVPNSpawnRotations.Count > 0)
		{
			Vector3 setPosition = this.currentAvaibleRemoteVPNSpawnLocations.Pop();
			Vector3 setRotation = this.currentAvaibleRemoteVPNSpawnRotations.Pop();
			RemoteVPNObject remoteVPNObject = this.remoteVPNPool.Pop();
			remoteVPNObject.SpawnMe(setPosition, setRotation);
			this.currentlyOwnedVPNS.Add(remoteVPNObject);
		}
	}

	private void clearRemoteVPNS()
	{
		for (int i = 0; i < this.currentlyOwnedVPNS.Count; i++)
		{
			this.remoteVPNPool.Push(this.currentlyOwnedVPNS[i]);
		}
		foreach (RemoteVPNObject remoteVPNObject in this.remoteVPNPool)
		{
			remoteVPNObject.EnteredPlacementMode -= this.remoteVPNWasPickedUp;
			remoteVPNObject.IWasPlaced -= this.remoteVPNWasPlaced;
		}
		this.currentlyOwnedVPNS.Clear();
		this.remoteVPNPool.Clear();
	}

	private void remoteVPNWasPickedUp(RemoteVPNObject TheRemoteVPN)
	{
		this.currentRemoteVPN = TheRemoteVPN;
		StateManager.PlayerState = PLAYER_STATE.REMOTE_VPN_PLACEMENT;
		this.myData.CurrentlyPlacedRemoteVPNs.Remove(TheRemoteVPN.Transform.position.GetHashCode());
		DataManager.Save<RemoteVPNManagerData>(this.myData);
		if (this.EnteredPlacementMode != null)
		{
			this.EnteredPlacementMode(TheRemoteVPN);
		}
	}

	private void remoteVPNWasPlaced(RemoteVPNObject TheRemoteVPN)
	{
		this.currentRemoteVPN = null;
		StateManager.PlayerState = PLAYER_STATE.ROAMING;
		SerTrans value = SerTrans.Convert(TheRemoteVPN.Transform.position, TheRemoteVPN.Transform.rotation.eulerAngles);
		this.myData.CurrentlyPlacedRemoteVPNs.Add(TheRemoteVPN.Transform.position.GetHashCode(), value);
		DataManager.Save<RemoteVPNManagerData>(this.myData);
		if (this.RemoteVPNWasPlaced != null)
		{
			this.RemoteVPNWasPlaced(TheRemoteVPN);
		}
		int num = 0;
		for (int i = 0; i < this.currentlyOwnedVPNS.Count; i++)
		{
			if (this.currentlyOwnedVPNS[i].Placed)
			{
				num++;
			}
		}
		if (num >= 5)
		{
			SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.PAIDTOSIT);
		}
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<RemoteVPNManagerData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new RemoteVPNManagerData(this.myID);
			this.myData.CurrentVPNVolumesCurrencyData = new List<VPNCurrencyData>(GameManager.WorldManager.CurrentVPNVolumes.Count);
			this.myData.CurrentlyPlacedRemoteVPNs = new Dictionary<int, SerTrans>(5);
			for (int i = 0; i < GameManager.WorldManager.CurrentVPNVolumes.Count; i++)
			{
				int index = UnityEngine.Random.Range(0, this.currentAvaibleVPNCurrencies.Count);
				VPNCurrencyData item = new VPNCurrencyData(this.currentAvaibleVPNCurrencies[index].GenerateTime, this.currentAvaibleVPNCurrencies[index].GenerateDOSCoinValue);
				this.currentAvaibleVPNCurrencies.RemoveAt(index);
				this.myData.CurrentVPNVolumesCurrencyData.Add(item);
			}
		}
		for (int j = 0; j < GameManager.WorldManager.CurrentVPNVolumes.Count; j++)
		{
			GameManager.WorldManager.CurrentVPNVolumes[j].MyCurrency = this.myData.CurrentVPNVolumesCurrencyData[j];
		}
		DataManager.Save<RemoteVPNManagerData>(this.myData);
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void gameLive()
	{
		int num = 0;
		foreach (KeyValuePair<int, SerTrans> keyValuePair in this.myData.CurrentlyPlacedRemoteVPNs)
		{
			if (this.currentlyOwnedVPNS[num] != null)
			{
				this.currentlyOwnedVPNS[num].SetPlaceMe(keyValuePair.Value);
				if (this.RemoteVPNWasPlaced != null)
				{
					this.RemoteVPNWasPlaced(this.currentlyOwnedVPNS[num]);
				}
			}
			num++;
		}
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
	}

	private void Awake()
	{
		this.myID = base.transform.position.GetHashCode();
		GameManager.ManagerSlinger.RemoteVPNManager = this;
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.TheGameIsLive += this.gameLive;
		GameManager.ManagerSlinger.ProductsManager.ShadowMarketProductWasActivated.Event += this.productWasPickedUp;
		this.remoteVPNPool = new PooledStack<RemoteVPNObject>(delegate()
		{
			RemoteVPNObject component = UnityEngine.Object.Instantiate<GameObject>(this.remoteVPNObject, this.remoteVPNParent).GetComponent<RemoteVPNObject>();
			component.SoftBuild();
			component.EnteredPlacementMode += this.remoteVPNWasPickedUp;
			component.IWasPlaced += this.remoteVPNWasPlaced;
			return component;
		}, this.REMOTE_VPN_POOL_COUNT);
		for (int i = this.remoteVPNSpawnLocations.Length - 1; i >= 0; i--)
		{
			this.currentAvaibleRemoteVPNSpawnLocations.Push(this.remoteVPNSpawnLocations[i]);
		}
		for (int j = this.remoteVPNSpawnRotations.Length - 1; j >= 0; j--)
		{
			this.currentAvaibleRemoteVPNSpawnRotations.Push(this.remoteVPNSpawnRotations[j]);
		}
		for (int k = 0; k < this.vpnCurrencies.Length; k++)
		{
			if (this.vpnCurrencies[k].GenerateTime == 163f)
			{
				this.vpnCurrencies[k].GenerateDOSCoinValue = 5.32f;
				this.vpnCurrencies[k].GenerateTime = 132f;
			}
			if (ModsManager.EasyModeActive)
			{
				this.vpnCurrencies[k].GenerateDOSCoinValue += 1.83f;
				this.vpnCurrencies[k].GenerateTime -= 9f;
			}
			this.currentAvaibleVPNCurrencies.Add(this.vpnCurrencies[k]);
		}
	}

	private void OnDestroy()
	{
		this.clearRemoteVPNS();
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event RemoteVPNManager.RemoteVPNVoidActions RemoteVPNWasReturned;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event RemoteVPNManager.RemoteVPNVoidActions RemoteVPNWasPlaced;

	[SerializeField]
	private int REMOTE_VPN_POOL_COUNT = 5;

	[SerializeField]
	private GameObject remoteVPNObject;

	[SerializeField]
	private Transform remoteVPNParent;

	[SerializeField]
	private Vector3[] remoteVPNSpawnLocations = new Vector3[0];

	[SerializeField]
	private Vector3[] remoteVPNSpawnRotations = new Vector3[0];

	[SerializeField]
	private VPNCurrencyDefinition[] vpnCurrencies = new VPNCurrencyDefinition[0];

	private PooledStack<RemoteVPNObject> remoteVPNPool;

	private List<RemoteVPNObject> currentlyOwnedVPNS = new List<RemoteVPNObject>(5);

	private List<VPNCurrencyDefinition> currentAvaibleVPNCurrencies = new List<VPNCurrencyDefinition>();

	private Stack<Vector3> currentAvaibleRemoteVPNSpawnLocations = new Stack<Vector3>(5);

	private Stack<Vector3> currentAvaibleRemoteVPNSpawnRotations = new Stack<Vector3>(5);

	private RemoteVPNObject currentRemoteVPN;

	private int myID;

	private RemoteVPNManagerData myData;

	private bool easyModeTweaked;

	public delegate void RemoteVPNVoidActions(RemoteVPNObject TheRemoteVPN);
}
