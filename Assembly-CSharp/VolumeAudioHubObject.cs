using System;
using UnityEngine;

[RequireComponent(typeof(AudioHubObject))]
[RequireComponent(typeof(BoxCollider))]
public class VolumeAudioHubObject : MonoBehaviour
{
	private void UpdateBounds()
	{
		if (this.boxCollider == null)
		{
			this.boxCollider = base.GetComponent<BoxCollider>();
		}
		this.bounds = new Bounds(base.transform.TransformPoint(this.boxCollider.center), this.boxCollider.size);
		if (this.fadeDistance <= 0f)
		{
			this.fadeDistance = 0f;
		}
		this.fadeBounds = this.bounds;
		this.fadeBounds.Expand(this.fadeDistance);
	}

	private void OnUpdate()
	{
		this.InBounds();
	}

	private void InBounds()
	{
		if (this.mainCamera != null)
		{
			Vector3 position = this.mainCamera.transform.position;
			if (this.bounds.Contains(position))
			{
				this.distance = 0f;
				this.setVolumeAMT = this.maxVolume;
			}
			else
			{
				Vector3 a = this.bounds.ClosestPoint(position);
				this.distance = (a - position).magnitude;
				if (this.distance <= this.fadeDistance)
				{
					this.setVolumeAMT = Mathf.Min((this.fadeDistance - this.distance) / this.fadeDistance, this.maxVolume);
				}
				else
				{
					this.setVolumeAMT = this.minVolume;
				}
			}
			this.myAudioHubObject.MuffleHub(this.setVolumeAMT, 0f);
		}
		else
		{
			this.setVolumeAMT = this.minVolume;
		}
	}

	private void OnEnable()
	{
		base.InvokeRepeating("OnUpdate", 0f, this.updateDelay);
	}

	private void OnDisable()
	{
		base.CancelInvoke("OnUpdate");
	}

	private void Awake()
	{
		this.mainCamera = Camera.main;
		this.myAudioHubObject = base.GetComponent<AudioHubObject>();
		this.UpdateBounds();
		this.boxCollider.enabled = false;
	}

	private void OnDrawGizmos()
	{
		this.UpdateBounds();
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(this.bounds.center, this.bounds.size);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(this.fadeBounds.center, this.fadeBounds.size);
	}

	[SerializeField]
	private float updateDelay = 0.1f;

	[SerializeField]
	private float fadeDistance = 1f;

	[SerializeField]
	[Range(0f, 1f)]
	private float minVolume;

	[SerializeField]
	[Range(0f, 1f)]
	private float maxVolume = 1f;

	private AudioHubObject myAudioHubObject;

	private BoxCollider boxCollider;

	private Bounds bounds;

	private Bounds fadeBounds;

	private Camera mainCamera;

	private float setVolumeAMT;

	private float distance;
}
