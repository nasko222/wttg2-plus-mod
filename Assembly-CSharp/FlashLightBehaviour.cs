using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioHubObject))]
public class FlashLightBehaviour : MonoBehaviour
{
	public bool LightOn
	{
		get
		{
			return this.flashLightIsOn && !this.flashLightIsDead;
		}
	}

	public void TriggerFlashLight()
	{
		if (InventoryManager.OwnsFlashlight)
		{
			this.flashLightIsOn = !this.flashLightIsOn;
			this.flashLight.enabled = this.flashLightIsOn;
			if (this.flashLightIsOn)
			{
				this.myAudioHub.PlaySound(this.flashLightOnSFX);
			}
			else
			{
				this.myAudioHub.PlaySound(this.flashLightOffSFX);
			}
			if (this.flashLightIsOn && !this.flashLightIsDead)
			{
				this.FlashLightWentOn.Execute(true);
			}
			else
			{
				this.FlashLightWentOn.Execute(false);
			}
		}
	}

	public void LockOut()
	{
		this.lockedOut = true;
		this.flashLightIsOn = false;
		this.flashLight.enabled = false;
	}

	public void UnLock()
	{
		this.lockedOut = false;
	}

	private void saveData()
	{
		DataManager.Save<FlashLightBehData>(this.myFlashLightData);
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.myFlashLightData = DataManager.Load<FlashLightBehData>(78);
		if (this.myFlashLightData == null)
		{
			this.myFlashLightData = new FlashLightBehData(78);
			this.myFlashLightData.BatteryLifeUsage = 1f;
		}
		this.batteryLifeUsage = this.myFlashLightData.BatteryLifeUsage;
		base.InvokeRepeating("saveData", 0f, 30f);
	}

	private void Awake()
	{
		FlashLightBehaviour.Ins = this;
		this.flashLight = base.GetComponent<Light>();
		this.myAudioHub = base.GetComponent<AudioHubObject>();
		GameManager.StageManager.Stage += this.stageMe;
		this.maxBatteryLife = this.flashLightLifeInMins * 60f;
		this.flashCheckTimeStamp = Time.time;
		this.flashLight.intensity = this.defaultFlashLightBrightness;
	}

	private void Update()
	{
		if (!this.lockedOut && CrossPlatformInputManager.GetButtonDown("FlashLight"))
		{
			FlashLightBehaviour.Ins.TriggerFlashLight();
		}
		if (this.flashLightIsOn && !this.flashlightHuntMode)
		{
			this.HuntFlashLight(EnemyManager.State == ENEMY_STATE.CULT);
		}
	}

	private void OnDestroy()
	{
		base.CancelInvoke("saveData");
	}

	private void HuntFlashLight(bool enabled)
	{
		if (!enabled)
		{
			this.flashLight.enabled = this.flashLightIsOn;
			this.flashlightHuntMode = false;
			return;
		}
		this.flashlightHuntMode = true;
		GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(0.25f, 0.75f), new Action(this.ToggleHuntFlashLight), 0);
	}

	private void ToggleHuntFlashLight()
	{
		if (this.flashLightIsOn)
		{
			this.flashLight.enabled = !this.flashLight.enabled;
		}
		if (EnemyManager.State == ENEMY_STATE.CULT)
		{
			this.flashlightHuntMode = true;
		}
		else
		{
			this.flashLight.enabled = this.flashLightIsOn;
			this.flashlightHuntMode = false;
		}
		if (this.flashlightHuntMode)
		{
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(0.2f, 0.6f), new Action(this.ToggleHuntFlashLight), 0);
		}
	}

	public static FlashLightBehaviour Ins;

	public CustomEvent<bool> FlashLightWentOn = new CustomEvent<bool>(1);

	[SerializeField]
	private float defaultFlashLightBrightness;

	[SerializeField]
	private float flashLightLifeInMins = 1f;

	[SerializeField]
	private AudioFileDefinition flashLightOnSFX;

	[SerializeField]
	private AudioFileDefinition flashLightOffSFX;

	private AudioHubObject myAudioHub;

	private Light flashLight;

	private bool flashLightIsOn;

	private bool lockedOut;

	private bool flashLightIsDead;

	private float batteryLifeUsage;

	private float maxBatteryLife;

	private float flashCheckTimeStamp;

	private FlashLightBehData myFlashLightData;

	private bool flashlightHuntMode;
}
