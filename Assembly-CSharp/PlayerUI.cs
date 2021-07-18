using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	private void Awake()
	{
		LookUp.PlayerUI = this;
	}

	public GameObject WifiDongleIcon;

	public GameObject MotionSensorIcon;

	public GameObject RemoteVPNIcon;

	public GameObject DollMakerMarkerIcon;

	public RectTransform LeftMouseClickTransform;

	public RectTransform RightMouseClickTransform;

	public RectTransform OpenDoorTransform;

	public RectTransform LockTransform;

	public RectTransform UnLockTransform;

	public RectTransform LightOnTransform;

	public RectTransform LightOffTransform;

	public RectTransform PeepEyeTransform;

	public RectTransform LeapTransform;

	public RectTransform HandTransform;

	public RectTransform HideTransform;

	public RectTransform SitTransform;

	public RectTransform ComputerTransform;

	public RectTransform PowerTransform;

	public RectTransform ComputerOnTransform;

	public RectTransform KnobTransform;

	public RectTransform DollMakerMarkerTransform;

	public RectTransform EnterBraceTransform;

	public RectTransform HoldTransform;

	public RectTransform EBarTransform;

	public CanvasGroup BlackScreenCG;

	public RectTransform MicGroupTransform;

	public CanvasGroup MicGreenCG;

	public CanvasGroup MicRedCG;

	public CanvasGroup GameOverGC;

	public TextMeshProUGUI GameOverReasonText;

	public CanvasGroup GameOverReasonCG;

	public CanvasGroup FlashScreenCG;

	public Image EBarBorder;

	public Image EBarFill;

	public Sprite RemoteVPNZeroBar;

	public Sprite RemoteVPNOneBar;

	public Sprite RemoteVPNTwoBar;
}
