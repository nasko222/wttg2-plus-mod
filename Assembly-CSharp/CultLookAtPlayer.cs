using System;
using UnityEngine;

public class CultLookAtPlayer : MonoBehaviour
{
	public void StageSpawnData(Definition SetSpawnData)
	{
		this.spawnData = (CultSpawnDefinition)SetSpawnData;
		if (this.spawnData.RotateSpawnTowardsPlayer)
		{
			Vector3 forward = roamController.Ins.transform.position - this.spawnData.Position;
			Vector3 eulerAngles = Quaternion.LookRotation(forward).eulerAngles;
			eulerAngles.x = 0f;
			eulerAngles.z = 0f;
			this.clampMin = eulerAngles.y - this.lookRange;
			this.clampMax = eulerAngles.y + this.lookRange;
		}
		else
		{
			this.clampMin = this.spawnData.Rotation.y - this.lookRange;
			this.clampMax = this.spawnData.Rotation.y + this.lookRange;
		}
	}

	public void SpawnAction()
	{
		if (this.spawnData != null && this.spawnData.RotateSpawnTowardsPlayer)
		{
			Vector3 forward = roamController.Ins.transform.position - this.spawnData.Position;
			Vector3 eulerAngles = Quaternion.LookRotation(forward).eulerAngles;
			eulerAngles.x = 0f;
			eulerAngles.z = 0f;
			this.clampMin = eulerAngles.y - this.lookRange;
			this.clampMax = eulerAngles.y + this.lookRange;
		}
		this.spawnActive = true;
	}

	public void DeSpawnAction()
	{
		this.spawnActive = false;
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.roamControllerTransform = roamController.Ins.transform;
	}

	private void Awake()
	{
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void LateUpdate()
	{
		if (this.spawnActive && this.spawnData != null && this.spawnData.LookAtPlayer)
		{
			Vector3 forward = this.roamControllerTransform.position - base.transform.position;
			this.lookRotation = Quaternion.LookRotation(forward, Vector3.up).eulerAngles;
			Vector3 zero = Vector3.zero;
			zero.x = this.neckBone.transform.rotation.x;
			zero.z = this.neckBone.transform.rotation.z;
			if (this.clampMin <= 0f)
			{
				float y;
				if (this.lookRotation.y >= 360f - Mathf.Abs(this.clampMin) || this.lookRotation.y <= this.clampMax)
				{
					y = this.lookRotation.y;
				}
				else if (this.lookRotation.y > 180f)
				{
					y = 360f - Mathf.Abs(this.clampMin);
				}
				else
				{
					y = this.clampMax;
				}
				zero.y = y;
			}
			else
			{
				zero.y = Mathf.Clamp(this.lookRotation.y, this.clampMin, this.clampMax);
			}
			this.neckBone.transform.rotation = Quaternion.Euler(zero);
		}
	}

	[SerializeField]
	private Transform neckBone;

	[SerializeField]
	private float lookRange;

	private CultSpawnDefinition spawnData;

	private Transform roamControllerTransform;

	private Vector3 lookRotation = Vector3.zero;

	private float clampMin;

	private float clampMax;

	private bool spawnActive;
}
