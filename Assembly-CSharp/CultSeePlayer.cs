using System;
using UnityEngine;

public class CultSeePlayer : MonoBehaviour
{
	public void StageSpawnData(Definition SetSpawnData)
	{
		this.spawnData = (CultSpawnDefinition)SetSpawnData;
	}

	public void SpawnAction()
	{
		if (this.spawnData != null && this.spawnData.SeePlayer)
		{
			this.lookForPlayer = true;
		}
	}

	public void DeSpawnAction()
	{
		this.lookForPlayer = false;
	}

	private void Awake()
	{
	}

	private void FixedUpdate()
	{
		if (this.lookForPlayer)
		{
			if (Physics.CheckSphere(this.firePosition.position, this.sphereRadius, this.visibleLayers))
			{
				if (this.playerVisibleSet != null)
				{
					this.playerVisibleSet.Invoke(true);
				}
			}
			else if (Physics.SphereCast(this.firePosition.position, this.sphereRadius, base.transform.forward, out this.ray, this.maxDistance, this.visibleLayers))
			{
				if (this.playerVisibleSet != null)
				{
					this.playerVisibleSet.Invoke(true);
				}
			}
			else if (this.playerVisibleSet != null)
			{
				this.playerVisibleSet.Invoke(false);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (this.firePosition != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.firePosition.position, this.sphereRadius);
			Gizmos.DrawLine(this.firePosition.position, this.firePosition.position + base.transform.forward * this.maxDistance);
			Gizmos.DrawWireSphere(this.firePosition.position + base.transform.forward * this.maxDistance, this.sphereRadius);
		}
	}

	[SerializeField]
	private Transform firePosition;

	[SerializeField]
	private float sphereRadius = 0.5f;

	[SerializeField]
	private float maxDistance = 5f;

	[SerializeField]
	private LayerMask visibleLayers;

	[SerializeField]
	private BoolUnityEvent playerVisibleSet;

	private bool lookForPlayer;

	private RaycastHit ray;

	private CultSpawnDefinition spawnData;
}
