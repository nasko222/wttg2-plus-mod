using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public static class AssetDownloader
{
	public static void Init()
	{
		AssetDownloader.WTTG2Plus = new AssetFile(AssetDownloader._size[0], "WTTG2Plus.assets", "WTTG2_Data\\Resources\\", "https://wttg2plus.ampersoft.cz/Resources/WTTG2Plus.assets");
		AssetDownloader.browser_assets = new AssetFile(AssetDownloader._size[1], "browser_assets", "WTTG2_Data\\Resources\\", "https://wttg2plus.ampersoft.cz/Resources/browser_assets");
		AssetDownloader.assetFiles.Add(AssetDownloader.WTTG2Plus);
		AssetDownloader.assetFiles.Add(AssetDownloader.browser_assets);
	}

	public static void Exec()
	{
		for (int i = 0; i < AssetDownloader.assetFiles.Count; i++)
		{
			if (!AssetDownloader.Validate(AssetDownloader.assetFiles[i]))
			{
				AssetDownloader.Download(AssetDownloader.assetFiles[i]);
				do
				{
					AssetDownloader.DownloadWorker(AssetDownloader.assetFiles[i]);
				}
				while (new FileInfo(AssetDownloader.assetFiles[i].path).Length < AssetDownloader.assetFiles[i].size);
				Debug.Log("Download Completed: " + AssetDownloader.assetFiles[i].fileName);
			}
		}
	}

	private static void Download(AssetFile file)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(file.externalURL, "GET");
		unityWebRequest.downloadHandler = new DownloadHandlerFile(file.path);
		unityWebRequest.SendWebRequest();
		if (unityWebRequest.isNetworkError)
		{
			Debug.Log(unityWebRequest.error);
			return;
		}
		Debug.Log("Download started: " + file.fileName);
	}

	public static bool Validate(AssetFile file)
	{
		return File.Exists(file.path) && new FileInfo(file.path).Length == file.size;
	}

	public static void ValidateAll()
	{
		for (int i = 0; i < AssetDownloader.assetFiles.Count; i++)
		{
			if (!AssetDownloader.Validate(AssetDownloader.assetFiles[i]))
			{
				Debug.Log("Cannot validate asset file: " + AssetDownloader.assetFiles[i].fileName);
				Application.Quit();
			}
		}
	}

	private static void DownloadWorker(AssetFile file)
	{
		TitleManager.wttg2plus_modText.GetComponent<TextMeshProUGUI>().text = string.Concat(new object[]
		{
			"Downloading WTTG2+ Assets: ",
			file.fileName,
			" | ",
			File.Exists(file.path) ? (new FileInfo(file.path).Length / 1048576L) : 0L,
			"MB",
			" / ",
			file.size / 1048576L,
			"MB"
		});
		Thread.Sleep(0);
	}

	public static bool CheckFiles()
	{
		for (int i = 0; i < AssetDownloader.assetFiles.Count; i++)
		{
			if (!AssetDownloader.Validate(AssetDownloader.assetFiles[i]))
			{
				return false;
			}
		}
		return true;
	}

	private static AssetFile WTTG2Plus;

	private static AssetFile browser_assets;

	private static List<AssetFile> assetFiles = new List<AssetFile>();

	private static readonly long[] _size = new long[]
	{
		142692836L,
		280756713L
	};
}
