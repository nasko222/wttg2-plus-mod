using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalHelperBehavior : MonoBehaviour
{
	public TerminalInputLineObject TerminalInput
	{
		get
		{
			return this.terminalInputLine;
		}
	}

	public void FullClear()
	{
		this.terminalInputLine.Clear();
		this.ClearTerminal();
	}

	public TerminalLineObject BuildSoftLine(TERMINAL_LINE_TYPE LineType, string SetLine, float SetLength = 0f, float SetDelay = 0f)
	{
		TerminalLineObject terminalLineObject = this.terminalLineObjectPool.Pop();
		terminalLineObject.SoftLine = true;
		terminalLineObject.Build(LineType, SetLine, SetLength, SetDelay);
		return terminalLineObject;
	}

	public void AddSoftLine(TerminalLineObject SoftLine)
	{
		SoftLine.Move(this.getLatestY());
		this.currentTerminalLines.Add(SoftLine);
	}

	public void AddLine(TerminalLineDefinition TheLine)
	{
		this.AddLine(TheLine.terminalLineType, TheLine.terminalText, TheLine.terminalAniLength, TheLine.terminalDelayAmount);
	}

	public void AddLine(TERMINAL_LINE_TYPE LineType, string SetLine, float SetLength = 0f, float SetDelay = 0f)
	{
		TerminalLineObject terminalLineObject = this.terminalLineObjectPool.Pop();
		terminalLineObject.ClearLine += this.clearLine;
		terminalLineObject.Build(LineType, SetLine, SetLength, SetDelay);
		terminalLineObject.Move(this.getLatestY());
		this.currentTerminalLines.Add(terminalLineObject);
	}

	public void AddLine(out TerminalLineObject ReturnTerminalLineObject, TERMINAL_LINE_TYPE LineType, string SetLine, float SetLength = 0f, float SetDelay = 0f)
	{
		ReturnTerminalLineObject = this.terminalLineObjectPool.Pop();
		ReturnTerminalLineObject.ClearLine += this.clearLine;
		ReturnTerminalLineObject.HardClearLine += this.hardClearLine;
		ReturnTerminalLineObject.Build(LineType, SetLine, SetLength, SetDelay);
		ReturnTerminalLineObject.Move(this.getLatestY());
		this.currentTerminalLines.Add(ReturnTerminalLineObject);
	}

	public void AddInputLine(Action<string> SetCallBack, string SetTitle = "")
	{
		this.myCallBackAction = SetCallBack;
		if (SetTitle != string.Empty)
		{
			this.terminalInputLine.UpdateTitle(SetTitle);
		}
		this.terminalInputLine.inputLine.ActivateInputField();
		this.terminalInputLine.Move(this.getLatestY());
		this.terminalInputLine.Active = true;
	}

	public void ClearTerminal()
	{
		for (int i = 0; i < this.currentTerminalLines.Count; i++)
		{
			this.currentTerminalLines[i].Clear();
		}
		this.currentTerminalLines.Clear();
		this.terminalContentSize.x = this.terminalContentBox.GetComponent<RectTransform>().sizeDelta.x;
		this.terminalContentSize.y = 300f;
		this.terminalContentBox.GetComponent<RectTransform>().sizeDelta = this.terminalContentSize;
		this.terminalContentHolder.GetComponent<ScrollRect>().normalizedPosition = Vector2.zero;
	}

	public void ClearInputLine()
	{
		this.terminalInputLine.Clear();
	}

	public void PushInputLineToBottom()
	{
		this.terminalInputLine.Move(this.getLatestY());
	}

	public void UpdateTerminalContentScrollHeight()
	{
		this.terminalContentSize.x = this.terminalContentBox.GetComponent<RectTransform>().sizeDelta.x;
		this.terminalContentSize.y = Mathf.Abs(this.getLatestY());
		this.terminalContentSize.y = this.terminalContentSize.y + 20f + 20f;
		this.terminalContentBox.GetComponent<RectTransform>().sizeDelta = this.terminalContentSize;
		this.terminalContentHolder.GetComponent<ScrollRect>().normalizedPosition = Vector2.zero;
	}

	public void UpdateTerminalContentScrollHeightHackingDump()
	{
		this.terminalContentSize.x = this.terminalContentBox.GetComponent<RectTransform>().sizeDelta.x;
		this.terminalContentSize.y = Mathf.Abs(this.getLatestY());
		this.terminalContentSize.y = this.terminalContentSize.y + 20f - 25f;
		this.terminalContentBox.GetComponent<RectTransform>().sizeDelta = this.terminalContentSize;
		this.terminalContentHolder.GetComponent<ScrollRect>().normalizedPosition = Vector2.zero;
	}

	public void KillCrackLines()
	{
		for (int i = 0; i < this.currentTerminalLines.Count; i++)
		{
			this.currentTerminalLines[i].KillCrackLine();
		}
	}

	private void clearLine(TerminalLineObject TheLine)
	{
		TheLine.ClearLine -= this.clearLine;
		TheLine.HardClearLine -= this.hardClearLine;
		this.terminalLineObjectPool.Push(TheLine);
	}

	private void hardClearLine(TerminalLineObject TheLine)
	{
		TheLine.ClearLine -= this.clearLine;
		TheLine.HardClearLine -= this.hardClearLine;
		this.terminalLineObjectPool.Push(TheLine);
		this.currentTerminalLines.Remove(TheLine);
	}

	private float getLatestY()
	{
		return -(18f + (float)this.currentTerminalLines.Count * 20f);
	}

	private void processCMD(string theCMD)
	{
		if (this.lastCommands.Count >= 1)
		{
			if (this.lastCommands[this.lastCommands.Count - 1] != theCMD)
			{
				this.lastCommands.Add(theCMD);
			}
			this.lastCommandIndex = this.lastCommands.Count - 1;
		}
		else
		{
			this.lastCommands.Add(theCMD);
			this.lastCommandIndex = 0;
		}
		this.myCallBackAction(theCMD);
	}

	private void getLastCommand()
	{
		if (this.lastCommandIndex >= 0 && this.lastCommands.Count != 0 && this.lastCommands[this.lastCommandIndex] != null)
		{
			this.terminalInputLine.inputLine.text = this.lastCommands[this.lastCommandIndex];
			this.terminalInputLine.inputLine.MoveTextEnd(false);
			this.lastCommandIndex--;
			if (this.lastCommandIndex < 0)
			{
				this.lastCommandIndex = 0;
			}
		}
	}

	private void getNextCommand()
	{
		int num = this.lastCommandIndex + 1;
		if (num == this.lastCommands.Count)
		{
			this.terminalInputLine.inputLine.text = string.Empty;
		}
		else if (num < this.lastCommands.Count && this.lastCommands[num] != null)
		{
			this.lastCommandIndex = num;
			this.terminalInputLine.inputLine.text = this.lastCommands[this.lastCommandIndex];
			this.terminalInputLine.inputLine.MoveTextEnd(false);
		}
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.terminalInputLine = UnityEngine.Object.Instantiate<GameObject>(this.terminalInputLineObject, this.terminalContentBox.GetComponent<RectTransform>()).GetComponent<TerminalInputLineObject>();
		this.terminalInputLine.SoftBuild(new Action<string>(this.processCMD));
	}

	private void Awake()
	{
		this.terminalLineObjectPool = new PooledStack<TerminalLineObject>(delegate()
		{
			TerminalLineObject component = UnityEngine.Object.Instantiate<GameObject>(this.terminalLineObject, this.terminalContentBox.GetComponent<RectTransform>()).GetComponent<TerminalLineObject>();
			component.SoftBuild();
			return component;
		}, this.STARTING_TERMINAL_LINE_POOL);
		this.clearLineAction = new TerminalLineObject.VoidActions(this.clearLine);
		this.hardClearLineAction = new TerminalLineObject.VoidActions(this.hardClearLine);
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void Update()
	{
		if (this.terminalInputLine != null && this.terminalInputLine.GetComponent<TerminalInputLineObject>().inputLine.isFocused)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.getLastCommand();
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.getNextCommand();
			}
		}
	}

	private const float TERMINAL_LINE_STARTY = 18f;

	public int STARTING_TERMINAL_LINE_POOL = 10;

	public GameObject terminalContentHolder;

	public GameObject terminalContentBox;

	public GameObject terminalLineObject;

	public GameObject terminalInputLineObject;

	private Action<string> myCallBackAction;

	private PooledStack<TerminalLineObject> terminalLineObjectPool;

	private List<TerminalLineObject> currentTerminalLines = new List<TerminalLineObject>(10);

	private TerminalInputLineObject terminalInputLine;

	private TerminalLineObject.VoidActions clearLineAction;

	private TerminalLineObject.VoidActions hardClearLineAction;

	private int lastCommandIndex;

	private List<string> lastCommands = new List<string>();

	private Vector2 terminalContentSize = Vector2.zero;
}
