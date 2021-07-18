using System;
using UnityEngine;

public class TutorialProductBehaviour : TutorialStepper
{
	public bool WasPresented
	{
		get
		{
			return this.myData.WasPresneted;
		}
	}

	public void Begin()
	{
		this.myData.WasPresneted = true;
		DataManager.Save<TutorialProductData>(this.myData);
		TutorialVoiceCallBehaviour.Ins.WasPresentedEvents.Event -= this.Begin;
		TutorialVoiceCallBehaviour.Ins.HangUpEvents.Event += this.hardEnd;
		base.StartTutorial();
	}

	public void AddRemoteVPNSpawn()
	{
		SpawnToDeadDropTrigger.Ins.PlayerSpawningToDeadDropEvent.Event += this.playerSpawnedToDeadDrop;
	}

	public void PlayerDeclinedCall()
	{
		this.myData.WasPresneted = true;
		DataManager.Save<TutorialProductData>(this.myData);
	}

	private void hardEnd()
	{
		TutorialVoiceCallBehaviour.Ins.HangUpEvents.Event -= this.hardEnd;
		base.HardClear();
		TutorialManager.Ins.ClearOut();
		this.AddRemoteVPNSpawn();
	}

	private void playerSpawnedToDeadDrop()
	{
		SpawnToDeadDropTrigger.Ins.PlayerSpawningToDeadDropEvent.Event -= this.playerSpawnedToDeadDrop;
		GameManager.ManagerSlinger.ProductsManager.ShipProduct(this.remoteVPNDef);
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.myData = DataManager.Load<TutorialProductData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new TutorialProductData(this.myID);
			this.myData.WasPresneted = false;
		}
	}

	private void Awake()
	{
		this.myID = 7788;
		TutorialProductBehaviour.Ins = this;
		GameManager.StageManager.Stage += this.stageMe;
	}

	public static TutorialProductBehaviour Ins;

	[SerializeField]
	private ShadowMarketProductDefinition remoteVPNDef;

	private int myID = 7788;

	private TutorialProductData myData;
}
