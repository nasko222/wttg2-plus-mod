using System;
using System.Collections.Generic;

public class GameControllerCompare : IEqualityComparer<GAME_CONTROLLER>
{
	public bool Equals(GAME_CONTROLLER a, GAME_CONTROLLER b)
	{
		return a == b;
	}

	public int GetHashCode(GAME_CONTROLLER obj)
	{
		return (int)obj;
	}
}
