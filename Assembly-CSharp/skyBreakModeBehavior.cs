using System;
using UnityEngine;

public class skyBreakModeBehavior : MonoBehaviour
{
	public bool ProcessCMD(string theCMD)
	{
		bool result = true;
		string[] array = theCMD.Split(new string[]
		{
			" "
		}, StringSplitOptions.None);
		array[0] = array[0].ToLower();
		string text = array[0];
		if (text != null)
		{
			if (!(text == "wep"))
			{
				if (!(text == "wpa"))
				{
					if (!(text == "wpa2"))
					{
						if (text == "list")
						{
							result = false;
							this.mySkyBreakBehavior.SpitLastCMD(theCMD);
							this.ModeListCrackers();
						}
					}
					else
					{
						result = false;
						this.mySkyBreakBehavior.SpitLastCMD(theCMD);
						this.ModeSwitchToWPA2();
					}
				}
				else
				{
					result = false;
					this.mySkyBreakBehavior.SpitLastCMD(theCMD);
					this.ModeSwitchToWPA();
				}
			}
			else
			{
				result = false;
				this.mySkyBreakBehavior.SpitLastCMD(theCMD);
				this.ModeSwitchToWEP();
			}
		}
		return result;
	}

	private void ModeListCrackers()
	{
		this.myTerminalHelper.ClearInputLine();
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "You have the following crackers installed:", 0.4f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.FADE, "    > WEP", 0.25f, 0.5f);
		float num = 0.75f;
		if (this.mySkyBreakBehavior.wpaCracking.productOwned)
		{
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.FADE, "    > WPA", 0.25f, num);
			num = 1f;
		}
		if (this.mySkyBreakBehavior.wpa2Cracking.productOwned)
		{
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.FADE, "    > WPA2", 0.25f, num);
			num = 1.25f;
		}
		GameManager.TimeSlinger.FireTimer(num, new Action(this.mySkyBreakBehavior.addCMDInputLine), 0);
	}

	private void ModeSwitchToWEP()
	{
		this.mySkyBreakBehavior.SwitchStateMode(SkyBreakActionState.WEP);
	}

	private void ModeSwitchToWPA()
	{
		if (this.mySkyBreakBehavior.wpaCracking.productOwned)
		{
			this.mySkyBreakBehavior.SwitchStateMode(SkyBreakActionState.WPA);
		}
		else
		{
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "You do not have the WPA cracker installed!", 0f, 0f);
			this.myTerminalHelper.PushInputLineToBottom();
		}
	}

	private void ModeSwitchToWPA2()
	{
		if (this.mySkyBreakBehavior.wpa2Cracking.productOwned)
		{
			this.mySkyBreakBehavior.SwitchStateMode(SkyBreakActionState.WPA2);
		}
		else
		{
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "You do not have the WPA2 cracker installed!", 0f, 0f);
			this.myTerminalHelper.PushInputLineToBottom();
		}
	}

	private void Awake()
	{
		this.mySkyBreakBehavior = base.GetComponent<skyBreakBehavior>();
		this.myTerminalHelper = base.GetComponent<TerminalHelperBehavior>();
	}

	private skyBreakBehavior mySkyBreakBehavior;

	private TerminalHelperBehavior myTerminalHelper;
}
