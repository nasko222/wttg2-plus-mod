using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnnBookmarkBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void setBookmarked(bool nowBookmarked)
	{
		if (nowBookmarked)
		{
			base.GetComponent<Image>().sprite = this.bookmarkedSprite;
		}
		else
		{
			base.GetComponent<Image>().sprite = this.defaultSprite;
		}
	}

	private void Awake()
	{
		this.defaultSprite = base.GetComponent<Image>().sprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.CursorManager.PointerCursorState(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.setBookmarked(GameManager.TheCloud.TriggerBookMark());
	}

	public Sprite bookmarkedSprite;

	private Sprite defaultSprite;
}
