using System;
using System.Collections.Generic;
using UnityEngine;

public class NotesBehaviour : WindowBehaviour
{
	public void AddNote(string NoteToAdd)
	{
		if (NoteToAdd != string.Empty)
		{
			if (NoteToAdd == "20/20/20/20" && !ModsManager.Nightmare && !ModsManager.EasyModeActive && !DataManager.LeetMode)
			{
				GameManager.TheCloud.attemptNightmare();
				return;
			}
			if (this.noteData != null)
			{
				this.noteData.Notes.Add(NoteToAdd);
				DataManager.Save<NotesData>(this.noteData);
			}
			this.currentNotes.Add(NoteToAdd);
			this.buildNotes();
		}
	}

	public void ClearNotes()
	{
		if (this.noteData != null)
		{
			this.noteData.Notes.Clear();
			DataManager.Save<NotesData>(this.noteData);
		}
		this.currentNotes.Clear();
		this.buildNotes();
	}

	protected override void OnLaunch()
	{
		if (!this.Window.activeSelf)
		{
			this.buildNotes();
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
		this.buildNotes();
	}

	protected override void OnUnMax()
	{
		this.buildNotes();
	}

	protected override void OnResized()
	{
		this.buildNotes();
	}

	private void buildNotes()
	{
		string text = string.Empty;
		for (int i = 0; i < this.currentNotes.Count; i++)
		{
			text = text + this.currentNotes[i] + "\n\r\n\r";
		}
		this.notesObject.GetComponent<NoteObject>().BuildMe(text, SOFTWARE_PRODUCTS.NOTES);
		this.notesObject.GetComponent<RectTransform>().anchoredPosition = this.notesStartPOS;
		this.resizeContentHolder();
	}

	private void resizeContentHolder()
	{
		this.notesObjectHolderSize.x = LookUp.DesktopUI.NOTES_WINDOW_OBJECT_HOLDER.sizeDelta.x;
		this.notesObjectHolderSize.y = -5f + this.notesObject.GetComponent<RectTransform>().sizeDelta.y + 5f;
		LookUp.DesktopUI.NOTES_WINDOW_OBJECT_HOLDER.sizeDelta = this.notesObjectHolderSize;
		LookUp.DesktopUI.NOTES_WINDOW_CONTENT.normalizedPosition = Vector2.zero;
	}

	private void stageMe()
	{
		this.noteData = DataManager.Load<NotesData>(this.noteID);
		if (this.noteData == null)
		{
			this.noteData = new NotesData(this.noteID);
			this.noteData.Notes = new List<string>(50);
		}
		this.currentNotes = new List<string>(this.noteData.Notes);
		this.buildNotes();
		GameManager.StageManager.Stage -= this.stageMe;
	}

	protected new void Awake()
	{
		this.noteID = 123;
		base.Awake();
		GameManager.BehaviourManager.NotesBehaviour = this;
		GameManager.StageManager.Stage += this.stageMe;
		this.notesObject = LookUp.DesktopUI.NOTES_NOTES_OBJECT;
	}

	private const float START_SET_Y = -5f;

	private const float NOTE_OBJ_SET_X = 5f;

	private const float NOTE_OBJ_BOT_SPACING = 5f;

	private const float NOTES_CONTENT_BOT_SPACE = 10f;

	private GameObject notesObject;

	private List<string> currentNotes = new List<string>();

	private Vector2 notesStartPOS = new Vector2(5f, -5f);

	private Vector2 notesObjectHolderSize = Vector2.zero;

	private int noteID;

	private NotesData noteData;
}
