using System;

[Serializable]
public class Options : DataObject
{
	public Options(int SetID) : base(SetID)
	{
	}

	public int ScreenWidth { get; set; }

	public int ScreenHeight { get; set; }

	public int QualitySettingIndex { get; set; }

	public bool WindowMode { get; set; }

	public bool VSync { get; set; }

	public bool Mic { get; set; }

	public bool Nudity { get; set; }

	public int MouseSens { get; set; }
}
