using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
[RequireComponent(typeof(AudioHubObject))]
[RequireComponent(typeof(BoxCollider))]
public class LightTrigger : MonoBehaviour
{
	public bool LightsAreOn
	{
		get
		{
			return this.lightsOn;
		}
	}

	public void HoverCheck()
	{
		if (this.setLights)
		{
			UIInteractionManager.Ins.ShowLightOff();
		}
		else
		{
			UIInteractionManager.Ins.ShowLightOn();
		}
	}

	public void ExitHoverCheck()
	{
		if (this.setLights)
		{
			UIInteractionManager.Ins.HideLightOff();
		}
		else
		{
			UIInteractionManager.Ins.HideLightOn();
		}
	}

	private void triggerLights()
	{
		if (!this.amBusy)
		{
			this.myInteractionHook.ForceLock = true;
			this.amBusy = true;
			this.lightsOn = !this.lightsOn;
			this.myData.LightsAreOff = !this.lightsOn;
			for (int i = 0; i < this.Lights.Length; i++)
			{
				this.Lights[i].TriggerLight();
			}
			if (this.LightSwitch != null)
			{
				if (this.lightsOn)
				{
					this.LightsWentOnEvent.Execute();
					this.LightSwitch.localRotation = Quaternion.Euler(this.SwitchOnPOS);
					this.myAudioHub.PlaySound(this.SwitchOnSFX);
				}
				else
				{
					this.LightsWentOffEvent.Execute();
					this.LightSwitch.localRotation = Quaternion.Euler(this.SwitchOffPOS);
					this.myAudioHub.PlaySound(this.SwitchOffSFX);
				}
			}
			DataManager.Save<LightTriggerData>(this.myData);
			GameManager.TimeSlinger.FireTimer(0.75f, delegate()
			{
				this.setLights = this.lightsOn;
				this.amBusy = false;
				this.myInteractionHook.ForceLock = false;
			}, 0);
		}
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<LightTriggerData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new LightTriggerData(this.myID);
			this.myData.LightsAreOff = false;
		}
		if (this.myData.LightsAreOff)
		{
			this.lightsOn = false;
			this.LightSwitch.localRotation = Quaternion.Euler(this.SwitchOffPOS);
			this.LightsWentOffEvent.Execute();
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		this.myID = base.transform.position.GetHashCode();
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myAudioHub = base.GetComponent<AudioHubObject>();
		this.myInteractionHook.LeftClickAction += this.triggerLights;
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.triggerLights;
	}

	public InteractiveLight[] Lights;

	public CustomEvent LightsWentOnEvent = new CustomEvent(2);

	public CustomEvent LightsWentOffEvent = new CustomEvent(2);

	public AudioFileDefinition SwitchOnSFX;

	public AudioFileDefinition SwitchOffSFX;

	public Transform LightSwitch;

	public Vector3 SwitchOnPOS = Vector3.zero;

	public Vector3 SwitchOffPOS = Vector3.zero;

	private int myID;

	private LightTriggerData myData;

	private InteractionHook myInteractionHook;

	private AudioHubObject myAudioHub;

	private bool lightsOn = true;

	private bool setLights = true;

	private bool amBusy;
}
