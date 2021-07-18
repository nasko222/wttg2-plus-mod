using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ShowHideHoverScrub : MonoBehaviour
{
	public void ShowMe()
	{
		this.myCG.alpha = 1f;
	}

	public void HideMe()
	{
		this.myCG.alpha = 0f;
	}

	private void Awake()
	{
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myCG.alpha = 0f;
	}

	private CanvasGroup myCG;
}
