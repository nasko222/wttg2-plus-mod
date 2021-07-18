using System;
using UnityEngine;

[RequireComponent(typeof(peepHoleController))]
public class HitmanPeepJumper : MonoBehaviour
{
	public void AddPeepJump()
	{
		this.myPeepHoleController.TakingOverEvents.Event += this.hitmanPeepJump.Stage;
		this.myPeepHoleController.TookOverEvents.Event += this.hitmanPeepJump.Execute;
		this.myPeepHoleController.TookOverEvents.Event += this.peepJump;
	}

	private void peepJump()
	{
		this.myPeepHoleController.TakingOverEvents.Event -= this.hitmanPeepJump.Stage;
		this.myPeepHoleController.TookOverEvents.Event -= this.hitmanPeepJump.Execute;
		this.myPeepHoleController.TookOverEvents.Event -= this.peepJump;
		this.myPeepHoleController.SetMasterLock(true);
	}

	private void Awake()
	{
		HitmanPeepJumper.Ins = this;
		this.myPeepHoleController = base.GetComponent<peepHoleController>();
	}

	public static HitmanPeepJumper Ins;

	private peepHoleController myPeepHoleController;

	private HitmanPeepJump hitmanPeepJump = new HitmanPeepJump();
}
