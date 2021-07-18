using System;
using System.Collections.Generic;

[Serializable]
public class DataContainer
{
	public void Add<T>(int SetID, T DataToSave) where T : class, IDataObject
	{
		IDataObject value;
		if (this.MyData.TryGetValue(SetID, out value))
		{
			value = DataToSave;
			this.MyData[SetID] = value;
		}
		else
		{
			this.MyData.Add(SetID, DataToSave);
		}
	}

	public Dictionary<int, IDataObject> MyData = new Dictionary<int, IDataObject>(20);
}
