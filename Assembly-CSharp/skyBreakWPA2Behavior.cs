﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class skyBreakWPA2Behavior : MonoBehaviour
{
	public void CloseMeOut()
	{
		GameManager.TweenSlinger.KillTween(this.wpa2UpdateKeysTested);
		GameManager.TimeSlinger.KillTimer(this.injectionResetTimer);
		GameManager.TimeSlinger.KillTimer(this.injectionLoopTimer);
		GameManager.TimeSlinger.KillTimer(this.wpaCrackedTimer);
		this.targetedWPA2Networks.Clear();
	}

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
			if (!(text == "exit"))
			{
				if (!(text == "scan"))
				{
					if (!(text == "inject"))
					{
						if (text == "crack")
						{
							result = false;
							this.PerformCrack(theCMD);
						}
					}
					else
					{
						result = false;
						this.PerformInject(theCMD);
					}
				}
				else
				{
					result = false;
					this.mySkyBreakBehavior.SpitLastCMD(theCMD);
					this.PerformScan();
				}
			}
			else
			{
				result = false;
				this.mySkyBreakBehavior.SpitLastCMD(theCMD);
				this.SwitchToMode();
			}
		}
		return result;
	}

	private void SwitchToMode()
	{
		this.mySkyBreakBehavior.SwitchStateMode(SkyBreakActionState.MODE);
	}

	private void PerformScan()
	{
		this.myTerminalHelper.ClearInputLine();
		this.myTerminalHelper.ClearTerminal();
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.TYPE, "Scanning...", 0.5f, 0f);
		GameManager.TimeSlinger.FireTimer(this.GetRandScanTime(), new Action(this.ScanResults), 0);
	}

	private void ScanResults()
	{
		if (this.mySkyBreakBehavior.Window.activeSelf)
		{
			List<WifiNetworkDefinition> secureNetworks = GameManager.ManagerSlinger.WifiManager.GetSecureNetworks(WIFI_SECURITY.WPA2);
			float num = 0.3f;
			this.myTerminalHelper.ClearTerminal();
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "Scanned WPA Network Results", 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.FADE, string.Concat(new string[]
			{
				MagicSlinger.FluffString("ESSID", " ", 20),
				"  ",
				MagicSlinger.FluffString("BSSID", " ", 20),
				"  ",
				MagicSlinger.FluffString("CH", " ", 10),
				"  ",
				MagicSlinger.FluffString("MAX", " ", 10),
				"  ",
				MagicSlinger.FluffString("CLD", " ", 10),
				"  ",
				MagicSlinger.FluffString("PING", " ", 10),
				"  SIG"
			}), 0.3f, 0f);
			for (int i = 0; i < secureNetworks.Count; i++)
			{
				string theString = ((int)(secureNetworks[i].networkMaxInjectionAmount - 1)).ToString();
				string theString2 = ((int)(secureNetworks[i].networkInjectionCoolOffTime - 1f)).ToString();
				if (ModsManager.Nightmare)
				{
					theString = "ERR";
					theString2 = "ERR";
				}
				this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.FADE, string.Concat(new string[]
				{
					MagicSlinger.FluffString(secureNetworks[i].networkName, " ", 20),
					"  ",
					MagicSlinger.FluffString(secureNetworks[i].networkBSSID, " ", 20),
					"  ",
					MagicSlinger.FluffString(secureNetworks[i].networkChannel.ToString(), " ", 10),
					"  ",
					MagicSlinger.FluffString(theString, " ", 10),
					"  ",
					MagicSlinger.FluffString(theString2, " ", 10),
					"  ",
					MagicSlinger.FluffString(secureNetworks[i].networkPower.ToString(), " ", 10),
					"  ",
					MagicSlinger.GetWifiSiginalType(secureNetworks[i].networkSignal)
				}), 0.2f, num);
				num += 0.2f;
			}
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0f, 0f);
			GameManager.TimeSlinger.FireTimer(num, new Action(this.mySkyBreakBehavior.addCMDInputLine), 0);
		}
	}

	private void PerformInject(string theCMD)
	{
		string setLine = string.Empty;
		bool flag = true;
		int num = -1;
		int num2 = -1;
		string[] array = theCMD.Split(new string[]
		{
			" "
		}, StringSplitOptions.None);
		if (ModsManager.SBGlitch)
		{
			flag = false;
			setLine = "Warning! Inject Skip is enabled. Instead of injecting, use crack <BSSID> <CH>";
		}
		if (array.Length < 4 && !ModsManager.SBGlitch)
		{
			flag = false;
			setLine = "Invalid usage of inject. Valid usage: inject <BSSID> <CH> <Injections>";
		}
		if (flag)
		{
			if (GameManager.ManagerSlinger.WifiManager.CheckBSSID(array[1], out this.currentTargetedWPA2))
			{
				if (!int.TryParse(array[2], out num))
				{
					flag = false;
					setLine = "Invalid channel number.";
				}
				if (!int.TryParse(array[3], out num2))
				{
					flag = false;
					setLine = "Invalid injection amount.";
				}
			}
			else
			{
				flag = false;
				setLine = "Could not find a network with the BSSID Of '" + array[1] + "'";
			}
		}
		if (flag)
		{
			if (this.currentTargetedWPA2.networkChannel != (short)num)
			{
				flag = false;
				setLine = "Invalid channel number.";
			}
			if (num2 < 1 || num2 > 1000)
			{
				flag = false;
				setLine = "Invalid injection amount. Please select 1 - 1000";
			}
		}
		if (flag)
		{
			this.mySkyBreakBehavior.SpitLastCMD(theCMD);
			this.myTerminalHelper.ClearInputLine();
			GameManager.TimeSlinger.FireHardTimer(out this.injectionLoopTimer, 0.1f, new Action(this.AddInjection), num2);
			if (this.injectionLoopTimer != null)
			{
				this.injectionLoopTimer.AddLoopCallBack(new Action(this.AddInjectionLoopOver));
				return;
			}
		}
		else
		{
			this.mySkyBreakBehavior.SpitLastCMD(theCMD);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, setLine, 0f, 0f);
			this.myTerminalHelper.PushInputLineToBottom();
		}
	}

	private void AddInjection()
	{
		string text = MagicSlinger.MD5It(this.currentTargetedWPA2.networkName);
		GameManager.TimeSlinger.KillTimer(this.injectionResetTimer);
		if (!this.targetedWPA2Networks.ContainsKey(text))
		{
			this.targetedWPA2Networks.Add(text, new WifiWPAObject(this.currentTargetedWPA2, 0, 0));
		}
		if (this.injectLine == null)
		{
			this.myTerminalHelper.AddLine(out this.injectLine, TERMINAL_LINE_TYPE.HARD, " ", 0f, 0f);
		}
		this.targetedWPA2Networks[text].TotalInjectionAmountAdded = this.targetedWPA2Networks[text].TotalInjectionAmountAdded + 1;
		this.targetedWPA2Networks[text].CurrentInjectionAmount = this.targetedWPA2Networks[text].CurrentInjectionAmount + 1;
		string setText = string.Concat(new string[]
		{
			"Injecting De-Auth request into '",
			this.currentTargetedWPA2.networkBSSID,
			"' - '",
			this.currentTargetedWPA2.networkName,
			"' #:",
			this.targetedWPA2Networks[text].TotalInjectionAmountAdded.ToString()
		});
		this.injectLine.UpdateMyText(setText);
		if (this.targetedWPA2Networks[text].CurrentInjectionAmount >= (int)this.targetedWPA2Networks[text].myWifiNetwork.networkMaxInjectionAmount)
		{
			GameManager.TimeSlinger.KillTimer(this.injectionResetTimer);
			GameManager.TimeSlinger.KillTimer(this.injectionLoopTimer);
			GameManager.ManagerSlinger.WifiManager.TakeNetworkOffLine(this.currentTargetedWPA2);
			string myLine = this.injectLine.GetMyLine();
			this.injectLine.HardClear();
			this.injectLine = null;
			this.targetedWPA2Networks[text].TotalInjectionAmountAdded = 0;
			this.targetedWPA2Networks[text].CurrentInjectionAmount = 0;
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, myLine, 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "Too many injection requests were sent at once. Network is now offline.", 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "Remember to only send a little at a time to prevent the system from going offline.", 0f, 0f);
			this.mySkyBreakBehavior.addCMDInputLine();
		}
		else if (this.targetedWPA2Networks[text].TotalInjectionAmountAdded >= (int)this.targetedWPA2Networks[text].myWifiNetwork.networkInjectionAmount)
		{
			this.targetedWPA2Networks[text].SetInjectionReady(true);
			GameManager.TimeSlinger.KillTimer(this.injectionResetTimer);
			GameManager.TimeSlinger.KillTimer(this.injectionLoopTimer);
			string myLine2 = this.injectLine.GetMyLine();
			this.injectLine.HardClear();
			this.injectLine = null;
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, myLine2, 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "Injection De-Auth injection was successful! Network is now ready to crack.", 0f, 0f);
			this.mySkyBreakBehavior.addCMDInputLine();
		}
		else
		{
			GameManager.TimeSlinger.FireHardTimer<string>(out this.injectionResetTimer, this.targetedWPA2Networks[text].myWifiNetwork.networkInjectionCoolOffTime, new Action<string>(this.ResetNetworkCurrentInjection), text, 0);
		}
	}

	private void AddInjectionLoopOver()
	{
		if (!this.myTerminalHelper.TerminalInput.Active && this.injectLine != null)
		{
			string myLine = this.injectLine.GetMyLine();
			this.injectLine.HardClear();
			this.injectLine = null;
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, myLine, 0f, 0f);
			this.mySkyBreakBehavior.addCMDInputLine();
		}
	}

	private void ResetNetworkCurrentInjection(string setKey)
	{
		if (this.targetedWPA2Networks.ContainsKey(setKey))
		{
			this.targetedWPA2Networks[setKey].CurrentInjectionAmount = 0;
		}
	}

	private void PerformCrack(string theCMD)
	{
		string setLine = string.Empty;
		bool flag = true;
		int num = -1;
		string[] array = theCMD.Split(new string[]
		{
			" "
		}, StringSplitOptions.None);
		if (array.Length < 3)
		{
			flag = false;
			setLine = "Invalid usage of crack. Valid usage: crack <BSSID> <CH>";
		}
		if (flag)
		{
			if (GameManager.ManagerSlinger.WifiManager.CheckBSSID(array[1], out this.currentTargetedWPA2))
			{
				if (!int.TryParse(array[2], out num))
				{
					flag = false;
					setLine = "Invalid channel number.";
				}
			}
			else
			{
				flag = false;
				setLine = "Could not find a network with the BSSID Of '" + array[1] + "'";
			}
		}
		if (flag && this.currentTargetedWPA2.networkChannel != (short)num)
		{
			flag = false;
			setLine = "Could not connect to network '" + this.currentTargetedWPA2.networkBSSID + "' on channel:" + num.ToString();
		}
		if (flag && !ModsManager.SBGlitch)
		{
			string key = MagicSlinger.MD5It(this.currentTargetedWPA2.networkName);
			if (this.targetedWPA2Networks.ContainsKey(key))
			{
				if (!this.targetedWPA2Networks[key].GetInjectionReady())
				{
					flag = false;
					setLine = "Can not crack network '" + this.currentTargetedWPA2.networkBSSID + "'. No De-Auth request has been set!";
				}
			}
			else
			{
				flag = false;
				setLine = "Can not crack network '" + this.currentTargetedWPA2.networkBSSID + "'. No De-Auth request has been set!";
			}
		}
		if (flag)
		{
			this.DoCrack();
			return;
		}
		this.mySkyBreakBehavior.SpitLastCMD(theCMD);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, setLine, 0f, 0f);
		this.myTerminalHelper.PushInputLineToBottom();
	}

	private void DoCrack()
	{
		this.anticheatWifiTag = this.currentTargetedWPA2;
		this.myTerminalHelper.ClearInputLine();
		this.myTerminalHelper.ClearTerminal();
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, MagicSlinger.FluffString(" ", " ", 24) + "    skyBREAK 1.5.89", 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0f, 0f);
		string setLine = MagicSlinger.FluffString(" ", " ", 16) + "    [10 keys tested (500 k/s)]";
		this.myTerminalHelper.AddLine(out this.wpa2CrackKeys, TERMINAL_LINE_TYPE.HARD, setLine, 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.CRACK, "Master Key", 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.CRACK, string.Empty, 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.CRACK, "Transient Key", 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.CRACK, string.Empty, 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.CRACK, string.Empty, 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.CRACK, string.Empty, 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.CRACK, "EAPOL HMAC", 0f, 0f);
		float num = UnityEngine.Random.Range(this.minCrackTime, this.maxCrackTime);
		float setToValue = Mathf.Round(num * 500f);
		this.wpa2UpdateKeysTested = GameManager.TweenSlinger.PlayDOSTweenLiner(0f, setToValue, num, new Action<float>(this.UpdateKeysTested));
		GameManager.TimeSlinger.FireHardTimer(out this.wpaCrackedTimer, num, new Action(this.Cracked), 0);
	}

	private void UpdateKeysTested(float setValue)
	{
		if (this.mySkyBreakBehavior.Window.activeSelf)
		{
			this.wpa2CrackKeys.UpdateMyText(MagicSlinger.FluffString(" ", " ", 16) + "    [" + Mathf.RoundToInt(setValue).ToString() + " keys tested (500 k/s)]");
		}
	}

	private void Cracked()
	{
		if (this.mySkyBreakBehavior.Window.activeSelf)
		{
			if (!ModsManager.SBGlitch)
			{
				this.currentTargetedWPA2 = this.anticheatWifiTag;
			}
			GameManager.TweenSlinger.KillTween(this.wpa2UpdateKeysTested);
			this.myTerminalHelper.KillCrackLines();
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Concat(new string[]
			{
				MagicSlinger.FluffString(" ", " ", 11),
				"'",
				this.currentTargetedWPA2.networkBSSID,
				"' - '",
				this.currentTargetedWPA2.networkName,
				"' HAS BEEN CRACKED!"
			}), 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, MagicSlinger.FluffString(" ", " ", 20) + "    PASSWORD FOUND! [" + this.currentTargetedWPA2.networkPassword + "]", 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0f, 0f);
			this.mySkyBreakBehavior.addCMDInputLine();
			SteamSlinger.Ins.CrackWifiNetwork(this.currentTargetedWPA2.GetHashCode());
		}
	}

	private float GetRandScanTime()
	{
		return UnityEngine.Random.Range(this.minScanTime, this.maxScanTime);
	}

	private void PrepMe()
	{
		this.targetedWPA2Networks = new Dictionary<string, WifiWPAObject>();
		this.mySkyBreakBehavior = base.GetComponent<skyBreakBehavior>();
		this.myTerminalHelper = base.GetComponent<TerminalHelperBehavior>();
	}

	private void Start()
	{
		this.PrepMe();
	}

	private void Update()
	{
	}

	[Range(3.5f, 8f)]
	public float minScanTime = 4f;

	[Range(5.5f, 15f)]
	public float maxScanTime = 10f;

	[Range(5f, 30f)]
	public float minCrackTime = 8f;

	[Range(15f, 120f)]
	public float maxCrackTime = 45f;

	private WifiNetworkDefinition currentTargetedWPA2;

	private Dictionary<string, WifiWPAObject> targetedWPA2Networks;

	private skyBreakBehavior mySkyBreakBehavior;

	private TerminalHelperBehavior myTerminalHelper;

	private DOSTween wpa2UpdateKeysTested;

	private Timer injectionResetTimer;

	private Timer injectionLoopTimer;

	private Timer wpaCrackedTimer;

	private TerminalLineObject injectLine;

	private TerminalLineObject wpa2CrackKeys;

	private WifiNetworkDefinition anticheatWifiTag;
}
