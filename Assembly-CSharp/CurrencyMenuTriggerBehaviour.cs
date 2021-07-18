using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class CurrencyMenuTriggerBehaviour : MonoBehaviour
{
	public void TriggerCurrencyMenu()
	{
		if (!this.currencyMenuAniActive)
		{
			this.currencyMenuAniActive = true;
			if (this.currencyMenuActive)
			{
				this.currencyMenuActive = false;
				Vector2 endValue = new Vector2(LookUp.DesktopUI.CURRENCY_MENU.anchoredPosition.x, LookUp.DesktopUI.CURRENCY_MENU.sizeDelta.y);
				DOTween.To(() => LookUp.DesktopUI.CURRENCY_MENU.anchoredPosition, delegate(Vector2 x)
				{
					LookUp.DesktopUI.CURRENCY_MENU.anchoredPosition = x;
				}, endValue, 0.25f).SetEase(Ease.InQuad).OnComplete(delegate
				{
					this.currencyMenuAniActive = false;
				});
			}
			else
			{
				this.currencyMenuActive = true;
				Vector2 endValue2 = new Vector2(LookUp.DesktopUI.CURRENCY_MENU.anchoredPosition.x, -41f);
				DOTween.To(() => LookUp.DesktopUI.CURRENCY_MENU.anchoredPosition, delegate(Vector2 x)
				{
					LookUp.DesktopUI.CURRENCY_MENU.anchoredPosition = x;
				}, endValue2, 0.25f).SetEase(Ease.OutQuad).OnComplete(delegate
				{
					this.currencyMenuAniActive = false;
				});
			}
		}
	}

	private bool currencyMenuAniActive;

	private bool currencyMenuActive;
}
