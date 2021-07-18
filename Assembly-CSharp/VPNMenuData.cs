using System;

[Serializable]
public class VPNMenuData : DataObject
{
	public VPNMenuData(int SetID) : base(SetID)
	{
	}

	public int CurrentActiveVPN { get; set; }
}
