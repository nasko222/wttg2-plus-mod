using System;

[Serializable]
public class CameraData
{
	public CameraData(CAMERA_ID SetCameraID, Vect3 SetLastPosition, Vect3 SetLastRotation, float SetLastFOV)
	{
		this.MyID = SetCameraID;
		this.LastPosition = SetLastPosition;
		this.LastRotation = SetLastRotation;
		this.LastFOV = SetLastFOV;
	}

	public CAMERA_ID MyID;

	public Vect3 LastPosition;

	public Vect3 LastRotation;

	public float LastFOV;
}
