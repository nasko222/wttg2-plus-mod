using System;
using UnityEngine;

public class BombMakerManager
{
	public void CheckSulphurInventory()
	{
		if (StateManager.PlayerState != PLAYER_STATE.COMPUTER)
		{
			GameManager.TimeSlinger.FireTimer(30f, new Action(this.CheckSulphurInventory), 0);
			return;
		}
		if (SulphurInventory.SulphurAmount <= 0)
		{
			this.ScheduleAttack();
			return;
		}
		SulphurInventory.RemoveSulphur(1);
		if (ModsManager.Nightmare)
		{
			CurrencyManager.AddCurrency(20f);
		}
		else
		{
			CurrencyManager.AddCurrency(UnityEngine.Random.Range(30f, 55f));
		}
		AudioFileDefinition jumpHit = LookUp.SoundLookUp.JumpHit1;
		jumpHit.Volume = 1f;
		jumpHit.Loop = false;
		jumpHit.AudioClip = DownloadTIFiles.BombmakerLaugh;
		GameManager.AudioSlinger.PlaySound(jumpHit);
	}

	private void ScheduleAttack()
	{
		if (EnemyManager.State != ENEMY_STATE.IDLE)
		{
			GameManager.TimeSlinger.FireTimer(30f, new Action(this.ScheduleAttack), 0);
			return;
		}
		EnemyManager.State = ENEMY_STATE.LOCKED;
		this.PerformAttack();
	}

	private void ExecExplosion()
	{
		DataManager.ClearGameData();
		MainCameraHook.Ins.ClearARF(2f);
		UIManager.TriggerHardGameOver("YOU DISAPPOINTED THE BOMB MAKER");
	}

	private void PerformAttack()
	{
		if (StateManager.PlayerState == PLAYER_STATE.COMPUTER)
		{
			AudioFileDefinition jumpHit = LookUp.SoundLookUp.JumpHit1;
			jumpHit.Volume = 1f;
			jumpHit.Loop = false;
			jumpHit.AudioClip = DownloadTIFiles.Explosion;
			GameManager.AudioSlinger.PlaySound(jumpHit);
			EnvironmentManager.PowerBehaviour.ForceBombMakerPowerOff();
			GameManager.TimeSlinger.FireTimer(1.5f, new Action(this.ExecExplosion), 0);
			return;
		}
		GameManager.TimeSlinger.FireTimer(30f, new Action(this.PerformAttack), 0);
	}

	public void BombMakerPayload()
	{
		if (StateManager.PlayerState != PLAYER_STATE.COMPUTER)
		{
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.BombMakerPayload), 0);
			return;
		}
		AudioFileDefinition jumpHit = LookUp.SoundLookUp.JumpHit1;
		jumpHit.Volume = 1f;
		jumpHit.Loop = false;
		jumpHit.AudioClip = DownloadTIFiles.BombmakerLaugh;
		GameManager.AudioSlinger.PlaySound(jumpHit);
		if (!ModsManager.Nightmare)
		{
			GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("BombMaker.txt", "Hello Clint, If you happen to be reading this, then you already know who I am. I hope you like my job. I saw you visited my bomb making website. You have to understand me, it takes a lot of time to make up all of these bombs, so I need some help from you. Acquire me some bomb materials, I will do the rest of my job. Do not fail me or I will explode you! HAHAHAHAHAHA!");
		}
	}
}
