using System;
using UnityEngine;

public class CultSeeAndHideSpawn : CultSpawner
{
	public void StageSpawn(Definition SetSpawnData)
	{
		this.currentSpawnData = (CultSpawnDefinition)SetSpawnData;
		CultLooker.Ins.TargetLocation = this.currentSpawnData.Position;
		this.stageEvents.Invoke(SetSpawnData);
		this.stageSpawn();
	}

	private void stageSpawn()
	{
		GameManager.TimeSlinger.KillTimer(this.reRollStageSpawn);
		if (CultLooker.Ins.IsTargetVisible(this.currentSpawnData.Position))
		{
			CultLooker.Ins.NotVisibleActions.Event += this.triggerSpawn;
			CultLooker.Ins.CheckForNotVisible = true;
		}
		else
		{
			this.triggerSpawn();
		}
	}

	private void triggerSpawn()
	{
		CultLooker.Ins.NotVisibleActions.Event -= this.triggerSpawn;
		float magnitude = (this.currentSpawnData.Position - this.mainCamera.transform.position).magnitude;
		if (magnitude >= 4f)
		{
			if (this.currentSpawnData.RotateSpawnTowardsPlayer)
			{
				Vector3 forward = roamController.Ins.transform.position - this.currentSpawnData.Position;
				Vector3 eulerAngles = Quaternion.LookRotation(forward).eulerAngles;
				eulerAngles.x = 0f;
				eulerAngles.z = 0f;
				base.Spawn(this.currentSpawnData.Position, eulerAngles);
			}
			else
			{
				base.Spawn(this.currentSpawnData.Position, this.currentSpawnData.Rotation);
			}
			GameManager.TimeSlinger.FireHardTimer(out this.autoDespawnTimer, 60f, new Action(base.DeSpawn), 0);
			CultLooker.Ins.VisibleActions.Event -= this.stageDeSpawn;
			CultLooker.Ins.VisibleActions.Event += this.stageDeSpawn;
			CultLooker.Ins.CheckForVisible = true;
		}
		else
		{
			GameManager.TimeSlinger.FireHardTimer(out this.reRollStageSpawn, 0.5f, new Action(this.stageSpawn), 0);
		}
	}

	private void stageDeSpawn()
	{
		GameManager.TimeSlinger.KillTimer(this.autoDespawnTimer);
		CultLooker.Ins.VisibleActions.Event -= this.stageDeSpawn;
		CultLooker.Ins.NotVisibleActions.Event += this.triggerDeSpawn;
		CultLooker.Ins.CheckForNotVisible = true;
	}

	private void triggerDeSpawn()
	{
		CultLooker.Ins.NotVisibleActions.Event -= this.triggerDeSpawn;
		if (this.currentSpawnData.HasLookAwayTime)
		{
			CultLooker.Ins.VisibleActions.Event -= this.resetLookAway;
			CultLooker.Ins.VisibleActions.Event += this.resetLookAway;
			CultLooker.Ins.CheckForVisible = true;
			this.lookAwayTimeStamp = Time.time;
			this.lookAwayTimeActive = true;
		}
		else
		{
			base.DeSpawn();
		}
	}

	private void resetLookAway()
	{
		this.lookAwayTimeActive = false;
		CultLooker.Ins.VisibleActions.Event -= this.resetLookAway;
		CultLooker.Ins.NotVisibleActions.Event += this.triggerDeSpawn;
		CultLooker.Ins.CheckForNotVisible = true;
	}

	private void balcDoorFix()
	{
		Debug.Log("This line should never appear in the debug log. Balcony door glitch was fixed. If you see this, then there is something seriously wrong with the mod. Please report this bug immediately.");
	}

	protected new void Awake()
	{
		base.Awake();
		CameraManager.Get(CAMERA_ID.MAIN, out this.mainCamera);
	}

	private void Update()
	{
		if (this.lookAwayTimeActive && Time.time - this.lookAwayTimeStamp >= this.currentSpawnData.LookAwayTime)
		{
			this.lookAwayTimeActive = false;
			CultLooker.Ins.CheckForVisible = false;
			CultLooker.Ins.VisibleActions.Event -= this.resetLookAway;
			base.DeSpawn();
		}
	}

	[SerializeField]
	private DefinitionUnityEvent stageEvents;

	private CultSpawnDefinition currentSpawnData;

	private bool lookAwayTimeActive;

	private float lookAwayTimeStamp;

	private Camera mainCamera;

	private Timer autoDespawnTimer;

	private Timer reRollStageSpawn;
}
