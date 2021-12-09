using System;
using DG.Tweening;
using UnityEngine;

public class BombMakerPresenceGunJump : MonoBehaviour
{
	public void ArmAppear()
	{
		this.ArmBM.transform.DOLocalMoveX(-0.5f, 0.3f, false);
		this.ArmBM.transform.DOLocalRotate(new Vector3(0f, -80f, -10f), 0.4f, RotateMode.Fast);
	}

	public void RandGunShake()
	{
		this.ArmBM.transform.DOLocalRotate(new Vector3(18.825f, -113.458f, -55.939f), 1f, RotateMode.Fast).OnComplete(delegate
		{
			this.GunShakeBack();
		});
	}

	private void GunShakeBack()
	{
		this.ArmBM.transform.DOLocalRotate(new Vector3(0f, -80f, -10f), 1f, RotateMode.Fast);
	}

	public GameObject ArmBM;

	public AudioHubObject hub;
}
