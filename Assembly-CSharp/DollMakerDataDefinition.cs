using System;

[Serializable]
public class DollMakerDataDefinition : Definition
{
	public float DelayTimeMin;

	public float DelayTimeMax;

	public float ForcePowerTripTime;

	public float MarkerCoolTimeMin;

	public float MarkerCoolTimeMax;

	public int TargetVictimCount;

	public float MarkerResetTimeMin;

	public float MarkerResetTimeMax;
}
