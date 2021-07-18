using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
[RequireComponent(typeof(AudioHubObject))]
[RequireComponent(typeof(BoxCollider))]
public class FakeDoorTrigger : MonoBehaviour
{
	private void openDoorAction()
	{
		this.myInteractionHook.ForceLock = true;
		this.myAudioHubObject.PlaySound(this.lockedDoorSFX);
		GameManager.TimeSlinger.FireTimer(1.2f, delegate()
		{
			this.myInteractionHook.ForceLock = false;
		}, 0);
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myAudioHubObject = base.GetComponent<AudioHubObject>();
		this.myInteractionHook.LeftClickAction += this.openDoorAction;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.openDoorAction;
	}

	[SerializeField]
	private AudioFileDefinition lockedDoorSFX;

	private InteractionHook myInteractionHook;

	private AudioHubObject myAudioHubObject;
}
