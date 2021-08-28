using System;
using ASoft.WTTG2;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
	public TitleMainMenuHook MainMenu
	{
		get
		{
			return this.mainMenu;
		}
		set
		{
			this.mainMenu = value;
		}
	}

	public void LogoWasPresented()
	{
		CursorManager.Ins.EnableCursor();
		CursorManager.Ins.SwitchToCustomCursor();
		this.TitlePresented.Execute();
	}

	public void DismissTitle()
	{
		DOTween.To(() => this.blackScreen.alpha, delegate(float x)
		{
			this.blackScreen.alpha = x;
		}, 1f, 0.5f).SetEase(Ease.Linear).SetDelay(1.8f);
		GameManager.AudioSlinger.MuffleAudioHub(AUDIO_HUB.TITLE_HUB, 0f, 2.3f);
		this.TitleDismissing.Execute();
		CursorManager.Ins.DisableCursor();
		CursorManager.Ins.SwitchToDefaultCursor();
		this.loadTimeStamp = Time.time;
		this.loadGameActive = true;
	}

	public void PresentOptions()
	{
		this.OptionsPresent.Execute();
	}

	private void presentTitle()
	{
		DOTween.To(() => this.bgScreen.alpha, delegate(float x)
		{
			this.bgScreen.alpha = x;
		}, 0f, 2f).SetEase(Ease.Linear);
		this.TitlePresent.Execute();
	}

	private void loadGame()
	{
		SceneManager.LoadScene(2, LoadSceneMode.Single);
	}

	private void Awake()
	{
		TitleManager.Ins = this;
	}

	private void Start()
	{
		GameManager.AudioSlinger.PlaySound(this.titleMusic);
		this.TitleStaging.Execute();
		this.startTimeStamp = Time.time;
		this.stageTimeActive = true;
		this.PrepareOptionMods();
	}

	private void Update()
	{
		if (this.stageTimeActive && Time.time - this.startTimeStamp >= this.stageTime)
		{
			this.stageTimeActive = false;
			this.presentTitle();
		}
		if (this.loadGameActive && Time.time - this.loadTimeStamp >= 2.4f)
		{
			this.loadGameActive = false;
			this.loadGame();
		}
	}

	private void PrepareOptionMods()
	{
		UnityEngine.Object.Destroy(GameObject.Find("ApplyButton"));
		OptionsUtil.BuildOptionsButton("Twitch Integration:", "[MOD]TTVInt", 1, 50f, null, null);
		OptionsUtil.BuildOptionsButton("Troll Poll:", "[MOD]TrolloPollo", 1, 100f, null, null);
		OptionsUtil.BuildOptionsButton("DevTools:", "[MOD]DevTools", 1, 150f, null, null);
		OptionsUtil.BuildOptionsButton("Easy Mode:", "[MOD]EasyMode", 0, 200f, null, null);
		OptionsUtil.BuildOptionsButton2("Show God Spot:", "[MOD]GODSpot", 0, 50f, null, null);
		OptionsUtil.BuildOptionsButton2("Force Hacks:", "[MOD]ForceHack", 0, 100f, null, null);
		OptionsUtil.BuildOptionsButton2("Unlimited Stamina:", "[MOD]UnlimitedStamina", 0, 150f, null, null);
		OptionsUtil.BuildOptionsButton2("Auto WiFi Crack:", "[MOD]SkybreakGlitch", 0, 200f, null, null);
	}

	public CustomEvent TitleStaging = new CustomEvent(5);

	public CustomEvent TitlePresent = new CustomEvent(5);

	public CustomEvent TitlePresented = new CustomEvent(5);

	public CustomEvent TitleDismissing = new CustomEvent(5);

	public CustomEvent OptionsPresent = new CustomEvent(5);

	public CustomEvent OptionsPresented = new CustomEvent(5);

	public CustomEvent OptionsDismissing = new CustomEvent(5);

	public CustomEvent OptionsDismissed = new CustomEvent(5);

	public static TitleManager Ins;

	[SerializeField]
	private float stageTime = 0.5f;

	[SerializeField]
	private CanvasGroup bgScreen;

	[SerializeField]
	private CanvasGroup blackScreen;

	[SerializeField]
	private AudioFileDefinition titleMusic;

	private float startTimeStamp;

	private float loadTimeStamp;

	private bool stageTimeActive;

	private bool loadGameActive;

	private TitleMainMenuHook mainMenu;
}
