using System;
using UnityEngine;
using UnityEngine.AI;

public class SwatManBehaviour : MonoBehaviour
{
	public NavMeshAgent NavMeshAgent
	{
		get
		{
			return this.myNavMeshAgent;
		}
	}

	public MeshRenderer FlashBangMesh
	{
		get
		{
			return this.flashBangMesh;
		}
	}

	public Light GunLight
	{
		get
		{
			return this.gunLight;
		}
	}

	public void Build()
	{
		this.myAC = base.GetComponent<Animator>();
		this.myNavMeshAgent = base.GetComponent<NavMeshAgent>();
		this.myCapCollider = base.GetComponent<CapsuleCollider>();
		this.flashBangMesh = this.flashBangObject.GetComponent<MeshRenderer>();
		this.myAgentLinkMover = base.GetComponent<AgentLinkMover>();
		this.myNavMeshAgent.enabled = false;
		this.myCapCollider.enabled = false;
		this.mySkinMesh.enabled = false;
		this.myGunMesh.enabled = false;
		this.gunLight.enabled = false;
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
		CameraManager.Get(CAMERA_ID.MAIN, out this.cameraIControl);
	}

	public void SpawnMe(Vector3 SetPOS, Vector3 SetROT)
	{
		this.mySkinMesh.enabled = true;
		this.myGunMesh.enabled = true;
		this.gunLight.enabled = true;
		base.transform.position = SetPOS;
		base.transform.rotation = Quaternion.Euler(SetROT);
	}

	public void SpawnMe(Vector3 SetPOS, Vector3 SetROT, string SetAnim)
	{
		this.TriggerAnim(SetAnim);
		this.SpawnMe(SetPOS, SetROT);
	}

	public void TriggerAnim(string SetTrigger)
	{
		this.myAC.SetTrigger(SetTrigger);
	}

	public void TriggerVoiceCommand(SWAT_VOICE_COMMANDS SetCommand, float DelayAmount = 0f)
	{
		switch (SetCommand)
		{
		case SWAT_VOICE_COMMANDS.GET_DOWN:
			this.myAudioHub.PlaySoundCustomDelay(this.getDownSFX, DelayAmount);
			break;
		case SWAT_VOICE_COMMANDS.POLICE_DEPT:
			this.myAudioHub.PlaySoundCustomDelay(this.policeDeptSFX, DelayAmount);
			break;
		case SWAT_VOICE_COMMANDS.STAY_DOWN:
			this.myAudioHub.PlaySoundCustomDelay(this.stayDownSFX, DelayAmount);
			break;
		case SWAT_VOICE_COMMANDS.GOT_YOU:
			this.myAudioHub.PlaySoundCustomDelay(this.gotYouNowSFX, DelayAmount);
			break;
		case SWAT_VOICE_COMMANDS.GO_GO:
			this.myAudioHub.PlaySoundCustomDelay(this.goGoGoSFX, DelayAmount);
			break;
		case SWAT_VOICE_COMMANDS.CLEAR:
			this.myAudioHub.PlaySoundCustomDelay(this.clearSFX, DelayAmount);
			break;
		}
	}

	public void TriggerFootSteps(float InitalDelayAmount, int FootStepCount, float FootStepDelay)
	{
		this.footStepCount = FootStepCount;
		this.footStepDelay = FootStepDelay;
		this.initalDelay = InitalDelayAmount;
		this.delayFootStepActiveTimeStamp = Time.time;
		this.delayFootStepActive = true;
	}

	public void TriggerFootSteps(int FootStepCount, float FootStepDelay)
	{
		this.footStepCount = FootStepCount;
		this.footStepDelay = FootStepDelay;
		this.footStepTimeStamp = Time.time;
		this.footStepActive = true;
	}

	public void TakeOverCamera()
	{
		this.cameraIControl.transform.SetParent(this.cameraHolder);
	}

	public void GoToTarget(Vector3 SetPOS)
	{
		this.myNavMeshAgent.enabled = true;
		this.myNavMeshAgent.SetDestination(SetPOS);
	}

	public void ChargePlayer()
	{
		this.myAgentLinkMover.CrossSpeed = 1f;
		this.myCapCollider.enabled = true;
		this.myNavMeshAgent.acceleration = 12f;
		this.myNavMeshAgent.speed = 4f;
		this.myNavMeshAgent.angularSpeed = 240f;
		this.myNavMeshAgent.autoTraverseOffMeshLink = false;
		this.myNavMeshAgent.enabled = true;
		this.playerLookAtMeActive = true;
		this.myNavMeshAgent.SetDestination(roamController.Ins.transform.position);
	}

	public void OnFootDown()
	{
		this.playFootStepSound();
	}

	public void BeginWalkCycle(Vector3 TargetDestination, float Speed)
	{
		this.myNavMeshAgent.acceleration = Speed * 0.5f;
		this.myNavMeshAgent.speed = Speed;
		this.BeginWalkCycle(TargetDestination);
	}

	public void BeginWalkCycle(Vector3 TargetDestination)
	{
		this.ReachedEndPoint.Event += this.EndWalkCycle;
		this.TriggerAnim("walk");
		this.GoToTarget(TargetDestination);
		this.destInProgress = true;
	}

