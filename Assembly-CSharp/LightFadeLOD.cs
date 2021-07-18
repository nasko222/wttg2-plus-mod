using System;
using UnityEngine;

public class LightFadeLOD : MonoBehaviour
{
	private void Awake()
	{
		this.mainCamera = Camera.main;
		this.defaultShadows = this.lodLight.shadows;
		this.defaultRange = this.lodLight.range;
		this.defaultIntensity = this.lodLight.intensity;
		this.defaultShadowStrength = this.lodLight.shadowStrength;
	}

	private void Update()
	{
		this.UpdateDistance();
		this.UpdateFadePercent();
		this.UpdateLightValues();
	}

	private void UpdateDistance()
	{
		if (this.lodLight == null || this.mainCamera == null)
		{
			this.distance = 0f;
			return;
		}
		Vector3 a = this.lodLight.transform.TransformPoint(this.center);
		Vector3 position = this.mainCamera.transform.position;
		this.distance = (a - position).magnitude;
	}

	private void UpdateFadePercent()
	{
		if (this.lodLight == null || this.mainCamera == null || this.distance < this.beginFadeDistance)
		{
			this.fadePercent = 1f;
			return;
		}
		this.fadePercent = Mathf.Clamp01(1f - (this.distance - this.beginFadeDistance) / (this.endFadeDistance - this.beginFadeDistance));
	}

	private void UpdateLightValues()
	{
		if (this.lodLight == null)
		{
			return;
		}
		if (this.intensity)
		{
			this.lodLight.intensity = Mathf.Lerp(0f, this.defaultIntensity, this.fadePercent);
		}
		else
		{
			this.lodLight.intensity = this.defaultIntensity;
		}
		if (this.range)
		{
			this.lodLight.range = Mathf.Lerp(0f, this.defaultRange, this.fadePercent);
		}
		else
		{
			this.lodLight.range = this.defaultRange;
		}
		if (this.shadowStrength)
		{
			this.lodLight.shadowStrength = Mathf.Lerp(0f, this.defaultShadowStrength, this.fadePercent);
		}
		else
		{
			this.lodLight.shadowStrength = this.defaultShadowStrength;
		}
		if (this.toggleShadows)
		{
			this.lodLight.shadows = ((this.fadePercent > 0f) ? this.defaultShadows : LightShadows.None);
		}
		else
		{
			LightShadows shadows = (this.lodLight.shadowStrength > 0f) ? this.defaultShadows : LightShadows.None;
			this.lodLight.shadows = shadows;
		}
	}

	private void OnValidate()
	{
		if (this.lodLight == null)
		{
			this.lodLight = base.GetComponent<Light>();
		}
		float min = (!(this.lodLight != null)) ? float.MinValue : this.lodLight.range;
		this.beginFadeDistance = Mathf.Clamp(this.beginFadeDistance, min, this.endFadeDistance);
		this.endFadeDistance = Mathf.Clamp(this.endFadeDistance, this.beginFadeDistance, float.MaxValue);
	}

	private void OnDrawGizmos()
	{
		if (!this.hideGizmo)
		{
			if (this.lodLight == null)
			{
				this.lodLight = base.GetComponent<Light>();
			}
			if (this.mainCamera == null)
			{
				this.mainCamera = Camera.main;
			}
			this.UpdateDistance();
			this.UpdateFadePercent();
			Vector3 from = (!(this.lodLight != null)) ? base.transform.TransformPoint(this.center) : this.lodLight.transform.TransformPoint(this.center);
			Gizmos.color = new Color(0.7f, 0.7f, 0f, 1f);
			Gizmos.DrawWireSphere(from, this.beginFadeDistance);
			Gizmos.color = new Color(0.7f, 0.7f, 0f, 0.5f);
			Gizmos.DrawWireSphere(from, this.endFadeDistance);
			if (Application.isPlaying && this.mainCamera != null)
			{
				Gizmos.DrawLine(from, this.mainCamera.transform.position);
			}
		}
	}

	[SerializeField]
	[Tooltip("Fades shadow strength to 0 when camera is leaving range.")]
	private bool shadowStrength = true;

	[SerializeField]
	[Tooltip("Disables shadows when camera is out of range. (Does not affect shadow strength)")]
	private bool toggleShadows = true;

	[SerializeField]
	[Tooltip("Fades light range to 0 when camera is leaving range.")]
	private bool range;

	[SerializeField]
	[Tooltip("Fades light intensity to 0 when camera is leaving range.")]
	private bool intensity = true;

	[SerializeField]
	private float beginFadeDistance = 10f;

	[SerializeField]
	private float endFadeDistance = 20f;

	[SerializeField]
	private Vector3 center = Vector3.zero;

	[SerializeField]
	private Light lodLight;

	[SerializeField]
	private bool hideGizmo;

	private float distance;

	private float fadePercent;

	private Camera mainCamera;

	private LightShadows defaultShadows;

	private float defaultRange;

	private float defaultIntensity;

	private float defaultShadowStrength;
}
