using System;
using UnityEngine;

public class BreatherDoorMechBehaviour : StateMachineBehaviour
{
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		EnemyManager.BreatherManager.TriggerDoorMech();
	}
}
