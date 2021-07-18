using System;
using UnityEngine;

public class KitchenWindowHook : MonoBehaviour
{
	public void OpenWindow()
	{
		StateManager.PlayerStateChangeEvents.Event -= this.OpenWindow;
		if (StateManager.PlayerState == PLAYER_STATE.COMPUTER)
		{
			base.transform.localPosition = this.openPOS;
			GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.OUTSIDE, 0.25f, 0f);
			this.patioDoorALMT.MuffleAmount = 0.25f;
			this.isOpen = true;
			return;
		}
		StateManager.PlayerStateChangeEvents.Event += this.OpenWindow;
	}

	private void keyWasDiscovered()
	{
		this.curKeyCount += 1;
		if ((int)this.curKeyCount == this.keysFoundCount)
		{
			this.OpenWindow();
		}
	}

	private void Awake()
	{
		this.closedPOS = base.transform.localPosition;
		KitchenWindowHook.Ins = this;
		GameManager.TheCloud.KeyDiscoveredEvent.Event += this.keyWasDiscovered;
	}

	public void CloseWindow()
	{
		StateManager.PlayerStateChangeEvents.Event -= this.CloseWindow;
		if (StateManager.PlayerState == PLAYER_STATE.COMPUTER)
		{
			base.transform.localPosition = this.closedPOS;
			GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.OUTSIDE, 0f, 0f);
			this.patioDoorALMT.MuffleAmount = 0f;
			this.isOpen = false;
			return;
		}
		StateManager.PlayerStateChangeEvents.Event += this.CloseWindow;
	}

	public static KitchenWindowHook Ins;

	[SerializeField]
	private AudioLayerMuffleTrigger patioDoorALMT;

	[SerializeField]
	private Vector3 openPOS;

	[SerializeField]
	private int keysFoundCount = 3;

	private short curKeyCount;

	public bool isOpen;

	private Vector3 closedPOS;
}
