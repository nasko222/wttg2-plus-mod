using System;
using System.Text.RegularExpressions;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class Cookie
	{
		public Cookie(CookieManager cookies)
		{
			this.cookies = cookies;
		}

		internal Cookie(CookieManager cookies, BrowserNative.NativeCookie cookie)
		{
			this.cookies = cookies;
			this.original = cookie;
			Cookie.Copy(this.original, this);
		}

		public static void Init()
		{
		}

		public void Delete()
		{
			if (this.original == null)
			{
				return;
			}
			BrowserNative.zfb_editCookie(this.cookies.browser.browserId, this.original, BrowserNative.CookieAction.Delete);
			this.original = null;
		}

		public void Update()
		{
			if (this.original != null)
			{
				this.Delete();
			}
			this.original = new BrowserNative.NativeCookie();
			Cookie.Copy(this, this.original);
			BrowserNative.zfb_editCookie(this.cookies.browser.browserId, this.original, BrowserNative.CookieAction.Create);
		}

		public static void Copy(BrowserNative.NativeCookie src, Cookie dest)
		{
			dest.name = src.name;
			dest.value = src.value;
			dest.domain = src.domain;
			dest.path = src.path;
			Func<string, DateTime> func = delegate(string s)
			{
				Match match = Cookie.dateRegex.Match(s);
				return new DateTime(int.Parse(match.Groups[1].ToString()), int.Parse(match.Groups[2].ToString()), int.Parse(match.Groups[3].ToString()), int.Parse(match.Groups[4].ToString()), int.Parse(match.Groups[5].ToString()), int.Parse(match.Groups[6].ToString()), int.Parse(match.Groups[7].ToString()));
			};
			dest.creation = func(src.creation);
			dest.expires = ((src.expires != null) ? new DateTime?(func(src.expires)) : null);
			dest.lastAccess = func(src.lastAccess);
			dest.secure = (src.secure != 0);
			dest.httpOnly = (src.httpOnly != 0);
		}

		public static void Copy(Cookie src, BrowserNative.NativeCookie dest)
		{
			dest.name = src.name;
			dest.value = src.value;
			dest.domain = src.domain;
			dest.path = src.path;
			Func<DateTime, string> func = (DateTime s) => s.ToString("yyyy-MM-dd hh:mm:ss.fff");
			dest.creation = func(src.creation);
			DateTime? dateTime = src.expires;
			dest.expires = ((dateTime != null) ? func(src.expires.Value) : null);
			dest.lastAccess = func(src.lastAccess);
			dest.secure = ((!src.secure) ? 0 : 1);
			dest.httpOnly = ((!src.httpOnly) ? 0 : 1);
		}

		private CookieManager cookies;

		private BrowserNative.NativeCookie original;

		public string name = string.Empty;

		public string value = string.Empty;

		public string domain = string.Empty;

		public string path = string.Empty;

		public DateTime creation;

		public DateTime lastAccess;

		public DateTime? expires;

		public bool secure;

		public bool httpOnly;

		private static readonly Regex dateRegex = new Regex("(\\d{4})-(\\d{2})-(\\d{2}) (\\d{2}):(\\d{2}):(\\d{2}).(\\d{3})");
	}
}
