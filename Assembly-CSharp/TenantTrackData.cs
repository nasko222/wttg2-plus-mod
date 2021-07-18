using System;
using System.Collections.Generic;

[Serializable]
public class TenantTrackData : DataObject
{
	public TenantTrackData(int SetID) : base(SetID)
	{
	}

	public Dictionary<int, List<TenantData>> TenantData { get; set; }
}
