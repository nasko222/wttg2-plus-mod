using System;
using UnityEngine;

public class UIInteractionHook : MonoBehaviour
{
	public void ShowLeftMouseButtonAction()
	{
		UIInteractionManager.Ins.ShowLeftMouseButtonAction();
	}

	public void HideLeftMouseButtonAction()
	{
		UIInteractionManager.Ins.HideLeftMouseButtonAction();
	}

	public void ShowRightMouseButtonAction()
	{
		UIInteractionManager.Ins.ShowRightMouseButtonAction();
	}

	public void HideRightMouseButtonAction()
	{
		UIInteractionManager.Ins.HideRightMouseButtonAction();
	}

	public void ShowOpenDoor()
	{
		UIInteractionManager.Ins.ShowOpenDoor();
	}

	public void HideOpenDoor()
	{
		UIInteractionManager.Ins.HideOpenDoor();
	}

	public void ShowLock()
	{
		UIInteractionManager.Ins.ShowLock();
	}

	public void HideLock()
	{
		UIInteractionManager.Ins.HideLock();
	}

	public void ShowUnLock()
	{
		UIInteractionManager.Ins.ShowUnLock();
	}

	public void HideUnLock()
	{
		UIInteractionManager.Ins.HideUnLock();
	}

	public void ShowLightOn()
	{
		UIInteractionManager.Ins.ShowLightOn();
	}

	public void HideLightOn()
	{
		UIInteractionManager.Ins.HideLightOn();
	}

	public void ShowLightOff()
	{
		UIInteractionManager.Ins.ShowLightOff();
	}

	public void HideLightOff()
	{
		UIInteractionManager.Ins.HideLightOff();
	}

	public void ShowPeep()
	{
		UIInteractionManager.Ins.ShowPeep();
	}

	public void HidePeep()
	{
		UIInteractionManager.Ins.HidePeep();
	}

	public void ShowLeap()
	{
		UIInteractionManager.Ins.ShowLeap();
	}

	public void HideLeap()
	{
		UIInteractionManager.Ins.HideLeap();
	}

	public void ShowHand()
	{
		UIInteractionManager.Ins.ShowHand();
	}

	public void HideHand()
	{
		UIInteractionManager.Ins.HideHand();
	}

	public void ShowHide()
	{
		UIInteractionManager.Ins.ShowHide();
	}

	public void HideHide()
	{
		UIInteractionManager.Ins.HideHide();
	}

	public void ShowSit()
	{
		UIInteractionManager.Ins.ShowSit();
	}

	public void HideSit()
	{
		UIInteractionManager.Ins.HideSit();
	}

	public void ShowComputer()
	{
		UIInteractionManager.Ins.ShowComputer();
	}

	public void HideComputer()
	{
		UIInteractionManager.Ins.HideComputer();
	}

	public void ShowPower()
	{
		UIInteractionManager.Ins.ShowPower();
	}

	public void HidePower()
	{
		UIInteractionManager.Ins.HidePower();
	}

	public void ShowComputerOn()
	{
		UIInteractionManager.Ins.ShowComputerOn();
	}

	public void HideComputerOn()
	{
		UIInteractionManager.Ins.HideComputerOn();
	}

	public void ShowKnob()
	{
		UIInteractionManager.Ins.ShowKnob();
	}

	public void HideKnob()
	{
		UIInteractionManager.Ins.HideKnob();
	}

	public void ShowDollMakerMarker()
	{
		UIInteractionManager.Ins.ShowDollMakerMarker();
	}

	public void HideDollMakerMarker()
	{
		UIInteractionManager.Ins.HideDollMakerMarker();
	}

	public void ShowEnterBraceMode()
	{
		UIInteractionManager.Ins.ShowEnterBraceMode();
	}

	public void HideEnterBraceMode()
	{
		UIInteractionManager.Ins.HideEnterBraceMode();
	}

	public void ShowHoldMode()
	{
		UIInteractionManager.Ins.ShowHoldMode();
	}

	public void HideHoldMode()
	{
		UIInteractionManager.Ins.HideHoldMode();
	}

	public void ShowEBar()
	{
		if (!ModsManager.UnlimitedStamina)
		{
			UIInteractionManager.Ins.ShowEBar();
		}
	}

	public void HideEBar()
	{
		if (!ModsManager.UnlimitedStamina)
		{
			UIInteractionManager.Ins.HideEBar();
		}
	}
}
