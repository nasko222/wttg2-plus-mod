using System;
using Colorful;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class EndingCameraHook : MonoBehaviour
{
	public void WakeUp()
	{
		base.transform.localRotation = Quaternion.Euler(new Vector3(8.7f, 13.73f, -20.29f));
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
		});
		float duration = 18f;
		sequence.Insert(0f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, Vector3.zero, 3f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Insert(0f, DOTween.To(() => this.blurPP.Strength, delegate(float x)
		{
			this.blurPP.Strength = x;
		}, 0f, 5f).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.dryLevel, delegate(float x)
		{
			this.myARF.dryLevel = x;
		}, 0f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.room, delegate(float x)
		{
			this.myARF.room = x;
		}, -10000f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.roomHF, delegate(float x)
		{
			this.myARF.roomHF = x;
		}, -10000f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.roomLF, delegate(float x)
		{
			this.myARF.roomLF = x;
		}, 0f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.decayHFRatio, delegate(float x)
		{
			this.myARF.decayHFRatio = x;
		}, 1f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.reflectionsLevel, delegate(float x)
		{
			this.myARF.reflectionsLevel = x;
		}, -2602f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.reflectionsDelay, delegate(float x)
		{
			this.myARF.reflectionsDelay = x;
		}, 0f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.reverbLevel, delegate(float x)
		{
			this.myARF.reverbLevel = x;
		}, 200f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.reverbDelay, delegate(float x)
		{
			this.myARF.reverbDelay = x;
		}, 0.011f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.hfReference, delegate(float x)
		{
			this.myARF.hfReference = x;
		}, 5000f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.lfReference, delegate(float x)
		{
			this.myARF.lfReference = x;
		}, 250f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.diffusion, delegate(float x)
		{
			this.myARF.diffusion = x;
		}, 0f, duration).SetEase(Ease.Linear));
		sequence.Insert(5f, DOTween.To(() => this.myARF.density, delegate(float x)
		{
			this.myARF.density = x;
		}, 0f, duration).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void AddHeadHit(float SetAmt = 1f)
	{
		this.myARF.enabled = true;
		this.myARF.reverbPreset = AudioReverbPreset.Drugged;
		this.doubleVis.enabled = true;
		this.doubleVis.Displace.x = this.doubleVis.Displace.x - SetAmt;
		this.doubleVis.Displace.y = this.doubleVis.Displace.y + SetAmt;
	}

	public void TriggerKnifeDeath()
	{
		this.doubleVis.enabled = true;
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.doubleVis.Displace.x, delegate(float x)
		{
			this.doubleVis.Displace.x = x;
		}, -50f, 0.75f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.doubleVis.Displace.y, delegate(float x)
		{
			this.doubleVis.Displace.y = x;
		}, 50f, 0.75f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.blurPP.Strength, delegate(float x)
		{
			this.blurPP.Strength = x;
		}, 1.25f, 0.75f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void EnableAFR()
	{
		this.myARF.enabled = true;
		this.myARF.reverbPreset = AudioReverbPreset.Drugged;
	}

	public void ClearAFR()
	{
		this.myARF.enabled = false;
	}

	private void Awake()
	{
		EndingCameraHook.Ins = this;
		this.myARF = base.GetComponent<AudioReverbFilter>();
		this.blurPP = base.GetComponent<DirectionalBlur>();
		this.doubleVis = base.GetComponent<DoubleVision>();
	}

	public static EndingCameraHook Ins;

	private AudioReverbFilter myARF;

	private DirectionalBlur blurPP;

	private DoubleVision doubleVis;
}
