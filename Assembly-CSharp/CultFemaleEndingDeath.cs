using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CultFemaleEndingDeath : MonoBehaviour
{
	public void StageTriggerDeath()
	{
		base.transform.position = new Vector3(-0.03598651f, 0f, 3.227f);
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
	}

	public void TriggerDeath()
	{
		this.myAC.SetTrigger("triggerDeath");
		GameManager.TimeSlinger.FireTimer(0.5f, delegate()
		{
			EndingCameraHook.Ins.transform.SetParent(this.cameraBone);
			Sequence sequence = DOTween.Sequence();
			sequence.Insert(0f, DOTween.To(() => EndingCameraHook.Ins.transform.localPosition, delegate(Vector3 x)
			{
				EndingCameraHook.Ins.transform.localPosition = x;
			}, Vector3.zero, 0.75f).SetEase(Ease.Linear));
			sequence.Play<Sequence>();
		}, 0);
	}

	public void TriggerAdamThanksForPlaying()
	{
		AdamBehaviour.Ins.ThanksForPlaying();
	}

	public void TriggerHeadBackHit()
	{
		EndingCameraHook.Ins.AddHeadHit(0.25f);
		GameManager.AudioSlinger.PlaySound(this.headBackHit);
	}

	public void TriggerKnifeSlash()
	{
		GameManager.AudioSlinger.PlaySound(this.knifeSlash);
		EndingCameraHook.Ins.TriggerKnifeDeath();
		EndingManager.Ins.ShowDeathFadeOut();
	}

	private void Awake()
	{
		CultFemaleEndingDeath.Ins = this;
		this.myAC = base.GetComponent<Animator>();
	}

	public static CultFemaleEndingDeath Ins;

	[SerializeField]
	private Transform cameraBone;

	[SerializeField]
	private AudioFileDefinition headBackHit;

	[SerializeField]
	private AudioFileDefinition knifeSlash;

	private Animator myAC;
}
