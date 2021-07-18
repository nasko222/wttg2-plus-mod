using System;
using UnityEngine;
using UnityEngine.Events;

public class CultPlayerDistance : MonoBehaviour
{
	public void StageSpawnData(Definition SetSpawnData)
	{
		this.currentSpawnData = (CultSpawnDefinition)SetSpawnData;
	}

	public void SpawnAction()
	{
		this.spawnActive = true;
	}

	public void DeSpawnAction()
	{
		this.spawnActive = false;
	}

	private void updateDistance()
	{
		Vector3 position = this.mainCamera.transform.position;
		this.distance = (base.transform.position - position).magnitude;
	}

	private void Awake()
	{
		this.mainCamera = Camera.main;
	}

	private void Update()
	{
		if (EnemyManager.State != ENEMY_STATE.LOCKED && this.spawnActive && this.currentSpawnData != null && this.currentSpawnData.CheckDistance)
		{
			this.updateDistance();
			if (this.distance <= this.currentSpawnData.DistanceThreshold)
			{
				this.spawnActive = false;
				if (this.ThresholdCrossedActions != null)
				{
					this.ThresholdCrossedActions.Invoke();
				}
			}
		}
	}

	[SerializeField]
	private UnityEvent ThresholdCrossedActions;

	private Camera mainCamera;

	private float distance;

	private CultSpawnDefinition currentSpawnData;

	private bool spawnActive;
}
