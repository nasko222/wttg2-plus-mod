using System;
using System.Diagnostics;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	public GAME_CONTROLLER DefaultController
	{
		get
		{
			return this.defaultController;
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event StageManager.StageManagerActions Stage;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event StageManager.LiveActions TheGameIsLive;

	public void ManuallyActivateThreats()
	{
		if (!this.threatsActivated)
		{
			base.CancelInvoke("updateTimeLeft");
			this.threatTimerActive = false;
			this.threatsActivated = true;
			this.myData.ThreatsActivated = true;
			this.myData.TimeLeft = 0f;
			DataManager.Save<StageManagerData>(this.myData);
			if (this.ThreatsNowActivated != null)
			{
				this.ThreatsNowActivated();
			}
		}
	}

	private void prepStageManager()
	{
		if (this.TheGameIsStageing != null)
		{
			this.TheGameIsStageing();
		}
		if (!this.DebugMode)
		{
			GameManager.WorldManager.WorldLoaded += this.stageLevel;
		}
		else
		{
			this.stageLevel();
		}
	}

	private void stageLevel()
	{
		if (!this.DebugMode)
		{
			GameManager.WorldManager.WorldLoaded -= this.stageLevel;
		}
		this.myData = DataManager.Load<StageManagerData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new StageManagerData(this.myID);
			this.myData.ThreatsActivated = false;
			this.myData.TimeLeft = 450f;
		}
		if (this.Stage != null)
		{
			this.Stage();
		}
		if (!this.DebugMode)
		{
			GameManager.TimeSlinger.FireTimer(this.stageTime, new Action(this.setGameLive), 0);
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.setGameLive), 0);
		}
	}

	private void setGameLive()
	{
		this.GameIsLive = true;
		if (this.TheGameIsLive != null)
		{
			this.TheGameIsLive();
		}
		PauseManager.UnLockPause();
		if (DataManager.LeetMode)
		{
			if (this.ThreatsNowActivated != null)
			{
				this.ThreatsNowActivated();
			}
			for (int i = 0; i < 8; i++)
			{
				GameManager.TheCloud.ForceKeyDiscover();
			}
		}
		else if (this.myData.ThreatsActivated)
		{
			if (this.ThreatsNowActivated != null)
			{
				this.ThreatsNowActivated();
			}
		}
		else
		{
			this.threatWindow = this.myData.TimeLeft;
			this.threatTimeStamp = Time.time;
			this.threatTimerActive = true;
			base.InvokeRepeating("updateTimeLeft", 0f, 10f);
		}
		GameManager.TheCloud.KeyDiscoveredEvent.Event += this.keyWasFound;
	}

	private void updateTimeLeft()
	{
		this.myData.TimeLeft = this.threatWindow - (Time.time - this.threatTimeStamp);
		DataManager.Save<StageManagerData>(this.myData);
	}

	private void keyWasFound()
	{
		GameManager.TheCloud.KeyDiscoveredEvent.Event -= this.keyWasFound;
		if (!this.threatsActivated)
		{
			base.CancelInvoke("updateTimeLeft");
			this.threatTimerActive = false;
			this.threatsActivated = true;
			this.myData.ThreatsActivated = true;
			this.myData.TimeLeft = 0f;
			DataManager.Save<StageManagerData>(this.myData);
			if (this.ThreatsNowActivated != null)
			{
				this.ThreatsNowActivated();
			}
		}
	}

	private void Awake()
	{
		this.myID = base.transform.position.GetHashCode();
		GameManager.StageManager = this;
		PauseManager.LockPause();
		StateManager.PlayerState = PLAYER_STATE.STAGING;
		if (this.DebugMode && this.LeetMode)
		{
			DataManager.LeetMode = true;
		}
	}

	private void Start()
	{
		this.prepStageManager();
	}

	private void Update()
	{
		if (this.threatTimerActive && Time.time - this.threatTimeStamp >= this.threatWindow)
		{
			base.CancelInvoke("updateTimeLeft");
			this.threatTimerActive = false;
			this.threatsActivated = true;
			this.myData.ThreatsActivated = true;
			this.myData.TimeLeft = 0f;
			DataManager.Save<StageManagerData>(this.myData);
			if (this.ThreatsNowActivated != null)
			{
				this.ThreatsNowActivated();
			}
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event StageManager.LiveActions TheGameIsStageing;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event StageManager.LiveActions ThreatsNowActivated;

	public bool DebugMode;

	public bool LeetMode;

	public bool GameIsLive;

	[SerializeField]
	private GAME_CONTROLLER defaultController;

	[SerializeField]
	private float stageTime = 0.5f;

	private int myID;

	private StageManagerData myData;

	private bool threatsActivated;

	private bool threatTimerActive;

	private float threatTimeStamp;

	private float threatWindow;

	public delegate void StageManagerActions();

	public delegate void LiveActions();
}
