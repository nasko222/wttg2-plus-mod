using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

public class AnnBehaviour : WindowBehaviour
{
	public void ForceLoadURL(string setURL)
	{
		this.annURLBox.text = setURL;
		this.GotoURL(setURL, true);
	}

	public void GotoURL(string setURL, bool AddHistory = true)
	{
		this.annURLBox.text = setURL;
		if (this.bookmarkMenuActive)
		{
			this.triggerBookmarksMenu();
		}
		if (setURL != this.lastURL)
		{
			this.clearCurrentMusic();
			if (AddHistory && this.lastURL != string.Empty)
			{
				this.backHistory.Add(this.lastURL);
				this.forwardHistory.Clear();
			}
			this.lastURL = setURL;
			float num = 0.75f;
			string callBackValue;
			if (this.userIsOffline)
			{
				GameManager.TheCloud.InvalidURL(out callBackValue);
			}
			else if (SpeedPoll.speedManipulatorActive)
			{
				GameManager.TheCloud.ValidateURL(out callBackValue, setURL);
				num = GameManager.ManagerSlinger.WifiManager.GenereatePageLoadingTime(SpeedPoll.speedManipulatorData);
			}
			else
			{
				GameManager.TheCloud.ValidateURL(out callBackValue, setURL);
				num = GameManager.ManagerSlinger.WifiManager.GenereatePageLoadingTime();
			}
			LookUp.DesktopUI.ANN_WINDOW_HOME_BTN.setLock(true);
			LookUp.DesktopUI.ANN_WINDOW_BACK_BTN.setLock(true);
			LookUp.DesktopUI.ANN_WINDOW_FORWARD_BTN.setLock(true);
			LookUp.DesktopUI.ANN_WINDOW_REFRESH_BTN.setLock(true);
			LookUp.DesktopUI.ANN_WINDOW_CODE_BTN.setLock(true);
			this.annURLBox.enabled = false;
			this.loadingPage = true;
			this.aniLoadingPage(num);
			GameManager.TimeSlinger.FireHardTimer<string>(out this.annLoadPageTimer, num, new Action<string>(this.loadBrowserURL), callBackValue, 0);
		}
	}

	public void GotoFakeURL(string FakeURL)
	{
		float num = 1.25f;
		this.annURLBox.text = FakeURL;
		LookUp.DesktopUI.ANN_WINDOW_HOME_BTN.setLock(true);
		LookUp.DesktopUI.ANN_WINDOW_BACK_BTN.setLock(true);
		LookUp.DesktopUI.ANN_WINDOW_FORWARD_BTN.setLock(true);
		LookUp.DesktopUI.ANN_WINDOW_REFRESH_BTN.setLock(true);
		LookUp.DesktopUI.ANN_WINDOW_CODE_BTN.setLock(true);
		this.loadingPage = true;
		this.aniLoadingPage(num);
		GameManager.TimeSlinger.FireTimer(num, delegate()
		{
			LookUp.DesktopUI.ANN_WINDOW_HOME_BTN.setLock(false);
			LookUp.DesktopUI.ANN_WINDOW_BACK_BTN.setLock(false);
			LookUp.DesktopUI.ANN_WINDOW_FORWARD_BTN.setLock(false);
			LookUp.DesktopUI.ANN_WINDOW_REFRESH_BTN.setLock(false);
			LookUp.DesktopUI.ANN_WINDOW_CODE_BTN.setLock(false);
			this.loadingPage = false;
			this.aniLoadingBarSeq.Kill(false);
			this.aniLoadingGlobeSeq.Kill(false);
			LookUp.DesktopUI.ANN_WINDOW_LOADING_BAR.fillAmount = 0f;
			LookUp.DesktopUI.ANN_WINDOW_GLOBE.alpha = 1f;
			this.FakePageLoaded.Execute();
		}, 0);
	}

	public void ClearFakeURL()
	{
		this.annURLBox.text = string.Empty;
	}

	public void AnnBTNAction(ANN_BTN_ACTIONS Action)
	{
		switch (Action)
		{
		case ANN_BTN_ACTIONS.BACK:
			this.goBack();
			break;
		case ANN_BTN_ACTIONS.FORWARD:
			this.goForward();
			break;
		case ANN_BTN_ACTIONS.REFRESH:
			this.refreshPage();
			break;
		case ANN_BTN_ACTIONS.BOOKMARKS:
			this.triggerBookmarksMenu();
			break;
		case ANN_BTN_ACTIONS.HOME:
			this.GotoURL(GameManager.TheCloud.GetWikiURL(0), true);
			break;
		case ANN_BTN_ACTIONS.CODE:
			GameManager.TheCloud.GetCurrentPageSourceCode();
			break;
		}
	}

