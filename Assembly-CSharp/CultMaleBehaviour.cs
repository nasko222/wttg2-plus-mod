using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CultMaleBehaviour : MonoBehaviour
{
	public void TriggerAnim(string SetTrigger)
	{
		this.myAC.SetTrigger(SetTrigger);
	}

	public void WasSpawned()
	{
		this.pickIdleAnim();
		this.generateHeadAction();
	}

	public void WasDeSpawned()
	{
		this.TriggerAnim("exitIdle");
		this.headActionActive = false;
	}

	public void StageDeskJump()
	{
		this.TriggerAnim("deskJumpIdle");
		this.mySpawner.Place(new Vector3(3.103f, 39.585f, -2.092f), new Vector3(0f, 180f, 0f));
	}

	public void StageEndJump()
	{
		this.headActionActive = false;
		this.TriggerAnim("stageEndJump");
		this.mySpawner.Place(new Vector3(24.448f, 0f, -6.319f), new Vector3(0f, 90f, 0f));
	}

	public void TriggerDeskJump()
	{
		this.TriggerAnim("deskJump");
	}

	public void BodyHit()
	{
		MainCameraHook.Ins.AddBodyHit();
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.BodyHit);
	}

	public void HeadHit()
	{
		MainCameraHook.Ins.AddHeadHit(2f);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.HeadHit);
	}

	public void FloorHit()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.FloorHit);
	}

	public void TriggerLoadEnding()
	{
		UIManager.TriggerLoadEnding();
	}

	private void generateHeadAction()
	{
		this.headActionTimeWindow = UnityEngine.Random.Range(this.headActionWindowMin, this.headActionWindowMax);
		this.headActionTimeStamp = Time.time;
		this.headActionActive = true;
	}

	private void performHeadAction()
	{
		int num = UnityEngine.Random.Range(0, 2);
		if (num != 0)
		{
			if (num == 1)
			{
				this.TriggerAnim("headTilt");
			}
		}
		else
		{
			this.TriggerAnim("neckCrack");
		}
		this.generateHeadAction();
	}

	private void pickIdleAnim()
	{
		switch (UnityEngine.Random.Range(0, 4))
		{
		case 0:
			this.TriggerAnim("idle1");
			break;
		case 1:
			this.TriggerAnim("idle2");
			break;
		case 2:
			this.TriggerAnim("idle3");
			break;
		case 3:
			this.TriggerAnim("idle4");
			break;
		}
	}

	private void Awake()
	{
		this.myAC = base.GetComponent<Animator>();
		this.mySpawner = base.GetComponent<CultSpawner>();
	}

	private void Update()
	{
		if (this.headActionActive && Time.time - this.headActionTimeStamp >= this.headActionTimeWindow)
		{
			this.headActionActive = false;
			this.performHeadAction();
		}
	}

	[SerializeField]
	private float headActionWindowMin = 6f;

	[SerializeField]
	private float headActionWindowMax = 60f;

	private Animator myAC;

	private float headActionTimeStamp;

	private float headActionTimeWindow;

	private bool headActionActive;

	private CultSpawner mySpawner;
}
