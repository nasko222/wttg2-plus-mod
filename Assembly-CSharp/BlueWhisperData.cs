using System;

[Serializable]
public class BlueWhisperData : DataObject
{
	public BlueWhisperData(int SetID) : base(SetID)
	{
	}

	public bool Pending { get; set; }

	public bool Owns { get; set; }
}
