using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractionHook))]
[RequireComponent(typeof(AudioHubObject))]
[RequireComponent(typeof(BoxCollider))]
public class DoorTrigger : MonoBehaviour
{
	public bool Locked
	{
		get
		{
			return this.isLocked;
		}
	}

	public AudioHubObject AudioHub
	{
		get
		{
			return this.myAudioHub;
		}
	}

	public bool DoingSomething
	{
		get
		{
			return this.amBusy || this.amOpend;
		}
	}

	public bool LockOutAutoClose
	{
		get
		{
			return this.lockoutAutoClose;
		}
		set
		{
			this.lockoutAutoClose = value;
		}
	}

	public void SetOpenDoorAnimation(DOTweenAnimation SetAnimation)
	{
		if (!this.doorOpenAnimations.Contains(SetAnimation))
		{
			this.doorOpenAnimations.Add(SetAnimation);
		}
	}

	public void SetCloseDoorAnimation(DOTweenAnimation SetAnimation)
	{
		if (!this.doorCloseAnimations.Contains(SetAnimation))
		{
			this.doorCloseAnimations.Add(SetAnimation);
		}
	}

	public void SetLockDoorAnimation(DOTweenAnimation SetAnimation)
	{
		this.doorLockAnimation = SetAnimation;
	}

	public void SetUnLockDoorAnimation(DOTweenAnimation SetAnimation)
	{
		this.doorUnLockAnimation = SetAnimation;
	}

	public void DoneWithAnimation()
	{
		if (this.amOpend)
		{
			if (!this.lockoutAutoClose)
			{
				this.checkDoorCloseRange = true;
			}
			this.DoorWasOpenedEvent.Invoke();
		}
		else
		{
			this.amBusy = false;
			this.DoorWasClosedEvent.Invoke();
			if (this.NoClip)
			{
				this.checkColliderRange = true;
			}
			if (this.SetDistanceLODs != null)
			{
				for (int i = 0; i < this.SetDistanceLODs.Length; i++)
				{
					this.SetDistanceLODs[i].OverwriteCulling = false;
				}
			}
		}
		if (this.DoneWithAnimationEvents != null)
		{
			this.DoneWithAnimationEvents.Invoke();
		}
	}

	public void DoneWithLockAnimations()
	{
		if (this.DoorLockable)
		{
			if (this.isLocked)
			{
				this.isLocked = false;
			}
			else
			{
				this.isLocked = true;
			}
			this.amBusy = false;
			this.myInteractionHook.MyBoxCollider.enabled = true;
		}
	}

	public void ShowLockState()
	{
		if (this.isLocked)
		{
			UIInteractionManager.Ins.ShowUnLock();
		}
		else
		{
			UIInteractionManager.Ins.ShowLock();
		}
	}

	public void HideLockState()
	{
		if (this.isLocked)
		{
			UIInteractionManager.Ins.HideUnLock();
		}
		else
		{
			UIInteractionManager.Ins.HideLock();
		}
	}

	public void ForceOpenDoor()
	{
		if (this.CheckPlayerIsBlockingDoorPath)
		{
			this.fireCheckForPlayer = true;
		}
		else
		{
			this.openTheDoor();
		}
	}

	public void ForceOpenDoorDisableAutoClose()
	{
		this.lockoutAutoClose = true;
		if (this.CheckPlayerIsBlockingDoorPath)
		{
			this.fireCheckForPlayer = true;
		}
		else
		{
			this.openTheDoor();
		}
		GameManager.TimeSlinger.FireTimer(120f, delegate()
		{
			if (this.lockoutAutoClose)
			{
				this.lockoutAutoClose = false;
			}
		}, 0);
	}

	public void NPCOpenDoor()
	{
		this.lockoutAutoClose = true;
		this.openTheDoor();
		GameManager.TimeSlinger.FireTimer(120f, delegate()
		{
			if (this.lockoutAutoClose)
			{
				this.lockoutAutoClose = false;
			}
		}, 0);
	}

	public void ForceDoorClose()
	{
		this.closeTheDoor();
		this.lockoutAutoClose = false;
	}

	public void CancelAutoClose()
	{
		this.lucasDoor = true;
		this.checkDoorCloseRange = false;
		GameManager.TimeSlinger.FireTimer(120f, delegate()
		{
			if (!this.checkDoorCloseRange)
			{
				this.checkDoorCloseRange = true;
			}
		}, 0);
	}

	public void SetCustomOpenDoorTime(float SetValue)
	{
		for (int i = 0; i < this.doorOpenAnimations.Count; i++)
		{
			this.doorOpenAnimations[i].duration = SetValue;
		}
	}

	public void KickDoorOpen()
	{
		if (this.DoorTransform != null)
		{
			DOTween.To(() => this.DoorTransform.localRotation, delegate(Quaternion x)
			{
				this.DoorTransform.localRotation = x;
			}, new Vector3(0f, 90f, 0f), 0.25f).SetEase(Ease.Linear);
		}
	}

