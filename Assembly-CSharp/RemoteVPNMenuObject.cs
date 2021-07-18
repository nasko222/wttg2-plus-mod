using System;
using TMPro;
using UnityEngine;

public class RemoteVPNMenuObject : MonoBehaviour
{
	public void ClearMe()
	{
		this.myRT.anchoredPosition = this.spawnPOS;
		this.vpnLabel.SetText(string.Empty);
		this.dosCoinLabel.SetText(string.Empty);
		this.timeLabel.SetText(string.Empty);
	}

	public void BuildMe(RemoteVPNObject TheRemoteVPN)
	{
		this.vpnLabel.SetText("VPN " + this.myIndex.ToString());
		this.dosCoinLabel.SetText(TheRemoteVPN.DOSCoinValue);
		this.timeLabel.SetText(TheRemoteVPN.TimeValue);
	}

	public void PutMe(int SetIndex)
	{
		float y = -(4f + ((float)SetIndex * 4f + (float)SetIndex * 22f));
		this.myRT.anchoredPosition = new Vector2(0f, y);
	}

	public void SoftBuild(int SetIndex)
	{
		this.myRT = base.GetComponent<RectTransform>();
		this.myRT.anchoredPosition = this.spawnPOS;
		this.myIndex = SetIndex;
	}

	[SerializeField]
	private TextMeshProUGUI vpnLabel;

	[SerializeField]
	private TextMeshProUGUI dosCoinLabel;

	[SerializeField]
	private TextMeshProUGUI timeLabel;

	private const float OPT_SPACING = 4f;

	private const float MENU_OPT_X = 0f;

	private RectTransform myRT;

	private Vector2 spawnPOS = new Vector2(0f, 22f);

	private int myIndex;
}
