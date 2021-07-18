using System;

[Serializable]
public class LightTriggerData : DataObject
{
	public LightTriggerData(int SetID) : base(SetID)
	{
	}

	public bool LightsAreOff { get; set; }
}
