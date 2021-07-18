using System;

public class WifiWPAObject
{
	public WifiWPAObject(WifiNetworkDefinition setWifiNetwork, int setTotalInjectionAmoutAdded, int currentInjectionAmout)
	{
		this.myWifiNetwork = setWifiNetwork;
		this.TotalInjectionAmountAdded = setTotalInjectionAmoutAdded;
		this.CurrentInjectionAmount = 0;
		this.injectionReady = false;
	}

	public void SetInjectionReady(bool setValue)
	{
		this.injectionReady = setValue;
	}

	public bool GetInjectionReady()
	{
		return this.injectionReady;
	}

	public WifiNetworkDefinition myWifiNetwork;

	public int TotalInjectionAmountAdded;

	public int CurrentInjectionAmount;

	private bool injectionReady;
}
