using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

public class EndingResponseObject : MonoBehaviour
{
	public void SoftBuild()
	{
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myRT = base.GetComponent<RectTransform>();
		this.myCG.alpha = 0f;
		this.myRT.anchoredPosition = new Vector2(0f, -50f);
	}

	public void Build(EndingResponseDefinition TheResponse, int SetIndex, float SetY, float SetDisplayDelay)
	{
		this.myRT.anchoredPosition = new Vector2(0f, SetY);
		this.responseText.SetText((SetIndex + 1).ToString() + ". " + TheResponse.ResponseOption);
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 1f).SetEase(Ease.Linear).SetDelay(SetDisplayDelay);
	}

	public void Dismiss(float setDelay)
	{
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.5f).SetDelay(setDelay).OnComplete(delegate
		{
			this.myCG.alpha = 0f;
			this.myRT.anchoredPosition = new Vector2(0f, -50f);
		});
	}

	[SerializeField]
	private TextMeshProUGUI responseText;

	private CanvasGroup myCG;

	private RectTransform myRT;
}
