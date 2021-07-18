using System;
using UnityEngine;

public class CultFemaleBehaviour : MonoBehaviour
{
	public Vector3 LookAtPosition
	{
		get
		{
			return this.lookAtObject.position;
		}
	}

	public Transform CameraBone
	{
		get
		{
			return this.cameraBone;
		}
	}

	public void TriggerAnim(string SetAnim)
	{
		this.myAC.SetTrigger(SetAnim);
	}

	public void EnableHammerMesh()
	{
		this.hammerMesh.enabled = true;
	}

	public void AttemptSpawnBehindPlayer()
	{
		this.mySpawner.InMeshEvents.Event += this.notValidSpawnLocation;
		this.mySpawner.NotInMeshEvents.Event += this.validSpawnLocation;
		this.mySpawner.SpawnBehindPlayer(roamController.Ins.transform, this.heightDifference);
	}

	public void HammerJump()
	{
		this.EnableHammerMesh();
		GameManager.TimeSlinger.FireTimer(0.5f, delegate()
		{
			this.TriggerVoiceCommand(CULT_FEMALE_VOICE_COMMANDS.BOO);
		}, 0);
		GameManager.TimeSlinger.FireTimer(2f, delegate()
		{
			this.TriggerVoiceCommand(CULT_FEMALE_VOICE_COMMANDS.LAUGH1);
		}, 0);
		GameManager.TimeSlinger.FireTimer(3f, delegate()
		{
			this.TriggerVoiceCommand(CULT_FEMALE_VOICE_COMMANDS.LAUGH1);
		}, 0);
		this.TriggerAnim("triggerHammerJump");
	}

	public void StageDeskJump()
	{
		this.mySpawner.Spawn(new Vector3(3.103f, 39.585f, -2.343f), new Vector3(0f, 180f, 0f));
		GameManager.TimeSlinger.FireTimer(0.1f, delegate()
		{
			this.EnableHammerMesh();
			this.TriggerAnim("deskJumpIdle");
		}, 0);
	}

	public void TriggerDeskJump()
	{
		this.TriggerAnim("deskJump");
	}

	public void TriggerVoiceCommand(CULT_FEMALE_VOICE_COMMANDS TheCommand)
	{
		if (TheCommand != CULT_FEMALE_VOICE_COMMANDS.BOO)
		{
			if (TheCommand == CULT_FEMALE_VOICE_COMMANDS.LAUGH1)
			{
				this.voiceHub.PlaySound(this.laugh1SFX);
			}
		}
		else
		{
			this.voiceHub.PlaySound(this.booSFX);
		}
	}

	private void notValidSpawnLocation()
	{
		this.mySpawner.InMeshEvents.Event -= this.notValidSpawnLocation;
		this.mySpawner.NotInMeshEvents.Event -= this.validSpawnLocation;
		this.InValidSpawnLocationEvent.Execute();
	}

	private void validSpawnLocation()
	{
		this.mySpawner.InMeshEvents.Event -= this.notValidSpawnLocation;
		this.mySpawner.NotInMeshEvents.Event -= this.validSpawnLocation;
		this.ValidSpawnLocationEvent.Execute();
	}

	public void BodyJump()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.BodyHit);
	}

	public void FloorHit()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.FloorHit);
	}

	public void HammerHit()
	{
		MainCameraHook.Ins.AddHeadHit(1f);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.HeadHit);
	}

	public void Laugh()
	{
		this.voiceHub.PlaySound(this.laugh1SFX);
	}

	private void Awake()
	{
		CultFemaleBehaviour.Ins = this;
		this.hammerMesh.enabled = false;
		this.myAC = base.GetComponent<Animator>();
		this.mySpawner = base.GetComponent<CultSpawner>();
	}

	private void Update()
	{
	}

	public static CultFemaleBehaviour Ins;

	public CustomEvent ValidSpawnLocationEvent = new CustomEvent(2);

	public CustomEvent InValidSpawnLocationEvent = new CustomEvent(2);

	[SerializeField]
	private SkinnedMeshRenderer hammerMesh;

	[SerializeField]
	private Transform lookAtObject;

	[SerializeField]
	private Transform cameraBone;

	[SerializeField]
	private AudioHubObject voiceHub;

	[SerializeField]
	private AudioFileDefinition booSFX;

	[SerializeField]
	private AudioFileDefinition laugh1SFX;

	[SerializeField]
	private float heightDifference = 1f;

	private Animator myAC;

	private CultSpawner mySpawner;
}
