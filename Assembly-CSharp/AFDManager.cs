using System;
using UnityEngine;

public static class AFDManager
{
	public static AudioFileDefinition CreateCustomAFD(bool isPC, AudioClip clip, AUDIO_HUB hub, AUDIO_LAYER layer, float volume, bool loop, int loopCount)
	{
		AudioFileDefinition audioFileDefinition = new AudioFileDefinition();
		if (!isPC)
		{
			audioFileDefinition = UnityEngine.Object.Instantiate<AudioFileDefinition>(LookUp.SoundLookUp.JumpHit1);
		}
		else
		{
			audioFileDefinition = UnityEngine.Object.Instantiate<AudioFileDefinition>(LookUp.SoundLookUp.vacationRinging);
		}
		audioFileDefinition.AudioClip = clip;
		audioFileDefinition.MyAudioHub = hub;
		audioFileDefinition.MyAudioLayer = layer;
		audioFileDefinition.Volume = volume;
		audioFileDefinition.Loop = loop;
		audioFileDefinition.LoopCount = loopCount;
		return audioFileDefinition;
	}
}
