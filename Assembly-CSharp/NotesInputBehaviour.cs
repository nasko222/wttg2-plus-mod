using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class NotesInputBehaviour : MonoBehaviour
{
	public void InputNote(string setNote)
	{
		setNote = setNote.Replace("\n", string.Empty);
		if (setNote != string.Empty)
		{
			GameManager.BehaviourManager.NotesBehaviour.AddNote(setNote);
			this.inputLine.text = string.Empty;
		}
	}

	private void Start()
	{
		this.inputLine = base.GetComponent<InputField>();
	}

	private void Update()
	{
		if (ControllerManager.Get<computerController>(GAME_CONTROLLER.COMPUTER).Active)
		{
			if (CrossPlatformInputManager.GetButtonDown("Return"))
			{
				this.InputNote(this.inputLine.text);
			}
		}
		else
		{
			this.inputLine.DeactivateInputField();
		}
	}

	private InputField inputLine;
}
