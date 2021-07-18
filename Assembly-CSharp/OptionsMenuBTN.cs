using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(GraphicRaycaster))]
public class OptionsMenuBTN : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler
{
	public bool Active
	{
		get
		{
			return this.isActive;
		}
	}

	public void Clear()
	{
		this.titleText.color = this.inActiveDefaultColor;
		this.isActive = false;
	}

	public void SetActive()
	{
		this.isActive = true;
		this.titleText.color = this.activeDefaultColor;
	}

	private void Awake()
	{
		this.Clear();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameManager.AudioSlinger.PlaySound(TitleLookUp.Ins.TitleMenuHoverSFX);
		CursorManager.Ins.PointerCursorState(true);
		if (this.isActive)
		{
			this.titleText.color = this.activeHoverColor;
		}
		else
		{
			this.titleText.color = this.inActiveHoverColor;
		}
		this.HoverAction.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		CursorManager.Ins.PointerCursorState(false);
		if (this.isActive)
		{
			this.titleText.color = this.activeDefaultColor;
		}
		else
		{
			this.titleText.color = this.inActiveDefaultColor;
		}
		this.ExitAction.Invoke();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		GameManager.AudioSlinger.PlaySound(TitleLookUp.Ins.TitleMenuClickSFX);
		this.SetActive();
		this.ClickAction.Invoke();
	}

	[SerializeField]
	private TextMeshProUGUI titleText;

	[SerializeField]
	private Color activeDefaultColor;

	[SerializeField]
	private Color activeHoverColor;

	[SerializeField]
	private Color inActiveDefaultColor;

	[SerializeField]
	private Color inActiveHoverColor;

	[SerializeField]
	public UnityEvent ClickAction;

	[SerializeField]
	private UnityEvent HoverAction;

	[SerializeField]
	private UnityEvent ExitAction;

	private bool isActive;
}
