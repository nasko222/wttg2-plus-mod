using System;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShadowMarketProductsBTNBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ShadowMarketProductsBTNBehaviour.BoolActions BuyItem;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ShadowMarketProductsBTNBehaviour.VoidActions ShipItem;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ShadowMarketProductsBTNBehaviour.VoidActions CantBuy;

	public void SetDefaults()
	{
		this.myImage = base.GetComponent<Image>();
		this.myRT = base.GetComponent<RectTransform>();
	}

	public void SetToBuy()
	{
		this.isDisabled = false;
		this.myImage.sprite = this.defaultSprite;
		this.btnText.text = "BUY";
		this.btnText.fontStyle = FontStyle.Normal;
	}

	public void SetToOwned()
	{
		this.isDisabled = true;
		this.myImage.sprite = this.disabledSprite;
		this.btnText.text = "OWNED";
		this.btnText.fontStyle = FontStyle.Italic;
	}

	public void SetToDisabled()
	{
		this.isDisabled = true;
		this.myImage.sprite = this.disabledSprite;
		this.btnText.fontStyle = FontStyle.Italic;
	}

	public void SetToShipped()
	{
		this.isDisabled = true;
		this.myImage.sprite = this.disabledSprite;
		this.btnText.text = "SHIPPED!";
		this.btnText.fontStyle = FontStyle.Bold;
		this.btnText.fontSize = 14;
	}

	public void ShipAni(float setTime)
	{
		this.isDisabled = true;
		this.myImage.sprite = this.disabledSprite;
		this.btnText.text = "Shipping...";
		this.btnText.fontStyle = FontStyle.Italic;
		this.btnText.fontSize = 14;
		Vector2 shipIMGSize = new Vector2(this.myRT.sizeDelta.x, 0f);
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			shipIMGSize.x = 0f;
			shipIMGSize.y = this.shippingIMGRT.sizeDelta.y;
			this.shippingIMGRT.sizeDelta = shipIMGSize;
		});
		sequence.Insert(0f, DOTween.To(() => this.shippingIMGRT.sizeDelta, delegate(Vector2 x)
		{
			this.shippingIMGRT.sizeDelta = x;
		}, shipIMGSize, setTime).SetEase(Ease.Linear).SetRelative(true));
		sequence.Play<Sequence>();
	}

	private void Awake()
	{
		this.myImage = base.GetComponent<Image>();
		this.myRT = base.GetComponent<RectTransform>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.isDisabled)
		{
			GameManager.ManagerSlinger.CursorManager.PointerCursorState(true);
			this.myImage.sprite = this.hoverSprite;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.isDisabled)
		{
			GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
			this.myImage.sprite = this.defaultSprite;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.isDisabled && this.BuyItem != null)
		{
			if (this.BuyItem())
			{
				if (this.ShipItem != null)
				{
					this.ShipItem();
				}
			}
			else
			{
				if (this.CantBuy != null)
				{
					this.CantBuy();
				}
				this.SetToDisabled();
				GameManager.TimeSlinger.FireTimer(3f, new Action(this.SetToBuy), 0);
			}
		}
		GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
	}

	[SerializeField]
	private Text btnText;

	[SerializeField]
	private RectTransform shippingIMGRT;

	[SerializeField]
	private Sprite defaultSprite;

	[SerializeField]
	private Sprite hoverSprite;

	[SerializeField]
	private Sprite disabledSprite;

	private bool isDisabled;

	private Image myImage;

	private RectTransform myRT;

	public delegate bool BoolActions();

	public delegate void VoidActions();
}
