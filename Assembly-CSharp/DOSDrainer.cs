using System;
using UnityEngine;

public class DOSDrainer
{
	public void tryConsume()
	{
		if (!this.consuming)
		{
			GameManager.TimeSlinger.FireTimer(1.5f, new Action(this.Consume), 0);
			this.consuming = true;
		}
	}

	private void Consume()
	{
		if (CurrencyManager.CurrentCurrency >= 10f && GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() != null && GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().affectedByDosDrainer && (GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().networkStrength > 0 || ModsManager.SBGlitch))
		{
			CurrencyManager.RemoveCurrency(UnityEngine.Random.Range(0.75f, 1.25f));
			GameManager.HackerManager.BlackHatSound2S();
		}
		this.consuming = false;
	}

	private bool consuming;
}
