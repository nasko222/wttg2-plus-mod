using System;
using System.Collections.Generic;
using UnityEngine;

public static class CameraManager
{
	public static void Add(CAMERA_ID SetCameraID, Camera SetCamera)
	{
		if (!CameraManager._cameras.ContainsKey(SetCameraID))
		{
			CameraManager._cameras.Add(SetCameraID, SetCamera);
		}
	}

	public static bool Get(CAMERA_ID CameraToGetID, out Camera ReturnCamera)
	{
		return CameraManager._cameras.TryGetValue(CameraToGetID, out ReturnCamera);
	}

	public static CameraHook GetCameraHook(CAMERA_ID CameraToGetID)
	{
		Camera camera;
		if (CameraManager._cameras.TryGetValue(CameraToGetID, out camera))
		{
			return camera.gameObject.GetComponent<CameraHook>();
		}
		return null;
	}

	public static void Remove(CAMERA_ID CameraToRemove)
	{
		CameraManager._cameras.Remove(CameraToRemove);
	}

	private static Dictionary<CAMERA_ID, Camera> _cameras = new Dictionary<CAMERA_ID, Camera>();
}
