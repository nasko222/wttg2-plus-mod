using System;
using UnityEngine;

public class mouseableController : baseController
{
	protected void Init()
	{
		if (this.cameraIsSet)
		{
			this.MyMouseCapture.Init(this.MouseRotatingObject, this.MyCamera.gameObject);
			this.MouseCaptureInit = true;
		}
	}

	private void RotateView()
	{
		if (this.MouseCaptureInit && !this.lockMouse)
		{
			this.MyMouseCapture.LookRotation();
		}
	}

	private void playerUpdateMouseSens(int SetValue)
	{
		this.MyMouseCapture.XSensitivity = (float)SetValue;
		this.MyMouseCapture.YSensitivity = (float)SetValue;
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		if (this.useOptDataForMouseSens)
		{
			PauseMenuHook.Ins.UpdatedMouseSens.Event += this.playerUpdateMouseSens;
		}
	}

	protected new void Awake()
	{
		base.Awake();
		if (this.useOptDataForMouseSens)
		{
			this.MyMouseCapture.XSensitivity = (float)OptionDataHook.Ins.Options.MouseSens;
			this.MyMouseCapture.YSensitivity = (float)OptionDataHook.Ins.Options.MouseSens;
		}
		if (GameManager.StageManager != null)
		{
			GameManager.StageManager.Stage += this.stageMe;
		}
	}

	protected new void Start()
	{
		base.Start();
	}

	protected new void Update()
	{
		base.Update();
		this.RotateView();
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
	}

	public GameObject MouseRotatingObject;

	public mouseCapture MyMouseCapture;

	[SerializeField]
	private bool useOptDataForMouseSens;

	protected bool MouseCaptureInit;

	private Options myOptData;
}
