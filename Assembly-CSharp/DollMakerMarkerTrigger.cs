using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
[RequireComponent(typeof(BoxCollider))]
public class DollMakerMarkerTrigger : MonoBehaviour
{
	public int UnitNumber
	{
		get
		{
			return this.unitNumber;
		}
	}

	public void Activate()
	{
		if (!this.lockedOut)
		{
			this.myInteractionHook.ForceLock = false;
		}
	}

	public void DeActivate()
	{
		this.myInteractionHook.ForceLock = true;
	}

	public void LockOut()
	{
		this.lockedOut = true;
	}

	private void placeMarker()
	{
		this.DeActivate();
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPutDown1);
		EnemyManager.DollMakerManager.MarkerWasPlaced(this.unitNumber, this.spawnPOS, this.spawnROT);
	}

	[ContextMenu("Reset Placement")]
	private void resetPlacement()
	{
		this.spawnPOS = base.transform.position;
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myBoxCollider = base.GetComponent<BoxCollider>();
		this.myInteractionHook.LeftClickAction += this.placeMarker;
	}

	private void Start()
	{
		this.myInteractionHook.ForceLock = true;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		if (this.placeMesh != null)
		{
			Gizmos.DrawMesh(this.placeMesh, this.spawnPOS, Quaternion.Euler(this.spawnROT));
		}
	}

	[SerializeField]
	private int unitNumber;

	[SerializeField]
	private Mesh placeMesh;

	[SerializeField]
	private Vector3 spawnPOS;

	[SerializeField]
	private Vector3 spawnROT;

	private InteractionHook myInteractionHook;

	private BoxCollider myBoxCollider;

	private bool lockedOut;
}