	public void DisableDoor()
	{
		this.amBusy = true;
		this.myInteractionHook.MyBoxCollider.enabled = false;
	}

	public void EnableDoor()
	{
		this.amBusy = false;
		this.myInteractionHook.MyBoxCollider.enabled = true;
	}

	private void leftClickAction()
	{
		if (!this.amBusy)
		{
			if (this.DoorLockable && TarotManager.HermitActive)
			{
				this.myAudioHub.PlaySound(this.DoorIsLockedSFX);
				this.amBusy = true;
				GameManager.TimeSlinger.FireTimer(1f, delegate()
				{
					this.amBusy = false;
				}, 0);
				return;
			}
			if (this.amOpend)
			{
				this.closeTheDoor();
				return;
			}
			if (!this.isLocked)
			{
				if (this.CheckPlayerIsBlockingDoorPath)
				{
					this.fireCheckForPlayer = true;
					return;
				}
				this.openTheDoor();
				return;
			}
			else
			{
				this.myAudioHub.PlaySound(this.DoorIsLockedSFX);
				this.amBusy = true;
				GameManager.TimeSlinger.FireTimer(1f, delegate()
				{
					this.amBusy = false;
				}, 0);
			}
		}
	}

	private void rightClickAction()
	{
		if (!this.amBusy && this.DoorLockable)
		{
			this.amBusy = true;
			this.myInteractionHook.MyBoxCollider.enabled = false;
			if (this.isLocked)
			{
				this.myAudioHub.PlaySound(this.UnLockDoorSFX);
				this.doorLockAnimation.DORestartById("unlock");
			}
			else
			{
				this.myAudioHub.PlaySound(this.LockDoorSFX);
				this.doorLockAnimation.DORestartById("lock");
			}
		}
	}

	private void openTheDoor()
	{
		if (this.doorOpenAnimations != null)
		{
			this.DoorOpenEvent.Invoke();
			this.myAudioHub.PlaySound(this.OpenSFX);
			for (int i = 0; i < this.doorOpenAnimations.Count; i++)
			{
				this.doorOpenAnimations[i].DORestartById("open");
			}
			this.amBusy = true;
			this.amOpend = true;
			if (this.SetDistanceLODs != null)
			{
				for (int j = 0; j < this.SetDistanceLODs.Length; j++)
				{
					this.SetDistanceLODs[j].OverwriteCulling = true;
				}
			}
		}
	}

	private void closeTheDoor()
	{
		if (this.doorCloseAnimations != null)
		{
			if (this.NoClip)
			{
				this.NoClipMesh.enabled = false;
			}
			this.DoorCloseEvent.Invoke();
			this.myAudioHub.PlaySound(this.CloseSFX);
			this.amBusy = true;
			this.amOpend = false;
			for (int i = 0; i < this.doorCloseAnimations.Count; i++)
			{
				this.doorCloseAnimations[i].DORestartById("close");
			}
		}
	}

	private void updatePlayerDistance()
	{
		this.playersCurrentDistance = (base.transform.position - this.mainCamera.transform.position).magnitude;
	}

	[ContextMenu("Reset Sphere")]
	private void resetSphere()
	{
		this.CheckForPlayerFirePOS = base.transform.position;
	}

	[ContextMenu("Recalculate Door Bounds")]
	private void calcDoorBounds()
	{
		this.doorMeshBounds = this.NoClipMesh.gameObject.GetComponent<Renderer>().bounds;
		this.doorMeshBounds.size = this.doorMeshBounds.size * 1.5f;
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myAudioHub = base.GetComponent<AudioHubObject>();
		if (!this.MakeManual)
		{
			this.myInteractionHook.LeftClickAction += this.leftClickAction;
			this.myInteractionHook.RightClickAction += this.rightClickAction;
		}
		this.doorMeshBounds = this.NoClipMesh.gameObject.GetComponent<Renderer>().bounds;
		this.doorMeshBounds.size = this.doorMeshBounds.size * 1.5f;
		this.lucasDoor = false;
	}

	private void Start()
	{
		this.mainCamera = Camera.main;
		if (this.MakeManual)
		{
			this.myInteractionHook.ForceLock = true;
		}
	}

	private void OnEnable()
	{
		base.InvokeRepeating("updatePlayerDistance", 0f, this.updateDelay);
	}

	private void OnDisable()
	{
		base.CancelInvoke("updatePlayerDistance");
	}

