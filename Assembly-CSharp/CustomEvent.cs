using System;
using System.Collections.Generic;

public class CustomEvent
{
	public CustomEvent(int capacity = 1)
	{
		this.events = new List<Action>(capacity);
	}

	public int Count
	{
		get
		{
			return this.events.Count;
		}
	}

	public event Action Event
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

	public void Execute()
	{
		for (int i = 0; i < this.events.Count; i++)
		{
			if (this.events[i] != null)
			{
				this.events[i]();
			}
		}
	}

	public void ExecuteAndKill()
	{
		for (int i = 0; i < this.events.Count; i++)
		{
			if (this.events[i] != null)
			{
				this.events[i]();
				this.events.Remove(this.events[i]);
			}
		}
	}

	public void Clear()
	{
		this.events.Clear();
	}

	private readonly List<Action> events;
}
