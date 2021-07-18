using System;
using System.Diagnostics;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PauseManager
{
	public bool Paused
	{
		get
		{
			return this.paused;
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PauseManager.PauseEvents GamePaused;

	public static void LockPause()
	{
		PauseManager._lockPause = true;
	}

	public static void UnLockPause()
	{
		PauseManager._lockPause = false;
	}

	public void Update()
	{
		if (!PauseManager._lockPause && CrossPlatformInputManager.GetButtonDown("Cancel"))
		{
			if (this.paused)
			{
				this.paused = false;
				StateManager.GameState = GAME_STATE.LIVE;
				if (this.GameUnPaused != null)
				{
					this.GameUnPaused();
				}
				Time.timeScale = 1f;
			}
			else
			{
				this.paused = true;
				StateManager.GameState = GAME_STATE.PAUSED;
				if (this.GamePaused != null)
				{
					this.GamePaused();
				}
				Time.timeScale = 0f;
			}
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PauseManager.PauseEvents GameUnPaused;

	private bool paused;

	private static bool _lockPause;

	public delegate void PauseEvents();
}
