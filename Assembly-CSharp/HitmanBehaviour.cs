using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using SWS;
using UnityEngine;
using UnityEngine.AI;

public class HitmanBehaviour : MonoBehaviour
{
	public bool FootStepSounds
	{
		get
		{
			return this.footStepSoundsEnabled;
		}
		set
		{
			this.footStepSoundsEnabled = value;
		}
	}

	public bool InBathRoom
	{
		get
		{
			return this.inBathRoom;
		}
	}

	public HitmanSpawnDefinition SpawnData
	{
		get
		{
			return this.spawnData;
		}
		set
		{
			this.spawnData = value;
		}
	}

	public splineMove SplineMove
	{
		get
		{
			return this.mySplineMove;
		}
	}

	public void Spawn(HitmanSpawnDefinition SetSpawnData = null)
	{
		if (SetSpawnData != null)
		{
			this.spawnData = SetSpawnData;
		}
		if (this.spawnData != null)
		{
			this.Spawn(this.spawnData.Position, this.spawnData.Rotation);
			if (this.spawnData.HasWalkPath)
			{
				this.FollowPath(this.spawnData.WalkPath);
			}
		}
	}

	public void Spawn(Vector3 SetPOS, Vector3 SetROT)
	{
		base.transform.position = SetPOS;
		base.transform.rotation = Quaternion.Euler(SetROT);
		this.hitmanMesh.enabled = true;
	}

