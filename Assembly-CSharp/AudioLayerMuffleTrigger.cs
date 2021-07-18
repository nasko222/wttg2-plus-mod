using System;
using UnityEngine;

public class AudioLayerMuffleTrigger : MonoBehaviour
{
	public float MuffleAmount
	{
		get
		{
			return this.muffleAmount;
		}
		set
		{
			this.muffleAmount = value;
		}
	}

	public void MuffleLayer()
	{
		if (StateManager.PlayerLocation == this.validLocationForMuffle)
		{
			for (int i = 0; i < this.muffleLayers.Length; i++)
			{
				GameManager.AudioSlinger.MuffleAudioLayer(this.muffleLayers[i], this.muffleAmount, this.muffleTime);
			}
		}
		GameManager.TimeSlinger.FireTimer(this.muffleTime + 0.5f, delegate()
		{
			if (StateManager.PlayerLocation != this.validLocationForMuffle)
			{
				for (int j = 0; j < this.muffleLayers.Length; j++)
				{
					GameManager.AudioSlinger.UnMuffleAudioLayer(this.muffleLayers[j], 0f);
				}
			}
		}, 0);
	}

	public void UnMuffleLayer()
	{
		if (StateManager.PlayerLocation == this.validLocationForUnMuffle)
		{
			for (int i = 0; i < this.muffleLayers.Length; i++)
			{
				GameManager.AudioSlinger.UnMuffleAudioLayer(this.muffleLayers[i], this.muffleTime);
			}
		}
	}

	[SerializeField]
	private AUDIO_LAYER[] muffleLayers = new AUDIO_LAYER[0];

	[SerializeField]
	private float muffleTime;

	[SerializeField]
	private float muffleAmount;

	[SerializeField]
	private PLAYER_LOCATION validLocationForMuffle;

	[SerializeField]
	private PLAYER_LOCATION validLocationForUnMuffle;
}
