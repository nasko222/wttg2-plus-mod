using System;
using UnityEngine;

public class FallFailSafeTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = this.roomSpawnLocation;
	}

	private void OnTriggerStay(Collider other)
	{
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = this.roomSpawnLocation;
	}

	private Vector3 roomSpawnLocation = new Vector3(-4.14f, 40.884f, -2.423f);
}
