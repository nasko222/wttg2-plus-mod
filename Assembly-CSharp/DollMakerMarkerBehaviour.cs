using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class DollMakerMarkerBehaviour : MonoBehaviour
{
	public void SpawnMeTo(Vector3 Position, Vector3 Rotation)
	{
		this.myMesh.enabled = true;
		UIInventoryManager.HideDollMakerMarker();
		GameManager.TimeSlinger.FireTimer(0.75f, delegate()
		{
			this.myInteractionHook.ForceLock = false;
		}, 0);
		base.transform.position = Position;
		base.transform.rotation = Quaternion.Euler(Rotation);
		this.myData.IsPlaced = true;
		this.myData.PlacedLocation = Position.ToVect3();
		this.myData.PlaceRotation = Rotation.ToVect3();
		DataManager.Save<DollMakerMarkerData>(this.myData);
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.myData = DataManager.Load<DollMakerMarkerData>(2247);
		if (this.myData == null)
		{
			this.myData = new DollMakerMarkerData(2247);
			this.myData.IsPlaced = false;
			this.myData.PlacedLocation = Vect3.zero;
			this.myData.PlaceRotation = Vect3.zero;
		}
		if (this.myData.IsPlaced)
		{
			this.myMesh.enabled = true;
			this.myInteractionHook.ForceLock = false;
			base.transform.position = this.myData.PlacedLocation.ToVector3;
			base.transform.rotation = Quaternion.Euler(this.myData.PlaceRotation.ToVector3);
		}
	}

	private void wasPickedUp()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPickUp2);
		UIInventoryManager.ShowDollMakerMarker();
		this.myMesh.enabled = false;
		this.myInteractionHook.ForceLock = true;
		base.transform.position = Vector3.zero;
		this.myData.IsPlaced = false;
		DataManager.Save<DollMakerMarkerData>(this.myData);
		this.MarkerWasPickedUp.Execute();
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myMesh.enabled = false;
		this.myInteractionHook.LeftClickAction += this.wasPickedUp;
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.wasPickedUp;
	}

	[SerializeField]
	private MeshRenderer myMesh;

	public CustomEvent MarkerWasPickedUp = new CustomEvent(2);

	private InteractionHook myInteractionHook;

	private DollMakerMarkerData myData;
}
