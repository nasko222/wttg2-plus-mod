using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class TutorialVoiceCallBehaviour : MonoBehaviour
{
	public void PresentMe()
	{
		DOTween.To(() => this.myRT.anchoredPosition, delegate(Vector2 x)
		{
			this.myRT.anchoredPosition = x;
		}, new Vector2(-25f, -62f), 0.5f).SetEase(Ease.OutQuad).OnComplete(delegate
		{
			this.WasPresentedEvents.Execute();
			this.currentTime = 0f;
			this.timeTimeStamp = Time.time;
			this.timeActive = true;
		});
	}

	public void DismissMe()
	{
		DOTween.To(() => this.myRT.anchoredPosition, delegate(Vector2 x)
		{
			this.myRT.anchoredPosition = x;
		}, new Vector2(252f, -62f), 0.5f).SetEase(Ease.InQuad).OnComplete(delegate
		{
			this.timeActive = false;
			this.currentTime = 0f;
			this.timeText.text = "00:00";
		});
	}

	public void ForceEndCall()
	{
		this.wasHungUp();
	}

	private void updateTimeText()
	{
		int num = Mathf.FloorToInt(this.currentTime / 60f);
		int num2 = Mathf.FloorToInt(this.currentTime - (float)num * 60f);
		this.timeText.text = num.ToString("D2") + ":" + num2.ToString("D2");
	}

	private void wasHungUp()
	{
		GameManager.AudioSlinger.PlaySound(TutorialManager.Ins.DeclineCallSFX);
		this.DismissMe();
		this.HangUpEvents.Execute();
	}

	private void Awake()
	{
		TutorialVoiceCallBehaviour.Ins = this;
		this.myRT = base.GetComponent<RectTransform>();
		this.hangUpBTN.ClickAction.Event += this.wasHungUp;
	}

	private void Update()
	{
		if (this.timeActive && Time.time - this.timeTimeStamp >= 1f)
		{
			this.timeTimeStamp = Time.time;
			this.currentTime += 1f;
			this.updateTimeText();
		}
	}

	private void OnDestroy()
	{
		this.hangUpBTN.ClickAction.Event -= this.wasHungUp;
	}

	public static TutorialVoiceCallBehaviour Ins;

	public CustomEvent WasPresentedEvents = new CustomEvent(2);

	public CustomEvent HangUpEvents = new CustomEvent(2);

	[SerializeField]
	private Text timeText;

	[SerializeField]
	private TutorialBTN hangUpBTN;

	private RectTransform myRT;

	private bool timeActive;

	private float timeTimeStamp;

	private float currentTime;
}
