using System;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class TerminalLineObject : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TerminalLineObject.VoidActions ClearLine;

	public void SoftBuild()
	{
		this.myCanvasGroup = base.gameObject.AddComponent<CanvasGroup>();
		this.myCanvasGroup.blocksRaycasts = false;
		this.myCanvasGroup.interactable = false;
		this.myRectTrans = base.gameObject.GetComponent<RectTransform>();
		this.myText = this.textLine.GetComponent<Text>();
		this.myRectTrans.anchoredPosition = TerminalLineObject.startPOS;
		this.updateStrTextAction = new Action<float>(this.updateStrText);
	}

	public void Build(TERMINAL_LINE_TYPE LineType, string SetLine, float LengthAmount = 0f, float SetDelay = 0f)
	{
		this.myLine = SetLine;
		switch (LineType)
		{
		case TERMINAL_LINE_TYPE.HARD:
			this.hardShow(SetDelay);
			break;
		case TERMINAL_LINE_TYPE.TYPE:
			this.typeShow(LengthAmount, SetDelay);
			break;
		case TERMINAL_LINE_TYPE.FADE:
			this.fadeShow(LengthAmount, SetDelay);
			break;
		case TERMINAL_LINE_TYPE.CRACK:
			this.buildCrackLine(SetLine);
			break;
		}
	}

	public void Clear()
	{
		this.myRectTrans.anchoredPosition = TerminalLineObject.startPOS;
		this.isCrackLine = false;
		this.crackTimeStamp = 0f;
		this.updateDelay = 0f;
		this.myTweener.Kill(false);
		GameManager.TimeSlinger.KillTimer(this.myTimer);
		GameManager.TweenSlinger.KillTween(this.myDOSTween);
		this.myTweener = null;
		this.myTimer = null;
		this.myDOSTween = null;
		if (!this.SoftLine)
		{
			this.myText.text = string.Empty;
			this.myLine = string.Empty;
			this.crackLineTitle = string.Empty;
			if (this.ClearLine != null)
			{
				this.ClearLine(this);
			}
		}
	}

	public void AniHardClear(TERMINAL_LINE_TYPE LineType, float SetTime)
	{
		if (LineType != TERMINAL_LINE_TYPE.HARD)
		{
			if (LineType != TERMINAL_LINE_TYPE.FADE)
			{
				if (LineType == TERMINAL_LINE_TYPE.TYPE)
				{
					this.typeHide(SetTime);
				}
			}
			else
			{
				this.fadeHide(SetTime);
			}
		}
		else
		{
			this.HardClear();
		}
		if (LineType != TERMINAL_LINE_TYPE.HARD)
		{
			GameManager.TimeSlinger.FireTimer(SetTime, new Action(this.HardClear), 0);
		}
	}

	public void HardClear()
	{
		this.myRectTrans.anchoredPosition = TerminalLineObject.startPOS;
		this.myText.text = string.Empty;
		this.myLine = string.Empty;
		this.crackLineTitle = string.Empty;
		this.isCrackLine = false;
		this.crackTimeStamp = 0f;
		this.updateDelay = 0f;
		this.myTweener.Kill(false);
		GameManager.TimeSlinger.KillTimer(this.myTimer);
		GameManager.TweenSlinger.KillTween(this.myDOSTween);
		this.myTweener = null;
		this.myTimer = null;
		this.myDOSTween = null;
		if (this.HardClearLine != null)
		{
			this.HardClearLine(this);
		}
	}

	public void Move(float SetY)
	{
		this.myPOS.y = SetY;
		this.myRectTrans.anchoredPosition = this.myPOS;
	}

	public void UpdateMyText(string setText)
	{
		this.myLine = setText;
		this.textLine.GetComponent<Text>().text = setText;
	}

	public void KillCrackLine()
	{
		if (this.isCrackLine)
		{
			this.isCrackLine = false;
		}
	}

	public string GetMyLine()
	{
		return this.myLine;
	}

	public void buildCrackLine(string SetTitle = "")
	{
		if (SetTitle == string.Empty)
		{
			this.crackLineTitle = MagicSlinger.FluffString(" ", " ", 17);
		}
		else
		{
			this.crackLineTitle = MagicSlinger.FluffString(SetTitle, " ", 15) + ": ";
		}
		this.myLine = this.crackLineTitle + MagicSlinger.GenerateRandomHexCode(2, 16, " ");
		this.hardShow();
		this.crackTimeStamp = Time.time;
		this.updateDelay = 0.15f;
		this.isCrackLine = true;
	}

	public void BuildBlank()
	{
		this.myLine = string.Empty;
		this.hardShow();
	}

	private void hardShow()
	{
		if (this.myText != null)
		{
			this.myText.text = this.myLine;
		}
	}

	private void hardShow(float SetDelay)
	{
		if (SetDelay > 0f)
		{
			GameManager.TimeSlinger.FireHardTimer(out this.myTimer, SetDelay, new Action(this.hardShow), 0);
		}
		else
		{
			this.hardShow();
		}
	}

	private void fadeShow(float SetTime)
	{
		this.myCanvasGroup.alpha = 0f;
		this.myText.text = this.myLine;
		this.myTweener = DOTween.To(() => this.myCanvasGroup.alpha, delegate(float x)
		{
			this.myCanvasGroup.alpha = x;
		}, 1f, SetTime).SetEase(Ease.Linear);
	}

	private void fadeShow(float SetTime, float SetDelay)
	{
		if (SetDelay > 0f)
		{
			GameManager.TimeSlinger.FireHardTimer<float>(out this.myTimer, SetDelay, new Action<float>(this.fadeShow), SetTime, 0);
		}
		else
		{
			this.fadeShow(SetTime);
		}
	}

	private void fadeHide(float SetTime)
	{
		DOTween.To(() => this.myCanvasGroup.alpha, delegate(float x)
		{
			this.myCanvasGroup.alpha = x;
		}, 0f, SetTime).SetEase(Ease.Linear);
	}

	private void typeShow(float SetTime)
	{
		this.myDOSTween = GameManager.TweenSlinger.PlayDOSTweenLiner(0f, (float)this.myLine.Length, SetTime, this.updateStrTextAction);
	}

	private void typeShow(float SetTime, float SetDelay)
	{
		if (SetDelay > 0f)
		{
			this.typeShowTime = SetTime;
			this.typeShowDelay = SetDelay;
			this.typeShowDelayTimeStamp = Time.time;
			this.delayTypeShowActive = true;
		}
		else
		{
			this.typeShow(SetTime);
		}
	}

	private void typeHide(float SetTime)
	{
		this.myDOSTween = GameManager.TweenSlinger.PlayDOSTweenLiner((float)this.myLine.Length, 0f, SetTime, this.updateStrTextAction);
	}

	private void updateStrText(float SetCount)
	{
		if (this.myLine.Length > 0 && Mathf.RoundToInt(SetCount) <= this.myLine.Length)
		{
			this.myText.text = this.myLine.Substring(0, Mathf.RoundToInt(SetCount));
		}
	}

	private void Awake()
	{
	}

	private void Update()
	{
		if (this.isCrackLine && Time.time - this.crackTimeStamp >= this.updateDelay)
		{
			this.crackTimeStamp = Time.time;
			this.myLine = this.crackLineTitle + MagicSlinger.GenerateRandomHexCode(2, 16, " ");
			this.hardShow();
		}
		if (this.delayTypeShowActive && Time.time - this.typeShowDelayTimeStamp >= this.typeShowDelay)
		{
			this.delayTypeShowActive = false;
			this.typeShow(this.typeShowTime);
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TerminalLineObject.VoidActions HardClearLine;

	public GameObject textLine;

	public bool SoftLine;

	protected static Vector2 startPOS = new Vector2(0f, 10f);

	private Vector2 myPOS = Vector2.zero;

	private Text myText;

	private CanvasGroup myCanvasGroup;

	private RectTransform myRectTrans;

	private Tweener myTweener;

	private Timer myTimer;

	private DOSTween myDOSTween;

	private bool isCrackLine;

	private bool delayTypeShowActive;

	private string myLine;

	private string crackLineTitle;

	private float crackTimeStamp;

	private float updateDelay;

	private float typeShowDelay;

	private float typeShowDelayTimeStamp;

	private float typeShowTime;

	private Action<float> updateStrTextAction;

	public delegate void VoidActions(TerminalLineObject TLO);
}
