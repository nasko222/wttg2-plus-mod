using System;
using DG.Tweening;
using UnityEngine;

public class BombMakerApartmentJump : MonoBehaviour
{
	public void ShoulderRotate()
	{
		this.shoulderBM.transform.DOLocalRotate(new Vector3(0f, -90f, 0f), 0.3f, RotateMode.Fast);
	}

	public void gunRecoil()
	{
		this.Bullet.SetActive(true);
		this.shoulderBM.transform.DOLocalRotate(new Vector3(0f, -90f, -2f), 0.1f, RotateMode.Fast).OnComplete(delegate
		{
			this.gunRecoilBack();
		});
	}

	private void gunRecoilBack()
	{
		this.shoulderBM.transform.DOLocalRotate(new Vector3(0f, -90f, 1f), 0.1f, RotateMode.Fast).OnComplete(delegate
		{
			this.Bullet.SetActive(false);
		});
	}

	[SerializeField]
	private GameObject shoulderBM;

	public GameObject Bullet;
}
