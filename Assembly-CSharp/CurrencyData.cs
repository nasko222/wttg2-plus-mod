using System;

[Serializable]
public class CurrencyData : DataObject
{
	public CurrencyData(int SetID) : base(SetID)
	{
	}

	public float CurrentCurrency { get; set; }
}
