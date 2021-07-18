using System;
using UnityEngine;

[Serializable]
public struct Vect2
{
	public Vect2(float x, float y)
	{
		this.X = x;
		this.Y = y;
	}

	public static Vect2 zero
	{
		get
		{
			return new Vect2(0f, 0f);
		}
	}

	public static Vect2 Convert(Vector2 From)
	{
		return new Vect2(From.x, From.y);
	}

	public Vector2 ToVector2
	{
		get
		{
			return new Vector2(this.X, this.Y);
		}
	}

	public override bool Equals(object obj)
	{
		return obj is Vect2 && this.Equals((Vect2)obj);
	}

	public bool Equals(Vect2 other)
	{
		return this.X == other.X && this.Y == other.Y;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool operator ==(Vect2 lhs, Vect2 rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Vect2 lhs, Vect2 rhs)
	{
		return !lhs.Equals(rhs);
	}

	public float X;

	public float Y;
}
