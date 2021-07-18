using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class WarmUpNumberObject : MonoBehaviour
{
	public void FireMe(string setNumber)
	{
		GameManager.AudioSlinger.PlaySound(this.MySFX);
		this.myRT.anchoredPosition = this.myStartPOS;
		this.myText.text = setNumber;
		this.myCG.alpha = 1f;
		this.scaleDownTween.Restart(true, -1f);
		this.fadeOutTween.Restart(true, -1f);
	}

	private void Awake()
	{
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myRT = base.GetComponent<RectTransform>();
		this.myText = base.GetComponent<Text>();
		this.myCG.alpha = 0f;
		this.myStartPOS.x = 0f;
		this.myStartPOS.y = -(this.myRT.sizeDelta.y / 2f + 5f);
		this.scaleDownTween = DOTween.To(() => this.myRT.localScale, delegate(Vector3 x)
		{
			this.myRT.localScale = x;
		}, WarmUpNumberObject.myDownScale, 0.6f).SetEase(Ease.InQuint);
		this.scaleDownTween.Pause<Tweener>();
		this.scaleDownTween.SetAutoKill(false);
		this.fadeOutTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.6f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.myCG.alpha = 0f;
			this.myRT.localScale = this.myDefaultScale;
		});
		this.fadeOutTween.Pause<Tweener>();
		this.fadeOutTween.SetAutoKill(false);
	}

	public AudioFileDefinition MySFX;

	private static Vector3 myDownScale = new Vector3(0.1f, 0.1f, 1f);

	private CanvasGroup myCG;

	private RectTransform myRT;

	private Text myText;

	private Vector2 myStartPOS = Vector2.zero;

	private Vector3 myDefaultScale = Vector3.one;

	private Tweener scaleDownTween;

	private Tweener fadeOutTween;
}
