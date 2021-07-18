using System;
using UnityEngine;

public class ParticlePlexusDemo_CameraRig : MonoBehaviour
{
	private void Start()
	{
		this.startRotation = this.pivot.localEulerAngles;
		this.targetRotation = this.startRotation;
	}

	private void Update()
	{
		float num = Input.GetAxis("Horizontal");
		float num2 = Input.GetAxis("Vertical");
		if (Input.GetKeyDown(KeyCode.R))
		{
			this.targetRotation = this.startRotation;
		}
		num *= this.rotationSpeed;
		num2 *= this.rotationSpeed;
		this.targetRotation.y = this.targetRotation.y + num;
		this.targetRotation.x = this.targetRotation.x + num2;
		this.targetRotation.x = Mathf.Clamp(this.targetRotation.x, -this.rotationLimit, this.rotationLimit);
		this.targetRotation.y = Mathf.Clamp(this.targetRotation.y, -this.rotationLimit, this.rotationLimit);
		Vector3 localEulerAngles = this.pivot.localEulerAngles;
		localEulerAngles.x = Mathf.LerpAngle(localEulerAngles.x, this.targetRotation.x, Time.deltaTime * this.rotationLerpSpeed);
		localEulerAngles.y = Mathf.LerpAngle(localEulerAngles.y, this.targetRotation.y, Time.deltaTime * this.rotationLerpSpeed);
		this.pivot.localEulerAngles = localEulerAngles;
	}

	public Transform pivot;

	private Vector3 targetRotation;

	[Range(0f, 90f)]
	public float rotationLimit = 90f;

	public float rotationSpeed = 2f;

	public float rotationLerpSpeed = 4f;

	private Vector3 startRotation;
}
