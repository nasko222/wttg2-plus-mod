using System;
using System.Collections.Generic;

public class CustomEvent<T>
{
	public CustomEvent(int capacity = 1)
	{
		this.events = new List<Action<T>>(capacity);
	}

	public event Action<T> Event
	{
		add
		{
			this.events.Add(value);
		}
		remove
		{
			this.events.Remove(value);
		}
	}

	public void Execute(T value)
	{
		for (int i = 0; i < this.events.Count; i++)
		{
			if (this.events[i] != null)
			{
				this.events[i](value);
			}
		}
	}

	public void Clear()
	{
		this.events.Clear();
	}

	private readonly List<Action<T>> events;
}
