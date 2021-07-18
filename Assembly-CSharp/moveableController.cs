using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

[RequireComponent(typeof(CharacterController))]
public class moveableController : mouseableController
{
	protected new void Init()
	{
		base.Init();
		this.MyCharcterController = base.GetComponent<CharacterController>();
		this.duckCameraDiff = this.DuckCameraPOS - this.DefaultCameraPOS;
		this.currentInput = Vector2.zero;
		this.currentMoveDir = Vector3.zero;
		this.stepCycle = 0f;
		this.nextStep = this.stepCycle / 2f;
		if (this.cameraIsSet)
		{
			this.HeadBob.Setup(this.MyCamera, this.StepInterval);
			this.MoveableControllerInit = true;
		}
	}

	private void getInput(out float setSpeed)
	{
		float num = 0f;
		float num2 = 0f;
		float axis = CrossPlatformInputManager.GetAxis("Horizontal");
		float axis2 = CrossPlatformInputManager.GetAxis("Vertical");
		if (!this.DisableRun)
		{
			num = CrossPlatformInputManager.GetAxis("Run");
		}
		if (!this.DisableDuck)
		{
			num2 = CrossPlatformInputManager.GetAxis("Duck");
		}
		this.currentInput = new Vector2(axis, axis2);
		if (this.currentInput.sqrMagnitude > 1f)
		{
			this.currentInput.Normalize();
		}
		if (num2 > 0f)
		{
			this.MyState = GAME_CONTROLLER_STATE.DUCKING;
			setSpeed = this.DuckSpeed;
		}
		else
		{
			setSpeed = Mathf.Max(Mathf.Abs(axis2), Mathf.Abs(axis));
			setSpeed = ((axis2 <= 0f || num <= 0f) ? (this.WalkSpeed * setSpeed) : (this.RunSpeed * Mathf.Abs(axis2)));
			if (setSpeed > this.WalkSpeed)
			{
				this.MyState = GAME_CONTROLLER_STATE.RUNING;
			}
			else if (setSpeed > 0f)
			{
				this.MyState = GAME_CONTROLLER_STATE.WALKING;
			}
			else
			{
				this.MyState = GAME_CONTROLLER_STATE.IDLE;
			}
		}
	}

	private void UpdateCameraPOS(float setSpeed)
	{
		if (this.UseHeadBob)
		{
			GAME_CONTROLLER_STATE myState = this.MyState;
			if (myState != GAME_CONTROLLER_STATE.RUNING)
			{
				if (myState != GAME_CONTROLLER_STATE.DUCKING)
				{
					Vector3 localPosition;
					if (this.MyCharcterController.velocity.magnitude > 0f && this.MyCharcterController.isGrounded)
					{
						localPosition = this.HeadBob.DoHeadBob(this.MyCharcterController.velocity.magnitude + setSpeed * this.WalkStepLengten, false);
					}
					else
					{
						localPosition = this.MyCamera.transform.localPosition;
						localPosition.y = this.DefaultCameraPOS.y;
					}
					this.MyCamera.transform.localPosition = localPosition;
				}
				else
				{
					float axis = CrossPlatformInputManager.GetAxis("Duck");
					Vector3 localPosition;
					localPosition.x = this.DefaultCameraPOS.x + this.duckCameraDiff.x * axis;
					localPosition.y = this.DefaultCameraPOS.y + this.duckCameraDiff.y * axis;
					localPosition.z = this.DefaultCameraPOS.z + this.duckCameraDiff.z * axis;
					this.MyCamera.transform.localPosition = localPosition;
				}
			}
			else
			{
				Vector3 localPosition;
				if (this.MyCharcterController.velocity.magnitude > 0f && this.MyCharcterController.isGrounded)
				{
					localPosition = this.HeadBob.DoHeadBob(this.MyCharcterController.velocity.magnitude + setSpeed * this.RunStepLenghten, true);
				}
				else
				{
					localPosition = this.MyCamera.transform.localPosition;
					localPosition.y = this.DefaultCameraPOS.y;
				}
				this.MyCamera.transform.localPosition = localPosition;
			}
		}
		else
		{
			GAME_CONTROLLER_STATE myState2 = this.MyState;
			if (myState2 != GAME_CONTROLLER_STATE.DUCKING)
			{
				this.MyCamera.transform.localPosition = this.DefaultCameraPOS;
			}
			else
			{
				float axis2 = CrossPlatformInputManager.GetAxis("Duck");
				Vector3 localPosition;
				localPosition.x = this.DefaultCameraPOS.x + this.duckCameraDiff.x * axis2;
				localPosition.y = this.DefaultCameraPOS.y + this.duckCameraDiff.y * axis2;
				localPosition.z = this.DefaultCameraPOS.z + this.duckCameraDiff.z * axis2;
				this.MyCamera.transform.localPosition = localPosition;
			}
		}
	}

