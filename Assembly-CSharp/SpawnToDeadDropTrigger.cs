using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class SpawnToDeadDropTrigger : MonoBehaviour
{
	private void spawnPlayerToDeadDrop()
	{
		this.PlayerSpawningToDeadDropEvent.Execute();
		EnemyManager.BreatherManager.PlayerSpawnedToDeadDrop();
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).SpawnMeTo(this.DeadDropPOS, this.DeadDropROT, 1f);
	}

	private void Awake()
	{
		SpawnToDeadDropTrigger.Ins = this;
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.LeftClickAction += this.spawnPlayerToDeadDrop;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.spawnPlayerToDeadDrop;
	}

	public static SpawnToDeadDropTrigger Ins;

	public CustomEvent PlayerSpawningToDeadDropEvent = new CustomEvent(2);

	public Vector3 DeadDropPOS = Vector3.zero;

	public Vector3 DeadDropROT = Vector3.zero;

	private InteractionHook myInteractionHook;
}
