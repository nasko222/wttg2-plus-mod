using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
	public void ManualProcessEndingStep(EndingStepDefinition TheStep)
	{
		if (TheStep.HasAnimationTrigger)
		{
			this.myAdamBehaviour.CallAniTrigger(TheStep.AnimationTriggerName);
		}
		if (TheStep.LookingForPlayerResponse)
		{
			this.playerResponseManager.ProcessPlayerReponse(TheStep);
		}
	}

	public void PlayerChoiceDeath()
	{
		this.tripodCamera.SetActive(true);
		CultFemaleEndingDeath.Ins.StageTriggerDeath();
		CultFemaleEnding.Ins.WalkBehindPlayer();
		GameManager.TimeSlinger.FireTimer(6f, new Action(CultMaleEnding.Ins.WalkBehindPlayer), 0);
		GameManager.TimeSlinger.FireTimer(12.5f, new Action(EndController.Ins.PrepareForDeath), 0);
	}

	public void PlayerChoiceLife()
	{
		CultMaleEnding.Ins.WalkBehindPlayer();
		GameManager.TimeSlinger.FireTimer(5f, new Action(EndController.Ins.PrepareForLife), 0);
		GameManager.AudioSlinger.PlaySoundWithCustomDelay(this.endingToture, 12f);
		GameManager.TimeSlinger.FireTimer(25f, new Action(this.ShowLifeFadeOut), 0);
	}

	public void ShowDeathFadeOut()
	{
		DOTween.To(() => this.blackScreenCG.alpha, delegate(float x)
		{
			this.blackScreenCG.alpha = x;
		}, 1f, 1.6f).SetEase(Ease.Linear).SetDelay(0.5f);
		GameManager.TimeSlinger.FireTimer(4f, delegate()
		{
			GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.MUSIC, 0f, 3f);
			GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.ENVIRONMENT, 0f, 3f);
		}, 0);
		GameManager.TimeSlinger.FireTimer(9f, delegate()
		{
			EndingCameraHook.Ins.ClearAFR();
			this.logoCG.alpha = 1f;
			GameManager.AudioSlinger.PlaySound(this.logoJump);
		}, 0);
		GameManager.TimeSlinger.FireTimer(22f, delegate()
		{
			this.logoCG.alpha = 0f;
			SceneManager.LoadScene(0);
		}, 0);
	}

	public void ShowLifeFadeOut()
	{
		DOTween.To(() => this.blackScreenCG.alpha, delegate(float x)
		{
			this.blackScreenCG.alpha = x;
		}, 1f, 1.6f).SetEase(Ease.Linear);
		this.doorHub.PlaySoundCustomDelay(this.doorOpenSFX, 2f);
		GameManager.TimeSlinger.FireTimer(15f, delegate()
		{
			EndingCameraHook.Ins.EnableAFR();
		}, 0);
		GameManager.TimeSlinger.FireTimer(20f, delegate()
		{
			GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.MUSIC, 0f, 3f);
			GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.ENVIRONMENT, 0f, 3f);
		}, 0);
		GameManager.TimeSlinger.FireTimer(26f, delegate()
		{
			EndingCameraHook.Ins.ClearAFR();
			this.logoCG.alpha = 1f;
			GameManager.AudioSlinger.PlaySound(this.logoJump);
		}, 0);
		GameManager.TimeSlinger.FireTimer(37f, delegate()
		{
			this.logoCG.alpha = 0f;
			SceneManager.LoadScene(0);
		}, 0);
	}

	private void startEndingTalk()
	{
		this.currentStepIndex = 0;
		this.processEndingStep();
	}

	private void wakeClintUp()
	{
		DataManager.ClearGameData();
		SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.THEONEPERCENT);
		GameManager.AudioSlinger.PlaySound(this.wakeHimUp);
		GameManager.AudioSlinger.PlaySoundWithCustomDelay(this.faceSlap, 1.75f);
		GameManager.TimeSlinger.FireTimer(2.5f, delegate()
		{
			DOTween.To(() => this.blackScreenCG.alpha, delegate(float x)
			{
				this.blackScreenCG.alpha = x;
			}, 0f, 1f).SetEase(Ease.Linear);
			EndingCameraHook.Ins.WakeUp();
		}, 0);
		GameManager.TimeSlinger.FireTimer(5.5f, delegate()
		{
			this.startEndingTalk();
			EndController.Ins.SetMasterLock(false);
		}, 0);
	}

	private void processEndingStep()
	{
		if (this.currentStepIndex < this.endingSteps.Length && this.endingSteps[this.currentStepIndex].HasAnimationTrigger)
		{
			this.myAdamBehaviour.CallAniTrigger(this.endingSteps[this.currentStepIndex].AnimationTriggerName);
		}
	}

	private void Awake()
	{
		EndingManager.Ins = this;
		this.playerResponseManager = base.GetComponent<EndingResponseManager>();
	}

	private void Start()
	{
		PauseManager.LockPause();
		SteamSlinger.Ins.CheckForPro();
		GameManager.TimeSlinger.FireTimer(5f, new Action(this.wakeClintUp), 0);
	}

	public static EndingManager Ins;

	[SerializeField]
	private CanvasGroup blackScreenCG;

	[SerializeField]
	private CanvasGroup logoCG;

	[SerializeField]
	private GameObject tripodCamera;

	[SerializeField]
	private AudioHubObject doorHub;

	[SerializeField]
	private AudioFileDefinition wakeHimUp;

	[SerializeField]
	private AudioFileDefinition faceSlap;

	[SerializeField]
	private AudioFileDefinition logoJump;

	[SerializeField]
	private AudioFileDefinition endingToture;

	[SerializeField]
	private AudioFileDefinition doorOpenSFX;

	[SerializeField]
	private AdamBehaviour myAdamBehaviour;

	[SerializeField]
	private EndingStepDefinition[] endingSteps = new EndingStepDefinition[0];

	private EndingResponseManager playerResponseManager;

	private int currentStepIndex;
}
