using System;

[Serializable]
public class GameControllerData
{
	public GameControllerData(GAME_CONTROLLER SetController, GAME_CONTROLLER_STATE SetControllerState, bool SetActive, Vect3 SetLastPosition, Vect3 SetLastRotation)
	{
		this.Controller = SetController;
		this.ControllerState = SetControllerState;
		this.Active = SetActive;
		this.LastPosition = SetLastPosition;
		this.LastRotation = SetLastRotation;
	}

	public GAME_CONTROLLER Controller;

	public GAME_CONTROLLER_STATE ControllerState;

	public bool Active;

	public Vect3 LastPosition;

	public Vect3 LastRotation;
}
