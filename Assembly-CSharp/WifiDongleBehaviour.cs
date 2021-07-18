using System;
using UnityEngine;

public class WifiDongleBehaviour : MonoBehaviour
{
	public void PickupDongle()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPickUp1);
		this.currentActiveWifiDongle.enabled = false;
		GameManager.ManagerSlinger.WifiManager.EnterWifiDonglePlacementMode();
	}

	public void PlaceDongle(Vector3 newPOS, Vector3 newROT, bool PlaySound = true)
	{
		if (PlaySound)
		{
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPutDown1);
		}
		base.transform.localPosition = newPOS;
		base.transform.localRotation = Quaternion.Euler(newROT);
		this.currentActiveWifiDongle.enabled = true;
	}

	public void RefreshActiveWifiDongleLevel()
	{
		this.currentActiveWifiDongle.enabled = false;
		switch (InventoryManager.WifiDongleLevel)
		{
		case WIFI_DONGLE_LEVEL.LEVEL1:
			this.currentActiveWifiDongle = this.WifiDongleLevel1;
			break;
		case WIFI_DONGLE_LEVEL.LEVEL2:
			this.currentActiveWifiDongle = this.WifiDongleLevel2;
			break;
		case WIFI_DONGLE_LEVEL.LEVEL3:
			this.currentActiveWifiDongle = this.WifiDongleLevel3;
			break;
		default:
			this.currentActiveWifiDongle = this.WifiDongleLevel1;
			break;
		}
		this.currentActiveWifiDongle.enabled = true;
	}

	private void gameLive()
	{
		this.RefreshActiveWifiDongleLevel();
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.currentActiveWifiDongle = this.WifiDongleLevel1;
		this.myInteractionHook.LeftClickAction += this.PickupDongle;
		GameManager.StageManager.TheGameIsLive += this.gameLive;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.PickupDongle;
	}

	public MeshRenderer WifiDongleLevel1;

	public MeshRenderer WifiDongleLevel2;

	public MeshRenderer WifiDongleLevel3;

	private MeshRenderer currentActiveWifiDongle;

	private InteractionHook myInteractionHook;
}
