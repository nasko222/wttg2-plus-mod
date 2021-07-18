using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AdamBehaviour : MonoBehaviour
{
	public void CallAniTrigger(string SetTrigger)
	{
		this.myAC.SetTrigger(SetTrigger);
	}

	public void ThanksForPlaying()
	{
		GameManager.AudioSlinger.PlaySound(this.thanksSFX);
	}

	public void LetHerGo()
	{
		GameManager.AudioSlinger.PlaySound(this.letHerGo);
	}

	public void ProcessEndingPrompt(EndingPromptDefinition ThePrompt)
	{
		if (ThePrompt.HasAnimationAudio)
		{
			GameManager.AudioSlinger.PlaySound(ThePrompt.AnimationAudioFile);
		}
	}

	public void ProcessEndingStep(EndingStepDefinition TheStep)
	{
		if (EndingManager.Ins != null)
		{
			EndingManager.Ins.ManualProcessEndingStep(TheStep);
		}
	}

	public void PlayerChoiceLife()
	{
		EndingManager.Ins.PlayerChoiceLife();
	}

	public void PlayerChoiceDeath()
	{
		EndingManager.Ins.PlayerChoiceDeath();
	}

	private void Awake()
	{
		AdamBehaviour.Ins = this;
		this.myAC = base.GetComponent<Animator>();
	}

	public static AdamBehaviour Ins;

	[SerializeField]
	private AudioFileDefinition thanksSFX;

	[SerializeField]
	private AudioFileDefinition letHerGo;

	private Animator myAC;
}
