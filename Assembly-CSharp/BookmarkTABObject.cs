using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookmarkTABObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void SoftBuild()
	{
		base.GetComponent<RectTransform>().anchoredPosition = BookmarkTABObject.StartPOS;
	}

	public void Build(BookmarkData SetBookMarkData, float SetY)
	{
		this.pageTitleText.text = SetBookMarkData.MyTitle;
		this.myPageTitle = SetBookMarkData.MyTitle;
		this.myPageURL = SetBookMarkData.MyURL;
		this.myCurrentPOS.y = SetY;
		base.GetComponent<RectTransform>().anchoredPosition = this.myCurrentPOS;
		DOTween.To(() => base.GetComponent<RectTransform>().sizeDelta, delegate(Vector2 x)
		{
			base.GetComponent<RectTransform>().sizeDelta = x;
		}, BookmarkTABObject.EndSize, 0.2f).SetEase(Ease.OutQuad);
	}

	public void KillMe()
	{
		DOTween.To(() => base.GetComponent<RectTransform>().sizeDelta, delegate(Vector2 x)
		{
			base.GetComponent<RectTransform>().sizeDelta = x;
		}, BookmarkTABObject.StartSize, 0.2f).SetEase(Ease.OutQuad).OnComplete(delegate
		{
			base.GetComponent<RectTransform>().anchoredPosition = BookmarkTABObject.StartPOS;
		});
	}

	public void RePOSMe(float setY)
	{
		this.myCurrentPOS.y = setY;
		DOTween.To(() => base.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			base.GetComponent<RectTransform>().anchoredPosition = x;
		}, this.myCurrentPOS, 0.2f).SetEase(Ease.Linear);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.CursorManager.PointerCursorState(true);
		base.GetComponent<Image>().color = this.hoverColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
		base.GetComponent<Image>().color = this.defaultColor;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.CursorManager.PointerCursorState(true);
		GameManager.BehaviourManager.AnnBehaviour.ForceLoadURL(this.myPageURL);
	}

	public Text pageTitleText;

	public Color hoverColor;

	public Color defaultColor;

	protected static Vector2 StartPOS = new Vector2(0f, 10f);

	protected static Vector2 StartSize = new Vector2(237f, 1f);

	protected static Vector2 EndSize = new Vector2(237f, 28f);

	private Vector2 myCurrentPOS = new Vector2(0f, 0f);

	private string myPageTitle;

	private string myPageURL;
}
