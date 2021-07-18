using System;
using UnityEngine;

public class PlayerAudioBehaviour : MonoBehaviour
{
	public void ListenToPlayer()
	{
		if (this.hasMic)
		{
			GameManager.MicManager.ListenToPlayer();
		}
	}

	public void StopListeningToPlayer()
	{
		if (this.hasMic)
		{
			GameManager.MicManager.StopListeningToPlayer();
		}
	}

	private void playerHasNoMic()
	{
		this.hasMic = false;
	}

	private void playerHasMic()
	{
		this.hasMic = true;
	}

	private void playersCurrentDBLevel(int DBLevel)
	{
		if (this.hasMic)
		{
			float value = Mathf.Clamp((float)DBLevel, 0f, (float)this.maxMicThreshold) / (float)this.maxMicThreshold;
			this.CurrentPlayersLoudLevel.Execute(value);
		}
	}

	private void playerHitPause()
	{
		this.StopListeningToPlayer();
	}

	private void playerHitUnPause()
	{
		this.ListenToPlayer();
	}

	private void Awake()
	{
		GameManager.BehaviourManager.PlayerAudioBehaviour = this;
		GameManager.MicManager.MicFound += this.playerHasMic;
		GameManager.MicManager.NoMic += this.playerHasNoMic;
		GameManager.MicManager.GetMicDB += this.playersCurrentDBLevel;
	}

	private void Start()
	{
		GameManager.PauseManager.GamePaused += this.playerHitPause;
		GameManager.PauseManager.GameUnPaused += this.playerHitUnPause;
	}

	private void OnDestroy()
	{
	}

	public CustomEvent<float> CurrentPlayersLoudLevel = new CustomEvent<float>(5);

	[SerializeField]
	private int maxMicThreshold = 20;

	private bool hasMic;
}
