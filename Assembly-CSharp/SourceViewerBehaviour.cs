using System;
using UnityEngine;

public class SourceViewerBehaviour : WindowBehaviour
{
	public void ViewSourceCode(string SetHTML, bool DoSetHTML = true)
	{
		if (DoSetHTML)
		{
			this.currentHTML = SetHTML;
		}
		if (!this.Window.activeSelf)
		{
			base.Launch();
		}
		else
		{
			this.Window.GetComponent<BringWindowToFrontBehaviour>().forceTap();
			this.buildHTML();
		}
	}

	protected override void OnLaunch()
	{
		if (!this.Window.activeSelf)
		{
			this.buildHTML();
		}
	}

	protected override void OnClose()
	{
	}

	protected override void OnMin()
	{
	}

	protected override void OnUnMin()
	{
	}

	protected override void OnMax()
	{
		this.buildHTML();
	}

	protected override void OnUnMax()
	{
		this.buildHTML();
	}

	protected override void OnResized()
	{
		this.buildHTML();
	}

	private void buildHTML()
	{
		this.notesObject.GetComponent<NoteObject>().BuildMe(this.currentHTML, SOFTWARE_PRODUCTS.SOURCE_VIEWER);
		this.notesObject.GetComponent<RectTransform>().anchoredPosition = this.notesStartPOS;
		this.resizeContentHolder();
	}

	private void resizeContentHolder()
	{
		this.sourceObjectHolderSize.x = LookUp.DesktopUI.SOURCE_WINDOW_OBJECT_HOLDER.sizeDelta.x;
		this.sourceObjectHolderSize.y = -5f + this.notesObject.GetComponent<RectTransform>().sizeDelta.y;
		LookUp.DesktopUI.SOURCE_WINDOW_OBJECT_HOLDER.sizeDelta = this.sourceObjectHolderSize;
	}

	protected new void Awake()
	{
		base.Awake();
		this.notesObject = LookUp.DesktopUI.SOURCE_NOTES_OBJECT;
		this.currentHTML = string.Empty;
		GameManager.BehaviourManager.SourceViewerBehaviour = this;
	}

	private const float START_SET_Y = -5f;

	private const float NOTE_OBJ_SET_X = 5f;

	private const float NOTE_OBJ_BOT_SPACING = 5f;

	private const float NOTES_CONTENT_BOT_SPACE = 10f;

	private GameObject notesObject;

	private Vector2 notesStartPOS = new Vector2(5f, -5f);

	private Vector2 sourceObjectHolderSize = Vector2.zero;

	private string currentHTML;
}
