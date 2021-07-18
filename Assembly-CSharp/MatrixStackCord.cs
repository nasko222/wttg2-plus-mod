using System;

[Serializable]
public struct MatrixStackCord
{
	public MatrixStackCord(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}

	public static MatrixStackCord zero
	{
		get
		{
			return new MatrixStackCord(0, 0);
		}
	}

	public override int GetHashCode()
	{
		return this.X.GetHashCode() ^ this.X.GetHashCode();
	}

	public static bool operator ==(MatrixStackCord lhs, MatrixStackCord rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(MatrixStackCord lhs, MatrixStackCord rhs)
	{
		return !lhs.Equals(rhs);
	}

	public int X;

	public int Y;
}
