using System;
using UnityEngine;
using UnityEngine.UI;

public class ComputerMuteBehaviour : MonoBehaviour
{
	public bool Muted
	{
		get
		{
			return this.muted;
		}
	}

	public void ToggleMute()
	{
		if (this.muted)
		{
			this.muted = false;
			GameManager.AudioSlinger.UnMuteAudioLayer(AUDIO_LAYER.WEBSITE);
			this.soundIcon.sprite = this.unmutedSprite;
		}
		else
		{
			this.muted = true;
			GameManager.AudioSlinger.MuteAudioLayer(AUDIO_LAYER.WEBSITE);
			this.soundIcon.sprite = this.mutedSprite;
		}
	}

	private void trollUnMute()
	{
		if (this.muted && !StateManager.BeingHacked)
		{
			this.muted = false;
			GameManager.AudioSlinger.UnMuteAudioLayer(AUDIO_LAYER.WEBSITE);
			this.soundIcon.sprite = this.unmutedSprite;
		}
		this.generateTrollWindow();
	}

	private void generateTrollWindow()
	{
		this.fireTimeWindow = UnityEngine.Random.Range(this.fireMin, this.fireMax);
		this.fireTimeStamp = Time.time;
		this.fireActive = true;
	}

	private void gameLive()
	{
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
		this.generateTrollWindow();
	}

	private void Awake()
	{
		ComputerMuteBehaviour.Ins = this;
		GameManager.StageManager.TheGameIsLive += this.gameLive;
	}

	private void Update()
	{
		if (this.fireActive && Time.time - this.fireTimeStamp >= this.fireTimeWindow)
		{
			this.fireActive = false;
			this.trollUnMute();
		}
	}

	public static ComputerMuteBehaviour Ins;

	[SerializeField]
	private float fireMin;

	[SerializeField]
	private float fireMax;

	[SerializeField]
	private Image soundIcon;

	[SerializeField]
	private Sprite mutedSprite;

	[SerializeField]
	private Sprite unmutedSprite;

	private bool muted;

	private bool fireActive;

	private float fireTimeStamp;

	private float fireTimeWindow;
}
