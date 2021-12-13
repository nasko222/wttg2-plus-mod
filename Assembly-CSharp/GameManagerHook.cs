using System;
using ASoft.WTTG2;
using UnityEngine;

public class GameManagerHook : MonoBehaviour
{
	private void Awake()
	{
		if (AFDManager.Ins == null)
		{
			new GameObject("AFDManager").AddComponent<AFDManager>();
		}
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
