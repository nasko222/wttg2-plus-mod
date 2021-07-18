using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaxWindowBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void HardMax()
	{
		this.isMaxed = true;
		base.GetComponent<Image>().sprite = this.defaultSpriteUnMax;
	}

	private void Awake()
	{
		this.defaultSprite = base.GetComponent<Image>().sprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.isMaxed)
		{
			base.GetComponent<Image>().sprite = this.hoverSpriteUnMax;
		}
		else
		{
			base.GetComponent<Image>().sprite = this.hoverSpriteDefault;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.isMaxed)
		{
			base.GetComponent<Image>().sprite = this.defaultSpriteUnMax;
		}
		else
		{
			base.GetComponent<Image>().sprite = this.defaultSprite;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.isMaxed)
		{
			this.isMaxed = false;
			if (this.MyProduct == SOFTWARE_PRODUCTS.UNIVERSAL)
			{
				GameManager.ManagerSlinger.AppManager.UnMaxApp(this.MyProductData);
			}
			else
			{
				GameManager.ManagerSlinger.AppManager.UnMaxApp(this.MyProduct);
			}
		}
		else
		{
			this.isMaxed = true;
			if (this.MyProduct == SOFTWARE_PRODUCTS.UNIVERSAL)
			{
				GameManager.ManagerSlinger.AppManager.MaxApp(this.MyProductData);
			}
			else
			{
				GameManager.ManagerSlinger.AppManager.MaxApp(this.MyProduct);
			}
		}
	}

	public SOFTWARE_PRODUCTS MyProduct;

	public SoftwareProductDefinition MyProductData;

	public Sprite defaultSpriteUnMax;

	public Sprite hoverSpriteDefault;

	public Sprite hoverSpriteUnMax;

	private Sprite defaultSprite;

	private bool isMaxed;
}
