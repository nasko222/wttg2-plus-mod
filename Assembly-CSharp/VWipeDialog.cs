using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class VWipeDialog : MonoBehaviour
{
	public void Present()
	{
		LookUp.DesktopUI.VWIPE_PROGRESS_BAR.fillAmount = 0f;
		LookUp.DesktopUI.DIALOG_HOLDER.SetActive(true);
		DOTween.To(() => LookUp.DesktopUI.DIALOG_BG_OBJECT.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			LookUp.DesktopUI.DIALOG_BG_OBJECT.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.35f).SetEase(Ease.Linear).OnComplete(delegate
		{
			LookUp.DesktopUI.VWIPE_DIALOG_HOLDER.SetActive(true);
			Sequence sequence = DOTween.Sequence().OnComplete(delegate
			{
				this.VWipeDialogWasPresented.Execute();
			});
			sequence.Insert(0f, DOTween.To(() => LookUp.DesktopUI.VWIPE_DIALOG_HOLDER.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
			{
				LookUp.DesktopUI.VWIPE_DIALOG_HOLDER.GetComponent<RectTransform>().localScale = x;
			}, Vector3.one, 0.35f).SetEase(Ease.OutCirc));
			sequence.Insert(0f, DOTween.To(() => LookUp.DesktopUI.VWIPE_DIALOG_HOLDER.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				LookUp.DesktopUI.VWIPE_DIALOG_HOLDER.GetComponent<CanvasGroup>().alpha = x;
			}, 1f, 0.25f).SetEase(Ease.Linear));
			sequence.Play<Sequence>();
		});
	}

	public void PerformScanRemove(int NumOfVir)
	{
		float duration = UnityEngine.Random.Range(5f, 12f);
		DOTween.To(() => LookUp.DesktopUI.VWIPE_PROGRESS_BAR.fillAmount, delegate(float x)
		{
			LookUp.DesktopUI.VWIPE_PROGRESS_BAR.fillAmount = x;
		}, 1f, duration).SetEase(Ease.Linear).OnComplete(delegate
		{
			LookUp.DesktopUI.VWIPE_VIRUS_FOUND_TEXT.text = "Viruses Found: " + NumOfVir.ToString();
			if (NumOfVir > 0)
			{
				float removeTime = UnityEngine.Random.Range(6f, 10f) + (float)NumOfVir * UnityEngine.Random.Range(3f, 5f);
				GameManager.TimeSlinger.FireTimer(1.5f, delegate()
				{
					LookUp.DesktopUI.VWIPE_INFO_TEXT.text = "Removing Viruses...";
					LookUp.DesktopUI.VWIPE_PROGRESS_BAR.fillAmount = 0f;
					DOTween.To(() => LookUp.DesktopUI.VWIPE_PROGRESS_BAR.fillAmount, delegate(float x)
					{
						LookUp.DesktopUI.VWIPE_PROGRESS_BAR.fillAmount = x;
					}, 1f, removeTime).SetEase(Ease.Linear).SetDelay(0.5f).OnComplete(delegate
					{
						LookUp.DesktopUI.VWIPE_INFO_TEXT.text = "Removed All Viruses!";
						GameManager.TimeSlinger.FireTimer(1.5f, new Action(<PerformScanRemove>c__AnonStorey.VWipeScanRemoveDone.Execute), 0);
					});
				}, 0);
			}
			else
			{
				LookUp.DesktopUI.VWIPE_INFO_TEXT.text = "No Viruses Found!";
				GameManager.TimeSlinger.FireTimer(1.5f, new Action(this.VWipeScanRemoveDone.Execute), 0);
			}
		});
	}

	public void Dismiss()
	{
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			DOTween.To(() => LookUp.DesktopUI.DIALOG_BG_OBJECT.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				LookUp.DesktopUI.DIALOG_BG_OBJECT.GetComponent<CanvasGroup>().alpha = x;
			}, 0f, 0.35f).SetEase(Ease.Linear).OnComplete(delegate
			{
				LookUp.DesktopUI.VWIPE_VIRUS_FOUND_TEXT.text = "Viruses Found: 0";
				LookUp.DesktopUI.VWIPE_INFO_TEXT.text = "Scanning For Viruses...";
				LookUp.DesktopUI.DIALOG_HOLDER.SetActive(false);
				LookUp.DesktopUI.VWIPE_DIALOG_HOLDER.SetActive(false);
			});
		});
		sequence.Insert(0f, DOTween.To(() => LookUp.DesktopUI.VWIPE_DIALOG_HOLDER.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			LookUp.DesktopUI.VWIPE_DIALOG_HOLDER.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(0.1f, 0.1f, 0.1f), 0.35f).SetEase(Ease.OutCirc));
		sequence.Insert(0f, DOTween.To(() => LookUp.DesktopUI.VWIPE_DIALOG_HOLDER.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			LookUp.DesktopUI.VWIPE_DIALOG_HOLDER.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.35f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	private void Awake()
	{
		UIDialogManager.VWipeDialog = this;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public CustomEvent VWipeDialogWasPresented = new CustomEvent(1);

	public CustomEvent VWipeScanRemoveDone = new CustomEvent(1);
}
