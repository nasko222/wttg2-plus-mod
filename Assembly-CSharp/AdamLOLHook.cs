using System;
using UnityEngine;

public class AdamLOLHook : MonoBehaviour
{
	public AdamLOLHook()
	{
		this.Renderes = new SkinnedMeshRenderer[0];
		this.spawnPOS = new Vector3(3.266f, 39.582f, -1.240706f);
		this.spawnROT = new Vector3(0f, 180f, 0f);
	}

	public void Spawn()
	{
		base.transform.localPosition = this.spawnPOS;
		base.transform.localRotation = Quaternion.Euler(this.spawnROT);
		for (int i = 0; i < this.Renderes.Length; i++)
		{
			this.Renderes[i].enabled = true;
		}
	}

	private void Awake()
	{
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.Euler(Vector3.zero);
		for (int i = 0; i < this.Renderes.Length; i++)
		{
			this.Renderes[i].enabled = false;
		}
		AdamLOLHook.Ins = this;
	}

	public void DeSpawn()
	{
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.Euler(Vector3.zero);
		for (int i = 0; i < this.Renderes.Length; i++)
		{
			this.Renderes[i].enabled = false;
		}
	}

	public static AdamLOLHook Ins;

	[SerializeField]
	private SkinnedMeshRenderer[] Renderes;

	private Vector3 spawnPOS;

	private Vector3 spawnROT;
}
