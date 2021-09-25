using System;
using UnityEngine;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

public class DocHook : MonoBehaviour
{
	public void OpenDoc()
	{
		this.docBrowser.LoadURL(this.docURL, false);
		SteamSlinger.Ins.ReadTutDoc(this.documentData.GetHashCode());
	}

	private void pageLoaded(JSONNode obj)
	{
		this.docBrowser.RegisterFunction("LinkHover", delegate(JSONNode args)
		{
			GameManager.ManagerSlinger.CursorManager.PointerCursorState(true);
		});
		this.docBrowser.RegisterFunction("LinkOut", delegate(JSONNode args)
		{
			GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
		});
		this.docBrowser.RegisterFunction("TestZone", delegate(JSONNode args)
		{
			GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
			GameManager.HackerManager.LaunchTestHack(HACK_TYPE.SWEEPER);
		});
		this.docBrowser.RegisterFunction("TestMem", delegate(JSONNode args)
		{
			GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
			GameManager.HackerManager.LaunchTestHack(HACK_TYPE.MEMDEFRAG);
		});
		this.docBrowser.RegisterFunction("TestStack", delegate(JSONNode args)
		{
			GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
			GameManager.HackerManager.LaunchTestHack(HACK_TYPE.STACKPUSHER);
		});
		this.docBrowser.RegisterFunction("TestNode", delegate(JSONNode args)
		{
			GameManager.ManagerSlinger.CursorManager.PointerCursorState(false);
			GameManager.HackerManager.LaunchTestHack(HACK_TYPE.NODEHEXER);
		});
	}

	private void Awake()
	{
		if (this.documentData.Title == "memD3FR4G3R")
		{
			this.documentData.Title = "Hacks";
			this.documentData.DocFolder = "DocHackers";
			foreach (Text text in UnityEngine.Object.FindObjectsOfType<Text>())
			{
				if (text.text == "memD3FR4G3R")
				{
					text.text = " Hacks";
				}
			}
		}
		if (this.documentData.Title == "stackPUSHER")
		{
			this.documentData.Title = "Viruses";
			this.documentData.DocFolder = "DocVirusTypes";
			foreach (Text text2 in UnityEngine.Object.FindObjectsOfType<Text>())
			{
				if (text2.text == "stackPUSHER")
				{
					text2.text = " Viruses";
				}
			}
		}
		this.docURL = "localGame://" + this.documentData.DocFolder + "/index.html";
		this.docBrowser.onLoad += this.pageLoaded;
		SteamSlinger.Ins.AddTutDoc(this.documentData.GetHashCode());
	}

	private const string BASE_DOC_URL = "localGame://";

	[SerializeField]
	public DocDefinition documentData;

	[SerializeField]
	private Browser docBrowser;

	private string docURL;
}
