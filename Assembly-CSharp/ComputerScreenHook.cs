using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ComputerScreenHook : MonoBehaviour
{
	public MeshRenderer MeshRenderer
	{
		get
		{
			return this.myMeshRenderer;
		}
	}

	private void Awake()
	{
		ComputerScreenHook.Ins = this;
		this.myMeshRenderer = base.GetComponent<MeshRenderer>();
	}

	public static ComputerScreenHook Ins;

	private MeshRenderer myMeshRenderer;
}