	public void DeSpawn()
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.hitmanMesh.enabled = false;
		this.gunMesh.enabled = false;
		this.gunFlash.enabled = false;
		this.footStepSoundsEnabled = false;
		this.GunFlashDoneEvents.Clear();
		this.WildCardEvents.Clear();
		this.ReachedEndPoint.Clear();
	}

	public void FollowPath(PathManagerDefinition SetPath)
	{
		this.killWalking = false;
		this.setWalkRate(0f, 1f, 0.35f);
		this.setWalkRate(1f, 0f, 0.4f, SetPath.PathTime - 0.15f);
		this.mySplineMove.pathContainer = SetPath.ThePath;
		this.mySplineMove.SetPath(SetPath.ThePath);
		this.mySplineMove.speed = SetPath.PathTime;
		this.mySplineMove.StartMove();
	}

	public void PatrolTo(PatrolPointDefinition Point)
	{
		this.currentPatrolPoint = Point;
		this.myNavMeshAgent.enabled = true;
		this.myNavMeshAgent.SetDestination(Point.Position);
		this.destInProgress = true;
	}

	public void GotoTarget(Vector3 Destination)
	{
		this.myNavMeshAgent.enabled = true;
		this.myNavMeshAgent.SetDestination(Destination);
		this.destInProgress = true;
	}

	public void TriggerAnim(string SetTrigger)
	{
		this.myAC.SetTrigger(SetTrigger);
	}

	public void ActivateGunMesh()
	{
		this.gunMesh.enabled = true;
	}

	public void WalkIntoMainRoom()
	{
		this.FollowPath(this.walkIntoMainRoomPath);
	}

	public void LeaveMainRoom()
	{
		this.WildCardEvents.Event += this.openMainDoorFromInside;
		DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, 160.688f, 0f), 0.35f).SetEase(Ease.Linear).SetOptions(true).OnComplete(delegate
		{
			this.TriggerAnim("mainDoorOpenInside");
		});
	}

	public void ExitMainRoom()
	{
		this.FollowPath(this.walkOutOfMainRoomPath);
	}

	public void WalkAwayFromMainDoor()
	{
		this.turnTween = DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, 90f, 0f), 0.75f).SetEase(Ease.Linear).SetOptions(true).OnComplete(delegate
		{
			this.FollowPath(this.walkFromMainDoorPath);
		});
	}

	public void KillWalk()
	{
		this.killWalking = true;
		this.myAC.SetFloat("walking", 0f);
		this.turnTween.Kill(false);
		this.mySplineMove.Stop();
	}

	public void KillPatrol()
	{
		this.ReachedEndPoint.Clear();
		this.myNavMeshAgent.enabled = false;
		this.destInProgress = false;
		this.myAC.SetFloat("walking", 0f);
	}

	public void EnterMainRoom()
	{
		this.WildCardEvents.Event += this.openMainDoorFromOutSide;
		HitmanBehaviour.Ins.Spawn(new Vector3(-2.309f, 39.589f, -5.935f), Vector3.zero);
		HitmanBehaviour.Ins.TriggerAnim("mainDoorOpenOutside");
	}

	public void EnterBathRoom()
	{
		this.inBathRoom = true;
		LookUp.Doors.BathroomDoor.NPCOpenDoor();
		GameManager.TimeSlinger.FireTimer(1f, delegate()
		{
			this.PatrolTo(this.bathRoomPatrolPoint);
			GameManager.TimeSlinger.FireTimer(16.5f, delegate()
			{
				this.inBathRoom = false;
				LookUp.Doors.BathroomDoor.ForceDoorClose();
			}, 0);
		}, 0);
	}

	public void TriggerGunFlash()
	{
		this.gunHub.PlaySound(this.gunShotSFX);
		this.gunFlash.enabled = true;
		GameManager.TimeSlinger.FireTimer(0.15f, delegate()
		{
			this.GunFlashDoneEvents.Execute();
			this.gunFlash.enabled = false;
		}, 0);
	}

	public void TriggerPrayForYouAni()
	{
		this.myAC.SetTrigger("prayForYou");
	}

	public void TriggerYouFool()
	{
		this.myAC.SetTrigger("youFool");
	}

	public void SayPrayForYou()
	{
		this.voiceHub.PlaySound(this.prayForYouSFX);
	}

	public void SayYouFool()
	{
		this.voiceHub.PlaySound(this.youFoolSFX);
	}

	public void TriggerWildCardEvent()
	{
		this.WildCardEvents.Execute();
		this.WildSelfDestructEvents.ExecuteAndKill();
	}

	public void TriggerFootSound()
	{
		if (this.footStepSoundsEnabled)
		{
			int num = UnityEngine.Random.Range(1, this.footStepSFXs.Length);
			AudioFileDefinition audioFileDefinition = this.footStepSFXs[num];
			this.footHub.PlaySoundWithWildPitch(this.footStepSFXs[num], 0.85f, 1.1f);
			this.footStepSFXs[num] = this.footStepSFXs[0];
			this.footStepSFXs[0] = audioFileDefinition;
		}
	}

	private void setWalkRate(float FromValue, float ToValue, float Duration)
	{
		if (!this.killWalking)
		{
			GameManager.TweenSlinger.FireDOSTweenLiner(FromValue, ToValue, Duration, delegate(float setValue)
			{
				if (!this.killWalking)
				{
					this.myAC.SetFloat("walking", setValue);
				}
			});
		}
	}

	private void setWalkRate(float FromValue, float ToValue, float Duration, float Delay)
	{
		GameManager.TimeSlinger.FireTimer<float, float, float>(Delay, new Action<float, float, float>(this.setWalkRate), FromValue, ToValue, Duration, 0);
	}

	private void reachedEndPoint()
	{
		this.myAC.SetFloat("walking", 0f);
		this.destInProgress = false;
		this.myNavMeshAgent.enabled = false;
		if (this.currentPatrolPoint != null)
		{
			this.currentPatrolPoint.InvokeEvents();
		}
		this.ReachedEndPoint.Execute();
	}

	private void pathIsDone()
	{
		this.ReachedEndPath.Execute();
	}

	private void openMainDoorFromOutSide()
	{
		this.WildCardEvents.Event -= this.openMainDoorFromOutSide;
		this.WildCardEvents.Event += this.enterMainRoomFromOutSide;
		LookUp.Doors.MainDoor.NPCOpenDoor();
	}

	private void enterMainRoomFromOutSide()
	{
		this.FootStepSounds = true;
		this.WildCardEvents.Event -= this.enterMainRoomFromOutSide;
		this.WalkIntoMainRoom();
	}

	private void openMainDoorFromInside()
	{
		this.WildCardEvents.Event -= this.openMainDoorFromInside;
		LookUp.Doors.MainDoor.NPCOpenDoor();
		GameManager.TimeSlinger.FireTimer(1.3f, new Action(this.exitRoom), 0);
	}

	private void exitRoom()
	{
		this.SplineMove.PathIsCompleted += this.deSpawn;
		this.ExitMainRoom();
		GameManager.TimeSlinger.FireTimer(2f, delegate()
		{
			this.footStepSoundsEnabled = false;
			LookUp.Doors.MainDoor.ForceDoorClose();
		}, 0);
	}

	private void deSpawn()
	{
		EnemyManager.HitManManager.DeSpawn();
		this.SplineMove.PathIsCompleted -= this.deSpawn;
		if (LookUp.Doors.MainDoor.SetDistanceLODs != null)
		{
			for (int i = 0; i < LookUp.Doors.MainDoor.SetDistanceLODs.Length; i++)
			{
				LookUp.Doors.MainDoor.SetDistanceLODs[i].OverwriteCulling = false;
			}
		}
		this.DeSpawn();
	}

	private void Awake()
	{
		HitmanBehaviour.Ins = this;
		this.myAC = base.GetComponent<Animator>();
		this.mySplineMove = base.GetComponent<splineMove>();
		this.myNavMeshAgent = base.GetComponent<NavMeshAgent>();
		this.hitmanMesh.enabled = false;
		this.gunMesh.enabled = false;
		this.gunFlash.enabled = false;
		this.myNavMeshAgent.enabled = false;
		this.mySplineMove.PathIsCompleted += this.pathIsDone;
	}

	private void Update()
	{
		if (this.destInProgress && this.myNavMeshAgent.enabled)
		{
			if (this.myNavMeshAgent.hasPath)
			{
				this.myAC.SetFloat("walking", this.myNavMeshAgent.velocity.magnitude);
				this.hadPathPreviousFrame = true;
			}
			else if (this.hadPathPreviousFrame)
			{
				this.hadPathPreviousFrame = false;
				this.reachedEndPoint();
			}
		}
	}

	private void OnDestroy()
	{
		this.mySplineMove.PathIsCompleted -= this.pathIsDone;
		this.GunFlashDoneEvents.Clear();
		this.WildCardEvents.Clear();
		this.ReachedEndPoint.Clear();
		this.ReachedEndPath.Clear();
	}

	public void GunFlashBombMaker()
	{
		AudioFileDefinition audioFileDefinition = UnityEngine.Object.Instantiate<AudioFileDefinition>(this.gunShotSFX);
		audioFileDefinition.MyAudioLayer = AUDIO_LAYER.PLAYER;
		audioFileDefinition.MyAudioHub = AUDIO_HUB.PLAYER_HUB;
		GameManager.AudioSlinger.PlaySound(audioFileDefinition);
		UnityEngine.Object.Destroy(audioFileDefinition);
	}

	public static HitmanBehaviour Ins;

	public CustomEvent GunFlashDoneEvents = new CustomEvent(2);

	public CustomEvent WildCardEvents = new CustomEvent(2);

	public CustomEvent WildSelfDestructEvents = new CustomEvent(2);

	public CustomEvent ReachedEndPath = new CustomEvent(2);

	public CustomEvent ReachedEndPoint = new CustomEvent(2);

	[SerializeField]
	private SkinnedMeshRenderer hitmanMesh;

	[SerializeField]
	private SkinnedMeshRenderer gunMesh;

	[SerializeField]
	private MeshRenderer gunFlash;

	[SerializeField]
	private AudioHubObject voiceHub;

	[SerializeField]
	private AudioHubObject footHub;

	[SerializeField]
	private AudioHubObject gunHub;

	[SerializeField]
	private PathManagerDefinition walkFromMainDoorPath;

	[SerializeField]
	private PathManagerDefinition walkIntoMainRoomPath;

	[SerializeField]
	private PathManagerDefinition walkOutOfMainRoomPath;

	[SerializeField]
	private AudioFileDefinition gunShotSFX;

	[SerializeField]
	private AudioFileDefinition prayForYouSFX;

	[SerializeField]
	private AudioFileDefinition youFoolSFX;

	[SerializeField]
	private AudioFileDefinition[] footStepSFXs = new AudioFileDefinition[0];

	[SerializeField]
	private PatrolPointDefinition bathRoomPatrolPoint;

	private Animator myAC;

	private splineMove mySplineMove;

	private Tween turnTween;

	private NavMeshAgent myNavMeshAgent;

	private bool killWalking;

	private bool destInProgress;

	private bool hadPathPreviousFrame;

	private bool footStepSoundsEnabled;

	private bool inBathRoom;

	private HitmanSpawnDefinition spawnData;

	private PatrolPointDefinition currentPatrolPoint;
}
