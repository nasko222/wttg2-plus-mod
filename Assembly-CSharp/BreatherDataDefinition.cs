using System;

[Serializable]
public class BreatherDataDefinition : Definition
{
	public int DoorAttemptsMin;

	public int DoorAttemptsMax;

	public float PatrolDelayMin;

	public float PatrolDelayMax;

	public float DoorAttemptWindowMin;

	public float DoorAttemptWindowMax;

	public float OpeningDoorWindowMin;

	public float OpeningDoorWindowMax;
}
