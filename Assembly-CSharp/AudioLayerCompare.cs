using System;
using System.Collections.Generic;

public class AudioLayerCompare : IEqualityComparer<AUDIO_LAYER>
{
	public bool Equals(AUDIO_LAYER a, AUDIO_LAYER b)
	{
		return a == b;
	}

	public int GetHashCode(AUDIO_LAYER obj)
	{
		return (int)obj;
	}
}
