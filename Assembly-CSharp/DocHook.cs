using System;
using UnityEngine;
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
		this.docURL = "localGame://" + this.documentData.DocFolder + "/index.html";
		this.docBrowser.onLoad += this.pageLoaded;
		SteamSlinger.Ins.AddTutDoc(this.documentData.GetHashCode());
	}

	private const string BASE_DOC_URL = "localGame://";

	[SerializeField]
	private DocDefinition documentData;

	[SerializeField]
	private Browser docBrowser;

	private string docURL;
}
