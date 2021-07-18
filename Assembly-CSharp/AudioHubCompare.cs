using System;
using System.Collections.Generic;

public class AudioHubCompare : IEqualityComparer<AUDIO_HUB>
{
	public bool Equals(AUDIO_HUB a, AUDIO_HUB b)
	{
		return a == b;
	}

	public int GetHashCode(AUDIO_HUB obj)
	{
		return (int)obj;
	}
}
