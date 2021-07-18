using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HackingTerminalBehaviour : MonoBehaviour
{
	public TerminalHelperBehavior TerminalHelper
	{
		get
		{
			return this.myTerminalHelper;
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event HackingTerminalBehaviour.VoidActions DumpDone;

	public void DoDump()
	{
		this.dumpTimeStamp = Time.time;
		this.lineIndex = 0;
		this.doDumpActive = true;
	}

	private void buildDumpLines()
	{
		for (int i = 0; i < this.terminalLines.Length; i++)
		{
			this.terminalDumpLineObjects.Add(this.myTerminalHelper.BuildSoftLine(TERMINAL_LINE_TYPE.HARD, this.terminalLines[i], 0f, 0f));
		}
	}

	private void Awake()
	{
		this.terminalLines = this.TerminalDumpText.PasswordList.Split(new string[]
		{
			"\r\n"
		}, StringSplitOptions.RemoveEmptyEntries);
		this.myTerminalHelper = base.GetComponent<TerminalHelperBehavior>();
	}

	private void Start()
	{
		this.buildDumpLines();
	}

	private void Update()
	{
		if (this.doDumpActive && Time.time - this.dumpTimeStamp >= 0.0165f)
		{
			this.dumpTimeStamp = Time.time;
			this.myTerminalHelper.AddSoftLine(this.terminalDumpLineObjects[this.lineIndex]);
			this.myTerminalHelper.UpdateTerminalContentScrollHeightHackingDump();
			this.lineIndex++;
			if (this.lineIndex >= this.terminalLines.Length - 1)
			{
				this.doDumpActive = false;
				if (this.DumpDone != null)
				{
					this.DumpDone();
				}
			}
		}
	}

	public PasswordListDefinition TerminalDumpText;

	private TerminalHelperBehavior myTerminalHelper;

	private List<TerminalLineObject> terminalDumpLineObjects = new List<TerminalLineObject>();

	private string[] terminalLines;

	private bool doDumpActive;

	private float dumpTimeStamp;

	private int lineIndex;

	public delegate void VoidActions();
}
