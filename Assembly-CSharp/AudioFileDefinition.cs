using System;
using UnityEngine;

[Serializable]
public class AudioFileDefinition : Definition
{
	public AudioFileDefinition()
	{
	}

	public AudioFileDefinition(AudioFileDefinition CopyAFD)
	{
		this.AudioClip = CopyAFD.AudioClip;
		this.MyAudioHub = CopyAFD.MyAudioHub;
		this.MyAudioLayer = CopyAFD.MyAudioLayer;
		this.Volume = CopyAFD.Volume;
		this.Delay = CopyAFD.Delay;
		this.DelayAmount = CopyAFD.DelayAmount;
		this.Loop = CopyAFD.Loop;
		this.LoopCount = CopyAFD.LoopCount;
	}

	public AudioClip AudioClip;

	public AUDIO_HUB MyAudioHub;

	public AUDIO_LAYER MyAudioLayer;

	public float Volume;

	public bool Delay;

	public float DelayAmount;

	public bool Loop;

	public int LoopCount;

	public AudioSourceDefinition MyAudioSourceDefinition;
}
