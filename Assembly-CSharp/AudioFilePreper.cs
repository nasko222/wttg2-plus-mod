using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioFilePreper
{
	public string FileStartString;

	public List<AudioClip> AudioClips = new List<AudioClip>();

	public AUDIO_HUB MyAudioHub;

	public AUDIO_LAYER MyAudioLayer;

	public float Volume;

	public bool Delay;

	public float DelayAmount;

	public bool Loop;

	public int LoopCount;

	public AudioSourceDefinition MyAudioSourceDefinition;
}
