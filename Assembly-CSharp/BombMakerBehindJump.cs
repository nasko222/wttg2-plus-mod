using System;
using DG.Tweening;
using UnityEngine;

public class BombMakerBehindJump : MonoBehaviour
{
	public void ElbowRot()
	{
		this.elbowBM.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.3f, RotateMode.Fast);
	}

	public void gunRecoil()
	{
		this.Bullet.SetActive(true);
		this.elbowBM.transform.DOLocalRotate(new Vector3(5f, 0f, -5f), 0.2f, RotateMode.Fast).OnComplete(delegate
		{
			this.gunRecoilBack();
		});
	}

	private void gunRecoilBack()
	{
		this.elbowBM.transform.DOLocalRotate(new Vector3(-3f, 0f, 3f), 0.2f, RotateMode.Fast).OnComplete(delegate
		{
			this.Bullet.SetActive(false);
		});
	}

	public GameObject elbowBM;

	public GameObject Bullet;
}