	private void processStep(float setSpeed)
	{
		if (this.MyCharcterController.velocity.sqrMagnitude > 0f && (this.currentInput.x != 0f || this.currentInput.y != 0f))
		{
			GAME_CONTROLLER_STATE myState = this.MyState;
			if (myState != GAME_CONTROLLER_STATE.WALKING)
			{
				if (myState != GAME_CONTROLLER_STATE.DUCKING)
				{
					if (myState == GAME_CONTROLLER_STATE.RUNING)
					{
						this.stepCycle += (this.MyCharcterController.velocity.magnitude + setSpeed * this.RunStepLenghten) * Time.fixedDeltaTime;
					}
				}
				else
				{
					this.stepCycle += (this.MyCharcterController.velocity.magnitude + setSpeed * this.DuckStepLenghten) * Time.fixedDeltaTime;
				}
			}
			else
			{
				this.stepCycle += (this.MyCharcterController.velocity.magnitude + setSpeed * this.WalkStepLengten) * Time.fixedDeltaTime;
			}
		}
	}

	private void playFootStepAudio()
	{
		if (this.MyCharcterController.isGrounded)
		{
			GAME_CONTROLLER_STATE myState = this.MyState;
			if (myState != GAME_CONTROLLER_STATE.WALKING)
			{
				if (myState != GAME_CONTROLLER_STATE.DUCKING)
				{
					if (myState == GAME_CONTROLLER_STATE.RUNING)
					{
						int num = UnityEngine.Random.Range(1, this.RunFootStepSFXS.Length);
						GameManager.AudioSlinger.PlaySoundWithWildPitch(this.RunFootStepSFXS[num], 0.85f, 1.1f);
						AudioFileDefinition audioFileDefinition = this.RunFootStepSFXS[num];
						this.RunFootStepSFXS[num] = this.RunFootStepSFXS[0];
						this.RunFootStepSFXS[0] = audioFileDefinition;
					}
				}
				else
				{
					int num = UnityEngine.Random.Range(1, this.DuckFootStepSFXS.Length);
					GameManager.AudioSlinger.PlaySoundWithWildPitch(this.DuckFootStepSFXS[num], 0.85f, 1.1f);
					AudioFileDefinition audioFileDefinition = this.DuckFootStepSFXS[num];
					this.DuckFootStepSFXS[num] = this.DuckFootStepSFXS[0];
					this.DuckFootStepSFXS[0] = audioFileDefinition;
				}
			}
			else
			{
				int num = UnityEngine.Random.Range(1, this.WalkFootStepSFXS.Length);
				GameManager.AudioSlinger.PlaySoundWithWildPitch(this.WalkFootStepSFXS[num], 0.85f, 1.1f);
				AudioFileDefinition audioFileDefinition = this.WalkFootStepSFXS[num];
				this.WalkFootStepSFXS[num] = this.WalkFootStepSFXS[0];
				this.WalkFootStepSFXS[0] = audioFileDefinition;
			}
		}
	}

