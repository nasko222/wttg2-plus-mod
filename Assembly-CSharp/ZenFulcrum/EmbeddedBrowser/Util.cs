using System;

namespace ZenFulcrum.EmbeddedBrowser
{
	public static class Util
	{
		public static bool SafeStartsWith(this string check, string starter)
		{
			if (check == null || starter == null)
			{
				return false;
			}
			if (check.Length < starter.Length)
			{
				return false;
			}
			for (int i = 0; i < starter.Length; i++)
			{
				if (check[i] != starter[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
