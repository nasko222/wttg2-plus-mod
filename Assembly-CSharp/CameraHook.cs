using System;
using UnityEngine;

public class CameraHook : MonoBehaviour
{
	public Transform CurrentParent
	{
		get
		{
			return this.lastParent;
		}
	}

	public void SetMyParent(Transform SetParent)
	{
		this.lastParent = SetParent;
		base.transform.SetParent(SetParent);
	}

	public void SwitchToGlobalParent()
	{
		base.transform.SetParent(this.GlobalParent);
	}

	public void ManualPushDataUpdate()
	{
		this.myData.POSX = base.transform.localPosition.x;
		this.myData.POSY = base.transform.localPosition.y;
		this.myData.POSZ = base.transform.localPosition.z;
		this.myData.ROTX = base.transform.localRotation.eulerAngles.x;
		this.myData.ROTY = base.transform.localRotation.eulerAngles.y;
		this.myData.ROTZ = base.transform.localRotation.eulerAngles.z;
		this.myData.FOV = this.MyCamera.fieldOfView;
	}

	private void updateMyData()
	{
		if (StateManager.PlayerState != PLAYER_STATE.BUSY)
		{
			this.myData.POSX = base.transform.localPosition.x;
			this.myData.POSY = base.transform.localPosition.y;
			this.myData.POSZ = base.transform.localPosition.z;
			this.myData.ROTX = base.transform.localRotation.eulerAngles.x;
			this.myData.ROTY = base.transform.localRotation.eulerAngles.y;
			this.myData.ROTZ = base.transform.localRotation.eulerAngles.z;
			this.myData.FOV = this.MyCamera.fieldOfView;
			DataManager.Save<CameraHookData>(this.myData);
		}
	}

	private void StageMe()
	{
		this.myData = DataManager.Load<CameraHookData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new CameraHookData(this.myID);
			this.myData.POSX = base.transform.localPosition.x;
			this.myData.POSY = base.transform.localPosition.y;
			this.myData.POSZ = base.transform.localPosition.z;
			this.myData.ROTX = base.transform.localRotation.eulerAngles.x;
			this.myData.ROTY = base.transform.localRotation.eulerAngles.y;
			this.myData.ROTZ = base.transform.localRotation.eulerAngles.z;
			this.myData.FOV = this.MyCamera.fieldOfView;
		}
		GameManager.StageManager.Stage -= this.StageMe;
	}

	private void gameLive()
	{
		base.transform.localPosition = new Vector3(this.myData.POSX, this.myData.POSY, this.myData.POSZ);
		base.transform.localRotation = Quaternion.Euler(new Vector3(this.myData.ROTX, this.myData.ROTY, this.myData.ROTZ));
		this.MyCamera.fieldOfView = this.myData.FOV;
		base.InvokeRepeating("updateMyData", 0f, 5f);
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
	}

	private void Awake()
	{
		this.MyCamera = base.GetComponent<Camera>();
		this.myID = (int)this.MyCameraID;
		CameraManager.Add(this.MyCameraID, base.GetComponent<Camera>());
		if (GameManager.StageManager != null)
		{
			GameManager.StageManager.Stage += this.StageMe;
			GameManager.StageManager.TheGameIsLive += this.gameLive;
		}
	}

	private void OnDestroy()
	{
		base.CancelInvoke("updateMyData");
		CameraManager.Remove(this.MyCameraID);
	}

	public CAMERA_ID MyCameraID;

	public Transform GlobalParent;

	private int myID;

	private Camera MyCamera;

	private Transform lastParent;

	private CameraHookData myData;
}
