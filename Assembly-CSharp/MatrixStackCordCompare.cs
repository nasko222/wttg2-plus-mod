using System;
using System.Collections.Generic;

public class MatrixStackCordCompare : IEqualityComparer<MatrixStackCord>
{
	public bool Equals(MatrixStackCord x, MatrixStackCord y)
	{
		return x.X == y.X && x.Y == y.Y;
	}

	public int GetHashCode(MatrixStackCord obj)
	{
		return obj.GetHashCode();
	}

	public static MatrixStackCordCompare Ins = new MatrixStackCordCompare();
}
