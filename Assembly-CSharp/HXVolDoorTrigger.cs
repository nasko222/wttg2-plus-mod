using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DoorTrigger))]
public class HXVolDoorTrigger : MonoBehaviour
{
	private void doorIsClosing()
	{
		if (StateManager.PlayerLocation == PLAYER_LOCATION.DEAD_DROP_ROOM)
		{
			DOTween.To(() => this.hxLight.Intensity, delegate(float x)
			{
				this.hxLight.Intensity = x;
			}, 0f, this.doorCloseTime);
		}
	}

	private void doorIsOpening()
	{
		if (StateManager.PlayerLocation == PLAYER_LOCATION.DEAD_DROP_ROOM)
		{
			DOTween.To(() => this.hxLight.Intensity, delegate(float x)
			{
				this.hxLight.Intensity = x;
			}, this.defaultHXLightInt, this.doorOpenTime);
		}
	}

	private void Awake()
	{
		this.myDoorTrigger = base.GetComponent<DoorTrigger>();
		this.myDoorTrigger.DoorCloseEvent.AddListener(new UnityAction(this.doorIsClosing));
		this.myDoorTrigger.DoorOpenEvent.AddListener(new UnityAction(this.doorIsOpening));
		this.defaultHXLightInt = this.hxLight.Intensity;
	}

	private void OnDestroy()
	{
		this.myDoorTrigger.DoorCloseEvent.RemoveListener(new UnityAction(this.doorIsClosing));
		this.myDoorTrigger.DoorOpenEvent.RemoveListener(new UnityAction(this.doorIsOpening));
	}

	[SerializeField]
	private float doorCloseTime;

	[SerializeField]
	private float doorOpenTime;

	[SerializeField]
	private HxVolumetricLight hxLight;

	[SerializeField]
	private PLAYER_LOCATION activeLocation;

	private DoorTrigger myDoorTrigger;

	private float defaultHXLightInt;
}
