using System;
using UnityEngine;

public class SENBDLCameraAnimation : MonoBehaviour
{
	private void Start()
	{
		this.randomRotation = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
		this.randomRotation = Vector3.Normalize(this.randomRotation);
		this.randomModRotation = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
		this.randomModRotation = Vector3.Normalize(this.randomModRotation);
	}

	private void Update()
	{
		float d = 15f + Mathf.Pow(Mathf.Cos(Time.time * 3.14159274f / 15f) * 0.5f + 0.5f, 3f) * 35f;
		Vector3 position = Quaternion.Euler(this.randomRotation * Time.time * 25f) * (Vector3.up * d);
		base.transform.position = position;
		base.transform.LookAt(Vector3.zero);
	}

	private Vector3 randomRotation;

	private Vector3 randomModRotation;
}
