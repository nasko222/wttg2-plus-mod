using System;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			ScreenCapture.CaptureScreenshot("ScreenShot.png");
		}
	}
}