	public void EndWalkCycle()
	{
		this.ReachedEndPoint.Event -= this.EndWalkCycle;
		this.destInProgress = false;
		this.TriggerAnim("crouchIdle1");
		this.lookAtPlayerActive = true;
	}

	public void TossFlashBang()
	{
		this.flashBangObject.transform.SetParent(null);
		this.flashBangObject.GetComponent<Rigidbody>().isKinematic = false;
		this.flashBangObject.GetComponent<Rigidbody>().AddForce(this.flashBangDirection * this.flashBangForce, ForceMode.VelocityChange);
		this.flashBangBehaviour.Thrown();
	}

	public void TriggerBodyHit()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.FloorHit);
	}

	public void TriggerGotYou()
	{
		this.TriggerVoiceCommand(SWAT_VOICE_COMMANDS.GOT_YOU, 0f);
	}

	private void playFootStepSound()
	{
		int num = UnityEngine.Random.Range(1, this.footStepSFXs.Length);
		AudioFileDefinition audioFileDefinition = this.footStepSFXs[num];
		this.footAudioHub.PlaySoundWithWildPitch(this.footStepSFXs[num], 0.5f, 1.25f);
		this.footStepSFXs[num] = this.footStepSFXs[0];
		this.footStepSFXs[0] = audioFileDefinition;
	}

	private void Update()
	{
		if (this.delayFootStepActive && Time.time - this.delayFootStepActiveTimeStamp >= this.initalDelay)
		{
			this.delayFootStepActive = false;
			this.footStepTimeStamp = Time.time;
			this.footStepActive = true;
		}
		if (this.footStepActive && Time.time - this.footStepTimeStamp >= this.footStepDelay)
		{
			this.footStepTimeStamp = Time.time;
			this.playFootStepSound();
			this.footStepCount--;
			if (this.footStepCount <= 0)
			{
				this.footStepActive = false;
			}
		}
		if (this.lookAtPlayerActive)
		{
			Vector3 forward = this.cameraIControl.transform.position - base.transform.position;
			forward.y = 0f;
			Quaternion b = Quaternion.LookRotation(forward);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, this.lookAtPlayerSpeed * Time.deltaTime);
		}
		if (this.playerLookAtMeActive)
		{
			PoliceRoamJumper.Ins.TriggerConstantLookAt(this.lookAtObject.position);
			PoliceRoamJumper.Ins.TriggerCameraConstantLookAt(this.lookAtObject.position);
		}
		if (this.cameraLookAtMeActive)
		{
			PoliceRoamJumper.Ins.TriggerCameraConstantLookAt(this.lookAtObject.position);
		}
		if (this.destInProgress && this.myNavMeshAgent.enabled)
		{
			if (this.myNavMeshAgent.hasPath)
			{
				this.hadPathPreviousFrame = true;
			}
			else if (this.hadPathPreviousFrame)
			{
				this.hadPathPreviousFrame = false;
				this.ReachedEndPoint.Execute();
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		this.myNavMeshAgent.enabled = false;
		this.myCapCollider.enabled = false;
		this.TakeOverCamera();
		this.cameraIControl.transform.localPosition = Vector3.zero;
		this.TriggerAnim("tackle");
		this.cameraLookAtMeActive = true;
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.BodyHit);
	}

	private void OnDrawGizmos()
	{
	}

	[SerializeField]
	private float lookAtPlayerSpeed = 1f;

	[SerializeField]
	private SkinnedMeshRenderer mySkinMesh;

	[SerializeField]
	private SkinnedMeshRenderer myGunMesh;

	[SerializeField]
	private GameObject flashBangObject;

	[SerializeField]
	private Light gunLight;

	[SerializeField]
	private Transform cameraHolder;

	[SerializeField]
	private Transform lookAtObject;

	[SerializeField]
	private AudioHubObject myAudioHub;

	[SerializeField]
	private AudioHubObject footAudioHub;

	[SerializeField]
	private AudioFileDefinition[] footStepSFXs = new AudioFileDefinition[0];

	[SerializeField]
	private AudioFileDefinition getDownSFX;

	[SerializeField]
	private AudioFileDefinition policeDeptSFX;

	[SerializeField]
	private AudioFileDefinition stayDownSFX;

	[SerializeField]
	private AudioFileDefinition gotYouNowSFX;

	[SerializeField]
	private AudioFileDefinition goGoGoSFX;

	[SerializeField]
	private AudioFileDefinition clearSFX;

	[SerializeField]
	private FlashBangBehaviour flashBangBehaviour;

	[SerializeField]
	private Vector3 flashBangDirection;

	[SerializeField]
	private float flashBangForce = 5f;

	private Animator myAC;

	public CustomEvent ReachedEndPoint = new CustomEvent(2);

	private bool delayFootStepActive;

	private bool footStepActive;

	private bool lookAtPlayerActive;

	private bool playerLookAtMeActive;

	private bool cameraLookAtMeActive;

	private bool destInProgress;

	private bool hadPathPreviousFrame;

	private float delayFootStepActiveTimeStamp;

	private float initalDelay;

	private float footStepTimeStamp;

	private float footStepDelay;

	private int footStepCount;

	private Camera cameraIControl;

	private NavMeshAgent myNavMeshAgent;

	private AgentLinkMover myAgentLinkMover;

	private CapsuleCollider myCapCollider;

	private MeshRenderer flashBangMesh;
}
