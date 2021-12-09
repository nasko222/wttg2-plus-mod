using System;
using UnityEngine;

public class SulphurPackageObject : MonoBehaviour
{
	private void Awake()
	{
		SulphurPackageObject.Ins = this;
		for (int i = 0; i < this.packages.Length; i++)
		{
			this.packages[i] = UnityEngine.Object.Instantiate<GameObject>(CustomObjectLookUp.PackageBox);
		}
		this.packages[0].transform.position = new Vector3(-0.808f, 40.648f, -0.224f);
		this.packages[1].transform.position = new Vector3(-0.813f, 40.849f, -0.224f);
		this.packages[2].transform.position = new Vector3(-0.794f, 41.049f, -0.224f);
		this.packages[3].transform.position = new Vector3(-0.578f, 40.687f, -0.224f);
		this.packages[4].transform.position = new Vector3(-0.632f, 40.654f, -0.013f);
		this.packages[2].transform.Rotate(new Vector3(0f, 0f, -1.13f));
		this.packages[3].transform.Rotate(new Vector3(0f, 0f, -84.25f));
		this.packages[2].transform.Rotate(new Vector3(85.98f, 0f, 0f));
		for (int j = 0; j < this.packages.Length; j++)
		{
			this.packages[j].transform.localScale = new Vector3(0.413f, 0.323f, 0.35f);
			this.packages[j].SetActive(false);
		}
	}

	public void UpdateSulphurPackages()
	{
		this.packages[0].SetActive(SulphurInventory.SulphurAmount >= 1);
		this.packages[1].SetActive(SulphurInventory.SulphurAmount >= 2);
		this.packages[2].SetActive(SulphurInventory.SulphurAmount >= 3);
		this.packages[3].SetActive(SulphurInventory.SulphurAmount >= 4);
		this.packages[4].SetActive(SulphurInventory.SulphurAmount >= 5);
	}

	[HideInInspector]
	public GameObject[] packages = new GameObject[5];

	public static SulphurPackageObject Ins;
}
