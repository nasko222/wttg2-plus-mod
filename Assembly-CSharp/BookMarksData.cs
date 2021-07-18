using System;
using System.Collections.Generic;

[Serializable]
public class BookMarksData : DataObject
{
	public BookMarksData(int SetID) : base(SetID)
	{
	}

	public Dictionary<int, BookmarkData> BookMarks { get; set; }
}
