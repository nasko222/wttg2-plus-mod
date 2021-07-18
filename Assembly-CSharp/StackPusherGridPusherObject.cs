using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class StackPusherGridPusherObject : MonoBehaviour
{
	public MatrixStackCord CurrentCord
	{
		get
		{
			return this.myCurrentCord;
		}
		set
		{
			this.myCurrentCord = value;
			this.setNewEffectedCords();
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event StackPusherGridPusherObject.MovedActions ClearOldPushBlock;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event StackPusherGridPusherObject.SetStackActions ResetStacks;

	public void Clear()
	{
		this.effectedCords.Clear();
		this.oldCords.Clear();
		this.resetCords.Clear();
		this.clearTween.Restart(true, -1f);
	}

	public void SetMyParent(RectTransform ParentRectTrans)
	{
		this.myRT.SetParent(ParentRectTrans);
		this.myRT.anchoredPosition = this.centerPOS;
	}

	public void PresentMe()
	{
		this.myCG.alpha = 1f;
		this.presentTween.Play<Tweener>();
	}

	public void PointerEnter()
	{
		this.hoverEnterTween.Restart(true, -1f);
	}

	public void PointerExit()
	{
		this.hoverExitTween.Restart(true, -1f);
	}

	public void SetActive()
	{
		this.setActiveTween.Restart(true, -1f);
	}

	public void SetInActive()
	{
		this.setInActiveTween.Restart(true, -1f);
	}

	public bool AmNextTo(MatrixStackCord CheckCord)
	{
		return this.effectedCords.ContainsKey(CheckCord);
	}

	public void Move(RectTransform NewParent, MatrixStackCord NewCord)
	{
		foreach (KeyValuePair<MatrixStackCord, short> keyValuePair in this.effectedCords)
		{
			this.oldCords.Add(keyValuePair.Key, keyValuePair.Value);
		}
		if (this.ClearOldPushBlock != null)
		{
			this.ClearOldPushBlock(this.myCurrentCord);
		}
		this.CurrentCord = NewCord;
		foreach (KeyValuePair<MatrixStackCord, short> keyValuePair2 in this.oldCords)
		{
			if (!this.effectedCords.ContainsKey(keyValuePair2.Key))
			{
				this.resetCords.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
		}
		if (this.ResetStacks != null)
		{
			this.ResetStacks(this.resetCords);
		}
		if (this.SetNewPushBlock != null)
		{
			this.SetNewPushBlock(NewCord);
		}
		this.SetInActive();
		this.gridScaleDownSeq.Restart(true, -1f);
		this.gridScaleUpSeq.Restart(true, -1f);
		GameManager.TimeSlinger.FireTimer(0.15f, delegate()
		{
			this.oldCords.Clear();
			this.resetCords.Clear();
			this.SetMyParent(NewParent);
			if (this.SetNewStacks != null)
			{
				this.SetNewStacks(this.effectedCords);
			}
		}, 0);
	}

	private void setNewEffectedCords()
	{
		this.effectedCords.Clear();
		int num = this.myCurrentCord.X - 1;
		int num2 = this.myCurrentCord.Y - 1;
		for (int i = 0; i < 9; i++)
		{
			this.effectedCords.Add(new MatrixStackCord(num, num2), 3);
			bool flag = num > this.myCurrentCord.X;
			num = ((!flag) ? (num + 1) : (this.myCurrentCord.X - 1));
			num2 = ((!flag) ? num2 : (num2 + 1));
		}
	}

	private void Awake()
	{
		this.myRT = base.GetComponent<RectTransform>();
		this.myCG = base.GetComponent<CanvasGroup>();
		this.inactiveCG = this.InactiveIMG.GetComponent<CanvasGroup>();
		this.hoverCG = this.HoverIMG.GetComponent<CanvasGroup>();
		this.activeCG = this.ActiveIMG.GetComponent<CanvasGroup>();
		this.defaultParent = (RectTransform)this.myRT.parent;
		this.presentTween = DOTween.To(() => this.inactiveCG.alpha, delegate(float x)
		{
			this.inactiveCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear);
		this.presentTween.Pause<Tweener>();
		this.presentTween.SetAutoKill(false);
		this.hoverEnterTween = DOTween.To(() => this.hoverCG.alpha, delegate(float x)
		{
			this.hoverCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear);
		this.hoverEnterTween.Pause<Tweener>();
		this.hoverEnterTween.SetAutoKill(false);
		this.hoverExitTween = DOTween.To(() => this.hoverCG.alpha, delegate(float x)
		{
			this.hoverCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear);
		this.hoverExitTween.Pause<Tweener>();
		this.hoverExitTween.SetAutoKill(false);
		this.setActiveTween = DOTween.To(() => this.activeCG.alpha, delegate(float x)
		{
			this.activeCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.hoverCG.alpha = 0f;
		});
		this.setActiveTween.Pause<Tweener>();
		this.setActiveTween.SetAutoKill(false);
		this.setInActiveTween = DOTween.To(() => this.activeCG.alpha, delegate(float x)
		{
			this.activeCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear);
		this.setInActiveTween.Pause<Tweener>();
		this.setInActiveTween.SetAutoKill(false);
		this.gridScaleDownSeq = DOTween.Sequence();
		this.gridScaleDownSeq.Insert(0f, DOTween.To(() => this.myRT.localScale, delegate(Vector3 x)
		{
			this.myRT.localScale = x;
		}, this.minScale, 0.15f).SetEase(Ease.Linear));
		this.gridScaleDownSeq.Insert(0f, DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear));
		this.gridScaleDownSeq.Pause<Sequence>();
		this.gridScaleDownSeq.SetAutoKill(false);
		this.gridScaleUpSeq = DOTween.Sequence();
		this.gridScaleUpSeq.Insert(0.15f, DOTween.To(() => this.myRT.localScale, delegate(Vector3 x)
		{
			this.myRT.localScale = x;
		}, this.maxScale, 0.15f).SetEase(Ease.Linear));
		this.gridScaleUpSeq.Insert(0.15f, DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear));
		this.gridScaleUpSeq.Pause<Sequence>();
		this.gridScaleUpSeq.SetAutoKill(false);
		this.clearTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.activeCG.alpha = 0f;
			this.SetMyParent(this.defaultParent);
		});
		this.clearTween.Pause<Tweener>();
		this.clearTween.SetAutoKill(false);
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event StackPusherGridPusherObject.MovedActions SetNewPushBlock;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event StackPusherGridPusherObject.SetStackActions SetNewStacks;

	public GameObject InactiveIMG;

	public GameObject HoverIMG;

	public GameObject ActiveIMG;

	private RectTransform myRT;

	private RectTransform defaultParent;

	private CanvasGroup myCG;

	private CanvasGroup inactiveCG;

	private CanvasGroup hoverCG;

	private CanvasGroup activeCG;

	private Vector3 minScale = new Vector3(0.1f, 0.1f, 1f);

	private Vector3 maxScale = Vector3.one;

	private Vector2 centerPOS = Vector2.zero;

	private MatrixStackCord myCurrentCord;

	private Dictionary<MatrixStackCord, short> effectedCords = new Dictionary<MatrixStackCord, short>(MatrixStackCordCompare.Ins);

	private Dictionary<MatrixStackCord, short> oldCords = new Dictionary<MatrixStackCord, short>(MatrixStackCordCompare.Ins);

	private Dictionary<MatrixStackCord, short> resetCords = new Dictionary<MatrixStackCord, short>(MatrixStackCordCompare.Ins);

	private Tweener hoverEnterTween;

	private Tweener hoverExitTween;

	private Tweener presentTween;

	private Tweener setActiveTween;

	private Tweener setInActiveTween;

	private Tweener clearTween;

	private Sequence gridScaleDownSeq;

	private Sequence gridScaleUpSeq;

	public delegate void MovedActions(MatrixStackCord Setcord);

	public delegate void SetStackActions(Dictionary<MatrixStackCord, short> TheCords);
}
