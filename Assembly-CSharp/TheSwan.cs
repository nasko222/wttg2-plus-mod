using System;
using UnityEngine;

public class TheSwan
{
	public void ActivateTheSwan()
	{
		if (this.isActivatedBefore)
		{
			return;
		}
		GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.failsafe);
		this.isActivatedBefore = true;
		this._108 = (float)UnityEngine.Random.Range(108, 324);
		this.SwanClock = (ModsManager.Nightmare ? 0.5f : 1.5f);
		GameManager.TimeSlinger.FireTimer(18f, new Action(this.StartCountdown), 0);
		GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("swan", "THE CODE IS LOST: 4 8 15 16 23 42");
	}

	private void StartCountdown()
	{
		if (this.firstTIME)
		{
			GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.reset);
			this.firstTIME = false;
			skyBreakBehavior.SwanNumbers[0] = 4;
			skyBreakBehavior.SwanNumbers[1] = 8;
			skyBreakBehavior.SwanNumbers[2] = 15;
			skyBreakBehavior.SwanNumbers[3] = 16;
			skyBreakBehavior.SwanNumbers[4] = 23;
			skyBreakBehavior.SwanNumbers[5] = 42;
		}
		if (this.SwanMalfunction || !ComputerPowerHook.Ins.PowerOn || EnvironmentManager.PowerState == POWER_STATE.OFF || StateManager.BeingHacked)
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.StartCountdown), 0);
			return;
		}
		if (RouterBehaviour.Ins.Owned && RouterBehaviour.Ins.RouterIsActive)
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.StartCountdown), 0);
		}
		else
		{
			GameManager.TimeSlinger.FireTimer((this._108 >= 108f) ? 1f : this.SwanClock, new Action(this.StartCountdown), 0);
		}
		if (this._108 > 0f)
		{
			this._108 -= 1f;
			this.MakeSwanSound();
			return;
		}
		this.SwanMalfunction = true;
		this.SystemFailure();
	}

	private void MakeSwanSound()
	{
		if (this._108 < 10f && this._108 > 0f)
		{
			GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.alarm);
			GameManager.AudioSlinger.PlaySoundWithCustomDelay(CustomSoundLookUp.alarm, 0.5f);
			return;
		}
		if (this._108 >= 10f && this._108 <= 20f)
		{
			GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.alarm);
			return;
		}
		if (this._108 > 20f && this._108 <= 60f)
		{
			GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.beep);
		}
	}

	private void SystemFailure()
	{
		TheSwan.extOn = false;
		this._108 = (float)UnityEngine.Random.Range(108, 324);
		skyBreakBehavior.SwanNumbers[0] = 4;
		skyBreakBehavior.SwanNumbers[1] = 8;
		skyBreakBehavior.SwanNumbers[2] = 15;
		skyBreakBehavior.SwanNumbers[3] = 16;
		skyBreakBehavior.SwanNumbers[4] = 23;
		skyBreakBehavior.SwanNumbers[5] = 42;
		if (this.SwanClock >= 0.75f)
		{
			this.SwanClock -= 0.25f;
		}
		if (ModsManager.Nightmare)
		{
			this.SwanClock = 0.5f;
		}
		if (this.skyBreak != null)
		{
			this.skyBreak.CauseSystemFailure();
		}
		GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.systemFailure);
		this.TakeSwanDOSB4();
		GameManager.TimeSlinger.FireTimer(11f, delegate()
		{
			this.TakeSwanDOS();
			this.SwanMalfunction = false;
		}, 0);
	}

	public void SwanReset()
	{
		if (this._108 > 60f)
		{
			return;
		}
		GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.reset);
		if (RouterBehaviour.Ins.Owned && RouterBehaviour.Ins.RouterIsActive)
		{
			this._108 = (float)UnityEngine.Random.Range(540, 864);
		}
		else
		{
			this._108 = (float)UnityEngine.Random.Range(108, 324);
		}
		CurrencyManager.AddCurrency(UnityEngine.Random.Range(2.5f * this.SwanClock * this.SwanClock, 3.5f * this.SwanClock * this.SwanClock));
		TheSwan.extOn = false;
	}

	private void TakeSwanDOS()
	{
		GameManager.BehaviourManager.NotesBehaviour.ClearNotes();
		if (ComputerPowerHook.Ins.PowerOn)
		{
			ComputerPowerHook.Ins.ShutDownComputerInsantly();
		}
	}

	public bool SwanError
	{
		get
		{
			return this.SwanMalfunction;
		}
	}

	public void TakeSwanDOSB4()
	{
		GameManager.BehaviourManager.NotesBehaviour.ClearNotes();
		double num = (double)CurrencyManager.CurrentCurrency;
		float num2 = UnityEngine.Random.Range(0.5f, 0.9f);
		if (ModsManager.Nightmare)
		{
			CurrencyManager.RemoveCurrency(CurrencyManager.CurrentCurrency);
			return;
		}
		CurrencyManager.RemoveCurrency((float)Math.Round(num * (double)num2, 3));
	}

	public string TheSwanDebug
	{
		get
		{
			if (!this.isActivatedBefore)
			{
				return 0.ToString();
			}
			if (this._108 > 0f)
			{
				return this._108.ToString();
			}
			return 0.ToString();
		}
	}

	public bool isActivatedBefore;

	private bool SwanMalfunction;

	public float _108 = 108f;

	public skyBreakBehavior skyBreak;

	private bool firstTIME = true;

	private float SwanClock;

	public static bool extOn;

	public static string extCode;
}
