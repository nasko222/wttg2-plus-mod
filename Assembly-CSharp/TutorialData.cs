using System;

[Serializable]
public class TutorialData : DataObject
{
	public TutorialData(int SetID) : base(SetID)
	{
	}

	public bool IconsShown { get; set; }
}
