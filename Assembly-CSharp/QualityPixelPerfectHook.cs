using System;
using UnityEngine;

public class QualityPixelPerfectHook : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		this.myCanvas = base.GetComponent<Canvas>();
		int qualityLevel = QualitySettings.GetQualityLevel();
		if (qualityLevel <= 1)
		{
			this.myCanvas.pixelPerfect = false;
		}
	}

	private Canvas myCanvas;
}
