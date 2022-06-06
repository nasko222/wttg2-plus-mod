using System;
using UnityEngine;

public class TarotCardsBehaviour : MonoBehaviour
{
	public void ShowInteractionIcon()
	{
		UIInteractionManager.Ins.ShowHoldMode();
		UIInteractionManager.Ins.ShowLeftMouseButtonAction();
	}

	public void HideInteractionIcon()
	{
		UIInteractionManager.Ins.HideHoldMode();
		UIInteractionManager.Ins.HideLeftMouseButtonAction();
	}

	public void SoftBuild()
	{
		TarotCardsBehaviour.Ins = this;
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
		for (int i = 0; i < this.myMeshRenderer.Length; i++)
		{
			this.myMeshRenderer[i].enabled = false;
		}
		this.myInteractionHook.LeftClickAction += this.leftClickAction;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.leftClickAction;
		Debug.Log("Tarot Card Module Unloaded!");
	}

	private void leftClickAction()
	{
		this.myInteractionHook.ForceLock = true;
		TarotCardPullAnim.Ins.DoPull();
		UIInteractionManager.Ins.HideHoldMode();
		UIInteractionManager.Ins.HideLeftMouseButtonAction();
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.ReEnableLeftClick), 0);
	}

	public void MoveMe(Vector3 SetPOS, Vector3 SetROT, Vector3 SetSCL)
	{
		TarotCardsBehaviour.Owned = true;
		for (int i = 0; i < this.myMeshRenderer.Length; i++)
		{
			this.myMeshRenderer[i].enabled = true;
		}
		base.transform.position = SetPOS;
		base.transform.rotation = Quaternion.Euler(SetROT);
		base.transform.localScale = SetSCL;
	}

	private void ReEnableLeftClick()
	{
		if (TarotCardPullAnim.currentCard < 10)
		{
			this.myInteractionHook.ForceLock = false;
		}
	}

	public InteractionHook myInteractionHook;

	public AudioHubObject myAudioHub;

	public static TarotCardsBehaviour Ins;

	public MeshRenderer[] myMeshRenderer;

	public static bool Owned;
}
