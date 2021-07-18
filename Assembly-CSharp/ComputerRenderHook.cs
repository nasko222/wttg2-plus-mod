using System;
using UnityEngine;

public class ComputerRenderHook : MonoBehaviour
{
	private void Awake()
	{
		this.renderMat = base.GetComponent<MeshRenderer>().material;
		this.renderMat.SetTexture("_MainTex", ComputerCameraManager.Ins.FinalRenderTexture);
	}

	private Material renderMat;
}
