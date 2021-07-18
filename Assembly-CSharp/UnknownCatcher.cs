using System;
using UnityEngine;

public class UnknownCatcher : MonoBehaviour
{
	private void triggerDooom()
	{
		this.doomIsSet = true;
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = new Vector3(-28.10953f, 40.51757f, -6.304061f);
	}

	private void Awake()
	{
	}

	private void Update()
	{
		if (!this.doomIsSet)
		{
			if (!this.triggerSet)
			{
				if (StateManager.PlayerLocation == PLAYER_LOCATION.UNKNOWN)
				{
					this.triggerSet = true;
					this.timeStamp = Time.time;
					return;
				}
			}
			else if (Time.time - this.timeStamp >= 1f)
			{
				if (StateManager.PlayerLocation == PLAYER_LOCATION.UNKNOWN)
				{
					Debug.Log("PLAYER WAS IN UNKNOWN POSITION!!!");
					return;
				}
				this.triggerSet = false;
			}
		}
	}

	private bool triggerSet;

	private bool doomIsSet;

	private float timeStamp;
}
