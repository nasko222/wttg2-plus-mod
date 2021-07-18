using System;
using System.Collections.Generic;

[Serializable]
public class DollMakerData : DataObject
{
	public DollMakerData(int SetID) : base(SetID)
	{
	}

	public bool IsReleased { get; set; }

	public bool IsActivated { get; set; }

	public int CurrentVictims { get; set; }

	public bool IsSatisfied { get; set; }

	public List<int> UsedUnitNumbers { get; set; }

	public int ActiveUnitNumber { get; set; }

	public bool PlayedHelpMeSound { get; set; }
}
