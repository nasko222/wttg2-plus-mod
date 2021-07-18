using System;

[Serializable]
public class CultData : DataObject
{
	public CultData(int SetID) : base(SetID)
	{
	}

	public int KeysDiscoveredCount { get; set; }

	public bool NormalSpawnActivated { get; set; }

	public bool PowerOffAttackActivated { get; set; }

	public bool LightsOffAttackActivated { get; set; }
}
