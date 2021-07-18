using System;
using System.Collections.Generic;

public class PooledStack<T> : Stack<T>
{
	public PooledStack(Func<T> createCallback, int preAllocate = 0)
	{
		this.createCallback = createCallback;
		if (createCallback != null)
		{
			for (int i = 0; i < preAllocate; i++)
			{
				base.Push(createCallback());
			}
		}
	}

	public new T Pop()
	{
		return (base.Count >= 1) ? base.Pop() : this.createCallback();
	}

	private readonly Func<T> createCallback;
}
