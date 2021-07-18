using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
	public GameData()
	{
		this.CurProgressState = PROGRESS_STATE.NEW;
		this.Controllers = new Dictionary<GAME_CONTROLLER, GameControllerData>();
		this.Cameras = new Dictionary<CAMERA_ID, CameraData>();
		this.PlayerData = new PlayerData();
	}

	public PROGRESS_STATE CurProgressState;

	public Dictionary<GAME_CONTROLLER, GameControllerData> Controllers;

	public Dictionary<CAMERA_ID, CameraData> Cameras;

	public PlayerData PlayerData;
}
