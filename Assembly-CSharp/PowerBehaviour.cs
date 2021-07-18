using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class PowerBehaviour : MonoBehaviour
{
	public AudioHubObject PowerOutageHub
	{
		get
		{
			return this.powerOutageHub;
		}
	}

	public void SwitchPowerOn()
	{
		this.generateFireWindow();
		this.breakerBoxHub.PlaySound(this.breakerSwitchOnSFX);
		this.powerOutageHub.PlaySound(this.powerOnSFX);
		this.switchOnTween.Restart(true, -1f);
		this.PowerOnEvent.Execute();
		this.powerOn();
	}

	public void ForcePowerOff()
	{
		this.powerOff(true);
	}

	public void ForcePowerOn()
	{
		this.powerOn();
	}

	public void ResetPowerTripTime()
	{
		this.fireWindow = UnityEngine.Random.Range(this.fireWindowMin, this.fireWindowMax);
		this.fireWindowTimeStamp = Time.time;
		this.fireWindowActive = true;
	}

	private void powerOff(bool PlaySound = true)
	{
		if (PlaySound)
		{
			this.powerOutageHub.PlaySound(this.powerOutSFX);
			this.breakerBoxHub.PlaySound(this.breakerSwitchOffSFX);
		}
		switchToComputerController.Ins.Lock();
		EnvironmentManager.PowerState = POWER_STATE.OFF;
		this.switchOffTween.Restart(true, -1f);
		this.PowerOffEvent.Execute();
		if (StateManager.PlayerState == PLAYER_STATE.COMPUTER)
		{
			ControllerManager.Get<computerController>(GAME_CONTROLLER.COMPUTER).LeaveMe();
		}
		for (int i = 0; i < this.Lights.Length; i++)
		{
			this.Lights[i].ForceOff();
		}
		for (int j = 0; j < this.HardEmissiveLights.Length; j++)
		{
			this.HardEmissiveLights[j].DisableKeyword("_EMISSION");
		}
		for (int k = 0; k < this.TriggerColliders.Length; k++)
		{
			this.TriggerColliders[k].ForceLock = true;
		}
		this.computerMaterial.DisableKeyword("_EMISSION");
		this.ComputerScreen.enabled = false;
		this.myData.LightsAreOff = true;
		DataManager.Save<PowerBehaviourData>(this.myData);
	}

	private void powerOn()
	{
		EnvironmentManager.PowerState = POWER_STATE.ON;
		for (int i = 0; i < this.Lights.Length; i++)
		{
			this.Lights[i].ReturnFromForceOff();
		}
		for (int j = 0; j < this.HardEmissiveLights.Length; j++)
		{
			this.HardEmissiveLights[j].EnableKeyword("_EMISSION");
		}
		for (int k = 0; k < this.TriggerColliders.Length; k++)
		{
			this.TriggerColliders[k].ForceLock = false;
		}
		if (ComputerPowerHook.Ins.PowerOn)
		{
			this.computerMaterial.EnableKeyword("_EMISSION");
			this.ComputerScreen.enabled = true;
			switchToComputerController.Ins.UnLock();
		}
		this.myData.LightsAreOff = false;
		DataManager.Save<PowerBehaviourData>(this.myData);
	}

	private void generateFireWindow()
	{
		this.fireWindow = UnityEngine.Random.Range(this.fireWindowMin, this.fireWindowMax);
		if (DataManager.LeetMode)
		{
			this.fireWindow *= 0.4f;
		}
		this.fireWindowTimeStamp = Time.time;
		this.fireWindowActive = true;
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<PowerBehaviourData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new PowerBehaviourData(this.myID);
			this.myData.LightsAreOff = false;
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void gameLive()
	{
		if (this.myData.LightsAreOff)
		{
			this.powerOff(false);
		}
		else
		{
			for (int i = 0; i < this.HardEmissiveLights.Length; i++)
			{
				this.HardEmissiveLights[i].EnableKeyword("_EMISSION");
			}
		}
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
	}

	private void threatsActivated()
	{
		GameManager.StageManager.ThreatsNowActivated -= this.threatsActivated;
		this.generateFireWindow();
	}

	private void Awake()
	{
		this.myID = base.transform.position.GetHashCode();
		EnvironmentManager.PowerBehaviour = this;
		this.switchOnTween = DOTween.To(() => this.SwitchOffPOS, delegate(Vector3 x)
		{
			this.BreakerSwitchTransform.transform.localPosition = x;
		}, this.SwitchOnPOS, 0.3f).SetEase(Ease.InCirc);
		this.switchOnTween.Pause<Tweener>();
		this.switchOnTween.SetAutoKill(false);
		this.switchOffTween = DOTween.To(() => this.SwitchOnPOS, delegate(Vector3 x)
		{
			this.BreakerSwitchTransform.transform.localPosition = x;
		}, this.SwitchOffPOS, 0.15f).SetEase(Ease.Linear);
		this.switchOffTween.Pause<Tweener>();
		this.switchOffTween.SetAutoKill(false);
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.TheGameIsLive += this.gameLive;
		GameManager.StageManager.ThreatsNowActivated += this.threatsActivated;
	}

	private void Update()
	{
		if (this.fireWindowActive && Time.time - this.fireWindowTimeStamp >= this.fireWindow)
		{
			this.fireWindowActive = false;
			if (!this.LockedOut && !StateManager.BeingHacked)
			{
				this.powerOff(true);
			}
			else
			{
				this.fireWindow = 30f;
				this.fireWindowTimeStamp = Time.time;
				this.fireWindowActive = true;
			}
		}
	}

	public void ForceTwitchPowerOff()
	{
		if (!this.LockedOut && !StateManager.BeingHacked && EnvironmentManager.PowerState == POWER_STATE.ON)
		{
			this.powerOff(true);
			this.ResetPowerTripTime();
			return;
		}
		GameManager.TimeSlinger.FireTimer(30f, new Action(this.ForceTwitchPowerOff), 0);
	}

	public void ForceSwanPowerOff()
	{
		if (!this.LockedOut && !StateManager.BeingHacked && EnvironmentManager.PowerState == POWER_STATE.ON)
		{
			this.powerOff(false);
			return;
		}
		this.ResetPowerTripTime();
	}

	public bool LockedOut;

	public CustomEvent PowerOffEvent = new CustomEvent(2);

	public CustomEvent PowerOnEvent = new CustomEvent(2);

	[SerializeField]
	private float fireWindowMin = 60f;

	[SerializeField]
	private float fireWindowMax = 120f;

	[SerializeField]
	private InteractiveLight[] Lights;

	[SerializeField]
	private AudioHubObject powerOutageHub;

	[SerializeField]
	private AudioHubObject breakerBoxHub;

	[SerializeField]
	private AudioFileDefinition powerOutSFX;

	[SerializeField]
	private AudioFileDefinition powerOnSFX;

	[SerializeField]
	private AudioFileDefinition breakerSwitchOffSFX;

	[SerializeField]
	private AudioFileDefinition breakerSwitchOnSFX;

	[SerializeField]
	private Material computerMaterial;

	[SerializeField]
	private Material[] HardEmissiveLights;

	[SerializeField]
	private InteractionHook[] TriggerColliders;

	[SerializeField]
	private MeshRenderer ComputerScreen;

	[SerializeField]
	private Transform BreakerSwitchTransform;

	[SerializeField]
	private Vector3 SwitchOnPOS;

	[SerializeField]
	private Vector3 SwitchOffPOS;

	private Tweener switchOnTween;

	private Tweener switchOffTween;

	private PowerBehaviourData myData;

	private int myID;

	private float fireWindow;

	private float fireWindowTimeStamp;

	private bool fireWindowActive;
}
