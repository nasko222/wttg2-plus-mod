using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LocationVolume : MonoBehaviour
{
	public PLAYER_LOCATION Location
	{
		get
		{
			return this.myLocation;
		}
	}

	private void UpdateBounds()
	{
		if (this.boxCollider == null)
		{
			this.boxCollider = base.GetComponent<BoxCollider>();
		}
		this.bounds = new Bounds(base.transform.TransformPoint(this.boxCollider.center), this.boxCollider.size);
	}

	private void OnUpdate()
	{
		this.InBounds();
	}

	private void InBounds()
	{
		if (this.mainCamera != null)
		{
			Vector3 position = this.mainCamera.transform.position;
			if (this.bounds.Contains(position))
			{
				if (!this.PlayerInside)
				{
					this.PlayerInside = true;
					GameManager.WorldManager.AddActiveLocationVolume(this);
				}
			}
			else if (this.PlayerInside)
			{
				this.PlayerInside = false;
				GameManager.WorldManager.RemoveActiveLocationVolume(this);
			}
		}
		else if (this.PlayerInside)
		{
			this.PlayerInside = false;
			GameManager.WorldManager.RemoveActiveLocationVolume(this);
		}
	}

	private void OnEnable()
	{
		base.InvokeRepeating("OnUpdate", 0f, this.updateDelay);
	}

	private void OnDisable()
	{
		base.CancelInvoke("OnUpdate");
	}

	private void Awake()
	{
		this.mainCamera = Camera.main;
		this.UpdateBounds();
		this.boxCollider.enabled = false;
	}

	private void OnDrawGizmos()
	{
		if (!this.hideGizmo)
		{
			this.UpdateBounds();
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(this.bounds.center, this.bounds.size);
		}
	}

	public bool PlayerInside;

	[SerializeField]
	private PLAYER_LOCATION myLocation;

	[SerializeField]
	private float updateDelay = 0.1f;

	[SerializeField]
	private bool hideGizmo;

	private BoxCollider boxCollider;

	private float distance;

	private Bounds bounds;

	private Camera mainCamera;
}
