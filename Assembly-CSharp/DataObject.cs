using System;

[Serializable]
public class DataObject : IDataObject
{
	public DataObject(int SetID)
	{
		this.ID = SetID;
	}

	public int ID { get; set; }
}
