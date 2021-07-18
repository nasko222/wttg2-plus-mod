using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
	public void OnEnable()
	{
		this.Event.RegisterListener(this);
	}

	public void OnDisable()
	{
		this.Event.UnregisterListener(this);
	}

	public void OnEventRaised()
	{
		this.Response.Invoke();
	}

	public void OnDefinitionEventRaised<T>(T SetDef) where T : Definition
	{
		this.DefinitionResponses.Invoke(SetDef);
	}

	public GameEvent Event;

	public UnityEvent Response;

	public DefinitionUnityEvent DefinitionResponses;
}
