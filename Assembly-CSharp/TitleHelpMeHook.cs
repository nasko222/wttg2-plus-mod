using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TitleHelpMeHook : MonoBehaviour
{
	private void videoIsPrepared(VideoPlayer VP)
	{
		this.myVideoPlayer.prepareCompleted -= this.videoIsPrepared;
		this.videoPreparedTimeStamp = Time.time;
		this.videoDelay = 2f;
		this.videoReadyToPlay = true;
	}

	private void videoDonePlaying(VideoPlayer VP)
	{
		this.myVideoPlayer.loopPointReached -= this.videoDonePlaying;
		this.helpMeRawImage.enabled = false;
		this.myVideoPlayer.enabled = false;
	}

	private void Awake()
	{
		this.myVideoPlayer = base.GetComponent<VideoPlayer>();
		this.myVideoPlayer.renderMode = VideoRenderMode.RenderTexture;
		this.myVideoPlayer.targetTexture = this.helpMeRenderTexture;
		this.myVideoPlayer.playOnAwake = false;
		this.myVideoPlayer.waitForFirstFrame = false;
		this.helpMeRawImage.enabled = false;
		this.helpMeRawImage.texture = this.helpMeRenderTexture;
		this.myVideoPlayer.prepareCompleted += this.videoIsPrepared;
		this.myVideoPlayer.loopPointReached += this.videoDonePlaying;
		this.myVideoPlayer.Prepare();
	}

	private void Update()
	{
		if (this.videoReadyToPlay && Time.time - this.videoPreparedTimeStamp >= this.videoDelay)
		{
			this.videoReadyToPlay = false;
			this.helpMeRawImage.enabled = true;
			this.myVideoPlayer.Play();
		}
	}

	[SerializeField]
	private RawImage helpMeRawImage;

	[SerializeField]
	private RenderTexture helpMeRenderTexture;

	private VideoPlayer myVideoPlayer;

	private bool videoReadyToPlay;

	private float videoDelay;

	private float videoPreparedTimeStamp;
}
