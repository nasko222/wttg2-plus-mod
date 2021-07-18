using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeWindowBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
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
		Vector3 vector;
		if (!flag && RectTransformUtility.ScreenPointToWorldPointInRectangle(this.dragPlane, data.position, data.pressEventCamera, out vector))
		{
			vector.x = this.startingSize.x + (vector.x - this.bufferPOS.x);
			vector.y = this.startingSize.y - (vector.y - this.bufferPOS.y);
			vector.x = Mathf.Max(vector.x, 335f);
			vector.y = Mathf.Max(vector.y, 300f);
			vector.x = Mathf.Round(vector.x);
			vector.y = Mathf.Round(vector.y);
			this.myWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(vector.x, vector.y);
		}
	}

	public void Start()
	{
		if (this.MyProduct == SOFTWARE_PRODUCTS.UNIVERSAL)
		{
			this.myWindow = WindowManager.Get(this.MyProductData).Window;
		}
		else
		{
			this.myWindow = WindowManager.Get(this.MyProduct).Window;
		}
		this.dragPlane = LookUp.DesktopUI.DRAG_PLANE;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.CursorManager.ResizeCursorState(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.CursorManager.ResizeCursorState(false);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		this.myWindow.gameObject.GetComponent<BringWindowToFrontBehaviour>().forceTap();
		Vector3 vector;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.dragPlane, eventData.position, eventData.pressEventCamera, out vector))
		{
			this.startingSize = this.myWindow.GetComponent<RectTransform>().sizeDelta;
			this.bufferPOS.x = vector.x;
			this.bufferPOS.y = vector.y;
		}
		this.SetDragPos(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.AppManager.ResizedApp(this.MyProduct);
	}

	public void OnDrag(PointerEventData eventData)
	{
		this.SetDragPos(eventData);
	}

	public SOFTWARE_PRODUCTS MyProduct;

	public SoftwareProductDefinition MyProductData;

	private RectTransform dragPlane;

	private GameObject myWindow;

	private Vector2 startingSize;

	private Vector3 bufferPOS;
}
