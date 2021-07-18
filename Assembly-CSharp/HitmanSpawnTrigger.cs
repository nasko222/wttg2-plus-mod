using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HitmanSpawnTrigger : MonoBehaviour
{
	public void SetActive()
	{
		this.myBoxCollider.enabled = true;
		this.isActive = true;
	}

	public void Deactivate()
	{
		this.myBoxCollider.enabled = false;
		this.isActive = false;
	}

	public void FailSafe()
	{
		if (this.TriggerEvents != null)
		{
			this.TriggerEvents.Invoke(this.spawnData);
		}
	}

	private void Awake()
	{
		this.myBoxCollider = base.GetComponent<BoxCollider>();
		this.myBoxCollider.enabled = false;
		this.isActive = false;
	}

	private void OnTriggerStay(Collider other)
	{
		if (this.isActive)
		{
			this.isActive = false;
			if (this.TriggerEvents != null)
			{
				this.TriggerEvents.Invoke(this.spawnData);
			}
		}
	}

	[SerializeField]
	private HitmanSpawnDefUnityEvent TriggerEvents;

	[SerializeField]
	private HitmanSpawnDefinition spawnData;

	private BoxCollider myBoxCollider;

	private bool isActive;
}
