using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CultFemaleEnding : MonoBehaviour
{
	public void WalkBehindPlayer()
	{
		this.lockOut = true;
		if (this.inIdle2)
		{
			this.myAC.SetTrigger("exitIdle2");
			GameManager.TimeSlinger.FireTimer(1f, delegate()
			{
				this.myAC.SetTrigger("walkBehindPlayer");
			}, 0);
		}
		else
		{
			this.myAC.SetTrigger("walkBehindPlayer");
		}
	}

	public void TriggerFootSound()
	{
		int num = UnityEngine.Random.Range(1, this.footStepSFXs.Length);
		AudioFileDefinition audioFileDefinition = this.footStepSFXs[num];
		this.footHub.PlaySound(this.footStepSFXs[num]);
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
		CultFemaleEnding.Ins = this;
		this.myAC = base.GetComponent<Animator>();
		this.inIdle2 = false;
		this.aniTimeStamp = Time.time;
		this.aniWindow = UnityEngine.Random.Range(15f, 45f);
	}

	private void Update()
	{
		if (!this.lockOut && Time.time - this.aniTimeStamp >= this.aniWindow)
		{
			this.aniTimeStamp = Time.time;
			this.aniWindow = UnityEngine.Random.Range(15f, 45f);
			if (!this.inIdle2)
			{
				int num = UnityEngine.Random.Range(0, 10);
				if (num < 2)
				{
					this.myAC.SetTrigger("fidget1");
				}
				else if (num >= 2 && num < 5)
				{
					this.myAC.SetTrigger("fidget2");
				}
				else if (num >= 5 && num < 8)
				{
					this.myAC.SetTrigger("fidget3");
				}
				else
				{
					this.myAC.SetTrigger("idle2");
					this.inIdle2 = true;
				}
			}
			else
			{
				this.myAC.SetTrigger("exitIdle2");
				this.inIdle2 = false;
			}
		}
	}

	public static CultFemaleEnding Ins;

	[SerializeField]
	private AudioHubObject footHub;

	[SerializeField]
	private AudioFileDefinition[] footStepSFXs = new AudioFileDefinition[0];

	[SerializeField]
	private SkinnedMeshRenderer[] rends = new SkinnedMeshRenderer[0];

	private Animator myAC;

	private float aniTimeStamp;

	private float aniWindow;

	private bool inIdle2;

	private bool lockOut;
}
