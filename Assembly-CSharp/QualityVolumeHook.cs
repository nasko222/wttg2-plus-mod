using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class QualityVolumeHook : MonoBehaviour
{
	private void Awake()
	{
		this.myPPVolume = base.GetComponent<PostProcessVolume>();
		if (this.myPPVolume != null)
		{
			int qualityLevel = QualitySettings.GetQualityLevel();
			if (this.PostProcessProfiles != null && this.PostProcessProfiles[qualityLevel] != null)
			{
				this.myPPVolume.profile = this.PostProcessProfiles[qualityLevel];
				this.myPPVolume.enabled = true;
			}
		}
	}

	public PostProcessProfile[] PostProcessProfiles;

	private PostProcessVolume myPPVolume;
}
