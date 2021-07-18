using System;

[Serializable]
public class WifiNetworkData : DataObject
{
	public WifiNetworkData(int SetID) : base(SetID)
	{
	}

	public string NetworkBSSID { get; set; }

	public string NetworkPassword { get; set; }

	public short NetworkOpenPort { get; set; }

	public short NetworkInjectionAmount { get; set; }
}
