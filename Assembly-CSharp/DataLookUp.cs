using System;
using System.Collections.Generic;

[Serializable]
public class DataLookUp
{
	public Dictionary<Type, DataContainer> Data = new Dictionary<Type, DataContainer>(20);
}
