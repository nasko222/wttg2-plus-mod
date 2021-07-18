using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MemCoreObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MemCoreObject.CoreActions KeyWasPresented;

	public void BuildMe(List<string> setKey)
	{
		this.isLocked = true;
		this.curKey = setKey;
		this.PresentMe();
		this.refreshRotateTween.Restart(true, -1f);
	}

	public void PresentKey()
	{
		GameManager.AudioSlinger.PlaySound(this.KeyFlashSFX);
		this.KeyText.GetComponent<Text>().text = this.curKey[0];
		for (int i = 1; i < this.curKey.Count; i++)
		{
			base.StartCoroutine(this.FlashKeyPart(this.curKey[i], (float)i * 1f));
		}
		GameManager.TimeSlinger.FireTimer(1f * (float)this.curKey.Count, delegate()
		{
			this.KeyText.GetComponent<Text>().text = string.Empty;
			if (this.KeyWasPresented != null)
			{
				this.KeyWasPresented();
				this.isLocked = false;
			}
		}, 0);
	}

	public void DismissMe()
	{
		GameManager.AudioSlinger.PlaySound(this.MemCoreHideSFX);
		GameManager.TimeSlinger.KillTimer(this.flashKeysTimer);
		this.flashKeysTimer = null;
		this.fadeMeOutTween.Restart(true, -1f);
		this.scaleMeDownTween.Restart(true, -1f);
	}

	public void UpdateKey(List<string> newKey)
	{
		this.curKey = newKey;
	}

	public void DismissRefreshIcon()
	{
		this.dismissRefreshIconTween.Restart(true, -1f);
		this.isLocked = true;
	}

	public void SetLock(bool setValue)
	{
		this.isLocked = setValue;
	}

	private void FlashKeys()
	{
		if (this.KeysWereFlashed != null)
		{
			this.KeysWereFlashed();
		}
		GameManager.AudioSlinger.PlaySound(this.KeyFlashSFX);
		this.RefreshIMG.GetComponent<CanvasGroup>().alpha = 0f;
		this.KeyText.GetComponent<Text>().text = this.curKey[0];
		for (int i = 1; i < this.curKey.Count; i++)
		{
			base.StartCoroutine(this.FlashKeyPart(this.curKey[i], (float)i * 0.5f));
		}
		GameManager.TimeSlinger.FireHardTimer(out this.flashKeysTimer, 0.5f * (float)this.curKey.Count, delegate()
		{
			this.KeyText.GetComponent<Text>().text = string.Empty;
			this.isLocked = false;
		}, 0);
	}

	private void PresentMe()
	{
		GameManager.AudioSlinger.PlaySound(this.MemCoreShowSFX);
		this.fadeMeInTween.Restart(true, -1f);
		this.scaleMeUpTween.Restart(true, -1f);
	}

	private void IWasPresented()
	{
		if (this.CoreWasShown != null)
		{
			this.CoreWasShown();
		}
	}

	private IEnumerator FlashKeyPart(string keyPart, float setDelay)
	{
		yield return new WaitForSeconds(setDelay);
		if (this.KeyText != null && this.KeyText.GetComponent<Text>() != null)
		{
			GameManager.AudioSlinger.PlaySound(this.KeyFlashSFX);
			this.KeyText.GetComponent<Text>().text = keyPart;
		}
		yield break;
	}

	private void Awake()
	{
		this.myCG = base.GetComponent<CanvasGroup>();
		this.refreshIMGCG = this.RefreshIMG.GetComponent<CanvasGroup>();
		this.myRT = base.GetComponent<RectTransform>();
		this.refreshIMGRT = this.RefreshIMG.GetComponent<RectTransform>();
		this.refreshRotateTween = this.refreshIMGRT.DORotate(new Vector3(0f, 0f, -360f), 2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
		this.refreshRotateTween.Pause<Tweener>();
		this.refreshRotateTween.SetAutoKill(false);
		this.fadeMeInTween = DOTween.To(() => base.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			base.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.3f).SetEase(Ease.Linear);
		this.fadeMeInTween.Pause<Tweener>();
		this.fadeMeInTween.SetAutoKill(false);
		this.scaleMeUpTween = DOTween.To(() => base.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			base.GetComponent<RectTransform>().localScale = x;
		}, Vector3.one, 0.3f).SetEase(Ease.OutCirc).OnComplete(new TweenCallback(this.IWasPresented));
		this.scaleMeUpTween.Pause<Tweener>();
		this.scaleMeUpTween.SetAutoKill(false);
		this.fadeMeOutTween = DOTween.To(() => base.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			base.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.3f).SetEase(Ease.Linear);
		this.fadeMeOutTween.Pause<Tweener>();
		this.fadeMeOutTween.SetAutoKill(false);
		this.scaleMeDownTween = DOTween.To(() => base.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			base.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(0.1f, 0.1f, 0.1f), 0.3f).SetEase(Ease.OutCirc);
		this.scaleMeDownTween.Pause<Tweener>();
		this.scaleMeDownTween.SetAutoKill(false);
		this.dismissRefreshIconTween = DOTween.To(() => this.RefreshIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.RefreshIMG.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear);
		this.dismissRefreshIconTween.Pause<Tweener>();
		this.dismissRefreshIconTween.SetAutoKill(false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.isLocked)
		{
			this.refreshFadeTween.Kill(false);
			this.refreshFadeTween = DOTween.To(() => this.refreshIMGCG.alpha, delegate(float x)
			{
				this.refreshIMGCG.alpha = x;
			}, 1f, 0.2f).SetEase(Ease.Linear);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.isLocked)
		{
			this.refreshFadeTween.Kill(false);
			this.refreshFadeTween = DOTween.To(() => this.refreshIMGCG.alpha, delegate(float x)
			{
				this.refreshIMGCG.alpha = x;
			}, 0f, 0.2f).SetEase(Ease.Linear);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.isLocked)
		{
			this.isLocked = true;
			this.refreshFadeTween.Kill(false);
			this.refreshIMGCG.alpha = 0f;
			this.FlashKeys();
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MemCoreObject.CoreActions CoreWasShown;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MemCoreObject.CoreActions KeysWereFlashed;

	public GameObject KeyText;

	public GameObject RefreshIMG;

	public AudioFileDefinition MemCoreShowSFX;

	public AudioFileDefinition MemCoreHideSFX;

	public AudioFileDefinition KeyFlashSFX;

	private const float CORE_FADE_IN_TIME = 0.3f;

	private const float CORE_SCALE_TIME = 0.3f;

	private const float REFRESH_FADE_TIME = 0.2f;

	private const float KEY_DELAY = 1f;

	private List<string> curKey = new List<string>();

	private bool isLocked = true;

	private CanvasGroup myCG;

	private CanvasGroup refreshIMGCG;

	private RectTransform myRT;

	private RectTransform refreshIMGRT;

	private Tweener refreshRotateTween;

	private Tweener refreshFadeTween;

	private Tweener fadeMeInTween;

	private Tweener scaleMeUpTween;

	private Tweener fadeMeOutTween;

	private Tweener scaleMeDownTween;

	private Tweener dismissRefreshIconTween;

	private Timer flashKeysTimer;

	public delegate void CoreActions();
}
