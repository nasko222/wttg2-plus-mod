using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VPNMenuObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VPNMenuObject.VPNMenuActions IWasPressed;

	public void SoftBuild()
	{
		this.myRT = base.GetComponent<RectTransform>();
		this.myImage = base.GetComponent<Image>();
		this.defaultColor = this.myImage.color;
		this.myRT.anchoredPosition = new Vector2(0f, 24f);
		this.vpnName1.text = string.Empty;
		this.vpnName2.text = string.Empty;
		this.connectedIMG.enabled = false;
	}

	public void SetMeConnected(VPN_LEVELS ConnectedVPN)
	{
		if (this.myLevel == ConnectedVPN)
		{
			this.connectedIMG.enabled = true;
		}
		else
		{
			this.connectedIMG.enabled = false;
		}
	}

	public void BuildMe(VPN_LEVELS SetLevel, int MyIndex)
	{
		this.myLevel = SetLevel;
		string text = string.Empty;
		switch (this.myLevel)
		{
		case VPN_LEVELS.LEVEL1:
			text = "VPN Level 1";
			break;
		case VPN_LEVELS.LEVEL2:
			text = "VPN Level 2";
			break;
		case VPN_LEVELS.LEVEL3:
			text = "VPN Level 3";
			break;
		case VPN_LEVELS.LEVEL4:
			text = "VPN Level 4";
			break;
		case VPN_LEVELS.LEVEL5:
			text = "VPN Level 5";
			break;
		}
		this.vpnName1.text = text;
		this.vpnName2.text = text;
		float y = -(4f + ((float)MyIndex * 4f + (float)MyIndex * 24f));
		this.myRT.anchoredPosition = new Vector2(10f, y);
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
		if (this.IWasPressed != null)
		{
			this.IWasPressed(this.myLevel);
		}
		GameManager.ManagerSlinger.VPNManager.TriggerVPNMenu();
	}

	[SerializeField]
	private Text vpnName1;

	[SerializeField]
	private Text vpnName2;

	[SerializeField]
	private Image connectedIMG;

	[SerializeField]
	private Color hoverColor;

	private const float SET_X = 10f;

	private const float OPT_SPACING = 4f;

	private Color defaultColor;

	private Image myImage;

	private RectTransform myRT;

	private VPN_LEVELS myLevel;

	public delegate void VPNMenuActions(VPN_LEVELS MyLevel);
}
