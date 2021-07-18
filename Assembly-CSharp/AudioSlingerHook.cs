using System;

public static class AudioSlingerHook
{
	public static audioSlinger Ins
	{
		get
		{
			if (AudioSlingerHook._audioSlinger == null)
			{
				AudioSlingerHook._audioSlinger = new audioSlinger();
			}
			return AudioSlingerHook._audioSlinger;
		}
	}

	private static audioSlinger _audioSlinger;
}
