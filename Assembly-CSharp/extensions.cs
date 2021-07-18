using System;
using UnityEngine;

public static class extensions
{
	public static Vect3 ToVect3(this Vector3 vector3)
	{
		return new Vect3(vector3.x, vector3.y, vector3.z);
	}

	public static Vect2 ToVect2(this Vector2 vector2)
	{
		return new Vect2(vector2.x, vector2.y);
	}
}
