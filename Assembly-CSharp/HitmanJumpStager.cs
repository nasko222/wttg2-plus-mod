using System;
using System.Collections.Generic;

public class HitmanJumpStager
{
	public HitmanJumpStager(int cap = 5)
	{
		this.currentJumps = new List<Jump>(cap);
	}

	public void Stage()
	{
		for (int i = 0; i < this.currentJumps.Count; i++)
		{
			this.currentJumps[i].Stage();
		}
	}

	public void Execute()
	{
		for (int i = 0; i < this.currentJumps.Count; i++)
		{
			this.currentJumps[i].Execute();
		}
	}

	public void Add<T>(T JumpToAdd) where T : Jump
	{
		if (!this.currentJumps.Contains(JumpToAdd))
		{
			this.currentJumps.Add(JumpToAdd);
		}
	}

	public void Remove<T>(T JumpToRemove) where T : Jump
	{
		this.currentJumps.Remove(JumpToRemove);
	}

	private readonly List<Jump> currentJumps;
}
