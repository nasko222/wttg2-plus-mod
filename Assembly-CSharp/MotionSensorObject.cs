using System;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class MotionSensorObject : MonoBehaviour
{
	public Vector3 SpawnLocation
	{
		get
		{
			return this.spawnLocation;
		}
	}

	public Vector3 SpawnRotation
	{
		get
		{
			return this.spawnRotation;
		}
	}

	public bool Placed
	{
		get
		{
			return this.amPlaced;
		}
	}

	public PLAYER_LOCATION Location
	{
		get
		{
			return this.lastLocation;
		}
	}

	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MotionSensorObject.motionSensorPlacement EnteredPlacementMode;

	public void SoftBuild()
	{
		this.myMeshRenderer = base.GetComponent<MeshRenderer>();
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.LeftClickAction += this.enterPlacementMode;
		this.myMeshRenderer.enabled = false;
		this.myInteractionHook.ForceLock = true;
	}

	public void PlaceMe(Vector3 SetPosition, Vector3 SetRotation)
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPutDown1);
		UIInventoryManager.HideMotionSensor();
		this.amPlaced = true;
		this.lastLocation = StateManager.PlayerLocation;
		base.transform.position = SetPosition;
		base.transform.rotation = Quaternion.Euler(SetRotation);
		this.myMeshRenderer.enabled = true;
		this.myInteractionHook.ForceLock = false;
		if (this.IWasPlaced != null)
		{
			this.IWasPlaced(this);
		}
	}

	public void SpawnMe(Vector3 SetPosition, Vector3 SetRotation)
	{
		this.amPlaced = false;
		base.transform.position = SetPosition;
		base.transform.rotation = Quaternion.Euler(SetRotation);
		this.spawnLocation = SetPosition;
		this.spawnRotation = SetRotation;
		this.myMeshRenderer.enabled = true;
		this.myInteractionHook.ForceLock = false;
	}

	public void SetPlaceMe(SerTrans WhereTo, PLAYER_LOCATION Location)
	{
		this.amPlaced = true;
		base.transform.position = WhereTo.PositionToVector3;
		base.transform.rotation = Quaternion.Euler(WhereTo.RotationToVector3);
		this.lastLocation = Location;
	}

	private void enterPlacementMode()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPickUp1);
		UIInventoryManager.ShowMotionSensor();
		this.amPlaced = false;
		this.myMeshRenderer.enabled = false;
		this.myInteractionHook.ForceLock = true;
		if (this.EnteredPlacementMode != null)
		{
			this.EnteredPlacementMode(this);
		}
	}

	private void FixedUpdate()
	{
		if (this.amPlaced)
		{
			RaycastHit raycastHit;
			if (Physics.CheckSphere(base.transform.position + base.transform.forward * this.MOTION_SENSOR_SPHERE_CAST_RADIUS, this.MOTION_SENSOR_SPHERE_CAST_RADIUS, this.enemyMask.value))
			{
				if (this.IWasTripped != null)
				{
					this.IWasTripped(this);
				}
			}
			else if (Physics.SphereCast(base.transform.position + base.transform.forward * this.MOTION_SENSOR_SPHERE_CAST_RADIUS, this.MOTION_SENSOR_SPHERE_CAST_RADIUS, base.transform.forward, out raycastHit, this.MOTION_SENSOR_SPHERE_CAST_DISTANCE, this.enemyMask.value) && this.IWasTripped != null)
			{
				this.IWasTripped(this);
			}
		}
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.enterPlacementMode;
	}

	private void OnDrawGizmos()
	{
		if (this.amPlaced)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(base.transform.position + base.transform.forward * this.MOTION_SENSOR_SPHERE_CAST_RADIUS, this.MOTION_SENSOR_SPHERE_CAST_RADIUS);
			Gizmos.DrawLine(base.transform.position + base.transform.forward * this.MOTION_SENSOR_SPHERE_CAST_RADIUS, base.transform.position + base.transform.forward * (this.MOTION_SENSOR_SPHERE_CAST_DISTANCE + this.MOTION_SENSOR_SPHERE_CAST_RADIUS));
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(base.transform.position + base.transform.forward * (this.MOTION_SENSOR_SPHERE_CAST_DISTANCE + this.MOTION_SENSOR_SPHERE_CAST_RADIUS), this.MOTION_SENSOR_SPHERE_CAST_RADIUS);
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MotionSensorObject.motionSensorPlacement IWasPlaced;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MotionSensorObject.motionSensorPlacement IWasTripped;

	[SerializeField]
	private LayerMask enemyMask;

	[SerializeField]
	private float MOTION_SENSOR_SPHERE_CAST_RADIUS = 0.43f;

	[SerializeField]
	private float MOTION_SENSOR_SPHERE_CAST_DISTANCE = 2f;

	private InteractionHook myInteractionHook;

	private MeshRenderer myMeshRenderer;

	private Vector3 spawnLocation = Vector3.zero;

	private Vector3 spawnRotation = Vector3.zero;

	private PLAYER_LOCATION lastLocation;

	private bool amPlaced;

	public delegate void motionSensorPlacement(MotionSensorObject TheMotionSensor);
}
