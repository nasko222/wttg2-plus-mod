using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	public AudioFileDefinition DeclineCallSFX
	{
		get
		{
			return this.declineCallSFX;
		}
	}

	public void ClearOut()
	{
		TutorialStartBehaviour.Ins.TutoralHasEndedEvents.Event -= this.endTutorialMode;
		computerController.Ins.SetMasterLock(false);
		this.blackGG.alpha = 0f;
		this.blackGG.interactable = false;
		this.blackGG.blocksRaycasts = false;
		GameManager.TimeSlinger.FireTimer(0.5f, delegate()
		{
			this.tutorialHolder.SetActive(false);
		}, 0);
	}

	public void PresentProductCall()
	{
		this.tutorialHolder.SetActive(true);
		EnvironmentManager.PowerBehaviour.PowerOffEvent.Event += this.powerWentOffWhileProdTutActive;
		this.incomingCallLargeAcceptBTN.ClickAction.Event += this.playerAcceptedProductCall;
		this.incomingCallLargeDeclineBTN.ClickAction.Event += this.playerDeclinedProductCall;
		this.blackGG.interactable = true;
		this.blackGG.blocksRaycasts = true;
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.vacationRinging);
			this.adamScaleTween = DOTween.To(() => this.adamLargeRT.localScale, delegate(Vector3 x)
			{
				this.adamLargeRT.localScale = x;
			}, new Vector3(0.9f, 0.9f, 1f), 0.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
			this.adamScaleTween.Play<Tweener>();
			this.incomingCallLargeAcceptBTN.SetLock(false);
			this.incomingCallLargeDeclineBTN.SetLock(false);
		});
		sequence.Insert(0f, DOTween.To(() => this.blackGG.alpha, delegate(float x)
		{
			this.blackGG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		sequence.Insert(0.3f, DOTween.To(() => this.incomingCallLargeRT.localScale, delegate(Vector3 x)
		{
			this.incomingCallLargeRT.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.35f).SetEase(Ease.InQuart));
		sequence.Insert(0.3f, DOTween.To(() => this.incomingCallLargeCG.alpha, delegate(float x)
		{
			this.incomingCallLargeCG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	private void presentStartCall()
	{
		this.tutorialHolder.SetActive(true);
		startController.Ins.TriggerTutorialEvents.Event -= this.presentStartCall;
		this.blackGG.interactable = true;
		this.blackGG.blocksRaycasts = true;
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.vacationRinging);
			this.adamScaleTween = DOTween.To(() => this.adamLargeRT.localScale, delegate(Vector3 x)
			{
				this.adamLargeRT.localScale = x;
			}, new Vector3(0.9f, 0.9f, 1f), 0.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
			this.adamScaleTween.Play<Tweener>();
			this.incomingCallLargeAcceptBTN.SetLock(false);
			this.incomingCallLargeDeclineBTN.SetLock(false);
		});
		sequence.Insert(0f, DOTween.To(() => this.blackGG.alpha, delegate(float x)
		{
			this.blackGG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		sequence.Insert(0.3f, DOTween.To(() => this.incomingCallLargeRT.localScale, delegate(Vector3 x)
		{
			this.incomingCallLargeRT.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.35f).SetEase(Ease.InQuart));
		sequence.Insert(0.3f, DOTween.To(() => this.incomingCallLargeCG.alpha, delegate(float x)
		{
			this.incomingCallLargeCG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	private void dismissStartCall()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.incomingCallLargeRT.localScale, delegate(Vector3 x)
		{
			this.incomingCallLargeRT.localScale = x;
		}, new Vector3(0.1f, 0.1f, 1f), 0.25f).SetEase(Ease.OutQuad));
		sequence.Insert(0.1f, DOTween.To(() => this.incomingCallLargeCG.alpha, delegate(float x)
		{
			this.incomingCallLargeCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear));
		sequence.Insert(0.25f, DOTween.To(() => this.blackGG.alpha, delegate(float x)
		{
			this.blackGG.alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	private void playerAcceptedStartCall()
	{
		GameManager.AudioSlinger.KillSound(LookUp.SoundLookUp.vacationRinging);
		GameManager.AudioSlinger.PlaySound(this.acceptCallSFX);
		this.incomingCallLargeAcceptBTN.ClickAction.Event -= this.playerAcceptedStartCall;
		this.incomingCallLargeDeclineBTN.ClickAction.Event -= this.playerDeclinedStartCall;
		this.dismissStartCall();
		computerController.Ins.SetMasterLock(true);
		GameManager.TimeSlinger.FireTimer(0.3f, delegate()
		{
			TutorialVoiceCallBehaviour.Ins.WasPresentedEvents.Event += TutorialStartBehaviour.Ins.Begin;
			TutorialStartBehaviour.Ins.TutoralHasEndedEvents.Event += this.triggerGoodGuyAdam;
			TutorialVoiceCallBehaviour.Ins.PresentMe();
		}, 0);
	}

	private void playerAcceptedProductCall()
	{
		GameManager.AudioSlinger.KillSound(LookUp.SoundLookUp.vacationRinging);
		GameManager.AudioSlinger.PlaySound(this.acceptCallSFX);
		this.incomingCallLargeAcceptBTN.ClickAction.Event -= this.playerAcceptedStartCall;
		this.incomingCallLargeDeclineBTN.ClickAction.Event -= this.playerDeclinedStartCall;
		this.dismissStartCall();
		computerController.Ins.SetMasterLock(true);
		GameManager.TimeSlinger.FireTimer(0.3f, delegate()
		{
			TutorialVoiceCallBehaviour.Ins.WasPresentedEvents.Event += TutorialProductBehaviour.Ins.Begin;
			TutorialProductBehaviour.Ins.TutoralHasEndedEvents.Event += this.endTutorialMode;
			TutorialVoiceCallBehaviour.Ins.PresentMe();
		}, 0);
	}

	private void playerDeclinedStartCall()
	{
		GameManager.AudioSlinger.KillSound(LookUp.SoundLookUp.vacationRinging);
		GameManager.AudioSlinger.PlaySound(this.declineCallSFX);
		EnvironmentManager.PowerBehaviour.PowerOffEvent.Event -= this.powerWentOffWhileProdTutActive;
		SteamSlinger.Ins.PlayerDeclinedStartCall();
		this.incomingCallLargeAcceptBTN.ClickAction.Event -= this.playerAcceptedStartCall;
		this.incomingCallLargeDeclineBTN.ClickAction.Event -= this.playerDeclinedStartCall;
		this.dismissStartCall();
		TutorialStartBehaviour.Ins.ShowNoirIcon();
		DeadOrNotBehaviour.Ins.ClearOut();
		this.endTutorialMode();
	}

	private void playerDeclinedProductCall()
	{
		GameManager.AudioSlinger.KillSound(LookUp.SoundLookUp.vacationRinging);
		GameManager.AudioSlinger.PlaySound(this.declineCallSFX);
		SteamSlinger.Ins.PlayerDeclinedProductCall();
		this.incomingCallLargeAcceptBTN.ClickAction.Event -= this.playerAcceptedProductCall;
		this.incomingCallLargeDeclineBTN.ClickAction.Event -= this.playerDeclinedProductCall;
		this.dismissStartCall();
		TutorialProductBehaviour.Ins.PlayerDeclinedCall();
		TutorialProductBehaviour.Ins.AddRemoteVPNSpawn();
		this.endTutorialMode();
	}

	private void endTutorialMode()
	{
		EnvironmentManager.PowerBehaviour.PowerOffEvent.Event -= this.powerWentOffWhileProdTutActive;
		TutorialVoiceCallBehaviour.Ins.DismissMe();
		computerController.Ins.SetMasterLock(false);
		this.myData.IconsShown = true;
		DataManager.Save<TutorialData>(this.myData);
		this.blackGG.alpha = 0f;
		this.blackGG.interactable = false;
		this.blackGG.blocksRaycasts = false;
		GameManager.TimeSlinger.FireTimer(0.5f, delegate()
		{
			this.tutorialHolder.SetActive(false);
		}, 0);
	}

	private void triggerGoodGuyAdam()
	{
		TutorialStartBehaviour.Ins.TutoralHasEndedEvents.Event -= this.triggerGoodGuyAdam;
		SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.GOOD_GUY_ADAM);
		this.endTutorialMode();
	}

	private void powerWentOffWhileProdTutActive()
	{
		EnvironmentManager.PowerBehaviour.PowerOffEvent.Event -= this.powerWentOffWhileProdTutActive;
		this.playerDeclinedProductCall();
		TutorialVoiceCallBehaviour.Ins.ForceEndCall();
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		startController.Ins.TriggerTutorialEvents.Event += this.presentStartCall;
		this.myData = DataManager.Load<TutorialData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new TutorialData(this.myID);
			this.myData.IconsShown = false;
		}
		if (this.myData.IconsShown)
		{
			TutorialStartBehaviour.Ins.ShowNoirIcon();
		}
	}

	private void Awake()
	{
		TutorialManager.Ins = this;
		this.incomingCallLargeAcceptBTN.SetLock(true);
		this.incomingCallLargeDeclineBTN.SetLock(true);
		this.incomingCallLargeAcceptBTN.ClickAction.Event += this.playerAcceptedStartCall;
		this.incomingCallLargeDeclineBTN.ClickAction.Event += this.playerDeclinedStartCall;
		GameManager.StageManager.Stage += this.stageMe;
	}

	public static TutorialManager Ins;

	[SerializeField]
	private GameObject tutorialHolder;

	[SerializeField]
	private CanvasGroup blackGG;

	[SerializeField]
	private RectTransform incomingCallLargeRT;

	[SerializeField]
	private CanvasGroup incomingCallLargeCG;

	[SerializeField]
	private RectTransform adamLargeRT;

	[SerializeField]
	private TutorialBTN incomingCallLargeAcceptBTN;

	[SerializeField]
	private TutorialBTN incomingCallLargeDeclineBTN;

	[SerializeField]
	private AudioFileDefinition phoneRingSFX;

	[SerializeField]
	private AudioFileDefinition acceptCallSFX;

	[SerializeField]
	private AudioFileDefinition declineCallSFX;

	private Tweener adamScaleTween;

	private int myID = 672;

	private TutorialData myData;
}
