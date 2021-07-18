using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TerminalInputLineObject : MonoBehaviour
{
	public void SoftBuild(Action<string> SetAction)
	{
		this.myRT = base.GetComponent<RectTransform>();
		InputField.OnChangeEvent onChangeEvent = new InputField.OnChangeEvent();
		onChangeEvent.AddListener(new UnityAction<string>(this.veryifyInput));
		this.inputLine.onValueChanged = onChangeEvent;
		if (computerController.Ins != null)
		{
			computerController.Ins.LeaveEvents.Event += this.looseFocus;
		}
		this.myCallBackAction = SetAction;
		this.myRT.anchoredPosition = this.startPOS;
	}

	public void Move(float SetY)
	{
		this.myPOS.y = SetY;
		this.myRT.anchoredPosition = this.myPOS;
	}

	public void Clear()
	{
		this.Active = false;
		this.inputLine.text = string.Empty;
		this.titleText.GetComponent<Text>().text = string.Empty;
		this.myRT.anchoredPosition = this.startPOS;
	}

	public void InputCMD(string setCMD)
	{
		setCMD = setCMD.Replace("\n", string.Empty);
		if (setCMD != string.Empty && this.myCallBackAction != null)
		{
			this.myCallBackAction(setCMD);
		}
	}

	public void UpdateTitle(string titleString)
	{
		float stringWidth = MagicSlinger.GetStringWidth(titleString, this.titleText.GetComponent<Text>().font, this.titleText.GetComponent<Text>().fontSize, this.titleText.GetComponent<RectTransform>().sizeDelta);
		this.titleText.GetComponent<Text>().text = titleString;
		this.titleText.GetComponent<RectTransform>().sizeDelta = new Vector2(stringWidth, this.titleText.GetComponent<RectTransform>().sizeDelta.y);
		this.inputLine.GetComponent<RectTransform>().anchoredPosition = new Vector2(this.titleText.GetComponent<RectTransform>().anchoredPosition.x + stringWidth + 8f, this.inputLine.GetComponent<RectTransform>().anchoredPosition.y);
		this.inputLine.GetComponent<RectTransform>().sizeDelta = this.mySize;
	}

	private void veryifyInput(string setInput)
	{
		if (setInput.Contains("\n"))
		{
			this.InputCMD(setInput);
			this.inputLine.text = string.Empty;
		}
	}

	private void looseFocus()
	{
		if (this.inputLine != null)
		{
			this.inputLine.DeactivateInputField();
		}
	}

	private void OnDestroy()
	{
		this.myCallBackAction = null;
		computerController.Ins.LeaveEvents.Event -= this.looseFocus;
	}

	public GameObject titleText;

	public InputField inputLine;

	public bool Active;

	private Action<string> myCallBackAction;

	private Vector2 startPOS = new Vector2(0f, 10f);

	private Vector2 myPOS = Vector2.zero;

	private Vector2 mySize = new Vector2(0f, 20f);

	private RectTransform myRT;
}
