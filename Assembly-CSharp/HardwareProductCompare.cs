using System;
using System.Collections.Generic;

public class HardwareProductCompare : IEqualityComparer<HARDWARE_PRODUCTS>
{
	public bool Equals(HARDWARE_PRODUCTS a, HARDWARE_PRODUCTS b)
	{
		return a == b;
	}

	public int GetHashCode(HARDWARE_PRODUCTS obj)
	{
		return (int)obj;
	}
}
