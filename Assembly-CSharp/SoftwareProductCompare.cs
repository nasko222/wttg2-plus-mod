using System;
using System.Collections.Generic;

public class SoftwareProductCompare : IEqualityComparer<SOFTWARE_PRODUCTS>
{
	public bool Equals(SOFTWARE_PRODUCTS a, SOFTWARE_PRODUCTS b)
	{
		return a == b;
	}

	public int GetHashCode(SOFTWARE_PRODUCTS obj)
	{
		return (int)obj;
	}
}
