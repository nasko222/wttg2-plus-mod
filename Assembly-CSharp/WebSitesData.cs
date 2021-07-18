using System;
using System.Collections.Generic;

[Serializable]
public class WebSitesData : DataObject
{
	public WebSitesData(int SetID) : base(SetID)
	{
	}

	public List<WebSiteData> Sites { get; set; }
}
