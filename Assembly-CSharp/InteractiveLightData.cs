using System;

[Serializable]
public class InteractiveLightData : DataObject
{
	public InteractiveLightData(int SetID) : base(SetID)
	{
	}

	public bool LightIsOff { get; set; }
}
