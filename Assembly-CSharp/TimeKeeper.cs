using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
	public string GetCurrentTimeString()
	{
		string text = this.hourArray[this.gameHour] + ":" + this.gameMin.ToString("D2");
		if (this.gameHour > 11)
		{
			text += " AM";
			if (this.gameHour < 16 && (this.gameMin == 0 || this.gameMin == 30) && UnityEngine.Random.Range(0, 100) <= 5)
			{
				GameManager.HackerManager.theSwan.ActivateTheSwan();
			}
		}
		else
		{
			text += " PM";
			if (ModsManager.Nightmare && UnityEngine.Random.Range(0, 1000) < 5 && GameManager.ManagerSlinger.WifiManager != null && GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() != null)
			{
				if (RouterBehaviour.Ins.Owned && RouterBehaviour.Ins.RouterIsActive)
				{
					return text;
				}
				GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().affectedByDosDrainer = true;
			}
		}
		return text;
	}

	public int GetCurrentGameHour()
	{
		return this.gameHour;
	}

	public int GetCurrentGameMin()
	{
		return this.gameMin;
	}

	private void prepHourArray()
	{
		this.hourArray.Add("12");
		this.hourArray.Add("1");
		this.hourArray.Add("2");
		this.hourArray.Add("3");
		this.hourArray.Add("4");
		this.hourArray.Add("5");
		this.hourArray.Add("6");
		this.hourArray.Add("7");
		this.hourArray.Add("8");
		this.hourArray.Add("9");
		this.hourArray.Add("10");
		this.hourArray.Add("11");
		this.hourArray.Add("12");
		this.hourArray.Add("1");
		this.hourArray.Add("2");
		this.hourArray.Add("3");
		this.hourArray.Add("4");
		this.hourArray.Add("5");
		this.hourArray.Add("6");
		this.hourArray.Add("7");
		this.hourArray.Add("8");
		this.hourArray.Add("9");
		this.hourArray.Add("10");
		this.hourArray.Add("11");
	}

	private void updateClock()
	{
		if (this.secondsToMin <= Time.time - this.curTimeStamp)
		{
			this.gameMin++;
			this.curTimeStamp = Time.time;
			this.updateAllClocks = true;
		}
		if (this.gameMin == 20 || this.gameMin == 40 || this.gameMin == 60)
		{
			DataManager.WriteData();
		}
		if (this.gameMin >= 60)
		{
			this.gameMin = 0;
			this.gameHour++;
		}
		if (this.gameHour >= 16)
		{
			DataManager.ClearGameData();
			UIManager.TriggerGameOver("TIMES UP!");
		}
		if (this.updateAllClocks)
		{
			this.updateAllClocks = false;
			this.myTimeData.GameHour = this.gameHour;
			this.myTimeData.GameMin = this.gameMin;
			DataManager.Save<TimeData>(this.myTimeData);
			this.UpdateClockEvents.Execute(this.GetCurrentTimeString());
		}
	}

	private void playerHitPause()
	{
		this.freezeTime = true;
	}

	private void playerHitUnPause()
	{
		this.freezeTime = false;
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.myTimeData = DataManager.Load<TimeData>(52085);
		if (this.myTimeData == null)
		{
			this.myTimeData = new TimeData(52085);
			this.myTimeData.GameHour = 10;
			this.myTimeData.GameMin = 0;
		}
		this.gameHour = this.myTimeData.GameHour;
		this.gameMin = this.myTimeData.GameMin;
		this.UpdateClockEvents.Execute(this.GetCurrentTimeString());
		this.freezeTime = false;
	}

	private void Awake()
	{
		GameManager.TimeKeeper = this;
		this.updateAllClocks = false;
		this.prepHourArray();
		this.curTimeStamp = Time.time;
		this.freezeTime = false;
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.PauseManager.GamePaused += this.playerHitPause;
		GameManager.PauseManager.GameUnPaused += this.playerHitUnPause;
		this.freezeTime = true;
	}

	private void Update()
	{
		if (!this.freezeTime)
		{
			this.updateClock();
		}
		this.secondsToMin = (float)TarotManager.TimeController;
	}

	private void OnDestroy()
	{
		GameManager.PauseManager.GamePaused -= this.playerHitPause;
		GameManager.PauseManager.GameUnPaused -= this.playerHitUnPause;
	}

	public CustomEvent<string> UpdateClockEvents = new CustomEvent<string>(3);

	[SerializeField]
	[Range(1f, 60f)]
	private float secondsToMin = 30f;

	private bool updateAllClocks;

	private bool freezeTime;

	private int gameHour;

	private int gameMin;

	private float curTimeStamp;

	private List<string> hourArray = new List<string>();

	private Dictionary<string, Action> currentClocks = new Dictionary<string, Action>();

	private TimeData myTimeData;
}
