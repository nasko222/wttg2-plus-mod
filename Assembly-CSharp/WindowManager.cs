using System;
using System.Collections.Generic;

public static class WindowManager
{
	public static void Add(WindowBehaviour WindowToAdd)
	{
		if (WindowToAdd.Product == SOFTWARE_PRODUCTS.UNIVERSAL)
		{
			if (!WindowManager._uniWindows.ContainsKey(WindowToAdd.UniProductData.GetHashCode()))
			{
				WindowManager._uniWindows.Add(WindowToAdd.UniProductData.GetHashCode(), WindowToAdd);
			}
		}
		else if (!WindowManager._windows.ContainsKey(WindowToAdd.Product))
		{
			WindowManager._windows.Add(WindowToAdd.Product, WindowToAdd);
		}
	}

	public static WindowBehaviour Get(SOFTWARE_PRODUCTS WindowToGet)
	{
		return WindowManager._windows[WindowToGet];
	}

	public static WindowBehaviour Get(SoftwareProductDefinition UniWindow)
	{
		return WindowManager._uniWindows[UniWindow.GetHashCode()];
	}

	public static void Remove(SOFTWARE_PRODUCTS WindowToRemove)
	{
		WindowManager._windows.Remove(WindowToRemove);
	}

	public static void Remove(SoftwareProductDefinition UniWindow)
	{
		WindowManager._uniWindows.Remove(UniWindow.GetHashCode());
	}

	public static void Clear()
	{
		WindowManager._windows.Clear();
		WindowManager._uniWindows.Clear();
		WindowManager._windows = new Dictionary<SOFTWARE_PRODUCTS, WindowBehaviour>();
		WindowManager._uniWindows = new Dictionary<int, WindowBehaviour>();
	}

	private static Dictionary<SOFTWARE_PRODUCTS, WindowBehaviour> _windows = new Dictionary<SOFTWARE_PRODUCTS, WindowBehaviour>();

	private static Dictionary<int, WindowBehaviour> _uniWindows = new Dictionary<int, WindowBehaviour>();
}
