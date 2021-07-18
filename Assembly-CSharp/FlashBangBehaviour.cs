using System;
using UnityEngine;

[RequireComponent(typeof(AudioHubObject))]
[RequireComponent(typeof(Rigidbody))]
public class FlashBangBehaviour : MonoBehaviour
{
	public void Thrown()
	{
		this.lookAtMeActive = true;
	}

	private void boom()
	{
		this.myMesnRenderer.enabled = false;
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.FlashBang);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.EarRing);
		UIManager.FlashPlayer();
		MainCameraHook.Ins.TriggerFlashBlur();
	}

	private void Awake()
	{
		this.myMesnRenderer = base.GetComponent<MeshRenderer>();
		this.myRigidBody = base.GetComponent<Rigidbody>();
		this.myAudioHub = base.GetComponent<AudioHubObject>();
	}

	private void Update()
	{
		if (this.lookAtMeActive)
		{
			PoliceRoamJumper.Ins.TriggerCameraConstantLookAt(base.transform.position);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		this.lookAtMeActive = false;
		if (!this.bangFired)
		{
			this.bangFired = true;
			GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.boom), 0);
		}
	}

	private MeshRenderer myMesnRenderer;

	private Rigidbody myRigidBody;

	private AudioHubObject myAudioHub;

	private bool bangFired;

	private bool lookAtMeActive;
}
