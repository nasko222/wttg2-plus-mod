using System;
using Colorful;
using UnityEngine;

public class LobbyComputerCameraManager : MonoBehaviour
{
	public void BecomeMaster()
	{
		this.myCamera.targetTexture = null;
		this.mainCamera.enabled = false;
	}

	public void BecomeSlave()
	{
		this.myCamera.targetTexture = this.myRenderTexture;
		this.mainCamera.enabled = true;
	}

	public void TriggerGlitch()
	{
		this.clitchPost.enabled = true;
		GameManager.TimeSlinger.FireTimer(3f, delegate()
		{
			this.clitchPost.enabled = false;
		}, 0);
	}

	private void Awake()
	{
		this.myCamera = base.GetComponent<Camera>();
		this.myRenderTexture.height = Screen.height;
		this.myRenderTexture.width = Screen.width;
		this.myCamera.orthographicSize = (float)Screen.height / 2f;
		base.transform.localPosition = new Vector3((float)Screen.width / 2f, (float)(-(float)(Screen.height / 2)), base.transform.localPosition.z);
		this.myCamera.targetTexture = this.myRenderTexture;
		this.clitchPost = base.GetComponent<Glitch>();
		CameraManager.Get(CAMERA_ID.MAIN, out this.mainCamera);
	}

	[SerializeField]
	private RenderTexture myRenderTexture;

	private Camera myCamera;

	private Camera mainCamera;

	private Glitch clitchPost;
}
