using System;
using UnityEngine;

public static class ModsManager
{
	public static void ApplyMods()
	{
		if (PlayerPrefs.GetInt("[MOD]TTVInt", 1) == 1)
		{
			ModsManager.DOSTwitchActive = true;
		}
		else
		{
			ModsManager.DOSTwitchActive = false;
		}
		if (PlayerPrefs.GetInt("[MOD]DevTools", 1) == 1)
		{
			ModsManager.DevToolsActive = true;
		}
		else
		{
			ModsManager.DevToolsActive = false;
		}
		if (PlayerPrefs.GetInt("[MOD]EasyMode", 1) == 1)
		{
			ModsManager.EasyModeActive = true;
		}
		else
		{
			ModsManager.EasyModeActive = false;
		}
		if (PlayerPrefs.GetInt("[MOD]GODSpot", 1) == 1)
		{
			ModsManager.ShowGodSpot = true;
		}
		else
		{
			ModsManager.ShowGodSpot = false;
		}
		if (PlayerPrefs.GetInt("[MOD]UnlimitedStamina", 1) == 1)
		{
			ModsManager.UnlimitedStamina = true;
		}
		else
		{
			ModsManager.UnlimitedStamina = false;
		}
		if (PlayerPrefs.GetInt("[MOD]SkybreakGlitch", 1) == 1)
		{
			ModsManager.SBGlitch = true;
		}
		else
		{
			ModsManager.SBGlitch = false;
		}
		if (PlayerPrefs.GetInt("[MOD]TrolloPollo", 1) == 1)
		{
			ModsManager.Trolling = true;
		}
		else
		{
			ModsManager.Trolling = false;
		}
		if (PlayerPrefs.GetInt("[MOD]Achievements", 1) == 1)
		{
			ModsManager.Achievements = true;
		}
		else
		{
			ModsManager.Achievements = false;
		}
		if (PlayerPrefs.GetInt("[MOD]Debugging", 1) == 1)
		{
			ModsManager.DebugEnabled = true;
		}
		else
		{
			ModsManager.DebugEnabled = false;
		}
		if (PlayerPrefs.GetInt("[MOD]PoliceScannerMod", 1) == 1)
		{
			ModsManager.PoliceScannerMod = true;
		}
		else
		{
			ModsManager.PoliceScannerMod = false;
		}
		if (PlayerPrefs.GetInt("[MOD]EasierEnemies", 1) == 1)
		{
			ModsManager.EasierEnemies = true;
		}
		else
		{
			ModsManager.EasierEnemies = false;
		}
		if (PlayerPrefs.GetInt("[MOD]AlwaysOpenSites", 1) == 1)
		{
			ModsManager.AlwaysOpenSites = true;
		}
		else
		{
			ModsManager.AlwaysOpenSites = false;
		}
		Debug.Log("[ModsManager] Applies mod settings");
	}

	public static bool DOSTwitchActive;

	public static bool DevToolsActive;

	public static bool EasyModeActive;

	public static bool ShowGodSpot;

	public static bool ForceHackingEnabled;

	public static bool UnlimitedStamina;

	public static bool SBGlitch;

	public static bool Trolling;

	public static bool Nightmare;

	public static bool ContentExtension;

	public static bool Jumpscares;

	public static bool Websites;

	public static readonly string ModVersion = "1.4";

	public static bool NoParameter;

	public static bool DebugEnabled;

	public static bool PoliceScannerMod;

	public static bool Achievements;

	public static bool EasierEnemies;

	public static bool AlwaysOpenSites;
}
