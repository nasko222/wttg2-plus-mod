using System;

[Serializable]
public class ZeroDayProductData : DataObject
{
	public ZeroDayProductData(int SetID) : base(SetID)
	{
	}

	public int InventoryCount { get; set; }

	public bool Owned { get; set; }

	public bool Installing { get; set; }
}
