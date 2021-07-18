using System;

public static class StateManager
{
	public static GAME_STATE GameState
	{
		get
		{
			return StateManager._gameState;
		}
		set
		{
			StateManager._gameState = value;
		}
	}

	public static PLAYER_STATE PlayerState
	{
		get
		{
			return StateManager._playerState;
		}
		set
		{
			StateManager._playerState = value;
			StateManager.PlayerStateChangeEvents.Execute();
		}
	}

	public static PLAYER_LOCATION PlayerLocation
	{
		get
		{
			return StateManager._playerLocation;
		}
		set
		{
			StateManager._playerLocation = value;
			StateManager.PlayerLocationChangeEvents.Execute();
		}
	}

	public static bool BeingHacked
	{
		get
		{
			return StateManager._beingHacked;
		}
		set
		{
			StateManager._beingHacked = value;
		}
	}

	public static void Clear()
	{
		StateManager.PlayerStateChangeEvents.Clear();
		StateManager.PlayerLocationChangeEvents.Clear();
		StateManager._gameState = GAME_STATE.LIVE;
		StateManager._playerState = PLAYER_STATE.BUSY;
		StateManager._playerLocation = PLAYER_LOCATION.UNKNOWN;
		StateManager._beingHacked = false;
	}

	public static CustomEvent PlayerStateChangeEvents = new CustomEvent(5);

	public static CustomEvent PlayerLocationChangeEvents = new CustomEvent(5);

	private static GAME_STATE _gameState;

	private static PLAYER_STATE _playerState;

	private static PLAYER_LOCATION _playerLocation;

	private static bool _beingHacked;
}
