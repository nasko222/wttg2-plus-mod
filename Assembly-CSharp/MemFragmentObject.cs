using System;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MemFragmentObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MemFragmentObject.FragmentActions IWasChosen;

	public void BuildMe(int setClockPOS, int setLayerIndex, string setKeyPart)
	{
		this.MyClockPOS = setClockPOS;
		this.MyLayerIndex = setLayerIndex;
		this.CurKeyPart = setKeyPart;
		this.KeyPart1.GetComponent<Text>().text = setKeyPart;
		this.KeyPart2.GetComponent<Text>().text = setKeyPart;
		this.isLocked = true;
	}

	public void ExpandMe()
	{
		Vector2 zero = Vector2.zero;
		float num = Mathf.Ceil(50f + base.GetComponent<RectTransform>().sizeDelta.x / 2f) + 20f;
		num += (base.GetComponent<RectTransform>().sizeDelta.x + 20f) * (float)this.MyLayerIndex;
		base.GetComponent<CanvasGroup>().alpha = 1f;
		switch (this.MyClockPOS)
		{
		case 1:
			zero.y = num;
			this.LineIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 21f);
			this.LineIMG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -Mathf.Ceil(base.GetComponent<RectTransform>().sizeDelta.x / 2f + 10f));
			break;
		case 2:
			zero.x = num;
			this.LineIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(21f, 1f);
			this.LineIMG.GetComponent<RectTransform>().anchoredPosition = new Vector2(-Mathf.Ceil(base.GetComponent<RectTransform>().sizeDelta.x / 2f + 10f), 0f);
			break;
		case 3:
			zero.y = -num;
			this.LineIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 21f);
			this.LineIMG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, Mathf.Ceil(base.GetComponent<RectTransform>().sizeDelta.x / 2f + 10f));
			break;
		case 4:
			zero.x = -num;
			this.LineIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(21f, 1f);
			this.LineIMG.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.Ceil(base.GetComponent<RectTransform>().sizeDelta.x / 2f + 10f), 0f);
			break;
		}
		DOTween.To(() => base.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			base.GetComponent<RectTransform>().anchoredPosition = x;
		}, zero, 0.5f).SetEase(Ease.OutCirc);
		this.isLocked = false;
	}

	public void CollapseMe()
	{
		GameManager.TimeSlinger.KillTimer(this.collapseTimer);
		this.collapseTimer = null;
		this.isLocked = true;
		DOTween.To(() => base.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			base.GetComponent<RectTransform>().anchoredPosition = x;
		}, Vector2.zero, 0.5f).SetEase(Ease.InCirc);
		GameManager.TimeSlinger.FireHardTimer(out this.collapseTimer, 0.5f, delegate()
		{
			base.GetComponent<CanvasGroup>().alpha = 0f;
		}, 0);
	}

	public void GoRed()
	{
		DOTween.To(() => this.HoverIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.HoverIMG.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear);
		DOTween.To(() => this.FragIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.FragIMG.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear);
		DOTween.To(() => this.CorrectIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.CorrectIMG.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear);
		DOTween.To(() => this.WrongIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.WrongIMG.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.2f).SetEase(Ease.Linear);
	}

	public void ResetMe()
	{
		this.IWasSolved = false;
		this.FragIMG.GetComponent<CanvasGroup>().alpha = 1f;
		this.HoverIMG.GetComponent<CanvasGroup>().alpha = 0f;
		this.CorrectIMG.GetComponent<CanvasGroup>().alpha = 0f;
		this.WrongIMG.GetComponent<CanvasGroup>().alpha = 0f;
	}

	public void UpdateMyKey(string newKey)
	{
		this.CurKeyPart = newKey;
		this.KeyPart1.GetComponent<Text>().text = newKey;
		this.KeyPart2.GetComponent<Text>().text = newKey;
	}

	public void LockMe()
	{
		if (!this.IWasSolved)
		{
			this.isLocked = true;
		}
	}

	public void UnLockMe()
	{
		if (!this.IWasSolved)
		{
			this.isLocked = false;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.isLocked)
		{
			this.HoverStateTweener.Kill(false);
			this.HoverStateTweener = DOTween.To(() => this.HoverIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.HoverIMG.GetComponent<CanvasGroup>().alpha = x;
			}, 1f, 0.2f).SetEase(Ease.Linear);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.isLocked)
		{
			this.HoverStateTweener.Kill(false);
			this.HoverStateTweener = DOTween.To(() => this.HoverIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.HoverIMG.GetComponent<CanvasGroup>().alpha = x;
			}, 0f, 0.2f).SetEase(Ease.Linear);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.isLocked && this.IWasChosen != null)
		{
			this.isLocked = true;
			if (this.IWasChosen(this.CurKeyPart))
			{
				this.IWasSolved = true;
				DOTween.To(() => this.HoverIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.HoverIMG.GetComponent<CanvasGroup>().alpha = x;
				}, 0f, 0.2f).SetEase(Ease.Linear);
				DOTween.To(() => this.FragIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.FragIMG.GetComponent<CanvasGroup>().alpha = x;
				}, 0f, 0.2f).SetEase(Ease.Linear);
				DOTween.To(() => this.CorrectIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.CorrectIMG.GetComponent<CanvasGroup>().alpha = x;
				}, 1f, 0.2f).SetEase(Ease.Linear);
			}
		}
	}

	private void OnDestroy()
	{
		GameManager.TimeSlinger.KillTimer(this.collapseTimer);
		this.collapseTimer = null;
	}

	public GameObject FragIMG;

	public GameObject HoverIMG;

	public GameObject CorrectIMG;

	public GameObject WrongIMG;

	public GameObject LineIMG;

	public GameObject KeyPart1;

	public GameObject KeyPart2;

	private const float CORE_WIDTH_HEIGHT = 100f;

	private const float FRAG_SPACING = 20f;

	private const float SLIDE_TIME = 0.5f;

	private const float IMG_FADE_TIME = 0.2f;

	private int MyClockPOS;

	private int MyLayerIndex;

	private string CurKeyPart;

	private bool isLocked = true;

	private bool IWasSolved;

	private Tweener HoverStateTweener;

	private Timer collapseTimer;

	public delegate bool FragmentActions(string keyPart);
}
