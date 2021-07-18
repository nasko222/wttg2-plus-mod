using System;
using UnityEngine;

public class TitleCultFemaleEnterIdleState : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		TitleCultBehaviour.Ins.TriggerLoopIdle();
	}
}
