using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinnedAppObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void SoftBuild()
	{
		base.GetComponent<RectTransform>().anchoredPosition = this.MyStartPOS;
	}

	public void BuildMe(SOFTWARE_PRODUCTS SetMyProduct, int MyCount)
	{
		this.MyProduct = SetMyProduct;
		this.title1Text.text = LookUp.SoftwareProducts.Get(SetMyProduct).MinProductTitle;
		this.title2Text.text = LookUp.SoftwareProducts.Get(SetMyProduct).MinProductTitle;
		this.appIconIMG.sprite = LookUp.SoftwareProducts.Get(SetMyProduct).MinProductSprite;
		float x2;
		if (MyCount != 0)
		{
			x2 = (float)MyCount * MinnedAppObject.MIN_APP_WIDTH + (float)MyCount * MinnedAppObject.MIN_APP_SPACING + MinnedAppObject.MIN_APP_SPACING;
		}
		else
		{
			x2 = MinnedAppObject.MIN_APP_SPACING;
		}
		this.MyStartPOS.x = x2;
		this.MyShowPOS.x = x2;
		base.GetComponent<RectTransform>().anchoredPosition = this.MyStartPOS;
		DOTween.To(() => base.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			base.GetComponent<RectTransform>().anchoredPosition = x;
		}, this.MyShowPOS, 0.2f).SetEase(Ease.Linear);
	}

	public void BuildMe(SoftwareProductDefinition SetMyProduct, int MyCount)
	{
		this.MyProductData = SetMyProduct;
		this.title1Text.text = this.MyProductData.MinProductTitle;
		this.title2Text.text = this.MyProductData.MinProductTitle;
		this.appIconIMG.sprite = this.MyProductData.MinProductSprite;
		float x2;
		if (MyCount != 0)
		{
			x2 = (float)MyCount * MinnedAppObject.MIN_APP_WIDTH + (float)MyCount * MinnedAppObject.MIN_APP_SPACING + MinnedAppObject.MIN_APP_SPACING;
		}
		else
		{
			x2 = MinnedAppObject.MIN_APP_SPACING;
		}
		this.MyStartPOS.x = x2;
		this.MyShowPOS.x = x2;
		base.GetComponent<RectTransform>().anchoredPosition = this.MyStartPOS;
		DOTween.To(() => base.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			base.GetComponent<RectTransform>().anchoredPosition = x;
		}, this.MyShowPOS, 0.2f).SetEase(Ease.Linear);
	}

	public void RePOSMe(int setIndex)
	{
		float x2;
		if (setIndex != 0)
		{
			x2 = (float)setIndex * MinnedAppObject.MIN_APP_WIDTH + (float)setIndex * MinnedAppObject.MIN_APP_SPACING + MinnedAppObject.MIN_APP_SPACING;
		}
		else
		{
			x2 = MinnedAppObject.MIN_APP_SPACING;
		}
		this.MyShowPOS.x = x2;
		this.MyStartPOS.x = x2;
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => base.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			base.GetComponent<RectTransform>().anchoredPosition = x;
		}, this.MyShowPOS, 0.1f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void ForceDismissMe()
	{
		this.dismissMe();
	}

	private void dismissMe()
	{
		this.hoverSEQ.Kill(false);
		Sequence sequence = DOTween.Sequence().OnComplete(new TweenCallback(this.unMinApp));
		sequence.Insert(0f, DOTween.To(() => this.tabHoverIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.tabHoverIMG.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => base.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			base.GetComponent<RectTransform>().anchoredPosition = x;
		}, this.MyStartPOS, 0.2f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	private void unMinApp()
	{
		if (this.MyProductData != null)
		{
			GameManager.ManagerSlinger.AppManager.UnMinApp(this.MyProductData);
		}
		else
		{
			GameManager.ManagerSlinger.AppManager.UnMinApp(this.MyProduct);
		}
	}

	public void Start()
	{
		this.hoverSEQ = DOTween.Sequence();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		this.hoverSEQ.Kill(false);
		this.hoverSEQ.Insert(0f, DOTween.To(() => this.tabHoverIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.tabHoverIMG.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.2f).SetEase(Ease.Linear));
		this.hoverSEQ.Play<Sequence>();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.hoverSEQ.Kill(false);
		this.hoverSEQ.Insert(0f, DOTween.To(() => this.tabHoverIMG.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.tabHoverIMG.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear));
		this.hoverSEQ.Play<Sequence>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.dismissMe();
	}

	public static float MIN_APP_SPACING = 5f;

	public static float MIN_APP_WIDTH = 137f;

	public Image appIconIMG;

	public Image tabHoverIMG;

	public Text title1Text;

	public Text title2Text;

	private SOFTWARE_PRODUCTS MyProduct;

	private SoftwareProductDefinition MyProductData;

	private Vector2 MyStartPOS = new Vector2(0f, -50f);

	private Vector2 MyShowPOS = new Vector2(0f, -4f);

	private Sequence hoverSEQ;

	private string myKey;
}
