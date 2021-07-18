using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class TutorialStepper : MonoBehaviour
{
	protected void StartTutorial()
	{
		this.currentIndex = 0;
		this.step();
	}

	protected void step()
	{
		this.stepAudioPlaying = false;
		this.locked = true;
		if (this.currentIndex < this.steps.Length)
		{
			GameManager.TimeSlinger.FireTimer(2f, delegate()
			{
				this.locked = false;
			}, 0);
			this.currentStep = this.steps[this.currentIndex];
			if (this.currentStep.HasDelay)
			{
				GameManager.TimeSlinger.FireTimer(this.currentStep.DelayAmount, delegate()
				{
					if (this.currentStep.HasAudioFile)
					{
						GameManager.AudioSlinger.PlaySound(this.currentStep.AudioFile);
						GameManager.TimeSlinger.FireHardTimer(out this.currentStepTimer, this.currentStep.AudioFile.AudioClip.length, new Action(this.currentStepTimesUp), 0);
						this.stepAudioPlaying = true;
					}
					if (this.currentStep.HasText)
					{
						TutorialTextHook.Ins.Process(this.currentStep.Text, this.currentStep.AudioFile.AudioClip.length);
					}
					if (this.currentStep.HasGameEvent)
					{
						this.currentStep.GameEvent.Raise();
					}
					this.stepInProgress = true;
					this.currentIndex++;
				}, 0);
			}
			else
			{
				if (this.currentStep.HasAudioFile)
				{
					GameManager.AudioSlinger.PlaySound(this.currentStep.AudioFile);
					GameManager.TimeSlinger.FireHardTimer(out this.currentStepTimer, this.currentStep.AudioFile.AudioClip.length, new Action(this.currentStepTimesUp), 0);
					this.stepAudioPlaying = true;
				}
				if (this.currentStep.HasText)
				{
					TutorialTextHook.Ins.Process(this.currentStep.Text, this.currentStep.AudioFile.AudioClip.length);
				}
				if (this.currentStep.HasGameEvent)
				{
					this.currentStep.GameEvent.Raise();
				}
				this.stepInProgress = true;
				this.currentIndex++;
			}
		}
		else
		{
			this.TutoralHasEndedEvents.Execute();
		}
	}

	protected void HardClear()
	{
		if (this.stepAudioPlaying)
		{
			GameManager.AudioSlinger.KillSound(this.currentStep.AudioFile);
		}
		this.stepAudioPlaying = false;
		this.stepInProgress = false;
		TutorialTextHook.Ins.Clear();
	}

	private void currentStepTimesUp()
	{
		this.stepAudioPlaying = false;
	}

	private void Update()
	{
		if (!this.locked && this.stepInProgress && this.currentStep.ClickToContinue && CrossPlatformInputManager.GetButtonDown("LeftClick"))
		{
			if (this.stepAudioPlaying)
			{
				GameManager.TimeSlinger.KillTimer(this.currentStepTimer);
				GameManager.AudioSlinger.KillSound(this.currentStep.AudioFile);
				this.stepAudioPlaying = false;
				TutorialTextHook.Ins.HardShow();
			}
			else
			{
				this.stepInProgress = false;
				TutorialTextHook.Ins.Clear();
				this.step();
			}
		}
	}

	public CustomEvent TutoralHasEndedEvents = new CustomEvent(5);

	[SerializeField]
	protected TutorialStepDefinition[] steps = new TutorialStepDefinition[0];

	protected int currentIndex;

	private TutorialStepDefinition currentStep;

	private Timer currentStepTimer;

	private bool stepInProgress;

	private bool stepAudioPlaying;

	private bool locked;
}
