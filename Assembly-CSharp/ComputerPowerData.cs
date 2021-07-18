using System;

[Serializable]
public class ComputerPowerData : DataObject
{
	public ComputerPowerData(int SetID) : base(SetID)
	{
	}

	public bool ComputerIsOff { get; set; }
}
