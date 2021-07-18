using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class MagicSlinger
{
	public static string MD5It(string hash)
	{
		MD5 md = MD5.Create();
		byte[] array = md.ComputeHash(Encoding.Default.GetBytes(hash));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	public static Vector2 GetXYByPerc(float xPer, float yPer)
	{
		return new Vector2((float)Screen.width * xPer, (float)Screen.height * yPer);
	}

	public static float GetScreenWidthPXByPerc(float per)
	{
		return (float)Screen.width * per;
	}

	public static float GetScreenHeightPXByPerc(float per)
	{
		return (float)Screen.height * per;
	}

	public static float GetPercOfSize(float size1, float size2)
	{
		return size1 / size2;
	}

	public static bool InRange(float checkValue, float minValue, float maxValue)
	{
		return checkValue >= minValue && checkValue <= maxValue;
	}

	public static string FluffString(string theString, string fluffValue, int targetFluffAmt)
	{
		string text = theString;
		if (text.Length < targetFluffAmt)
		{
			int num = targetFluffAmt - text.Length;
			for (int i = 0; i < num; i++)
			{
				text += fluffValue;
			}
		}
		return text;
	}

	public static string GenerateRandomAlphaNumHash(int hashLen)
	{
		string text = string.Empty;
		for (int i = 0; i < hashLen; i++)
		{
			text += MagicSlinger._alphaNum[UnityEngine.Random.Range(0, MagicSlinger._alphaNum.Length)];
		}
		return text;
	}

	public static string GenerateRandomHexCode(int hexLenPerSeq, int hexSeqs, string hexSpacer)
	{
		string text = string.Empty;
		for (int i = 0; i < hexSeqs; i++)
		{
			text += MagicSlinger.GenerateRandomAlphaNumHash(hexLenPerSeq);
			if (i < hexSeqs - 1)
			{
				text += hexSpacer;
			}
		}
		return text;
	}

	public static float GetStringHeight(string theString, Font setFont, int setFontSize, Vector2 extends)
	{
		TextGenerationSettings settings = default(TextGenerationSettings);
		TextGenerator textGenerator = new TextGenerator();
		settings.textAnchor = TextAnchor.UpperLeft;
		settings.generateOutOfBounds = true;
		settings.generationExtents = extends;
		settings.pivot = Vector2.zero;
		settings.richText = false;
		settings.font = setFont;
		settings.fontSize = setFontSize;
		settings.fontStyle = FontStyle.Normal;
		settings.lineSpacing = 1f;
		settings.scaleFactor = 1f;
		settings.resizeTextForBestFit = false;
		settings.verticalOverflow = VerticalWrapMode.Overflow;
		settings.horizontalOverflow = HorizontalWrapMode.Wrap;
		return textGenerator.GetPreferredHeight(theString, settings);
	}

	public static float GetStringWidth(string theString, Font setFont, int setFontSize, Vector2 extends)
	{
		TextGenerationSettings settings = default(TextGenerationSettings);
		TextGenerator textGenerator = new TextGenerator();
		settings.textAnchor = TextAnchor.UpperLeft;
		settings.generateOutOfBounds = true;
		settings.generationExtents = extends;
		settings.pivot = Vector2.zero;
		settings.richText = false;
		settings.font = setFont;
		settings.fontSize = setFontSize;
		settings.fontStyle = FontStyle.Normal;
		settings.lineSpacing = 1f;
		settings.scaleFactor = 1f;
		settings.verticalOverflow = VerticalWrapMode.Truncate;
		settings.horizontalOverflow = HorizontalWrapMode.Overflow;
		return textGenerator.GetPreferredWidth(theString, settings);
	}

	public static string GetNetworkSecurityType(WIFI_SECURITY securityLevel)
	{
		string result = "None";
		if (securityLevel != WIFI_SECURITY.WEP)
		{
			if (securityLevel != WIFI_SECURITY.WPA)
			{
				if (securityLevel == WIFI_SECURITY.WPA2)
				{
					result = "WPA2";
				}
			}
			else
			{
				result = "WPA";
			}
		}
		else
		{
			result = "WEP";
		}
		return result;
	}

	public static string GetWifiSiginalType(WIFI_SIGNAL_TYPE signalType)
	{
		string result = "Unknown";
		switch (signalType)
		{
		case WIFI_SIGNAL_TYPE.W80211B:
			result = "802.11B";
			break;
		case WIFI_SIGNAL_TYPE.W80211BP:
			result = "802.11B+";
			break;
		case WIFI_SIGNAL_TYPE.W80211G:
			result = "802.11G";
			break;
		case WIFI_SIGNAL_TYPE.W80211N:
			result = "802.11N";
			break;
		case WIFI_SIGNAL_TYPE.W80211AC:
			result = "802.11AC";
			break;
		}
		return result;
	}

	public static string GetFriendlyLocationName(PLAYER_LOCATION SetLocation)
	{
		string result = "Unknown";
		switch (SetLocation)
		{
		case PLAYER_LOCATION.MAIN_ROON:
			result = "Main Room";
			break;
		case PLAYER_LOCATION.BATH_ROOM:
			result = "Bath Room";
			break;
		case PLAYER_LOCATION.HALL_WAY10:
			result = "Floor 10 Hallway";
			break;
		case PLAYER_LOCATION.HALL_WAY8:
			result = "Floor 8 Hallway";
			break;
		case PLAYER_LOCATION.HALL_WAY6:
			result = "Floor 6 Hallway";
			break;
		case PLAYER_LOCATION.HALL_WAY5:
			result = "Floor 5 Hallway";
			break;
		case PLAYER_LOCATION.HALL_WAY3:
			result = "Floor 3 Hallway";
			break;
		case PLAYER_LOCATION.HALL_WAY1:
			result = "Lobby Floor Hallway";
			break;
		case PLAYER_LOCATION.STAIR_WAY:
			result = "Stairway";
			break;
		case PLAYER_LOCATION.MAINTENANCE_ROOM:
			result = "Maintenace Room";
			break;
		case PLAYER_LOCATION.LOBBY:
			result = "Lobby";
			break;
		case PLAYER_LOCATION.OUTSIDE:
			result = "Outside";
			break;
		case PLAYER_LOCATION.DEAD_DROP:
			result = "Dead Drop";
			break;
		}
		return result;
	}

	public static float ClampAngle(float angle, float min, float max, bool wrap = true)
	{
		if (wrap)
		{
			while (angle < 0f)
			{
				angle += 360f;
			}
			while (angle >= 360f)
			{
				angle -= 360f;
			}
		}
		return MagicSlinger.Clamp(angle, min, max);
	}

	public static float Clamp(float value, float min, float max)
	{
		if (value < min)
		{
			value = min;
		}
		else if (value > max)
		{
			value = max;
		}
		return value;
	}

	private static string[] _alphaNum = new string[]
	{
		"A",
		"B",
		"C",
		"D",
		"E",
		"F",
		"G",
		"H",
		"I",
		"J",
		"K",
		"L",
		"N",
		"P",
		"Q",
		"R",
		"S",
		"T",
		"U",
		"V",
		"W",
		"X",
		"Y",
		"Z",
		"0",
		"1",
		"2",
		"3",
		"4",
		"5",
		"6",
		"7",
		"8",
		"9"
	};
}
