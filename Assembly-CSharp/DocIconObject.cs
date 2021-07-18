using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DocIconObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	private void setDragPOS(PointerEventData data)
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
			vector -= this.bufferPos;
			this.myRT.position = vector;
			this.myData.MyPOS = Vect2.Convert(this.myRT.anchoredPosition);
			DataManager.Save<IconData>(this.myData);
		}
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
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
		this.myRayCaster = base.GetComponent<GraphicRaycaster>();
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myRT = base.GetComponent<RectTransform>();
		this.dragPlane = LookUp.DesktopUI.DRAG_PLANE;
		GameManager.StageManager.Stage += this.stageMe;
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
		this.hoverIMG.enabled = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.hoverIMG.enabled = false;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.clickCount++;
		if (this.clickCount >= 2)
		{
			this.clickCount = 0;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Vector3 a;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.dragPlane, eventData.position, eventData.pressEventCamera, out a))
		{
			this.bufferPos = a - this.myRT.position;
		}
		this.setDragPOS(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		this.setDragPOS(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		this.UpdateMyData.Execute();
	}

	[SerializeField]
	private Image defaultIMG;

	[SerializeField]
	private Image hoverIMG;

	[SerializeField]
	private Text title1;

	[SerializeField]
	private Text title2;

	public CustomEvent UpdateMyData = new CustomEvent(1);

	public CustomEvent<TextDocIconData> OpenEvents = new CustomEvent<TextDocIconData>(1);

	private RectTransform dragPlane;

	private Vector3 bufferPos;

	private RectTransform myRT;

	private GraphicRaycaster myRayCaster;

	private CanvasGroup myCG;

	private int clickCount;

	private float myTimeStamp;

	private int myID;

	private IconData myData;
}
