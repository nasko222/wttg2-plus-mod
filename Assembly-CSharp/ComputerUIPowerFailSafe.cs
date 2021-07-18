using System;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ComputerUIPowerFailSafe : MonoBehaviour
{
	private void powerWentOff()
	{
		this.previousValue = this.myCanvas.enabled;
		this.myCanvas.enabled = false;
	}

	private void powerWentOn()
	{
		this.myCanvas.enabled = this.previousValue;
	}

	private void Awake()
	{
		this.myCanvas = base.GetComponent<Canvas>();
	}

	private void Start()
	{
		if (EnvironmentManager.PowerBehaviour != null)
		{
			EnvironmentManager.PowerBehaviour.PowerOffEvent.Event += this.powerWentOff;
			EnvironmentManager.PowerBehaviour.PowerOnEvent.Event += this.powerWentOn;
		}
		else
		{
			this.timeStamp = Time.time;
			this.envCheck = true;
		}
	}

	private void Update()
	{
		if (this.envCheck && Time.time - this.timeStamp >= 10f)
		{
			if (EnvironmentManager.PowerBehaviour != null)
			{
				EnvironmentManager.PowerBehaviour.PowerOffEvent.Event += this.powerWentOff;
				EnvironmentManager.PowerBehaviour.PowerOnEvent.Event += this.powerWentOn;
				this.envCheck = false;
			}
			else
			{
				this.timeStamp = Time.time;
			}
		}
	}

	private void OnDestroy()
	{
	}

	private Canvas myCanvas;

	private bool previousValue;

	private bool envCheck;

	private float timeStamp;
}
