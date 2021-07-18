using System;

[Serializable]
public class HitmanDataDefinition : Definition
{
	public int KeysRequiredToTrigger = 2;

	public float FireWindowMin = 30f;

	public float FireWindowMax = 60f;

	public float LockPickingTime = 10f;

	public float NotInMainRoomLeeyWayTime = 20f;

	public int MaxPeakCount = 3;

	public float CheckPeakingInterval = 3f;

	public int MaxMicCheck = 5;

	public int AddMicCheckThreshold = 10;

	public float MicCheckInterval = 1f;
}
