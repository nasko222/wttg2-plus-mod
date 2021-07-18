using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VPNMenuDisObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VPNMenuDisObject.PressedActions IWasPressed;

	private void Awake()
	{
		this.myImage = base.GetComponent<Image>();
		this.defaultColor = this.myImage.color;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		this.myImage.color = this.hoverColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.myImage.color = this.defaultColor;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.VPNManager.DisconnectFromVPN();
		GameManager.ManagerSlinger.VPNManager.TriggerVPNMenu();
		if (this.IWasPressed != null)
		{
			this.IWasPressed();
		}
	}

	[SerializeField]
	private Color hoverColor;

	private Color defaultColor;

	private Image myImage;

	public delegate void PressedActions();
}
