using System;
using System.Collections.Generic;
using System.Diagnostics;

public class audioSlinger
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event audioSlinger.AudioHubActions AddedNewHub;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event audioSlinger.AudioLayerActions AddedNewLayer;

	public void AddAudioHub(AudioHubObject theHub)
	{
		AudioHubObject audioHubObject;
		if (this.currentAudioHubs.TryGetValue(theHub.MyAudioHub, out audioHubObject))
		{
			audioHubObject.KillMe();
			this.currentAudioHubs.Remove(theHub.MyAudioHub);
		}
		this.currentAudioHubs.Add(theHub.MyAudioHub, theHub);
		if (this.AddedNewHub != null)
		{
			this.AddedNewHub(theHub);
		}
	}

	public void RemoveAudioHub(AUDIO_HUB hubToRemove)
	{
		AudioHubObject audioHubObject;
		if (this.currentAudioHubs.TryGetValue(hubToRemove, out audioHubObject))
		{
			audioHubObject.KillMe();
			this.currentAudioHubs.Remove(hubToRemove);
		}
	}

	public void MuffleAudioHub(AUDIO_HUB hubToMuffle, float setPer, float fadeTime = 0f)
	{
		if (this.currentAudioHubs.ContainsKey(hubToMuffle))
		{
			this.currentAudioHubs[hubToMuffle].MuffleHub(setPer, fadeTime);
		}
	}

	public void UnMuffleAudioHub(AUDIO_HUB hubToUnMuffle, float fadeTime = 0f)
	{
		if (this.currentAudioHubs.ContainsKey(hubToUnMuffle))
		{
			this.currentAudioHubs[hubToUnMuffle].UnMuffleHub(fadeTime);
		}
	}

	public void MuteAudioHub(AUDIO_HUB hubToMute)
	{
		if (this.currentAudioHubs.ContainsKey(hubToMute))
		{
			this.currentAudioHubs[hubToMute].MuteHub();
		}
	}

	public void UnMuteAudioHub(AUDIO_HUB hubToUnMute)
	{
		if (this.currentAudioHubs.ContainsKey(hubToUnMute))
		{
			this.currentAudioHubs[hubToUnMute].UnMuteHub();
		}
	}

	public void PauseAudioHub(AUDIO_HUB hubToPause)
	{
		if (this.currentAudioHubs.ContainsKey(hubToPause))
		{
			this.currentAudioHubs[hubToPause].PauseHub();
		}
	}

	public void UnPauseAudioHub(AUDIO_HUB hubToUnPause)
	{
		if (this.currentAudioHubs.ContainsKey(hubToUnPause))
		{
			this.currentAudioHubs[hubToUnPause].UnPauseHub();
		}
	}

	public void AddAudioLayer(AUDIO_LAYER AudioLayer)
	{
		if (!this.currentAudioLayers.ContainsKey(AudioLayer))
		{
			this.currentAudioLayers.Add(AudioLayer, new AudioLayerObject(AudioLayer));
			if (this.AddedNewLayer != null)
			{
				this.AddedNewLayer(this.currentAudioLayers[AudioLayer]);
			}
		}
	}

	public void AddAudioLayer(AUDIO_LAYER AudioLayer, out AudioLayerObject returnAudioLayer)
	{
		if (!this.currentAudioLayers.ContainsKey(AudioLayer))
		{
			this.currentAudioLayers.Add(AudioLayer, new AudioLayerObject(AudioLayer));
			if (this.AddedNewLayer != null)
			{
				this.AddedNewLayer(this.currentAudioLayers[AudioLayer]);
			}
		}
		returnAudioLayer = this.currentAudioLayers[AudioLayer];
	}

	public void RemoveAudioLayer(AUDIO_LAYER AudioLayerToRemove)
	{
		AudioLayerObject audioLayerObject;
		if (this.currentAudioLayers.TryGetValue(AudioLayerToRemove, out audioLayerObject))
		{
			audioLayerObject.KillMe();
			this.currentAudioLayers.Remove(AudioLayerToRemove);
		}
	}

	public void MuffleAudioLayer(AUDIO_LAYER AudioLayerToMuffle, float setPer, float fadeTime = 0f)
	{
		if (this.currentAudioLayers.ContainsKey(AudioLayerToMuffle))
		{
			this.currentAudioLayers[AudioLayerToMuffle].MuffleMe(setPer, fadeTime);
		}
	}

	public void UnMuffleAudioLayer(AUDIO_LAYER AudioLayerToUnMuffle, float fadeTime = 0f)
	{
		if (this.currentAudioLayers.ContainsKey(AudioLayerToUnMuffle))
		{
			this.currentAudioLayers[AudioLayerToUnMuffle].UnMuffleMe(fadeTime);
		}
	}

	public void MuteAudioLayer(AUDIO_LAYER AudioLayerToMute)
	{
		if (this.currentAudioLayers.ContainsKey(AudioLayerToMute))
		{
			this.currentAudioLayers[AudioLayerToMute].MuteMe();
		}
	}

	public void UnMuteAudioLayer(AUDIO_LAYER AudioLayerToUnMute)
	{
		if (this.currentAudioLayers.ContainsKey(AudioLayerToUnMute))
		{
			this.currentAudioLayers[AudioLayerToUnMute].UnMuteMe();
		}
	}

	public void PauseAudioLayer(AUDIO_LAYER AudioLayerToPause)
	{
		if (this.currentAudioLayers.ContainsKey(AudioLayerToPause))
		{
			this.currentAudioLayers[AudioLayerToPause].PauseMe();
		}
	}

	public void UnPauseAudioLayer(AUDIO_LAYER AudioLayerToUnPause)
	{
		if (this.currentAudioLayers.ContainsKey(AudioLayerToUnPause))
		{
			this.currentAudioLayers[AudioLayerToUnPause].UnPauseMe();
		}
	}

	public void PlaySound(AudioFileDefinition AudioFile)
	{
		if (this.currentAudioHubs.ContainsKey(AudioFile.MyAudioHub))
		{
			this.currentAudioHubs[AudioFile.MyAudioHub].PlaySound(AudioFile);
		}
	}

	public void PlaySoundWithWildPitch(AudioFileDefinition AudioFile, float Min, float Max)
	{
		if (this.currentAudioHubs.ContainsKey(AudioFile.MyAudioHub))
		{
			this.currentAudioHubs[AudioFile.MyAudioHub].PlaySoundWithWildPitch(AudioFile, Min, Max);
		}
	}

	public void PlaySoundWithCustomDelay(AudioFileDefinition AudioFile, float SetDelay)
	{
		if (this.currentAudioHubs.ContainsKey(AudioFile.MyAudioHub))
		{
			this.currentAudioHubs[AudioFile.MyAudioHub].PlaySoundCustomDelay(AudioFile, SetDelay);
		}
	}

	public void KillSound(AudioFileDefinition AudioFile)
	{
		if (this.currentAudioHubs.ContainsKey(AudioFile.MyAudioHub))
		{
			this.currentAudioHubs[AudioFile.MyAudioHub].KillSound(AudioFile.AudioClip);
		}
	}

	public void UniKillSound(AudioFileDefinition AudioFile)
	{
		foreach (KeyValuePair<AUDIO_HUB, AudioHubObject> keyValuePair in this.currentAudioHubs)
		{
			keyValuePair.Value.KillSound(AudioFile.AudioClip);
		}
	}

	private Dictionary<AUDIO_HUB, AudioHubObject> currentAudioHubs = new Dictionary<AUDIO_HUB, AudioHubObject>(EnumComparer.AudioHubCompare);

	private Dictionary<AUDIO_LAYER, AudioLayerObject> currentAudioLayers = new Dictionary<AUDIO_LAYER, AudioLayerObject>(EnumComparer.AudioLayerCompare);

	public delegate void AudioHubActions(AudioHubObject theHub);

	public delegate void AudioLayerActions(AudioLayerObject theLayer);
}
