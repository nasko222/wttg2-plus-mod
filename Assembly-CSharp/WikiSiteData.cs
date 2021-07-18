using System;
using System.Collections.Generic;

[Serializable]
public class WikiSiteData : DataObject
{
	public WikiSiteData(int SetID) : base(SetID)
	{
	}

	public List<WebSiteData> Wikis { get; set; }

	public List<List<int>> WikiSites { get; set; }

	public int PickedSiteToHoldSecondWiki { get; set; }
}
