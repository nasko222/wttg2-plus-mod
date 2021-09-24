using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RemoteVPNPlacementBehaviour : MonoBehaviour
{
	private void takeInput()
	{
		if (!this.gameIsPaused && this.canPlaceRemote && CrossPlatformInputManager.GetButtonDown("LeftClick"))
		{
			this.placeRemoteVPN();
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

	private void triggerPlacementMode(RemoteVPNObject TheRemoteVPN)
	{
		this.currentRemoteVPNBeingPlaced = TheRemoteVPN;
		this.remoteVPNObjectMeshRenderer.enabled = true;
	}

	private void placeRemoteVPN()
	{
		if (this.currentRemoteVPNBeingPlaced != null)
		{
			this.currentRemoteVPNBeingPlaced.PlaceMe(this.remoteVPNObject.transform.position, this.remoteVPNObject.transform.rotation.eulerAngles);
			this.remoteVPNObjectMeshRenderer.enabled = false;
		}
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		GameManager.ManagerSlinger.RemoteVPNManager.EnteredPlacementMode += this.triggerPlacementMode;
	}

	private void theGameIsLive()
	{
		this.gameIsLive = true;
		GameManager.StageManager.TheGameIsLive -= this.theGameIsLive;
	}

	private void Awake()
	{
		this.remoteVPNObjectMeshRenderer = this.remoteVPNObject.GetComponent<MeshRenderer>();
		this.remoteVPNObjectMeshRenderer.enabled = false;
		this.remoteVPNPlacementBeh = this.remoteVPNObject.GetComponent<RemoteVPNPlacementPreview>();
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.TheGameIsLive += this.theGameIsLive;
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
			if (StateManager.PlayerState == PLAYER_STATE.REMOTE_VPN_PLACEMENT)
			{
				RaycastHit raycastHit;
				if (!Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit, 1f))
				{
					this.remoteVPNObject.transform.position = Vector3.zero;
					this.canPlaceRemote = false;
					return;
				}
				bool flag = false;
				if (raycastHit.collider != null && raycastHit.collider.GetComponent<StickySurface>() != null)
				{
					flag = true;
				}
				if (flag)
				{
					float vpnvalues = GameManager.WorldManager.GetVPNValues(this.remoteVPNObject.transform);
					if (vpnvalues > 2700f)
					{
						if (ModsManager.ShowGodSpot)
						{
							this.remoteVPNPlacementBeh.GoBlue();
						}
						else
						{
							this.remoteVPNPlacementBeh.GoRed();
						}
					}
					else if (vpnvalues >= 1000f && vpnvalues < 2700f)
					{
						this.remoteVPNPlacementBeh.GoRed();
					}
					else if (vpnvalues < 1000f && vpnvalues > 600f)
					{
						this.remoteVPNPlacementBeh.GoOOrange();
					}
					else if (vpnvalues < 600f && vpnvalues > 300f)
					{
						this.remoteVPNPlacementBeh.GoOrange();
					}
					else if (vpnvalues < 300f && vpnvalues > 150f)
					{
						this.remoteVPNPlacementBeh.GoLime();
					}
					else
					{
						this.remoteVPNPlacementBeh.GoGreen();
					}
					this.remoteVPNObject.transform.position = raycastHit.point;
					this.remoteVPNObject.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
					this.canPlaceRemote = true;
					return;
				}
				this.remoteVPNObject.transform.position = Vector3.zero;
				this.canPlaceRemote = false;
				return;
			}
			else
			{
				this.canPlaceRemote = false;
			}
		}
	}

	private void OnDestroy()
	{
		GameManager.PauseManager.GamePaused -= this.playerPausedGame;
		GameManager.PauseManager.GameUnPaused -= this.playerUnPausedGame;
	}

	[SerializeField]
	private GameObject remoteVPNObject;

	private MeshRenderer remoteVPNObjectMeshRenderer;

	private bool gameIsLive;

	private bool gameIsPaused;

	private bool canPlaceRemote;

	private RemoteVPNObject currentRemoteVPNBeingPlaced;

	private RemoteVPNPlacementPreview remoteVPNPlacementBeh;
}
