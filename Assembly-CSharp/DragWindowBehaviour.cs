using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindowBehaviour : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
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
			this.setPOS.x = Mathf.Round(this.tempPOS.x);
			this.setPOS.y = Mathf.Round(this.tempPOS.y);
			this.windowRT.anchoredPosition = this.setPOS;
		}
	}

	private void Awake()
	{
		if (this.MyProduct == SOFTWARE_PRODUCTS.UNIVERSAL)
		{
			this.myWindowBeh = WindowManager.Get(this.MyProductData);
		}
		else
		{
			this.myWindowBeh = WindowManager.Get(this.MyProduct);
		}
		this.windowRT = this.myWindowBeh.Window.GetComponent<RectTransform>();
		this.dragPlane = LookUp.DesktopUI.DRAG_PLANE;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.dragPlane, eventData.position, eventData.pressEventCamera, out this.tempPOS))
		{
			this.bufferPos = this.tempPOS - this.windowRT.position;
		}
		this.SetDragPos(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		this.SetDragPos(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		this.myWindowBeh.MoveMe(this.windowRT.anchoredPosition);
	}

	public SOFTWARE_PRODUCTS MyProduct;

	public SoftwareProductDefinition MyProductData;

	private RectTransform windowRT;

	private RectTransform dragPlane;

	private Vector3 bufferPos;

	private Vector3 tempPOS;

	private Vector2 setPOS;

	private WindowBehaviour myWindowBeh;
}
