using System;

[Serializable]
public class TextDocIconData
{
	public TextDocIconData(int SetID, string SetName, string SetText, Vect2 SetPOS)
	{
		this.ID = SetID;
		this.TextDocName = SetName;
		this.TextDocText = SetText;
		this.TextDocPOS = SetPOS;
	}

	public int ID;

	public string TextDocName;

	public string TextDocText;

	public Vect2 TextDocPOS;
}
