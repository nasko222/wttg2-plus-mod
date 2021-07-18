using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class EvilSkullBehavior : MonoBehaviour
{
	public void PresentMe()
	{
		GameManager.AudioSlinger.PlaySound(this.SkullInSFX);
		BlueWhisperManager.Ins.ProcessSound(this.SkullInSFX);
		this.skullObjectCG.alpha = 1f;
		this.presentSkullTween.Restart(true, -1f);
	}

	public void HackedLaugh()
	{
		this.skullObjectCG.alpha = 1f;
		this.skullLaughSeq.Restart(true, -1f);
		this.hackedLaughSeq.Restart(true, -1f);
	}

	private void haHaHa()
	{
		GameManager.AudioSlinger.PlaySound(this.EvilLaugh);
		BlueWhisperManager.Ins.ProcessSound(this.EvilLaugh);
		this.haHaHaSeq.Restart(true, -1f);
	}

	private void dismissMe()
	{
		GameManager.AudioSlinger.PlaySound(this.SkullOutSFX);
		BlueWhisperManager.Ins.ProcessSound(this.SkullOutSFX);
		this.dismissTween.Restart(true, -1f);
	}

	private void killMe()
	{
		this.skullObjectCG.alpha = 0f;
		GameManager.HackerManager.PresentHackGame();
	}

	private void Awake()
	{
		this.skullObjectCG = this.SkullObject.GetComponent<CanvasGroup>();
		this.skullBotRT = this.SkullBot.GetComponent<RectTransform>();
		this.skullObjectRT = this.SkullObject.GetComponent<RectTransform>();
		this.defaultSkullBotPOS = this.SkullBot.GetComponent<RectTransform>().anchoredPosition;
		this.presentSkullTween = DOTween.To(() => this.skullObjectRT.localScale, delegate(Vector3 x)
		{
			this.skullObjectRT.localScale = x;
		}, this.presentSkullScale, 0.75f).SetEase(Ease.OutBack).OnComplete(new TweenCallback(this.haHaHa));
		this.presentSkullTween.Pause<Tweener>();
		this.presentSkullTween.SetAutoKill(false);
		this.dismissTween = DOTween.To(() => this.skullObjectRT.localScale, delegate(Vector3 x)
		{
			this.skullObjectRT.localScale = x;
		}, this.defaultSkullScale, 0.4f).SetEase(Ease.InBack).OnComplete(new TweenCallback(this.killMe));
		this.dismissTween.Pause<Tweener>();
		this.dismissTween.SetAutoKill(false);
		this.haHaHaSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.dismissMe));
		this.haHaHaSeq.Insert(0f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotOpen, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(0.32f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotClose, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(0.46f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotOpen, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(0.66f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotClose, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(0.81f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotOpen, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(1.02f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotClose, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(1.17f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotOpen, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(1.32f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotClose, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(1.44f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotOpen, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(1.67f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotClose, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(1.81f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotOpen, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(2.02f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotClose, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(2.12f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotOpen, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(2.33f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotClose, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(2.47f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotOpen, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(2.67f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotClose, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(2.82f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotOpen, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(3.06f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotClose, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(3.18f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotOpen, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Insert(3.72f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotClose, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.haHaHaSeq.Pause<Sequence>();
		this.haHaHaSeq.SetAutoKill(false);
		this.skullLaughSeq = DOTween.Sequence();
		this.skullLaughSeq.Insert(0f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotOpen2, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.skullLaughSeq.Insert(0.1f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.laughSkullBotClose2, 0.1f).SetEase(Ease.Linear).SetRelative(true));
		this.skullLaughSeq.SetLoops(-1);
		this.skullLaughSeq.Pause<Sequence>();
		this.skullLaughSeq.SetAutoKill(false);
		this.hackedLaughSeq = DOTween.Sequence().OnComplete(delegate
		{
			this.skullLaughSeq.Pause<Sequence>();
			this.skullBotRT.anchoredPosition = this.defaultSkullBotPOS;
			this.skullObjectRT.localScale = this.defaultSkullScale;
		});
		this.hackedLaughSeq.Insert(0f, DOTween.To(() => this.skullObjectRT.localScale, delegate(Vector3 x)
		{
			this.skullObjectRT.localScale = x;
		}, this.hackedSkullSmallScale, 0.75f).SetEase(Ease.OutCirc));
		this.hackedLaughSeq.Insert(0.56f, DOTween.To(() => this.SkullObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.SkullObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear));
		this.hackedLaughSeq.Pause<Sequence>();
		this.hackedLaughSeq.SetAutoKill(false);
	}

	public GameObject SkullObject;

	public GameObject SkullTop;

	public GameObject SkullBot;

	public AudioFileDefinition EvilLaugh;

	public AudioFileDefinition SkullInSFX;

	public AudioFileDefinition SkullOutSFX;

	private Vector2 defaultSkullBotPOS;

	private Vector3 presentSkullScale = new Vector3(0.22f, 0.22f, 1f);

	private Vector3 hackedSkullSmallScale = new Vector3(0.01f, 0.01f, 1f);

	private Vector3 defaultSkullScale = Vector3.one;

	private Vector2 laughSkullBotOpen = new Vector2(0f, -145f);

	private Vector2 laughSkullBotClose = new Vector2(0f, 145f);

	private Vector2 laughSkullBotOpen2 = new Vector2(0f, -250f);

	private Vector2 laughSkullBotClose2 = new Vector2(0f, 250f);

	private CanvasGroup skullObjectCG;

	private RectTransform skullBotRT;

	private RectTransform skullObjectRT;

	private Tweener presentSkullTween;

	private Tweener dismissTween;

	private Sequence skullLaughSeq;

	private Sequence haHaHaSeq;

	private Sequence hackedLaughSeq;
}
