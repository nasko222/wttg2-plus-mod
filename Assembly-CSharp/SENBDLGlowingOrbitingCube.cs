using System;
using UnityEngine;

public class SENBDLGlowingOrbitingCube : MonoBehaviour
{
	private Vector3 Vec3(float x)
	{
		return new Vector3(x, x, x);
	}

	private void Start()
	{
		base.transform.localScale = this.Vec3(1.5f);
		this.pulseSpeed = UnityEngine.Random.Range(4f, 8f);
		this.phase = UnityEngine.Random.Range(0f, 6.28318548f);
	}

	private void Update()
	{
		Color color = SENBDLGlobal.mainCube.glowColor;
		color.r = 1f - color.r;
		color.g = 1f - color.g;
		color.b = 1f - color.b;
		color = Color.Lerp(color, Color.white, 0.1f);
		color *= Mathf.Pow(Mathf.Sin(Time.time * this.pulseSpeed + this.phase) * 0.49f + 0.51f, 2f);
		base.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
		base.GetComponent<Light>().color = color;
	}

	private float pulseSpeed;

	private float phase;
}
