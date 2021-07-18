using System;
using UnityEngine;

public class BreatherLeftDoorMechBehaviour : StateMachineBehaviour
{
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		BreatherPatrolBehaviour.Ins.LeftDoorPatrol();
	}
}
