using System;
using UnityEngine;

public class GameManagerHook : MonoBehaviour
{
	private void Awake()
	{
		DownloadTIFiles.startDownloadingFiles();
		ModsManager.ApplyMods();
		GameManager.Instance.Init();
	}

	private void Update()
	{
		GameManager.Instance.Update();
	}

	private void OnDestroy()
	{
		SteamSlinger.Ins.ClearReset();
		WindowManager.Clear();
		EnvironmentManager.Clear();
		EnemyManager.Clear();
		DataManager.Reset();
		StateManager.Clear();
		InventoryManager.Clear();
		GameManager.Kill();
	}
}
