using System;
using Colorful;
using UnityEngine;

public class TitleCameraManager : MonoBehaviour
{
	private void titleIsPresenting()
	{
		this.analogPost.enabled = true;
		this.noisePost.enabled = true;
		this.glitchPost.enabled = true;
	}

	private void Awake()
	{
		this.myCamera = base.GetComponent<Camera>();
		this.analogPost = base.GetComponent<AnalogTV>();
		this.noisePost = base.GetComponent<Noise>();
		this.glitchPost = base.GetComponent<Glitch>();
		this.analogPost.enabled = false;
		this.noisePost.enabled = false;
		this.glitchPost.enabled = false;
		TitleManager.Ins.TitlePresent.Event += this.titleIsPresenting;
	}

	private void OnDestroy()
	{
		TitleManager.Ins.TitlePresent.Event -= this.titleIsPresenting;
	}

	private Camera myCamera;

	private AnalogTV analogPost;

	private Noise noisePost;

	private Glitch glitchPost;
}
