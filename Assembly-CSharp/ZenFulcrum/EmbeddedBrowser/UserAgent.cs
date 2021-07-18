using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class UserAgent
	{
		public static string GetUserAgent()
		{
			if (UserAgent.agentOverride != null)
			{
				return UserAgent.agentOverride;
			}
			string text = Marshal.PtrToStringAnsi(BrowserNative.zfb_getVersion());
			string text2 = "Windows NT 6.1";
			string input = string.Concat(new string[]
			{
				"Mozilla/5.0 (",
				text2,
				"; Unity 3D; ZFBrowser 1.1.0; ",
				Application.productName,
				" ",
				Application.version,
				") AppleWebKit/537.36 (KHTML, like Gecko) Chrome/",
				text,
				" Safari/537.36"
			});
			return Regex.Replace(input, "[^\\u0020-\\u007E]", "?");
		}

		public static void SetUserAgent(string userAgent)
		{
			if (BrowserNative.NativeLoaded)
			{
				throw new InvalidOperationException("User Agent can only be changed before native backend is initialized.");
			}
			UserAgent.agentOverride = userAgent;
		}

		private static string agentOverride;
	}
}