	private void playSurfaceFootStepAudio(SurfaceTypeObject STO)
	{
		if (this.MyCharcterController.isGrounded)
		{
			GAME_CONTROLLER_STATE myState = this.MyState;
			if (myState != GAME_CONTROLLER_STATE.WALKING)
			{
				if (myState != GAME_CONTROLLER_STATE.DUCKING)
				{
					if (myState == GAME_CONTROLLER_STATE.RUNING)
					{
						int index = UnityEngine.Random.Range(1, STO.MyFootStepSFXS.RunFootStepSFXS.Count);
						GameManager.AudioSlinger.PlaySoundWithWildPitch(STO.MyFootStepSFXS.RunFootStepSFXS[index], 0.85f, 1.1f);
						AudioFileDefinition value = STO.MyFootStepSFXS.RunFootStepSFXS[index];
						STO.MyFootStepSFXS.RunFootStepSFXS[index] = STO.MyFootStepSFXS.RunFootStepSFXS[0];
						STO.MyFootStepSFXS.RunFootStepSFXS[0] = value;
					}
				}
				else
				{
					int index = UnityEngine.Random.Range(1, STO.MyFootStepSFXS.DuckFootStepSFXS.Count);
					GameManager.AudioSlinger.PlaySoundWithWildPitch(STO.MyFootStepSFXS.DuckFootStepSFXS[index], 0.85f, 1.1f);
					AudioFileDefinition value = STO.MyFootStepSFXS.DuckFootStepSFXS[index];
					STO.MyFootStepSFXS.DuckFootStepSFXS[index] = STO.MyFootStepSFXS.DuckFootStepSFXS[0];
					STO.MyFootStepSFXS.DuckFootStepSFXS[0] = value;
				}
			}
			else
			{
				int index = UnityEngine.Random.Range(1, STO.MyFootStepSFXS.WalkFootStepSFXS.Count);
				GameManager.AudioSlinger.PlaySoundWithWildPitch(STO.MyFootStepSFXS.WalkFootStepSFXS[index], 0.85f, 1.1f);
				AudioFileDefinition value = STO.MyFootStepSFXS.WalkFootStepSFXS[index];
				STO.MyFootStepSFXS.WalkFootStepSFXS[index] = STO.MyFootStepSFXS.WalkFootStepSFXS[0];
				STO.MyFootStepSFXS.WalkFootStepSFXS[0] = value;
			}
		}
	}

	protected new void Awake()
	{
		base.Awake();
	}

	protected new void Start()
	{
		base.Start();
	}

	protected new void Update()
	{
		base.Update();
	}

	private void FixedUpdate()
	{
		if (this.MoveableControllerInit && !this.lockControl)
		{
			this.getInput(out this.speed);
			this.theMove = base.transform.forward * this.currentInput.y + base.transform.right * (this.currentInput.x * 1f);
			Physics.SphereCast(base.transform.position, this.MyCharcterController.radius, Vector3.down, out this.hitInfo, this.MyCharcterController.height / 2f, this.hitLayers.value);
			this.theMove = Vector3.ProjectOnPlane(this.theMove, this.hitInfo.normal).normalized;
			this.currentMoveDir.x = this.theMove.x * this.speed;
			this.currentMoveDir.z = this.theMove.z * this.speed;
			if (this.MyCharcterController.isGrounded)
			{
				this.currentMoveDir.y = -this.StickToGroundForce;
			}
			else
			{
				this.currentMoveDir += Physics.gravity * this.GravityMultiplier * Time.fixedDeltaTime;
			}
			this.MyCharcterController.Move(this.currentMoveDir * Time.fixedDeltaTime);
			this.UpdateCameraPOS(this.speed);
			this.processStep(this.speed);
			if (this.hitInfo.collider != null)
			{
				SurfaceTypeObject component = this.hitInfo.collider.GetComponent<SurfaceTypeObject>();
				if (this.stepCycle > this.nextStep)
				{
					this.nextStep = this.stepCycle + this.StepInterval;
					if (component != null && component.HasCustomFootStepSFXS)
					{
						this.playSurfaceFootStepAudio(component);
					}
					else
					{
						this.playFootStepAudio();
					}
				}
			}
		}
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
	}

	public bool DisableRun;

	public bool DisableDuck;

	public Vector3 DuckCameraPOS;

	public Vector3 DefaultCameraPOS;

	public Vector3 DefaultCameraROT;

	public float DuckSpeed = 3f;

	public float WalkSpeed = 5f;

	public float RunSpeed = 10f;

	[Range(0f, 1f)]
	public float RunStepLenghten;

	[Range(0f, 1f)]
	public float WalkStepLengten;

	public float DuckStepLenghten = 3f;

	public float GravityMultiplier = 2f;

	public float StickToGroundForce = 10f;

	public bool UseHeadBob;

	public float StepInterval;

	public CurveControlledBob HeadBob = new CurveControlledBob();

	[SerializeField]
	private LayerMask hitLayers;

	public AUDIO_HUB MyFootAudioHub;

	public AudioFileDefinition[] WalkFootStepSFXS;

	public AudioFileDefinition[] RunFootStepSFXS;

	public AudioFileDefinition[] DuckFootStepSFXS;

	protected CharacterController MyCharcterController;

	protected bool MoveableControllerInit;

	private Vector2 currentInput;

	private Vector3 currentMoveDir;

	private Vector3 duckCameraDiff;

	private Vector3 theMove;

	private float stepCycle;

	private float nextStep;

	private float speed;

	private RaycastHit hitInfo;
}
