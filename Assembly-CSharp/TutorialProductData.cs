using System;

[Serializable]
public class TutorialProductData : DataObject
{
	public TutorialProductData(int SetID) : base(SetID)
	{
	}

	public bool WasPresneted { get; set; }
}
