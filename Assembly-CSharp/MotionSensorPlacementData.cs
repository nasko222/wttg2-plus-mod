using System;

[Serializable]
public class MotionSensorPlacementData
{
	public MotionSensorPlacementData(PLAYER_LOCATION SetLocation, SerTrans SetPosition)
	{
		this.LocationName = (int)SetLocation;
		this.LocationPoisition = SetPosition;
	}

	public int LocationName;

	public SerTrans LocationPoisition;
}
