using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialBTN : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void SetLock(bool value)
	{
		this.locked = value;
	}

	private void Awake()
	{
		this.defaultSprite = this.bgImage.sprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.CursorManager.PointerCursorState(true);
		if (!this.locked)
		{
			this.bgImage.sprite = this.hoverSprite;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
		this.bgImage.sprite = this.defaultSprite;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.locked)
		{
			this.locked = true;
			this.bgImage.sprite = this.defaultSprite;
			this.ClickAction.Execute();
		}
	}

	public CustomEvent ClickAction = new CustomEvent(2);

	[SerializeField]
	private Image bgImage;

	[SerializeField]
	private Sprite hoverSprite;

	private Sprite defaultSprite;

	private bool locked;
}
