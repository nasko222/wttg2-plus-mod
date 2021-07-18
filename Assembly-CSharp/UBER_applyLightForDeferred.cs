using System;
using UnityEngine;

[AddComponentMenu("UBER/Apply Light for Deferred")]
[ExecuteInEditMode]
public class UBER_applyLightForDeferred : MonoBehaviour
{
	private void Start()
	{
		this.Reset();
	}

	private void Reset()
	{
		if (base.GetComponent<Light>() && this.lightForSelfShadowing == null)
		{
			this.lightForSelfShadowing = base.GetComponent<Light>();
		}
		if (base.GetComponent<Renderer>() && this._renderer == null)
		{
			this._renderer = base.GetComponent<Renderer>();
		}
	}

	private void Update()
	{
		if (this.lightForSelfShadowing)
		{
			if (this._renderer)
			{
				if (this.lightForSelfShadowing.type == LightType.Directional)
				{
					for (int i = 0; i < this._renderer.sharedMaterials.Length; i++)
					{
						this._renderer.sharedMaterials[i].SetVector("_WorldSpaceLightPosCustom", -this.lightForSelfShadowing.transform.forward);
					}
				}
				else
				{
					for (int j = 0; j < this._renderer.materials.Length; j++)
					{
						this._renderer.sharedMaterials[j].SetVector("_WorldSpaceLightPosCustom", new Vector4(this.lightForSelfShadowing.transform.position.x, this.lightForSelfShadowing.transform.position.y, this.lightForSelfShadowing.transform.position.z, 1f));
					}
				}
			}
			else if (this.lightForSelfShadowing.type == LightType.Directional)
			{
				Shader.SetGlobalVector("_WorldSpaceLightPosCustom", -this.lightForSelfShadowing.transform.forward);
			}
			else
			{
				Shader.SetGlobalVector("_WorldSpaceLightPosCustom", new Vector4(this.lightForSelfShadowing.transform.position.x, this.lightForSelfShadowing.transform.position.y, this.lightForSelfShadowing.transform.position.z, 1f));
			}
		}
	}

	public Light lightForSelfShadowing;

	private Renderer _renderer;
}
