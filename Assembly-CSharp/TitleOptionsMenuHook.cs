using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TitleOptionsMenuHook : MonoBehaviour
{
	private void presentMe()
	{
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.myCG.interactable = true;
			this.myCG.blocksRaycasts = true;
		});
	}

	private void dismissMe()
	{
		this.myCG.interactable = false;
		this.myCG.blocksRaycasts = false;
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			TitleManager.Ins.OptionsDismissing.Execute();
		});
	}

	private void resoultionChange(float value)
	{
		int index = Mathf.RoundToInt(value);
		int key = this.screenSizeLookUp[index];
		string text = this.screenSizes[key].width.ToString() + "x" + this.screenSizes[key].height.ToString();
		this.resoultionValue.SetText(text);
	}

	private void qualityChange(float value)
	{
		int num = Mathf.RoundToInt(value);
		this.qualityValue.SetText(this.quailtySettingNames[num]);
	}

	private void applySettings()
	{
		int index = Mathf.RoundToInt(this.resoultionSlider.value);
		int key = this.screenSizeLookUp[index];
		bool fullscreen = !this.windowModeOnBTN.Active;
		int vSyncCount = (!this.vSyncOnBTN.Active) ? 0 : 1;
		int num = Mathf.RoundToInt(this.qualitySlider.value);
		this.myOptionData.ScreenWidth = this.screenSizes[key].width;
		this.myOptionData.ScreenHeight = this.screenSizes[key].height;
		this.myOptionData.QualitySettingIndex = num;
		this.myOptionData.WindowMode = this.windowModeOnBTN.Active;
		this.myOptionData.VSync = this.vSyncOnBTN.Active;
		this.myOptionData.Mic = this.micOnBTN.Active;
		this.myOptionData.Nudity = this.nudityOnBTN.Active;
		DataManager.SaveOption<Options>(this.myOptionData);
		DataManager.WriteOptionData();
		QualitySettings.SetQualityLevel(num, true);
		Screen.SetResolution(this.screenSizes[key].width, this.screenSizes[key].height, fullscreen);
		QualitySettings.vSyncCount = vSyncCount;
		TitleOptionsMenuHook.SettingsApplied.Execute();
	}

	private void Awake()
	{
		TitleOptionsMenuHook.Ins = this;
		this.myOptionData = DataManager.LoadOption<Options>(12);
		if (this.myOptionData == null)
		{
			this.myOptionData = new Options(12);
			this.myOptionData.ScreenWidth = Screen.width;
			this.myOptionData.ScreenHeight = Screen.height;
			this.myOptionData.QualitySettingIndex = QualitySettings.GetQualityLevel();
			this.myOptionData.WindowMode = !Screen.fullScreen;
			this.myOptionData.VSync = (QualitySettings.vSyncCount > 0);
			this.myOptionData.Mic = true;
			this.myOptionData.Nudity = true;
			this.myOptionData.MouseSens = 2;
			DataManager.SaveOption<Options>(this.myOptionData);
			DataManager.WriteOptionData();
		}
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myCG.interactable = false;
		this.myCG.blocksRaycasts = false;
		foreach (Resolution value in Screen.resolutions)
		{
			int num = value.width + value.height;
			if (value.height >= 720 && !this.screenSizes.ContainsKey(num))
			{
				this.screenSizes.Add(num, value);
				this.screenSizeLookUp.Add(num);
			}
		}
		int num2 = 0;
		int num3 = 0;
		foreach (KeyValuePair<int, Resolution> keyValuePair in this.screenSizes)
		{
			if (Screen.width == keyValuePair.Value.width && Screen.height == keyValuePair.Value.height)
			{
				num2 = num3;
			}
			num3++;
		}
		this.resoultionSlider.wholeNumbers = true;
		this.resoultionSlider.minValue = 0f;
		this.resoultionSlider.maxValue = (float)(this.screenSizeLookUp.Count - 1);
		this.resoultionSlider.onValueChanged.AddListener(new UnityAction<float>(this.resoultionChange));
		this.resoultionSlider.value = (float)num2;
		this.quailtySettingNames = QualitySettings.names;
		this.qualitySlider.wholeNumbers = true;
		this.qualitySlider.minValue = 0f;
		this.qualitySlider.maxValue = (float)(this.quailtySettingNames.Length - 1);
		this.qualitySlider.onValueChanged.AddListener(new UnityAction<float>(this.qualityChange));
		this.qualitySlider.value = (float)QualitySettings.GetQualityLevel();
		if (Screen.fullScreen)
		{
			this.windowModeOffBTN.SetActive();
		}
		else
		{
			this.windowModeOnBTN.SetActive();
		}
		if (QualitySettings.vSyncCount > 0)
		{
			this.vSyncOnBTN.SetActive();
		}
		else
		{
			this.vSyncOffBTN.SetActive();
		}
		if (this.myOptionData.Mic)
		{
			this.micOnBTN.SetActive();
		}
		else
		{
			this.micOffBTN.SetActive();
		}
		if (this.myOptionData.Nudity)
		{
			this.nudityOnBTN.SetActive();
		}
		else
		{
			this.nudityOffBTN.SetActive();
		}
		TitleManager.Ins.OptionsPresented.Event += this.presentMe;
		this.backBTN.MyAction.Event += this.applySettings;
		this.backBTN.MyAction.Event += this.dismissMe;
	}

	private void OnDestroy()
	{
		TitleManager.Ins.OptionsPresented.Event -= this.presentMe;
		this.backBTN.MyAction.Event -= this.applySettings;
		this.backBTN.MyAction.Event -= this.dismissMe;
	}

	public static TitleOptionsMenuHook Ins;

	[SerializeField]
	private Slider qualitySlider;

	[SerializeField]
	private Slider resoultionSlider;

	[SerializeField]
	private OptionsMenuBTN vSyncOnBTN;

	[SerializeField]
	private OptionsMenuBTN vSyncOffBTN;

	[SerializeField]
	private OptionsMenuBTN windowModeOnBTN;

	[SerializeField]
	private OptionsMenuBTN windowModeOffBTN;

	[SerializeField]
	private OptionsMenuBTN micOnBTN;

	[SerializeField]
	private OptionsMenuBTN micOffBTN;

	[SerializeField]
	private OptionsMenuBTN nudityOnBTN;

	[SerializeField]
	private OptionsMenuBTN nudityOffBTN;

	[SerializeField]
	private TextMeshProUGUI qualityValue;

	[SerializeField]
	private TextMeshProUGUI resoultionValue;

	[SerializeField]
	private TitleMenuBTN applyBTN;

	[SerializeField]
	private TitleMenuBTN backBTN;

	private CanvasGroup myCG;

	private Dictionary<int, Resolution> screenSizes = new Dictionary<int, Resolution>(10);

	private List<int> screenSizeLookUp = new List<int>(10);

	private string[] quailtySettingNames = new string[0];

	private Options myOptionData;

	public static CustomEvent SettingsApplied = new CustomEvent(1);
}
