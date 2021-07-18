using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioHubObject))]
public class ShowerCurtianTrigger : MonoBehaviour
{
	public void TriggerOpen()
	{
		this.myAudioHub.PlaySound(this.OpenSFX);
		this.myAC.SetTrigger("triggerOpen");
		this.UpdatePeakValue(0f);
	}

	public void TriggerClose()
	{
		this.myAudioHub.PlaySound(this.CloseSFX);
		this.myAC.SetTrigger("triggerClose");
	}

	public void UpdatePeakValue(float SetValue)
	{
		this.myAC.SetFloat("Peak", SetValue);
	}

	public void AnimationCompleted(SHOWER_CURTIAN_STATES TheState)
	{
		if (TheState != SHOWER_CURTIAN_STATES.CLOSED)
		{
			if (TheState == SHOWER_CURTIAN_STATES.OPENED)
			{
				if (this.OpenEvents != null)
				{
					this.OpenEvents.Invoke();
				}
			}
		}
		else if (this.CloseEvents != null)
		{
			this.CloseEvents.Invoke();
		}
	}

	private void Awake()
	{
		ShowerCurtianTrigger.Ins = this;
		this.myAudioHub = base.GetComponent<AudioHubObject>();
	}

	public static ShowerCurtianTrigger Ins;

	[SerializeField]
	private Animator myAC;

	[SerializeField]
	private AudioFileDefinition OpenSFX;

	[SerializeField]
	private AudioFileDefinition CloseSFX;

	[SerializeField]
	private UnityEvent OpenEvents;

	[SerializeField]
	private UnityEvent CloseEvents;

	private AudioHubObject myAudioHub;
}
