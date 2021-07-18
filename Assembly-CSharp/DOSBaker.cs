using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DOSBaker : MonoBehaviour
{
	public void prepTheMeshes()
	{
		this.hasPrepErrors = false;
		this.prepErrors = string.Empty;
		this.objectsToCombineNames = string.Empty;
		if (this.meshObjects.Count > 0)
		{
			if (this.meshFilters == null)
			{
				this.meshFilters = new List<MeshFilter>();
			}
			for (int i = 0; i < this.meshObjects.Count; i++)
			{
				if (this.meshObjects[i] != null)
				{
					if (this.meshObjects[i].GetComponent<MeshFilter>() != null)
					{
						this.meshFilters.Add(this.meshObjects[i].GetComponent<MeshFilter>());
						string text = this.objectsToCombineNames;
						this.objectsToCombineNames = string.Concat(new string[]
						{
							text,
							"\nObject #",
							i.ToString(),
							" - ",
							this.meshObjects[i].name.ToString()
						});
					}
					else
					{
						this.hasPrepErrors = true;
						string text = this.prepErrors;
						this.prepErrors = string.Concat(new string[]
						{
							text,
							"\nObject #",
							i.ToString(),
							" - ",
							this.meshObjects[i].name.ToString(),
							": Has no MeshFilter Component!"
						});
					}
				}
				else
				{
					this.hasPrepErrors = true;
					this.prepErrors = this.prepErrors + "\nObject #" + i.ToString() + ": Is NULL!";
				}
			}
		}
		else
		{
			this.prepErrors += "\nThere are no mesh objects";
			this.hasPrepErrors = true;
		}
		if (!this.hasPrepErrors)
		{
			this.meshesArePrepped = true;
		}
	}

	public void resetPrep()
	{
		this.meshFilters = new List<MeshFilter>();
		this.hasPrepErrors = false;
		this.meshesArePrepped = false;
		this.prepErrors = string.Empty;
		this.objectsToCombineNames = string.Empty;
	}

	public void bakeTheMeshes()
	{
		if (this.meshesArePrepped && this.meshFilters.Count > 0)
		{
			GameObject gameObject = new GameObject("MeshBaked");
			gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
			CombineInstance[] array = new CombineInstance[this.meshFilters.Count];
			for (int i = 0; i < this.meshFilters.Count; i++)
			{
				array[i].mesh = this.meshFilters[i].sharedMesh;
				array[i].transform = this.meshFilters[i].transform.localToWorldMatrix;
				this.meshFilters[i].gameObject.SetActive(false);
			}
			gameObject.GetComponent<MeshFilter>().mesh = new Mesh();
			gameObject.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(array, true);
		}
	}

	public void fooBar()
	{
		Debug.Log("FOOBAR");
		Texture2D texture2D = new Texture2D(2048, 2048);
		this.theRects = texture2D.PackTextures(this.textsToPack, 2, 2048);
		Debug.Log(this.theRects);
		Debug.Log(texture2D);
		byte[] bytes = texture2D.EncodeToJPG();
		File.WriteAllBytes(Application.dataPath + "/foobar.jpg", bytes);
		Debug.Log(Application.dataPath);
	}

	public Texture2D[] textsToPack;

	public Rect[] theRects;

	public List<GameObject> meshObjects;

	public List<MeshFilter> meshFilters;

	public GameObject fooObject;

	public bool hasPrepErrors;

	public bool meshesArePrepped;

	public string prepErrors = string.Empty;

	public string objectsToCombineNames = string.Empty;
}
