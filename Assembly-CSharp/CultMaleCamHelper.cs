using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CultMaleCamHelper : MonoBehaviour
{
	public void StageEndJump()
	{
		base.transform.position = new Vector3(24.448f, 0f, -6.319f);
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
	}

	public void TriggerEndJump()
	{
		this.myCamera.transform.SetParent(this.camHelper);
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, Vector3.zero, 0.25f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(0f, 180f, 0f), 0.25f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Play<Sequence>();
		this.myAC.SetTrigger("triggerJump");
	}

	private void Awake()
	{
		CultMaleCamHelper.Ins = this;
		this.myAC = base.GetComponent<Animator>();
		CameraManager.Get(CAMERA_ID.MAIN, out this.myCamera);
	}

	public static CultMaleCamHelper Ins;

	[SerializeField]
	private Transform camHelper;

	private Camera myCamera;

	private Animator myAC;
}
