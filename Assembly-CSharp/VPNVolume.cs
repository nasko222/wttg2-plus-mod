using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VPNVolume : MonoBehaviour
{
	public bool VPNInRange(RemoteVPNObject TheRemoteVPN)
	{
		if (!this.bounds.Contains(TheRemoteVPN.CurrentLocation))
		{
			return false;
		}
		if (this.currentPlacedRemoteVPNS.Count > 0)
		{
			TheRemoteVPN.EnteredPlacementMode += this.clearRemoteVPNFromList;
			this.currentPlacedRemoteVPNS.Add(TheRemoteVPN);
			return false;
		}
		TheRemoteVPN.EnteredPlacementMode += this.clearRemoteVPNFromList;
		this.currentPlacedRemoteVPNS.Add(TheRemoteVPN);
		TheRemoteVPN.SetCurrency(this.MyCurrency);
		return true;
	}

	public bool VPNRangeCheck(Transform ThePosition, out float TimeValue)
	{
		if (!this.bounds.Contains(ThePosition.position))
		{
			TimeValue = 2500f;
			return false;
		}
		if (this.currentPlacedRemoteVPNS.Count > 0)
		{
			TimeValue = 2500f;
			return false;
		}
		TimeValue = this.MyCurrency.GenerateTime;
		return true;
	}

	private void updateBounds()
	{
		if (this.boxCollider == null)
		{
			this.boxCollider = base.GetComponent<BoxCollider>();
		}
		this.bounds = new Bounds(base.transform.TransformPoint(this.boxCollider.center), this.boxCollider.size);
	}

	private void clearRemoteVPNFromList(RemoteVPNObject TheRemoteVPN)
	{
		TheRemoteVPN.EnteredPlacementMode -= this.clearRemoteVPNFromList;
		this.currentPlacedRemoteVPNS.Remove(TheRemoteVPN);
		if (this.currentPlacedRemoteVPNS.Count == 1)
		{
			this.currentPlacedRemoteVPNS[0].SetCurrency(this.MyCurrency);
		}
	}

	private void Awake()
	{
		this.updateBounds();
		this.boxCollider.enabled = false;
		GameManager.WorldManager.AddVPNVolume(this);
	}

	private void OnDrawGizmos()
	{
		if (!this.hideGizmo)
		{
			this.updateBounds();
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(this.bounds.center, this.bounds.size);
		}
	}

	public VPNCurrencyData MyCurrency;

	[SerializeField]
	private float updateDelay = 0.1f;

	[SerializeField]
	private bool hideGizmo;

	private BoxCollider boxCollider;

	private Bounds bounds;

	private List<RemoteVPNObject> currentPlacedRemoteVPNS = new List<RemoteVPNObject>(5);
}
