using System;

public static class EnvironmentManager
{
	public static PowerBehaviour PowerBehaviour
	{
		get
		{
			return EnvironmentManager._powerBehaviour;
		}
		set
		{
			EnvironmentManager._powerBehaviour = value;
		}
	}

	public static POWER_STATE PowerState
	{
		get
		{
			return EnvironmentManager._powerState;
		}
		set
		{
			EnvironmentManager._powerState = value;
		}
	}

	public static void Clear()
	{
		EnvironmentManager._powerState = POWER_STATE.ON;
		EnvironmentManager._powerBehaviour = null;
		GC.Collect();
	}

	private static PowerBehaviour _powerBehaviour;

	private static POWER_STATE _powerState;
}
