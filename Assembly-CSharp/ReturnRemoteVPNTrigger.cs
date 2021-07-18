using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class ReturnRemoteVPNTrigger : MonoBehaviour
{
	private void hoverAction()
	{
		this.remoteVPNObject.transform.position = GameManager.ManagerSlinger.RemoteVPNManager.CurrentRemoteVPNSpawnLocation;
		this.remoteVPNObject.transform.rotation = Quaternion.Euler(GameManager.ManagerSlinger.RemoteVPNManager.CurrentRemoteVPNSpawnRotation);
		this.remoteVPNMeshRenderer.enabled = true;
	}

	private void exitAction()
	{
		this.remoteVPNMeshRenderer.enabled = false;
	}

	private void leftClickAction()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPutDown1);
		UIInventoryManager.HideRemoteVPN();
		this.remoteVPNMeshRenderer.enabled = false;
		GameManager.ManagerSlinger.RemoteVPNManager.ReturnRemoteVPN();
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.remoteVPNMeshRenderer = this.remoteVPNObject.GetComponent<MeshRenderer>();
		this.myInteractionHook.RecvAction += this.hoverAction;
		this.myInteractionHook.RecindAction += this.exitAction;
		this.myInteractionHook.LeftClickAction += this.leftClickAction;
	}

	private void Update()
	{
		if (StateManager.PlayerState == PLAYER_STATE.REMOTE_VPN_PLACEMENT)
		{
			this.myInteractionHook.ForceLock = false;
		}
		else
		{
			this.myInteractionHook.ForceLock = true;
		}
	}

	private void OnDestroy()
	{
		this.myInteractionHook.RecvAction -= this.hoverAction;
		this.myInteractionHook.RecindAction -= this.exitAction;
		this.myInteractionHook.LeftClickAction -= this.leftClickAction;
	}

	[SerializeField]
	private GameObject remoteVPNObject;

	private MeshRenderer remoteVPNMeshRenderer;

	private InteractionHook myInteractionHook;
}
