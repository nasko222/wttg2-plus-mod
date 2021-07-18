using System;
using UnityEngine;

public class SpawnToLobbyTrigger : MonoBehaviour
{
	public void LockOut()
	{
		this.lockedOut = true;
	}

	private void triggerSpawn()
	{
		if (!this.lockedOut)
		{
			EnemyManager.BreatherManager.PlayerLeftDeadDrop();
			ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).SpawnMeTo(this.LobbyPOS, this.LobbyROT, 1f);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		this.triggerSpawn();
	}

	public Vector3 LobbyPOS;

	public Vector3 LobbyROT;

	private bool lockedOut;
}
