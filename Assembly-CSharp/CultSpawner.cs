using System;
using UnityEngine;
using UnityEngine.Events;

public class CultSpawner : MonoBehaviour
{
	public void SpawnBehindPlayer(Transform TargetTransform, float YOffSet = 0f)
	{
		Vector3 position = TargetTransform.position - TargetTransform.forward * 0.85f;
		position.y -= YOffSet;
		base.transform.position = position;
		base.transform.rotation = TargetTransform.rotation;
		this.inMeshCheckTimeStamp = Time.time;
		this.inMeshCheckActive = true;
	}

	public void Spawn(Vector3 TargetPOS, Vector3 TargetROT)
	{
		this.myRenderer.enabled = true;
		base.transform.position = TargetPOS;
		base.transform.rotation = Quaternion.Euler(TargetROT);
		this.SpawnEvents.Invoke();
	}

	public void Place(Vector3 TargetPOS, Vector3 TargetROT)
	{
		this.myRenderer.enabled = true;
		base.transform.position = TargetPOS;
		base.transform.rotation = Quaternion.Euler(TargetROT);
	}

	protected void DeSpawn()
	{
		this.myRenderer.enabled = false;
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.DeSpawnEvents.Invoke();
	}

	protected virtual void Awake()
	{
		this.myRenderer.enabled = false;
	}

	private void Update()
	{
		if (this.inMeshCheckActive && Time.time - this.inMeshCheckTimeStamp >= 0.05f)
		{
			this.inMeshCheckActive = false;
			if (this.inMesh)
			{
				this.InMeshEvents.Execute();
			}
			else
			{
				this.myRenderer.enabled = true;
				this.NotInMeshEvents.Execute();
			}
			this.inMesh = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		this.inMesh = true;
	}

	private void OnTriggerStay(Collider other)
	{
		this.inMesh = true;
	}

	public CustomEvent InMeshEvents = new CustomEvent(2);

	public CustomEvent NotInMeshEvents = new CustomEvent(2);

	[SerializeField]
	private SkinnedMeshRenderer myRenderer;

	[SerializeField]
	private UnityEvent DeSpawnEvents;

	[SerializeField]
	private UnityEvent SpawnEvents;

	private bool inMesh;

	private bool inMeshCheckActive;

	private float inMeshCheckTimeStamp;
}
