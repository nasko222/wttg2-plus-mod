using System;
using System.Collections.Generic;

public static class ControllerManager
{
	public static void Add(baseController ControllerToAdd)
	{
		if (!ControllerManager._controllers.ContainsKey(ControllerToAdd.Controller))
		{
			ControllerManager._controllers.Add(ControllerToAdd.Controller, ControllerToAdd);
		}
	}

	public static T Get<T>(GAME_CONTROLLER ControllerType) where T : baseController
	{
		if (ControllerManager._controllers.ContainsKey(ControllerType))
		{
			return (T)((object)ControllerManager._controllers[ControllerType]);
		}
		return (T)((object)null);
	}

	public static void Remove(GAME_CONTROLLER ControllerToRemove)
	{
		ControllerManager._controllers.Remove(ControllerToRemove);
	}

	private static Dictionary<GAME_CONTROLLER, baseController> _controllers = new Dictionary<GAME_CONTROLLER, baseController>(EnumComparer.GameControllerCompare);
}
