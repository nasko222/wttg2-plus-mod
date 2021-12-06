using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UIManager
{
	public static void FadeScreen(float ToValue, float Duration)
	{
		DOTween.To(() => LookUp.PlayerUI.BlackScreenCG.alpha, delegate(float x)
		{
			LookUp.PlayerUI.BlackScreenCG.alpha = x;
		}, ToValue, Duration).SetEase(Ease.Linear);
	}

	public static void TriggerGameOver(string Reason)
	{
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.UNIVERSAL, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.PLAYER, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.MUSIC, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.COMPUTER_SFX, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.HACKING_SFX, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.ENVIRONMENT, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.OUTSIDE, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.DEAD_DROP, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.ENEMY, 0f, 0.5f);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.GameOver);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.GameOverHit);
		LookUp.PlayerUI.GameOverReasonText.SetText(Reason);
		UIManager.FadeScreen(1f, 0.5f);
		DOTween.To(() => LookUp.PlayerUI.GameOverGC.alpha, delegate(float x)
		{
			LookUp.PlayerUI.GameOverGC.alpha = x;
		}, 1f, 1f).SetEase(Ease.Linear).SetDelay(0.75f);
		DOTween.To(() => LookUp.PlayerUI.GameOverReasonCG.alpha, delegate(float x)
		{
			LookUp.PlayerUI.GameOverReasonCG.alpha = x;
		}, 1f, 1f).SetEase(Ease.Linear).SetDelay(2.4f);
		GameManager.TimeSlinger.FireTimer(6f, delegate()
		{
			SceneManager.LoadScene(1);
		}, 0);
	}

	public static void TriggerHardGameOver(string Reason)
	{
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.UNIVERSAL, 0f, 0f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.PLAYER, 0f, 0f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.MUSIC, 0f, 0f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.COMPUTER_SFX, 0f, 0f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.HACKING_SFX, 0f, 0f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.ENVIRONMENT, 0f, 0f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.OUTSIDE, 0f, 0f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.DEAD_DROP, 0f, 0f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.ENEMY, 0f, 0f);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.GameOver);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.GameOverHit);
		LookUp.PlayerUI.GameOverReasonText.SetText(Reason);
		LookUp.PlayerUI.BlackScreenCG.alpha = 1f;
		DOTween.To(() => LookUp.PlayerUI.GameOverGC.alpha, delegate(float x)
		{
			LookUp.PlayerUI.GameOverGC.alpha = x;
		}, 1f, 1f).SetEase(Ease.Linear).SetDelay(0.25f);
		DOTween.To(() => LookUp.PlayerUI.GameOverReasonCG.alpha, delegate(float x)
		{
			LookUp.PlayerUI.GameOverReasonCG.alpha = x;
		}, 1f, 1f).SetEase(Ease.Linear).SetDelay(1.9f);
		GameManager.TimeSlinger.FireTimer(6f, delegate()
		{
			SceneManager.LoadScene(0);
		}, 0);
	}

	public static void TriggerLoadEnding()
	{
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.UNIVERSAL, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.PLAYER, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.MUSIC, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.COMPUTER_SFX, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.HACKING_SFX, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.ENVIRONMENT, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.OUTSIDE, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.DEAD_DROP, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioLayer(AUDIO_LAYER.ENEMY, 0f, 0.5f);
		UIManager.FadeScreen(1f, 0.5f);
		MainCameraHook.Ins.ClearARF(4f);
		GameManager.TimeSlinger.FireTimer(5f, delegate()
		{
			if (DataManager.LeetMode || ModsManager.Nightmare)
			{
				SceneManager.LoadScene(7);
				return;
			}
			SceneManager.LoadScene(5);
		}, 0);
	}

	public static void FlashPlayer()
	{
		LookUp.PlayerUI.FlashScreenCG.alpha = 1f;
		DOTween.To(() => LookUp.PlayerUI.FlashScreenCG.alpha, delegate(float x)
		{
			LookUp.PlayerUI.FlashScreenCG.alpha = x;
		}, 0f, 1.2f).SetEase(Ease.Linear).SetDelay(3.5f);
	}

	public static void ShowDebug(string text)
	{
		LookUp.PlayerUI.GameOverReasonText.SetText(text);
		LookUp.PlayerUI.GameOverReasonText.alignment = TextAlignmentOptions.BottomLeft;
		LookUp.PlayerUI.GameOverReasonCG.alpha = 1f;
		LookUp.PlayerUI.GameOverReasonCG.transform.position = new Vector3((float)Screen.currentResolution.width / 2f, 20f, 0f);
	}
}
