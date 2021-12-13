using System;
using DG.Tweening;
using UnityEngine;

public class BombMakerYoureUseless : MonoBehaviour
{
	public void StagePCKill()
	{
		this.BMNeck.transform.DOLocalRotate(new Vector3(-8.264f, 5.885f, -28.896f), 0.4f, RotateMode.Fast);
		this.BMShoulder.transform.DOLocalRotate(new Vector3(-164.6f, -245.8f, 184.5f), 0.8f, RotateMode.Fast).OnComplete(delegate
		{
			this.StageYouAreUseless();
		});
	}

	public void StageYouAreUseless()
	{
		this.AnimStage1();
		this.BMMouth.SetActive(true);
		this.MouthTalkHub.PlaySound(CustomSoundLookUp.youreuseless);
	}

	public void StageGunRecoil()
	{
		this.Bullet.SetActive(true);
		this.BMShoulder.transform.DOLocalMoveY(0.03f, 0.1f, false).OnComplete(delegate
		{
			HitmanBehaviour.Ins.GunFlashBombMaker();
			this.gunRecoilBack();
		});
	}

	private void gunRecoilBack()
	{
		this.BMShoulder.transform.DOLocalMoveY(0f, 0.1f, false).OnComplete(delegate
		{
			this.Bullet.SetActive(false);
		});
	}

	private void AnimStage1()
	{
		this.BMMouth.transform.DOScaleX(0.01f, 0.5f).OnComplete(delegate
		{
			this.AnimStage2();
		});
	}

	private void AnimStage2()
	{
		this.BMMouth.transform.DOScaleX(-0.01f, 0.5f).OnComplete(delegate
		{
			this.AnimStage3();
		});
	}

	private void AnimStage3()
	{
		this.BMMouth.transform.DOScaleX(0.01f, 0.2f).OnComplete(delegate
		{
			this.AnimStage4();
		});
	}

	private void AnimStage4()
	{
		this.BMMouth.transform.DOScaleX(-0.01f, 0.2f).OnComplete(delegate
		{
			this.AnimStageFinish();
		});
	}

	private void AnimStageFinish()
	{
		this.BMMouth.transform.DOScaleX(0f, 0.3f).OnComplete(delegate
		{
			this.BMMouth.SetActive(false);
			GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.StageGunRecoil), 0);
		});
	}

	public GameObject BMMouth;

	public GameObject BMNeck;

	public GameObject BMShoulder;

	public AudioHubObject MouthTalkHub;

	public GameObject Bullet;
}
