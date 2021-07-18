using System;
using System.Collections.Generic;

[Serializable]
public class TextDocManagerData : DataObject
{
	public TextDocManagerData(int SetID) : base(SetID)
	{
	}

	public List<TextDocIconData> CurrentDocs { get; set; }
}
