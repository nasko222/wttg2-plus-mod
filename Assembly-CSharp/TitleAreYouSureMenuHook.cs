using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class TitleAreYouSureMenuHook : MonoBehaviour
{
	public void Present()
	{
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.myCG.interactable = true;
			this.myCG.blocksRaycasts = true;
		});
	}

	public void Dismiss()
	{
		this.myCG.interactable = false;
		this.myCG.blocksRaycasts = false;
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.DismissActions.ExecuteAndKill();
		});
	}

	private void yesAction()
	{
		DataManager.ClearGameData();
		this.DismissActions.Event += TitleDiffSelectMenuHook.Ins.Present;
		this.Dismiss();
	}

	private void noAction()
	{
		this.DismissActions.Event += TitleMainMenuHook.Ins.Present;
		this.Dismiss();
	}

	private void Awake()
	{
		TitleAreYouSureMenuHook.Ins = this;
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myCG.alpha = 0f;
		this.myCG.interactable = false;
		this.myCG.ignoreParentGroups = false;
		this.myCG.blocksRaycasts = false;
		this.yesBTN.MyAction.Event += this.yesAction;
		this.noBTN.MyAction.Event += this.noAction;
	}

	private void OnDestroy()
	{
		this.yesBTN.MyAction.Event -= this.yesAction;
		this.noBTN.MyAction.Event -= this.noAction;
	}

	public static TitleAreYouSureMenuHook Ins;

	[SerializeField]
	private TitleMenuBTN yesBTN;

	[SerializeField]
	private TitleMenuBTN noBTN;

	public CustomEvent DismissActions = new CustomEvent(3);

	private CanvasGroup myCG;
}
