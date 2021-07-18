using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class DollMakerBehaviour : MonoBehaviour
{
	public Transform ManikinTransform
	{
		get
		{
			return this.manikin.transform;
		}
	}

	public Transform HelperBone
	{
		get
		{
			return this.helperBone;
		}
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
		Vector3 position = TargetTransform.position - TargetTransform.forward * 0.85f;
		position.y -= YOffSet;
		base.transform.position = position;
		base.transform.rotation = TargetTransform.rotation;
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = true;
		}
		this.knife.enabled = true;
	}

	public void SpawnBehindDesk()
	{
		this.TriggerAnim("triggerDeskJumpIdle");
		base.transform.position = new Vector3(2.013f, 39.5846f, -3.731f);
		base.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = true;
		}
		this.knife.enabled = true;
	}

	public void TriggerAnim(string SetTrigger)
	{
		this.myAC.SetTrigger(SetTrigger);
	}

	public void StageDoorSpawn()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = true;
		}
		this.manikin.enabled = true;
		this.TriggerAnim("triggerDollGrabIdle");
		base.transform.position = this.doorSpawnPOS;
		base.transform.rotation = Quaternion.Euler(this.doorSpawnROT);
		this.manikin.transform.localRotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
	}

	public void StageSpeech()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = true;
		}
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit5);
		this.TriggerAnim("triggerSpeechIdle");
		this.knife.enabled = true;
		Vector3 position = roamController.Ins.transform.position - roamController.Ins.transform.forward * 1f;
		position.y = this.doorSpawnPOS.y;
		base.transform.position = position;
		base.transform.rotation = roamController.Ins.transform.rotation;
	}

	public void TriggerSpeech()
	{
		this.TriggerAnim("triggerSpeechStart");
	}

	public void TriggerUniJump()
	{
		this.TriggerAnim("triggerUniJump");
	}

	public void TriggerDeskJump()
	{
		this.TriggerAnim("triggerDeskJump");
	}

	public void DeSpawn()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = false;
		}
		this.knife.enabled = false;
		this.manikin.enabled = false;
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.manikin.transform.localPosition = Vector3.zero;
		this.manikin.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	public void TriggerFootSound()
	{
		int num = UnityEngine.Random.Range(1, this.footStepSFXs.Length);
		AudioFileDefinition audioFileDefinition = this.footStepSFXs[num];
		this.footHub.PlaySoundWithWildPitch(this.footStepSFXs[num], 0.9f, 1f);
		this.footStepSFXs[num] = this.footStepSFXs[0];
		this.footStepSFXs[0] = audioFileDefinition;
	}

	public void TriggerPower()
	{
		EnemyManager.DollMakerManager.DoorPowerTrip();
	}

	public void TriggerTheTalk()
	{
		this.hardVoiceSource.clip = this.theTalkClip;
		this.hardVoiceSource.Play();
		GameManager.TimeSlinger.FireTimer(58f, new Action(this.endSpeech), 0);
		this.fidgetTimeWindow = UnityEngine.Random.Range(7f, 16f);
		this.fidgetTimeStamp = Time.time;
		this.fidgetActive = true;
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.hardVoiceSource.transform.localPosition, delegate(Vector3 x)
		{
			this.hardVoiceSource.transform.localPosition = x;
		}, new Vector3(0.2836f, -0.2156f, 0.328f), 2f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.hardVoiceSource.panStereo, delegate(float x)
		{
			this.hardVoiceSource.panStereo = x;
		}, 1f, 2.4f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void TriggerDisapointMe()
	{
		this.voiceHub.PlaySound(this.disapointMeClip);
	}

	public void TriggerStab()
	{
		MainCameraHook.Ins.AddHeadHit(0f);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KnifeStab2);
	}

	public void TriggerSlash()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KnifeStab1);
		MainCameraHook.Ins.FadeDoubleVis(2f, 3f);
	}

	public void FloorHit()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.FloorHit);
	}

	private void endSpeech()
	{
		this.TriggerAnim("triggerEndSpeech");
		GameManager.AudioSlinger.PlaySoundWithCustomDelay(LookUp.SoundLookUp.HeadHit, 0.75f);
		GameManager.TimeSlinger.FireTimer(1f, delegate()
		{
			DollMakerRoamJumper.Ins.ClearSpeechJump();
		}, 0);
		GameManager.TimeSlinger.FireTimer(2f, delegate()
		{
			this.DeSpawn();
			LookUp.Doors.Door8.ForceOpenDoor();
		}, 0);
		GameManager.TimeSlinger.FireTimer(4.5f, delegate()
		{
			this.TriggerAnim("triggerClear");
			EnemyManager.DollMakerManager.ClearWarningTrigger();
		}, 0);
	}

	private void Awake()
	{
		DollMakerBehaviour.Ins = this;
		this.myAC = base.GetComponent<Animator>();
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = false;
		}
		this.manikin.enabled = false;
		this.knife.enabled = false;
	}

	private void Update()
	{
		if (this.fidgetActive && Time.time - this.fidgetTimeStamp >= this.fidgetTimeWindow)
		{
			this.fidgetActive = false;
			int num = UnityEngine.Random.Range(0, 10);
			if (num < 5)
			{
				this.TriggerAnim("fidget1");
			}
			else
			{
				this.TriggerAnim("fidget2");
			}
			this.fidgetTimeWindow = UnityEngine.Random.Range(7f, 16f);
			this.fidgetTimeStamp = Time.time;
			this.fidgetActive = true;
		}
		if (this.inMeshCheckActive && Time.time - this.inMeshCheckTimeStamp >= 0.05f)
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
		this.inMesh = true;
	}

	private void OnTriggerStay(Collider other)
	{
		this.inMesh = true;
	}

	public static DollMakerBehaviour Ins;

	public CustomEvent InMeshEvents = new CustomEvent(2);

	public CustomEvent NotInMeshEvents = new CustomEvent(2);

	[SerializeField]
	private Vector3 doorSpawnPOS = Vector3.zero;

	[SerializeField]
	private Vector3 doorSpawnROT = Vector3.zero;

	[SerializeField]
	private Transform helperBone;

	[SerializeField]
	private AudioHubObject footHub;

	[SerializeField]
	private AudioHubObject voiceHub;

	[SerializeField]
	private AudioSource hardVoiceSource;

	[SerializeField]
	private AudioClip theTalkClip;

	[SerializeField]
	private AudioFileDefinition disapointMeClip;

	[SerializeField]
	private SkinnedMeshRenderer[] renderers = new SkinnedMeshRenderer[0];

	[SerializeField]
	private SkinnedMeshRenderer knife;

	[SerializeField]
	private MeshRenderer manikin;

	[SerializeField]
	private AudioFileDefinition[] footStepSFXs = new AudioFileDefinition[0];

	private Animator myAC;

	private bool fidgetActive;

	private bool inMesh;

	private bool inMeshCheckActive;

	private float fidgetTimeStamp;

	private float fidgetTimeWindow;

	private float inMeshCheckTimeStamp;
}
