using System;
using System.Collections.Generic;

[Serializable]
public class NotesData : DataObject
{
	public NotesData(int SetID) : base(SetID)
	{
	}

	public List<string> Notes { get; set; }
}
