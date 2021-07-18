using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	[RequireComponent(typeof(Browser))]
	public class ExplainUnzip : MonoBehaviour
	{
		public void Start()
		{
			Browser browser = base.GetComponent<Browser>();
			browser.onFetch += delegate(JSONNode data)
			{
				if (data["status"] == 404)
				{
					browser.LoadHTML(Resources.Load<TextAsset>("ExplainUnzip").text, null);
					if (HUDManager.Instance)
					{
						HUDManager.Instance.Pause();
					}
					Time.timeScale = 1f;
				}
			};
			browser.onFetchError += delegate(JSONNode data)
			{
				if (data["error"] == "ERR_ABORTED")
				{
					browser.QueuePageReplacer(delegate
					{
					}, 1f);
				}
			};
		}
	}
}
