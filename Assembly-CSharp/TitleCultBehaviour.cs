using System;
using UnityEngine;

public class TitleCultBehaviour : MonoBehaviour
{
	public void TriggerLoopIdle()
	{
		this.myAC.SetTrigger("triggerLoopIdle");
	}

	private void generateFireTime()
	{
		this.fireWindow = UnityEngine.Random.Range(this.fireMin, this.fireMin);
		this.fireTimeStamp = Time.time;
		this.fireActive = true;
	}

	private void doHeadTilt()
	{
		int num = UnityEngine.Random.Range(1, 11);
		if (num <= 5)
		{
			this.myAC.SetTrigger("headTiltTrigger");
		}
		else
		{
			this.myAC.SetTrigger("headTiltTrigger2");
		}
		this.generateFireTime();
	}

	private void Awake()
	{
		this.myAC = base.GetComponent<Animator>();
		TitleCultBehaviour.Ins = this;
		TitleManager.Ins.TitlePresented.Event += this.generateFireTime;
	}

	private void Update()
	{
		if (this.fireActive && Time.time - this.fireTimeStamp >= this.fireWindow)
		{
			this.fireActive = false;
			this.doHeadTilt();
		}
	}

	private void OnDestroy()
	{
		TitleManager.Ins.TitlePresented.Event -= this.generateFireTime;
	}

	public static TitleCultBehaviour Ins;

	[SerializeField]
	private float fireMin = 10f;

	[SerializeField]
	private float fireMax = 20f;

	private Animator myAC;

	private float fireTimeStamp;

	private float fireWindow;

	private bool fireActive;
}
