using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeHexInteractObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event NodeHexInteractObject.SetData CounterDirectionMouseEnter;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event NodeHexInteractObject.SetDirection SetNodeHexDirection;

	public void ClearState()
	{
		if (!this.amLocked)
		{
			this.HoverCG.alpha = 0f;
			this.ActiveCG.alpha = 0f;
			this.amSet = false;
		}
	}

	public void CounterDirectionMouseOver()
	{
		this.HoverCG.alpha = 1f;
	}

	public void CounterDirectionMouseOut()
	{
		this.HoverCG.alpha = 0f;
	}

	public void ActivateCounterDirection()
	{
		this.amLocked = true;
		this.ActiveCG.alpha = 1f;
	}

	public void DeActivateCounterDirection()
	{
		this.amLocked = false;
		this.ActiveCG.alpha = 0f;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.Playable && !this.amLocked)
		{
			this.HoverCG.alpha = 1f;
			if (this.CounterDirectionMouseEnter != null)
			{
				this.CounterDirectionMouseEnter(this.Position);
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.Playable && !this.amLocked)
		{
			this.HoverCG.alpha = 0f;
			if (this.CounterDirectionMouseExit != null)
			{
				this.CounterDirectionMouseExit(this.Position);
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.Playable && !this.amLocked)
		{
			if (this.amSet)
			{
				if (this.SetNodeHexDirection != null && this.SetNodeHexDirection(MATRIX_STACK_CLOCK_POSITION.NEUTRAL))
				{
					this.HoverCG.alpha = 0f;
					this.ActiveCG.alpha = 0f;
					this.amSet = false;
				}
			}
			else if (this.SetNodeHexDirection != null && this.SetNodeHexDirection(this.Position))
			{
				this.HoverCG.alpha = 0f;
				this.ActiveCG.alpha = 1f;
				this.amSet = true;
			}
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event NodeHexInteractObject.SetData CounterDirectionMouseExit;

	public bool Playable;

	public MATRIX_STACK_CLOCK_POSITION Position;

	public CanvasGroup HoverCG;

	public CanvasGroup ActiveCG;

	private bool amSet;

	private bool amLocked;

	public delegate void SetData(MATRIX_STACK_CLOCK_POSITION SetPOS);

	public delegate bool SetDirection(MATRIX_STACK_CLOCK_POSITION SetPOS);
}
