using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioBehaviour : MonoBehaviour
{
	private void prepLiveGameAudio()
	{
		this.gameIsLive = true;
		for (int i = 0; i < this.preGameLiveHubs.Count; i++)
		{
			this.processNewHub(this.preGameLiveHubs[i]);
		}
		this.preGameLiveHubs.Clear();
	}

	private void processNewHub(AudioHubObject theHub)
	{
		if (this.gameIsLive)
		{
			AUDIO_HUB myAudioHub = theHub.MyAudioHub;
			if (myAudioHub == AUDIO_HUB.COMPUTER_HUB)
			{
				if (!ControllerManager.Get<computerController>(GAME_CONTROLLER.COMPUTER).Active)
				{
					GameManager.AudioSlinger.MuffleAudioHub(AUDIO_HUB.COMPUTER_HUB, 0.6f, 0f);
				}
			}
		}
		else
		{
			this.preGameLiveHubs.Add(theHub);
		}
	}

	private void processNewLayer(AudioLayerObject theLayer)
	{
		AUDIO_LAYER myAudioLayer = theLayer.MyAudioLayer;
		if (myAudioLayer == AUDIO_LAYER.OUTSIDE)
		{
			if (StateManager.PlayerLocation != PLAYER_LOCATION.OUTSIDE)
			{
				GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.OUTSIDE, 0f, 0f);
			}
		}
	}

	private void Awake()
	{
		this.gameIsLive = false;
		GameManager.StageManager.TheGameIsLive += this.prepLiveGameAudio;
		GameManager.AudioSlinger.AddedNewHub += this.processNewHub;
		GameManager.AudioSlinger.AddedNewLayer += this.processNewLayer;
	}

	private void OnDestroy()
	{
	}

	private List<AudioHubObject> preGameLiveHubs = new List<AudioHubObject>();

	private bool gameIsLive;
}
