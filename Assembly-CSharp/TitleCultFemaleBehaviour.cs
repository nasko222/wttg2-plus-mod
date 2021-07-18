using System;
using UnityEngine;

public class TitleCultFemaleBehaviour : MonoBehaviour
{
	private void triggerWalkUp()
	{
		this.aniTriggerTimeStamp = Time.time;
		this.aniTriggerActive = true;
	}

	private void generateHeadTiltFire()
	{
		this.headTiltWindow = UnityEngine.Random.Range(this.fireMin, this.fireMax);
		this.headTiltTimeStamp = Time.time;
		this.headTiltActive = true;
	}

	private void Awake()
	{
		this.myAC = base.GetComponent<Animator>();
		this.mySkinMesh.enabled = false;
		TitleManager.Ins.TitlePresent.Event += this.triggerWalkUp;
	}

	private void Update()
	{
		if (this.aniTriggerActive && Time.time - this.aniTriggerTimeStamp >= 3f)
		{
			this.aniTriggerActive = false;
			this.myAC.SetTrigger("triggerWalkUp");
			this.walkUpTimeStamp = Time.time;
			this.walkUpActive = true;
		}
		if (this.walkUpActive && Time.time - this.walkUpTimeStamp >= 1f)
		{
			this.walkUpActive = false;
			this.mySkinMesh.enabled = true;
			this.generateHeadTiltFire();
		}
		if (this.headTiltActive && Time.time - this.headTiltTimeStamp >= this.headTiltWindow)
		{
			this.headTiltActive = false;
			this.myAC.SetTrigger("triggerHeadTilt");
			this.generateHeadTiltFire();
		}
	}

	private void OnDestroy()
	{
		TitleManager.Ins.TitlePresent.Event -= this.triggerWalkUp;
	}

	[SerializeField]
	private SkinnedMeshRenderer mySkinMesh;

	[SerializeField]
	private float fireMin = 10f;

	[SerializeField]
	private float fireMax = 30f;

	private Animator myAC;

	private float walkUpTimeStamp;

	private float aniTriggerTimeStamp;

	private float headTiltWindow;

	private float headTiltTimeStamp;

	private bool walkUpActive;

	private bool aniTriggerActive;

	private bool headTiltActive;
}
