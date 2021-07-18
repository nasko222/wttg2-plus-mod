using System;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class InteractiveLight : MonoBehaviour
{
	public void TriggerLight()
	{
		this.SetToOff = !this.SetToOff;
		this.MyLight.enabled = !this.SetToOff;
		this.myData.LightIsOff = this.SetToOff;
		for (int i = 0; i < this.EmissiveMats.Length; i++)
		{
			if (this.SetToOff)
			{
				this.EmissiveMats[i].DisableKeyword("_EMISSION");
			}
			else
			{
				this.EmissiveMats[i].EnableKeyword("_EMISSION");
			}
		}
		DataManager.Save<InteractiveLightData>(this.myData);
	}

	public void ForceOff()
	{
		this.beforeForceOff = this.SetToOff;
		this.myData.LightIsOff = this.SetToOff;
		this.SetToOff = true;
		this.MyLight.enabled = false;
		for (int i = 0; i < this.EmissiveMats.Length; i++)
		{
			this.EmissiveMats[i].DisableKeyword("_EMISSION");
		}
		DataManager.Save<InteractiveLightData>(this.myData);
	}

	public void ReturnFromForceOff()
	{
		this.SetToOff = this.beforeForceOff;
		this.myData.LightIsOff = this.SetToOff;
		if (!this.SetToOff)
		{
			this.MyLight.enabled = true;
			for (int i = 0; i < this.EmissiveMats.Length; i++)
			{
				this.EmissiveMats[i].EnableKeyword("_EMISSION");
			}
		}
		DataManager.Save<InteractiveLightData>(this.myData);
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<InteractiveLightData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new InteractiveLightData(this.myID);
			this.myData.LightIsOff = false;
		}
		if (this.myData.LightIsOff)
		{
			this.SetToOff = false;
			this.TriggerLight();
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		this.myID = Mathf.Abs(base.transform.position.GetHashCode());
		this.MyLight = base.GetComponent<Light>();
		for (int i = 0; i < this.EmissiveMats.Length; i++)
		{
			this.EmissiveMats[i].EnableKeyword("_EMISSION");
		}
		GameManager.StageManager.Stage += this.stageMe;
	}

	public Light MyLight;

	public Material[] EmissiveMats = new Material[0];

	public bool SetToOff;

	private int myID;

	private bool beforeForceOff;

	private InteractiveLightData myData;
}
