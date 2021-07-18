using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class EnemyHotZoneTrigger : MonoBehaviour
{
	public void SetActive()
	{
		this.myBoxCollider.enabled = true;
		this.isActive = true;
	}

	public void SetInActive()
	{
		this.myBoxCollider.enabled = false;
		this.isActive = false;
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
			this.triggerActions.Invoke();
		}
	}

	private void OnDestroy()
	{
		this.triggerActions.RemoveAllListeners();
	}

	[SerializeField]
	private UnityEvent triggerActions;

	private BoxCollider myBoxCollider;

	private bool isActive;
}
