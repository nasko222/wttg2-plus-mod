using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class roamController : moveableController
{
	public void LoseControl()
	{
		this.Active = false;
		base.SetMasterLock(true);
		this.MyState = GAME_CONTROLLER_STATE.LOCKED;
		StateManager.PlayerState = PLAYER_STATE.BUSY;
	}

	public void TakeControl()
	{
		if (!this.MoveableControllerInit)
		{
			base.Init();
		}
		this.Active = true;
		base.SetMasterLock(false);
		this.MyState = GAME_CONTROLLER_STATE.IDLE;
		StateManager.PlayerState = PLAYER_STATE.ROAMING;
		this.TookControlActions.Execute();
	}

	public void GlobalTakeOver()
	{
		this.MyMouseCapture.setCameraTargetRot(this.MyCamera.transform.localRotation.x);
		CameraManager.GetCameraHook(this.CameraIControl).SetMyParent(this.HeadTiltHolder);
		Sequence sequence = DOTween.Sequence().OnComplete(new TweenCallback(this.TakeControl));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, this.DefaultCameraPOS, 0.75f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, this.DefaultCameraPOS, 0.5f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Play<Sequence>();
	}

	public void GlobalTakeOver(float POSTime, float ROTTime, float DelayTime)
	{
		this.MyMouseCapture.setCameraTargetRot(this.MyCamera.transform.localRotation.x);
		CameraManager.GetCameraHook(this.CameraIControl).SetMyParent(this.HeadTiltHolder);
		Sequence sequence = DOTween.Sequence().OnComplete(new TweenCallback(this.TakeControl));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, this.DefaultCameraPOS, POSTime).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, this.DefaultCameraPOS, ROTTime).SetEase(Ease.Linear).SetOptions(true));
		sequence.SetDelay(DelayTime);
		sequence.Play<Sequence>();
	}

	public void SwitchToPeepHoleController()
	{
		DataManager.LockSave = true;
		this.LoseControl();
		GameManager.InteractionManager.LockInteraction();
		GameManager.BehaviourManager.CrossHairBehaviour.HideCrossHairGroup();
		this.lastPOS = base.transform.position;
		this.lastROT = base.transform.rotation.eulerAngles;
		this.lastCameraPOS = this.MyCamera.transform.localPosition;
		this.lastCameraROT = this.MyCamera.transform.localRotation.eulerAngles;
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			ControllerManager.Get<peepHoleController>(GAME_CONTROLLER.PEEP_HOLE).TakeOver();
		});
		sequence.Insert(0f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, new Vector3(-2.194f, 40.5183f, -4.827071f), 0.75f).SetEase(Ease.OutQuart));
		sequence.Insert(0f, DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, 180f, 0f), 0.75f).SetEase(Ease.OutQuart).SetOptions(true));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, this.DefaultCameraPOS, 0.45f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, this.DefaultCameraROT, 0.45f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Insert(0.45f, DOTween.To(() => this.MyCamera.fieldOfView, delegate(float x)
		{
			this.MyCamera.fieldOfView = x;
		}, 15.5f, 0.4f).SetEase(Ease.Linear));
		sequence.Insert(0.45f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, new Vector3(0.0042f, 0.2592f, 0.1175f), 0.6f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void TakeOverFromPeep()
	{
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			DataManager.LockSave = false;
			GameManager.InteractionManager.UnLockInteraction();
			GameManager.BehaviourManager.CrossHairBehaviour.ShowCrossHairGroup();
			this.TakeControl();
		});
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.fieldOfView, delegate(float x)
		{
			this.MyCamera.fieldOfView = x;
		}, 60f, 0.4f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, this.DefaultCameraPOS, 0.4f).SetEase(Ease.Linear));
		sequence.Insert(0.4f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, this.lastPOS, 0.75f).SetEase(Ease.InQuart));
		sequence.Insert(0.4f, DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, this.lastROT, 0.75f).SetEase(Ease.InQuart).SetOptions(true));
		sequence.Insert(0.4f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, this.lastCameraPOS, 0.4f).SetEase(Ease.Linear));
		sequence.Insert(0.4f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, this.lastCameraROT, 0.4f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void JumpRailingFromBalcoy()
	{
		base.SetMasterLock(true);
		GameManager.InteractionManager.LockInteraction();
		GameManager.BehaviourManager.CrossHairBehaviour.HideCrossHairGroup();
		this.MyMouseCapture.setFullCameraTargetRot(Vector3.zero);
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			CameraManager.GetCameraHook(CAMERA_ID.MAIN).SwitchToGlobalParent();
			base.transform.position = new Vector3(-1.973f, 40.138f, 4.16f);
			CameraManager.GetCameraHook(this.CameraIControl).SetMyParent(this.HeadTiltHolder);
			Sequence sequence2 = DOTween.Sequence().OnComplete(delegate
			{
				GameManager.InteractionManager.UnLockInteraction();
				GameManager.BehaviourManager.CrossHairBehaviour.ShowCrossHairGroup();
				base.SetMasterLock(false);
			});
			sequence2.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.MyCamera.transform.localPosition = x;
			}, new Vector3(-0.05f, 0.2764f, -0.401f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, new Vector3(31.414f, -12.121f, -6.387f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.3f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.MyCamera.transform.localPosition = x;
			}, new Vector3(-0.4608f, 0.218f, -0.181f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.3f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, new Vector3(44.885f, -19.738f, 2.839f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.6f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.MyCamera.transform.localPosition = x;
			}, new Vector3(-0.628f, 0.109f, 0.061f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.6f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, new Vector3(38.745f, 1.447f, 34.416f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.9f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.MyCamera.transform.localPosition = x;
			}, new Vector3(-0.328f, 0.078f, 0.107f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.9f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, new Vector3(21.487f, -7.096f, 15.127f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(1.2f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.MyCamera.transform.localPosition = x;
			}, this.DefaultCameraPOS, 0.6f).SetEase(Ease.Linear));
			sequence2.Insert(1.2f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, this.DefaultCameraROT, 0.6f).SetEase(Ease.Linear));
			sequence2.Play<Sequence>();
		});
		sequence.Insert(0f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, new Vector3(-2.92f, 40.138f, 4.16f), 0.3f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, 90f, 0f), 0.3f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, this.DefaultCameraPOS, 0.3f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, this.DefaultCameraROT, 0.3f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void JumpRailingFromStairWell()
	{
		base.SetMasterLock(true);
		GameManager.InteractionManager.LockInteraction();
		GameManager.BehaviourManager.CrossHairBehaviour.HideCrossHairGroup();
		this.MyMouseCapture.setFullCameraTargetRot(Vector3.zero);
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			CameraManager.GetCameraHook(CAMERA_ID.MAIN).SwitchToGlobalParent();
			base.transform.position = new Vector3(-2.92f, 40.138f, 4.16f);
			CameraManager.GetCameraHook(this.CameraIControl).SetMyParent(this.HeadTiltHolder);
			Sequence sequence2 = DOTween.Sequence().OnComplete(delegate
			{
				GameManager.InteractionManager.UnLockInteraction();
				GameManager.BehaviourManager.CrossHairBehaviour.ShowCrossHairGroup();
				base.SetMasterLock(false);
			});
			sequence2.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.MyCamera.transform.localPosition = x;
			}, new Vector3(-0.0636f, 0.2971f, -0.7757f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, new Vector3(27.067f, 4.084f, 8.391f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.3f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.MyCamera.transform.localPosition = x;
			}, new Vector3(-0.212f, 0.247f, -0.544f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.3f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, new Vector3(30.671f, 8.478f, 17.479f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.6f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.MyCamera.transform.localPosition = x;
			}, new Vector3(-0.3255f, 0.1413f, -0.2621f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.6f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, new Vector3(30.268f, 15.123f, 34.3f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.9f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.MyCamera.transform.localPosition = x;
			}, new Vector3(-0.2697f, 0.1515f, -0.1587f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(0.9f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, new Vector3(23.563f, 8.448f, 15.347f), 0.3f).SetEase(Ease.Linear));
			sequence2.Insert(1.2f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.MyCamera.transform.localPosition = x;
			}, this.DefaultCameraPOS, 0.6f).SetEase(Ease.Linear));
			sequence2.Insert(1.2f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, this.DefaultCameraROT, 0.6f).SetEase(Ease.Linear));
			sequence2.Play<Sequence>();
		});
		sequence.Insert(0f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, new Vector3(-2.122f, 40.138f, 4.16f), 0.3f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, -90f, 0f), 0.3f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, this.DefaultCameraPOS, 0.3f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, this.DefaultCameraROT, 0.3f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void PutMe(Vector3 SetPOS, Vector3 SetROT, bool RememberLastLocation = false)
	{
		if (RememberLastLocation)
		{
			this.lastPOS = base.transform.position;
			this.lastROT = base.transform.rotation.eulerAngles;
		}
		base.transform.position = SetPOS;
		base.transform.rotation = Quaternion.Euler(SetROT);
	}

	public void ReturnMe()
	{
		base.transform.position = this.lastPOS;
		base.transform.rotation = Quaternion.Euler(this.lastROT);
	}

	public void ReturnMe(Vector3 CustomROT)
	{
		this.MyMouseCapture.setRotatingObjectRotation(CustomROT);
		this.MyMouseCapture.setRotatingObjectTargetRot(CustomROT);
		base.transform.position = this.lastPOS;
		base.transform.rotation = Quaternion.Euler(CustomROT);
	}

	public void SpawnMeTo(Vector3 ToLocation, Vector3 ToRotation, float ReSpawnDelay)
	{
		UIManager.FadeScreen(1f, 0.5f);
		base.SetMasterLock(true);
		this.lastPlayerState = StateManager.PlayerState;
		this.MyState = GAME_CONTROLLER_STATE.IDLE;
		StateManager.PlayerState = PLAYER_STATE.BUSY;
		GameManager.InteractionManager.LockInteraction();
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.TimeSlinger.FireTimer(0.5f, delegate()
		{
			this.transform.position = ToLocation;
			this.transform.rotation = Quaternion.Euler(ToRotation);
			this.MyMouseCapture.setRotatingObjectRotation(ToRotation);
			this.MyMouseCapture.setRotatingObjectTargetRot(ToRotation);
			GameManager.TimeSlinger.FireTimer(ReSpawnDelay, delegate()
			{
				UIManager.FadeScreen(0f, 0.5f);
				GameManager.InteractionManager.UnLockInteraction();
				this.SetMasterLock(false);
				StateManager.PlayerState = this.lastPlayerState;
				DataManager.LockSave = false;
				PauseManager.UnLockPause();
			}, 0);
		}, 0);
	}

	public void MoveOutOfTheWay(float LookDir, Vector3 Destination, CARDINAL_DIR Direction)
	{
		base.SetMasterLock(true);
		this.lastPlayerState = StateManager.PlayerState;
		this.MyState = GAME_CONTROLLER_STATE.IDLE;
		StateManager.PlayerState = PLAYER_STATE.BUSY;
		Vector3 endValue = Vector3.zero;
		Vector3 endValue2 = Vector3.zero;
		Vector3 vector = new Vector3(0f, LookDir, 0f);
		switch (Direction)
		{
		case CARDINAL_DIR.FORWARD:
			endValue = Destination + base.transform.forward * this.MyCharcterController.radius;
			endValue2 = Destination + base.transform.forward * (this.MyCharcterController.radius + 0.5f);
			endValue2.x = endValue.x;
			break;
		case CARDINAL_DIR.BACK:
			endValue = Destination + -base.transform.forward * this.MyCharcterController.radius;
			endValue2 = Destination + -base.transform.forward * (this.MyCharcterController.radius + 0.5f);
			endValue2.x = endValue.x;
			break;
		case CARDINAL_DIR.LEFT:
			endValue = Destination + -base.transform.right * this.MyCharcterController.radius;
			endValue2 = Destination + -base.transform.right * (this.MyCharcterController.radius + 0.5f);
			endValue2.z = endValue.z;
			break;
		case CARDINAL_DIR.RIGHT:
			endValue = Destination + base.transform.right * this.MyCharcterController.radius;
			endValue2 = Destination + base.transform.right * (this.MyCharcterController.radius + 0.5f);
			endValue2.z = endValue.z;
			break;
		}
		endValue.y = base.transform.position.y;
		endValue2.y = base.transform.position.y;
		this.MyMouseCapture.setCameraTargetRot(0f);
		this.MyMouseCapture.setRotatingObjectTargetRot(vector);
		this.getOutOfTheWaySeq = DOTween.Sequence().OnComplete(delegate
		{
			if (!this.lockFromDoorOpen)
			{
				StateManager.PlayerState = this.lastPlayerState;
				base.SetMasterLock(false);
			}
		});
		this.getOutOfTheWaySeq.Insert(0f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, endValue2, 0.45f).SetEase(Ease.OutCubic));
		this.getOutOfTheWaySeq.Insert(0f, DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, vector, 0.45f).SetEase(Ease.OutCubic));
		this.getOutOfTheWaySeq.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, this.DefaultCameraPOS, 0.65f).SetEase(Ease.OutSine));
		this.getOutOfTheWaySeq.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, this.DefaultCameraPOS, 0.65f).SetEase(Ease.OutSine).SetOptions(true));
		this.getOutOfTheWaySeq.Insert(0.65f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, endValue, 0.3f).SetEase(Ease.Linear));
		this.getOutOfTheWaySeq.Play<Sequence>();
	}

	public void HardMove(Vector3 Destination)
	{
		Vector3 position = Vector3.zero;
		position = Destination;
		position.y = base.transform.position.y;
		base.transform.position = position;
	}

	public void LockFromDoorRecovry()
	{
		this.lockFromDoorOpen = true;
	}

	public void KillOutOfWay()
	{
		if (this.getOutOfTheWaySeq != null)
		{
			this.getOutOfTheWaySeq.Pause<Sequence>();
			this.getOutOfTheWaySeq.Kill(false);
		}
	}

	private void takeInput()
	{
		if (!this.lockControl)
		{
			float num = CrossPlatformInputManager.GetAxis("HeadTilt") * this.HeadTiltValue;
			float x = CrossPlatformInputManager.GetAxis("HeadTilt") * this.HeadTitleXValue;
			this.DisableRun = (Mathf.Abs(num) > 0f);
			this.HeadTiltHolder.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, num));
			this.HeadTiltHolder.transform.localPosition = new Vector3(x, this.defaultHeadTiltPOS.y, this.defaultHeadTiltPOS.z);
		}
	}

	private void postBaseStage()
	{
		this.PostStage.Event -= this.postBaseStage;
		if (DataManager.ContinuedGame)
		{
			this.MyState = GAME_CONTROLLER_STATE.IDLE;
			this.Active = true;
			CameraManager.GetCameraHook(this.CameraIControl).SetMyParent(this.HeadTiltHolder);
			this.MyCamera.transform.localPosition = this.DefaultCameraPOS;
			this.MyCamera.transform.localRotation = Quaternion.Euler(this.DefaultCameraROT);
			CameraManager.GetCameraHook(this.CameraIControl).ManualPushDataUpdate();
		}
	}

	private void postBaseLive()
	{
		this.PostLive.Event -= this.postBaseLive;
		if (this.Active)
		{
			this.TakeControl();
		}
	}

	protected new void Awake()
	{
		roamController.Ins = this;
		base.Awake();
		ControllerManager.Add(this);
		this.defaultHeadTiltPOS = this.HeadTiltHolder.transform.localPosition;
		this.PostStage.Event += this.postBaseStage;
		this.PostLive.Event += this.postBaseLive;
	}

	protected new void Start()
	{
		base.Start();
	}

	protected new void Update()
	{
		base.Update();
		this.takeInput();
	}

	protected new void OnDestroy()
	{
		ControllerManager.Remove(this.Controller);
		base.OnDestroy();
	}

	private void OnDrawGizmos()
	{
		if (this.MyCharcterController != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(base.transform.position, this.MyCharcterController.radius);
			Gizmos.DrawLine(base.transform.position, base.transform.position + Vector3.down * (this.MyCharcterController.height / 2f + this.MyCharcterController.radius));
		}
	}

	public static roamController Ins;

	public Transform HeadTiltHolder;

	public float HeadTiltValue;

	public float HeadTitleXValue;

	public CustomEvent TookControlActions = new CustomEvent(5);

	private PLAYER_STATE lastPlayerState;

	private Sequence switchToPeepHoleSequence;

	private Sequence getOutOfTheWaySeq;

	private Vector3 lastPOS;

	private Vector3 lastROT;

	private Vector3 lastCameraPOS;

	private Vector3 lastCameraROT;

	private Vector3 defaultHeadTiltPOS;

	private bool lockFromDoorOpen;
}
