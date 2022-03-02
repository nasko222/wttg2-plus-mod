using System;
using UnityEngine;

public class BombMakerManager : MonoBehaviour
{
	public void ReleaseTheBombMaker()
	{
		if (!this.bombMakerReleased)
		{
			this.SulphurTaken = 0;
			GameManager.StageManager.ManuallyActivateThreats();
			this.bombMakerReleased = true;
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(210f, 390f), new Action(this.BombMakerPresence), 0);
		}
	}

	private void triggerBombMakerKill()
	{
		if ((EnemyManager.State != ENEMY_STATE.IDLE || EnvironmentManager.PowerState != POWER_STATE.ON || StateManager.BeingHacked) && (EnemyManager.State != ENEMY_STATE.BOMB_MAKER || EnvironmentManager.PowerState != POWER_STATE.ON || StateManager.BeingHacked))
		{
			this.sulphurWindow = 40f;
			this.sulphurTimeStamp = Time.time;
			this.sulphurActive = true;
			return;
		}
		EnvironmentManager.PowerBehaviour.LockedOut = true;
		DataManager.LockSave = true;
		DataManager.ClearGameData();
		EnemyManager.State = ENEMY_STATE.BOMB_MAKER;
		BombMakerDeskJumper.Ins.AddComputerJump();
		BombMakerBehaviour.Ins.StageBombMakerOutsideKill();
	}

	private void activateSulphurTime()
	{
		this.sulphurWindow = UnityEngine.Random.Range(this.bmData.SulphurCoolTimeMin, this.bmData.SulphurCoolTimeMax);
		this.sulphurTimeStamp = Time.time;
		this.sulphurActive = true;
	}

	private void triggerSulphurTimesUp()
	{
		if ((EnemyManager.State != ENEMY_STATE.IDLE || EnvironmentManager.PowerState != POWER_STATE.ON || StateManager.BeingHacked) && (EnemyManager.State != ENEMY_STATE.BOMB_MAKER || EnvironmentManager.PowerState != POWER_STATE.ON || StateManager.BeingHacked))
		{
			this.sulphurWindow = 40f;
			this.sulphurTimeStamp = Time.time;
			this.sulphurActive = true;
			return;
		}
		EnemyManager.State = ENEMY_STATE.BOMB_MAKER;
		if (SulphurInventory.SulphurAmount <= 0)
		{
			this.triggerBombMakerKill();
			return;
		}
		EnemyManager.State = ENEMY_STATE.IDLE;
		SulphurInventory.RemoveSulphur(1);
		if (!ModsManager.Nightmare)
		{
			CurrencyManager.AddCurrency(ModsManager.EasyModeActive ? 20f : 65f);
		}
		if (this.SulphurTaken >= this.bmData.maxSulphurReq)
		{
			CurrencyManager.AddCurrency(ModsManager.Nightmare ? 50f : 100f);
			return;
		}
		this.SulphurTaken++;
		this.PlayLaugh();
		this.activateSulphurTime();
		if (ModsManager.Nightmare)
		{
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				GameManager.TimeSlinger.FireTimer((float)UnityEngine.Random.Range(80, 120), new Action(this.PlayExplosion), 0);
				return;
			}
			GameManager.TimeSlinger.FireTimer((float)UnityEngine.Random.Range(50, 90), new Action(this.PlayExplosion2), 0);
			return;
		}
		else
		{
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(340f, 480f), new Action(this.PlayExplosion), 0);
				return;
			}
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(280f, 360f), new Action(this.PlayExplosion2), 0);
			return;
		}
	}

	private void Awake()
	{
		EnemyManager.BombMakerManager = this;
		this.bmData = new BombMakerDataDefinition();
		this.myID = base.transform.position.GetHashCode();
	}

	private void Update()
	{
		if (this.sulphurActive && Time.time - this.sulphurTimeStamp >= this.sulphurWindow)
		{
			this.sulphurActive = false;
			this.triggerSulphurTimesUp();
		}
	}

	private void OnDestroy()
	{
		EnemyManager.BombMakerManager = null;
		UnityEngine.Object.Destroy(this.packageHub);
		UnityEngine.Object.Destroy(this.explosionHub1);
		UnityEngine.Object.Destroy(this.explosionHub2);
		BombMakerManager.scheduledAutoLeave = false;
		Debug.Log("[BombMaker Mod] Unloaded successfully");
	}

	private void Start()
	{
		new GameObject("BombMakerBehaviour").AddComponent<BombMakerBehaviour>();
		GameObject.Find("deskController").AddComponent<BombMakerDeskJumper>();
		GameObject.Find("deskController").AddComponent<BombMakerDeskPresence>();
		AudioHubObject audioHubObject = new GameObject().AddComponent<AudioHubObject>();
		audioHubObject.transform.position = new Vector3(-0.813f, 40.849f, -0.224f);
		this.packageHub = audioHubObject;
		AudioHubObject audioHubObject2 = new GameObject().AddComponent<AudioHubObject>();
		audioHubObject2.transform.position = new Vector3(-24.266f, 39.582f, -6.24f);
		this.explosionHub1 = audioHubObject2;
		AudioHubObject audioHubObject3 = new GameObject().AddComponent<AudioHubObject>();
		audioHubObject3.transform.position = new Vector3(24.266f, 39.582f, -12.24f);
		this.explosionHub2 = audioHubObject3;
	}

	public void PlayLaugh()
	{
		if (StateManager.PlayerLocation == PLAYER_LOCATION.MAIN_ROON || StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY8)
		{
			this.packageHub.PlaySound(CustomSoundLookUp.bombmaker);
		}
	}

	public void PlayExplosion()
	{
		if (StateManager.PlayerLocation == PLAYER_LOCATION.MAIN_ROON || StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY8)
		{
			this.explosionHub1.PlaySound(CustomSoundLookUp.explosion);
		}
	}

	public void PlayExplosion2()
	{
		if (StateManager.PlayerLocation == PLAYER_LOCATION.MAIN_ROON || StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY8)
		{
			this.explosionHub2.PlaySound(CustomSoundLookUp.explosion);
		}
	}

	public string SulphurDebug
	{
		get
		{
			if (this.sulphurWindow - (Time.time - this.sulphurTimeStamp) > 0f)
			{
				return ((int)(this.sulphurWindow - (Time.time - this.sulphurTimeStamp))).ToString();
			}
			return 0.ToString();
		}
	}

	private void BombMakerPresence()
	{
		if ((EnemyManager.State != ENEMY_STATE.IDLE || EnvironmentManager.PowerState != POWER_STATE.ON || StateManager.BeingHacked) && (EnemyManager.State != ENEMY_STATE.BOMB_MAKER || EnvironmentManager.PowerState != POWER_STATE.ON || StateManager.BeingHacked))
		{
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(20f, 40f), new Action(this.BombMakerPresence), 0);
			return;
		}
		EnemyManager.State = ENEMY_STATE.BOMB_MAKER;
		BombMakerDeskPresence.Ins.AddComputerPresence();
		EnvironmentManager.PowerBehaviour.LockedOut = true;
		BombMakerManager.scheduledAutoLeave = true;
		GameManager.TimeSlinger.FireTimer(25f, new Action(this.TriggerAutoPCLeave), 0);
	}

	public void ClearPresenceState()
	{
		EnemyManager.State = ENEMY_STATE.IDLE;
		EnvironmentManager.PowerBehaviour.LockedOut = false;
		this.activateSulphurTime();
	}

	public void ReleaseTheBombMakerInstantly()
	{
		if (!this.bombMakerReleased)
		{
			this.SulphurTaken = 0;
			GameManager.StageManager.ManuallyActivateThreats();
			this.bombMakerReleased = true;
			this.activateSulphurTime();
			this.PlayExplosion();
			GameManager.TimeSlinger.FireTimer(3f, new Action(this.PlayLaugh), 0);
		}
	}

	private void TriggerAutoPCLeave()
	{
		if (!BombMakerManager.scheduledAutoLeave)
		{
			return;
		}
		if (StateManager.PlayerState == PLAYER_STATE.COMPUTER)
		{
			computerController.Ins.LeaveMe();
			return;
		}
		GameManager.TimeSlinger.FireTimer(20f, new Action(this.TriggerAutoPCLeave), 0);
	}

	public void ReleaseTheBombMakerFastDBG()
	{
		if (!this.bombMakerReleased)
		{
			this.SulphurTaken = 0;
			GameManager.StageManager.ManuallyActivateThreats();
			this.bombMakerReleased = true;
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(2f, 8f), new Action(this.BombMakerPresence), 0);
		}
	}

	private float sulphurTimeStamp;

	private float sulphurWindow;

	private bool bombMakerReleased;

	private bool bombMakerActivated;

	private bool sulphurActive;

	private int myID;

	private bool forced;

	[HideInInspector]
	public BombMakerBehaviour bombMakerBehaviour;

	[HideInInspector]
	public BombMakerDataDefinition bmData;

	[HideInInspector]
	public int SulphurTaken;

	private AudioHubObject packageHub;

	private AudioHubObject explosionHub1;

	private AudioHubObject explosionHub2;

	public static bool scheduledAutoLeave;
}
