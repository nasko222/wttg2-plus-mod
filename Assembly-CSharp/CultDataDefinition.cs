using System;

[Serializable]
public class CultDataDefinition : Definition
{
	public float TimeRequredForNormalSpawn;

	public int KeysRequiredForNormalSpawn;

	public int KeysRequiredForPowerSpawn;

	public int KeysRequiredForLightTrigger;

	public float NormalSpawnFireWindowMin;

	public float NormalSpawnFireWindowMax;

	public float LightCheckTimerLength;
}
