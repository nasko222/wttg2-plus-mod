using System;
using System.Collections.Generic;

[Serializable]
public class WebSiteData
{
	public string PageURL { get; set; }

	public bool Fake { get; set; }

	public bool Visted { get; set; }

	public bool IsTapped { get; set; }

	public List<WebPageData> Pages { get; set; }
}
