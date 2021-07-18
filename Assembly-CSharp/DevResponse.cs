using System;
using UnityEngine;

[Serializable]
public class DevResponse
{
	public static DevResponse CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<DevResponse>(jsonString);
	}

	public string GameHash;

	public string Action;

	public string Additional;
}
