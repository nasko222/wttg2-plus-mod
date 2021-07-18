using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class AnnURLInputBehaviour : MonoBehaviour
{
	public void InputURL(string setURL)
	{
		setURL = setURL.Replace("\n", string.Empty);
		if (setURL != string.Empty)
		{
			GameManager.BehaviourManager.AnnBehaviour.GotoURL(setURL, true);
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
				this.InputURL(this.inputLine.text);
			}
		}
		else
		{
			this.inputLine.DeactivateInputField();
		}
	}

	private InputField inputLine;
}
