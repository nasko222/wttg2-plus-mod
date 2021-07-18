using System;
using UnityEngine;

public class DeBugger : MonoBehaviour
{
	public void ClearGameData()
	{
		FileSlinger.wildDeleteFile("WTTG2.gd");
	}

	public void ClearOptionData()
	{
		FileSlinger.wildDeleteFile("WTTG2OPTDATA.gd");
	}

	public void SpawnRoom()
	{
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = this.ReSpawnLocation;
	}

	public void SpawnLobby()
	{
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = this.LobbyLocation;
	}

	public void SpawnDeadDrop()
	{
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = this.DeadDropLocation;
	}

	private void gameLiveDebug()
	{
		this.gameIsLive = true;
	}

	private void Awake()
	{
		GameManager.StageManager.TheGameIsLive += this.gameLiveDebug;
	}

	public void Update()
	{
		this.CurrentGameState = StateManager.GameState;
		this.CurrentPlayerState = StateManager.PlayerState;
		this.CurrentPlayerLocation = StateManager.PlayerLocation;
		this.BeingHacked = StateManager.BeingHacked;
		this.SaveLocked = DataManager.LockSave;
		if (this.gameIsLive)
		{
			this.CurrentRoamControllerState = ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).MyState;
			this.CurrentDeskControllerState = ControllerManager.Get<deskController>(GAME_CONTROLLER.DESK).MyState;
			this.CurrentComputerControllerState = ControllerManager.Get<computerController>(GAME_CONTROLLER.COMPUTER).MyState;
			this.CurrentHideControllerState = ControllerManager.Get<hideController>(GAME_CONTROLLER.HIDE).MyState;
			this.CurrentEnemyState = EnemyManager.State;
		}
	}

	private void OnDestroy()
	{
	}

	public GAME_STATE CurrentGameState;

	public PLAYER_STATE CurrentPlayerState;

	public PLAYER_LOCATION CurrentPlayerLocation;

	public ENEMY_STATE CurrentEnemyState;

	public bool BeingHacked;

	public GAME_CONTROLLER_STATE CurrentRoamControllerState;

	public GAME_CONTROLLER_STATE CurrentDeskControllerState;

	public GAME_CONTROLLER_STATE CurrentComputerControllerState;

	public GAME_CONTROLLER_STATE CurrentHideControllerState;

	public bool SaveLocked;

	public Vector3 ReSpawnLocation = Vector3.zero;

	public Vector3 LobbyLocation = Vector3.zero;

	public Vector3 DeadDropLocation = Vector3.zero;

	private bool gameIsLive;
}
