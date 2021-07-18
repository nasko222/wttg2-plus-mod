using System;
using System.Collections.Generic;
using UnityEngine;

public class DistanceLOD : MonoBehaviour
{
	private void Awake()
	{
		this.mainCamera = Camera.main;
		this.UpdateBounds();
	}

	private void OnEnable()
	{
		base.InvokeRepeating("OnUpdate", 0f, this.updateDelay);
	}

	private void OnDisable()
	{
		base.CancelInvoke("OnUpdate");
	}

	private void OnUpdate()
	{
		this.UpdateDistance();
		this.DoEnable(this.cullDistance > this.distance);
	}

	private void DoEnable(bool enable)
	{
		if (this.enableForceEnableHotZones)
		{
			for (int i = 0; i < this.enableHotZones.Length; i++)
			{
				if (this.enableHotZones[i].IsHot)
				{
					enable = true;
					i = this.enableHotZones.Length;
				}
			}
		}
		if (this.OverwriteCulling)
		{
			enable = true;
		}
		for (int j = 0; j < this.renderers.Length; j++)
		{
			if (!(this.renderers[j] == null))
			{
				this.renderers[j].enabled = enable;
			}
		}
		for (int k = 0; k < this.lights.Length; k++)
		{
			if (!(this.lights[k] == null))
			{
				this.lights[k].enabled = enable;
			}
		}
		for (int l = 0; l < this.interactiveLights.Length; l++)
		{
			if (!(this.interactiveLights[l] == null))
			{
				if (!this.interactiveLights[l].SetToOff)
				{
					this.interactiveLights[l].MyLight.enabled = enable;
				}
			}
		}
	}

	private void UpdateDistance()
	{
		if (this.mainCamera == null)
		{
			return;
		}
		Vector3 position = this.mainCamera.transform.position;
		if (this.bounds.Contains(position))
		{
			this.distance = 0f;
		}
		else
		{
			Vector3 a = this.bounds.ClosestPoint(position);
			this.distance = (a - position).magnitude;
		}
	}

	private void UpdateBounds()
	{
		this.bounds = new Bounds(base.transform.TransformPoint(this.center), this.size);
	}

	[ContextMenu("Recalculate Bounds")]
	private void RecalculateBounds()
	{
		if (this.renderers.Length < 1)
		{
			this.bounds = new Bounds(base.transform.position, Vector3.zero);
			return;
		}
		bool flag = true;
		for (int i = 0; i < this.renderers.Length; i++)
		{
			if (!(this.renderers[i] == null))
			{
				if (flag)
				{
					this.bounds = this.renderers[i].bounds;
					flag = false;
				}
				else
				{
					this.bounds.Encapsulate(this.renderers[i].bounds);
				}
			}
		}
		this.center = base.transform.InverseTransformPoint(this.bounds.center);
		this.size = this.bounds.size;
	}

	[ContextMenu("Gather Target Children")]
	private void GatherTargetChildren()
	{
		if (this.gatherChildrenTarget == null)
		{
			this.gatherChildrenTarget = base.gameObject;
		}
		List<Renderer> list = new List<Renderer>(this.renderers);
		list.AddRange(this.gatherChildrenTarget.GetComponentsInChildren<Renderer>());
		this.renderers = list.ToArray();
		List<Light> list2 = new List<Light>(this.lights);
		list2.AddRange(this.gatherChildrenTarget.GetComponentsInChildren<Light>());
		this.lights = list2.ToArray();
		this.renderers = new List<Renderer>(new HashSet<Renderer>(this.renderers)).ToArray();
		this.lights = new List<Light>(new HashSet<Light>(this.lights)).ToArray();
		this.gatherChildrenTarget = null;
	}

	private void OnDrawGizmos()
	{
		if (!this.hideGizmo)
		{
			if (this.mainCamera == null)
			{
				this.mainCamera = Camera.main;
			}
			this.UpdateBounds();
			this.UpdateDistance();
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(this.bounds.center, this.bounds.size);
			if (this.mainCamera != null)
			{
				Vector3 position = this.mainCamera.transform.position;
				Vector3 to = this.bounds.ClosestPoint(position);
				Gizmos.DrawLine(position, to);
			}
		}
	}

	public bool OverwriteCulling;

	[SerializeField]
	private GameObject gatherChildrenTarget;

	[SerializeField]
	private float updateDelay = 0.1f;

	[SerializeField]
	private float cullDistance = 10f;

	[SerializeField]
	private bool enableForceEnableHotZones;

	[SerializeField]
	private HotZoneTrigger[] enableHotZones = new HotZoneTrigger[0];

	[SerializeField]
	private Renderer[] renderers = new Renderer[0];

	[SerializeField]
	private Light[] lights = new Light[0];

	[SerializeField]
	private InteractiveLight[] interactiveLights = new InteractiveLight[0];

	[SerializeField]
	private Vector3 center = Vector3.zero;

	[SerializeField]
	private Vector3 size = Vector3.one;

	[SerializeField]
	private bool hideGizmo;

	private float distance;

	private Bounds bounds;

	private Camera mainCamera;
}
