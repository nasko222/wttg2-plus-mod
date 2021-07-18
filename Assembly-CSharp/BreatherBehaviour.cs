using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.AI;

public class BreatherBehaviour : MonoBehaviour
{
	public Transform HelperBone
	{
		get
		{
			return this.helperBone;
		}
	}

	public CapsuleCollider CapsuleCollider
	{
		get
		{
			return this.myCapsuleCollider;
		}
	}

	public void TriggerAnim(string SetAnim)
	{
		this.myAC.SetTrigger(SetAnim);
	}

	public void SoftSpawn()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = true;
		}
		this.knife.enabled = true;
	}

	public void DeSpawn()
	{
		this.voiceAudioHub.KillSound(this.breathing.AudioClip);
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = false;
		}
		this.knife.enabled = false;
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
	}

	public void HardDeSpawn()
	{
		this.voiceAudioHub.KillSound(this.breathing.AudioClip);
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = false;
		}
		this.knife.enabled = false;
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.myNavMeshAgent.enabled = false;
		BreatherPatrolBehaviour.Ins.KillPatrol();
		this.TriggerAnim("triggerIdle");
	}

	public void AttemptSpawnBehindPlayer(Transform TargetTransform, float YOffSet = 0f)
	{
		Vector3 position = TargetTransform.position - TargetTransform.forward * 0.85f;
		position.y -= YOffSet;
		base.transform.position = position;
		base.transform.rotation = TargetTransform.rotation;
		this.inMeshCheckTimeStamp = Time.time;
		this.inMeshCheckActive = true;
	}

	public void SpawnBehindPlayer(Transform TargetTransform, float YOffSet = 0f)
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = true;
		}
		this.knife.enabled = true;
		Vector3 position = TargetTransform.position - TargetTransform.forward * 0.85f;
		position.y -= YOffSet;
		base.transform.position = position;
		base.transform.rotation = TargetTransform.rotation;
	}

	public void TriggerExitRush()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = true;
		}
		this.knife.enabled = true;
		base.transform.position = new Vector3(-0.623f, 0f, 202.565f);
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
		this.ReachedEndPoint.Event += this.triggerExitJump;
		this.chargePlayer();
	}

	public void TriggerPickUpRush()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = true;
		}
		this.knife.enabled = true;
		base.transform.position = new Vector3(21.62f, 0f, 199.889f);
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		this.ReachedEndPoint.Event += this.triggerExitJump;
		this.chargePlayer();
	}

	public void TriggerDumpsterJump()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = true;
		}
		this.knife.enabled = true;
		base.transform.position = new Vector3(15.003f, 0f, 200.83f);
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
		this.TriggerAnim("triggerDumpsterJump");
	}

	public void TriggerWalkToDoor()
	{
		DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, 108.791f, 0f), 0.5f).SetEase(Ease.Linear).SetOptions(true).OnComplete(delegate
		{
			this.voiceAudioHub.PlaySound(this.breathing);
			this.TriggerAnim("triggerWalkToDoor");
		});
	}

	public void TriggerWalkAwayFromDoor()
	{
		this.TriggerAnim("triggerWalkAwayFromDoor");
	}

	public void TriggerDoorJump()
	{
		this.voiceAudioHub.KillSound(this.breathing.AudioClip);
		this.TriggerAnim("triggerDoorJump");
		this.voiceAudioHub.PlaySoundCustomDelay(this.laugh1, 0.3f);
		GameManager.TimeSlinger.FireTimer(3f, delegate()
		{
			SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.HOLDTHEDOOR);
		}, 0);
	}

	public void TriggerPeekABooJump()
	{
		this.TriggerAnim("triggerPeekABooJump");
	}

	public void TriggerVoice(BREATHER_VOICE_COMMANDS TheCommand)
	{
		if (TheCommand != BREATHER_VOICE_COMMANDS.LAUGH1)
		{
			if (TheCommand != BREATHER_VOICE_COMMANDS.HI)
			{
				if (TheCommand == BREATHER_VOICE_COMMANDS.PEEKABOO)
				{
					this.voiceAudioHub.PlaySound(this.peekABoo);
				}
			}
			else
			{
				this.voiceAudioHub.PlaySound(this.hi);
			}
		}
		else
		{
			this.voiceAudioHub.PlaySound(this.laugh1);
		}
	}

	private void chargePlayer()
	{
		this.TriggerAnim("triggerRush");
		this.myNavMeshAgent.enabled = true;
		this.myNavMeshAgent.acceleration = 12f;
		this.myNavMeshAgent.speed = 7.5f;
		this.myNavMeshAgent.angularSpeed = 300f;
		this.autoUpdateDest = true;
		this.destInProgress = true;
	}

	private void triggerExitJump()
	{
		this.ReachedEndPoint.Event -= this.triggerExitJump;
		MainCameraHook.Ins.AddHeadHit(0f);
		BreatherRoamJumper.Ins.TriggerExitRushJump();
		this.TriggerAnim("triggerRushJump");
	}

	private void playFootStepSound()
	{
		int num = UnityEngine.Random.Range(1, this.footStepSFXs.Length);
		AudioFileDefinition audioFileDefinition = this.footStepSFXs[num];
		this.footAudioHub.PlaySound(this.footStepSFXs[num]);
		this.footStepSFXs[num] = this.footStepSFXs[0];
		this.footStepSFXs[0] = audioFileDefinition;
	}

	private void playCementFootStepSound()
	{
		int num = UnityEngine.Random.Range(1, this.cementFootStepsSFXs.Length);
		AudioFileDefinition audioFileDefinition = this.cementFootStepsSFXs[num];
		this.footAudioHub.PlaySound(this.cementFootStepsSFXs[num]);
		this.cementFootStepsSFXs[num] = this.cementFootStepsSFXs[0];
		this.cementFootStepsSFXs[0] = audioFileDefinition;
	}

	public void SayHi()
	{
		this.voiceAudioHub.PlaySound(this.hi);
	}

	public void SayPeekABoo()
	{
		this.TriggerVoice(BREATHER_VOICE_COMMANDS.PEEKABOO);
	}

	public void BodyJump()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.BodyHit);
	}

	public void FloorHit()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.FloorHit);
	}

	public void KnifeStab()
	{
		MainCameraHook.Ins.AddHeadHit(0.5f);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KnifeStab3);
	}

	public void QuickKnifeStab()
	{
		MainCameraHook.Ins.AddHeadHit(0f);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KnifeStab2);
	}

	public void KnifeSlash()
	{
		MainCameraHook.Ins.AddHeadHit(0.5f);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KnifeStab1);
	}

	public void TriggerGameOver()
	{
		UIManager.TriggerGameOver("YOU WERE KILLED");
		MainCameraHook.Ins.ClearARF(1f);
	}

	private void Awake()
	{
		BreatherBehaviour.Ins = this;
		this.myAC = base.GetComponent<Animator>();
		this.myNavMeshAgent = base.GetComponent<NavMeshAgent>();
		this.myCapsuleCollider = base.GetComponent<CapsuleCollider>();
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = false;
		}
		this.knife.enabled = false;
	}

	private void Update()
	{
		if (this.autoUpdateDest)
		{
			this.myNavMeshAgent.SetDestination(roamController.Ins.transform.position);
		}
		if (this.inMeshCheckActive && Time.time - this.inMeshCheckTimeStamp >= 0.1f)
		{
			this.inMeshCheckActive = false;
			if (this.inMesh)
			{
				this.InMeshEvents.Execute();
			}
			else
			{
				this.NotInMeshEvents.Execute();
			}
			this.inMesh = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.destInProgress && other.tag == "Player")
		{
			this.destInProgress = false;
			this.autoUpdateDest = false;
			this.myNavMeshAgent.enabled = false;
			this.ReachedEndPoint.Execute();
		}
		this.inMesh = true;
	}

	private void OnTriggerStay(Collider other)
	{
		this.inMesh = true;
	}

	public static BreatherBehaviour Ins;

	public CustomEvent ReachedEndPoint = new CustomEvent(2);

	public CustomEvent InMeshEvents = new CustomEvent(2);

	public CustomEvent NotInMeshEvents = new CustomEvent(2);

	[SerializeField]
	private SkinnedMeshRenderer[] renderers = new SkinnedMeshRenderer[0];

	[SerializeField]
	private SkinnedMeshRenderer knife;

	[SerializeField]
	private Transform helperBone;

	[SerializeField]
	private AudioHubObject voiceAudioHub;

	[SerializeField]
	private AudioHubObject footAudioHub;

	[SerializeField]
	private AudioFileDefinition[] footStepSFXs = new AudioFileDefinition[0];

	[SerializeField]
	private AudioFileDefinition[] cementFootStepsSFXs = new AudioFileDefinition[0];

	[SerializeField]
	private AudioFileDefinition laugh1;

	[SerializeField]
	private AudioFileDefinition hi;

	[SerializeField]
	private AudioFileDefinition breathing;

	[SerializeField]
	private AudioFileDefinition peekABoo;

	private Animator myAC;

	private NavMeshAgent myNavMeshAgent;

	private CapsuleCollider myCapsuleCollider;

	private float inMeshCheckTimeStamp;

	private bool destInProgress;

	private bool hadPathPreviousFrame;

	private bool autoUpdateDest;

	private bool inMeshCheckActive;

	private bool inMesh;
}
