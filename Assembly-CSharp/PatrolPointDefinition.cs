using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PatrolPointDefinition : Definition
{
	public void InvokeEvents()
	{
		if (this.Events != null)
		{
			for (int i = 0; i < this.Events.Count; i++)
			{
				this.Events[i].Raise();
			}
		}
	}

	public Vector3 Position;

	public List<GameEvent> Events;
}
