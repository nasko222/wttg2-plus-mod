using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class desktopBehavior : MonoBehaviour
{
	public void ChangeWifiBars(int wifiBarAmount)
	{
		this.WifiIcon.GetComponent<Image>().sprite = this.WifiSprites[wifiBarAmount];
	}

	public void TriggerWifiMenu()
	{
		if (!this.wifiMenuAniActive)
		{
			this.wifiMenuAniActive = true;
			GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.resetWifiMenuAniActive), 0);
			if (this.wifiMenuActive)
			{
				this.wifiMenuActive = false;
				float num = Mathf.Floor(this.WifiMenu.GetComponent<RectTransform>().sizeDelta.y / 2f + 41f);
				DOTween.To(() => this.WifiMenu.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
				{
					this.WifiMenu.GetComponent<RectTransform>().localPosition = x;
				}, new Vector3(this.WifiMenu.GetComponent<RectTransform>().localPosition.x, num, 0f), 0.25f).SetEase(Ease.InQuad);
			}
			else
			{
				this.wifiMenuActive = true;
				float num = Mathf.Floor(this.WifiMenu.GetComponent<RectTransform>().sizeDelta.y / 2f + 41f);
				DOTween.To(() => this.WifiMenu.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
				{
					this.WifiMenu.GetComponent<RectTransform>().localPosition = x;
				}, new Vector3(this.WifiMenu.GetComponent<RectTransform>().localPosition.x, -num, 0f), 0.25f).SetEase(Ease.OutQuad);
			}
		}
	}

	private void prepDesktopBehavior()
	{
		this.wifiMenuActive = false;
	}

	private void updateClocks()
	{
	}

	private void resetWifiMenuAniActive()
	{
		this.wifiMenuAniActive = false;
	}

	private void Awake()
	{
	}

	private void Start()
	{
		this.prepDesktopBehavior();
	}

	private void Update()
	{
	}

	public GameObject WifiIcon;

	public List<Sprite> WifiSprites;

	public GameObject WifiMenu;

	private bool wifiMenuActive;

	private bool wifiMenuAniActive;
}
