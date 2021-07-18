using System;
using UnityEngine;

public abstract class baseController : MonoBehaviour
{
	public void SetMasterLock(bool setLock)
	{
		this.masterLock = setLock;
		this.lockControl = setLock;
		this.lockMouse = setLock;
		this.overWriteLocks = setLock;
		if (setLock)
		{
			this.MyState = GAME_CONTROLLER_STATE.LOCKED;
		}
	}

	private void PlayerHitPause()
	{
		if (!this.overWriteLocks)
		{
			this.lockControl = true;
			this.lockMouse = true;
			this.MyState = GAME_CONTROLLER_STATE.LOCKED;
		}
	}

	private void PlayerHitUnPause()
	{
		if (!this.overWriteLocks)
		{
			this.lockControl = false;
			this.lockMouse = false;
			this.MyState = GAME_CONTROLLER_STATE.IDLE;
		}
	}

	private void updateMyData()
	{
		if (this.myData != null && StateManager.PlayerState != PLAYER_STATE.BUSY)
		{
			this.myData.MyState = (int)this.MyState;
			this.myData.Active = this.Active;
			this.myData.POSX = base.transform.position.x;
			this.myData.POSY = base.transform.position.y;
			this.myData.POSZ = base.transform.position.z;
			this.myData.ROTX = base.transform.rotation.eulerAngles.x;
			this.myData.ROTY = base.transform.rotation.eulerAngles.y;
			this.myData.ROTZ = base.transform.rotation.eulerAngles.z;
			DataManager.Save<BaseControllerData>(this.myData);
		}
	}

	private void gameStaging()
	{
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<BaseControllerData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new BaseControllerData(this.myID);
			this.myData.MyState = (int)this.MyState;
			if (this.Controller == GameManager.StageManager.DefaultController)
			{
				this.myData.Active = true;
			}
			else
			{
				this.myData.Active = false;
			}
			this.myData.POSX = base.transform.position.x;
			this.myData.POSY = base.transform.position.y;
			this.myData.POSZ = base.transform.position.z;
			this.myData.ROTX = base.transform.rotation.eulerAngles.x;
			this.myData.ROTY = base.transform.rotation.eulerAngles.y;
			this.myData.ROTZ = base.transform.rotation.eulerAngles.z;
		}
		if (!DataManager.ContinuedGame)
		{
			this.MyState = (GAME_CONTROLLER_STATE)this.myData.MyState;
			this.Active = this.myData.Active;
		}
		GameManager.StageManager.Stage -= this.stageMe;
		GameManager.PauseManager.GamePaused += this.PlayerHitPause;
		GameManager.PauseManager.GameUnPaused += this.PlayerHitUnPause;
		this.PostStage.Execute();
	}

	private void gameLive()
	{
		base.transform.position = new Vector3(this.myData.POSX, this.myData.POSY, this.myData.POSZ);
		base.transform.rotation = Quaternion.Euler(new Vector3(this.myData.ROTX, this.myData.ROTY, this.myData.ROTZ));
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
		this.PostLive.Execute();
	}

	protected virtual void Awake()
	{
		this.myID = (int)this.Controller;
		if (GameManager.StageManager != null)
		{
			GameManager.StageManager.Stage += this.stageMe;
			GameManager.StageManager.TheGameIsLive += this.gameLive;
		}
		if (CameraManager.Get(this.CameraIControl, out this.MyCamera))
		{
			this.cameraIsSet = true;
		}
		this.MyState = GAME_CONTROLLER_STATE.LOCKED;
		this.Active = false;
		this.SetMasterLock(true);
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnDestroy()
	{
		base.CancelInvoke("updateMyData");
		GameManager.PauseManager.GamePaused -= this.PlayerHitPause;
		GameManager.PauseManager.GameUnPaused -= this.PlayerHitUnPause;
	}

	public GAME_CONTROLLER Controller;

	public CAMERA_ID CameraIControl;

	public GAME_CONTROLLER_STATE MyState;

	public bool Active;

	protected CustomEvent PostStage = new CustomEvent(2);

	protected CustomEvent PostLive = new CustomEvent(2);

	protected Camera MyCamera;

	protected bool cameraIsSet;

	protected bool masterLock;

	protected bool lockMouse;

	protected bool lockControl;

	protected bool overWriteLocks;

	private int myID;

	private BaseControllerData myData;
}
