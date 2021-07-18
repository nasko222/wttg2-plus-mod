using System;

[Serializable]
public class StageManagerData : DataObject
{
	public StageManagerData(int SetID) : base(SetID)
	{
	}

	public bool ThreatsActivated { get; set; }

	public float TimeLeft { get; set; }
}
