using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseWindowBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void Start()
	{
		this.defaultSprite = base.GetComponent<Image>().sprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.hoverSprite != null)
		{
			base.GetComponent<Image>().sprite = this.hoverSprite;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		base.GetComponent<Image>().sprite = this.defaultSprite;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.IgnoreProduct)
		{
			if (this.MyProduct == SOFTWARE_PRODUCTS.UNIVERSAL)
			{
				GameManager.ManagerSlinger.AppManager.CloseApp(this.MyProductData);
			}
			else
			{
				GameManager.ManagerSlinger.AppManager.CloseApp(this.MyProduct);
			}
		}
		base.GetComponent<Image>().sprite = this.defaultSprite;
		this.parentWindow.gameObject.SetActive(false);
	}

	public Image parentWindow;

	public Sprite hoverSprite;

	public SOFTWARE_PRODUCTS MyProduct;

	public SoftwareProductDefinition MyProductData;

	public bool IgnoreProduct;

	private Sprite defaultSprite;
}
