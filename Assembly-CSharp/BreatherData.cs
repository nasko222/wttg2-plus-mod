using System;

[Serializable]
public class BreatherData : DataObject
{
	public BreatherData(int SetID) : base(SetID)
	{
	}

	public int KeysDiscoveredCount { get; set; }

	public bool ProductWasPickedUp { get; set; }
}
