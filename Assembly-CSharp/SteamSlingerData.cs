using System;

[Serializable]
public class SteamSlingerData : DataObject
{
	public SteamSlingerData(int SetID) : base(SetID)
	{
	}

	public int ProductPickUpCount { get; set; }
}
