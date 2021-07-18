using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class NetworkDialog : MonoBehaviour
{
	public void Present(WifiNetworkDefinition NetworkToJoin)
	{
		this.curNetworkToJoin = NetworkToJoin;
		LookUp.DesktopUI.DIALOG_HOLDER.SetActive(true);
		DOTween.To(() => LookUp.DesktopUI.DIALOG_BG_OBJECT.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			LookUp.DesktopUI.DIALOG_BG_OBJECT.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.NetworkDialogObject.SetActive(true);
			this.PasswordInput.GetComponent<InputField>().text = string.Empty;
			this.WifiNameType.GetComponent<Text>().text = string.Concat(new string[]
			{
				"The WiFi network ",
				NetworkToJoin.networkName,
				" requires a  ",
				MagicSlinger.GetNetworkSecurityType(NetworkToJoin.networkSecurity),
				" password."
			});
			DOTween.To(() => this.NetworkDialogObject.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
			{
				this.NetworkDialogObject.GetComponent<RectTransform>().localScale = x;
			}, this.scaleMaxSize, 0.35f).SetEase(Ease.OutCirc);
		});
	}

	private void dismiss()
	{
		DOTween.To(() => this.NetworkDialogObject.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.NetworkDialogObject.GetComponent<RectTransform>().localScale = x;
		}, this.scaleMinSize, 0.35f).SetEase(Ease.InCirc).OnComplete(delegate
		{
			this.curNetworkToJoin = null;
			this.PasswordInput.GetComponent<InputField>().text = string.Empty;
			this.WifiNameType.GetComponent<Text>().text = string.Empty;
			this.NetworkDialogObject.SetActive(false);
			DOTween.To(() => LookUp.DesktopUI.DIALOG_BG_OBJECT.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				LookUp.DesktopUI.DIALOG_BG_OBJECT.GetComponent<CanvasGroup>().alpha = x;
			}, 0f, 0.25f).OnComplete(delegate
			{
				LookUp.DesktopUI.DIALOG_HOLDER.SetActive(false);
			});
		});
	}

	private void connectToNetwork()
	{
		if (this.PasswordInput.GetComponent<InputField>().text.Equals(this.curNetworkToJoin.networkPassword))
		{
			GameManager.ManagerSlinger.WifiManager.ConnectToWifi(this.curNetworkToJoin, true);
			this.dismiss();
		}
		else
		{
			this.PasswordInput.GetComponent<InputField>().text = string.Empty;
			this.noSeq = DOTween.Sequence();
			this.noSeq.Insert(0f, DOTween.To(() => this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
			{
				this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition = x;
			}, new Vector2(-30f, 0f), 0.15f).SetEase(Ease.InSine).SetRelative(true));
			this.noSeq.Insert(0.15f, DOTween.To(() => this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
			{
				this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition = x;
			}, new Vector2(60f, 0f), 0.15f).SetEase(Ease.OutSine).SetRelative(true));
			this.noSeq.Insert(0.3f, DOTween.To(() => this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
			{
				this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition = x;
			}, new Vector2(-50f, 0f), 0.15f).SetEase(Ease.InSine).SetRelative(true));
			this.noSeq.Insert(0.45f, DOTween.To(() => this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
			{
				this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition = x;
			}, new Vector2(40f, 0f), 0.15f).SetEase(Ease.OutSine).SetRelative(true));
			this.noSeq.Insert(0.6f, DOTween.To(() => this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
			{
				this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition = x;
			}, new Vector2(-30f, 0f), 0.15f).SetEase(Ease.InSine).SetRelative(true));
			this.noSeq.Insert(0.75f, DOTween.To(() => this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
			{
				this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition = x;
			}, new Vector2(20f, 0f), 0.15f).SetEase(Ease.OutSine).SetRelative(true));
			this.noSeq.Insert(0.9f, DOTween.To(() => this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
			{
				this.NetworkDialogObject.GetComponent<RectTransform>().anchoredPosition = x;
			}, new Vector2(-10f, 0f), 0.15f).SetEase(Ease.InSine).SetRelative(true));
			this.noSeq.Play<Sequence>();
		}
	}

	private void cancelConnect()
	{
		this.dismiss();
	}

	private void Awake()
	{
		UIDialogManager.NetworkDialog = this;
		this.dialogBTNConnect = this.ConnectBTN.GetComponent<DialogBTNBehaviour>();
		this.dialogBTNCancel = this.CancelBTN.GetComponent<DialogBTNBehaviour>();
		this.dialogBTNConnect.OnPress += this.connectToNetwork;
		this.dialogBTNCancel.OnPress += this.cancelConnect;
	}

	private void OnDestroy()
	{
		this.dialogBTNConnect.OnPress -= this.connectToNetwork;
		this.dialogBTNCancel.OnPress -= this.cancelConnect;
	}

	public GameObject NetworkDialogObject;

	public GameObject ConnectBTN;

	public GameObject CancelBTN;

	public GameObject WifiNameType;

	public GameObject PasswordInput;

	private const float BG_FADE_TIME = 0.25f;

	private WifiNetworkDefinition curNetworkToJoin;

	private Vector3 scaleMaxSize = Vector3.one;

	private Vector3 scaleMinSize = new Vector3(0.1f, 0.1f, 0.1f);

	private Sequence noSeq;

	private DialogBTNBehaviour dialogBTNConnect;

	private DialogBTNBehaviour dialogBTNCancel;
}
