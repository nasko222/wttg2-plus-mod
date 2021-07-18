using System;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

public class TutorialAnnHook : MonoBehaviour
{
	public void LaunchAm()
	{
		this.annWindowRT.sizeDelta = new Vector2((float)Screen.width - 310f, (float)Screen.height - 60f - 40f);
		this.annWindow.SetActive(true);
		GameManager.BehaviourManager.AnnBehaviour.FakePageLoaded.Event += this.amPageLoadedIn;
		GameManager.BehaviourManager.AnnBehaviour.GotoFakeURL("http://2872602c087233465e485c5a4c250851.ann");
	}

	public void PrepVid1()
	{
		this.tutVidBrowser.onLoad += this.vidPageLoadedIn;
		this.tutVidBrowser.LoadURL("localGame://tutVid1/index.html", false);
		this.tutVidBrowser.RegisterFunction("EndVideo", delegate(JSONNode args)
		{
			this.videoIsDonePlaying();
		});
	}

	public void PrepVid2()
	{
		this.tutVidBrowser.onLoad += this.vidPageLoadedIn;
		this.tutVidBrowser.LoadURL("localGame://tutVid2/index.html", false);
		this.tutVidBrowser.RegisterFunction("EndVideo", delegate(JSONNode args)
		{
			this.videoIsDonePlaying();
		});
	}

	public void PrepVid3()
	{
		this.tutVidBrowser.onLoad += this.vidPageLoadedIn;
		this.tutVidBrowser.LoadURL("localGame://tutVid3/index.html", false);
		this.tutVidBrowser.RegisterFunction("EndVideo", delegate(JSONNode args)
		{
			this.videoIsDonePlaying();
		});
	}

	public void HardClear()
	{
		this.tutVidCG.alpha = 0f;
		this.tutVidBrowser.LoadURL("localGame://blank.html", false);
		this.ClearAM();
	}

	public void ClearAM()
	{
		LookUp.DesktopUI.TUT_BROWSER.SetActive(false);
		this.annWindow.SetActive(false);
		this.amHolder.SetActive(false);
		this.browserHolder.SetActive(true);
		GameManager.BehaviourManager.AnnBehaviour.ClearFakeURL();
	}

	private void amPageLoadedIn()
	{
		GameManager.BehaviourManager.AnnBehaviour.FakePageLoaded.Event -= this.amPageLoadedIn;
		this.amHolder.SetActive(true);
		this.browserHolder.SetActive(false);
		TutorialStartBehaviour.Ins.ForceNextStep();
	}

	private void videoIsDonePlaying()
	{
		this.tutVidCG.alpha = 0f;
		TutorialStartBehaviour.Ins.ForceNextStep();
	}

	private void vidPageLoadedIn(JSONNode obj)
	{
		this.tutVidBrowser.onLoad -= this.vidPageLoadedIn;
		GameManager.TimeSlinger.FireTimer(0.1f, delegate()
		{
			this.tutVidCG.alpha = 1f;
		}, 0);
	}

	private void Awake()
	{
		TutorialAnnHook.Ins = this;
		this.annWindowRT = this.annWindow.GetComponent<RectTransform>();
		this.tutVidBrowser = LookUp.DesktopUI.TUT_BROWSER.GetComponent<Browser>();
		this.tutVidCG = LookUp.DesktopUI.TUT_BROWSER.GetComponent<CanvasGroup>();
	}

	public static TutorialAnnHook Ins;

	[SerializeField]
	private GameObject annWindow;

	[SerializeField]
	private GameObject amHolder;

	[SerializeField]
	private GameObject browserHolder;

	private RectTransform annWindowRT;

	private CanvasGroup tutVidCG;

	private Browser tutVidBrowser;
}
