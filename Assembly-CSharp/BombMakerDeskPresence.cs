using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[RequireComponent(typeof(deskController))]
public class BombMakerDeskPresence : MonoBehaviour
{
	public void AddComputerPresence()
	{
		HitmanComputerJumper.Ins.myComputerController.LeaveEvents.Event += this.TriggerPresence;
	}

	public void TriggerPresence()
	{
		HitmanComputerJumper.Ins.myComputerController.LeaveEvents.Event -= this.TriggerPresence;
		BombMakerManager.scheduledAutoLeave = false;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myDeskController.LockRecovery();
		this.myDeskController.SetMasterLock(true);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CustomObjectLookUp.BombMakerPresence);
		gameObject.transform.position = new Vector3(2.73f, 39.575f, -4.294f);
		gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		BombMakerPresenceGunJump component = gameObject.GetComponent<BombMakerPresenceGunJump>();
		component.hub.PlaySoundCustomDelay(CustomSoundLookUp.bombmakertalk, 2f);
		DOTween.To(() => this.transform.rotation, delegate(Quaternion x)
		{
			this.transform.rotation = x;
		}, new Vector3(0f, 60f, 0f), 0.25f).SetEase(Ease.Linear).SetOptions(true).OnComplete(delegate
		{
			MainCameraHook.Ins.TriggerHitManJump();
			component.ArmAppear();
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit1);
		});
		GameManager.TimeSlinger.FireTimer(9f, new Action(component.RandGunShake), 5);
		GameManager.TimeSlinger.FireTimer(51f, delegate()
		{
			UnityEngine.Object.Destroy(gameObject);
		}, 0);
		GameManager.TimeSlinger.FireTimer(51f, new Action(this.UnlockController), 0);
		GameManager.TimeSlinger.FireTimer(52f, new Action(LookUp.Doors.MainDoor.ForceOpenDoor), 0);
		GameManager.TimeSlinger.FireTimer(54f, new Action(EnemyManager.BombMakerManager.ClearPresenceState), 0);
	}

	private void Awake()
	{
		BombMakerDeskPresence.Ins = this;
		this.myDeskController = base.GetComponent<deskController>();
	}

	private void OnDestroy()
	{
		HitmanComputerJumper.Ins.myComputerController.LeaveEvents.Event -= this.TriggerPresence;
		BombMakerDeskPresence.Ins = null;
	}

	private void UnlockController()
	{
		PauseManager.UnLockPause();
		GameManager.BehaviourManager.CrossHairBehaviour.ShowCrossHairGroup();
		GameManager.InteractionManager.UnLockInteraction();
		this.myDeskController.SetMasterLock(false);
		this.myDeskController.UnLockRecovery();
		StateManager.PlayerState = PLAYER_STATE.DESK;
		DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, 0f, 0f), 0.25f).SetEase(Ease.Linear).SetOptions(true);
	}

	public static BombMakerDeskPresence Ins;

	private deskController myDeskController;
}
