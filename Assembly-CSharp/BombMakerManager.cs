using System;

public class BombMakerManager
{
	public void CheckSulphurInventory()
	{
		if (SulphurInventory.SulphurAmount <= 0)
		{
			this.ScheduleAttack();
			return;
		}
		SulphurInventory.RemoveSulphur(1);
		CurrencyManager.AddCurrency(35f);
		AudioFileDefinition jumpHit = LookUp.SoundLookUp.JumpHit1;
		jumpHit.Volume = 1f;
		jumpHit.Loop = false;
		jumpHit.AudioClip = DownloadTIFiles.BombmakerLaugh;
		GameManager.AudioSlinger.PlaySound(jumpHit);
	}

	private void ScheduleAttack()
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
		GameManager.TimeSlinger.FireTimer(30f, new Action(this.ScheduleAttack), 0);
	}

	private void ExecExplosion()
	{
		DataManager.ClearGameData();
		MainCameraHook.Ins.ClearARF(2f);
		UIManager.TriggerHardGameOver("YOU DISAPPOINTED THE BOMB MAKER");
	}
}
