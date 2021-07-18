using System;
using UnityEngine;

[Serializable]
public struct Vect3
{
	public Vect3(float x, float y, float z)
	{
		this.X = x;
		this.Y = y;
		this.Z = z;
	}

	public static Vect3 zero
	{
		get
		{
			return new Vect3(0f, 0f, 0f);
		}
	}

	public Vector3 ToVector3
	{
		get
		{
			return new Vector3(this.X, this.Y, this.Z);
		}
	}

	public override bool Equals(object obj)
	{
		return obj is Vect3 && this.Equals((Vect3)obj);
	}

	public bool Equals(Vect3 other)
	{
		return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool operator ==(Vect3 lhs, Vect3 rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Vect3 lhs, Vect3 rhs)
	{
		return !lhs.Equals(rhs);
	}

	public float X;

	public float Y;

	public float Z;
}
