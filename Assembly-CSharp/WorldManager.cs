using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
	public List<VPNVolume> CurrentVPNVolumes
	{
		get
		{
			return this.currentVPNVolumes;
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event WorldManager.WorldManagerActions WorldLoaded;

	public void StageGame()
	{
		if (!this.debugMode)
		{
			GameManager.TimeSlinger.FireTimer<int>(1.5f, new Action<int>(this.loadInWorld), this.worldIDs[this.worldIDIndex], 0);
		}
	}

	public void loadInWorld(int worldID)
	{
		base.StartCoroutine(this.loadWorld(worldID));
	}

	public void AddLocationVolume(LocationVolume SetVolume)
	{
		this.currentLocationVolumes.Add(SetVolume);
	}

	public void AddActiveLocationVolume(LocationVolume SetVolume)
	{
		if (!this.activeLocationVolumes.Contains(SetVolume))
		{
			this.activeLocationVolumes.Add(SetVolume);
		}
	}

	public void RemoveActiveLocationVolume(LocationVolume SetVolume)
	{
		this.activeLocationVolumes.Remove(SetVolume);
	}

	public void AddVPNVolume(VPNVolume SetVolume)
	{
		this.currentVPNVolumes.Add(SetVolume);
	}

	public void SetVPNValues(RemoteVPNObject TheRemoteVPN)
	{
		bool flag = false;
		for (int i = 0; i < this.currentVPNVolumes.Count; i++)
		{
			if (this.currentVPNVolumes[i].VPNInRange(TheRemoteVPN))
			{
				flag = true;
				i = this.currentVPNVolumes.Count;
			}
		}
		if (!flag)
		{
			TheRemoteVPN.SetCurrency(this.defaultVPNCurrency);
		}
	}

	public float GetVPNValues(Transform ThePosition)
	{
		float result = 2500f;
		for (int i = 0; i < this.currentVPNVolumes.Count; i++)
		{
			if (this.currentVPNVolumes[i].VPNRangeCheck(ThePosition, out result))
			{
				i = this.currentVPNVolumes.Count;
			}
		}
		return result;
	}

	private void checkPlayerLocation()
	{
		if (this.activeLocationVolumes.Count > 0)
		{
			StateManager.PlayerLocation = this.activeLocationVolumes[this.activeLocationVolumes.Count - 1].Location;
		}
		else
		{
			StateManager.PlayerLocation = PLAYER_LOCATION.UNKNOWN;
		}
	}

	private IEnumerator loadWorld(int worldID)
	{
		AsyncOperation result = SceneManager.LoadSceneAsync(worldID, LoadSceneMode.Additive);
		while (!result.isDone)
		{
			yield return new WaitForEndOfFrame();
		}
		this.worldIDIndex++;
		if (this.worldIDIndex < this.worldIDs.Length)
		{
			this.StageGame();
		}
		else if (this.WorldLoaded != null)
		{
			this.WorldLoaded();
		}
		yield break;
	}

	private void Awake()
	{
		GameManager.WorldManager = this;
		this.worldIDIndex = 0;
	}

	private void Update()
	{
		this.checkPlayerLocation();
		if (DollMakerManager.Lucassed && !WorldManager.LucasSpawnedToKill && (StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY1 || StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY3 || StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY5 || StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY6 || StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY10))
		{
			WorldManager.LucasSpawnedToKill = true;
			EnemyManager.HitManManager.LucassedJump();
		}
	}

	[SerializeField]
	private bool debugMode;

	[SerializeField]
	private int[] worldIDs;

	[SerializeField]
	private VPNCurrencyDefinition defaultVPNCurrency;

	private List<LocationVolume> currentLocationVolumes = new List<LocationVolume>(20);

	private List<LocationVolume> activeLocationVolumes = new List<LocationVolume>(20);

	private List<VPNVolume> currentVPNVolumes = new List<VPNVolume>(70);

	private int worldIDIndex;

	public static bool LucasSpawnedToKill;

	public delegate void WorldManagerActions();
}
