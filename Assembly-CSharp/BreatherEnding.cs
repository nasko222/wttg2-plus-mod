using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BreatherEnding : MonoBehaviour
{
	private void Awake()
	{
		this.myAC = base.GetComponent<Animator>();
	}

	private void Start()
	{
		this.aniTimeStamp = Time.time;
	}

	private void Update()
	{
		if (Time.time - this.aniTimeStamp >= 20f)
		{
			int num = UnityEngine.Random.Range(0, 10);
			this.aniTimeStamp = Time.time;
			if (num < 4)
			{
				this.myAC.SetTrigger("triggerFidget");
			}
		}
	}

	private Animator myAC;

	private float aniTimeStamp;
}