	private void Update()
	{
		if (!this.MakeManual)
		{
			this.myInteractionHook.ForceLock = this.amBusy;
		}
		if (this.checkDoorCloseRange && this.playersCurrentDistance > this.closeDistance)
		{
			this.checkDoorCloseRange = false;
			if (!this.lucasDoor)
			{
				this.closeTheDoor();
			}
		}
		if (this.checkColliderRange)
		{
			if (!this.doorMeshBounds.Contains(this.mainCamera.transform.position))
			{
				this.NoClipMesh.enabled = true;
				this.checkColliderRange = false;
				return;
			}
			ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).HardMove(this.moveMeToTrans.position);
		}
	}

	private void FixedUpdate()
	{
		if (this.fireCheckForPlayer)
		{
			RaycastHit raycastHit;
			if (Physics.CheckSphere(this.CheckForPlayerFirePOS + base.transform.forward * this.SphereCastRadius, this.SphereCastRadius, this.PlayerMask.value))
			{
				Vector3 destination = this.CheckForPlayerFirePOS + base.transform.forward * (this.SphereCastDistance + this.SphereCastRadius * 2f);
				ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).MoveOutOfTheWay(this.PlayerLookAtDirection, destination, CARDINAL_DIR.BACK);
			}
			else if (Physics.SphereCast(this.CheckForPlayerFirePOS + base.transform.forward * this.SphereCastRadius, this.SphereCastRadius, Vector3.forward, out raycastHit, this.SphereCastDistance, this.PlayerMask.value))
			{
				Vector3 destination2 = this.CheckForPlayerFirePOS + base.transform.forward * (this.SphereCastDistance + this.SphereCastRadius * 2f);
				ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).MoveOutOfTheWay(this.PlayerLookAtDirection, destination2, CARDINAL_DIR.BACK);
			}
			this.fireCheckForPlayer = false;
			this.openTheDoor();
		}
	}

	private void OnDestroy()
	{
		if (!this.MakeManual)
		{
			this.myInteractionHook.LeftClickAction -= this.leftClickAction;
			this.myInteractionHook.RightClickAction -= this.rightClickAction;
		}
		this.DoneWithAnimationEvents.RemoveAllListeners();
		this.DoorWasOpenedEvent.RemoveAllListeners();
		this.DoorWasClosedEvent.RemoveAllListeners();
		this.DoorWasOpenedEvent.RemoveAllListeners();
		this.DoorCloseEvent.RemoveAllListeners();
	}

	private void OnDrawGizmos()
	{
		if (this.CheckPlayerIsBlockingDoorPath)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(this.CheckForPlayerFirePOS + base.transform.forward * this.SphereCastRadius, this.SphereCastRadius);
			Gizmos.DrawLine(this.CheckForPlayerFirePOS + base.transform.forward * this.SphereCastRadius, this.CheckForPlayerFirePOS + base.transform.forward * (this.SphereCastDistance + this.SphereCastRadius));
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(this.CheckForPlayerFirePOS + base.transform.forward * (this.SphereCastDistance + this.SphereCastRadius), this.SphereCastRadius);
		}
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(this.doorMeshBounds.center, this.doorMeshBounds.size);
	}

	public void ToggleLock()
	{
		this.rightClickAction();
	}

	public bool MakeManual;

	public bool NoClip;

	public MeshCollider NoClipMesh;

	public DistanceLOD[] SetDistanceLODs;

	[SerializeField]
	private float updateDelay = 0.1f;

	[SerializeField]
	private float closeDistance = 1f;

	[SerializeField]
	private Transform moveMeToTrans;

	public bool DoorLockable;

	public bool CheckPlayerIsBlockingDoorPath;

	public LayerMask PlayerMask;

	public Vector3 CheckForPlayerFirePOS = Vector3.zero;

	public float SphereCastRadius = 0.2f;

	public float SphereCastDistance = 0.2f;

	public float PlayerLookAtDirection;

	public Transform DoorTransform;

	public Transform DoorMeshTransform;

	public AudioFileDefinition OpenSFX;

	public AudioFileDefinition CloseSFX;

	public AudioFileDefinition LockDoorSFX;

	public AudioFileDefinition UnLockDoorSFX;

	public AudioFileDefinition DoorIsLockedSFX;

	public UnityEvent DoneWithAnimationEvents;

	public UnityEvent DoorWasOpenedEvent;

	public UnityEvent DoorWasClosedEvent;

	public UnityEvent DoorOpenEvent;

	public UnityEvent DoorCloseEvent;

	private InteractionHook myInteractionHook;

	private AudioHubObject myAudioHub;

	private List<DOTweenAnimation> doorOpenAnimations = new List<DOTweenAnimation>(5);

	private List<DOTweenAnimation> doorCloseAnimations = new List<DOTweenAnimation>(5);

	private DOTweenAnimation doorLockAnimation;

	private DOTweenAnimation doorUnLockAnimation;

	private bool fireCheckForPlayer;

	private bool amOpend;

	private bool amBusy;

	private bool isLocked;

	private Timer autoCloseTimer;

	private Camera mainCamera;

	private float playersCurrentDistance;

	private bool checkDoorCloseRange;

	private bool checkColliderRange;

	private bool lockoutAutoClose;

	private Bounds doorMeshBounds;

	private bool lucasDoor;
}
