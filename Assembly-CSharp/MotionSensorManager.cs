using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class MotionSensorManager : MonoBehaviour
{
	public Vector3 CurrentMotionSensorSpawnLocation
	{
		get
		{
			if (this.currentActiveMotionSensor != null)
			{
				return this.currentActiveMotionSensor.SpawnLocation;
			}
			return this.motionSensorSpawnLocations[0];
		}
	}

	public Vector3 CurrentMotionSensorSpawRotat
	{
		get
		{
			if (this.currentActiveMotionSensor != null)
			{
				return this.currentActiveMotionSensor.SpawnRotation;
			}
			return this.motionSensorSpawnRotations[0];
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MotionSensorManager.MotionSensorVoidActions EnteredPlacementMode;

	public void ReturnMotionSensor()
	{
		if (this.currentActiveMotionSensor != null)
		{
			StateManager.PlayerState = PLAYER_STATE.ROAMING;
			if (this.MotionSensorWasReturned != null)
			{
				this.MotionSensorWasReturned(this.currentActiveMotionSensor);
			}
			this.currentActiveMotionSensor.SpawnMe(this.currentActiveMotionSensor.SpawnLocation, this.currentActiveMotionSensor.SpawnRotation);
			this.currentActiveMotionSensor = null;
		}
	}

	public void TriggerMotionSensorMenu()
	{
		if (!this.motionSensorMenuAniActive)
		{
			this.motionSensorMenuAniActive = true;
			if (this.motionSensorMenuActive)
			{
				this.motionSensorMenuActive = false;
				Vector2 endValue = new Vector2(LookUp.DesktopUI.MOTION_SENSOR_MENU.anchoredPosition.x, LookUp.DesktopUI.MOTION_SENSOR_MENU.sizeDelta.y);
				DOTween.To(() => LookUp.DesktopUI.MOTION_SENSOR_MENU.anchoredPosition, delegate(Vector2 x)
				{
					LookUp.DesktopUI.MOTION_SENSOR_MENU.anchoredPosition = x;
				}, endValue, 0.25f).SetEase(Ease.InQuad).OnComplete(delegate
				{
					this.motionSensorMenuAniActive = false;
				});
			}
			else
			{
				this.motionSensorMenuActive = true;
				Vector2 endValue2 = new Vector2(LookUp.DesktopUI.MOTION_SENSOR_MENU.anchoredPosition.x, -41f);
				DOTween.To(() => LookUp.DesktopUI.MOTION_SENSOR_MENU.anchoredPosition, delegate(Vector2 x)
				{
					LookUp.DesktopUI.MOTION_SENSOR_MENU.anchoredPosition = x;
				}, endValue2, 0.25f).SetEase(Ease.OutQuad).OnComplete(delegate
				{
					this.motionSensorMenuAniActive = false;
				});
			}
		}
	}

	private void spawnMotionSensor()
	{
		if (this.currentAvaibleMotionSensorSpawnLocations.Count > 0 && this.currentAvaibleMotionSensorSpawnRotations.Count > 0)
		{
			Vector3 setPosition = this.currentAvaibleMotionSensorSpawnLocations.Pop();
			Vector3 setRotation = this.currentAvaibleMotionSensorSpawnRotations.Pop();
			MotionSensorObject motionSensorObject = this.motionSensorPool.Pop();
			motionSensorObject.SpawnMe(setPosition, setRotation);
			this.currentlyOwnedMotionSensors.Add(motionSensorObject);
		}
	}

	private void productWasPickedUp(ShadowMarketProductDefinition TheProduct)
	{
		HARDWARE_PRODUCTS productID = TheProduct.productID;
		if (productID == HARDWARE_PRODUCTS.MOTION_SENSOR)
		{
			this.spawnMotionSensor();
		}
	}

	private void motionSensorWasPickedUp(MotionSensorObject TheMotionSensor)
	{
		this.currentActiveMotionSensor = TheMotionSensor;
		StateManager.PlayerState = PLAYER_STATE.MOTION_SENSOR_PLACEMENT;
		this.myData.CurrentlyPlacedMotionSensors.Remove(TheMotionSensor.Transform.position.GetHashCode());
		DataManager.Save<MotionSensorManagerData>(this.myData);
		if (this.EnteredPlacementMode != null)
		{
			this.EnteredPlacementMode(TheMotionSensor);
		}
	}

	private void motionSensorWasPlaced(MotionSensorObject TheMotionSensor)
	{
		this.currentActiveMotionSensor = null;
		StateManager.PlayerState = PLAYER_STATE.ROAMING;
		SerTrans setPosition = SerTrans.Convert(TheMotionSensor.Transform.position, TheMotionSensor.Transform.rotation.eulerAngles);
		this.myData.CurrentlyPlacedMotionSensors.Add(TheMotionSensor.transform.position.GetHashCode(), new MotionSensorPlacementData(TheMotionSensor.Location, setPosition));
		DataManager.Save<MotionSensorManagerData>(this.myData);
		if (this.MotionSensorPlaced != null)
		{
			this.MotionSensorPlaced(TheMotionSensor);
		}
		int num = 0;
		for (int i = 0; i < this.currentlyOwnedMotionSensors.Count; i++)
		{
			if (this.currentlyOwnedMotionSensors[i].Placed)
			{
				num++;
			}
		}
		if (num >= 4 && BlueWhisperManager.Ins.Owns)
		{
			SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.PARANOID);
		}
	}

	private void motionSensorWasTripped(MotionSensorObject TheMotionSensor)
	{
		if (!this.triggerWarningActive)
		{
			if (InventoryManager.OwnsMotionSensorAudioCue)
			{
				GameManager.AudioSlinger.PlaySound(this.motionSensorAlertSFX);
				BlueWhisperManager.Ins.ProcessSound(this.motionSensorAlertSFX);
			}
			LookUp.DesktopUI.MOTION_SENSOR_MENU_ICON_ACTIVE.alpha = 1f;
			LookUp.DesktopUI.MOTION_SENSOR_MENU_ICON_IDLE.alpha = 0f;
			this.triggerWarningTimeStamp = Time.time;
			this.triggerWarningActive = true;
		}
	}

	private void clearMotionSensors()
	{
		for (int i = 0; i < this.currentlyOwnedMotionSensors.Count; i++)
		{
			this.motionSensorPool.Push(this.currentlyOwnedMotionSensors[i]);
		}
		foreach (MotionSensorObject motionSensorObject in this.motionSensorPool)
		{
			motionSensorObject.EnteredPlacementMode -= this.motionSensorWasPickedUp;
			motionSensorObject.IWasPlaced -= this.motionSensorWasPlaced;
			motionSensorObject.IWasTripped -= this.motionSensorWasTripped;
		}
		this.currentlyOwnedMotionSensors.Clear();
		this.motionSensorPool.Clear();
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<MotionSensorManagerData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new MotionSensorManagerData(this.myID);
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void gameIsLive()
	{
		int num = 0;
		foreach (KeyValuePair<int, MotionSensorPlacementData> keyValuePair in this.myData.CurrentlyPlacedMotionSensors)
		{
			if (this.currentlyOwnedMotionSensors[num] != null)
			{
				this.currentlyOwnedMotionSensors[num].SetPlaceMe(keyValuePair.Value.LocationPoisition, (PLAYER_LOCATION)keyValuePair.Value.LocationName);
				this.MotionSensorPlaced(this.currentlyOwnedMotionSensors[num]);
			}
			num++;
		}
		GameManager.StageManager.TheGameIsLive -= this.gameIsLive;
	}

	private void Awake()
	{
		this.myID = base.transform.position.GetHashCode();
		GameManager.ManagerSlinger.MotionSensorManager = this;
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.TheGameIsLive += this.gameIsLive;
		GameManager.ManagerSlinger.ProductsManager.ShadowMarketProductWasActivated.Event += this.productWasPickedUp;
		this.motionSensorPool = new PooledStack<MotionSensorObject>(delegate()
		{
			MotionSensorObject component = UnityEngine.Object.Instantiate<GameObject>(this.motionSensorObject, this.motionSensorParent).GetComponent<MotionSensorObject>();
			component.EnteredPlacementMode += this.motionSensorWasPickedUp;
			component.IWasPlaced += this.motionSensorWasPlaced;
			component.IWasTripped += this.motionSensorWasTripped;
			component.SoftBuild();
			return component;
		}, this.MOTION_SENSOR_POOL_COUNT);
		for (int i = this.motionSensorSpawnLocations.Length - 1; i >= 0; i--)
		{
			this.currentAvaibleMotionSensorSpawnLocations.Push(this.motionSensorSpawnLocations[i]);
		}
		for (int j = this.motionSensorSpawnRotations.Length - 1; j >= 0; j--)
		{
			this.currentAvaibleMotionSensorSpawnRotations.Push(this.motionSensorSpawnRotations[j]);
		}
	}

	private void Update()
	{
		if (this.triggerWarningActive)
		{
			LookUp.DesktopUI.MOTION_SENSOR_MENU_ICON_ACTIVE.alpha = Mathf.Lerp(1f, 0f, (Time.time - this.triggerWarningTimeStamp) / this.triggerWarningFadeTime);
			if (Time.time - this.triggerWarningTimeStamp >= this.triggerWarningFadeTime)
			{
				this.triggerWarningActive = false;
				LookUp.DesktopUI.MOTION_SENSOR_MENU_ICON_ACTIVE.alpha = 0f;
				LookUp.DesktopUI.MOTION_SENSOR_MENU_ICON_IDLE.alpha = 1f;
			}
		}
	}

	private void OnDestroy()
	{
		this.clearMotionSensors();
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MotionSensorManager.MotionSensorVoidActions MotionSensorPlaced;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MotionSensorManager.MotionSensorVoidActions MotionSensorWasReturned;

	[SerializeField]
	private int MOTION_SENSOR_POOL_COUNT = 5;

	[SerializeField]
	private GameObject motionSensorObject;

	[SerializeField]
	private Transform motionSensorParent;

	[SerializeField]
	private Vector3[] motionSensorSpawnLocations = new Vector3[0];

	[SerializeField]
	private Vector3[] motionSensorSpawnRotations = new Vector3[0];

	[SerializeField]
	private float triggerWarningFadeTime = 0.1f;

	[SerializeField]
	private AudioFileDefinition motionSensorAlertSFX;

	private PooledStack<MotionSensorObject> motionSensorPool;

	private List<MotionSensorObject> currentlyOwnedMotionSensors = new List<MotionSensorObject>(5);

	private Stack<Vector3> currentAvaibleMotionSensorSpawnLocations = new Stack<Vector3>();

	private Stack<Vector3> currentAvaibleMotionSensorSpawnRotations = new Stack<Vector3>();

	private MotionSensorObject currentActiveMotionSensor;

	private bool motionSensorMenuAniActive;

	private bool motionSensorMenuActive;

	private bool triggerWarningActive;

	private float triggerWarningTimeStamp;

	private int myID;

	private MotionSensorManagerData myData;

	public delegate void MotionSensorVoidActions(MotionSensorObject TheMotionSensor);
}
