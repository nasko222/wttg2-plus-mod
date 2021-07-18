using System;
using System.Collections.Generic;
using UnityEngine;

public class skyBreakWEPBehavior : MonoBehaviour
{
	public void CloseMeOut()
	{
		this.currentTargetedWEP = null;
		GameManager.TweenSlinger.KillTween(this.skyBREAKWepPortProbe);
		GameManager.TweenSlinger.KillTween(this.wepUpdateKeysTested);
		GameManager.TimeSlinger.KillTimer(this.wepCrackedTimer);
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
					if (!(text == "probe"))
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
						this.PerformProbe(theCMD);
					}
				}
				else
				{
					result = false;
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
			List<WifiNetworkDefinition> secureNetworks = GameManager.ManagerSlinger.WifiManager.GetSecureNetworks(WIFI_SECURITY.WEP);
			float num = 0.3f;
			string setLine = string.Concat(new string[]
			{
				MagicSlinger.FluffString("ESSID", " ", 20),
				"  ",
				MagicSlinger.FluffString("BSSID", " ", 20),
				"  ",
				MagicSlinger.FluffString("CH", " ", 10),
				"  ",
				MagicSlinger.FluffString("RATE", " ", 10),
				"  ",
				MagicSlinger.FluffString("PROB", " ", 10),
				"  ",
				MagicSlinger.FluffString("PING", " ", 10),
				"  SIG"
			});
			this.myTerminalHelper.ClearTerminal();
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "Scanned WEP Network Results", 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.FADE, setLine, 0.3f, 0f);
			for (int i = 0; i < secureNetworks.Count; i++)
			{
				float num2 = secureNetworks[i].networkTrackRate;
				if (DataManager.LeetMode)
				{
					num2 *= 0.7f;
				}
				int num3 = (int)num2;
				int num4 = Mathf.RoundToInt(secureNetworks[i].networkTrackProbability * 100f);
				string setLine2 = string.Concat(new string[]
				{
					MagicSlinger.FluffString(secureNetworks[i].networkName, " ", 20),
					"  ",
					MagicSlinger.FluffString(secureNetworks[i].networkBSSID, " ", 20),
					"  ",
					MagicSlinger.FluffString(secureNetworks[i].networkChannel.ToString(), " ", 10),
					"  ",
					MagicSlinger.FluffString(num3.ToString(), " ", 10),
					"  ",
					MagicSlinger.FluffString(num4.ToString() + "%", " ", 10),
					"  ",
					MagicSlinger.FluffString(secureNetworks[i].networkPower.ToString(), " ", 10),
					"  ",
					MagicSlinger.GetWifiSiginalType(secureNetworks[i].networkSignal)
				});
				this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.FADE, setLine2, 0.2f, num);
				num += 0.2f;
			}
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0f, 0f);
			GameManager.TimeSlinger.FireTimer(num, new Action(this.mySkyBreakBehavior.addCMDInputLine), 0);
		}
	}

	private void PerformProbe(string theCMD)
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
			setLine = "Auto WiFi Crack is enabled. Probe is disabled.";
		}
		if (array.Length < 4 && !ModsManager.SBGlitch)
		{
			flag = false;
			setLine = "Invalid usage of probe. Valid usage: probe <BSSID> <Port Start> <Port End>";
		}
		if (flag)
		{
			if (GameManager.ManagerSlinger.WifiManager.CheckBSSID(array[1], out this.currentTargetedWEP))
			{
				if (!int.TryParse(array[2], out num))
				{
					flag = false;
					setLine = "Invalid port number. Valid port range is 1 - 1000.";
				}
				if (!int.TryParse(array[3], out num2))
				{
					flag = false;
					setLine = "Invalid port number. Valid port range is 1 - 1000.";
				}
			}
			else
			{
				flag = false;
				setLine = "Could not find a network with the BSSID Of '" + array[1] + "'";
			}
		}
		if (flag && (num > 1000 || num < 1 || num2 > 1000 || num2 < 1))
		{
			flag = false;
			setLine = "Invalid port number. Valid port range is 1 - 1000.";
		}
		if (flag && num2 < num)
		{
			flag = false;
			setLine = "Invalid port entry. Ending port can not be greater then starting port.";
		}
		if (flag)
		{
			this.mySkyBreakBehavior.SpitLastCMD(theCMD);
			this.DoProbe(num, num2);
			return;
		}
		this.mySkyBreakBehavior.SpitLastCMD(theCMD);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, setLine, 0f, 0f);
		this.myTerminalHelper.PushInputLineToBottom();
	}

	private void DoProbe(int targetStartingPort, int targetEndingPort)
	{
		this.myTerminalHelper.ClearInputLine();
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Concat(new string[]
		{
			"Targeting '",
			this.currentTargetedWEP.networkBSSID,
			"' - '",
			this.currentTargetedWEP.networkName,
			"'"
		}), 0f, 0f);
		this.myTerminalHelper.AddLine(out this.wepProbeLine, TERMINAL_LINE_TYPE.HARD, "Probing port: " + targetStartingPort, 0f, 0f);
		this.currentTargetedWEPMaxPort = targetEndingPort;
		this.skyBREAKWepPortProbe = GameManager.TweenSlinger.PlayDOSTweenLiner((float)targetStartingPort, (float)targetEndingPort, (float)(targetEndingPort - targetStartingPort) * 0.1f, new Action<float>(this.UpdateProbeLine));
	}

	private void UpdateProbeLine(float setValue)
	{
		int num = Mathf.RoundToInt(setValue);
		if ((short)num == this.currentTargetedWEP.networkOpenPort)
		{
			GameManager.TweenSlinger.KillTween(this.skyBREAKWepPortProbe);
			this.ProbeFoundPort();
		}
		else
		{
			if (this.wepProbeLine != null)
			{
				this.wepProbeLine.UpdateMyText("Probing port: " + num);
			}
			if (num == this.currentTargetedWEPMaxPort)
			{
				GameManager.TweenSlinger.KillTween(this.skyBREAKWepPortProbe);
				this.ProbeNoPortFound();
			}
		}
	}

	private void ProbeFoundPort()
	{
		if (this.mySkyBreakBehavior.Window.activeSelf)
		{
			this.wepProbeLine.HardClear();
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Concat(new string[]
			{
				"FOUND OPEN PORT! ON '",
				this.currentTargetedWEP.networkBSSID,
				"' - '",
				this.currentTargetedWEP.networkName,
				"'"
			}), 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "Open Port #:" + this.currentTargetedWEP.networkOpenPort, 0f, 0f);
			this.mySkyBreakBehavior.addCMDInputLine();
		}
	}

	private void ProbeNoPortFound()
	{
		if (this.mySkyBreakBehavior.Window.activeSelf)
		{
			this.wepProbeLine.HardClear();
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Concat(new string[]
			{
				"Could not find an open port on '",
				this.currentTargetedWEP.networkBSSID,
				"' - '",
				this.currentTargetedWEP.networkName,
				"'"
			}), 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, "Try a differnt port range.", 0f, 0f);
			this.mySkyBreakBehavior.addCMDInputLine();
		}
	}

	private void PerformCrack(string theCMD)
	{
		string setLine = string.Empty;
		bool flag = true;
		int num = -1;
		int num2 = 1;
		string[] array = theCMD.Split(new string[]
		{
			" "
		}, StringSplitOptions.None);
		if (ModsManager.SBGlitch)
		{
			if (array.Length < 3)
			{
				flag = false;
				setLine = "Invalid usage of crack. Valid usage: crack <BSSID> <CH>";
			}
		}
		else if (array.Length < 4)
		{
			flag = false;
			setLine = "Invalid usage of crack. Valid usage: crack <BSSID> <CH> <PORT>";
		}
		if (flag)
		{
			if (GameManager.ManagerSlinger.WifiManager.CheckBSSID(array[1], out this.currentTargetedWEP))
			{
				if (!int.TryParse(array[2], out num))
				{
					flag = false;
					setLine = "Invalid channel number.";
				}
				if (!ModsManager.SBGlitch && !int.TryParse(array[3], out num2))
				{
					flag = false;
					setLine = "Invalid port number.";
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
			if (this.currentTargetedWEP.networkChannel != (short)num)
			{
				flag = false;
				setLine = "Could not connect to network '" + this.currentTargetedWEP.networkBSSID + "' on channel:" + num.ToString();
			}
			if (this.currentTargetedWEP.networkOpenPort != (short)num2 && !ModsManager.SBGlitch)
			{
				flag = false;
				setLine = "Could not connect to network '" + this.currentTargetedWEP.networkBSSID + "' with port:" + num2.ToString();
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
		this.anticheatWifiTag = this.currentTargetedWEP;
		this.myTerminalHelper.ClearInputLine();
		this.myTerminalHelper.ClearTerminal();
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, MagicSlinger.FluffString(" ", " ", 24) + "    skyBREAK 1.5.89", 0f, 0f);
		this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0f, 0f);
		string setLine = MagicSlinger.FluffString(" ", " ", 16) + "    [10 keys tested (500 k/s)]";
		this.myTerminalHelper.AddLine(out this.wepCrackKeys, TERMINAL_LINE_TYPE.HARD, setLine, 0f, 0f);
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
		this.wepUpdateKeysTested = GameManager.TweenSlinger.PlayDOSTweenLiner(0f, setToValue, num, new Action<float>(this.UpdateKeysTested));
		GameManager.TimeSlinger.FireHardTimer(out this.wepCrackedTimer, num, new Action(this.Cracked), 0);
	}

	private void UpdateKeysTested(float setValue)
	{
		if (this.mySkyBreakBehavior.Window.activeSelf)
		{
			this.wepCrackKeys.UpdateMyText(MagicSlinger.FluffString(" ", " ", 16) + "    [" + Mathf.RoundToInt(setValue).ToString() + " keys tested (500 k/s)]");
		}
	}

	private void Cracked()
	{
		if (this.mySkyBreakBehavior.Window.activeSelf)
		{
			if (!ModsManager.SBGlitch)
			{
				this.currentTargetedWEP = this.anticheatWifiTag;
			}
			GameManager.TweenSlinger.KillTween(this.wepUpdateKeysTested);
			this.myTerminalHelper.KillCrackLines();
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Concat(new string[]
			{
				MagicSlinger.FluffString(" ", " ", 11),
				"'",
				this.currentTargetedWEP.networkBSSID,
				"' - '",
				this.currentTargetedWEP.networkName,
				"' HAS BEEN CRACKED!"
			}), 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, MagicSlinger.FluffString(" ", " ", 20) + "    PASSWORD FOUND! [" + this.currentTargetedWEP.networkPassword + "]", 0f, 0f);
			this.myTerminalHelper.AddLine(TERMINAL_LINE_TYPE.HARD, string.Empty, 0f, 0f);
			this.mySkyBreakBehavior.addCMDInputLine();
			SteamSlinger.Ins.CrackWifiNetwork(this.currentTargetedWEP.GetHashCode());
		}
	}

	private float GetRandScanTime()
	{
		return UnityEngine.Random.Range(this.minScanTime, this.maxScanTime);
	}

	private void PrepMe()
	{
		this.mySkyBreakBehavior = base.GetComponent<skyBreakBehavior>();
		this.myTerminalHelper = base.GetComponent<TerminalHelperBehavior>();
	}

	private void Start()
	{
		this.PrepMe();
	}

	[Range(3.5f, 8f)]
	public float minScanTime = 4f;

	[Range(5.5f, 15f)]
	public float maxScanTime = 10f;

	[Range(5f, 30f)]
	public float minCrackTime = 8f;

	[Range(15f, 120f)]
	public float maxCrackTime = 45f;

	private WifiNetworkDefinition currentTargetedWEP;

	private int currentTargetedWEPMaxPort;

	private skyBreakBehavior mySkyBreakBehavior;

	private TerminalHelperBehavior myTerminalHelper;

	private DOSTween wepUpdateKeysTested;

	private DOSTween skyBREAKWepPortProbe;

	private Timer wepCrackedTimer;

	private TerminalLineObject wepProbeLine;

	private TerminalLineObject wepCrackKeys;

	private WifiNetworkDefinition anticheatWifiTag;
}
