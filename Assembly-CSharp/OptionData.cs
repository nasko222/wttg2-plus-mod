using System;
using UnityEngine;

public class OptionData : MonoBehaviour
{
	private void Awake()
	{
		Options options = DataManager.LoadOption<Options>(12);
		if (options != null)
		{
			bool fullscreen = !options.WindowMode;
			int vSyncCount = (!options.VSync) ? 0 : 1;
			QualitySettings.SetQualityLevel(options.QualitySettingIndex, true);
			Screen.SetResolution(options.ScreenWidth, options.ScreenHeight, fullscreen);
			QualitySettings.vSyncCount = vSyncCount;
		}
	}
}
