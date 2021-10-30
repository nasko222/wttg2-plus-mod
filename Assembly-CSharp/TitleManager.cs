using System;
using ASoft.WTTG2;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
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
		this.ShowModMenu();
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
		foreach (TextMeshProUGUI textMeshProUGUI in UnityEngine.Object.FindObjectsOfType<TextMeshProUGUI>())
		{
			if (textMeshProUGUI.gameObject.name == "Ver")
			{
				textMeshProUGUI.text = "V " + ModsManager.ModVersion;
			}
		}
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
		Transform transform = GameObject.Find("NuidtyTitle").transform;
		Transform transform2 = GameObject.Find("GameOptions").transform;
		Transform transform3 = GameObject.Find("Logo").transform;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("NuidtyTitle"));
		gameObject.transform.position = new Vector2(transform.position.x + 600f, transform.position.y + 200f);
		gameObject.GetComponent<TextMeshProUGUI>().text = "WTTG2+ Mod by nasko222 [v" + ModsManager.ModVersion + "]";
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 40f);
		gameObject.transform.SetParent(transform2);
		OptionsUtil.BuildOptionsButton("Twitch Integration:", "[MOD]TTVInt", 1, 50f, null, null);
		OptionsUtil.BuildOptionsButton("Troll Poll:", "[MOD]TrolloPollo", 1, 100f, null, null);
		OptionsUtil.BuildOptionsButton("DevTools:", "[MOD]DevTools", 1, 150f, null, null);
		OptionsUtil.BuildOptionsButton("Easy Mode:", "[MOD]EasyMode", 0, 200f, null, null);
		OptionsUtil.BuildOptionsButton2("Show God Spot:", "[MOD]GODSpot", 0, 50f, null, null);
		OptionsUtil.BuildOptionsButton2("Force Hacks:", "[MOD]ForceHack", 0, 100f, null, null);
		OptionsUtil.BuildOptionsButton2("Unlimited Stamina:", "[MOD]UnlimitedStamina", 0, 150f, null, null);
		OptionsUtil.BuildOptionsButton2("Auto WiFi Crack:", "[MOD]SkybreakGlitch", 0, 200f, null, null);
	}

	private void ShowModMenu()
	{
		Transform transform = GameObject.Find("NuidtyTitle").transform;
		Transform transform2 = GameObject.Find("GameOptions").transform;
		Transform transform3 = GameObject.Find("Logo").transform;
		float num = 0f;
		float num2 = 0f;
		if (Screen.currentResolution.height == 720 || Screen.currentResolution.height == 768)
		{
			num = 350f;
			num2 = 50f;
		}
		if (Screen.currentResolution.height == 900)
		{
			num = 500f;
			num2 = 200f;
		}
		if (Screen.currentResolution.height == 1050)
		{
			num = 600f;
			num2 = 350f;
		}
		if (Screen.currentResolution.height >= 1080)
		{
			num = 700f;
			num2 = 400f;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("NuidtyTitle"));
		gameObject.name = "modmenu1";
		gameObject.transform.position = new Vector2(transform.position.x - num, transform.position.y - num2);
		gameObject.GetComponent<TextMeshProUGUI>().text = "Twitch Integration - " + ((PlayerPrefs.GetInt("[MOD]TTVInt") == 1) ? "ON" : "OFF");
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 40f);
		gameObject.transform.SetParent(transform3);
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("NuidtyTitle"));
		gameObject2.name = "modmenu2";
		gameObject2.transform.position = new Vector2(transform.position.x - num, transform.position.y - num2 - 50f);
		gameObject2.GetComponent<TextMeshProUGUI>().text = "Troll Poll - " + ((PlayerPrefs.GetInt("[MOD]TrolloPollo") == 1) ? "ON" : "OFF");
		gameObject2.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 40f);
		gameObject2.transform.SetParent(transform3);
		GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("NuidtyTitle"));
		gameObject3.name = "modmenu3";
		gameObject3.transform.position = new Vector2(transform.position.x - num, transform.position.y - num2 - 100f);
		gameObject3.GetComponent<TextMeshProUGUI>().text = "DevTools - " + ((PlayerPrefs.GetInt("[MOD]DevTools") == 1) ? "ON" : "OFF");
		gameObject3.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 40f);
		gameObject3.transform.SetParent(transform3);
		GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("NuidtyTitle"));
		gameObject4.name = "modmenu4";
		gameObject4.transform.position = new Vector2(transform.position.x - num, transform.position.y - num2 - 150f);
		gameObject4.GetComponent<TextMeshProUGUI>().text = "Easy Mode - " + ((PlayerPrefs.GetInt("[MOD]EasyMode") == 1) ? "ON" : "OFF");
		gameObject4.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 40f);
		gameObject4.transform.SetParent(transform3);
		GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("NuidtyTitle"));
		gameObject5.name = "modmenu5";
		gameObject5.transform.position = new Vector2(transform.position.x - num, transform.position.y - num2 - 200f);
		gameObject5.GetComponent<TextMeshProUGUI>().text = "Show God Spot - " + ((PlayerPrefs.GetInt("[MOD]GODSpot") == 1) ? "ON" : "OFF");
		gameObject5.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 40f);
		gameObject5.transform.SetParent(transform3);
		GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("NuidtyTitle"));
		gameObject6.name = "modmenu6";
		gameObject6.transform.position = new Vector2(transform.position.x - num, transform.position.y - num2 - 250f);
		gameObject6.GetComponent<TextMeshProUGUI>().text = "Force Hacks - " + ((PlayerPrefs.GetInt("[MOD]ForceHack") == 1) ? "ON" : "OFF");
		gameObject6.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 40f);
		gameObject6.transform.SetParent(transform3);
		GameObject gameObject7 = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("NuidtyTitle"));
		gameObject7.name = "modmenu7";
		gameObject7.transform.position = new Vector2(transform.position.x - num, transform.position.y - num2 - 300f);
		gameObject7.GetComponent<TextMeshProUGUI>().text = "Unlimited Stamina - " + ((PlayerPrefs.GetInt("[MOD]UnlimitedStamina") == 1) ? "ON" : "OFF");
		gameObject7.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 40f);
		gameObject7.transform.SetParent(transform3);
		GameObject gameObject8 = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("NuidtyTitle"));
		gameObject8.name = "modmenu8";
		gameObject8.transform.position = new Vector2(transform.position.x - num, transform.position.y - num2 - 350f);
		gameObject8.GetComponent<TextMeshProUGUI>().text = "Auto WiFi Crack - " + ((PlayerPrefs.GetInt("[MOD]SkybreakGlitch") == 1) ? "ON" : "OFF");
		gameObject8.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 40f);
		gameObject8.transform.SetParent(transform3);
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
