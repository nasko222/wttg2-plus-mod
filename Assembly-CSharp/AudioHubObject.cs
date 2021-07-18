using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AudioHubObject : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AudioHubObject.AudioHubVoidActions IAmAlive;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AudioHubObject.AudioHubMuffleActions MuffleSound;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AudioHubObject.PlayingActions DonePlaying;

	public void PlaySoundCustomDelay(AudioFileDefinition AudioFile, float SetDelay)
	{
		AudioFileDefinition audioFileDefinition = UnityEngine.Object.Instantiate<AudioFileDefinition>(AudioFile);
		audioFileDefinition.Delay = true;
		audioFileDefinition.DelayAmount = SetDelay;
		this.PlaySound(audioFileDefinition);
	}

	public void PlaySound(AudioFileDefinition AudioFile)
	{
		SoundObject soundObject = this.soundObjectsPool.Pop();
		List<SoundObject> list;
		if (!this.mySoundObjects.TryGetValue(AudioFile.AudioClip, out list))
		{
			list = new List<SoundObject>();
			this.mySoundObjects[AudioFile.AudioClip] = list;
		}
		list.Add(soundObject);
		soundObject.enabled = true;
		soundObject.DonePlaying += this.soundObjectDoneAction;
		soundObject.Fire(AudioFile);
	}

	public void PlaySoundWithWildPitch(AudioFileDefinition AudioFile, float MinLevel, float MaxLevel)
	{
		SoundObject soundObject = this.soundObjectsPool.Pop();
		List<SoundObject> list;
		if (!this.mySoundObjects.TryGetValue(AudioFile.AudioClip, out list))
		{
			list = new List<SoundObject>();
			this.mySoundObjects[AudioFile.AudioClip] = list;
		}
		list.Add(soundObject);
		soundObject.enabled = true;
		soundObject.DonePlaying += this.soundObjectDoneAction;
		soundObject.SetCustomPitch(UnityEngine.Random.Range(MinLevel, MaxLevel));
		soundObject.Fire(AudioFile);
	}

	public void KillSound(AudioClip setAudioClip)
	{
		List<SoundObject> list;
		if (this.mySoundObjects.TryGetValue(setAudioClip, out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i].KillMe();
			}
		}
	}

	public void KillMe()
	{
		if (this.IAmDead != null)
		{
			this.IAmDead();
		}
		this.mySoundObjects.Clear();
		this.soundObjectsPool.Clear();
		UnityEngine.Object.Destroy(this);
	}

	public void MuffleHub(float setMuffleAmount, float fadeTime = 0f)
	{
		this.HubMuffled = true;
		this.MuffledPerc = setMuffleAmount;
		if (this.MuffleSound != null)
		{
			this.MuffleSound(fadeTime);
		}
	}

	public void UnMuffleHub(float fadeTime = 0f)
	{
		this.HubMuffled = false;
		if (this.UnMuffleSound != null)
		{
			this.UnMuffleSound(fadeTime);
		}
	}

	public void PauseHub()
	{
		this.HubPaused = true;
		if (this.TriggerPause != null)
		{
			this.TriggerPause();
		}
	}

	public void UnPauseHub()
	{
		this.HubPaused = false;
		if (this.TriggerPause != null)
		{
			this.TriggerPause();
		}
	}

	public void MuteHub()
	{
		this.HubMuted = true;
		if (this.TriggerMute != null)
		{
			this.TriggerMute();
		}
	}

	public void UnMuteHub()
	{
		this.HubMuted = false;
		if (this.TriggerMute != null)
		{
			this.TriggerMute();
		}
	}

	private void RemoveSoundObject(SoundObject soundObjectToRemove)
	{
		soundObjectToRemove.DonePlaying -= this.soundObjectDoneAction;
		if (this.DonePlaying != null)
		{
			this.DonePlaying(soundObjectToRemove.MyAudioFile);
		}
		List<SoundObject> list;
		if (this.mySoundObjects.TryGetValue(soundObjectToRemove.MyAudioFile.AudioClip, out list))
		{
			list.Remove(soundObjectToRemove);
			this.soundObjectsPool.Push(soundObjectToRemove);
		}
	}

	private void Awake()
	{
		this.soundObjectDoneAction = new SoundObject.SoundObjectDone(this.RemoveSoundObject);
		this.soundObjectsPool = new PooledStack<SoundObject>(delegate()
		{
			SoundObject soundObject = base.gameObject.AddComponent<SoundObject>();
			soundObject.AttachAudioHubObject(this);
			soundObject.Build();
			soundObject.enabled = false;
			return soundObject;
		}, this.START_SO_POOL_COUNT);
		if (this.MyAudioHub != AUDIO_HUB.UNIVERSAL)
		{
			GameManager.AudioSlinger.AddAudioHub(this);
		}
		if (this.IAmAlive != null)
		{
			this.IAmAlive();
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AudioHubObject.AudioHubVoidActions IAmDead;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AudioHubObject.AudioHubVoidActions TriggerPause;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AudioHubObject.AudioHubVoidActions TriggerMute;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AudioHubObject.AudioHubMuffleActions UnMuffleSound;

	public int START_SO_POOL_COUNT = 2;

	public AUDIO_HUB MyAudioHub;

	public bool HubPaused;

	public bool HubMuted;

	public bool HubMuffled;

	public float MuffledPerc;

	private PooledStack<SoundObject> soundObjectsPool;

	private Dictionary<AudioClip, List<SoundObject>> mySoundObjects = new Dictionary<AudioClip, List<SoundObject>>(10, AudioClipComparer.Ins);

	private SoundObject.SoundObjectDone soundObjectDoneAction;

	public delegate void AudioHubVoidActions();

	public delegate void AudioHubMuffleActions(float fadeTime);

	public delegate void PlayingActions(AudioFileDefinition TheAFD);
}
