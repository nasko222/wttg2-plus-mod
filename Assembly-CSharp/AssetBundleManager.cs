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
		AssetBundleManager.CheckAssetExistance("WTTG2_Data\\Resources\\custom_audio.assets");
		AssetBundleManager.customAudio = AssetBundle.LoadFromFile("WTTG2_Data\\Resources\\custom_audio.assets");
		AssetBundleManager.CheckAssetExistance("WTTG2_Data\\Resources\\custom_tex.assets");
		AssetBundleManager.customTex = AssetBundle.LoadFromFile("WTTG2_Data\\Resources\\custom_tex.assets");
		AssetBundleManager.CheckAssetExistance("WTTG2_Data\\Resources\\custom_source.assets");
		AssetBundleManager.customSource = AssetBundle.LoadFromFile("WTTG2_Data\\Resources\\custom_source.assets");
		AssetBundleManager.CheckAssetExistance("WTTG2_Data\\Resources\\WTTG2Plus.assets");
		AssetBundleManager.WTTG2PlusProps = AssetBundle.LoadFromFile("WTTG2_Data\\Resources\\WTTG2Plus.assets");
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

	public static AssetBundle customAudio;

	public static AssetBundle customTex;

	public static AssetBundle customSource;

	public static AssetBundle WTTG2PlusProps;
}
