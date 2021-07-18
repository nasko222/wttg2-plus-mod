using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public static class UIInventoryManager
{
	public static void ShowWifiDongle()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.WifiDongleIcon.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			LookUp.PlayerUI.WifiDongleIcon.GetComponent<RectTransform>().localScale = x;
		}, UIInventoryManager._iconInvFullSize, 0.25f).SetEase(Ease.OutExpo));
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.WifiDongleIcon.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			LookUp.PlayerUI.WifiDongleIcon.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public static void HideWifiDongle()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.WifiDongleIcon.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			LookUp.PlayerUI.WifiDongleIcon.GetComponent<RectTransform>().localScale = x;
		}, UIInventoryManager._iconInvMinSize, 0.25f).SetEase(Ease.InExpo));
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.WifiDongleIcon.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			LookUp.PlayerUI.WifiDongleIcon.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public static void ShowMotionSensor()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.MotionSensorIcon.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			LookUp.PlayerUI.MotionSensorIcon.GetComponent<RectTransform>().localScale = x;
		}, UIInventoryManager._iconInvFullSize, 0.25f).SetEase(Ease.OutExpo));
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.MotionSensorIcon.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			LookUp.PlayerUI.MotionSensorIcon.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public static void HideMotionSensor()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.MotionSensorIcon.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			LookUp.PlayerUI.MotionSensorIcon.GetComponent<RectTransform>().localScale = x;
		}, UIInventoryManager._iconInvMinSize, 0.25f).SetEase(Ease.InExpo));
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.MotionSensorIcon.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			LookUp.PlayerUI.MotionSensorIcon.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public static void ShowRemoteVPN()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.RemoteVPNIcon.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			LookUp.PlayerUI.RemoteVPNIcon.GetComponent<RectTransform>().localScale = x;
		}, UIInventoryManager._iconInvFullSize, 0.25f).SetEase(Ease.OutExpo));
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.RemoteVPNIcon.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			LookUp.PlayerUI.RemoteVPNIcon.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public static void HideRemoteVPN()
	{
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNZeroBar;
		});
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.RemoteVPNIcon.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			LookUp.PlayerUI.RemoteVPNIcon.GetComponent<RectTransform>().localScale = x;
		}, UIInventoryManager._iconInvMinSize, 0.25f).SetEase(Ease.InExpo));
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.RemoteVPNIcon.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			LookUp.PlayerUI.RemoteVPNIcon.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public static void ShowDollMakerMarker()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.DollMakerMarkerIcon.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			LookUp.PlayerUI.DollMakerMarkerIcon.GetComponent<RectTransform>().localScale = x;
		}, UIInventoryManager._iconInvFullSize, 0.25f).SetEase(Ease.OutExpo));
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.DollMakerMarkerIcon.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			LookUp.PlayerUI.DollMakerMarkerIcon.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public static void HideDollMakerMarker()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.DollMakerMarkerIcon.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			LookUp.PlayerUI.DollMakerMarkerIcon.GetComponent<RectTransform>().localScale = x;
		}, UIInventoryManager._iconInvMinSize, 0.25f).SetEase(Ease.InExpo));
		sequence.Insert(0f, DOTween.To(() => LookUp.PlayerUI.DollMakerMarkerIcon.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			LookUp.PlayerUI.DollMakerMarkerIcon.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	private static Vector3 _iconInvFullSize = Vector3.one;

	private static Vector3 _iconInvMinSize = new Vector3(0.1f, 0.1f, 0.1f);
}
