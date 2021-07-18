using System;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
	public void LockInteraction()
	{
		this.masterLock = true;
		this.lockInteraction = true;
	}

	public void UnLockInteraction()
	{
		this.masterLock = false;
		this.lockInteraction = false;
	}

	private void PlayerHitPause()
	{
		this.lockInteraction = true;
	}

	private void PlayerHitUnPause()
	{
		if (!this.masterLock)
		{
			this.lockInteraction = false;
		}
	}

	private void Awake()
	{
		GameManager.InteractionManager = this;
		GameManager.PauseManager.GamePaused += this.PlayerHitPause;
		GameManager.PauseManager.GameUnPaused += this.PlayerHitUnPause;
	}

	private void Start()
	{
		CameraManager.Get(this.CameraToUse, out this.myCamera);
	}

	private void FixedUpdate()
	{
		if (!this.lockInteraction)
		{
			if (Physics.Raycast(this.myCamera.ViewportPointToRay(this.aimFrom), out this.interactionHit, this.Distance, this.Layer.value))
			{
				this.interactionHit.collider.SendMessageUpwards("Receive");
			}
			else
			{
				this.Rescind.Execute();
			}
		}
		else
		{
			this.Rescind.Execute();
		}
	}

	private void OnDestroy()
	{
		GameManager.PauseManager.GamePaused -= this.PlayerHitPause;
		GameManager.PauseManager.GameUnPaused -= this.PlayerHitUnPause;
	}

	public CAMERA_ID CameraToUse;

	public LayerMask Layer;

	public float Distance;

	public CustomEvent Rescind = new CustomEvent(5);

	private bool masterLock;

	private bool lockInteraction;

	private RaycastHit interactionHit;

	private Camera myCamera;

	private Vector3 aimFrom = new Vector3(0.5f, 0.5f, 0.5f);
}
