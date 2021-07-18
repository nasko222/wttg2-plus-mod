using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	internal class StandaloneShutdown : MonoBehaviour
	{
		public static void Create()
		{
			GameObject gameObject = new GameObject("ZFB Shutdown");
			gameObject.AddComponent<StandaloneShutdown>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}

		public void OnApplicationQuit()
		{
			BrowserNative.UnloadNative();
		}
	}
}
