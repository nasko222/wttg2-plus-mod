using System;
using UnityEngine;

public class VirusManager : MonoBehaviour
{
	public void AddVirus()
	{
		if (UnityEngine.Random.Range(0, 100) <= this.chanceOfGettingVirus)
		{
			if (this.virusCount <= 0)
			{
				this.virusTickTimeStamp = Time.time;
				this.hasVirus = true;
			}
			this.virusCount++;
			this.myData.HasVirus = true;
			this.myData.VirusCount = this.virusCount;
			DataManager.Save<VirusManagerData>(this.myData);
			DataManager.WriteData();
		}
	}

	public void ForceVirus()
	{
		if (this.virusCount <= 0)
		{
			this.virusTickTimeStamp = Time.time;
			this.hasVirus = true;
		}
		this.virusCount++;
		this.myData.HasVirus = true;
		this.myData.VirusCount = this.virusCount;
		DataManager.Save<VirusManagerData>(this.myData);
		DataManager.WriteData();
	}

	public void ClearVirus()
	{
		this.previousPowerLockState = EnvironmentManager.PowerBehaviour.LockedOut;
		this.previousOpenForHacksState = GameManager.HackerManager.OpenForHacks;
		EnvironmentManager.PowerBehaviour.LockedOut = true;
		GameManager.HackerManager.OpenForHacks = false;
		UIDialogManager.VWipeDialog.VWipeDialogWasPresented.Event += this.dialogWasPresented;
		UIDialogManager.VWipeDialog.Present();
	}

	private void triggerVirusEffects()
	{
		if (UnityEngine.Random.Range(0, 100) <= this.chanceOfLosingDOSCoin)
		{
			float setAMT = (float)this.virusCount * this.dosCoinDeductPerVirus;
			CurrencyManager.RemoveCurrency(setAMT);
		}
		if (UnityEngine.Random.Range(0, 100) <= this.chanceOfShutDown && !StateManager.BeingHacked)
		{
			ComputerPowerHook.Ins.ShutDownComputer();
		}
	}

	private void dialogWasPresented()
	{
		UIDialogManager.VWipeDialog.VWipeDialogWasPresented.Event -= this.dialogWasPresented;
		UIDialogManager.VWipeDialog.VWipeScanRemoveDone.Event += this.scanCompleted;
		UIDialogManager.VWipeDialog.PerformScanRemove(this.virusCount);
	}

	private void scanCompleted()
	{
		UIDialogManager.VWipeDialog.VWipeScanRemoveDone.Event -= this.scanCompleted;
		UIDialogManager.VWipeDialog.Dismiss();
		this.hasVirus = false;
		this.virusCount = 0;
		this.myData.HasVirus = false;
		this.myData.VirusCount = 0;
		DataManager.Save<VirusManagerData>(this.myData);
		DataManager.WriteData();
		GameManager.HackerManager.OpenForHacks = this.previousOpenForHacksState;
		EnvironmentManager.PowerBehaviour.LockedOut = this.previousPowerLockState;
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.myData = DataManager.Load<VirusManagerData>(608806);
		if (this.myData == null)
		{
			this.myData = new VirusManagerData(608806);
			this.myData.HasVirus = false;
			this.myData.VirusCount = 0;
		}
		this.hasVirus = this.myData.HasVirus;
		this.virusCount = this.myData.VirusCount;
		DataManager.Save<VirusManagerData>(this.myData);
	}

	private void Awake()
	{
		VirusManager.Ins = this;
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void Update()
	{
		if (ComputerPowerHook.Ins.PowerOn)
		{
			if (this.hasVirus && Time.time - this.virusTickTimeStamp >= this.virusTick)
			{
				this.virusTickTimeStamp = Time.time;
				this.triggerVirusEffects();
			}
		}
		else if (this.hasVirus)
		{
			this.virusTickTimeStamp = Time.time;
		}
	}

	public int getVirusCount
	{
		get
		{
			return this.virusCount;
		}
	}

	public static VirusManager Ins;

	[SerializeField]
	private float virusTick = 30f;

	[SerializeField]
	private float dosCoinDeductPerVirus = 0.5f;

	[SerializeField]
	private int chanceOfGettingVirus = 75;

	[SerializeField]
	private int chanceOfLosingDOSCoin = 55;

	[SerializeField]
	private int chanceOfShutDown = 25;

	private int virusCount;

	private bool hasVirus;

	private bool previousPowerLockState;

	private bool previousOpenForHacksState;

	private float virusTickTimeStamp;

	private VirusManagerData myData;
}
