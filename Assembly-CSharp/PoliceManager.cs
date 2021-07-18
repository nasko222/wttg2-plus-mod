using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;

public class PoliceManager : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PoliceManager.WarningActions FireWarning;

	private void TriggerRaidRoomRoam()
	{
		if (this.powerWasTripped)
		{
			EnvironmentManager.PowerBehaviour.ForcePowerOn();
		}
		this.currentActiveSwatMen[1].TriggerVoiceCommand(SWAT_VOICE_COMMANDS.POLICE_DEPT, 0f);
		PoliceRoamJumper.Ins.TriggerRoomRaid(this.MainDoorTrigger.DoorMeshTransform.position);
		this.performRoomRaid();
	}

	public void TriggerPowerTrip()
	{
		if (!this.powerWasTripped)
		{
			this.powerWasTripped = true;
			EnvironmentManager.PowerBehaviour.ForcePowerOff();
		}
	}

	private void performRoomRaid()
	{
		DataManager.LockSave = true;
		DataManager.ClearGameData();
		this.currentActiveSwatMen[0].GunLight.enabled = false;
		this.currentActiveSwatMen[0].TriggerAnim("doorKick");
		if (this.MainDoorTrigger.SetDistanceLODs != null)
		{
			for (int i = 0; i < this.MainDoorTrigger.SetDistanceLODs.Length; i++)
			{
				this.MainDoorTrigger.SetDistanceLODs[i].OverwriteCulling = true;
			}
		}
		this.MainDoorTrigger.AudioHub.PlaySound(LookUp.SoundLookUp.SlamOpenDoor);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit2);
		this.MainDoorTrigger.KickDoorOpen();
		GameManager.TimeSlinger.FireTimer(0.2f, delegate()
		{
			this.currentActiveSwatMen[1].FlashBangMesh.enabled = true;
			this.currentActiveSwatMen[1].TriggerAnim("flashBang");
		}, 0);
		GameManager.TimeSlinger.FireTimer(2f, delegate()
		{
			this.currentActiveSwatMen[1].NavMeshAgent.enabled = true;
			this.currentActiveSwatMen[2].TriggerAnim("walk");
			Sequence sequence = DOTween.Sequence().OnComplete(delegate
			{
				this.currentActiveSwatMen[2].EndWalkCycle();
			});
			this.currentActiveSwatMen[2].TriggerVoiceCommand(SWAT_VOICE_COMMANDS.GO_GO, 1.5f);
			sequence.Insert(0f, DOTween.To(() => this.currentActiveSwatMen[2].transform.position, delegate(Vector3 x)
			{
				this.currentActiveSwatMen[2].transform.position = x;
			}, new Vector3(-2.249f, 39.599f, -6.46f), 0.5f).SetEase(Ease.Linear));
			sequence.Insert(0f, DOTween.To(() => this.currentActiveSwatMen[2].transform.rotation, delegate(Quaternion x)
			{
				this.currentActiveSwatMen[2].transform.rotation = x;
			}, Vector3.zero, 0.5f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Insert(0.5f, DOTween.To(() => this.currentActiveSwatMen[2].transform.position, delegate(Vector3 x)
			{
				this.currentActiveSwatMen[2].transform.position = x;
			}, new Vector3(-2.249f, 39.599f, -3.561f), 1f).SetEase(Ease.Linear));
			sequence.Insert(1f, DOTween.To(() => this.currentActiveSwatMen[2].transform.rotation, delegate(Quaternion x)
			{
				this.currentActiveSwatMen[2].transform.rotation = x;
			}, new Vector3(0f, -45f, 0f), 0.5f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Insert(1.5f, DOTween.To(() => this.currentActiveSwatMen[2].transform.position, delegate(Vector3 x)
			{
				this.currentActiveSwatMen[2].transform.position = x;
			}, this.swatEndPOS[0], 1.5f).SetEase(Ease.Linear));
			sequence.Insert(1.5f, DOTween.To(() => this.currentActiveSwatMen[2].transform.rotation, delegate(Quaternion x)
			{
				this.currentActiveSwatMen[2].transform.rotation = x;
			}, new Vector3(0f, -90f, 0f), 0.75f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Play<Sequence>();
		}, 0);
		GameManager.TimeSlinger.FireTimer(3.5f, delegate()
		{
			this.currentActiveSwatMen[3].TriggerAnim("walk");
			Sequence sequence = DOTween.Sequence().OnComplete(delegate
			{
				this.currentActiveSwatMen[3].EndWalkCycle();
			});
			sequence.Insert(0f, DOTween.To(() => this.currentActiveSwatMen[3].transform.position, delegate(Vector3 x)
			{
				this.currentActiveSwatMen[3].transform.position = x;
			}, new Vector3(-2.249f, 39.599f, -6.46f), 0.5f).SetEase(Ease.Linear));
			sequence.Insert(0f, DOTween.To(() => this.currentActiveSwatMen[3].transform.rotation, delegate(Quaternion x)
			{
				this.currentActiveSwatMen[3].transform.rotation = x;
			}, Vector3.zero, 0.5f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Insert(0.5f, DOTween.To(() => this.currentActiveSwatMen[3].transform.position, delegate(Vector3 x)
			{
				this.currentActiveSwatMen[3].transform.position = x;
			}, new Vector3(-2.249f, 39.599f, -3.561f), 1f).SetEase(Ease.Linear));
			sequence.Insert(1f, DOTween.To(() => this.currentActiveSwatMen[3].transform.rotation, delegate(Quaternion x)
			{
				this.currentActiveSwatMen[3].transform.rotation = x;
			}, new Vector3(0f, 45f, 0f), 0.5f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Insert(1.5f, DOTween.To(() => this.currentActiveSwatMen[3].transform.position, delegate(Vector3 x)
			{
				this.currentActiveSwatMen[3].transform.position = x;
			}, this.swatEndPOS[1], 2f).SetEase(Ease.Linear));
			sequence.Insert(1.5f, DOTween.To(() => this.currentActiveSwatMen[3].transform.rotation, delegate(Quaternion x)
			{
				this.currentActiveSwatMen[3].transform.rotation = x;
			}, new Vector3(0f, 90f, 0f), 0.75f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Play<Sequence>();
		}, 0);
		GameManager.TimeSlinger.FireTimer(5f, delegate()
		{
			this.currentActiveSwatMen[0].TriggerAnim("charge");
			DOTween.To(() => this.currentActiveSwatMen[0].transform.position, delegate(Vector3 x)
			{
				this.currentActiveSwatMen[0].transform.position = x;
			}, new Vector3(-2.249f, 39.599f, -6.46f), 0.25f).SetEase(Ease.Linear);
			DOTween.To(() => this.currentActiveSwatMen[0].transform.rotation, delegate(Quaternion x)
			{
				this.currentActiveSwatMen[0].transform.rotation = x;
			}, Vector3.zero, 0.25f).SetEase(Ease.Linear).SetOptions(true).OnComplete(delegate
			{
				this.currentActiveSwatMen[1].GunLight.enabled = true;
				this.currentActiveSwatMen[0].ChargePlayer();
			});
		}, 0);
		GameManager.TimeSlinger.FireTimer(6f, delegate()
		{
			this.currentActiveSwatMen[1].GunLight.enabled = false;
			this.currentActiveSwatMen[1].NavMeshAgent.enabled = false;
			this.currentActiveSwatMen[1].TriggerAnim("walk");
			Sequence s = DOTween.Sequence().OnComplete(delegate
			{
				this.currentActiveSwatMen[1].EndWalkCycle();
			});
			s.Insert(0f, DOTween.To(() => this.currentActiveSwatMen[1].transform.position, delegate(Vector3 x)
			{
				this.currentActiveSwatMen[1].transform.position = x;
			}, new Vector3(-2.249f, 39.599f, -6.46f), 0.5f).SetEase(Ease.Linear));
			s.Insert(0f, DOTween.To(() => this.currentActiveSwatMen[1].transform.rotation, delegate(Quaternion x)
			{
				this.currentActiveSwatMen[1].transform.rotation = x;
			}, Vector3.zero, 0.5f).SetEase(Ease.Linear).SetOptions(true));
			s.Insert(0.5f, DOTween.To(() => this.currentActiveSwatMen[1].transform.position, delegate(Vector3 x)
			{
				this.currentActiveSwatMen[1].transform.position = x;
			}, new Vector3(-2.249f, 39.599f, -3.561f), 1f).SetEase(Ease.Linear));
			s.Insert(1f, DOTween.To(() => this.currentActiveSwatMen[1].transform.rotation, delegate(Quaternion x)
			{
				this.currentActiveSwatMen[1].transform.rotation = x;
			}, new Vector3(0f, 90f, 0f), 0.5f).SetEase(Ease.Linear).SetOptions(true));
		}, 0);
		this.currentActiveSwatMen[2].TriggerVoiceCommand(SWAT_VOICE_COMMANDS.CLEAR, 11f);
		GameManager.TimeSlinger.FireTimer(12.5f, delegate()
		{
			MainCameraHook.Ins.ClearARF(2f);
			UIManager.TriggerGameOver("ARRESTED");
		}, 0);
	}

	private void triggerFloor8PreJump()
	{
		this.currentActiveSwatMen[0].SpawnMe(new Vector3(24.173f, this.currentActiveSwatMen[0].transform.position.y, this.currentActiveSwatMen[0].transform.position.z), this.currentActiveSwatMen[0].transform.localRotation.eulerAngles);
		GameManager.TimeSlinger.FireTimer(0.45f, delegate()
		{
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit2);
			roamController.Ins.KillOutOfWay();
			this.currentActiveSwatMen[0].TakeOverCamera();
			PoliceRoamJumper.Ins.TriggerFloor8DoorJump();
			this.currentActiveSwatMen[0].TriggerAnim("hallTackle");
		}, 0);
	}

	private void triggerFloor8DoorJump()
	{
		DataManager.LockSave = true;
		DataManager.ClearGameData();
		this.Floor8DoorTrigger.CancelAutoClose();
		this.currentActiveSwatMen[2].TriggerVoiceCommand(SWAT_VOICE_COMMANDS.POLICE_DEPT, 0f);
		this.currentActiveSwatMen[0].TriggerVoiceCommand(SWAT_VOICE_COMMANDS.GOT_YOU, 1.5f);
		GameManager.TimeSlinger.FireTimer(3.3f, delegate()
		{
			UIManager.TriggerGameOver("ARRESTED");
		}, 0);
	}

	private void triggerStairJumpPre()
	{
		DataManager.LockSave = true;
		DataManager.ClearGameData();
		if (this.powerWasTripped)
		{
			EnvironmentManager.PowerBehaviour.ForcePowerOn();
		}
		GameManager.AudioSlinger.PlaySoundWithCustomDelay(LookUp.SoundLookUp.HeadHit, 0.5f);
		PoliceRoamJumper.Ins.TriggerStairWayJump();
		this.currentActiveSwatMen[1].TriggerVoiceCommand(SWAT_VOICE_COMMANDS.POLICE_DEPT, 0.35f);
		this.currentActiveSwatMen[0].TriggerVoiceCommand(SWAT_VOICE_COMMANDS.GOT_YOU, 3f);
		GameManager.TimeSlinger.FireTimer(0.25f, delegate()
		{
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit2);
			this.currentActiveSwatMen[0].TriggerAnim("headSmash");
			this.currentActiveSwatMen[0].TriggerFootSteps(1.3f, 4, 0.4f);
		}, 0);
		GameManager.TimeSlinger.FireTimer(5f, delegate()
		{
			UIManager.TriggerGameOver("ARRESTED");
		}, 0);
	}

	private void triggerStairJumpPost()
	{
		this.Floor8DoorTrigger.CancelAutoClose();
	}

	private void userWentOnline(WifiNetworkDefinition TheNetwork)
	{
		this.currentActiveWifiNetwork = TheNetwork;
		if (this.hotNetworks.ContainsKey(TheNetwork))
		{
			this.triggerTimeWindow = this.hotNetworks[TheNetwork].TimeLeft;
			this.triggerTimeStamp = Time.time;
			this.triggerActive = true;
			this.warningActive = true;
			this.hotNetworks.Remove(TheNetwork);
			return;
		}
		this.triggerTimeWindow = TheNetwork.networkTrackRate;
		if (DataManager.LeetMode)
		{
			this.triggerTimeWindow *= 0.7f;
		}
		this.triggerTimeStamp = Time.time;
		this.triggerActive = true;
		this.warningActive = true;
	}

	private void userWentOffline()
	{
		this.triggerActive = false;
		this.warningActive = false;
		float setTimeLeft = this.triggerTimeWindow - (Time.time - this.triggerTimeStamp);
		if (this.currentActiveWifiNetwork != null)
		{
			HotWifiNetwork value = new HotWifiNetwork(this.currentActiveWifiNetwork.GetHashCode(), this.networkHotTime, Time.time, setTimeLeft);
			this.hotNetworks.Remove(this.currentActiveWifiNetwork);
			this.hotNetworks.Add(this.currentActiveWifiNetwork, value);
		}
	}

	private void attemptAttack()
	{
		int num = UnityEngine.Random.Range(0, 101);
		int num2 = Mathf.RoundToInt(this.currentActiveWifiNetwork.networkTrackProbability * 100f);
		if (num < num2)
		{
			this.triggerAttack();
			return;
		}
		if (ModsManager.Nightmare)
		{
			this.triggerAttack();
			return;
		}
		this.triggerTimeWindow = this.currentActiveWifiNetwork.networkTrackRate;
		this.triggerTimeStamp = Time.time;
		this.triggerActive = true;
		this.warningActive = true;
	}

	public void triggerAttack()
	{
		if (EnemyManager.State == ENEMY_STATE.IDLE && EnvironmentManager.PowerState == POWER_STATE.ON)
		{
			this.processAttack();
			return;
		}
		this.triggerTimeWindow = this.currentActiveWifiNetwork.networkTrackRate / 2f;
		this.triggerTimeStamp = Time.time;
		this.triggerActive = true;
		this.warningActive = true;
	}

	private void processAttack()
	{
		if (!StateManager.BeingHacked && StateManager.PlayerState != PLAYER_STATE.PEEPING)
		{
			if (StateManager.PlayerState != PLAYER_STATE.BUSY)
			{
				if (StateManager.PlayerLocation != PLAYER_LOCATION.UNKNOWN)
				{
					EnemyManager.State = ENEMY_STATE.POILCE;
					EnvironmentManager.PowerBehaviour.LockedOut = true;
					DataManager.LockSave = true;
					if (StateManager.PlayerLocation == PLAYER_LOCATION.MAIN_ROON || StateManager.PlayerLocation == PLAYER_LOCATION.OUTSIDE || StateManager.PlayerLocation == PLAYER_LOCATION.BATH_ROOM)
					{
						if (StateManager.PlayerState == PLAYER_STATE.COMPUTER || StateManager.PlayerState == PLAYER_STATE.DESK)
						{
							int num = UnityEngine.Random.Range(0, 100);
							if (num < 25)
							{
								this.powerWasTripped = true;
								EnvironmentManager.PowerBehaviour.ForcePowerOff();
							}
						}
						this.spawnForRaid();
						for (int i = 0; i < this.roomRaidTriggers.Length; i++)
						{
							this.roomRaidTriggers[i].SetActive();
						}
					}
					else if (StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY8)
					{
						for (int j = 0; j < this.powerTripTriggers.Length; j++)
						{
							this.powerTripTriggers[j].SetActive();
						}
						this.Floor8DoorTrigger.DoorOpenEvent.AddListener(new UnityAction(this.triggerStairJumpPre));
						this.Floor8DoorTrigger.DoorWasOpenedEvent.AddListener(new UnityAction(this.triggerStairJumpPost));
						this.spawnInStairWay();
					}
					else
					{
						this.Floor8DoorTrigger.DoorOpenEvent.AddListener(new UnityAction(this.triggerFloor8PreJump));
						this.Floor8DoorTrigger.DoorWasOpenedEvent.AddListener(new UnityAction(this.triggerFloor8DoorJump));
						this.Floor8DoorTrigger.SetCustomOpenDoorTime(0.45f);
						this.spawnSwatInHallWay8();
					}
				}
				else
				{
					this.triggerTimeWindow = 10f;
					this.triggerTimeStamp = Time.time;
					this.triggerActive = true;
				}
			}
			else
			{
				this.triggerTimeWindow = 5f;
				this.triggerTimeStamp = Time.time;
				this.triggerActive = true;
			}
		}
		else
		{
			this.triggerTimeWindow = 30f;
			this.triggerTimeStamp = Time.time;
			this.triggerActive = true;
		}
	}

	private void triggerWarning()
	{
		if (this.FireWarning != null)
		{
			this.FireWarning();
		}
	}

	private void spawnSwatInHallWay8()
	{
		for (int i = 0; i < this.floor8SpawnPOS.Length; i++)
		{
			SwatManBehaviour swatManBehaviour = this.swatManPool.Pop();
			if (i == 0)
			{
				swatManBehaviour.TriggerAnim("hallTackleIdle");
			}
			swatManBehaviour.SpawnMe(this.floor8SpawnPOS[i], this.floor8SpwanROT[i]);
			this.currentActiveSwatMen.Add(swatManBehaviour);
		}
		this.currentActiveSwatMen[2].TriggerAnim("standIdle2");
		this.currentActiveSwatMen[1].TriggerAnim("crouchIdle2");
	}

	private void spawnInStairWay()
	{
		for (int i = 0; i < this.stairWayPOS.Length; i++)
		{
			SwatManBehaviour swatManBehaviour = this.swatManPool.Pop();
			swatManBehaviour.SpawnMe(this.stairWayPOS[i], this.stairWayROT[i]);
			this.currentActiveSwatMen.Add(swatManBehaviour);
		}
		this.currentActiveSwatMen[0].TriggerAnim("headSmashIdle");
		this.currentActiveSwatMen[1].TriggerAnim("crouchIdle1");
		this.currentActiveSwatMen[3].TriggerAnim("crouchIdle2");
	}

	private void spawnForRaid()
	{
		for (int i = 0; i < this.roomRaidPOS.Length; i++)
		{
			SwatManBehaviour swatManBehaviour = this.swatManPool.Pop();
			swatManBehaviour.SpawnMe(this.roomRaidPOS[i], this.roomRaidROT[i]);
			this.currentActiveSwatMen.Add(swatManBehaviour);
		}
		this.currentActiveSwatMen[0].TriggerAnim("doorKickIdle");
		this.currentActiveSwatMen[1].TriggerAnim("flashBangIdle");
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void gameLive()
	{
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
	}

	private void threatsActivated()
	{
		GameManager.StageManager.ThreatsNowActivated -= this.threatsActivated;
		if (this.triggerActive)
		{
			this.triggerTimeStamp = Time.time;
		}
		this.threatsActive = true;
	}

	private void Awake()
	{
		EnemyManager.PoliceManager = this;
		this.swatManPool = new PooledStack<SwatManBehaviour>(delegate()
		{
			SwatManBehaviour component = UnityEngine.Object.Instantiate<GameObject>(this.swatManObject, this.swatParent).GetComponent<SwatManBehaviour>();
			component.Build();
			return component;
		}, this.SWAT_POOL_COUNT);
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.TheGameIsLive += this.gameLive;
		GameManager.StageManager.ThreatsNowActivated += this.threatsActivated;
		GameManager.ManagerSlinger.WifiManager.OnlineWithNetwork += this.userWentOnline;
		GameManager.ManagerSlinger.WifiManager.WentOffline += this.userWentOffline;
	}

	private void Update()
	{
		if (this.threatsActive && this.triggerActive)
		{
			if (this.warningActive && Time.time - this.triggerTimeStamp >= this.triggerTimeWindow - this.warningTime)
			{
				this.warningActive = false;
				this.triggerWarning();
			}
			if (Time.time - this.triggerTimeStamp >= this.triggerTimeWindow)
			{
				this.triggerActive = false;
				this.attemptAttack();
			}
		}
		foreach (KeyValuePair<WifiNetworkDefinition, HotWifiNetwork> keyValuePair in this.hotNetworks)
		{
			if (Time.time - keyValuePair.Value.TimeStamp >= keyValuePair.Value.HotTime)
			{
				this.hotNetworksToRemove.Enqueue(keyValuePair.Key);
			}
		}
		while (this.hotNetworksToRemove.Count > 0)
		{
			this.hotNetworks.Remove(this.hotNetworksToRemove.Dequeue());
		}
	}

	private void OnDestroy()
	{
		this.Floor8DoorTrigger.DoorOpenEvent.RemoveListener(new UnityAction(this.triggerStairJumpPre));
		this.Floor8DoorTrigger.DoorWasOpenedEvent.RemoveListener(new UnityAction(this.triggerStairJumpPost));
		this.Floor8DoorTrigger.DoorOpenEvent.RemoveListener(new UnityAction(this.triggerFloor8PreJump));
		this.Floor8DoorTrigger.DoorWasOpenedEvent.RemoveListener(new UnityAction(this.triggerFloor8DoorJump));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		for (int i = 0; i < this.swatEndPOS.Length; i++)
		{
			Gizmos.DrawWireCube(this.swatEndPOS[i], new Vector3(0.2f, 0.2f, 0.2f));
		}
	}

	public void triggerDevSwat()
	{
		if (EnemyManager.State == ENEMY_STATE.IDLE && EnvironmentManager.PowerState == POWER_STATE.ON)
		{
			this.spawnForRaid();
			for (int i = 0; i < this.roomRaidTriggers.Length; i++)
			{
				this.roomRaidTriggers[i].SetActive();
			}
		}
	}

	public void TrollPoliceScanner()
	{
		if (this.FireWarning != null)
		{
			this.FireWarning();
		}
	}

	[SerializeField]
	private EnemyHotZoneTrigger[] roomRaidTriggers = new EnemyHotZoneTrigger[0];

	[SerializeField]
	private EnemyHotZoneTrigger[] powerTripTriggers = new EnemyHotZoneTrigger[0];

	[SerializeField]
	private DoorTrigger Floor8DoorTrigger;

	[SerializeField]
	private DoorTrigger MainDoorTrigger;

	[SerializeField]
	private float warningTime = 30f;

	[SerializeField]
	private float networkHotTime = 60f;

	[SerializeField]
	private int SWAT_POOL_COUNT = 4;

	[SerializeField]
	private Transform swatParent;

	[SerializeField]
	private GameObject swatManObject;

	[SerializeField]
	private Vector3[] floor8SpawnPOS = new Vector3[0];

	[SerializeField]
	private Vector3[] floor8SpwanROT = new Vector3[0];

	[SerializeField]
	private Vector3[] stairWayPOS = new Vector3[0];

	[SerializeField]
	private Vector3[] stairWayROT = new Vector3[0];

	[SerializeField]
	private Vector3[] roomRaidPOS = new Vector3[0];

	[SerializeField]
	private Vector3[] roomRaidROT = new Vector3[0];

	[SerializeField]
	private Vector3[] swatEndPOS = new Vector3[0];

	private WifiNetworkDefinition currentActiveWifiNetwork;

	private Dictionary<WifiNetworkDefinition, HotWifiNetwork> hotNetworks = new Dictionary<WifiNetworkDefinition, HotWifiNetwork>(10);

	private Queue<WifiNetworkDefinition> hotNetworksToRemove = new Queue<WifiNetworkDefinition>(10);

	private PooledStack<SwatManBehaviour> swatManPool;

	private List<SwatManBehaviour> currentActiveSwatMen = new List<SwatManBehaviour>(4);

	private float triggerTimeWindow;

	private float triggerTimeStamp;

	private bool threatsActive;

	private bool triggerActive;

	private bool warningActive;

	private bool powerWasTripped;

	public delegate void WarningActions();
}
