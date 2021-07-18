using System;

[Serializable]
public class PowerBehaviourData : DataObject
{
	public PowerBehaviourData(int SetID) : base(SetID)
	{
	}

	public bool LightsAreOff { get; set; }
}
