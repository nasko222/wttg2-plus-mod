using System;

[Serializable]
public class TimeData : DataObject
{
	public TimeData(int SetID) : base(SetID)
	{
	}

	public int GameHour { get; set; }

	public int GameMin { get; set; }
}
