using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class MicMeterHook : MonoBehaviour
{
	public void PresentMicGroup(float PresentTime = 1f)
	{
		DOTween.To(() => LookUp.PlayerUI.MicGroupTransform.anchoredPosition, delegate(Vector2 x)
		{
			LookUp.PlayerUI.MicGroupTransform.anchoredPosition = x;
		}, new Vector2(0f, 55f), PresentTime).SetEase(Ease.InCubic);
	}

	public void DismissMicGroup(float DismissTime = 1f)
	{
		DOTween.To(() => LookUp.PlayerUI.MicGroupTransform.anchoredPosition, delegate(Vector2 x)
		{
			LookUp.PlayerUI.MicGroupTransform.anchoredPosition = x;
		}, new Vector2(0f, 0f), DismissTime).SetEase(Ease.InCubic);
	}

	private void displayPlayersLoudness(float loudLevel)
	{
		float alpha = 1f - loudLevel;
		LookUp.PlayerUI.MicGreenCG.alpha = alpha;
		LookUp.PlayerUI.MicRedCG.alpha = loudLevel;
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		GameManager.BehaviourManager.PlayerAudioBehaviour.CurrentPlayersLoudLevel.Event += this.displayPlayersLoudness;
	}

	private void Awake()
	{
		MicMeterHook.Ins = this;
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void OnDestroy()
	{
	}

	public static MicMeterHook Ins;
}
