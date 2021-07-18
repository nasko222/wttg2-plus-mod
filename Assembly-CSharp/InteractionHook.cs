using System;
using System.Diagnostics;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(BoxCollider))]
public class InteractionHook : MonoBehaviour
{
	public bool ForceLock
	{
		get
		{
			return this.forceLock;
		}
		set
		{
			this.forceLock = value;
			this.myBoxCollider.enabled = !this.forceLock;
		}
	}

	public BoxCollider MyBoxCollider
	{
		get
		{
			return this.myBoxCollider;
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event InteractionHook.ReceiveActions RecvAction;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event InteractionHook.ReciveActionsFloat LeftAxisAction;

	public void Receive()
	{
		if (!this.isReceving)
		{
			if (this.AllowMultiStates)
			{
				for (int i = 0; i < this.MultiStatesActive.Length; i++)
				{
					if (StateManager.PlayerState == this.MultiStatesActive[i])
					{
						this.isReceving = true;
						i = this.MultiStatesActive.Length;
					}
				}
			}
			else if (StateManager.PlayerState == this.StateActive)
			{
				this.isReceving = true;
			}
			if (this.isReceving && this.RequireLocationCheck)
			{
				if (StateManager.PlayerLocation == this.LocationToCheck)
				{
					this.isReceving = true;
				}
				else
				{
					this.isReceving = false;
				}
			}
			if (this.isReceving)
			{
				GameManager.InteractionManager.Rescind.Event += this.rescindCache;
				if (this.RecvAction != null)
				{
					this.RecvAction();
				}
			}
		}
	}

	private void rescind()
	{
		this.isReceving = false;
		GameManager.InteractionManager.Rescind.Event -= this.rescindCache;
		if (this.RecindAction != null)
		{
			this.RecindAction();
		}
		if (this.LeftAxisAction != null)
		{
			this.LeftAxisAction(0f);
		}
	}

	private void Awake()
	{
		this.myBoxCollider = base.GetComponent<BoxCollider>();
		this.rescindCache = new Action(this.rescind);
	}

	private void Update()
	{
		bool flag = true;
		bool flag2 = true;
		if (!this.forceLock)
		{
			if (this.AllowMultiStates)
			{
				bool enabled = false;
				for (int i = 0; i < this.MultiStatesActive.Length; i++)
				{
					if (StateManager.PlayerState == this.MultiStatesActive[i])
					{
						enabled = true;
						i = this.MultiStatesActive.Length;
					}
				}
				this.myBoxCollider.enabled = enabled;
			}
			else
			{
				this.myBoxCollider.enabled = (StateManager.PlayerState == this.StateActive);
			}
			if (this.AllStates)
			{
				this.myBoxCollider.enabled = true;
			}
			if (this.isReceving)
			{
				if (this.RequireLocationCheck && StateManager.PlayerLocation != this.LocationToCheck)
				{
					flag2 = false;
				}
				if (flag2)
				{
					if (this.AllowMultiStates)
					{
						flag2 = false;
						for (int j = 0; j < this.MultiStatesActive.Length; j++)
						{
							if (StateManager.PlayerState == this.MultiStatesActive[j])
							{
								flag2 = true;
								j = this.MultiStatesActive.Length;
							}
						}
					}
					else if (StateManager.PlayerState == this.StateActive)
					{
						flag = false;
					}
					else
					{
						flag2 = false;
					}
				}
				if (this.AllStates)
				{
					flag2 = true;
				}
				if (flag2)
				{
					if (!this.activeCrossHairShown)
					{
						this.activeCrossHairShown = true;
						GameManager.BehaviourManager.CrossHairBehaviour.ShowActiveCrossHair();
					}
					if (this.UseAxis)
					{
						if (this.LeftAxisAction != null)
						{
							this.LeftAxisAction(CrossPlatformInputManager.GetAxis("LeftClickWeighted"));
						}
					}
					else if (CrossPlatformInputManager.GetButtonDown("LeftClick"))
					{
						if (this.LeftClickAction != null)
						{
							this.LeftClickAction();
						}
					}
					else if (CrossPlatformInputManager.GetButtonDown("RightClick"))
					{
						if (this.RequireLocationCheckForRightClick && StateManager.PlayerLocation != this.LocationCheckForRightClick)
						{
							flag2 = false;
						}
						if (flag2 && this.RightClickAction != null)
						{
							this.RightClickAction();
						}
					}
				}
			}
		}
		if (flag && this.activeCrossHairShown)
		{
			this.activeCrossHairShown = false;
			GameManager.BehaviourManager.CrossHairBehaviour.HideActiveCrossHair();
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event InteractionHook.ReceiveActions LeftClickAction;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event InteractionHook.ReceiveActions RightClickAction;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event InteractionHook.ReceiveActions RecindAction;

	public PLAYER_STATE StateActive;

	public bool AllStates;

	public bool RequireLocationCheck;

	public PLAYER_LOCATION LocationToCheck;

	public bool RequireLocationCheckForRightClick;

	public PLAYER_LOCATION LocationCheckForRightClick;

	public bool AllowMultiStates;

	public PLAYER_STATE[] MultiStatesActive = new PLAYER_STATE[0];

	public bool UseAxis;

	private BoxCollider myBoxCollider;

	private bool isReceving;

	private bool activeCrossHairShown;

	private bool forceLock;

	private Action rescindCache;

	public delegate void ReceiveActions();

	public delegate void ReciveActionsFloat(float SetValue);
}
