using System;

public class GameManager
{
	public GameManager()
	{
		this.inited = false;
	}

	public static GameManager Instance
	{
		get
		{
			if (GameManager._instance == null)
			{
				GameManager._instance = new GameManager();
			}
			return GameManager._instance;
		}
	}

	public static audioSlinger AudioSlinger
	{
		get
		{
			if (GameManager._audioSlinger == null)
			{
				GameManager._audioSlinger = new audioSlinger();
			}
			return GameManager._audioSlinger;
		}
	}

	public static timeSlinger TimeSlinger
	{
		get
		{
			if (GameManager._timeSlinger == null)
			{
				GameManager._timeSlinger = new timeSlinger(true);
			}
			return GameManager._timeSlinger;
		}
	}

	public static tweenSlinger TweenSlinger
	{
		get
		{
			if (GameManager._tweenSlinger == null)
			{
				GameManager._tweenSlinger = new tweenSlinger();
			}
			return GameManager._tweenSlinger;
		}
	}

	public static PauseManager PauseManager
	{
		get
		{
			if (GameManager._pauseManager == null)
			{
				GameManager._pauseManager = new PauseManager();
			}
			return GameManager._pauseManager;
		}
	}

	public static BehaviourManager BehaviourManager
	{
		get
		{
			if (GameManager._behaviourManager == null)
			{
				GameManager._behaviourManager = new BehaviourManager();
			}
			return GameManager._behaviourManager;
		}
	}

	public static ManagerSlinger ManagerSlinger
	{
		get
		{
			if (GameManager._managerSlinger == null)
			{
				GameManager._managerSlinger = new ManagerSlinger();
			}
			return GameManager._managerSlinger;
		}
	}

	public static WorldManager WorldManager
	{
		get
		{
			return GameManager._worldManager;
		}
		set
		{
			GameManager._worldManager = value;
		}
	}

	public static StageManager StageManager
	{
		get
		{
			return GameManager._stageManager;
		}
		set
		{
			GameManager._stageManager = value;
		}
	}

	public static InteractionManager InteractionManager
	{
		get
		{
			return GameManager._interactionManager;
		}
		set
		{
			GameManager._interactionManager = value;
		}
	}

	public static TheCloud TheCloud
	{
		get
		{
			return GameManager._theCloud;
		}
		set
		{
			GameManager._theCloud = value;
		}
	}

	public static TimeKeeper TimeKeeper
	{
		get
		{
			return GameManager._timeKeeper;
		}
		set
		{
			GameManager._timeKeeper = value;
		}
	}

	public static HackerManager HackerManager
	{
		get
		{
			return GameManager._hackerManager;
		}
		set
		{
			GameManager._hackerManager = value;
		}
	}

	public static MicManager MicManager
	{
		get
		{
			return GameManager._micManager;
		}
		set
		{
			GameManager._micManager = value;
		}
	}

	public static void Kill()
	{
		GameManager._instance = null;
		GameManager._audioSlinger = null;
		GameManager._timeSlinger = null;
		GameManager._tweenSlinger = null;
		GameManager._pauseManager = null;
		GameManager._behaviourManager = null;
		GameManager._managerSlinger = null;
		GameManager._worldManager = null;
		GameManager._stageManager = null;
		GameManager._interactionManager = null;
		GameManager._theCloud = null;
		GameManager._timeKeeper = null;
		GameManager._hackerManager = null;
		GameManager._micManager = null;
		GC.Collect();
	}

	public void Init()
	{
		this.inited = true;
	}

	public bool isInited()
	{
		return this.inited;
	}

	public void Update()
	{
		GameManager.TweenSlinger.Update();
		GameManager.PauseManager.Update();
		GameManager.TimeSlinger.Update();
		CurrencyManager.Tick();
	}

	public static void SetDOSTwitch(DOSTwitch setDOSTwitch)
	{
		GameManager.myDOSTwitch = setDOSTwitch;
	}

	public static DOSTwitch GetDOSTwitch()
	{
		return GameManager.myDOSTwitch;
	}

	private static GameManager _instance;

	private static audioSlinger _audioSlinger;

	private static timeSlinger _timeSlinger;

	private static tweenSlinger _tweenSlinger;

	private static PauseManager _pauseManager;

	private static BehaviourManager _behaviourManager;

	private static ManagerSlinger _managerSlinger;

	private static WorldManager _worldManager;

	private static StageManager _stageManager;

	private static InteractionManager _interactionManager;

	private static TheCloud _theCloud;

	private static TimeKeeper _timeKeeper;

	private static HackerManager _hackerManager;

	private static MicManager _micManager;

	private bool inited;

	private static DOSTwitch myDOSTwitch;
}
