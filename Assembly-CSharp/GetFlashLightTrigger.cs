using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class GetFlashLightTrigger : MonoBehaviour
{
	private void getFlashLight()
	{
		if (EnvironmentManager.PowerState == POWER_STATE.OFF)
		{
			SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.NIGHTVISION);
		}
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPickUp2);
		this.myInteractionHook.ForceLock = true;
		this.flashLightMesh.enabled = false;
		InventoryManager.OwnsFlashlight = true;
		this.myData.OwnsFlashLight = true;
		DataManager.Save<FlashLightData>(this.myData);
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<FlashLightData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new FlashLightData(this.myID);
			this.myData.OwnsFlashLight = false;
		}
		if (this.myData.OwnsFlashLight)
		{
			this.myInteractionHook.ForceLock = true;
			this.flashLightMesh.enabled = false;
			InventoryManager.OwnsFlashlight = true;
		}
		else
		{
			InventoryManager.OwnsFlashlight = false;
		}
		GameManager.StageManager.TheGameIsLive -= this.stageMe;
	}

	private void Awake()
	{
		this.myID = base.transform.position.GetHashCode();
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.LeftClickAction += this.getFlashLight;
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.getFlashLight;
	}

	[SerializeField]
	private MeshRenderer flashLightMesh;

	private InteractionHook myInteractionHook;

	private int myID;

	private FlashLightData myData;
}
