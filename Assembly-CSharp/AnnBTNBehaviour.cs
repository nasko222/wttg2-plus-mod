using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnnBTNBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void setActive(bool setValue)
	{
		this.isActive = setValue;
	}

	public void setLock(bool setValue)
	{
		this.isLocked = setValue;
	}

	public bool GetActiveState()
	{
		return this.isActive;
	}

	private void prepBTN()
	{
		this.isActive = this.activeAtStart;
		this.defaultSprite = base.GetComponent<Image>().sprite;
		this.myCG = base.gameObject.AddComponent<CanvasGroup>();
		this.myCG.interactable = true;
		this.myCG.blocksRaycasts = true;
	}

	private void Start()
	{
		this.prepBTN();
	}

	private void Update()
	{
		if (this.isActive && !this.isLocked)
		{
			this.myCG.alpha = 1f;
		}
		else
		{
			base.GetComponent<Image>().sprite = this.defaultSprite;
			this.myCG.alpha = 0.25f;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.isLocked && this.isActive && this.hoverSprite != null)
		{
			base.GetComponent<Image>().sprite = this.hoverSprite;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.isLocked && this.isActive)
		{
			base.GetComponent<Image>().sprite = this.defaultSprite;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.isLocked && this.isActive)
		{
			GameManager.BehaviourManager.AnnBehaviour.AnnBTNAction(this.MyAction);
		}
	}

	public bool activeAtStart;

	public Sprite hoverSprite;

	public ANN_BTN_ACTIONS MyAction;

	private Sprite defaultSprite;

	private bool isActive;

	private bool isLocked;

	private CanvasGroup myCG;
}
