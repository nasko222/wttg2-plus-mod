using System;
using System.Collections.Generic;

public class MatrixStack<T>
{
	public MatrixStack()
	{
		this.size = 0;
		this.myData = new T[0, 0];
	}

	public MatrixStack(int MatrixSize)
	{
		this.size = MatrixSize;
		this.myData = new T[MatrixSize, MatrixSize];
	}

	public int Size
	{
		get
		{
			return this.size;
		}
	}

	public MatrixStackCord Pointer
	{
		get
		{
			return this.pointer;
		}
	}

	public T[,] Data
	{
		get
		{
			return this.myData;
		}
	}

	public void SetMatrixSize(int SetValue)
	{
		if (this.size != SetValue)
		{
			this.size = SetValue;
			this.myData = this.resizeArray(this.myData, SetValue, SetValue);
		}
	}

	public void Push(T Item)
	{
		if ((this.pointer.X + 1) * (this.pointer.Y + 1) < this.size * this.size)
		{
			bool flag = this.pointer.X + 1 > this.size - 1;
			this.pointer.X = ((!flag) ? (this.pointer.X + 1) : 0);
			this.pointer.Y = ((!flag) ? this.pointer.Y : (this.pointer.Y + 1));
			this.myData[this.pointer.X, this.pointer.Y] = Item;
		}
	}

	public bool Set(T SetValue, MatrixStackCord SetCord)
	{
		if (SetCord.X < this.size && SetCord.X >= 0 && SetCord.Y < this.size && SetCord.Y >= 0)
		{
			this.myData[SetCord.X, SetCord.Y] = SetValue;
			return true;
		}
		return false;
	}

	public bool Set(T SetValue, int XCord, int YCord)
	{
		if (XCord < this.size && XCord >= 0 && YCord < this.size && YCord >= 0)
		{
			this.myData[XCord, YCord] = SetValue;
			return true;
		}
		return false;
	}

	public T Get(MatrixStackCord SetCord)
	{
		if (SetCord.X < this.size && SetCord.X >= 0 && SetCord.Y < this.size && SetCord.Y >= 0)
		{
			return this.myData[SetCord.X, SetCord.Y];
		}
		return default(T);
	}

	public T Get(int XCord, int YCord)
	{
		if (XCord < this.size && XCord >= 0 && YCord < this.size && YCord >= 0)
		{
			return this.myData[XCord, YCord];
		}
		return default(T);
	}

	public bool TryAndGetValue(out T ReturnValue, MatrixStackCord SetCord)
	{
		if (SetCord.X < this.size && SetCord.X >= 0 && SetCord.Y < this.size && SetCord.Y >= 0)
		{
			ReturnValue = this.myData[SetCord.X, SetCord.Y];
			return true;
		}
		ReturnValue = default(T);
		return false;
	}

	public bool TryAndGetValue(out T ReturnValue, int XCord, int YCord)
	{
		if (XCord < this.size && XCord >= 0 && YCord < this.size && YCord >= 0)
		{
			ReturnValue = this.myData[XCord, YCord];
			return true;
		}
		ReturnValue = default(T);
		return false;
	}

	public bool TryAndGetValueByClock(out T ReturnValue, MatrixStackCord SetCord, MATRIX_STACK_CLOCK_POSITION ClockPOS)
	{
		bool result = false;
		switch (ClockPOS)
		{
		case MATRIX_STACK_CLOCK_POSITION.HIGH_NOON:
		{
			MatrixStackCord setCord = new MatrixStackCord(SetCord.X, SetCord.Y - 2);
			result = this.TryAndGetValue(out ReturnValue, setCord);
			break;
		}
		case MATRIX_STACK_CLOCK_POSITION.NOON:
		{
			MatrixStackCord setCord2 = new MatrixStackCord(SetCord.X, SetCord.Y - 1);
			result = this.TryAndGetValue(out ReturnValue, setCord2);
			break;
		}
		case MATRIX_STACK_CLOCK_POSITION.ONE:
		{
			MatrixStackCord setCord3 = new MatrixStackCord(SetCord.X + 1, SetCord.Y - 1);
			result = this.TryAndGetValue(out ReturnValue, setCord3);
			break;
		}
		case MATRIX_STACK_CLOCK_POSITION.THREE:
		{
			MatrixStackCord setCord4 = new MatrixStackCord(SetCord.X + 1, SetCord.Y);
			result = this.TryAndGetValue(out ReturnValue, setCord4);
			break;
		}
		case MATRIX_STACK_CLOCK_POSITION.FOUR:
		{
			MatrixStackCord setCord5 = new MatrixStackCord(SetCord.X + 1, SetCord.Y + 1);
			result = this.TryAndGetValue(out ReturnValue, setCord5);
			break;
		}
		case MATRIX_STACK_CLOCK_POSITION.SIX:
		{
			MatrixStackCord setCord6 = new MatrixStackCord(SetCord.X, SetCord.Y + 1);
			result = this.TryAndGetValue(out ReturnValue, setCord6);
			break;
		}
		case MATRIX_STACK_CLOCK_POSITION.SEVEN:
		{
			MatrixStackCord setCord7 = new MatrixStackCord(SetCord.X - 1, SetCord.Y + 1);
			result = this.TryAndGetValue(out ReturnValue, setCord7);
			break;
		}
		case MATRIX_STACK_CLOCK_POSITION.NINE:
		{
			MatrixStackCord setCord8 = new MatrixStackCord(SetCord.X - 1, SetCord.Y);
			result = this.TryAndGetValue(out ReturnValue, setCord8);
			break;
		}
		case MATRIX_STACK_CLOCK_POSITION.TEN:
		{
			MatrixStackCord setCord9 = new MatrixStackCord(SetCord.X - 1, SetCord.Y - 1);
			result = this.TryAndGetValue(out ReturnValue, setCord9);
			break;
		}
		default:
			ReturnValue = default(T);
			break;
		}
		return result;
	}

	public IEnumerable<T> GetAll()
	{
		for (int i = 0; i < this.size; i++)
		{
			for (int j = 0; j < this.size; j++)
			{
				yield return this.myData[j, i];
			}
		}
		yield break;
	}

	public void Clear()
	{
		for (int i = 0; i < this.size; i++)
		{
			for (int j = 0; j < this.size; j++)
			{
				this.myData[i, j] = default(T);
			}
		}
		this.pointer = new MatrixStackCord(-1, 0);
	}

	private T[,] resizeArray(T[,] original, int rows, int cols)
	{
		return new T[rows, cols];
	}

	private int size;

	private MatrixStackCord pointer = new MatrixStackCord(-1, 0);

	private T[,] myData;
}
