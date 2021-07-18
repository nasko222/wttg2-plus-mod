using System;

[Serializable]
public class ShadowMarketProductData : DataObject
{
	public ShadowMarketProductData(int SetID) : base(SetID)
	{
	}

	public int InventoryCount { get; set; }

	public bool Owned { get; set; }

	public bool Pending { get; set; }

	public bool Shipped { get; set; }
}
