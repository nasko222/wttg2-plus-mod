using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class TitleLogoHook : MonoBehaviour
{
	private void presentLogo()
	{
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 2.5f).SetEase(Ease.Linear).SetDelay(5.5f).OnComplete(delegate
		{
			TitleManager.Ins.LogoWasPresented();
		});
	}

	private void dismissLogo()
	{
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.75f).SetEase(Ease.Linear).SetDelay(0.75f);
	}

	private void presentLogoQuick()
	{
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear);
	}

	private void dismissLogoQuick()
	{
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear);
	}

	private void Awake()
	{
		TitleLogoHook.Ins = this;
		this.myCG = base.GetComponent<CanvasGroup>();
		TitleManager.Ins.TitlePresent.Event += this.presentLogo;
		TitleManager.Ins.TitleDismissing.Event += this.dismissLogo;
		TitleManager.Ins.OptionsDismissed.Event += this.presentLogoQuick;
		TitleManager.Ins.OptionsPresent.Event += this.dismissLogoQuick;
	}

	private void OnDestroy()
	{
		TitleManager.Ins.TitlePresent.Event -= this.presentLogo;
		TitleManager.Ins.TitleDismissing.Event -= this.dismissLogo;
		TitleManager.Ins.OptionsDismissed.Event -= this.presentLogoQuick;
		TitleManager.Ins.OptionsPresent.Event -= this.dismissLogoQuick;
	}

	public static TitleLogoHook Ins;

	private CanvasGroup myCG;
}
