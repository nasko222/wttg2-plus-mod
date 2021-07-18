using System;
using UnityEngine;

public class ComputerChairObject : MonoBehaviour
{
	public void SetToInUsePosition()
	{
		this.chairAudioHub.PlaySound(this.getOnSFX);
		base.transform.localPosition = this.InUsePosition;
		base.transform.localRotation = Quaternion.Euler(this.InUseRotation);
	}

	public void SetToNotInUsePosition()
	{
		this.chairAudioHub.PlaySound(this.getOffSFX);
		base.transform.localPosition = this.NotInUsePosition;
		base.transform.localRotation = Quaternion.Euler(this.NotInUseRotation);
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	private void Awake()
	{
		ComputerChairObject.Ins = this;
		this.myMesh = base.GetComponent<MeshRenderer>();
	}

	public static ComputerChairObject Ins;

	[SerializeField]
	private AudioHubObject chairAudioHub;

	[SerializeField]
	private Vector3 InUsePosition = Vector3.zero;

	[SerializeField]
	private Vector3 InUseRotation = Vector3.zero;

	[SerializeField]
	private Vector3 NotInUsePosition = Vector3.zero;

	[SerializeField]
	private Vector3 NotInUseRotation = Vector3.zero;

	[SerializeField]
	private AudioFileDefinition getOnSFX;

	[SerializeField]
	private AudioFileDefinition getOffSFX;

	private MeshRenderer myMesh;
}
