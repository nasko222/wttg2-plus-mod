using System;
using UnityEngine;
using UnityEngine.Events;

public class BombMakerBehaviour : MonoBehaviour
{
	private void Awake()
	{
		BombMakerBehaviour.Ins = this;
		this.BMF10j = new BombMakerFloor10Jump();
		this.BMF8j = new BombMakerFloor8Jump();
		this.BMF6j = new BombMakerFloor6Jump();
		this.BMF5j = new BombMakerFloor5Jump();
		this.BMF3j = new BombMakerFloor3Jump();
		this.BMF1j = new BombMakerFloor1Jump();
		this.BMAj = new BombMakerMainDoorJump();
	}

	private void stageFloor10Jump()
	{
		LookUp.Doors.Door10.DoorOpenEvent.AddListener(new UnityAction(this.BMF10j.Stage));
		LookUp.Doors.Door10.DoorWasOpenedEvent.AddListener(new UnityAction(this.BMF10j.Execute));
	}

	public void stageFloor8Jump()
	{
		LookUp.Doors.Door8.DoorOpenEvent.AddListener(new UnityAction(this.BMF8j.Stage));
		LookUp.Doors.Door8.DoorWasOpenedEvent.AddListener(new UnityAction(this.BMF8j.Execute));
	}

	private void stageFloor6Jump()
	{
		LookUp.Doors.Door6.DoorOpenEvent.AddListener(new UnityAction(this.BMF6j.Stage));
		LookUp.Doors.Door6.DoorWasOpenedEvent.AddListener(new UnityAction(this.BMF6j.Execute));
	}

	private void stageFloor5Jump()
	{
		LookUp.Doors.Door5.DoorOpenEvent.AddListener(new UnityAction(this.BMF5j.Stage));
		LookUp.Doors.Door5.DoorWasOpenedEvent.AddListener(new UnityAction(this.BMF5j.Execute));
	}

	private void stageFloor3Jump()
	{
		LookUp.Doors.Door3.DoorOpenEvent.AddListener(new UnityAction(this.BMF3j.Stage));
		LookUp.Doors.Door3.DoorWasOpenedEvent.AddListener(new UnityAction(this.BMF3j.Execute));
	}

	public void stageFloor1Jump()
	{
		LookUp.Doors.Door1.DoorOpenEvent.AddListener(new UnityAction(this.BMF1j.Stage));
		LookUp.Doors.Door1.DoorWasOpenedEvent.AddListener(new UnityAction(this.BMF1j.Execute));
	}

	public void StageBombMakerOutsideKill()
	{
		if (StateManager.PlayerLocation == PLAYER_LOCATION.DEAD_DROP || StateManager.PlayerLocation == PLAYER_LOCATION.DEAD_DROP_ROOM || StateManager.PlayerLocation == PLAYER_LOCATION.LOBBY || StateManager.PlayerLocation == PLAYER_LOCATION.LOBBY_COMPUTER)
		{
			this.stageFloor1Jump();
			return;
		}
		if (StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY1)
		{
			this.stageFloor1Jump();
			this.stageApartmentJump();
			return;
		}
		if (StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY3)
		{
			this.stageFloor3Jump();
			this.stageApartmentJump();
			return;
		}
		if (StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY5)
		{
			this.stageFloor5Jump();
			this.stageApartmentJump();
			return;
		}
		if (StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY6)
		{
			this.stageFloor6Jump();
			this.stageApartmentJump();
			return;
		}
		if (StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY8)
		{
			this.stageFloor8Jump();
			this.stageApartmentJump();
			return;
		}
		if (StateManager.PlayerLocation == PLAYER_LOCATION.HALL_WAY10)
		{
			this.stageFloor10Jump();
			this.stageApartmentJump();
			return;
		}
		if (StateManager.PlayerLocation == PLAYER_LOCATION.MAINTENANCE_ROOM || StateManager.PlayerLocation == PLAYER_LOCATION.STAIR_WAY)
		{
			this.stageApartmentJump();
		}
		if (StateManager.PlayerLocation == PLAYER_LOCATION.MAIN_ROON || StateManager.PlayerLocation == PLAYER_LOCATION.BATH_ROOM || StateManager.PlayerLocation == PLAYER_LOCATION.OUTSIDE)
		{
			this.stageFloor8Jump();
		}
	}

	public void stageApartmentJump()
	{
		LookUp.Doors.MainDoor.DoorOpenEvent.AddListener(new UnityAction(this.BMAj.Stage));
		LookUp.Doors.MainDoor.DoorWasOpenedEvent.AddListener(new UnityAction(this.BMAj.Execute));
	}

	public static BombMakerBehaviour Ins;

	private BombMakerFloor10Jump BMF10j;

	private BombMakerFloor8Jump BMF8j;

	private BombMakerFloor6Jump BMF6j;

	private BombMakerFloor5Jump BMF5j;

	private BombMakerFloor3Jump BMF3j;

	private BombMakerFloor1Jump BMF1j;

	private BombMakerMainDoorJump BMAj;
}
