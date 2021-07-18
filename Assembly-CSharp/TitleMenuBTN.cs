using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(GraphicRaycaster))]
public class TitleMenuBTN : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void ActiveState(bool Active)
	{
		this.myCG.alpha = ((!Active) ? 0.15f : 1f);
		this.myCG.interactable = Active;
		this.myCG.blocksRaycasts = Active;
	}

	private void Awake()
	{
		this.myCG = base.GetComponent<CanvasGroup>();
		this.defaultColor = this.titleText.color;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.isPauseMenu)
		{
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.MenuHover);
		}
		else
		{
			GameManager.AudioSlinger.PlaySound(TitleLookUp.Ins.TitleMenuHoverSFX);
		}
		this.titleText.color = this.hoverColor;
		if (CursorManager.Ins != null)
		{
			CursorManager.Ins.PointerCursorState(true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.titleText.color = this.defaultColor;
		if (CursorManager.Ins != null)
		{
			CursorManager.Ins.PointerCursorState(false);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.isPauseMenu)
		{
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.MenuClick);
		}
		else
		{
			GameManager.AudioSlinger.PlaySound(TitleLookUp.Ins.TitleMenuClickSFX);
		}
		this.MyAction.Execute();
	}

	public CustomEvent MyAction = new CustomEvent(1);

	[SerializeField]
	private TextMeshProUGUI titleText;

	[SerializeField]
	private Color hoverColor;

	[SerializeField]
	private bool isPauseMenu;

	private CanvasGroup myCG;

	private Color defaultColor;
}
