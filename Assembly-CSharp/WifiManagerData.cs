using System;

[Serializable]
public class WifiManagerData : DataObject
{
	public WifiManagerData(int SetID) : base(SetID)
	{
	}

	public int ActiveWifiHotSpotIndex { get; set; }

	public int CurrentWifiNetworkIndex { get; set; }

	public bool IsConnected { get; set; }

	public int OwnedWifiDongleLevel { get; set; }
}
