using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class BlueWhisperBehaviour : MonoBehaviour
{
	public void SpawnMe()
	{
		this.myMeshRenderer.enabled = true;
		this.myInteractionHook.ForceLock = false;
	}

	private void pickUpAction()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPickUp2);
		this.myInteractionHook.ForceLock = true;
		this.myMeshRenderer.enabled = false;
		BlueWhisperManager.Ins.PickedUpBlueWhisper();
	}

	private void stageMe()
	{
		this.myMeshRenderer.enabled = false;
		this.myInteractionHook.ForceLock = true;
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		BlueWhisperBehaviour.Ins = this;
		this.myMeshRenderer = base.GetComponent<MeshRenderer>();
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		GameManager.StageManager.Stage += this.stageMe;
		this.myInteractionHook.LeftClickAction += this.pickUpAction;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.pickUpAction;
	}

	public static BlueWhisperBehaviour Ins;

	private MeshRenderer myMeshRenderer;

	private InteractionHook myInteractionHook;
}
