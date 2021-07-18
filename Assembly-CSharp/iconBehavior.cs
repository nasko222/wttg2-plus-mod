using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class iconBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEventSystemHandler
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
		if (!flag && RectTransformUtility.ScreenPointToWorldPointInRectangle(this.DragPlane, data.position, data.pressEventCamera, out vector))
		{
			vector -= this.bufferPos;
			this.myRT.position = vector;
			this.myData.MyPOS = Vect2.Convert(this.myRT.anchoredPosition);
			DataManager.Save<IconData>(this.myData);
		}
	}

	public void ActivateMe()
	{
		this.myCG.alpha = 1f;
		this.myRayCaster.enabled = true;
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<IconData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new IconData(this.myID);
			this.myData.MyPOS = Vect2.Convert(this.myRT.anchoredPosition);
		}
		this.myRT.anchoredPosition = this.myData.MyPOS.ToVector2;
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		if (this.MyProduct == SOFTWARE_PRODUCTS.UNIVERSAL)
		{
			this.myID = this.UniProductData.GetHashCode();
		}
		else
		{
			this.myID = (int)this.MyProduct;
		}
		this.myRayCaster = base.GetComponent<GraphicRaycaster>();
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myRT = base.GetComponent<RectTransform>();
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void Start()
	{
		this.clickCount = 0;
	}

	private void Update()
	{
		if (Time.time - this.myTimeStamp >= 1f)
		{
			this.clickCount = 0;
			this.myTimeStamp = Time.time;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		this.HoverIMG.enabled = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.HoverIMG.enabled = false;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.clickCount++;
		if (this.clickCount >= 2)
		{
			if (this.MyProduct == SOFTWARE_PRODUCTS.UNIVERSAL)
			{
				GameManager.ManagerSlinger.AppManager.LaunchApp(this.UniProductData);
			}
			else
			{
				GameManager.ManagerSlinger.AppManager.LaunchApp(this.MyProduct);
			}
			this.clickCount = 0;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Vector3 a;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.DragPlane, eventData.position, eventData.pressEventCamera, out a))
		{
			this.bufferPos = a - this.myRT.position;
		}
		this.SetDragPos(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		this.SetDragPos(eventData);
	}

	public Image DefaultIMG;

	public Image HoverIMG;

	public RectTransform DragPlane;

	public SOFTWARE_PRODUCTS MyProduct;

	public SoftwareProductDefinition UniProductData;

	private Vector3 bufferPos;

	private int clickCount;

	private float myTimeStamp;

	private GraphicRaycaster myRayCaster;

	private CanvasGroup myCG;

	private RectTransform myRT;

	private int myID;

	private IconData myData;
}
