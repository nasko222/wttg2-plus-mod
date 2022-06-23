using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

public class TheCloud : MonoBehaviour
{
	public string MasterKey
	{
		get
		{
			return this.masterKey;
		}
	}

	public void InvalidURL(out string ReturnURL)
	{
		ReturnURL = "localGame://NotFound/index.html";
		this.curWebPageDef = null;
		this.curWebsiteDef = null;
	}

	public bool SoftValidateURL(out string returnURL, string checkURL = "")
	{
		returnURL = "localGame://NotFound/index.html";
		checkURL = checkURL.Replace("http://", string.Empty);
		checkURL = checkURL.Replace("https://", string.Empty);
		checkURL = checkURL.Replace("www.", string.Empty);
		string[] array = checkURL.Split(new string[]
		{
			"/"
		}, StringSplitOptions.None);
		return array[0].Equals("game.local") || this.validDomains.Contains(array[0].ToLower());
	}

	public bool ValidateURL(out string returnURL, string checkURL = "")
	{
		returnURL = "localGame://NotFound/index.html";
		if (checkURL != string.Empty)
		{
			checkURL = checkURL.Replace("http://", string.Empty);
			checkURL = checkURL.Replace("www.", string.Empty);
			string[] array = checkURL.Split(new string[]
			{
				"/"
			}, StringSplitOptions.None);
			string key = array[0].Replace(".ann", string.Empty);
			this.curWebPageDef = null;
			this.curWebsiteDef = null;
			if (this.validDomains.Contains(array[0].ToLower()))
			{
				returnURL = "http://www." + checkURL;
				SteamSlinger.Ins.CheckStalkerURL(returnURL);
			}
			else if (array[0].Contains(".ann"))
			{
				if (this.wikiLookUp.ContainsKey(key))
				{
					int index = this.wikiLookUp[key];
					this.curWebsiteDef = this.wikis[index];
					returnURL = "localGame://" + this.wikis[index].DocumentRoot + "/" + this.wikis[index].HomePage.FileName;
					this.curWebPageDef = this.wikis[index].HomePage;
				}
				else if (this.websiteLookUp.ContainsKey(key))
				{
					int index2 = this.websiteLookUp[key];
					this.curWebsiteDef = this.Websites[index2];
					if (array.Length > 1)
					{
						if (array[1] != string.Empty)
						{
							if (array[1].ToLower() == this.Websites[index2].HomePage.FileName)
							{
								returnURL = "localGame://" + this.Websites[index2].DocumentRoot + "/" + this.Websites[index2].HomePage.FileName;
								this.curWebPageDef = this.Websites[index2].HomePage;
							}
							else
							{
								for (int i = 0; i < this.Websites[index2].SubPages.Count; i++)
								{
									if (array[1].ToLower() == this.Websites[index2].SubPages[i].FileName)
									{
										returnURL = "localGame://" + this.Websites[index2].DocumentRoot + "/" + this.Websites[index2].SubPages[i].FileName;
										this.curWebPageDef = this.Websites[index2].SubPages[i];
										i = this.Websites[index2].SubPages.Count;
									}
								}
							}
						}
						else
						{
							returnURL = "localGame://" + this.Websites[index2].DocumentRoot + "/" + this.Websites[index2].HomePage.FileName;
							this.curWebPageDef = this.Websites[index2].HomePage;
						}
					}
					else if (this.Websites[index2].isFake)
					{
						returnURL = "localGame://NotFound/index.html";
						this.curWebPageDef = null;
						this.curWebsiteDef = null;
						GameManager.HackerManager.RollHack();
					}
					else
					{
						returnURL = "localGame://" + this.Websites[index2].DocumentRoot + "/" + this.Websites[index2].HomePage.FileName;
						this.curWebPageDef = this.Websites[index2].HomePage;
					}
					if (this.Websites[index2].HasWindow && !ModsManager.AlwaysOpenSites)
					{
						bool flag = true;
						switch (this.Websites[index2].WindowTime)
						{
						case WEBSITE_WINDOW_TIME.FIRST_QUARTER:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 0 && GameManager.TimeKeeper.GetCurrentGameMin() <= 15)
							{
								flag = false;
							}
							break;
						case WEBSITE_WINDOW_TIME.SECOND_QUARTER:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 15 && GameManager.TimeKeeper.GetCurrentGameMin() <= 30)
							{
								flag = false;
							}
							break;
						case WEBSITE_WINDOW_TIME.THRID_QUARTER:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 30 && GameManager.TimeKeeper.GetCurrentGameMin() <= 45)
							{
								flag = false;
							}
							break;
						case WEBSITE_WINDOW_TIME.FOURTH_QUARTER:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 45 && GameManager.TimeKeeper.GetCurrentGameMin() <= 60)
							{
								flag = false;
							}
							break;
						case WEBSITE_WINDOW_TIME.FIRST_HALF:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 0 && GameManager.TimeKeeper.GetCurrentGameMin() <= 30)
							{
								flag = false;
							}
							break;
						case WEBSITE_WINDOW_TIME.SECNOND_HALF:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 30 && GameManager.TimeKeeper.GetCurrentGameMin() <= 60)
							{
								flag = false;
							}
							break;
						}
						if (flag)
						{
							returnURL = "localGame://NotFound/index.html";
							this.curWebPageDef = null;
							this.curWebsiteDef = null;
						}
					}
				}
			}
		}
		if (returnURL == "localGame://NotFound/index.html")
		{
			return false;
		}
		if (this.curWebsiteDef != null && this.websiteLookUp.ContainsKey(this.curWebsiteDef.PageURL))
		{
			if (!this.curWebsiteDef.WasVisted && this.curWebsiteDef.IsTapped && !DataManager.LeetMode)
			{
				this.KeyDiscoveredEvent.Execute();
			}
			this.curWebsiteDef.WasVisted = true;
			int index3 = this.websiteLookUp[this.curWebsiteDef.PageURL];
			this.myWebSitesData.Sites[index3].Visted = true;
			DataManager.Save<WebSitesData>(this.myWebSitesData);
		}
		if (this.curWebPageDef != null)
		{
			this.curWebPageDef.InvokePageEvent();
		}
		return true;
	}

	public void ClearCurrentWebDeff()
	{
		this.curWebsiteDef = null;
		this.curWebPageDef = null;
	}

	public WebPageDefinition GetCurrentWebPageDef()
	{
		return this.curWebPageDef;
	}

	public void GetCurrentPageSourceCode()
	{
		if (this.curWebsiteDef != null && this.curWebPageDef != null)
		{
			string text = this.curWebPageDef.PageHTML;
			bool doSetHTML = false;
			if (this.curWebPageDef != this.lastSourceWebPageDef)
			{
				this.lastSourceWebPageDef = this.curWebPageDef;
				doSetHTML = true;
				if (this.curWebsiteDef.HoldsSecondWikiLink && this.curWebsiteDef.HomePage == this.curWebPageDef)
				{
					int num = UnityEngine.Random.Range(Mathf.RoundToInt((float)text.Length * 0.1f), text.Length);
					string text2 = text.Substring(0, num);
					string text3 = text.Substring(num);
					text = string.Concat(new string[]
					{
						text2,
						"<!-- ",
						this.GetWikiURL(1),
						" -->",
						text3
					});
				}
				if (this.curWebPageDef.IsTapped && this.curWebPageDef.KeyDiscoverMode == KEY_DISCOVERY_MODES.SOURCE_CODE)
				{
					int num2 = UnityEngine.Random.Range(Mathf.RoundToInt((float)text.Length * 0.3f), text.Length);
					string text4 = text.Substring(0, num2);
					string text5 = text.Substring(num2);
					text = string.Concat(new string[]
					{
						text4,
						"<!-- ",
						(this.curWebPageDef.HashIndex + 1).ToString(),
						" - ",
						this.curWebPageDef.HashValue,
						" -->",
						text5
					});
				}
			}
			GameManager.BehaviourManager.SourceViewerBehaviour.ViewSourceCode(text, doSetHTML);
		}
	}

	public bool TriggerBookMark()
	{
		if (!(this.curWebsiteDef != null) || !(this.curWebPageDef != null))
		{
			return false;
		}
		if (!this.bookmarks.ContainsKey(this.curWebPageDef.GetHashCode()))
		{
			string setURL = "http://" + this.curWebsiteDef.PageURL + ".ann/" + this.curWebPageDef.FileName;
			BookmarkData bookmarkData = new BookmarkData(this.curWebPageDef.PageName, setURL);
			this.bookmarks.Add(this.curWebPageDef.GetHashCode(), bookmarkData);
			this.myBookMarksData.BookMarks.Add(this.curWebPageDef.GetHashCode(), bookmarkData);
			GameManager.BehaviourManager.AnnBehaviour.AddBookmarkTab(this.curWebPageDef.GetHashCode(), bookmarkData);
			DataManager.Save<BookMarksData>(this.myBookMarksData);
			return true;
		}
		this.bookmarks.Remove(this.curWebPageDef.GetHashCode());
		this.myBookMarksData.BookMarks.Remove(this.curWebPageDef.GetHashCode());
		GameManager.BehaviourManager.AnnBehaviour.RemoveBookmarkTab(this.curWebPageDef.GetHashCode());
		DataManager.Save<BookMarksData>(this.myBookMarksData);
		return false;
	}

	public bool CheckToSeeIfPageIsBookMarked()
	{
		return this.curWebsiteDef != null && this.curWebPageDef != null && this.bookmarks.ContainsKey(this.curWebPageDef.GetHashCode());
	}

	public bool CheckIfSiteWasTapped()
	{
		return this.curWebPageDef != null && this.curWebPageDef.IsTapped;
	}

	public bool CheckIfWiki()
	{
		return this.curWebsiteDef != null && this.wikiLookUp.ContainsKey(this.curWebsiteDef.PageURL);
	}

	public JSONNode BuildCurrentWiki()
	{
		List<JSONNode> list = new List<JSONNode>(20);
		if (this.curWebsiteDef != null && this.wikiLookUp.ContainsKey(this.curWebsiteDef.PageURL))
		{
			int index = this.wikiLookUp[this.curWebsiteDef.PageURL];
			for (int i = 0; i < this.wikiSites[index].Count; i++)
			{
				int index2 = this.wikiSites[index][i];
				string text = string.Concat(new string[]
				{
					this.Websites[index2].PageURL,
					"|",
					this.Websites[index2].PageTitle,
					"|",
					this.Websites[index2].PageDesc,
					"|"
				});
				if (this.Websites[index2].WasVisted)
				{
					text += "1";
				}
				else
				{
					text += "0";
				}
				list.Add(text);
			}
		}
		return new JSONNode(list);
	}

	public string GetWikiURL(int WikiIndex)
	{
		string result = string.Empty;
		if (this.wikis[WikiIndex] != null)
		{
			result = "http://" + this.wikis[WikiIndex].PageURL + ".ann";
		}
		return result;
	}

	public void ForceKeyDiscover()
	{
		if (this.KeyDiscoveredEvent != null)
		{
			this.KeyDiscoveredEvent.Execute();
		}
	}

	private void prepTheMasterKey()
	{
		this.myMasterKeyData = DataManager.Load<MasterKeyData>(1010);
		if (this.myMasterKeyData == null)
		{
			this.myMasterKeyData = new MasterKeyData(1010);
			this.myMasterKeyData.Keys = new List<string>(8);
			for (int i = 0; i < 8; i++)
			{
				string item = MagicSlinger.MD5It(string.Concat(new object[]
				{
					Time.time.ToString(),
					UnityEngine.Random.Range(0, 99999).ToString(),
					":REFLECTSTUDIOS:",
					Time.deltaTime,
					":",
					i
				})).Substring(0, 12);
				this.myMasterKeyData.Keys.Add(item);
			}
		}
		this.keys = new List<string>(this.myMasterKeyData.Keys);
		for (int j = 0; j < this.keys.Count; j++)
		{
			this.masterKey += this.keys[j];
		}
		DataManager.Save<MasterKeyData>(this.myMasterKeyData);
	}

	private void prepWikis()
	{
		this.myWikiSiteData = DataManager.Load<WikiSiteData>(1919);
		if (this.myWikiSiteData == null)
		{
			this.myWikiSiteData = new WikiSiteData(1919);
			this.myWikiSiteData.Wikis = new List<WebSiteData>(this.wikis.Count);
			this.myWikiSiteData.WikiSites = new List<List<int>>(3);
			List<WebSiteDefinition> list = new List<WebSiteDefinition>(this.Websites);
			List<int> list2 = new List<int>(this.fakeDomains);
			list2.Remove(17);
			list2.Remove(18);
			list2.Remove(20);
			list2.Remove(21);
			list2.Remove(22);
			list2.Remove(23);
			list2.Remove(25);
			list2.Remove(26);
			list2.Remove(27);
			list2.Remove(28);
			list2.Remove(37);
			for (int i = 0; i < this.wikis.Count; i++)
			{
				WebSiteData webSiteData = new WebSiteData();
				webSiteData.PageURL = MagicSlinger.MD5It(this.Websites[i].PageTitle + Time.time.ToString() + UnityEngine.Random.Range(0, 9999).ToString());
				this.myWikiSiteData.Wikis.Add(webSiteData);
			}
			for (int j = list.Count - 1; j >= 0; j--)
			{
				if (list[j].isFake)
				{
					list.RemoveAt(j);
				}
			}
			for (int k = 0; k < 3; k++)
			{
				Dictionary<int, string> dictionary = new Dictionary<int, string>(20);
				List<int> list3 = new List<int>(20);
				int num = 20;
				if (k != 0)
				{
					if (k != 1)
					{
						if (k == 2)
						{
							for (int l = list.Count - 1; l >= 0; l--)
							{
								if (list[l].WikiSpecific && list[l].WikiIndex == 2)
								{
									dictionary.Add(this.websiteLookUp[list[l].PageURL], list[l].PageTitle);
									list.RemoveAt(l);
									num--;
								}
							}
						}
					}
					else
					{
						for (int m = list.Count - 1; m >= 0; m--)
						{
							if (list[m].WikiSpecific && list[m].WikiIndex == 1)
							{
								dictionary.Add(this.websiteLookUp[list[m].PageURL], list[m].PageTitle);
								list.RemoveAt(m);
								num--;
							}
						}
					}
				}
				else
				{
					for (int n = list.Count - 1; n >= 0; n--)
					{
						if (list[n].WikiSpecific && list[n].WikiIndex == 0)
						{
							dictionary.Add(this.websiteLookUp[list[n].PageURL], list[n].PageTitle);
							list.RemoveAt(n);
							num--;
						}
					}
					bool flag = false;
					while (!flag)
					{
						int index = UnityEngine.Random.Range(0, list.Count);
						if (!list[index].WikiSpecific && (!list[index].HasWindow || ModsManager.AlwaysOpenSites))
						{
							list[index].HoldsSecondWikiLink = true;
							this.myWikiSiteData.PickedSiteToHoldSecondWiki = list[index].PageTitle.GetHashCode();
							dictionary.Add(this.websiteLookUp[list[index].PageURL], list[index].PageTitle);
							list.RemoveAt(index);
							num--;
							flag = true;
						}
					}
				}
				int num2 = 0;
				while (num2 < num)
				{
					int index2 = UnityEngine.Random.Range(0, list.Count);
					if (!list[index2].WikiSpecific)
					{
						dictionary.Add(this.websiteLookUp[list[index2].PageURL], list[index2].PageTitle);
						list.RemoveAt(index2);
						num2++;
					}
				}
				for (int num3 = 0; num3 < (ModsManager.AlwaysOpenSites ? 2 : 10); num3++)
				{
					int index3 = UnityEngine.Random.Range(0, list2.Count);
					dictionary.Add(list2[index3], this.Websites[list2[index3]].PageTitle);
					list2.RemoveAt(index3);
				}
				List<KeyValuePair<int, string>> list4 = dictionary.ToList<KeyValuePair<int, string>>();
				list4.Sort((KeyValuePair<int, string> pair1, KeyValuePair<int, string> pair2) => pair1.Value.CompareTo(pair2.Value));
				for (int num4 = 0; num4 < list4.Count; num4++)
				{
					list3.Add(list4[num4].Key);
				}
				this.myWikiSiteData.WikiSites.Add(list3);
			}
		}
		this.wikiLookUp = new Dictionary<string, int>(this.myWikiSiteData.Wikis.Count);
		for (int num5 = 0; num5 < this.myWikiSiteData.Wikis.Count; num5++)
		{
			this.wikis[num5].PageURL = this.myWikiSiteData.Wikis[num5].PageURL;
			this.wikiLookUp.Add(this.myWikiSiteData.Wikis[num5].PageURL, num5);
		}
		for (int num6 = 0; num6 < this.myWikiSiteData.WikiSites.Count; num6++)
		{
			this.wikiSites.Add(this.myWikiSiteData.WikiSites[num6]);
		}
		for (int num7 = 0; num7 < this.Websites.Count; num7++)
		{
			if (this.Websites[num7].PageTitle.GetHashCode() == this.myWikiSiteData.PickedSiteToHoldSecondWiki)
			{
				this.Websites[num7].HoldsSecondWikiLink = true;
			}
			else
			{
				this.Websites[num7].HoldsSecondWikiLink = false;
			}
		}
		DataManager.Save<WikiSiteData>(this.myWikiSiteData);
	}

	private void prepWebsites()
	{
		for (int i = 0; i < this.Websites.Count; i++)
		{
			if (this.Websites[i].PageTitle == "Chosen Awake")
			{
				this.Websites[i].PageTitle = "Chopper";
				this.Websites[i].PageDesc = "Best tutorials for human meat cooking on the Deep Web.";
			}
			if (this.Websites[i].PageTitle == "Illumanti")
			{
				this.Websites[i].PageTitle = "Illumination";
				this.Websites[i].PageDesc = "Website dedicated to showing people's \"bright\" future.";
			}
			if (this.Websites[i].PageTitle == "Roses Destruction")
			{
				this.Websites[i].PageTitle = "Rotten Meal";
				this.Websites[i].PageDesc = "Weird ass people selling meal for canniballs.";
			}
			if (this.Websites[i].PageTitle == "The Doll Maker" || this.Websites[i].PageTitle == "The Bomb Maker")
			{
				this.Websites[i].WikiSpecific = true;
				this.Websites[i].WikiIndex = 2;
			}
			if (this.Websites[i].PageTitle == "Forgive Me")
			{
				this.Websites[i].WikiSpecific = true;
				this.Websites[i].WikiIndex = 0;
				this.Websites[i].isStatic = true;
				this.Websites[i].PageURL = "forgiver4i838pl22t4yk1np3confess";
			}
		}
		new WebsiteExtension().ExtendWebsites(this.Websites);
		this.myWebSitesData = DataManager.Load<WebSitesData>(2020);
		if (this.myWebSitesData == null)
		{
			this.myWebSitesData = new WebSitesData(2020);
			this.myWebSitesData.Sites = new List<WebSiteData>(this.Websites.Count);
			for (int j = 0; j < this.Websites.Count; j++)
			{
				WebSiteData webSiteData = new WebSiteData();
				webSiteData.Pages = new List<WebPageData>();
				if (!this.Websites[j].isStatic)
				{
					webSiteData.PageURL = MagicSlinger.MD5It(this.Websites[j].PageTitle + Time.time.ToString() + UnityEngine.Random.Range(0, 9999).ToString());
				}
				else
				{
					webSiteData.PageURL = this.Websites[j].PageURL;
				}
				webSiteData.Fake = this.Websites[j].isFake;
				webSiteData.Visted = false;
				webSiteData.IsTapped = false;
				WebPageData webPageData = new WebPageData();
				webPageData.KeyDiscoveryMode = UnityEngine.Random.Range(0, 4);
				webPageData.IsTapped = false;
				webPageData.HashIndex = 0;
				webPageData.HashValue = string.Empty;
				webSiteData.Pages.Add(webPageData);
				if (this.Websites[j].SubPages != null)
				{
					for (int k = 0; k < this.Websites[j].SubPages.Count; k++)
					{
						WebPageData webPageData2 = new WebPageData();
						webPageData2.KeyDiscoveryMode = UnityEngine.Random.Range(0, 4);
						webPageData2.IsTapped = false;
						webPageData2.HashIndex = 0;
						webPageData2.HashValue = string.Empty;
						webSiteData.Pages.Add(webPageData2);
					}
				}
				this.myWebSitesData.Sites.Add(webSiteData);
			}
			this.itsNewATap = true;
		}
		this.websiteLookUp = new Dictionary<string, int>(this.myWebSitesData.Sites.Count);
		for (int l = 0; l < this.myWebSitesData.Sites.Count; l++)
		{
			this.Websites[l].PageURL = this.myWebSitesData.Sites[l].PageURL;
			this.Websites[l].WasVisted = this.myWebSitesData.Sites[l].Visted;
			this.Websites[l].IsTapped = this.myWebSitesData.Sites[l].IsTapped;
			if (this.myWebSitesData.Sites[l].Fake)
			{
				this.fakeDomains.Add(l);
			}
			else if (this.myWebSitesData.Sites[l].Pages != null)
			{
				this.Websites[l].HomePage.KeyDiscoverMode = (KEY_DISCOVERY_MODES)this.myWebSitesData.Sites[l].Pages[0].KeyDiscoveryMode;
				this.Websites[l].HomePage.IsTapped = this.myWebSitesData.Sites[l].Pages[0].IsTapped;
				this.Websites[l].HomePage.HashIndex = this.myWebSitesData.Sites[l].Pages[0].HashIndex;
				this.Websites[l].HomePage.HashValue = this.myWebSitesData.Sites[l].Pages[0].HashValue;
				for (int m = 0; m < this.Websites[l].SubPages.Count; m++)
				{
					if (this.myWebSitesData.Sites[l].Pages[m + 1] != null)
					{
						this.Websites[l].SubPages[m].KeyDiscoverMode = (KEY_DISCOVERY_MODES)this.myWebSitesData.Sites[l].Pages[m + 1].KeyDiscoveryMode;
						this.Websites[l].SubPages[m].IsTapped = this.myWebSitesData.Sites[l].Pages[m + 1].IsTapped;
						this.Websites[l].SubPages[m].HashIndex = this.myWebSitesData.Sites[l].Pages[m + 1].HashIndex;
						this.Websites[l].SubPages[m].HashValue = this.myWebSitesData.Sites[l].Pages[m + 1].HashValue;
					}
				}
			}
			this.websiteLookUp.Add(this.myWebSitesData.Sites[l].PageURL, l);
		}
		DataManager.Save<WebSitesData>(this.myWebSitesData);
	}

	private void prepBookmarks()
	{
		this.myBookMarksData = DataManager.Load<BookMarksData>(2021);
		if (this.myBookMarksData == null)
		{
			this.myBookMarksData = new BookMarksData(2021);
			this.myBookMarksData.BookMarks = new Dictionary<int, BookmarkData>();
		}
		this.bookmarks = new Dictionary<int, BookmarkData>(this.myBookMarksData.BookMarks.Count);
		foreach (KeyValuePair<int, BookmarkData> keyValuePair in this.myBookMarksData.BookMarks)
		{
			this.bookmarks.Add(keyValuePair.Key, keyValuePair.Value);
			GameManager.BehaviourManager.AnnBehaviour.AddBookmarkTab(keyValuePair.Key, keyValuePair.Value);
		}
	}

	private void tapSites()
	{
		if (this.itsNewATap)
		{
			List<string> list = new List<string>(this.keys);
			Dictionary<string, int> dictionary = new Dictionary<string, int>(8);
			for (int i = 0; i < this.keys.Count; i++)
			{
				dictionary.Add(this.keys[i], i);
			}
			for (int j = 0; j < this.wikiSites.Count; j++)
			{
				List<int> list2 = new List<int>(this.wikiSites[j]);
				int k = 0;
				while (k < 2)
				{
					int index = UnityEngine.Random.Range(0, list2.Count);
					WebSiteDefinition webSiteDefinition = this.Websites[list2[index]];
					if (!webSiteDefinition.isFake && !webSiteDefinition.DoNotTap && !webSiteDefinition.IsTapped)
					{
						webSiteDefinition.IsTapped = true;
						this.websitesToTap = this.websitesToTap + webSiteDefinition.PageTitle + ":";
						this.myWebSitesData.Sites[list2[index]].IsTapped = true;
						int index2 = UnityEngine.Random.Range(0, list.Count);
						string text = list[index2];
						int hashIndex = dictionary[text];
						if (webSiteDefinition.SubPages.Count > 0)
						{
							if (UnityEngine.Random.Range(0, 4) == 3)
							{
								webSiteDefinition.HomePage.IsTapped = true;
								webSiteDefinition.HomePage.HashIndex = hashIndex;
								webSiteDefinition.HomePage.HashValue = text;
								this.myWebSitesData.Sites[list2[index]].Pages[0].IsTapped = true;
								this.myWebSitesData.Sites[list2[index]].Pages[0].HashIndex = hashIndex;
								this.myWebSitesData.Sites[list2[index]].Pages[0].HashValue = text;
							}
							else
							{
								int num = UnityEngine.Random.Range(0, webSiteDefinition.SubPages.Count);
								webSiteDefinition.SubPages[num].IsTapped = true;
								webSiteDefinition.SubPages[num].HashIndex = hashIndex;
								webSiteDefinition.SubPages[num].HashValue = text;
								if (this.myWebSitesData.Sites[list2[index]].Pages[num + 1] != null)
								{
									this.myWebSitesData.Sites[list2[index]].Pages[num + 1].IsTapped = true;
									this.myWebSitesData.Sites[list2[index]].Pages[num + 1].HashIndex = hashIndex;
									this.myWebSitesData.Sites[list2[index]].Pages[num + 1].HashValue = text;
								}
							}
						}
						else
						{
							webSiteDefinition.HomePage.IsTapped = true;
							webSiteDefinition.HomePage.HashIndex = hashIndex;
							webSiteDefinition.HomePage.HashValue = text;
							this.myWebSitesData.Sites[list2[index]].Pages[0].IsTapped = true;
							this.myWebSitesData.Sites[list2[index]].Pages[0].HashIndex = hashIndex;
							this.myWebSitesData.Sites[list2[index]].Pages[0].HashValue = text;
						}
						list.RemoveAt(index2);
						dictionary.Remove(text);
						k++;
					}
					list2.RemoveAt(index);
				}
			}
			for (int l = 1; l < this.wikiSites.Count; l++)
			{
				List<int> list3 = new List<int>(this.wikiSites[l]);
				int m = 0;
				while (m < 1)
				{
					int index3 = UnityEngine.Random.Range(0, list3.Count);
					WebSiteDefinition webSiteDefinition2 = this.Websites[list3[index3]];
					if (!webSiteDefinition2.isFake && !webSiteDefinition2.DoNotTap && !webSiteDefinition2.IsTapped)
					{
						webSiteDefinition2.IsTapped = true;
						this.websitesToTap = this.websitesToTap + webSiteDefinition2.PageTitle + ":";
						this.myWebSitesData.Sites[index3].IsTapped = true;
						int index4 = UnityEngine.Random.Range(0, list.Count);
						string text2 = list[index4];
						int hashIndex2 = dictionary[text2];
						if (webSiteDefinition2.SubPages.Count > 0)
						{
							if (UnityEngine.Random.Range(0, 4) == 3)
							{
								webSiteDefinition2.HomePage.IsTapped = true;
								webSiteDefinition2.HomePage.HashIndex = hashIndex2;
								webSiteDefinition2.HomePage.HashValue = text2;
								this.myWebSitesData.Sites[list3[index3]].Pages[0].IsTapped = true;
								this.myWebSitesData.Sites[list3[index3]].Pages[0].HashIndex = hashIndex2;
								this.myWebSitesData.Sites[list3[index3]].Pages[0].HashValue = text2;
							}
							else
							{
								int num2 = UnityEngine.Random.Range(0, webSiteDefinition2.SubPages.Count);
								webSiteDefinition2.SubPages[num2].IsTapped = true;
								webSiteDefinition2.SubPages[num2].HashIndex = hashIndex2;
								webSiteDefinition2.SubPages[num2].HashValue = text2;
								if (this.myWebSitesData.Sites[list3[index3]].Pages[num2 + 1] != null)
								{
									this.myWebSitesData.Sites[list3[index3]].Pages[num2 + 1].IsTapped = true;
									this.myWebSitesData.Sites[list3[index3]].Pages[num2 + 1].HashIndex = hashIndex2;
									this.myWebSitesData.Sites[list3[index3]].Pages[num2 + 1].HashValue = text2;
								}
							}
						}
						else
						{
							webSiteDefinition2.HomePage.IsTapped = true;
							webSiteDefinition2.HomePage.HashIndex = hashIndex2;
							webSiteDefinition2.HomePage.HashValue = text2;
							this.myWebSitesData.Sites[list3[index3]].Pages[0].IsTapped = true;
							this.myWebSitesData.Sites[list3[index3]].Pages[0].HashIndex = hashIndex2;
							this.myWebSitesData.Sites[list3[index3]].Pages[0].HashValue = text2;
						}
						list.RemoveAt(index4);
						dictionary.Remove(text2);
						m++;
					}
				}
			}
			DataManager.Save<WebSitesData>(this.myWebSitesData);
		}
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.prepWebsites();
		this.prepWikis();
		this.prepBookmarks();
		this.prepTheMasterKey();
		this.tapSites();
		this.challenge = -1;
		if (ModsManager.DevToolsActive)
		{
			this.myDevTools = new DevTools();
			this.prepareMods(this.myDevTools);
			Debug.Log("DevTools enabled in the cloud.");
		}
		else
		{
			Debug.Log("DevTools are disabled");
		}
		if (ModsManager.DOSTwitchActive)
		{
			this.myDOSTwitch = new DOSTwitch();
			this.PrepTwitchIntegration(this.myDOSTwitch);
			Debug.Log("Twitch integration is active");
		}
		else
		{
			Debug.Log("DOSTwitch is disabled");
		}
		this.nightmarePossible = true;
		GameManager.TimeSlinger.FireTimer(120f, delegate()
		{
			this.nightmarePossible = false;
		}, 0);
		this.LoadMods();
	}

	private void Awake()
	{
		this.GFschedule = false;
		ZeroDayProductObject.isDiscountOn = false;
		ShadowProductObject.isDiscountOn = false;
		if (!TheCloud.vpnFIX)
		{
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[6].isDiscounted = false;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 7].deliveryTimeMax = GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 7].deliveryTimeMin;
			TheCloud.vpnFIX = true;
		}
		GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[2].productToOwn = GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[1];
		if (ModsManager.ShowGodSpot)
		{
			Debug.Log("TheCloud - Show God Spot is ON");
		}
		else
		{
			Debug.Log("TheCloud - Show God Spot is OFF");
		}
		if (ModsManager.UnlimitedStamina)
		{
			Debug.Log("TheCloud - Unlimited Breather Stamina is ON");
		}
		else
		{
			Debug.Log("TheCloud - Unlimited Breather Stamina is OFF");
		}
		if (ModsManager.SBGlitch)
		{
			Debug.Log("TheCloud - Skybreak Wifi Glitch is ON");
		}
		else
		{
			Debug.Log("TheCloud - Skybreak Wifi Glitch is OFF");
		}
		if (ModsManager.EasyModeActive)
		{
			Debug.Log("TheCloud - Easy Mode is ON");
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[0].productPrice = 65f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[1].productPrice = 115f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[2].productPrice = 30f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[3].productPrice = 10f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[4].productPrice = 50f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[5].productPrice = 35f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[6].productPrice = 45f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[0].productPrice = 10f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[1].productPrice = 25f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[2].productPrice = 50f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[3].productPrice = 3f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[4].productPrice = 20f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[5].productPrice = 5f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[6].productPrice = 10f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[7].productPrice = 15f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[8].productPrice = 20f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[9].productPrice = 25f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[10].productPrice = 200f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[11].productPrice = 5f;
		}
		else
		{
			Debug.Log("TheCloud - Easy Mode is OFF");
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[0].productPrice = 160f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[1].productPrice = 225f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[2].productPrice = 45f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[3].productPrice = 25f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[4].productPrice = 150f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[5].productPrice = 135f;
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[6].productPrice = 60f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[0].productPrice = 8f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[1].productPrice = 30f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[2].productPrice = 45f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[3].productPrice = 1f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[4].productPrice = 45f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[5].productPrice = 5f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[6].productPrice = 15f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[7].productPrice = 30f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[8].productPrice = 40f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[9].productPrice = 55f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[10].productPrice = 375f;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[11].productPrice = 8f;
		}
		GameManager.TheCloud = this;
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void Update()
	{
		if (DOSTwitch.dosTwitchEnabled)
		{
			this.myDOSTwitch.Update();
			this.myDOSTwitch.myTwitchIRC.Update();
		}
		if (ModsManager.DebugEnabled)
		{
			UIManager.ShowDebug(string.Concat(new string[]
			{
				"POILCE: ",
				EnemyManager.PoliceManager.PoliceDebug,
				" | CULT: ",
				EnemyManager.CultManager.NoirDebug,
				" | HITMAN: ",
				EnemyManager.HitManManager.LucasDebug,
				" | BOMB_MAKER: ",
				EnemyManager.BombMakerManager.SulphurDebug,
				" | DOLL_MAKER: ",
				EnemyManager.DollMakerManager.MarkerDebug,
				" | HACK: ",
				GameManager.HackerManager.HackDebug,
				" | POWER: ",
				EnvironmentManager.PowerBehaviour.PowerDebug,
				" | SWAN: ",
				GameManager.HackerManager.theSwan.TheSwanDebug,
				" | FREEZE: ",
				GameManager.HackerManager.HackFreezeDebug,
				" | STATE: ",
				EnemyManager.State.ToString()
			}));
		}
	}

	private void PrepTwitchIntegration(DOSTwitch dOSTwitch)
	{
		dOSTwitch.Start();
	}

	private void OnDisable()
	{
		if (DOSTwitch.dosTwitchEnabled)
		{
			this.myDOSTwitch.OnDisable();
		}
		DOSTwitch.dosTwitchEnabled = false;
		SpeedPoll.speedManipulatorActive = false;
		KeyPoll.keyManipulatorData = KEY_CUE_MODE.DEFAULT;
		WiFiPoll.resetWiFiStats();
		DOSCoinPoll.moneyLoan = 0;
		ProductsManager.ownsWhitehatScanner = false;
		ProductsManager.ownsWhitehatRouter = false;
		ProductsManager.ownsWhitehatRemoteVPN2 = false;
		ProductsManager.ownsWhitehatRemoteVPN3 = false;
		ProductsManager.ownsWhitehatDongle2 = false;
		ProductsManager.ownsWhitehatDongle3 = false;
		TrollPoll.isTrollPlaying = false;
		RemoteVPNObject.ObjectBuilt = false;
		RemoteVPNObject.RemoteVPNLevel = 1;
		DevTools.InsanityMode = false;
		DollMakerManager.Lucassed = false;
		WorldManager.LucasSpawnedToKill = false;
		ModsManager.Nightmare = false;
		SulphurInventory.SulphurAmount = 0;
		TarotCardsBehaviour.Owned = false;
		TarotManager.HermitActive = false;
		TarotManager.DizzyActive = false;
		TarotManager.BreatherUndertaker = false;
		TarotManager.TimeController = 30;
		TarotManager.CurSpeed = playerSpeedMode.NORMAL;
		Debug.Log("TheCloud is disabled.");
	}

	private void prepareMods(DevTools _devTools)
	{
		new GameObject("DevTools").AddComponent<DevTools>();
	}

	public void TenTwentyMode()
	{
		this.challenge = 0;
		GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.xor);
		GameManager.TimeSlinger.FireTimer(5f, new Action(this.NewChallenger), 0);
	}

	private void NewChallenger()
	{
		GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.challenger);
		if (this.challenge == 0)
		{
			for (int i = 0; i < 8; i++)
			{
				this.ForceKeyDiscover();
			}
			this.challenge++;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.NewChallenger), 0);
			return;
		}
		if (this.challenge == 1)
		{
			GameManager.TimeSlinger.FireTimer(3f, new Action(EnemyManager.CultManager.attemptSpawn), 0);
			this.challenge++;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.NewChallenger), 0);
			return;
		}
		if (this.challenge == 2)
		{
			GameManager.TimeSlinger.FireTimer(2f, new Action(EnemyManager.DollMakerManager.ForceMarker), 0);
			GameManager.TimeSlinger.FireTimer(2f, new Action(EnemyManager.DollMakerManager.ThrowAllTenants), 0);
			this.challenge++;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.NewChallenger), 0);
			return;
		}
		if (this.challenge == 3)
		{
			GameManager.TimeSlinger.FireTimer(2f, new Action(EnemyManager.BombMakerManager.ReleaseTheBombMakerInstantly), 0);
			this.challenge++;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.NewChallenger), 0);
			return;
		}
		if (this.challenge == 4)
		{
			GameManager.TimeSlinger.FireTimer(2f, new Action(GameManager.TheCloud.ScheduleGoldenFreddy), 0);
			this.challenge++;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.NewChallenger), 0);
			return;
		}
		if (this.challenge == 5)
		{
			GameManager.TimeSlinger.FireTimer(2f, new Action(GameManager.HackerManager.theSwan.ActivateTheSwan), 0);
			this.challenge++;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.NewChallenger), 0);
			return;
		}
		this.challenge = -1;
	}

	public void SpawnManipulatorIcon(float timeFor, Sprite icon, float widthOffset, float heightOffset)
	{
		GameObject gameObject = new GameObject();
		gameObject.AddComponent<Image>().sprite = icon;
		gameObject.GetComponent<RectTransform>().SetParent(LookUp.PlayerUI.HandTransform.transform);
		gameObject.GetComponent<RectTransform>().transform.position = new Vector3((float)Screen.width - widthOffset, heightOffset, 0f);
		gameObject.SetActive(true);
		gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.manipulator);
		GameManager.TimeSlinger.FireTimer(timeFor, delegate()
		{
			UnityEngine.Object.Destroy(gameObject);
		}, 0);
	}

	public void ScheduleGoldenFreddy()
	{
		if (!this.GFschedule)
		{
			this.GFschedule = true;
			GameManager.TimeSlinger.FireTimer(45f, new Action(this.ScheduleGoldenFreddy), 0);
			GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.gfpresence);
			return;
		}
		if (StateManager.PlayerState == PLAYER_STATE.PEEPING || StateManager.PlayerState == PLAYER_STATE.BRACE || StateManager.BeingHacked)
		{
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(5f, 20f), new Action(this.ScheduleGoldenFreddy), 0);
			return;
		}
		GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(150f, 350f), new Action(this.ScheduleGoldenFreddy), 0);
		this.SpawnGF();
	}

	private void SpawnGF()
	{
		GameObject gameObject = new GameObject();
		gameObject.AddComponent<Image>().sprite = Sprite.Create(CustomSpriteLookUp.freddy, new Rect(0f, 0f, 800f, 600f), new Vector2(0.5f, 0.5f), 100f);
		gameObject.GetComponent<RectTransform>().SetParent(LookUp.PlayerUI.HandTransform.transform);
		gameObject.GetComponent<RectTransform>().transform.position = new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f);
		Debug.Log(gameObject.GetComponent<RectTransform>().transform.position.x);
		Debug.Log(gameObject.GetComponent<RectTransform>().transform.position.y);
		Debug.Log(gameObject.GetComponent<RectTransform>().transform.position.z);
		if (Screen.currentResolution.height == 720)
		{
			gameObject.transform.localScale = new Vector3(13f, 7f, 1f);
		}
		else if (Screen.currentResolution.height == 768)
		{
			gameObject.transform.localScale = new Vector3(14f, 8f, 1f);
		}
		else if (Screen.currentResolution.height == 900)
		{
			gameObject.transform.localScale = new Vector3(16f, 9f, 1f);
		}
		else if (Screen.currentResolution.height >= 1080)
		{
			gameObject.transform.localScale = new Vector3(20f, 11f, 1f);
		}
		gameObject.SetActive(true);
		GameManager.AudioSlinger.PlaySoundWithWildPitch(CustomSoundLookUp.gflaugh, 0.77f, 1.22f);
		GameManager.TimeSlinger.FireTimer(0.85f, delegate()
		{
			UnityEngine.Object.Destroy(gameObject);
		}, 0);
	}

	public void attemptNightmare()
	{
		if (this.nightmarePossible)
		{
			ModsManager.Nightmare = true;
			this.TenTwentyMode();
		}
	}

	public void spawnNoir(Vector3 Pos, Vector3 Rot)
	{
		this.dancingNoir.transform.localPosition = Pos;
		this.dancingNoir.transform.localRotation = Quaternion.Euler(Rot);
		this.dancingNoirSpawned = true;
	}

	public void despawnNoir()
	{
		this.dancingNoir.transform.localPosition = Vector3.zero;
		this.dancingNoir.transform.localRotation = Quaternion.Euler(Vector3.zero);
		this.dancingNoirSpawned = false;
	}

	public void instantinateNoir(Vector3 Pos, Vector3 Rot)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dancingNoir);
		gameObject.transform.localPosition = Pos;
		gameObject.transform.localRotation = Quaternion.Euler(Rot);
	}

	public void ForceInsanityEnding()
	{
		this.despawnNoir();
		for (int i = 0; i < 20; i++)
		{
			this.instantinateNoir(new Vector3(UnityEngine.Random.Range(-5f, 5f), 39.582f, UnityEngine.Random.Range(-5f, 5f)), new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 0f));
		}
		DevTools.InsanityMode = true;
		if (TrollPoll.isTrollPlaying)
		{
			GameManager.AudioSlinger.KillSound(TrollPoll.trollAudio);
		}
		else
		{
			TrollPoll.isTrollPlaying = true;
		}
		EnemyManager.State = ENEMY_STATE.CULT;
		GameManager.TimeSlinger.FireTimer(30f, delegate()
		{
			CultComputerJumper.Ins.AddLightsOffJump();
		}, 0);
		GameManager.AudioSlinger.PlaySoundWithCustomDelay(CustomSoundLookUp.party, 0.4f);
		EnvironmentManager.PowerBehaviour.ForcePowerOff();
	}

	private void LoadMods()
	{
		GameManager.TimeSlinger.FireTimer(5f, delegate()
		{
			new GameObject("DancingLoader").AddComponent<DancingLoader>();
			if (ModsManager.DebugEnabled)
			{
				CurrencyManager.AddCurrency(910f);
				KeyPoll.DevEnableManipulator(KEY_CUE_MODE.ENABLED);
				SpeedPoll.DevEnableManipulator(TWITCH_NET_SPEED.FAST);
				GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Wiki2.txt", GameManager.TheCloud.GetWikiURL(1));
				GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Wiki3.txt", GameManager.TheCloud.GetWikiURL(2));
				GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("keys.txt", string.Concat(new object[]
				{
					"- " + TarotManager.tappedSites[0] + "\n",
					"- " + TarotManager.tappedSites[1] + "\n",
					"- " + TarotManager.tappedSites[2] + "\n",
					"- " + TarotManager.tappedSites[3] + "\n",
					"- " + TarotManager.tappedSites[4] + "\n",
					"- " + TarotManager.tappedSites[5] + "\n",
					"- " + TarotManager.tappedSites[6] + "\n",
					"- " + TarotManager.tappedSites[7]
				}));
			}
		}, 0);
		new GameObject("BombMakerManager").AddComponent<BombMakerManager>();
		new GameObject("TarotManager").AddComponent<TarotManager>();
		new GameObject("DeepWebRadioManager").AddComponent<DeepWebRadioManager>();
		TarotManager.tappedSites = this.websitesToTap.Split(new char[]
		{
			':'
		});
	}

	public bool IsGFActive
	{
		get
		{
			return this.GFschedule;
		}
	}

	public CustomEvent KeyDiscoveredEvent = new CustomEvent(6);

	private const string NOT_FOUND_URL = "localGame://NotFound/index.html";

	private const string DOC_ROOT = "localGame://";

	[SerializeField]
	private List<WebSiteDefinition> wikis;

	[SerializeField]
	private List<WebSiteDefinition> Websites;

	private List<string> validDomains = new List<string>();

	private List<int> fakeDomains = new List<int>();

	private List<List<int>> wikiSites = new List<List<int>>();

	private List<string> keys = new List<string>();

	private Dictionary<string, int> wikiLookUp = new Dictionary<string, int>();

	private Dictionary<string, int> websiteLookUp = new Dictionary<string, int>();

	private Dictionary<int, BookmarkData> bookmarks = new Dictionary<int, BookmarkData>();

	private Dictionary<string, string> passwordList = new Dictionary<string, string>();

	private WebSiteDefinition curWebsiteDef;

	private WebSiteDefinition lastSourceWebSiteDef;

	private WebPageDefinition curWebPageDef;

	private WebPageDefinition lastSourceWebPageDef;

	private WikiSiteData myWikiSiteData;

	private WebSitesData myWebSitesData;

	private BookMarksData myBookMarksData;

	private MasterKeyData myMasterKeyData;

	private bool itsNewATap;

	private string masterKey;

	private bool rollCoolDownActive;

	private float rollTimeStamp;

	private DOSTwitch myDOSTwitch;

	private DevTools myDevTools;

	private static bool vpnFIX;

	private int challenge;

	private bool GFschedule;

	private bool nightmarePossible;

	public GameObject dancingNoir;

	public bool dancingNoirSpawned;

	public string websitesToTap;
}
