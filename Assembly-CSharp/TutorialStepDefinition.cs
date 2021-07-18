using System;

[Serializable]
public class TutorialStepDefinition : Definition
{
	public bool HasDelay;

	public float DelayAmount;

	public bool HasAudioFile;

	public AudioFileDefinition AudioFile;

	public bool HasText;

	public string Text;

	public bool HasGameEvent;

	public GameEvent GameEvent;

	public bool ClickToContinue;
}
