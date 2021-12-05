using System;

public static class EnemyManager
{
	public static PoliceManager PoliceManager
	{
		get
		{
			return EnemyManager._policeManager;
		}
		set
		{
			EnemyManager._policeManager = value;
		}
	}

	public static HitManManager HitManManager
	{
		get
		{
			return EnemyManager._hitManManager;
		}
		set
		{
			EnemyManager._hitManManager = value;
		}
	}

	public static CultManager CultManager
	{
		get
		{
			return EnemyManager._cultManager;
		}
		set
		{
			EnemyManager._cultManager = value;
		}
	}

	public static DollMakerManager DollMakerManager
	{
		get
		{
			return EnemyManager._dollMakerManager;
		}
		set
		{
			EnemyManager._dollMakerManager = value;
		}
	}

	public static BreatherManager BreatherManager
	{
		get
		{
			return EnemyManager._breatherManager;
		}
		set
		{
			EnemyManager._breatherManager = value;
		}
	}

	public static void Clear()
	{
		EnemyManager.State = ENEMY_STATE.IDLE;
		EnemyManager._policeManager = null;
		EnemyManager._hitManManager = null;
		EnemyManager._cultManager = null;
		EnemyManager._dollMakerManager = null;
		EnemyManager._breatherManager = null;
		EnemyManager._bombMakerManager = null;
		GC.Collect();
	}

	public static BombMakerManager BombMakerManager
	{
		get
		{
			return EnemyManager._bombMakerManager;
		}
		set
		{
			EnemyManager._bombMakerManager = value;
		}
	}

	public static ENEMY_STATE State;

	private static PoliceManager _policeManager;

	private static HitManManager _hitManManager;

	private static CultManager _cultManager;

	private static DollMakerManager _dollMakerManager;

	private static BreatherManager _breatherManager;

	private static BombMakerManager _bombMakerManager;
}
