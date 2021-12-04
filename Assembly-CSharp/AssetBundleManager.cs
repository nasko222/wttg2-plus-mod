using System;
using System.IO;
using UnityEngine;

public static class AssetBundleManager
{
	public static void LoadAssetBundles()
	{
		if (AssetBundleManager.loaded)
		{
			return;
		}
		AssetBundleManager.loaded = true;
		AssetBundleManager.CheckAssetExistance("WTTG2_Data\\Resources\\bombmaker.assets");
		AssetBundleManager.Bombmaker = AssetBundle.LoadFromFile("WTTG2_Data\\Resources\\bombmaker.assets");
	}

	private static void CheckAssetExistance(string path)
	{
		if (!File.Exists(path))
		{
			Debug.Log("FATAL ERROR: " + path + " does not exist!");
			Application.Quit();
			return;
		}
	}

	public static AssetBundle Bombmaker;

	public static bool loaded;

	public static AudioFileDefinition BombMakerJumpSFX;
}
