using System;
using UnityEngine;

[Serializable]
public class mouseCapture
{
	public void Init(GameObject RotatingObject, GameObject RotatingCamera)
	{
		this.MyRotatingObject = RotatingObject;
		this.MyRotatingCamera = RotatingCamera;
		this.myRotatingObjectTargetRot = this.MyRotatingObject.transform.localRotation;
		this.myRotatingCameraTargetRot = this.MyRotatingCamera.transform.localRotation;
	}

	public void Init(Camera setCamera)
	{
		this.myCamera = setCamera;
		this.myRotatingCameraTargetRot = this.myCamera.transform.localRotation;
	}

	public void setCameraTargetRot(float setX = 0f)
	{
		this.myRotatingCameraTargetRot = Quaternion.Euler(setX, 0f, 0f);
	}

	public void setFullCameraTargetRot(Vector3 setRot)
	{
		this.myRotatingCameraTargetRot = Quaternion.Euler(setRot);
	}

	public void setRotatingObjectTargetRot(Vector3 setValue)
	{
		this.myRotatingObjectTargetRot = Quaternion.Euler(setValue);
	}

	public void setRotatingObjectRotation(Vector3 setvalue)
	{
		this.MyRotatingObject.transform.localRotation = Quaternion.Euler(setvalue);
	}

	public void setRestrictMovement(bool setValue)
	{
		this.setRestrictMovement(setValue, 0f, 0f, 0f, 0f);
	}

	public void setRestrictMovement(bool setValue, float setMinVX, float setMaxVX, float setMinHY, float setMaxHY)
	{
		this.restrictMovement = setValue;
		this.restrictHorz = setValue;
		this.restricVert = setValue;
		this.restrictMinVertX = setMinVX;
		this.restrictMaxVertX = setMaxVX;
		this.restrictMinHorzY = setMinHY;
		this.restrictMaxHorzY = setMaxHY;
	}

	public void setRestricVertMovement(bool setValue, float setMinVX, float setMaxVX)
	{
		this.restrictMovement = setValue;
		this.restricVert = setValue;
		this.restrictMinVertX = setMinVX;
		this.restrictMaxVertX = setMaxVX;
	}

	public void setRestricHorzMovement(bool setValue, float setMinHY, float setMaxHY)
	{
		this.restrictMovement = setValue;
		this.restrictHorz = setValue;
		this.restrictMinHorzY = setMinHY;
		this.restrictMaxHorzY = setMaxHY;
	}

	public void updateRestricVertMin(float setValue)
	{
		this.restrictMinVertX = setValue;
	}

	public void updateRestricVertMax(float setValue)
	{
		this.restrictMaxVertX = setValue;
	}

	public void updateRestricHorzMin(float setValue)
	{
		this.restrictMinHorzY = setValue;
	}

	public void updateRestricHorzMax(float setValue)
	{
		this.restrictMaxHorzY = setValue;
	}

	public void updateMinVertX(float setValue)
	{
		this.MinVertX = setValue;
	}

	public void setLockCharROT(bool setValue)
	{
		this.lockCharRot = setValue;
	}

	public void LookRotation()
	{
		float num = Input.GetAxis("Mouse Y") * this.XSensitivity;
		float y = Input.GetAxis("Mouse X") * this.YSensitivity;
		this.myRotatingObjectTargetRot *= Quaternion.Euler(0f, y, 0f);
		this.myRotatingCameraTargetRot *= Quaternion.Euler(-num, 0f, 0f);
		if (this.clampVerticalRotation)
		{
			this.myRotatingCameraTargetRot = this.ClampRotationAroundXAxis(this.myRotatingCameraTargetRot);
		}
		if (this.clampHorzRotation)
		{
			this.myRotatingObjectTargetRot = this.ClampRotationAroundYAxis(this.myRotatingObjectTargetRot);
		}
		if (this.restrictMovement)
		{
			if (this.restricVert)
			{
				this.myRotatingCameraTargetRot = this.ClampRotationAroundXAxis(this.myRotatingCameraTargetRot);
			}
			if (this.restrictHorz)
			{
				this.myRotatingObjectTargetRot = this.ClampRotationAroundYAxis(this.myRotatingObjectTargetRot);
			}
		}
		if (this.smooth)
		{
			if (!this.lockCharRot)
			{
				this.MyRotatingObject.transform.localRotation = Quaternion.Slerp(this.MyRotatingObject.transform.localRotation, this.myRotatingObjectTargetRot, this.smoothTime * Time.deltaTime);
			}
			this.MyRotatingCamera.transform.localRotation = Quaternion.Slerp(this.MyRotatingCamera.transform.localRotation, this.myRotatingCameraTargetRot, this.smoothTime * Time.deltaTime);
		}
		else
		{
			if (!this.lockCharRot)
			{
				this.MyRotatingObject.transform.localRotation = this.myRotatingObjectTargetRot;
			}
			this.MyRotatingCamera.transform.localRotation = this.myRotatingCameraTargetRot;
		}
	}

	public Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;
		float num = 114.59156f * Mathf.Atan(q.x);
		if (this.restricVert)
		{
			num = Mathf.Clamp(num, this.restrictMinVertX, this.restrictMaxVertX);
		}
		else
		{
			num = Mathf.Clamp(num, this.MinVertX, this.MaxVertX);
		}
		q.x = Mathf.Tan(0.008726646f * num);
		return q;
	}

	public Quaternion ClampRotationAroundYAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;
		float num = 114.59156f * Mathf.Atan(q.y);
		if (this.restrictHorz)
		{
			num = Mathf.Clamp(num, this.restrictMinHorzY, this.restrictMaxHorzY);
		}
		else
		{
			num = Mathf.Clamp(num, this.MinHorzY, this.MaxHorzY);
		}
		q.y = Mathf.Tan(0.008726646f * num);
		return q;
	}

	public float XSensitivity = 2f;

	public float YSensitivity = 2f;

	public bool clampVerticalRotation = true;

	public bool clampHorzRotation;

	public float MinVertX = -90f;

	public float MaxVertX = 90f;

	public float MinHorzY = -180f;

	public float MaxHorzY = 180f;

	public bool smooth;

	public float smoothTime = 5f;

	private bool restrictMovement;

	private bool restrictHorz;

	private bool restricVert;

	private bool lockCharRot;

	private float restrictMinVertX;

	private float restrictMaxVertX;

	private float restrictMinHorzY;

	private float restrictMaxHorzY;

	private Quaternion myRotatingObjectTargetRot;

	private Quaternion myRotatingCameraTargetRot;

	private GameObject MyRotatingObject;

	private GameObject MyRotatingCamera;

	private Camera myCamera;
}
