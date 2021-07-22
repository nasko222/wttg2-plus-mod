using System;

public class skyBreakBehavior : WindowBehaviour
{
	public void ProcessCMD(string theCMD)
	{
		string[] array = theCMD.Split(new string[]
		{
			" "
		}, StringSplitOptions.None);
		array[0] = array[0].ToLower();
		bool flag = true;
		string text = array[0];
		if (text != null)
		{
			if (!(text == "clear"))
			{
				if (!(text == "exit"))
				{
					if (!(text == "quit"))
					{
						if (!(text == "help"))
						{
							if (text == "swan")
							{
								flag = false;
								this.SwanCommand(theCMD);
							}
						}
						else
						{
							flag = false;
							this.SpitLastCMD(theCMD);
							this.processHelp();
						}
					}
					else
					{
						flag = false;
						this.Window.SetActive(false);
						this.OnClose();
					}
				}
				else if (this.currentState == SkyBreakActionState.MODE)
				{
					flag = false;
					this.Window.SetActive(false);
					this.OnClose();
				}
			}
			else
			{
				flag = false;
				this.myTerminalHelper.ClearTerminal();
				this.myTerminalHelper.PushInputLineToBottom();
			}
		}
		if (flag)
		{
			switch (this.currentState)
			{
			case SkyBreakActionState.MODE:
				flag = this.mySkyBreakModeBeahvior.ProcessCMD(theCMD);
				break;
			case SkyBreakActionState.WEP:
				flag = this.mySkyBreakWEPBehavior.ProcessCMD(theCMD);
				break;
			case SkyBreakActionState.WPA:
				flag = this.mySkyBreakWPABehavior.ProcessCMD(theCMD);
				break;
			case SkyBreakActionState.WPA2:
				flag = this.mySkyBreakWPA2Behavior.ProcessCMD(theCMD);
				break;
			}
		}
		if (flag)
		{
			this.SpitLastCMD(theCMD);
			this.SpitInvalidCMD(theCMD);
			this.myTerminalHelper.PushInputLineToBottom();
		}
		this.myTerminalHelper.UpdateTerminalContentScrollHeight();
	}

	public void SetCMDInputAsFocus()
	{
		if (this.myTerminalHelper.TerminalInput != null)
		{
			this.myTerminalHelper.TerminalInput.inputLine.ActivateInputField();
		}
	}

