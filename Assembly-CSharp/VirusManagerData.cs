using System;

[Serializable]
public class VirusManagerData : DataObject
{
	public VirusManagerData(int SetID) : base(SetID)
	{
	}

	public bool HasVirus { get; set; }

	public int VirusCount { get; set; }
}
