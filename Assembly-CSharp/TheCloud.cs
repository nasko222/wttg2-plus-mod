using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
					if (this.Websites[index2].HasWindow && !ModsManager.EasyModeActive)
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
				int num = 15;
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
						if (!list[index].WikiSpecific && (!list[index].HasWindow || ModsManager.EasyModeActive))
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
				for (int num3 = 0; num3 < 10; num3++)
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
		this.myWebSitesData = DataManager.Load<WebSitesData>(2020);
		if (this.myWebSitesData == null)
		{
			this.myWebSitesData = new WebSitesData(2020);
			this.myWebSitesData.Sites = new List<WebSiteData>(this.Websites.Count);
			for (int i = 0; i < this.Websites.Count; i++)
			{
				WebSiteData webSiteData = new WebSiteData();
				webSiteData.Pages = new List<WebPageData>();
				if (!this.Websites[i].isStatic)
				{
					webSiteData.PageURL = MagicSlinger.MD5It(this.Websites[i].PageTitle + Time.time.ToString() + UnityEngine.Random.Range(0, 9999).ToString());
				}
				else
				{
					webSiteData.PageURL = this.Websites[i].PageURL;
				}
				if (this.Websites[i].PageTitle.ToLower() == "vacation")
				{
					LookUp.SoundLookUp.vacationRinging = this.Websites[i].HomePage.AudioFile;
				}
				if (this.Websites[i].PageTitle.ToLower() == "thehall" || this.Websites[i].PageTitle.ToLower() == "the hall")
				{
					LookUp.SoundLookUp.theHall = this.Websites[i].HomePage.AudioFile;
				}
				if (this.Websites[i].PageTitle.ToLower() == "isevil" || this.Websites[i].PageTitle.ToLower() == "is evil")
				{
					LookUp.SoundLookUp.isEvil = this.Websites[i].HomePage.AudioFile;
				}
				if (this.Websites[i].PageTitle.ToLower() == "oneless")
				{
					LookUp.SoundLookUp.oneless = this.Websites[i].HomePage.AudioFile;
				}
				if (this.Websites[i].PageTitle.ToLower() == "thedollmaker" || this.Websites[i].PageTitle.ToLower() == "the doll maker")
				{
					LookUp.SoundLookUp.dollMusics = this.Websites[i].HomePage.AudioFile;
				}
				if (this.Websites[i].PageTitle.ToLower() == "thanksforvisiting" || this.Websites[i].PageTitle.ToLower() == "thanks for visiting")
				{
					LookUp.SoundLookUp.creepy = this.Websites[i].SubPages[3].AudioFile;
				}
				if (this.Websites[i].PageTitle.ToLower() == "evilcollection" || this.Websites[i].PageTitle.ToLower() == "evil collection")
				{
					LookUp.SoundLookUp.babyTime = this.Websites[i].SubPages[2].AudioFile;
				}
				if (this.Websites[i].PageTitle.ToLower() == "numberstation" || this.Websites[i].PageTitle.ToLower() == "number station")
				{
					LookUp.SoundLookUp.numberStationScare = this.Websites[i].HomePage.AudioFile;
				}
				webSiteData.Fake = this.Websites[i].isFake;
				webSiteData.Visted = false;
				webSiteData.IsTapped = false;
				WebPageData webPageData = new WebPageData();
				webPageData.KeyDiscoveryMode = UnityEngine.Random.Range(0, 4);
				webPageData.IsTapped = false;
				webPageData.HashIndex = 0;
				webPageData.HashValue = string.Empty;
				webSiteData.Pages.Add(webPageData);
				if (this.Websites[i].SubPages != null)
				{
					for (int j = 0; j < this.Websites[i].SubPages.Count; j++)
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
		for (int k = 0; k < this.myWebSitesData.Sites.Count; k++)
		{
			this.Websites[k].PageURL = this.myWebSitesData.Sites[k].PageURL;
			this.Websites[k].WasVisted = this.myWebSitesData.Sites[k].Visted;
			this.Websites[k].IsTapped = this.myWebSitesData.Sites[k].IsTapped;
			if (this.myWebSitesData.Sites[k].Fake)
			{
				this.fakeDomains.Add(k);
			}
			else if (this.myWebSitesData.Sites[k].Pages != null)
			{
				this.Websites[k].HomePage.KeyDiscoverMode = (KEY_DISCOVERY_MODES)this.myWebSitesData.Sites[k].Pages[0].KeyDiscoveryMode;
				this.Websites[k].HomePage.IsTapped = this.myWebSitesData.Sites[k].Pages[0].IsTapped;
				this.Websites[k].HomePage.HashIndex = this.myWebSitesData.Sites[k].Pages[0].HashIndex;
				this.Websites[k].HomePage.HashValue = this.myWebSitesData.Sites[k].Pages[0].HashValue;
				for (int l = 0; l < this.Websites[k].SubPages.Count; l++)
				{
					if (this.myWebSitesData.Sites[k].Pages[l + 1] != null)
					{
						this.Websites[k].SubPages[l].KeyDiscoverMode = (KEY_DISCOVERY_MODES)this.myWebSitesData.Sites[k].Pages[l + 1].KeyDiscoveryMode;
						this.Websites[k].SubPages[l].IsTapped = this.myWebSitesData.Sites[k].Pages[l + 1].IsTapped;
						this.Websites[k].SubPages[l].HashIndex = this.myWebSitesData.Sites[k].Pages[l + 1].HashIndex;
						this.Websites[k].SubPages[l].HashValue = this.myWebSitesData.Sites[k].Pages[l + 1].HashValue;
					}
				}
			}
			this.websiteLookUp.Add(this.myWebSitesData.Sites[k].PageURL, k);
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
			BookmarkData value = new BookmarkData("Adam's Twitter", "http://www.twitter.com/thewebpro");
			BookmarkData value2 = new BookmarkData("Adam's YouTube", "http://www.youtube.com/c/ReflectStudios");
			this.myBookMarksData.BookMarks.Add(14, value);
			this.myBookMarksData.BookMarks.Add(15, value2);
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
						this.myWebSitesData.Sites[index3].IsTapped = true;
						int index4 = UnityEngine.Random.Range(0, list.Count);
						string text2 = list[index4];
						int hashIndex2 = dictionary[text2];
						if (webSiteDefinition2.SubPages.Count > 0)
						{
							if (UnityEngine.Random.Range(0, 10) == 3)
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
			return;
		}
		this.challenge = -1;
		Debug.Log("DOSTwitch is disabled");
	}

	private void Awake()
	{
		ZeroDayProductObject.isDiscountOn = false;
		ShadowProductObject.isDiscountOn = false;
		if (!TheCloud.vpnFIX)
		{
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[6].isDiscounted = false;
			GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[2].productToOwn = GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[1];
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 2].deliveryTimeMax = GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 2].deliveryTimeMin;
			TheCloud.vpnFIX = true;
		}
		if (ModsManager.ShowGodSpot)
		{
			Debug.Log("TheCloud - Show God Spot is ON");
		}
		else
		{
			Debug.Log("TheCloud - Show God Spot is OFF");
		}
		if (ModsManager.ForceHackingEnabled)
		{
			Debug.Log("TheCloud - Force Hacking is ON");
		}
		else
		{
			Debug.Log("TheCloud - Force Hacking is OFF");
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
		this.validDomains.Add("youtube.com");
		this.validDomains.Add("twitch.tv");
		this.validDomains.Add("twitter.com");
		this.validDomains.Add("google.com");
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void Update()
	{
		if (DOSTwitch.dosTwitchEnabled)
		{
			this.myDOSTwitch.Update();
			this.myDOSTwitch.myTwitchIRC.Update();
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
		ProductsManager.ownsWhitehatDongle2 = false;
		ProductsManager.ownsWhitehatDongle3 = false;
		TrollPoll.isTrollPlaying = false;
		RemoteVPNObject.ObjectBuilt = false;
		RemoteVPNObject.RemoteVPNLevel = 1;
		DevTools.InsanityMode = false;
		ModsManager.Nightmare = false;
		Debug.Log("TheCloud is disabled.");
	}

	private void prepareMods(DevTools _devTools)
	{
		new GameObject("DevTools").AddComponent<DevTools>();
	}

	public void TenTwentyMode()
	{
		AudioFileDefinition jumpHit = LookUp.SoundLookUp.JumpHit1;
		jumpHit.AudioClip = DownloadTIFiles.XOR;
		jumpHit.Volume = 1f;
		jumpHit.Loop = false;
		GameManager.AudioSlinger.PlaySound(jumpHit);
		GameManager.TimeSlinger.FireTimer(5f, new Action(this.NewChallenger), 0);
	}

	private void NewChallenger()
	{
		AudioFileDefinition jumpHit = LookUp.SoundLookUp.JumpHit1;
		jumpHit.AudioClip = DownloadTIFiles.Challenger;
		jumpHit.Volume = 1f;
		jumpHit.Loop = false;
		GameManager.AudioSlinger.PlaySound(jumpHit);
		if (this.challenge == 0)
		{
			GameManager.TimeSlinger.FireTimer(2f, new Action(GameManager.HackerManager.theSwan.ActivateTheSwan), 0);
			this.challenge++;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.NewChallenger), 0);
			return;
		}
		if (this.challenge == 1)
		{
			this.challenge++;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.NewChallenger), 0);
			return;
		}
		if (this.challenge == 2)
		{
			GameManager.TimeSlinger.FireTimer(2f, new Action(EnemyManager.DollMakerManager.ForceMarker), 0);
			this.challenge++;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.NewChallenger), 0);
			return;
		}
		if (this.challenge == 3)
		{
			GameManager.TimeSlinger.FireTimer(3f, new Action(EnemyManager.CultManager.attemptSpawn), 0);
			this.challenge++;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.NewChallenger), 0);
			return;
		}
		if (this.challenge == 4)
		{
			if (!DataManager.LeetMode)
			{
				this.ForceKeyDiscover();
				this.ForceKeyDiscover();
				this.ForceKeyDiscover();
				this.ForceKeyDiscover();
				this.ForceKeyDiscover();
				this.ForceKeyDiscover();
			}
			this.challenge++;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.NewChallenger), 0);
			return;
		}
		this.challenge = -1;
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
}
