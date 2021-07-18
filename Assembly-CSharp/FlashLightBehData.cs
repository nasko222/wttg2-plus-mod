using System;

[Serializable]
public class FlashLightBehData : DataObject
{
	public FlashLightBehData(int SetID) : base(SetID)
	{
	}

	public float BatteryLifeUsage { get; set; }
}
