using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class QualityCameraHook : MonoBehaviour
{
	private void Awake()
	{
		this.myPPLayer = base.GetComponent<PostProcessLayer>();
		if (this.myPPLayer != null)
		{
			switch (QualitySettings.GetQualityLevel())
			{
			case 0:
				this.myPPLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
				break;
			case 1:
				this.myPPLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
				break;
			case 2:
				this.myPPLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
				break;
			case 3:
				this.myPPLayer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
				break;
			case 4:
				this.myPPLayer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
				break;
			case 5:
				this.myPPLayer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
				break;
			case 6:
				this.myPPLayer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
				break;
			default:
				this.myPPLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
				break;
			}
		}
	}

	private PostProcessLayer myPPLayer;
}
