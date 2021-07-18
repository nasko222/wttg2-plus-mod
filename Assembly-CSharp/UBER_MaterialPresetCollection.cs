using System;
using UnityEngine;

public class UBER_MaterialPresetCollection : ScriptableObject
{
	[SerializeField]
	[HideInInspector]
	public string currentPresetName;

	[SerializeField]
	[HideInInspector]
	public UBER_PresetParamSection whatToRestore;

	[SerializeField]
	[HideInInspector]
	public UBER_MaterialPreset[] matPresets;

	[SerializeField]
	[HideInInspector]
	public string[] names;
}
