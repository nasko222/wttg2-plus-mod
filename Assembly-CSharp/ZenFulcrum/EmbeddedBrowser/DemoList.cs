using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	[RequireComponent(typeof(Browser))]
	public class DemoList : MonoBehaviour
	{
		protected void Start()
		{
			this.panelBrowser = base.GetComponent<Browser>();
			this.panelBrowser.RegisterFunction("go", delegate(JSONNode args)
			{
				this.DemoNav(args[0].Check());
			});
			this.demoBrowser.onLoad += delegate(JSONNode info)
			{
				this.panelBrowser.CallFunction("setDisplayedUrl", new JSONNode[]
				{
					this.demoBrowser.Url
				});
			};
			this.demoBrowser.Url = this.demoSites[0];
		}

		private void DemoNav(int dir)
		{
			if (dir > 0)
			{
				this.currentIndex = (this.currentIndex + 1) % this.demoSites.Count;
			}
			else
			{
				this.currentIndex = (this.currentIndex - 1 + this.demoSites.Count) % this.demoSites.Count;
			}
			this.demoBrowser.Url = this.demoSites[this.currentIndex];
		}

		protected List<string> demoSites = new List<string>
		{
			"localGame://demo/MouseShow.html",
			"http://js1k.com/2013-spring/demo/1487",
			"http://js1k.com/2014-dragons/demo/1868",
			"http://glimr.rubyforge.org/cake/missile_fleet.html",
			"http://js1k.com/2015-hypetrain/demo/2231",
			"http://js1k.com/2015-hypetrain/demo/2313",
			"http://js1k.com/2015-hypetrain/demo/2331",
			"http://js1k.com/2015-hypetrain/demo/2315",
			"http://js1k.com/2015-hypetrain/demo/2161",
			"http://js1k.com/2013-spring/demo/1533",
			"http://js1k.com/2014-dragons/demo/1969",
			"http://www.snappymaria.com/misc/TouchEventTest.html"
		};

		public Browser demoBrowser;

		private Browser panelBrowser;

		private int currentIndex;
	}
}
