using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class crossHairBehaviour : MonoBehaviour
{
	public void ShowCrossHairGroup()
	{
		this.CrossHairCanvas.enabled = true;
		this.hideCrossHairGroup = false;
	}

	public void HideCrossHairGroup()
	{
		this.CrossHairCanvas.enabled = false;
		this.hideCrossHairGroup = true;
	}

	public void ShowActiveCrossHair()
	{
		this.deActivateCrossHairTrans.Pause<Tweener>();
		this.deActivateCrossHairCG.Pause<Tweener>();
		this.activateCrossHairTrans.Restart(true, -1f);
		this.activateCrossHairCG.Restart(true, -1f);
	}

	public void HideActiveCrossHair()
	{
		this.activateCrossHairTrans.Pause<Tweener>();
		this.activateCrossHairCG.Pause<Tweener>();
		this.deActivateCrossHairTrans.Restart(true, -1f);
		this.deActivateCrossHairCG.Restart(true, -1f);
	}

	private void PlayerHitPause()
	{
		if (!this.hideCrossHairGroup)
		{
			this.CrossHairCanvas.enabled = false;
		}
	}

	private void PlayerHitUnPause()
	{
		if (!this.hideCrossHairGroup)
		{
			this.CrossHairCanvas.enabled = true;
		}
	}

	private void Awake()
	{
		GameManager.BehaviourManager.CrossHairBehaviour = this;
		GameManager.PauseManager.GamePaused += this.PlayerHitPause;
		GameManager.PauseManager.GameUnPaused += this.PlayerHitUnPause;
		this.activateCrossHairTrans = DOTween.To(() => new Vector3(0.5f, 0.5f, 1f), delegate(Vector3 x)
		{
			this.CrossHairTrans.localScale = x;
		}, Vector3.one, 0.15f).SetEase(Ease.Linear);
		this.activateCrossHairTrans.SetAutoKill(false);
		this.activateCrossHairTrans.Pause<Tweener>();
		if (crossHairBehaviour.<>f__mg$cache0 == null)
		{
			crossHairBehaviour.<>f__mg$cache0 = new DOGetter<Vector3>(Vector3.get_one);
		}
		this.deActivateCrossHairTrans = DOTween.To(crossHairBehaviour.<>f__mg$cache0, delegate(Vector3 x)
		{
			this.CrossHairTrans.localScale = x;
		}, new Vector3(0.5f, 0.5f, 1f), 0.15f).SetEase(Ease.Linear);
		this.deActivateCrossHairTrans.SetAutoKill(false);
		this.deActivateCrossHairTrans.Pause<Tweener>();
		this.activateCrossHairCG = DOTween.To(() => 0.25f, delegate(float x)
		{
			this.CrossHairCG.alpha = x;
		}, 0.9f, 0.15f).SetEase(Ease.Linear);
		this.activateCrossHairCG.SetAutoKill(false);
		this.activateCrossHairCG.Pause<Tweener>();
		this.deActivateCrossHairCG = DOTween.To(() => 0.9f, delegate(float x)
		{
			this.CrossHairCG.alpha = x;
		}, 0.25f, 0.15f).SetEase(Ease.Linear);
		this.deActivateCrossHairCG.SetAutoKill(false);
		this.deActivateCrossHairCG.Pause<Tweener>();
	}

	private void OnDestroy()
	{
		GameManager.PauseManager.GamePaused -= this.PlayerHitPause;
		GameManager.PauseManager.GameUnPaused -= this.PlayerHitUnPause;
	}

	public Canvas CrossHairCanvas;

	public Transform CrossHairTrans;

	public CanvasGroup CrossHairCG;

	private bool hideCrossHairGroup;

	private Tweener activateCrossHairTrans;

	private Tweener deActivateCrossHairTrans;

	private Tweener activateCrossHairCG;

	private Tweener deActivateCrossHairCG;

	[CompilerGenerated]
	private static DOGetter<Vector3> <>f__mg$cache0;
}
