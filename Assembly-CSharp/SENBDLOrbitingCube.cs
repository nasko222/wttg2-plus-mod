using System;
using UnityEngine;

public class SENBDLOrbitingCube : MonoBehaviour
{
	private Vector3 Vec3(float x)
	{
		return new Vector3(x, x, x);
	}

	private void Start()
	{
		this.transf = base.transform;
		this.rotationVector = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
		this.rotationVector = Vector3.Normalize(this.rotationVector);
		this.spherePosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
		this.spherePosition = Vector3.Normalize(this.spherePosition);
		this.spherePosition *= UnityEngine.Random.Range(16.5f, 18f);
		this.randomSphereRotation = new Vector3(UnityEngine.Random.Range(-1.1f, 1f), UnityEngine.Random.Range(0f, 0.1f), UnityEngine.Random.Range(0.5f, 1f));
		this.randomSphereRotation = Vector3.Normalize(this.randomSphereRotation);
		this.sphereRotationSpeed = UnityEngine.Random.Range(10f, 20f);
		this.rotationSpeed = UnityEngine.Random.Range(1f, 90f);
		this.transf.localScale = this.Vec3(UnityEngine.Random.Range(1f, 2f));
	}

	private void Update()
	{
		Quaternion rotation = Quaternion.Euler(this.randomSphereRotation * Time.time * this.sphereRotationSpeed);
		Vector3 vector = rotation * this.spherePosition;
		vector += this.spherePosition * (Mathf.Sin(Time.time - this.spherePosition.magnitude / 10f) * 0.5f + 0.5f);
		vector += rotation * this.spherePosition * (Mathf.Sin(Time.time * 3.14152646f / 4f - this.spherePosition.magnitude / 10f) * 0.5f + 0.5f);
		this.transf.position = vector;
		this.transf.rotation = Quaternion.Euler(this.rotationVector * Time.time * this.rotationSpeed);
	}

	private Transform transf;

	private Vector3 rotationVector;

	private float rotationSpeed;

	private Vector3 spherePosition;

	private Vector3 randomSphereRotation;

	private float sphereRotationSpeed;
}