	public void AddBookmarkTab(int SetHashCode, BookmarkData SetBookmarkData)
	{
		float setY = -(28f * (float)this.currentBookmarkTabs.Count + 2f * (float)this.currentBookmarkTabs.Count);
		BookmarkTABObject bookmarkTABObject = this.bookmarkTabObjectPool.Pop();
		bookmarkTABObject.Build(SetBookmarkData, setY);
		this.currentBookmarkTabs.Add(SetHashCode, bookmarkTABObject);
		this.bookmarksTabHolderSize.y = 30f * (float)this.currentBookmarkTabs.Count;
		this.BookmarksMenuTabHolder.GetComponent<RectTransform>().sizeDelta = this.bookmarksTabHolderSize;
	}

	public void RemoveBookmarkTab(int SetHashCode)
	{
		BookmarkTABObject bookmarkTABObject;
		if (this.currentBookmarkTabs.TryGetValue(SetHashCode, out bookmarkTABObject))
		{
			bookmarkTABObject.KillMe();
			this.currentBookmarkTabs.Remove(SetHashCode);
			this.bookmarkTabObjectPool.Push(bookmarkTABObject);
			this.rePOSBookmarkTabs();
		}
	}

	protected override void OnLaunch()
	{
		if (!this.Window.activeSelf && this.myBrowser == null)
		{
			this.myBrowser = this.browserObject.GetComponent<Browser>();
			this.myBrowser.onLoad += this.pageLoaded;
		}
	}

