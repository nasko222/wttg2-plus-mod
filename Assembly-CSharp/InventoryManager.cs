using System;
using System.Collections.Generic;

public static class InventoryManager
{
	public static bool OwnsFlashlight
	{
		get
		{
			return InventoryManager._flashLight;
		}
		set
		{
			InventoryManager._flashLight = value;
		}
	}

	public static bool OwnsMotionSensorAudioCue
	{
		get
		{
			return InventoryManager._ownnsMotionSensorAudioCue;
		}
		set
		{
			InventoryManager._ownnsMotionSensorAudioCue = value;
		}
	}

	public static bool OwnsKeyCue
	{
		get
		{
			return InventoryManager._ownsKeyCue;
		}
		set
		{
			InventoryManager._ownsKeyCue = value;
		}
	}

	public static WIFI_DONGLE_LEVEL WifiDongleLevel
	{
		get
		{
			return InventoryManager._currentWifiDongleLevel;
		}
		set
		{
			InventoryManager._currentWifiDongleLevel = value;
		}
	}

	public static void Clear()
	{
		InventoryManager.AddedSoftwareProduct.Clear();
		InventoryManager.RemovedSoftwareProduct.Clear();
		InventoryManager._flashLight = false;
		InventoryManager._currentWifiDongleLevel = WIFI_DONGLE_LEVEL.LEVEL1;
		InventoryManager._currentActiveVPN = VPN_LEVELS.LEVEL0;
		InventoryManager._ownnsMotionSensorAudioCue = false;
		InventoryManager._ownsKeyCue = false;
		InventoryManager._currentlyOwnedHardwareProducts.Clear();
		InventoryManager._currentlyOwnedSoftwareProducts.Clear();
		InventoryManager.AddedSoftwareProduct = new CustomEvent<SOFTWARE_PRODUCTS>(10);
		InventoryManager.RemovedSoftwareProduct = new CustomEvent<SOFTWARE_PRODUCTS>(10);
		InventoryManager._currentlyOwnedHardwareProducts = new Dictionary<HARDWARE_PRODUCTS, int>(EnumComparer.HardwareProductCompare);
		InventoryManager._currentlyOwnedSoftwareProducts = new Dictionary<SOFTWARE_PRODUCTS, int>(EnumComparer.SoftwareProductCompare);
	}

	public static void AddProduct(ShadowMarketProductDefinition TheProduct)
	{
		if (InventoryManager._currentlyOwnedHardwareProducts.ContainsKey(TheProduct.productID))
		{
			InventoryManager._currentlyOwnedHardwareProducts[TheProduct.productID] = TheProduct.productCurrentInventoryCount;
		}
		else
		{
			InventoryManager._currentlyOwnedHardwareProducts.Add(TheProduct.productID, TheProduct.productCurrentInventoryCount);
		}
	}

	public static void AddProduct(ZeroDayProductDefinition TheProduct)
	{
		if (InventoryManager._currentlyOwnedSoftwareProducts.ContainsKey(TheProduct.productID))
		{
			InventoryManager._currentlyOwnedSoftwareProducts[TheProduct.productID] = TheProduct.productInventory;
		}
		else
		{
			InventoryManager._currentlyOwnedSoftwareProducts.Add(TheProduct.productID, TheProduct.productInventory);
		}
		InventoryManager.AddedSoftwareProduct.Execute(TheProduct.productID);
	}

	public static void RemoveProduct(HARDWARE_PRODUCTS ProductID)
	{
		if (InventoryManager._currentlyOwnedHardwareProducts.ContainsKey(ProductID))
		{
			InventoryManager._currentlyOwnedHardwareProducts[ProductID] = InventoryManager._currentlyOwnedHardwareProducts[ProductID] - 1;
			if (InventoryManager._currentlyOwnedHardwareProducts[ProductID] <= 0)
			{
				InventoryManager._currentlyOwnedHardwareProducts.Remove(ProductID);
			}
		}
	}

	public static void RemoveProduct(SOFTWARE_PRODUCTS ProductID)
	{
		if (InventoryManager._currentlyOwnedSoftwareProducts.ContainsKey(ProductID))
		{
			InventoryManager._currentlyOwnedSoftwareProducts[ProductID] = InventoryManager._currentlyOwnedSoftwareProducts[ProductID] - 1;
			if (InventoryManager._currentlyOwnedSoftwareProducts[ProductID] <= 0)
			{
				InventoryManager._currentlyOwnedSoftwareProducts.Remove(ProductID);
			}
			InventoryManager.RemovedSoftwareProduct.Execute(ProductID);
		}
	}

	public static int GetProductCount(HARDWARE_PRODUCTS ProductID)
	{
		if (InventoryManager._currentlyOwnedHardwareProducts.ContainsKey(ProductID))
		{
			return InventoryManager._currentlyOwnedHardwareProducts[ProductID];
		}
		return 0;
	}

	public static int GetProductCount(SOFTWARE_PRODUCTS ProductID)
	{
		if (InventoryManager._currentlyOwnedSoftwareProducts.ContainsKey(ProductID))
		{
			return InventoryManager._currentlyOwnedSoftwareProducts[ProductID];
		}
		return 0;
	}

	public static int GetWifiBoostLevel()
	{
		int result = 0;
		WIFI_DONGLE_LEVEL currentWifiDongleLevel = InventoryManager._currentWifiDongleLevel;
		if (currentWifiDongleLevel != WIFI_DONGLE_LEVEL.LEVEL2)
		{
			if (currentWifiDongleLevel == WIFI_DONGLE_LEVEL.LEVEL3)
			{
				result = 2;
			}
		}
		else
		{
			result = 1;
		}
		return result;
	}

	public static CustomEvent<SOFTWARE_PRODUCTS> AddedSoftwareProduct = new CustomEvent<SOFTWARE_PRODUCTS>(10);

	public static CustomEvent<SOFTWARE_PRODUCTS> RemovedSoftwareProduct = new CustomEvent<SOFTWARE_PRODUCTS>(10);

	private static bool _flashLight;

	private static WIFI_DONGLE_LEVEL _currentWifiDongleLevel;

	private static VPN_LEVELS _currentActiveVPN;

	private static bool _ownnsMotionSensorAudioCue;

	private static bool _ownsKeyCue;

	private static Dictionary<HARDWARE_PRODUCTS, int> _currentlyOwnedHardwareProducts = new Dictionary<HARDWARE_PRODUCTS, int>(EnumComparer.HardwareProductCompare);

	private static Dictionary<SOFTWARE_PRODUCTS, int> _currentlyOwnedSoftwareProducts = new Dictionary<SOFTWARE_PRODUCTS, int>(EnumComparer.SoftwareProductCompare);
}
