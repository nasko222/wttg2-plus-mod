using System;

[Serializable]
public struct BookmarkData
{
	public BookmarkData(string SetTitle, string SetURL)
	{
		this.MyTitle = SetTitle;
		this.MyURL = SetURL;
	}

	public string MyTitle;

	public string MyURL;
}
