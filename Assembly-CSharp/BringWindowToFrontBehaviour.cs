using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

public class BringWindowToFrontBehaviour : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event BringWindowToFrontBehaviour.TapFuctions OnTap;

	public void forceTap()
	{
		Transform child = this.parentTrans.GetChild(this.parentTrans.childCount - 1);
		child.SetSiblingIndex(0);
		base.transform.SetSiblingIndex(this.parentTrans.childCount);
		if (this.OnTap != null)
		{
			this.OnTap();
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Transform child = this.parentTrans.GetChild(this.parentTrans.childCount - 1);
		child.SetSiblingIndex(0);
		base.transform.SetSiblingIndex(this.parentTrans.childCount);
		if (this.OnTap != null)
		{
			this.OnTap();
		}
	}

	public Transform parentTrans;

	public delegate void TapFuctions();
}
