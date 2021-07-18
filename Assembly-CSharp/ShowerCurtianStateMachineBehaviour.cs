using System;
using UnityEngine;

public class ShowerCurtianStateMachineBehaviour : StateMachineBehaviour
{
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		ShowerCurtianTrigger.Ins.AnimationCompleted(this.myState);
	}

	[SerializeField]
	private SHOWER_CURTIAN_STATES myState;
}
