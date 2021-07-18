using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class UIInteractionManager : MonoBehaviour
{
	public void ShowLeftMouseButtonAction()
	{
		this.showLeftMouseClick.Restart(true, -1f);
	}

	public void HideLeftMouseButtonAction()
	{
		this.hideLeftMouseClick.Restart(true, -1f);
	}

	public void ShowRightMouseButtonAction()
	{
		this.showRightMouseClick.Restart(true, -1f);
	}

	public void HideRightMouseButtonAction()
	{
		this.hideRightMouseClick.Restart(true, -1f);
	}

	public void ShowOpenDoor()
	{
		this.showOpenDoor.Restart(true, -1f);
	}

	public void HideOpenDoor()
	{
		this.hideOpenDoor.Restart(true, -1f);
	}

	public void ShowLock()
	{
		this.showLock.Restart(true, -1f);
	}

	public void HideLock()
	{
		this.hideLock.Restart(true, -1f);
	}

	public void ShowUnLock()
	{
		this.showUnLock.Restart(true, -1f);
	}

	public void HideUnLock()
	{
		this.hideUnLock.Restart(true, -1f);
	}

	public void ShowLightOn()
	{
		this.showLightOn.Restart(true, -1f);
	}

	public void HideLightOn()
	{
		this.hideLightOn.Restart(true, -1f);
	}

	public void ShowLightOff()
	{
		this.showLightOff.Restart(true, -1f);
	}

	public void HideLightOff()
	{
		this.hideLightOff.Restart(true, -1f);
	}

	public void ShowPeep()
	{
		this.showPeep.Restart(true, -1f);
	}

	public void HidePeep()
	{
		this.hidePeep.Restart(true, -1f);
	}

	public void ShowLeap()
	{
		this.showLeap.Restart(true, -1f);
	}

	public void HideLeap()
	{
		this.hideLeap.Restart(true, -1f);
	}

	public void ShowHand()
	{
		this.showHand.Restart(true, -1f);
	}

	public void HideHand()
	{
		this.hideHand.Restart(true, -1f);
	}

	public void ShowHide()
	{
		this.showHide.Restart(true, -1f);
	}

	public void HideHide()
	{
		this.hideHide.Restart(true, -1f);
	}

	public void ShowSit()
	{
		this.showSit.Restart(true, -1f);
	}

	public void HideSit()
	{
		this.hideSit.Restart(true, -1f);
	}

	public void ShowComputer()
	{
		this.showComputer.Restart(true, -1f);
	}

	public void HideComputer()
	{
		this.hideComputer.Restart(true, -1f);
	}

	public void ShowPower()
	{
		this.showPower.Restart(true, -1f);
	}

	public void HidePower()
	{
		this.hidePower.Restart(true, -1f);
	}

	public void ShowComputerOn()
	{
		this.showComputerOn.Restart(true, -1f);
	}

	public void HideComputerOn()
	{
		this.hideComputerOn.Restart(true, -1f);
	}

	public void ShowKnob()
	{
		this.showKnob.Restart(true, -1f);
	}

	public void HideKnob()
	{
		this.hideKnob.Restart(true, -1f);
	}

	public void ShowDollMakerMarker()
	{
		this.showDollMakerMarker.Restart(true, -1f);
	}

	public void HideDollMakerMarker()
	{
		this.hideDollMakerMarker.Restart(true, -1f);
	}

	public void ShowEnterBraceMode()
	{
		this.showEnterBraceMode.Restart(true, -1f);
	}

	public void HideEnterBraceMode()
	{
		this.hideEnterBraceMode.Restart(true, -1f);
	}

	public void ShowHoldMode()
	{
		this.showHoldMode.Restart(true, -1f);
	}

	public void HideHoldMode()
	{
		this.hideHoldMode.Restart(true, -1f);
	}

	public void ShowEBar()
	{
		this.showEBar.Restart(true, -1f);
	}

	public void HideEBar()
	{
		this.hideEBar.Restart(true, -1f);
	}

	private void Awake()
	{
		UIInteractionManager.Ins = this;
		this.showLeftMouseClick = DOTween.To(() => new Vector2(-40f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.LeftMouseClickTransform.anchoredPosition = x;
		}, new Vector2(-40f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showLeftMouseClick.SetAutoKill(false);
		this.showLeftMouseClick.Pause<Tweener>();
		this.hideLeftMouseClick = DOTween.To(() => new Vector2(-40f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.LeftMouseClickTransform.anchoredPosition = x;
		}, new Vector2(-40f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideLeftMouseClick.SetAutoKill(false);
		this.hideLeftMouseClick.Pause<Tweener>();
		this.showRightMouseClick = DOTween.To(() => new Vector2(40f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.RightMouseClickTransform.anchoredPosition = x;
		}, new Vector2(40f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showRightMouseClick.SetAutoKill(false);
		this.showRightMouseClick.Pause<Tweener>();
		this.hideRightMouseClick = DOTween.To(() => new Vector2(40f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.RightMouseClickTransform.anchoredPosition = x;
		}, new Vector2(40f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideRightMouseClick.SetAutoKill(false);
		this.hideRightMouseClick.Pause<Tweener>();
		this.showOpenDoor = DOTween.To(() => new Vector2(-80f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.OpenDoorTransform.anchoredPosition = x;
		}, new Vector2(-80f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showOpenDoor.SetAutoKill(false);
		this.showOpenDoor.Pause<Tweener>();
		this.hideOpenDoor = DOTween.To(() => new Vector2(-80f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.OpenDoorTransform.anchoredPosition = x;
		}, new Vector2(-80f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideOpenDoor.SetAutoKill(false);
		this.hideOpenDoor.Pause<Tweener>();
		this.showLock = DOTween.To(() => new Vector2(80f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.LockTransform.anchoredPosition = x;
		}, new Vector2(80f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showLock.SetAutoKill(false);
		this.showLock.Pause<Tweener>();
		this.hideLock = DOTween.To(() => new Vector2(80f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.LockTransform.anchoredPosition = x;
		}, new Vector2(80f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideLock.SetAutoKill(false);
		this.hideLock.Pause<Tweener>();
		this.showUnLock = DOTween.To(() => new Vector2(80f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.UnLockTransform.anchoredPosition = x;
		}, new Vector2(80f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showUnLock.SetAutoKill(false);
		this.showUnLock.Pause<Tweener>();
		this.hideUnLock = DOTween.To(() => new Vector2(80f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.UnLockTransform.anchoredPosition = x;
		}, new Vector2(80f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideUnLock.SetAutoKill(false);
		this.hideUnLock.Pause<Tweener>();
		this.showLightOn = DOTween.To(() => new Vector2(-80f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.LightOnTransform.anchoredPosition = x;
		}, new Vector2(-80f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showLightOn.SetAutoKill(false);
		this.showLightOn.Pause<Tweener>();
		this.hideLightOn = DOTween.To(() => new Vector2(-80f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.LightOnTransform.anchoredPosition = x;
		}, new Vector2(-80f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideLightOn.SetAutoKill(false);
		this.hideLightOn.Pause<Tweener>();
		this.showLightOff = DOTween.To(() => new Vector2(-80f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.LightOffTransform.anchoredPosition = x;
		}, new Vector2(-80f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showLightOff.SetAutoKill(false);
		this.showLightOff.Pause<Tweener>();
		this.hideLightOff = DOTween.To(() => new Vector2(-80f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.LightOffTransform.anchoredPosition = x;
		}, new Vector2(-80f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideLightOff.SetAutoKill(false);
		this.hideLightOff.Pause<Tweener>();
		this.showPeep = DOTween.To(() => new Vector2(-86f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.PeepEyeTransform.anchoredPosition = x;
		}, new Vector2(-86f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showPeep.SetAutoKill(false);
		this.showPeep.Pause<Tweener>();
		this.hidePeep = DOTween.To(() => new Vector2(-86f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.PeepEyeTransform.anchoredPosition = x;
		}, new Vector2(-86f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hidePeep.SetAutoKill(false);
		this.hidePeep.Pause<Tweener>();
		this.showLeap = DOTween.To(() => new Vector2(-86f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.LeapTransform.anchoredPosition = x;
		}, new Vector2(-86f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showLeap.SetAutoKill(false);
		this.showLeap.Pause<Tweener>();
		this.hideLeap = DOTween.To(() => new Vector2(-86f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.LeapTransform.anchoredPosition = x;
		}, new Vector2(-86f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideLeap.SetAutoKill(false);
		this.hideLeap.Pause<Tweener>();
		this.showHand = DOTween.To(() => new Vector2(-80f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.HandTransform.anchoredPosition = x;
		}, new Vector2(-80f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showHand.SetAutoKill(false);
		this.showHand.Pause<Tweener>();
		this.hideHand = DOTween.To(() => new Vector2(-80f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.HandTransform.anchoredPosition = x;
		}, new Vector2(-80f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideHand.SetAutoKill(false);
		this.hideHand.Pause<Tweener>();
		this.showHide = DOTween.To(() => new Vector2(-84f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.HideTransform.anchoredPosition = x;
		}, new Vector2(-84f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showHide.SetAutoKill(false);
		this.showHide.Pause<Tweener>();
		this.hideHide = DOTween.To(() => new Vector2(-84f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.HideTransform.anchoredPosition = x;
		}, new Vector2(-84f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideHide.SetAutoKill(false);
		this.hideHide.Pause<Tweener>();
		this.showSit = DOTween.To(() => new Vector2(-80f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.SitTransform.anchoredPosition = x;
		}, new Vector2(-80f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showSit.SetAutoKill(false);
		this.showSit.Pause<Tweener>();
		this.hideSit = DOTween.To(() => new Vector2(-80f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.SitTransform.anchoredPosition = x;
		}, new Vector2(-80f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideSit.SetAutoKill(false);
		this.hideSit.Pause<Tweener>();
		this.showComputer = DOTween.To(() => new Vector2(-92f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.ComputerTransform.anchoredPosition = x;
		}, new Vector2(-92f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showComputer.SetAutoKill(false);
		this.showComputer.Pause<Tweener>();
		this.hideComputer = DOTween.To(() => new Vector2(-92f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.ComputerTransform.anchoredPosition = x;
		}, new Vector2(-92f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideComputer.SetAutoKill(false);
		this.hideComputer.Pause<Tweener>();
		this.showPower = DOTween.To(() => new Vector2(-80f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.PowerTransform.anchoredPosition = x;
		}, new Vector2(-80f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showPower.SetAutoKill(false);
		this.showPower.Pause<Tweener>();
		this.hidePower = DOTween.To(() => new Vector2(-80f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.PowerTransform.anchoredPosition = x;
		}, new Vector2(-80f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hidePower.SetAutoKill(false);
		this.hidePower.Pause<Tweener>();
		this.showComputerOn = DOTween.To(() => new Vector2(-84f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.ComputerOnTransform.anchoredPosition = x;
		}, new Vector2(-84f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showComputerOn.SetAutoKill(false);
		this.showComputerOn.Pause<Tweener>();
		this.hideComputerOn = DOTween.To(() => new Vector2(-84f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.ComputerOnTransform.anchoredPosition = x;
		}, new Vector2(-84f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideComputerOn.SetAutoKill(false);
		this.hideComputerOn.Pause<Tweener>();
		this.showKnob = DOTween.To(() => new Vector2(-92f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.KnobTransform.anchoredPosition = x;
		}, new Vector2(-92f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showKnob.SetAutoKill(false);
		this.showKnob.Pause<Tweener>();
		this.hideKnob = DOTween.To(() => new Vector2(-92f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.KnobTransform.anchoredPosition = x;
		}, new Vector2(-92f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideKnob.SetAutoKill(false);
		this.hideKnob.Pause<Tweener>();
		this.showDollMakerMarker = DOTween.To(() => new Vector2(-80f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.DollMakerMarkerTransform.anchoredPosition = x;
		}, new Vector2(-80f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showDollMakerMarker.SetAutoKill(false);
		this.showDollMakerMarker.Pause<Tweener>();
		this.hideDollMakerMarker = DOTween.To(() => new Vector2(-80f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.DollMakerMarkerTransform.anchoredPosition = x;
		}, new Vector2(-80f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideDollMakerMarker.SetAutoKill(false);
		this.hideDollMakerMarker.Pause<Tweener>();
		this.showEnterBraceMode = DOTween.To(() => new Vector2(82f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.EnterBraceTransform.anchoredPosition = x;
		}, new Vector2(82f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showEnterBraceMode.SetAutoKill(false);
		this.showEnterBraceMode.Pause<Tweener>();
		this.hideEnterBraceMode = DOTween.To(() => new Vector2(82f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.EnterBraceTransform.anchoredPosition = x;
		}, new Vector2(82f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideEnterBraceMode.SetAutoKill(false);
		this.hideEnterBraceMode.Pause<Tweener>();
		this.showHoldMode = DOTween.To(() => new Vector2(-86f, 66f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.HoldTransform.anchoredPosition = x;
		}, new Vector2(-86f, -30f), 0.2f).SetEase(Ease.Linear);
		this.showHoldMode.SetAutoKill(false);
		this.showHoldMode.Pause<Tweener>();
		this.hideHoldMode = DOTween.To(() => new Vector2(-86f, -30f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.HoldTransform.anchoredPosition = x;
		}, new Vector2(-86f, 66f), 0.2f).SetEase(Ease.Linear);
		this.hideHoldMode.SetAutoKill(false);
		this.hideHoldMode.Pause<Tweener>();
		if (UIInteractionManager.<>f__mg$cache0 == null)
		{
			UIInteractionManager.<>f__mg$cache0 = new DOGetter<Vector2>(Vector2.get_zero);
		}
		this.showEBar = DOTween.To(UIInteractionManager.<>f__mg$cache0, delegate(Vector2 x)
		{
			LookUp.PlayerUI.EBarTransform.anchoredPosition = x;
		}, new Vector2(-55f, 0f), 0.2f).SetEase(Ease.Linear);
		this.showEBar.SetAutoKill(false);
		this.showEBar.Pause<Tweener>();
		this.hideEBar = DOTween.To(() => new Vector2(-55f, 0f), delegate(Vector2 x)
		{
			LookUp.PlayerUI.EBarTransform.anchoredPosition = x;
		}, Vector2.zero, 0.2f).SetEase(Ease.Linear);
		this.hideEBar.SetAutoKill(false);
		this.hideEBar.Pause<Tweener>();
	}

	public static UIInteractionManager Ins;

	private Tweener showLeftMouseClick;

	private Tweener hideLeftMouseClick;

	private Tweener showRightMouseClick;

	private Tweener hideRightMouseClick;

	private Tweener showOpenDoor;

	private Tweener hideOpenDoor;

	private Tweener showLock;

	private Tweener hideLock;

	private Tweener showUnLock;

	private Tweener hideUnLock;

	private Tweener showLightOn;

	private Tweener hideLightOn;

	private Tweener showLightOff;

	private Tweener hideLightOff;

	private Tweener showPeep;

	private Tweener hidePeep;

	private Tweener showLeap;

	private Tweener hideLeap;

	private Tweener showHand;

	private Tweener hideHand;

	private Tweener showHide;

	private Tweener hideHide;

	private Tweener showSit;

	private Tweener hideSit;

	private Tweener showComputer;

	private Tweener hideComputer;

	private Tweener showPower;

	private Tweener hidePower;

	private Tweener showComputerOn;

	private Tweener hideComputerOn;

	private Tweener showKnob;

	private Tweener hideKnob;

	private Tweener showDollMakerMarker;

	private Tweener hideDollMakerMarker;

	private Tweener showEnterBraceMode;

	private Tweener hideEnterBraceMode;

	private Tweener showHoldMode;

	private Tweener hideHoldMode;

	private Tweener showEBar;

	private Tweener hideEBar;

	[CompilerGenerated]
	private static DOGetter<Vector2> <>f__mg$cache0;
}
