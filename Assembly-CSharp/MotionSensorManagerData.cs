using System;
using System.Collections.Generic;

[Serializable]
public class MotionSensorManagerData : DataObject
{
	public MotionSensorManagerData(int SetID) : base(SetID)
	{
	}

	public Dictionary<int, MotionSensorPlacementData> CurrentlyPlacedMotionSensors = new Dictionary<int, MotionSensorPlacementData>(4);
}
