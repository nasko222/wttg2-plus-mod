using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NoirTunnelBehaviour : WindowBehaviour
{
	protected override void OnLaunch()
	{
	}

	protected override void OnClose()
	{
		this.masterKeyText.text = string.Empty;
	}

	protected override void OnMax()
	{
	}

	protected override void OnMin()
	{
	}

	protected override void OnResized()
	{
	}

	protected override void OnUnMax()
	{
	}

	protected override void OnUnMin()
	{
	}

	private void veirfyKey()
	{
		int defaultNoirTunnel = 250;
		if (DOSCoinPoll.moneyLoan <= 0)
		{
			if (this.masterKeyText.text.Equals(GameManager.TheCloud.MasterKey))
			{
				if (CurrencyManager.CurrentCurrency >= 250f)
				{
					CurrencyManager.RemoveCurrency(250f);
					EnemyManager.CultManager.StageEndJump();
					EnemyManager.State = ENEMY_STATE.LOCKED;
					DataManager.LockSave = true;
					EnvironmentManager.PowerBehaviour.LockedOut = true;
					this.presentLocation();
					return;
				}
				if (!this.notEnoughFired)
				{
					this.notEnoughFired = true;
					GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.CantBuyItem);
					this.cost1.color = this.notEnoughCostColor;
					GameManager.TimeSlinger.FireTimer(2f, delegate()
					{
						this.cost1.color = this.defaultCostColor;
						this.notEnoughFired = false;
					}, 0);
					return;
				}
			}
			else
			{
				this.masterKeyText.text = string.Empty;
				GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.CantBuyItem);
			}
			return;
		}
		if (!this.wannaReturnLoan)
		{
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.CantBuyItem);
			this.cost1.text = DOSCoinPoll.moneyLoan.ToString();
			this.cost2.text = DOSCoinPoll.moneyLoan.ToString();
			this.cost1.color = new Color(0f, 128f, 0f);
			this.wannaReturnLoan = true;
			GameManager.TimeSlinger.FireTimer(2f, delegate()
			{
				this.cost1.color = this.defaultCostColor;
				this.cost1.text = defaultNoirTunnel.ToString();
				this.cost2.text = defaultNoirTunnel.ToString();
				this.wannaReturnLoan = false;
			}, 0);
			return;
		}
		if (CurrencyManager.CurrentCurrency >= (float)DOSCoinPoll.moneyLoan)
		{
			this.ReturnLoanOf(DOSCoinPoll.moneyLoan);
			return;
		}
	}

	private void presentLocation()
	{
		GameManager.AudioSlinger.MuteAudioLayer(AUDIO_LAYER.WEBSITE);
		LookUp.DesktopUI.DesktopGraphicRaycaster.enabled = false;
		GameManager.AudioSlinger.PlaySound(this.showLocationSFX);
		this.hackerOverlayCG.blocksRaycasts = true;
		this.hackerOverlayCG.ignoreParentGroups = true;
		computerController.Ins.SetMasterLock(true);
		ComputerCameraManager.Ins.TriggerShowEndLocation();
		GameManager.TimeSlinger.FireTimer(3.98f, delegate()
		{
			ComputerCameraManager.Ins.ClearPostFXs();
			this.hackerOverlayCG.alpha = 1f;
			GameManager.AudioSlinger.PlaySound(GameManager.HackerManager.HackingTypeSFX);
			GameManager.AudioSlinger.PlaySoundWithCustomDelay(GameManager.HackerManager.HackingTypeSFX, 1.5f);
			GameManager.AudioSlinger.PlaySoundWithCustomDelay(GameManager.HackerManager.HackingTypeSFX, 2.3f);
			GameManager.AudioSlinger.PlaySoundWithCustomDelay(GameManager.HackerManager.HackingTypeSFX, 2.9f);
			GameManager.AudioSlinger.PlaySoundWithCustomDelay(GameManager.HackerManager.HackingTypeSFX, 5.5f);
			GameManager.AudioSlinger.PlaySoundWithCustomDelay(GameManager.HackerManager.HackingTypeSFX, 6.1f);
			GameManager.AudioSlinger.PlaySoundWithCustomDelay(GameManager.HackerManager.HackingTypeSFX, 6.7f);
			GameManager.AudioSlinger.PlaySoundWithCustomDelay(GameManager.HackerManager.HackingTypeSFX, 7.3f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine1, TERMINAL_LINE_TYPE.TYPE, "> ./NOIRTUNNEL", 0.2f, 0f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine2, TERMINAL_LINE_TYPE.TYPE, "  Loading NOIRTUNNEL v2.15", 0.2f, 0.2f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine3, TERMINAL_LINE_TYPE.TYPE, "  Locating...", 0.2f, 0.4f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0.5f, 0.5f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "  Location Found!", 0.6f, 1.5f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 2.2f, 0.1f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "  Lat: 41.064282", 0.6f, 2.3f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "  Lon: -71.877133", 0.6f, 2.9f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 3.5f, 0.1f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 3.5f, 0.1f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "  Incoming Message...", 0.6f, 5.5f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, ">  Hey it's Adam.", 0.6f, 6.1f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, ">  You found the location! Excellent work!", 0.6f, 6.7f);
			GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, ">  Head there now by exiting the lobby.", 0.6f, 7.3f);
		}, 0);
		GameManager.TimeSlinger.FireTimer(12f, delegate()
		{
			computerController.Ins.SetMasterLock(false);
		}, 0);
	}

	protected new void Awake()
	{
		this.masterKeyText = LookUp.DesktopUI.NOIR_TUNNEL_MASTER_KEY_INPUT;
		this.unlockButton = LookUp.DesktopUI.NOIR_TUNNEL_UNLOCK_BUTTON;
		this.cost1 = LookUp.DesktopUI.NOIR_TUNNEL_COST1;
		this.cost2 = LookUp.DesktopUI.NOIR_TUNNEL_COST2;
		this.unlockButton.onClick.AddListener(new UnityAction(this.veirfyKey));
		this.cost1.text = 250f.ToString();
		this.cost2.text = this.cost1.text;
		base.Awake();
	}

	protected new void OnDestroy()
	{
		this.unlockButton.onClick.RemoveListener(new UnityAction(this.veirfyKey));
		base.OnDestroy();
	}

	private void ReturnLoanOf(int amount)
	{
		CurrencyManager.RemoveCurrency((float)amount);
		DOSCoinPoll.moneyLoan = 0;
	}

	[SerializeField]
	private Color defaultCostColor;

	[SerializeField]
	private Color notEnoughCostColor;

	[SerializeField]
	private CanvasGroup hackerOverlayCG;

	[SerializeField]
	private AudioFileDefinition showLocationSFX;

	private InputField masterKeyText;

	private Button unlockButton;

	private Text cost1;

	private Text cost2;

	private bool notEnoughFired;

	private TerminalLineObject termLine1;

	private TerminalLineObject termLine2;

	private TerminalLineObject termLine3;

	private bool wannaReturnLoan;
}
