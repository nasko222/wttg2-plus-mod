using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
	public void Raise()
	{
		for (int i = this.eventListeners.Count - 1; i >= 0; i--)
		{
			this.eventListeners[i].OnEventRaised();
		}
	}

	public void DefinitionRaise<T>(T SetDef) where T : Definition
	{
		for (int i = this.eventListeners.Count - 1; i >= 0; i--)
		{
			this.eventListeners[i].OnDefinitionEventRaised<T>(SetDef);
		}
	}

	public void RegisterListener(GameEventListener listener)
	{
		this.eventListeners.Add(listener);
	}

	public void UnregisterListener(GameEventListener listener)
	{
		this.eventListeners.Remove(listener);
	}

	private readonly List<GameEventListener> eventListeners = new List<GameEventListener>();
}
