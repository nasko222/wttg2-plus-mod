using System;

public static class SulphurInventory
{
	public static void AddSulphur(int amount)
	{
		SulphurInventory.SulphurAmount += amount;
	}

	public static void RemoveSulphur(int amount)
	{
		SulphurInventory.SulphurAmount -= amount;
	}

	public static int SulphurAmount;
}
