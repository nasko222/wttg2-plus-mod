using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CultMaleEnding : MonoBehaviour
{
	public void WalkBehindPlayer()
	{
		this.lockOut = true;
		this.myAC.SetTrigger("triggerWalkBehindPlayer");
	}

	public void TriggerFootSound()
	{
		int num = UnityEngine.Random.Range(1, this.footStepSFXs.Length);
		AudioFileDefinition audioFileDefinition = this.footStepSFXs[num];
		this.footHub.PlaySoundWithWildPitch(this.footStepSFXs[num], 1.1f, 1.2f);
		this.footStepSFXs[num] = this.footStepSFXs[0];
		this.footStepSFXs[0] = audioFileDefinition;
	}

	public void DeSpawn()
	{
		this.lockOut = true;
		for (int i = 0; i < this.rends.Length; i++)
		{
			this.rends[i].enabled = false;
		}
	}

	private void Awake()
	{
		CultMaleEnding.Ins = this;
		this.myAC = base.GetComponent<Animator>();
	}

	private void Start()
	{
		this.aniTimeStamp = Time.time;
	}

	private void Update()
	{
		if (!this.lockOut && Time.time - this.aniTimeStamp >= 30f)
		{
			int num = UnityEngine.Random.Range(0, 10);
			this.aniTimeStamp = Time.time;
			if (num < 3)
			{
				int num2 = UnityEngine.Random.Range(0, 10);
				if (num2 < 5)
				{
					this.myAC.SetTrigger("triggerHeadTilt");
				}
				else
				{
					this.myAC.SetTrigger("triggerNeckCrack");
				}
			}
		}
	}

	public static CultMaleEnding Ins;

	[SerializeField]
	private AudioHubObject footHub;

	[SerializeField]
	private AudioFileDefinition[] footStepSFXs = new AudioFileDefinition[0];

	[SerializeField]
	private SkinnedMeshRenderer[] rends = new SkinnedMeshRenderer[0];

	private Animator myAC;

	private float aniTimeStamp;

	private bool lockOut;
}
