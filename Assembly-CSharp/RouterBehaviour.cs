using System;
using UnityEngine;

public class RouterBehaviour : MonoBehaviour
{
	public void SoftBuild()
	{
		RouterBehaviour.Ins = this;
		this.onSFX = UnityEngine.Object.Instantiate<AudioFileDefinition>(PoliceScannerBehaviour.Ins.onSFX);
		this.offSFX = UnityEngine.Object.Instantiate<AudioFileDefinition>(PoliceScannerBehaviour.Ins.offSFX);
		this.onSFX.MyAudioHub = AUDIO_HUB.PLAYER_HUB;
		this.onSFX.MyAudioLayer = AUDIO_LAYER.PLAYER;
		this.offSFX.MyAudioHub = AUDIO_HUB.PLAYER_HUB;
		this.offSFX.MyAudioLayer = AUDIO_LAYER.PLAYER;
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.myMeshRenderer.enabled = false;
		this.myInteractionHook.LeftClickAction += this.leftClickAction;
		this.myInteractionHook.RightClickAction += this.rightClickAction;
		this.myMeshRenderer.material = this.matReset;
		this.routerHubSwitch = 0;
	}

	private void leftClickAction()
	{
		this.toggleRouter();
	}

	private void toggleRouter()
	{
		if (this.RouterLocked)
		{
			return;
		}
		if (this.RouterIsActive && this.routerHubSwitch == 4)
		{
			this.routerHubSwitch = 0;
			this.RouterIsActive = false;
			if (GameManager.ManagerSlinger.WifiManager.IsOnline)
			{
				GameManager.ManagerSlinger.WifiManager.DisconnectFromWifi();
			}
			else
			{
				WifiMenuBehaviour.Ins.refreshNetworks();
			}
			this.myMeshRenderer.material = this.matReset;
			this.myAudioHub.PlaySound(this.offSFX);
			return;
		}
		this.RouterIsActive = true;
		this.routerHubSwitch++;
		if (GameManager.ManagerSlinger.WifiManager.IsOnline)
		{
			GameManager.ManagerSlinger.WifiManager.DisconnectFromWifi();
		}
		else
		{
			WifiMenuBehaviour.Ins.refreshNetworks();
		}
		switch (this.routerHubSwitch)
		{
		case 1:
			this.myMeshRenderer.material = this.matOn;
			break;
		case 2:
			this.myMeshRenderer.material = this.matOnLY2;
			break;
		case 3:
			this.myMeshRenderer.material = this.matOnLY3;
			break;
		case 4:
			this.myMeshRenderer.material = this.matOnLY4;
			break;
		default:
			this.myMeshRenderer.material = this.matOnAll;
			break;
		}
		this.myAudioHub.PlaySound(this.onSFX);
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.leftClickAction;
		this.myInteractionHook.RightClickAction -= this.rightClickAction;
		UnityEngine.Object.Destroy(this.onSFX);
		UnityEngine.Object.Destroy(this.offSFX);
	}

	public void MoveMe(Vector3 SetPOS, Vector3 SetROT, Vector3 SetSCL)
	{
		this.Owned = true;
		this.myMeshRenderer.enabled = true;
		this.RouterIsActive = false;
		this.myMeshRenderer.material = this.matReset;
		this.routerHubSwitch = 0;
		base.transform.position = SetPOS;
		base.transform.rotation = Quaternion.Euler(SetROT);
		base.transform.localScale = SetSCL;
	}

	public void ShowInteractionIcon()
	{
		UIInteractionManager.Ins.ShowKnob();
		UIInteractionManager.Ins.ShowLeftMouseButtonAction();
		UIInteractionManager.Ins.ShowRightMouseButtonAction();
	}

	public void HideInteractionIcon()
	{
		UIInteractionManager.Ins.HideKnob();
		UIInteractionManager.Ins.HideLeftMouseButtonAction();
		UIInteractionManager.Ins.HideRightMouseButtonAction();
	}

	private void rightClickAction()
	{
		if (this.RouterIsActive && !this.RouterLocked)
		{
			this.RestartModem();
		}
	}

	public void RestartModem()
	{
		int num = this.routerHubSwitch;
		Material material = this.myMeshRenderer.material;
		this.RouterIsActive = false;
		this.routerHubSwitch = 0;
		this.RouterLocked = true;
		if (GameManager.ManagerSlinger.WifiManager.IsOnline)
		{
			GameManager.ManagerSlinger.WifiManager.DisconnectFromWifi();
		}
		else
		{
			WifiMenuBehaviour.Ins.refreshNetworks();
		}
		this.myMeshRenderer.material = this.matOff;
		this.myAudioHub.PlaySound(CustomSoundLookUp.routerreset);
		GameManager.TimeSlinger.FireTimer(0.5f, delegate()
		{
			this.myMeshRenderer.material = this.matOn;
		}, 0);
		GameManager.TimeSlinger.FireTimer(1f, delegate()
		{
			this.myMeshRenderer.material = this.matOnLY2;
		}, 0);
		GameManager.TimeSlinger.FireTimer(1.5f, delegate()
		{
			this.myMeshRenderer.material = this.matOnLY3;
		}, 0);
		GameManager.TimeSlinger.FireTimer(2f, delegate()
		{
			this.myMeshRenderer.material = this.matOnLY4;
		}, 0);
		GameManager.TimeSlinger.FireTimer(2.5f, delegate()
		{
			this.myMeshRenderer.material = this.matOnAll;
		}, 0);
		GameManager.TimeSlinger.FireTimer(3f, delegate()
		{
			this.myMeshRenderer.material = material;
			this.RouterIsActive = true;
			this.routerHubSwitch = num;
			this.RouterLocked = false;
			this.IsJammed = false;
			if (GameManager.ManagerSlinger.WifiManager.IsOnline)
			{
				GameManager.ManagerSlinger.WifiManager.DisconnectFromWifi();
				return;
			}
			WifiMenuBehaviour.Ins.refreshNetworks();
		}, 0);
	}

	public void trollReset()
	{
		this.myAudioHub.PlaySound(CustomSoundLookUp.routerjammed);
	}

	public void JamTheRouter()
	{
		if (this.IsJammed)
		{
			return;
		}
		this.myAudioHub.PlaySound(CustomSoundLookUp.routerjammed);
		this.IsJammed = true;
	}

	[HideInInspector]
	public bool Owned;

	[HideInInspector]
	public bool RouterIsActive;

	[SerializeField]
	private Material matOn;

	[SerializeField]
	private Material matOff;

	public static RouterBehaviour Ins;

	[HideInInspector]
	public AudioFileDefinition onSFX;

	[HideInInspector]
	public AudioFileDefinition offSFX;

	[SerializeField]
	private MeshRenderer myMeshRenderer;

	[SerializeField]
	private InteractionHook myInteractionHook;

	[SerializeField]
	private AudioHubObject myAudioHub;

	[SerializeField]
	private Material matOnLY2;

	[SerializeField]
	private Material matOnLY3;

	[SerializeField]
	private Material matOnLY4;

	[SerializeField]
	private Material matOnAll;

	[SerializeField]
	private Material matReset;

	[HideInInspector]
	public int routerHubSwitch;

	private bool RouterLocked;

	[HideInInspector]
	public bool IsJammed;
}
