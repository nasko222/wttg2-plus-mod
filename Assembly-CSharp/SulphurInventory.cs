using System;

public static class SulphurInventory
{
	public static void AddSulphur(int amount)
	{
		SulphurInventory.SulphurAmount += amount;
		SulphurPackageObject.Ins.UpdateSulphurPackages();
	}

	public static void RemoveSulphur(int amount)
	{
		SulphurInventory.SulphurAmount -= amount;
		SulphurPackageObject.Ins.UpdateSulphurPackages();
	}

	public static int SulphurAmount;
}
