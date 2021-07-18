using System;
using UnityEngine;

[Serializable]
public class HitmanSpawnDefinition : Definition
{
	public Vector3 Position;

	public Vector3 Rotation;

	public bool HasWalkPath;

	public PathManagerDefinition WalkPath;
}
