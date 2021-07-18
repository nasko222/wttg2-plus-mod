using System;
using Colorful;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class MainCameraHook : MonoBehaviour
{
	public void TriggerFlashBlur()
	{
		this.lensDistBlurPost.Distortion = 1.5f;
		this.lensDistBlurPost.CubicDistortion = 1.1f;
		this.lensDistBlurPost.enabled = true;
		this.myARF.enabled = true;
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.lensDistBlurPost.Distortion, delegate(float x)
		{
			this.lensDistBlurPost.Distortion = x;
		}, 0f, 2.5f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.lensDistBlurPost.CubicDistortion, delegate(float x)
		{
			this.lensDistBlurPost.CubicDistortion = x;
		}, 0f, 2.5f).SetEase(Ease.Linear));
		sequence.SetDelay(5f);
		sequence.Play<Sequence>();
	}

	public void TriggerHitManJump()
	{
		this.myARF.reverbPreset = AudioReverbPreset.Livingroom;
		this.myARF.enabled = true;
	}

	public void AddBodyHit()
	{
		this.myARF.enabled = true;
		this.doubleVis.Displace.x = this.doubleVis.Displace.x - 0.4f;
		this.doubleVis.Displace.y = this.doubleVis.Displace.y + 0.4f;
	}

	public void AddHeadHit(float SetAmt = 1f)
	{
		this.myARF.enabled = true;
		this.doubleVis.enabled = true;
		this.doubleVis.Displace.x = this.doubleVis.Displace.x - SetAmt;
		this.doubleVis.Displace.y = this.doubleVis.Displace.y + SetAmt;
	}

	public void BlackOut(float Delay, float Fade)
	{
		LookUp.PlayerUI.BlackScreenCG.alpha = 1f;
		DOTween.To(() => LookUp.PlayerUI.BlackScreenCG.alpha, delegate(float x)
		{
			LookUp.PlayerUI.BlackScreenCG.alpha = x;
		}, 0f, Fade).SetDelay(Delay);
	}

	public void FadeDoubleVis(float Duration, float Value)
	{
		this.doubleVis.enabled = true;
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.doubleVis.Displace.x, delegate(float x)
		{
			this.doubleVis.Displace.x = x;
		}, -Value, Duration).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.doubleVis.Displace.y, delegate(float x)
		{
			this.doubleVis.Displace.y = x;
		}, Value, Duration).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void ClearARF(float ClearTime = 2f)
	{
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			this.myARF.enabled = false;
		});
		sequence.Insert(0f, DOTween.To(() => this.myARF.dryLevel, delegate(float x)
		{
			this.myARF.dryLevel = x;
		}, 0f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.room, delegate(float x)
		{
			this.myARF.room = x;
		}, -10000f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.roomHF, delegate(float x)
		{
			this.myARF.roomHF = x;
		}, -10000f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.roomLF, delegate(float x)
		{
			this.myARF.roomLF = x;
		}, 0f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.decayHFRatio, delegate(float x)
		{
			this.myARF.decayHFRatio = x;
		}, 1f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.reflectionsLevel, delegate(float x)
		{
			this.myARF.reflectionsLevel = x;
		}, -2602f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.reflectionsDelay, delegate(float x)
		{
			this.myARF.reflectionsDelay = x;
		}, 0f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.reverbLevel, delegate(float x)
		{
			this.myARF.reverbLevel = x;
		}, 200f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.reverbDelay, delegate(float x)
		{
			this.myARF.reverbDelay = x;
		}, 0.011f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.hfReference, delegate(float x)
		{
			this.myARF.hfReference = x;
		}, 5000f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.lfReference, delegate(float x)
		{
			this.myARF.lfReference = x;
		}, 250f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.diffusion, delegate(float x)
		{
			this.myARF.diffusion = x;
		}, 0f, ClearTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myARF.density, delegate(float x)
		{
			this.myARF.density = x;
		}, 0f, ClearTime).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void ResetARF()
	{
		this.myARF.dryLevel = 0f;
		this.myARF.room = -1000f;
		this.myARF.roomHF = -151f;
		this.myARF.roomLF = 0f;
		this.myARF.decayTime = 7.56f;
		this.myARF.decayHFRatio = 0.91f;
		this.myARF.reflectionsLevel = -626f;
		this.myARF.reflectionsDelay = 0f;
		this.myARF.reverbLevel = 774f;
		this.myARF.reverbDelay = 0.03f;
		this.myARF.hfReference = 5000f;
		this.myARF.lfReference = 250f;
		this.myARF.diffusion = 100f;
		this.myARF.density = 100f;
	}

	public void ClearDoubleVis(float Time)
	{
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			this.doubleVis.enabled = false;
		});
		sequence.Insert(0f, DOTween.To(() => this.doubleVis.Displace.x, delegate(float x)
		{
			this.doubleVis.Displace.x = x;
		}, 0f, Time).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.doubleVis.Displace.y, delegate(float x)
		{
			this.doubleVis.Displace.y = x;
		}, 0f, Time).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void HardClearARF()
	{
		this.myARF.enabled = false;
	}

	private void Awake()
	{
		MainCameraHook.Ins = this;
		this.myCamera = base.GetComponent<Camera>();
		this.lensDistBlurPost = base.GetComponent<LensDistortionBlur>();
		this.doubleVis = base.GetComponent<DoubleVision>();
		this.myARF = base.GetComponent<AudioReverbFilter>();
		this.lensDistBlurPost.enabled = false;
		this.doubleVis.enabled = false;
		this.myARF.enabled = false;
	}

	public static MainCameraHook Ins;

	private Camera myCamera;

	private LensDistortionBlur lensDistBlurPost;

	private DoubleVision doubleVis;

	private AudioReverbFilter myARF;
}
