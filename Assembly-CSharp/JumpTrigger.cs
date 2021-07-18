using System;
using System.Diagnostics;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event JumpTrigger.JumpActions JumpAction;

	public void Activate()
	{
		this.activated = true;
	}

	public void Trigger()
	{
		if (this.activated && this.JumpAction != null)
		{
			this.JumpAction();
		}
	}

	private bool activated;

	public delegate void JumpActions();
}
