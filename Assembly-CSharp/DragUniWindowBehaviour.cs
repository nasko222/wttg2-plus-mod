using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragUniWindowBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEventSystemHandler
{
	private void SetDragPos(PointerEventData data)
	{
		bool flag = false;
		if (Input.mousePosition.x < MagicSlinger.GetScreenWidthPXByPerc(0.01f))
		{
			flag = true;
		}
		else if (Input.mousePosition.x > MagicSlinger.GetScreenWidthPXByPerc(0.98f))
		{
			flag = true;
		}
		else if (Input.mousePosition.y < MagicSlinger.GetScreenHeightPXByPerc(0.05f))
		{
			flag = true;
		}
		else if (Input.mousePosition.y > MagicSlinger.GetScreenHeightPXByPerc(0.95f))
		{
			flag = true;
		}
		if (!flag && RectTransformUtility.ScreenPointToWorldPointInRectangle(this.dragPlane, data.position, data.pressEventCamera, out this.tempPOS))
		{
			this.tempPOS -= this.bufferPos;
			if (this.FactorInScreenSize)
			{
				this.setPOS.x = Mathf.Round(this.tempPOS.x - (float)Screen.width / 2f);
				this.setPOS.y = Mathf.Round(this.tempPOS.y + (float)Screen.height / 2f);
			}
			else
			{
				this.setPOS.x = Mathf.Round(this.tempPOS.x);
				this.setPOS.y = Mathf.Round(this.tempPOS.y);
			}
			this.parentWindow.rectTransform.anchoredPosition = this.setPOS;
		}
	}

	private void Awake()
	{
		this.dragPlane = LookUp.DesktopUI.DRAG_PLANE;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!GameManager.PauseManager.Paused)
		{
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.dragPlane, eventData.position, eventData.pressEventCamera, out this.tempPOS))
			{
				this.bufferPos = this.tempPOS - this.parentWindow.rectTransform.position;
			}
			this.SetDragPos(eventData);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!GameManager.PauseManager.Paused)
		{
			this.SetDragPos(eventData);
		}
	}

	public Image parentWindow;

	public bool FactorInScreenSize;

	private Vector3 bufferPos;

	private Vector3 tempPOS;

	private Vector2 setPOS;

	private RectTransform dragPlane;
}
