using System;
using UnityEngine;

public class RotateWardrobeDoorScrub : MonoBehaviour
{
	public void TriggerRotate(float WeightAmount)
	{
		base.transform.localRotation = Quaternion.Euler(new Vector3(base.transform.localRotation.x, Mathf.Lerp(0f, this.rotateAmount, WeightAmount), base.transform.localRotation.z));
	}

	[SerializeField]
	private float rotateAmount;
}
