using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TopMenuIconBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	private void Awake()
	{
		this.showHoverSeq = DOTween.Sequence();
		this.showHoverSeq.Insert(0f, DOTween.To(() => this.smallScale, delegate(Vector3 x)
		{
			this.hoverIMGRT.localScale = x;
		}, this.fullScale, 0.3f).SetEase(Ease.Linear));
		this.showHoverSeq.Insert(0f, DOTween.To(() => 0f, delegate(float x)
		{
			this.hoverIMGCG.alpha = x;
		}, 1f, 0.2f).SetEase(Ease.Linear));
		this.showHoverSeq.Pause<Sequence>();
		this.showHoverSeq.SetAutoKill(false);
		this.hideOverSeq = DOTween.Sequence();
		this.hideOverSeq.Insert(0f, DOTween.To(() => this.fullScale, delegate(Vector3 x)
		{
			this.hoverIMGRT.localScale = x;
		}, this.smallScale, 0.2f).SetEase(Ease.Linear));
		this.hideOverSeq.Insert(0f, DOTween.To(() => 1f, delegate(float x)
		{
			this.hoverIMGCG.alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear));
		this.hideOverSeq.Pause<Sequence>();
		this.hideOverSeq.SetAutoKill(false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		this.showHoverSeq.Restart(true, -1f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.hideOverSeq.Restart(true, -1f);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.clickAction != null)
		{
			this.clickAction.Invoke();
		}
	}

	[SerializeField]
	private RectTransform hoverIMGRT;

	[SerializeField]
	private CanvasGroup hoverIMGCG;

	[SerializeField]
	private UnityEvent clickAction;

	private Vector3 fullScale = Vector3.one;

	private Vector3 smallScale = new Vector3(0.25f, 0.25f, 0.25f);

	private Sequence showHoverSeq;

	private Sequence hideOverSeq;
}
