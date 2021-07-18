using System;

[Serializable]
public class TerminalLineDefinition : Definition
{
	public TERMINAL_LINE_TYPE terminalLineType;

	public string terminalText;

	public bool terminalHasDelay;

	public float terminalDelayAmount;

	public float terminalAniLength;
}
