using System;

public class AudioLayerObject
{
	public AudioLayerObject(AUDIO_LAYER setAudioLayer)
	{
		this.MyAudioLayer = setAudioLayer;
		this.IAmMuffled = false;
		this.MuffledAMT = 0f;
	}

	public AudioLayerObject(AUDIO_LAYER setAudioLayer, bool setAmMuffled, float setMuffleAMT)
	{
		this.MyAudioLayer = setAudioLayer;
		this.IAmMuffled = setAmMuffled;
		this.MuffledAMT = setMuffleAMT;
	}

	public void MuffleMe(float setAMT, float fadeTime = 0f)
	{
		this.IAmMuffled = true;
		this.MuffledAMT = setAMT;
		this.MuffleSound.Execute(fadeTime);
	}

	public void UnMuffleMe(float fadeTime = 0f)
	{
		this.IAmMuffled = false;
		this.MuffledAMT = 0f;
		this.MuffleSound.Execute(fadeTime);
	}

	public void PauseMe()
	{
		this.IAmPaused = true;
		this.TriggerPause.Execute();
	}

	public void UnPauseMe()
	{
		this.IAmPaused = false;
		this.TriggerPause.Execute();
	}

	public void MuteMe()
	{
		this.IAmMuted = true;
		this.TriggerMute.Execute();
	}

	public void UnMuteMe()
	{
		this.IAmMuted = false;
		this.TriggerMute.Execute();
	}

	public void KillMe()
	{
		this.IWasDestroyed.Execute();
	}

	public AUDIO_LAYER MyAudioLayer;

	public bool IAmMuffled;

	public bool IAmPaused;

	public bool IAmMuted;

	public float MuffledAMT;

	public CustomEvent IWasDestroyed = new CustomEvent(10);

	public CustomEvent TriggerMute = new CustomEvent(10);

	public CustomEvent TriggerPause = new CustomEvent(10);

	public CustomEvent<float> MuffleSound = new CustomEvent<float>(10);

	public CustomEvent<float> UnMuffleSound = new CustomEvent<float>(10);
}
