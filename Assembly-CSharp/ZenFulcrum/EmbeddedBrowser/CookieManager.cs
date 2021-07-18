using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class CookieManager
	{
		public CookieManager(Browser browser)
		{
			this.browser = browser;
		}

		public IPromise<List<Cookie>> GetCookies()
		{
			Cookie.Init();
			List<Cookie> ret = new List<Cookie>();
			if (!this.browser.IsReady || !this.browser.enabled)
			{
				return Promise<List<Cookie>>.Resolved(ret);
			}
			Promise<List<Cookie>> promise = new Promise<List<Cookie>>();
			BrowserNative.GetCookieFunc getCookieFunc = delegate(BrowserNative.NativeCookie cookie)
			{
				try
				{
					if (cookie == null)
					{
						this.browser.RunOnMainThread(delegate
						{
							promise.Resolve(ret);
						});
						CookieManager.cookieFuncs.Remove(promise);
					}
					else
					{
						ret.Add(new Cookie(this, cookie));
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			};
			BrowserNative.zfb_getCookies(this.browser.browserId, getCookieFunc);
			CookieManager.cookieFuncs[promise] = getCookieFunc;
			return promise;
		}

		public void ClearAll()
		{
			if (this.browser.DeferUnready(new Action(this.ClearAll)))
			{
				return;
			}
			BrowserNative.zfb_clearCookies(this.browser.browserId);
		}

		internal readonly Browser browser;

		private static readonly Dictionary<IPromise<List<Cookie>>, BrowserNative.GetCookieFunc> cookieFuncs = new Dictionary<IPromise<List<Cookie>>, BrowserNative.GetCookieFunc>();
	}
}
