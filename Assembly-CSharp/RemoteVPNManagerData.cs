using System;
using System.Collections.Generic;

[Serializable]
public class RemoteVPNManagerData : DataObject
{
	public RemoteVPNManagerData(int SetID) : base(SetID)
	{
	}

	public List<VPNCurrencyData> CurrentVPNVolumesCurrencyData { get; set; }

	public Dictionary<int, SerTrans> CurrentlyPlacedRemoteVPNs { get; set; }
}
