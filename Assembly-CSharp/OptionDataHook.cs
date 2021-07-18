using System;
using UnityEngine;

public class OptionDataHook : MonoBehaviour
{
	public Options Options
	{
		get
		{
			return this.opts;
		}
	}

	public void SaveOptionData()
	{
		DataManager.SaveOption<Options>(this.opts);
		DataManager.WriteOptionData();
	}

	private void Awake()
	{
		OptionDataHook.Ins = this;
		this.opts = DataManager.LoadOption<Options>(12);
		if (this.opts == null)
		{
			this.opts = new Options(12);
			this.opts.ScreenWidth = Screen.width;
			this.opts.ScreenHeight = Screen.height;
			this.opts.QualitySettingIndex = QualitySettings.GetQualityLevel();
			this.opts.WindowMode = !Screen.fullScreen;
			this.opts.VSync = (QualitySettings.vSyncCount > 0);
			this.opts.Mic = true;
			this.opts.Nudity = true;
			this.opts.MouseSens = 2;
			DataManager.SaveOption<Options>(this.opts);
			DataManager.WriteOptionData();
		}
	}

	private void OnDestroy()
	{
		OptionDataHook.Ins = null;
	}

	public static OptionDataHook Ins;

	private Options opts;
}
