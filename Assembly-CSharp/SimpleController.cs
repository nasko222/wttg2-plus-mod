using System;
using UnityEngine;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

[RequireComponent(typeof(Browser))]
public class SimpleController : MonoBehaviour
{
	public void Start()
	{
		this.browser = base.GetComponent<Browser>();
	}

	public void GoToURLInput()
	{
		this.browser.Url = this.urlInput.text;
	}

	private Browser browser;

	public InputField urlInput;
}