	protected override void OnClose()
	{
		this.clearCurrentMusic();
		if (this.loadingPage)
		{
			GameManager.TimeSlinger.KillTimer(this.annLoadPageTimer);
			this.aniLoadingPageStop();
		}
		if (this.myBrowser != null)
		{
			this.myBrowser.CookieManager.ClearAll();
			this.loadBrowserURL("localGame://blank.html");
		}
		this.lastURL = string.Empty;
		this.annURLBox.text = string.Empty;
		this.backHistory.Clear();
		this.forwardHistory.Clear();
		if (this.bookmarkMenuActive)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.Insert(0f, DOTween.To(() => this.BookmarksMenu.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
			{
				this.BookmarksMenu.GetComponent<RectTransform>().localPosition = x;
			}, new Vector3(237f, 0f, 0f), 0.01f).SetRelative(true).SetEase(Ease.OutSine));
			sequence.Play<Sequence>();
			this.bookmarkMenuActive = false;
		}
	}

	protected override void OnMin()
	{
	}

	protected override void OnUnMin()
	{
	}

	protected override void OnMax()
	{
	}

	protected override void OnUnMax()
	{
	}

	protected override void OnResized()
	{
	}

	public void goOffLine()
	{
		if (this.loadingPage)
		{
			GameManager.TimeSlinger.KillTimer(this.annLoadPageTimer);
			this.aniLoadingPageStop();
		}
		if (this.myBrowser != null)
		{
			this.myBrowser.LoadURL("localGame://NotFound/index.html", false);
		}
		this.clearCurrentMusic();
	}

	private void loadBrowserURL(string setURL)
	{
		if (setURL != string.Empty)
		{
			this.myBrowser.LoadURL(setURL, false);
		}
	}

	private void pageLoaded(JSONNode obj)
	{
		string empty = string.Empty;
		if (GameManager.TheCloud.SoftValidateURL(out empty, this.myBrowser.Url))
		{
			this.registerPageJS();
			this.checkToSeeIfPageHasMusic();
			this.checkToSeeIfPageIsTapped();
			this.thePrey();
			this.aniLoadingPageStop();
			this.checkToSeeIfPageIsBookmakred();
			this.annBTNCheck();
			return;
		}
		this.myBrowser.LoadURL(empty, true);
	}

	public void aniLoadingPageStop()
	{
		this.aniLoadingBarSeq.Kill(false);
		this.aniLoadingGlobeSeq.Kill(false);
		LookUp.DesktopUI.ANN_WINDOW_LOADING_BAR.fillAmount = 0f;
		LookUp.DesktopUI.ANN_WINDOW_GLOBE.alpha = 1f;
		LookUp.DesktopUI.ANN_WINDOW_HOME_BTN.setLock(false);
		LookUp.DesktopUI.ANN_WINDOW_BACK_BTN.setLock(false);
		LookUp.DesktopUI.ANN_WINDOW_FORWARD_BTN.setLock(false);
		LookUp.DesktopUI.ANN_WINDOW_REFRESH_BTN.setLock(false);
		LookUp.DesktopUI.ANN_WINDOW_CODE_BTN.setLock(false);
		this.annURLBox.enabled = true;
		this.loadingPage = false;
	}

	private void checkToSeeIfPageHasMusic()
	{
		if (GameManager.TheCloud.GetCurrentWebPageDef() != null && GameManager.TheCloud.GetCurrentWebPageDef().HasMusic)
		{
			GameManager.AudioSlinger.PlaySound(GameManager.TheCloud.GetCurrentWebPageDef().AudioFile);
		}
	}

	private void checkToSeeIfPageIsTapped()
	{
		if (GameManager.TheCloud.CheckIfSiteWasTapped())
		{
			WebPageDefinition pageDef = GameManager.TheCloud.GetCurrentWebPageDef();
			if (KeyPoll.keyManipulatorData == KEY_CUE_MODE.DEFAULT)
			{
				if (InventoryManager.OwnsKeyCue)
				{
					LookUp.DesktopUI.ANN_KEY_CUE.enabled = true;
				}
			}
			else if (KeyPoll.keyManipulatorData == KEY_CUE_MODE.ENABLED)
			{
				LookUp.DesktopUI.ANN_KEY_CUE.enabled = true;
			}
			KEY_DISCOVERY_MODES keyDiscoverMode = pageDef.KeyDiscoverMode;
			if (keyDiscoverMode != KEY_DISCOVERY_MODES.PLAIN_SIGHT)
			{
				if (keyDiscoverMode == KEY_DISCOVERY_MODES.CLICK_TO_PLAIN_SIGHT)
				{
					this.myBrowser.CallFunction("PickCPTag", new JSONNode[]
					{
						string.Empty
					});
					this.myBrowser.RegisterFunction("PlainKeyShow", delegate(JSONNode args)
					{
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
						this.myBrowser.CallFunction("PickPTag", new JSONNode[]
						{
							new JSONNode((double)(pageDef.HashIndex + 1)),
							new JSONNode(pageDef.HashValue)
						});
					});
					return;
				}
				if (keyDiscoverMode == KEY_DISCOVERY_MODES.CLICK_TO_FILE)
				{
					this.myBrowser.CallFunction("PickCFTag", new JSONNode[]
					{
						string.Empty
					});
					this.myBrowser.RegisterFunction("FileKeyShow", delegate(JSONNode args)
					{
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Key" + (pageDef.HashIndex + 1).ToString() + ".txt", (pageDef.HashIndex + 1).ToString() + " - " + pageDef.HashValue);
					});
					return;
				}
			}
			else
			{
				this.myBrowser.CallFunction("PickPTag", new JSONNode[]
				{
					new JSONNode((double)(pageDef.HashIndex + 1)),
					new JSONNode(pageDef.HashValue)
				});
			}
		}
	}

	private void checkToSeeIfPageIsBookmakred()
	{
		if (GameManager.TheCloud.CheckToSeeIfPageIsBookMarked())
		{
			LookUp.DesktopUI.ANN_WINDOW_BOOKMARK_BTN.setBookmarked(true);
		}
		else
		{
			LookUp.DesktopUI.ANN_WINDOW_BOOKMARK_BTN.setBookmarked(false);
		}
	}

	private void clearCurrentMusic()
	{
		LookUp.DesktopUI.ANN_KEY_CUE.enabled = false;
		if (GameManager.TheCloud.GetCurrentWebPageDef() != null && GameManager.TheCloud.GetCurrentWebPageDef().HasMusic)
		{
			GameManager.AudioSlinger.KillSound(GameManager.TheCloud.GetCurrentWebPageDef().AudioFile);
		}
		GameManager.TheCloud.ClearCurrentWebDeff();
	}

	private void aniLoadingPage(float setLoadingTime)
	{
		this.aniLoadingBarSeq.Kill(false);
		this.aniLoadingGlobeSeq.Kill(false);
		this.aniLoadingGlobeSeq = DOTween.Sequence();
		this.aniLoadingGlobeSeq.Insert(0f, DOTween.To(() => LookUp.DesktopUI.ANN_WINDOW_GLOBE.alpha, delegate(float x)
		{
			LookUp.DesktopUI.ANN_WINDOW_GLOBE.alpha = x;
		}, 0.3f, 0.5f)).SetEase(Ease.Linear);
		this.aniLoadingGlobeSeq.Insert(0.5f, DOTween.To(() => LookUp.DesktopUI.ANN_WINDOW_GLOBE.alpha, delegate(float x)
		{
			LookUp.DesktopUI.ANN_WINDOW_GLOBE.alpha = x;
		}, 1f, 0.5f)).SetEase(Ease.Linear);
		this.aniLoadingGlobeSeq.SetLoops(-1);
		this.aniLoadingGlobeSeq.Play<Sequence>();
		LookUp.DesktopUI.ANN_WINDOW_LOADING_BAR.fillAmount = 0f;
		this.aniLoadingBarSeq = DOTween.Sequence();
		this.aniLoadingBarSeq.Insert(0f, DOTween.To(() => LookUp.DesktopUI.ANN_WINDOW_LOADING_BAR.fillAmount, delegate(float x)
		{
			LookUp.DesktopUI.ANN_WINDOW_LOADING_BAR.fillAmount = x;
		}, 1f, setLoadingTime));
		this.aniLoadingBarSeq.Play<Sequence>();
	}

	private void registerPageJS()
	{
		this.myBrowser.RegisterFunction("LinkHover", delegate(JSONNode args)
		{
			if (!this.loadingPage)
			{
				GameManager.ManagerSlinger.CursorManager.PointerCursorState(true);
			}
		});
		this.myBrowser.RegisterFunction("LinkOut", delegate(JSONNode args)
		{
			if (!this.loadingPage)
			{
				GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
			}
		});
		this.myBrowser.RegisterFunction("LinkClick", delegate(JSONNode args)
		{
			if (!this.loadingPage)
			{
				GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.MouseClick);
				string pageFile = args[0];
				GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
				this.pageLinkClick(pageFile);
			}
		});
		this.myBrowser.RegisterFunction("EmptyClick", delegate(JSONNode args)
		{
			if (!this.loadingPage)
			{
				GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.MouseClick);
			}
		});
		this.myBrowser.RegisterFunction("SiteClick", delegate(JSONNode args)
		{
			if (!this.loadingPage)
			{
				GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.MouseClick);
				string setURL = args[0];
				GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
				this.GotoURL(setURL, true);
			}
		});
		this.myBrowser.RegisterFunction("KeyPress", delegate(JSONNode args)
		{
			if (!this.loadingPage)
			{
				int num = UnityEngine.Random.Range(1, LookUp.SoundLookUp.KeyboardSounds.Length);
				AudioFileDefinition audioFileDefinition = LookUp.SoundLookUp.KeyboardSounds[num];
				GameManager.AudioSlinger.PlaySound(audioFileDefinition);
				LookUp.SoundLookUp.KeyboardSounds[num] = LookUp.SoundLookUp.KeyboardSounds[num];
				LookUp.SoundLookUp.KeyboardSounds[0] = audioFileDefinition;
			}
		});
		this.myBrowser.RegisterFunction("HolyClick", delegate(JSONNode args)
		{
			if (!this.loadingPage)
			{
				GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
				GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.HolyClick);
			}
		});
		this.myBrowser.RegisterFunction("puzzleClickedGood", delegate(JSONNode args)
		{
			if (!this.loadingPage)
			{
				GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
				GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.PuzzleGoodClick);
			}
		});
		this.myBrowser.RegisterFunction("puzzleClickedWrong", delegate(JSONNode args)
		{
			if (!this.loadingPage)
			{
				GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
				GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.PuzzleFailClick);
			}
		});
		this.myBrowser.RegisterFunction("puzzleSolved", delegate(JSONNode args)
		{
			if (!this.loadingPage)
			{
				GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
				GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.PuzzleSolved);
				this.myBrowser.CallFunction("presentWiki", new JSONNode[]
				{
					new JSONNode(GameManager.TheCloud.GetWikiURL(2))
				});
			}
		});
		if (!OptionDataHook.Ins.Options.Nudity)
		{
			this.myBrowser.CallFunction("ApplyNudityFilter", new JSONNode[]
			{
				string.Empty
			});
		}
		if (GameManager.TheCloud.CheckIfWiki())
		{
			this.myBrowser.CallFunction("BuildWiki", new JSONNode[]
			{
				GameManager.TheCloud.BuildCurrentWiki()
			});
		}
	}

	private void pageLinkClick(string pageFile)
	{
		string text = this.lastURL;
		text = text.Replace("http://", string.Empty);
		text = text.Replace(".ann", string.Empty);
		string[] array = text.Split(new string[]
		{
			"/"
		}, StringSplitOptions.None);
		if (array.Length > 0)
		{
			text = "http://" + array[0] + ".ann/" + pageFile;
			this.ForceLoadURL(text);
		}
	}

	private void triggerBookmarksMenu()
	{
		if (!this.bookmarkMenuAniActive)
		{
			this.bookmarkMenuAniActive = true;
			GameManager.TimeSlinger.FireTimer(0.3f, delegate()
			{
				this.bookmarkMenuAniActive = false;
			}, 0);
			if (this.bookmarkMenuActive)
			{
				this.bookmarkMenuActive = false;
				Sequence sequence = DOTween.Sequence();
				sequence.Insert(0f, DOTween.To(() => this.BookmarksMenu.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
				{
					this.BookmarksMenu.GetComponent<RectTransform>().localPosition = x;
				}, new Vector3(237f, 0f, 0f), 0.25f).SetRelative(true).SetEase(Ease.OutSine));
				sequence.Play<Sequence>();
			}
			else
			{
				this.bookmarkMenuActive = true;
				Sequence sequence2 = DOTween.Sequence();
				sequence2.Insert(0f, DOTween.To(() => this.BookmarksMenu.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
				{
					this.BookmarksMenu.GetComponent<RectTransform>().localPosition = x;
				}, new Vector3(-237f, 0f, 0f), 0.25f).SetRelative(true).SetEase(Ease.InSine));
				sequence2.Play<Sequence>();
			}
		}
	}

	private void rePOSBookmarkTabs()
	{
		int num = 0;
		foreach (KeyValuePair<int, BookmarkTABObject> keyValuePair in this.currentBookmarkTabs)
		{
			float setY = -(28f * (float)num + 2f * (float)num);
			keyValuePair.Value.RePOSMe(setY);
			num++;
		}
		this.bookmarksTabHolderSize.y = 30f * (float)this.currentBookmarkTabs.Count;
		this.BookmarksMenuTabHolder.GetComponent<RectTransform>().sizeDelta = this.bookmarksTabHolderSize;
	}

	private void refreshPage()
	{
		string text = this.lastURL;
		this.lastURL = string.Empty;
		this.annURLBox.text = text;
		this.GotoURL(text, false);
	}

	private void goBack()
	{
		if (this.backHistory.Count != 0)
		{
			this.forwardHistory.Add(this.lastURL);
			string text = this.backHistory[this.backHistory.Count - 1];
			this.backHistory.RemoveAt(this.backHistory.Count - 1);
			this.annURLBox.text = text;
			this.GotoURL(text, false);
		}
	}

	private void goForward()
	{
		if (this.forwardHistory.Count != 0)
		{
			this.backHistory.Add(this.lastURL);
			string text = this.forwardHistory[this.forwardHistory.Count - 1];
			this.forwardHistory.RemoveAt(this.forwardHistory.Count - 1);
			this.annURLBox.text = text;
			this.GotoURL(text, false);
		}
	}

	private void annBTNCheck()
	{
		if (this.backHistory.Count > 0)
		{
			LookUp.DesktopUI.ANN_WINDOW_BACK_BTN.setActive(true);
		}
		else
		{
			LookUp.DesktopUI.ANN_WINDOW_BACK_BTN.setActive(false);
		}
		if (this.forwardHistory.Count > 0)
		{
			LookUp.DesktopUI.ANN_WINDOW_FORWARD_BTN.setActive(true);
		}
		else
		{
			LookUp.DesktopUI.ANN_WINDOW_FORWARD_BTN.setActive(false);
		}
		if (this.lastURL != string.Empty)
		{
			LookUp.DesktopUI.ANN_WINDOW_REFRESH_BTN.setActive(true);
			LookUp.DesktopUI.ANN_WINDOW_CODE_BTN.setActive(true);
		}
		else
		{
			LookUp.DesktopUI.ANN_WINDOW_REFRESH_BTN.setActive(false);
			LookUp.DesktopUI.ANN_WINDOW_CODE_BTN.setActive(false);
		}
	}

	private void stageMe()
	{
		GameManager.StageManager.TheGameIsLive -= this.stageMe;
		GameManager.ManagerSlinger.WifiManager.WentOnline += this.userWentOnline;
		GameManager.ManagerSlinger.WifiManager.WentOffline += this.userWentOffLine;
	}

	private void userWentOnline()
	{
		this.userIsOffline = false;
	}

	private void userWentOffLine()
	{
		this.userIsOffline = true;
		this.goOffLine();
	}

	protected new void Awake()
	{
		base.Awake();
		GameManager.BehaviourManager.AnnBehaviour = this;
		this.browserObject = LookUp.DesktopUI.ANN_WINDOW_BROWSER_OBJECT;
		this.annURLBox = LookUp.DesktopUI.ANN_WINDOW_URL_BOX;
		this.bookmarksBTN = LookUp.DesktopUI.ANN_WINDOW_BOOKMARKS_BTN;
		this.BookmarksMenu = LookUp.DesktopUI.ANN_WINDOW_BOOKMARKS_MENU;
		this.BookmarksMenuTabHolder = LookUp.DesktopUI.ANN_WINDOW_BOOKMARKS_MENU_TAB_HOLDER;
		this.BookmarkTabObject = LookUp.DesktopUI.ANN_WINDOW_BOOKMARKS_TAB_OBJECT;
		this.bookmarkTabObjectPool = new PooledStack<BookmarkTABObject>(delegate()
		{
			BookmarkTABObject component = UnityEngine.Object.Instantiate<GameObject>(this.BookmarkTabObject, this.BookmarksMenuTabHolder.GetComponent<RectTransform>()).GetComponent<BookmarkTABObject>();
			component.SoftBuild();
			return component;
		}, this.BOOKMARK_TAB_START_POOL_COUNT);
		GameManager.StageManager.Stage += this.stageMe;
	}

	protected new void Update()
	{
		base.Update();
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
	}

	public void TwitchTrollURL(string SetURL)
	{
		this.goOffLine();
		this.myBrowser.LoadURL(SetURL, true);
	}

	public bool isThePageLoading
	{
		get
		{
			return this.loadingPage;
		}
	}

	private void thePrey()
	{
		if ((GameManager.TheCloud.GetCurrentWebPageDef() != null && GameManager.TheCloud.GetCurrentWebPageDef().PageName.ToLower() == "the prey") || (GameManager.TheCloud.GetCurrentWebPageDef() != null && GameManager.TheCloud.GetCurrentWebPageDef().PageName.ToLower() == "theprey"))
		{
			EnemyManager.CultManager.attemptSpawn();
		}
	}

	public int BOOKMARK_TAB_START_POOL_COUNT = 10;

	private const string NOT_FOUND_URL = "localGame://NotFound/index.html";

	public CustomEvent FakePageLoaded = new CustomEvent(2);

	private Browser myBrowser;

	private GameObject browserObject;

	private InputField annURLBox;

	private GameObject bookmarksBTN;

	private GameObject BookmarksMenu;

	private GameObject BookmarksMenuTabHolder;

	private GameObject BookmarksMenuScrollBar;

	private GameObject BookmarkTabObject;

	private PooledStack<BookmarkTABObject> bookmarkTabObjectPool;

	private Dictionary<int, BookmarkTABObject> currentBookmarkTabs = new Dictionary<int, BookmarkTABObject>();

	private List<string> backHistory = new List<string>();

	private List<string> forwardHistory = new List<string>();

	private Vector2 bookmarksTabHolderSize = Vector2.zero;

	private Sequence aniLoadingBarSeq;

	private Sequence aniLoadingGlobeSeq;

	private Timer annLoadPageTimer;

	private bool isOpened;

	private bool isMin;

	private bool loadingPage;

	private bool bookmarkMenuActive;

	private bool bookmarkMenuAniActive;

	private bool userIsOffline;

	private string lastURL = string.Empty;
}
