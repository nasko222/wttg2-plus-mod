using System;
using UnityEngine;

public class Doors : MonoBehaviour
{
	private void Awake()
	{
		LookUp.Doors = this;
	}

	public DoorTrigger MainDoor;

	public DoorTrigger BathroomDoor;

	public DoorTrigger BalconyDoor;

	public DoorTrigger DeadDropDoor;

	public DoorTrigger Door1;

	public DoorTrigger Door3;

	public DoorTrigger Door5;

	public DoorTrigger Door6;

	public DoorTrigger Door8;

	public DoorTrigger Door10;
}
