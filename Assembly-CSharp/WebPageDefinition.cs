using System;

[Serializable]
public class WebPageDefinition : Definition
{
	public void InvokePageEvent()
	{
		if (this.PageEvent != null)
		{
			this.PageEvent.Raise();
		}
	}

	public string PageName;

	public string FileName;

	public string PageHTML;

	public bool HasMusic;

	public AudioFileDefinition AudioFile;

	public KEY_DISCOVERY_MODES KeyDiscoverMode;

	public bool IsTapped;

	public int HashIndex;

	public string HashValue;

	public GameEvent PageEvent;
}
