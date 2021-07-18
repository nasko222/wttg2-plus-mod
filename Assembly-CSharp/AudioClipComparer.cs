using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipComparer : IEqualityComparer<AudioClip>
{
	public bool Equals(AudioClip x, AudioClip y)
	{
		return x == y;
	}

	public int GetHashCode(AudioClip obj)
	{
		return obj.GetHashCode();
	}

	public static AudioClipComparer Ins = new AudioClipComparer();
}
