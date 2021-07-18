using System;

[Serializable]
public class LOLPYDiscData : DataObject
{
	public LOLPYDiscData(int SetID) : base(SetID)
	{
	}

	public bool WasInserted { get; set; }
}
