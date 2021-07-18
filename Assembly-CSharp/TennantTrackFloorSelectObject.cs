using System;
using UnityEngine;
using UnityEngine.UI;

public class TennantTrackFloorSelectObject : MonoBehaviour
{
	public int FloorNumber
	{
		get
		{
			return this.myFloorNumber;
		}
	}

	public void ActivateMe()
	{
		this.Active = true;
		this.activeBGImage.enabled = true;
		this.floorNumberText.color = this.activeColorText;
	}

	public void DeActivateMe()
	{
		this.Active = false;
		this.activeBGImage.enabled = false;
		this.floorNumberText.color = this.idleColorText;
	}

	public bool Active;

	[SerializeField]
	private Image activeBGImage;

	[SerializeField]
	private Text floorNumberText;

	[SerializeField]
	private Color activeColorText;

	[SerializeField]
	private Color idleColorText;

	[SerializeField]
	private int myFloorNumber;
}
