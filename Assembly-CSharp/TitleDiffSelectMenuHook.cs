using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class TitleDiffSelectMenuHook : MonoBehaviour
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

	private void leetAction()
	{
		DataManager.LeetMode = true;
		this.DismissActions.Event += TitleManager.Ins.DismissTitle;
		this.Dismiss();
	}

	private void normalAction()
	{
		DataManager.LeetMode = false;
		this.DismissActions.Event += TitleManager.Ins.DismissTitle;
		this.Dismiss();
	}

	private void backAction()
	{
		this.DismissActions.Event += TitleMainMenuHook.Ins.Present;
		this.Dismiss();
	}

	private void Awake()
	{
		TitleDiffSelectMenuHook.Ins = this;
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myCG.alpha = 0f;
		this.myCG.interactable = false;
		this.myCG.ignoreParentGroups = false;
		this.myCG.blocksRaycasts = false;
		this.leetBTN.MyAction.Event += this.leetAction;
		this.normalBTN.MyAction.Event += this.normalAction;
		this.backBTN.MyAction.Event += this.backAction;
	}

	private void OnDestroy()
	{
		this.leetBTN.MyAction.Event -= this.leetAction;
		this.normalBTN.MyAction.Event -= this.normalAction;
		this.backBTN.MyAction.Event -= this.backAction;
	}

	public static TitleDiffSelectMenuHook Ins;

	public CustomEvent DismissActions = new CustomEvent(3);

	[SerializeField]
	private TitleMenuBTN leetBTN;

	[SerializeField]
	private TitleMenuBTN normalBTN;

	[SerializeField]
	private TitleMenuBTN backBTN;

	private CanvasGroup myCG;
}
