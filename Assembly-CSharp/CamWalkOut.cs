using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CamWalkOut : MonoBehaviour
{
	public void WalkOut()
	{
		EndingCameraHook.Ins.transform.SetParent(this.cameraBone);
		this.myAC.SetTrigger("walkOut");
	}

	public void TriggerFootStep()
	{
		int num = UnityEngine.Random.Range(1, this.footStepSFXs.Length);
		AudioFileDefinition audioFileDefinition = this.footStepSFXs[num];
		this.footHub.PlaySound(this.footStepSFXs[num]);
		this.footStepSFXs[num] = this.footStepSFXs[0];
		this.footStepSFXs[0] = audioFileDefinition;
	}

	private void Awake()
	{
		CamWalkOut.Ins = this;
		this.myAC = base.GetComponent<Animator>();
	}

	public static CamWalkOut Ins;

	[SerializeField]
	private Transform cameraBone;

	[SerializeField]
	private AudioHubObject footHub;

	[SerializeField]
	private AudioFileDefinition[] footStepSFXs = new AudioFileDefinition[0];

	private Animator myAC;
}
