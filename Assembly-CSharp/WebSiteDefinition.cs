using System;
using System.Collections.Generic;

[Serializable]
public class WebSiteDefinition : Definition
{
	public bool isFake;

	public bool DoNotList;

	public bool DoNotTap;

	public bool isStatic;

	public bool WikiSpecific;

	public int WikiIndex;

	public bool HoldsSecondWikiLink;

	public bool IsTapped;

	public bool WasVisted;

	public bool HasWindow;

	public WEBSITE_WINDOW_TIME WindowTime;

	public string PageTitle;

	public string PageDesc;

	public string PageURL;

	public string DocumentRoot;

	public WebPageDefinition HomePage;

	public List<WebPageDefinition> SubPages = new List<WebPageDefinition>();
}
