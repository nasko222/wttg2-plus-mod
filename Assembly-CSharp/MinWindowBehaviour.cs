using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinWindowBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	private void Awake()
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
		if (this.MyProduct == SOFTWARE_PRODUCTS.UNIVERSAL)
		{
			GameManager.ManagerSlinger.AppManager.MinApp(this.UniProductData);
		}
		else
		{
			GameManager.ManagerSlinger.AppManager.MinApp(this.MyProduct);
		}
	}

	public Sprite hoverSprite;

	public SOFTWARE_PRODUCTS MyProduct;

	public SoftwareProductDefinition UniProductData;

	private Sprite defaultSprite;
}
