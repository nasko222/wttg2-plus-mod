using System;
using UnityEngine;

[RequireComponent(typeof(AudioReverbZone))]
[RequireComponent(typeof(HotZoneTrigger))]
public class ReverbZoneTrigger : MonoBehaviour
{
	private void Awake()
	{
		this.myReverbZone = base.GetComponent<AudioReverbZone>();
		this.myHotZone = base.GetComponent<HotZoneTrigger>();
	}

	private void Update()
	{
		if (this.myHotZone.IsHot)
		{
			this.myReverbZone.enabled = true;
		}
		else
		{
			this.myReverbZone.enabled = false;
		}
	}

	private AudioReverbZone myReverbZone;

	private HotZoneTrigger myHotZone;
}
