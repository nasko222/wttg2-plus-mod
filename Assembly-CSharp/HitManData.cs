using System;

[Serializable]
public class HitManData : DataObject
{
	public HitManData(int SetID) : base(SetID)
	{
	}

	public int KeysDiscoveredCount { get; set; }

	public bool IsActivated { get; set; }

	public float TimeLeftOnWindow { get; set; }
}
