using System;
using UnityEngine;

[Serializable]
public class AudioSourceDefinition : Definition
{
	public bool GoCustom;

	public AudioSource CustomAudioSource;

	public float PanStero;

	public float SpatialBlend;

	public float ReverbZoneMix;

	public float DopplerLevel;

	public bool IsLiner;

	public float Spread;

	public float MinDistance;

	public float MaxDistance;
}
