using System;

[Serializable]
public class FlashLightData : DataObject
{
	public FlashLightData(int SetID) : base(SetID)
	{
	}

	public bool OwnsFlashLight { get; set; }
}
