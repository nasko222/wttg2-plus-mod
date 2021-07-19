using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ComputerPowerHook : MonoBehaviour
{
	public bool PowerOn
	{
		get
		{
			return this.powerOn;
		}
	}

	public void ShutDownComputer()
	{
		if (GameManager.HackerManager.theSwan.SwanError)
		{
			return;
		}
		GameManager.AudioSlinger.MuteAudioHub(AUDIO_HUB.COMPUTER_HUB);
		this.myRayCaster.enabled = true;
		this.powerLabelText.SetText("Powering Off");
		GameManager.ManagerSlinger.CursorManager.SetOverwrite(false);
		GameManager.ManagerSlinger.CursorManager.SwitchToDefaultCursor();
		GameManager.ManagerSlinger.WifiManager.DisconnectFromWifi();
		switchToComputerController.Ins.Lock();
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			DOTween.To(() => this.powerLabelCG.alpha, delegate(float x)
			{
				this.powerLabelCG.alpha = x;
			}, 1f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
			{
				GameManager.TimeSlinger.FireTimer(3f, new Action(this.shutComputerDown), 0);
			});
		});
	}

	public void PowerComputer()
	{
		this.powerOn = true;
		PowerComputerTrigger.Ins.Lock();
		this.powerLabelCG.alpha = 0f;
		this.powerLabelText.SetText("Powering On");
		switchToComputerController.Ins.UnLock();
		this.computerMaterial.EnableKeyword("_EMISSION");
		ComputerScreenHook.Ins.MeshRenderer.enabled = true;
		DOTween.To(() => this.powerLabelCG.alpha, delegate(float x)
		{
			this.powerLabelCG.alpha = x;
		}, 1f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			GameManager.TimeSlinger.FireTimer(3f, new Action(this.powerComputerOn), 0);
		});
	}

	private void shutComputerDown()
	{
		this.powerOn = false;
		if (StateManager.PlayerState == PLAYER_STATE.COMPUTER)
		{
			computerController.Ins.LeaveMe();
		}
		this.computerMaterial.DisableKeyword("_EMISSION");
		ComputerScreenHook.Ins.MeshRenderer.enabled = false;
		PowerComputerTrigger.Ins.UnLock();
		this.myData.ComputerIsOff = true;
		DataManager.Save<ComputerPowerData>(this.myData);
	}

	private void powerComputerOn()
	{
		this.powerLabelCG.alpha = 0f;
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			GameManager.AudioSlinger.UnMuteAudioHub(AUDIO_HUB.COMPUTER_HUB);
			this.myRayCaster.enabled = false;
		});
		this.myData.ComputerIsOff = false;
		DataManager.Save<ComputerPowerData>(this.myData);
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<ComputerPowerData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new ComputerPowerData(this.myID);
			this.myData.ComputerIsOff = false;
		}
		if (this.myData.ComputerIsOff)
		{
			this.powerOn = false;
			PowerComputerTrigger.Ins.UnLock();
			this.computerMaterial.DisableKeyword("_EMISSION");
			ComputerScreenHook.Ins.MeshRenderer.enabled = false;
			switchToComputerController.Ins.Lock();
			this.myCG.alpha = 1f;
			this.powerLabelCG.alpha = 0f;
			this.powerLabelText.SetText("Powering On");
		}
		else
		{
			this.powerOn = true;
			if (PowerComputerTrigger.Ins != null)
			{
				PowerComputerTrigger.Ins.Lock();
			}
			ComputerScreenHook.Ins.MeshRenderer.enabled = true;
			this.computerMaterial.EnableKeyword("_EMISSION");
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		ComputerPowerHook.Ins = this;
		this.myID = base.transform.position.GetHashCode();
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myRayCaster = base.GetComponent<GraphicRaycaster>();
		this.myRayCaster.enabled = false;
		GameManager.StageManager.Stage += this.stageMe;
	}

	public void ShutDownComputerInsantly()
	{
		GameManager.AudioSlinger.MuteAudioHub(AUDIO_HUB.COMPUTER_HUB);
		this.myRayCaster.enabled = true;
		this.powerLabelText.SetText("SYSTEM FAILURE");
		GameManager.ManagerSlinger.CursorManager.SetOverwrite(false);
		GameManager.ManagerSlinger.CursorManager.SwitchToDefaultCursor();
		GameManager.ManagerSlinger.WifiManager.DisconnectFromWifi();
		switchToComputerController.Ins.Lock();
		DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			DOTween.To(() => this.powerLabelCG.alpha, delegate(float x)
			{
				this.powerLabelCG.alpha = x;
			}, 1f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
			{
				GameManager.TimeSlinger.FireTimer(0.01f, new Action(this.shutComputerDown), 0);
			});
		});
	}

	public static ComputerPowerHook Ins;

	[SerializeField]
	private CanvasGroup powerLabelCG;

	[SerializeField]
	private TextMeshProUGUI powerLabelText;

	[SerializeField]
	private Material computerMaterial;

	private GraphicRaycaster myRayCaster;

	private CanvasGroup myCG;

	private bool powerOn;

	private int myID;

	private ComputerPowerData myData;
}
