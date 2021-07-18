using System;
using System.Collections.Generic;

[Serializable]
public class MasterKeyData : DataObject
{
	public MasterKeyData(int SetID) : base(SetID)
	{
	}

	public List<string> Keys { get; set; }
}
