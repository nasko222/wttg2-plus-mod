using System;
using UnityEngine;

public class SecretController : moveableController
{
	public void Release()
	{
		this.MyCamera.transform.SetParent(this.MouseRotatingObject.transform);
		this.MyCamera.transform.localPosition = this.DefaultCameraPOS;
		this.MyCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
		if (!this.MouseCaptureInit)
		{
			base.Init();
		}
		this.Active = true;
		this.MyState = GAME_CONTROLLER_STATE.IDLE;
		base.SetMasterLock(false);
	}

	protected new void Awake()
	{
		SecretController.Ins = this;
		base.Awake();
		ControllerManager.Add(this);
	}

	protected new void Start()
	{
		base.Start();
	}

	protected new void Update()
	{
		if (StateManager.GameState == GAME_STATE.PAUSED)
		{
			if (!this.lockControl)
			{
				base.SetMasterLock(true);
			}
			if (Input.GetKeyDown(KeyCode.Q))
			{
				Application.Quit();
			}
		}
		else if (this.lockControl)
		{
			base.SetMasterLock(false);
		}
		base.Update();
	}

	protected new void OnDestroy()
	{
		ControllerManager.Remove(this.Controller);
		base.OnDestroy();
	}

	public static SecretController Ins;
}