	public void SpitLastCMD(string theCMD)
	{
		string setLine = string.Empty;
		switch (this.currentState)
		{
		case SkyBreakActionState.MODE:
			setLine = "skyBREAK > " + theCMD;
			break;
		case SkyBreakActionState.WEP:
			setLine = "skyBREAK > WEP > " + theCMD;
			break;
		case SkyBreakActionState.WPA:
			setLine = "skyBREAK > WPA > " + theCMD;
			break;
		case SkyBreakActionState.WPA2:
			setLine = "skyBREAK > WPA2 > " + theCMD;
			break;
		}
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, setLine, 0f, 0f);
	}

	public void addCMDInputLine()
	{
		if (this.Window.activeSelf)
		{
			switch (this.currentState)
			{
			case SkyBreakActionState.MODE:
				this.myTerminalHelper.AddInputLine(new Action<string>(this.ProcessCMD), "skyBREAK >");
				break;
			case SkyBreakActionState.WEP:
				this.myTerminalHelper.AddInputLine(new Action<string>(this.ProcessCMD), "skyBREAK > WEP >");
				break;
			case SkyBreakActionState.WPA:
				this.myTerminalHelper.AddInputLine(new Action<string>(this.ProcessCMD), "skyBREAK > WPA >");
				break;
			case SkyBreakActionState.WPA2:
				this.myTerminalHelper.AddInputLine(new Action<string>(this.ProcessCMD), "skyBREAK > WPA2 >");
				break;
			}
		}
	}

	public void UpdateTerminalInputLineMode()
	{
		if (this.Window.activeSelf && this.myTerminalHelper.TerminalInput != null)
		{
			switch (this.currentState)
			{
			case SkyBreakActionState.MODE:
				this.myTerminalHelper.TerminalInput.UpdateTitle("skyBREAK >");
				break;
			case SkyBreakActionState.WEP:
				this.myTerminalHelper.TerminalInput.UpdateTitle("skyBREAK > WEP >");
				break;
			case SkyBreakActionState.WPA:
				this.myTerminalHelper.TerminalInput.UpdateTitle("skyBREAK > WPA >");
				break;
			case SkyBreakActionState.WPA2:
				this.myTerminalHelper.TerminalInput.UpdateTitle("skyBREAK > WPA2 >");
				break;
			}
		}
	}

	public void SwitchStateMode(SkyBreakActionState setState)
	{
		this.currentState = setState;
		this.UpdateTerminalInputLineMode();
		this.myTerminalHelper.PushInputLineToBottom();
	}

	protected override void OnLaunch()
	{
		if (!this.Window.activeSelf)
		{
			this.currentState = SkyBreakActionState.MODE;
			this.showIntro();
		}
	}

	protected override void OnClose()
	{
		this.myTerminalHelper.FullClear();
		this.mySkyBreakWEPBehavior.CloseMeOut();
		this.mySkyBreakWPABehavior.CloseMeOut();
		this.mySkyBreakWPA2Behavior.CloseMeOut();
	}

	protected override void OnMin()
	{
	}

	protected override void OnUnMin()
	{
	}

	protected override void OnMax()
	{
	}

	protected override void OnUnMax()
	{
	}

	protected override void OnResized()
	{
	}

	private void showIntro()
	{
		if (this.introBlock != null)
		{
			for (int i = 0; i < this.introBlock.terminalLines.Count; i++)
			{
				this.myTerminalHelper.AddLine(this.introBlock.terminalLines[i]);
			}
		}
		float num = 2.5f;
		if (this.wpaCracking.productOwned)
		{
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.FADE, "        > WPA", 0.3f, num);
			num = 2.8f;
		}
		if (this.wpa2Cracking.productOwned)
		{
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.FADE, "        > WPA2", 0.3f, num);
			num = 3.1f;
		}
		GameManager.TimeSlinger.FireTimer(num, new Action(this.addCMDInputLine), 0);
	}

	private void processTerminalBlock(SkyBreakTerminalBlockDefinition termBlock)
	{
		if (termBlock != null)
		{
			for (int i = 0; i < termBlock.terminalLines.Count; i++)
			{
				this.myTerminalHelper.AddLine(termBlock.terminalLines[i]);
			}
			this.myTerminalHelper.PushInputLineToBottom();
		}
	}

	private void processHelp()
	{
		switch (this.currentState)
		{
		case SkyBreakActionState.MODE:
			this.processTerminalBlock(this.modeHelpBlock);
			break;
		case SkyBreakActionState.WEP:
			this.processTerminalBlock(this.wepHelpBlock);
			break;
		case SkyBreakActionState.WPA:
			this.processTerminalBlock(this.wpaHelpBlock);
			break;
		case SkyBreakActionState.WPA2:
			this.processTerminalBlock(this.wpa2HelpBlock);
			break;
		}
	}

	private void SpitInvalidCMD(string theCMD)
	{
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "'" + theCMD + "' is not a recognized valid command. Type HELP for more information.", 0f, 0f);
	}

	protected new void Awake()
	{
		base.Awake();
		GameManager.HackerManager.theSwan.skyBreak = this;
		this.myTerminalHelper = base.GetComponent<TerminalHelperBehavior>();
		this.mySkyBreakModeBeahvior = base.GetComponent<skyBreakModeBehavior>();
		this.mySkyBreakWEPBehavior = base.GetComponent<skyBreakWEPBehavior>();
		this.mySkyBreakWPABehavior = base.GetComponent<skyBreakWPABehavior>();
		this.mySkyBreakWPA2Behavior = base.GetComponent<skyBreakWPA2Behavior>();
		if (this.Window.GetComponent<BringWindowToFrontBehaviour>() != null)
		{
			this.Window.GetComponent<BringWindowToFrontBehaviour>().OnTap += this.SetCMDInputAsFocus;
		}
	}

	private void SwanCommand(string theCMD)
	{
		string setLine = string.Empty;
		bool flag = true;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		string[] array = theCMD.Split(new string[]
		{
			" "
		}, StringSplitOptions.None);
		if (array.Length < 2)
		{
			flag = false;
			setLine = "swan (query/code)";
		}
		if (flag)
		{
			array[1] = array[1].ToLower();
		}
		if (flag && !(array[1] == "query"))
		{
			if (array[1] == "code")
			{
				if (TheSwan.extOn)
				{
					if (array.Length < 3)
					{
						flag = false;
						setLine = "parameter must be greater than 0";
					}
					else
					{
						if (!(array[2] != TheSwan.extCode))
						{
							this.SwanCode(theCMD);
							return;
						}
						flag = false;
						setLine = "PARAMETER UNCLEAR!";
					}
				}
				else if (array.Length < 7)
				{
					flag = false;
					setLine = "WRONG!";
				}
				else if (!int.TryParse(array[2], out num))
				{
					flag = false;
					setLine = "WRONG!";
				}
				else if (!int.TryParse(array[3], out num2))
				{
					flag = false;
					setLine = "WRONG!";
				}
				else if (!int.TryParse(array[4], out num3))
				{
					flag = false;
					setLine = "WRONG!";
				}
				else if (!int.TryParse(array[5], out num4))
				{
					flag = false;
					setLine = "WRONG!";
				}
				else if (!int.TryParse(array[6], out num5))
				{
					flag = false;
					setLine = "WRONG!";
				}
				else if (!int.TryParse(array[7], out num6))
				{
					flag = false;
					setLine = "WRONG!";
				}
				if (flag && num != skyBreakBehavior.SwanNumbers[0])
				{
					flag = false;
					setLine = "WRONG!";
				}
				if (flag && num2 != skyBreakBehavior.SwanNumbers[1])
				{
					flag = false;
					setLine = "WRONG!";
				}
				if (flag && num3 != skyBreakBehavior.SwanNumbers[2])
				{
					flag = false;
					setLine = "WRONG!";
				}
				if (flag && num4 != skyBreakBehavior.SwanNumbers[3])
				{
					flag = false;
					setLine = "WRONG!";
				}
				if (flag && num5 != skyBreakBehavior.SwanNumbers[4])
				{
					flag = false;
					setLine = "WRONG!";
				}
				if (flag && num6 != skyBreakBehavior.SwanNumbers[5])
				{
					flag = false;
					setLine = "WRONG!";
				}
				if (GameManager.HackerManager.theSwan._108 > 60f)
				{
					flag = false;
					setLine = "ERROR!";
				}
			}
			else
			{
				flag = false;
				setLine = "swan (query/code)";
			}
		}
		if (!flag)
		{
			this.SpitLastCMD(theCMD);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, setLine, 0f, 0f);
			this.myTerminalHelper.PushInputLineToBottom();
			return;
		}
		if (array[1] == "query")
		{
			this.SwanQuery(theCMD);
			return;
		}
		if (array[1] == "code")
		{
			this.SwanCode(theCMD);
			return;
		}
	}

	private void SwanQuery(string theCMD)
	{
		this.SpitLastCMD(theCMD);
		if (!GameManager.HackerManager.theSwan.isActivatedBefore)
		{
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "Swan is disabled!", 0f, 0f);
		}
		else if (GameManager.HackerManager.theSwan._108 > 108f)
		{
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "108:00!", 0f, 0f);
		}
		else
		{
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, GameManager.HackerManager.theSwan._108.ToString() + ":00!", 0f, 0f);
		}
		this.myTerminalHelper.PushInputLineToBottom();
	}

	private void SwanCode(string theCMD)
	{
		this.SpitLastCMD(theCMD);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "CORRECT!", 0f, 0f);
		this.myTerminalHelper.PushInputLineToBottom();
		GameManager.HackerManager.theSwan.SwanReset();
	}

	public void CauseSystemFailure()
	{
		this.myTerminalHelper.ClearInputLine();
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 0.5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 1f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 1.5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 2f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 2.5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 3f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 3.5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 4f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 4.5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 5.5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 6f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 6.5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 7f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 7.5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 8f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 8.5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 9f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 9.5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 10f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 10.5f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "System FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem FailureSystem Failure", 0.5f, 11f);
		GameManager.TimeSlinger.FireTimer(15f, new Action(this.ResetSwan), 0);
	}

	private void ResetSwan()
	{
		this.myTerminalHelper.ClearTerminal();
		this.addCMDInputLine();
	}

	public ZeroDayProductDefinition wpaCracking;

	public ZeroDayProductDefinition wpa2Cracking;

	public SkyBreakTerminalBlockDefinition introBlock;

	public SkyBreakTerminalBlockDefinition modeHelpBlock;

	public SkyBreakTerminalBlockDefinition wepHelpBlock;

	public SkyBreakTerminalBlockDefinition wpaHelpBlock;

	public SkyBreakTerminalBlockDefinition wpa2HelpBlock;

	private SkyBreakActionState currentState;

	private TerminalHelperBehavior myTerminalHelper;

	private skyBreakModeBehavior mySkyBreakModeBeahvior;

	private skyBreakWEPBehavior mySkyBreakWEPBehavior;

	private skyBreakWPABehavior mySkyBreakWPABehavior;

	private skyBreakWPA2Behavior mySkyBreakWPA2Behavior;

	public static int[] SwanNumbers = new int[]
	{
		4,
		8,
		15,
		16,
		23,
		42
	};
}
