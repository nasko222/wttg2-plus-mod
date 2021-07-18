using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogBTNBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DialogBTNBehaviour.MyActions OnPress;

	public void Start()
	{
		this.defaultSprite = base.GetComponent<Image>().sprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		base.GetComponent<Image>().sprite = this.hoverSprite;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		base.GetComponent<Image>().sprite = this.defaultSprite;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.OnPress != null)
		{
			base.GetComponent<Image>().sprite = this.defaultSprite;
			this.OnPress();
		}
	}

	public Sprite hoverSprite;

	private Sprite defaultSprite;

	public delegate void MyActions();
}
