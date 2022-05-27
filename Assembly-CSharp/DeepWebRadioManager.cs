using System;
using UnityEngine;

public class DeepWebRadioManager : MonoBehaviour
{
	private void Awake()
	{
		DeepWebRadioManager.Ins = this;
		DeepWebRadioManager.currentClip = UnityEngine.Random.Range(0, CustomRadioLookUp.tracks.Length - 5);
		DeepWebRadioManager.RadioAS = base.gameObject.AddComponent<AudioSource>();
		DeepWebRadioManager.RadioAS.volume = 0f;
		DeepWebRadioManager.RadioAS.clip = CustomRadioLookUp.tracks[DeepWebRadioManager.currentClip];
		DeepWebRadioManager.RadioAS.time = 0f;
		DeepWebRadioManager.RadioAS.Play();
	}

	private void Update()
	{
		if (!DeepWebRadioManager.RadioAS.isPlaying)
		{
			DeepWebRadioManager.currentClip++;
			if (DeepWebRadioManager.currentClip >= CustomRadioLookUp.tracks.Length)
			{
				DeepWebRadioManager.currentClip = 0;
			}
			DeepWebRadioManager.RadioAS.clip = CustomRadioLookUp.tracks[DeepWebRadioManager.currentClip];
			DeepWebRadioManager.RadioAS.time = 0f;
			DeepWebRadioManager.RadioAS.Play();
		}
	}

	private void OnDestroy()
	{
		DeepWebRadioManager.RadioAS = null;
	}

	public static DeepWebRadioManager Ins;

	public static int currentClip;

	public static AudioSource RadioAS;
}
