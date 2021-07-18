using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MotionSensorPlacementBehaviour : MonoBehaviour
{
	private void takeInput()
	{
		if (!this.gameIsPaused && this.canPlaceSensor && CrossPlatformInputManager.GetButtonDown("LeftClick"))
		{
			this.placeMotionSensor();
		}
	}

	private void playerPausedGame()
	{
		this.gameIsPaused = true;
	}

	private void playerUnPausedGame()
	{
		this.gameIsPaused = false;
	}

	private void triggerPlacementMode(MotionSensorObject TheMotionSensor)
	{
		this.currentMotionSensorBeingPlaced = TheMotionSensor;
		this.motionSensorObjectMeshRenderer.enabled = true;
	}

	private void placeMotionSensor()
	{
		if (this.currentMotionSensorBeingPlaced != null)
		{
			this.currentMotionSensorBeingPlaced.PlaceMe(this.motionSensorObject.transform.position, this.motionSensorObject.transform.rotation.eulerAngles);
			this.motionSensorObjectMeshRenderer.enabled = false;
		}
	}

	private void gameIsNowLive()
	{
		this.gameIsLive = true;
		GameManager.StageManager.TheGameIsLive -= this.gameIsNowLive;
		GameManager.ManagerSlinger.MotionSensorManager.EnteredPlacementMode += this.triggerPlacementMode;
	}

	private void Awake()
	{
		this.motionSensorObjectMeshRenderer = this.motionSensorObject.GetComponent<MeshRenderer>();
		this.motionSensorObjectMeshRenderer.enabled = false;
		GameManager.StageManager.TheGameIsLive += this.gameIsNowLive;
	}

	private void Start()
	{
		GameManager.PauseManager.GamePaused += this.playerPausedGame;
		GameManager.PauseManager.GameUnPaused += this.playerUnPausedGame;
	}

	private void Update()
	{
		this.takeInput();
	}

	private void FixedUpdate()
	{
		if (this.gameIsLive)
		{
			if (StateManager.PlayerState == PLAYER_STATE.MOTION_SENSOR_PLACEMENT)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit, 1f))
				{
					bool flag = false;
					if (raycastHit.collider != null)
					{
						StickySurface component = raycastHit.collider.GetComponent<StickySurface>();
						if (component != null)
						{
							flag = true;
						}
					}
					if (flag)
					{
						this.motionSensorObject.transform.position = raycastHit.point;
						this.motionSensorObject.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
						this.canPlaceSensor = true;
					}
					else
					{
						this.motionSensorObject.transform.position = Vector3.zero;
						this.canPlaceSensor = false;
					}
				}
				else
				{
					this.motionSensorObject.transform.position = Vector3.zero;
					this.canPlaceSensor = false;
				}
			}
			else
			{
				this.canPlaceSensor = false;
			}
		}
	}

	private void OnDestroy()
	{
		GameManager.PauseManager.GamePaused -= this.playerPausedGame;
		GameManager.PauseManager.GameUnPaused -= this.playerUnPausedGame;
	}

	[SerializeField]
	private GameObject motionSensorObject;

	private MeshRenderer motionSensorObjectMeshRenderer;

	private bool gameIsLive;

	private bool gameIsPaused;

	private bool canPlaceSensor;

	private MotionSensorObject currentMotionSensorBeingPlaced;
}
