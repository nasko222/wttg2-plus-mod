using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SweeperHack : MonoBehaviour
{
	public void ActivateMe()
	{
		this.sweeperHackOverlayCG.alpha = 1f;
		this.sweeperHackOverlayCG.interactable = true;
		this.sweeperHackOverlayCG.blocksRaycasts = true;
		this.sweeperHackOverlayCG.ignoreParentGroups = true;
	}

	public void DeActivateMe()
	{
		this.sweeperHackOverlayCG.alpha = 0f;
		this.sweeperHackOverlayCG.interactable = false;
		this.sweeperHackOverlayCG.blocksRaycasts = false;
		this.sweeperHackOverlayCG.ignoreParentGroups = false;
	}

	public void PrepSweepAttack(bool TestMode = false)
	{
		this.setCurrentLevelIndex();
		if (TestMode)
		{
			this.isTestMode = true;
			this.curLevelIndex = 0;
		}
		GameManager.AudioSlinger.PlaySound(GameManager.HackerManager.HackingTypeSFX);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine1, TERMINAL_LINE_TYPE.TYPE, "> ./ZONEWALL", 0.2f, 0f);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine2, TERMINAL_LINE_TYPE.TYPE, "  Loading ZONEWALL v1.88.520", 0.2f, 0.2f);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine3, TERMINAL_LINE_TYPE.TYPE, "  Initialzing...", 0.2f, 0.4f);
		this.termBlockTargetWidth.x = (float)this.SweeperLevels[this.curLevelIndex].NumOfDotsPerSweeper * 11f + 50f;
		this.termBlockTargetWidth.y = this.termBlockRT.sizeDelta.y;
		this.termBlockTargetHeight.x = (float)this.SweeperLevels[this.curLevelIndex].NumOfDotsPerSweeper * 11f + 50f;
		this.termBlockTargetHeight.y = (float)this.SweeperLevels[this.curLevelIndex].NumOfSweepers * this.sweeperObjectRT.sizeDelta.y + (float)this.SweeperLevels[this.curLevelIndex].NumOfSweepers * 10f + 20f;
		GameManager.TimeSlinger.FireTimer(0.6f, delegate()
		{
			GameManager.AudioSlinger.PlaySound(this.TermBlockShowSFX);
			this.termBlockPresent1.Restart(true, -1f);
			DOTween.To(() => this.termBlockRT.sizeDelta, delegate(Vector2 x)
			{
				this.termBlockRT.sizeDelta = x;
			}, this.termBlockTargetWidth, 0.35f).SetEase(Ease.Linear).SetDelay(0.25f);
			DOTween.To(() => this.termBlockRT.sizeDelta, delegate(Vector2 x)
			{
				this.termBlockRT.sizeDelta = x;
			}, this.termBlockTargetHeight, 0.35f).SetEase(Ease.Linear).SetDelay(0.6f).OnComplete(new TweenCallback(this.BuildSweepAttack));
		}, 0);
	}

	public void BuildSweepAttack()
	{
		Vector2 zero = Vector2.zero;
		this.termBlockRT.pivot = new Vector2(0.5f, 1f);
		this.termBlockRT.anchoredPosition = new Vector2(0f, this.termBlockRT.sizeDelta.y / 2f);
		this.sweeperObjectHolderRT.sizeDelta = new Vector2(this.sweeperObjectRT.sizeDelta.x, (float)this.SweeperLevels[this.curLevelIndex].NumOfSweepers * this.sweeperObjectRT.sizeDelta.y + (float)this.SweeperLevels[this.curLevelIndex].NumOfSweepers * 10f);
		for (int i = 0; i < this.SweeperLevels[this.curLevelIndex].NumOfSweepers; i++)
		{
			SweeperObject sweeperObject = this.sweeperObjectPool.Pop();
			sweeperObject.IWasDismissed += this.fireNextSweeperAction;
			sweeperObject.WarmMe(i, this.SweeperLevels[this.curLevelIndex]);
			sweeperObject.gameObject.GetComponent<RectTransform>().anchoredPosition = zero;
			this.curSweeperObjects.Add(sweeperObject);
			zero.y = zero.y - this.sweeperObjectRT.sizeDelta.y - 10f;
		}
		this.curSweeperIndex = 0;
		GameManager.HackerManager.HackingTimer.FireWarmUpTimer(this.SweeperLevels[this.curLevelIndex].WarmUpTime);
		GameManager.TimeSlinger.FireTimer((float)this.SweeperLevels[this.curLevelIndex].WarmUpTime, new Action(this.BeginGame), 0);
	}

	private void BeginGame()
	{
		float setDur = (float)this.SweeperLevels[this.curLevelIndex].NumOfSweepsPerSweeper * this.SweeperLevels[this.curLevelIndex].ScrollSpeedMax + (float)this.SweeperLevels[this.curLevelIndex].NumOfSweepers + this.SweeperLevels[this.curLevelIndex].BufferTime;
		GameManager.HackerManager.HackingTimer.FireHackingTimer(setDur, new Action(this.TimesUp));
		this.curSweeperObjects[this.curSweeperIndex].FireMe();
		this.gameIsActive = true;
	}

	private void TimesUp()
	{
		this.gameIsActive = false;
		for (int i = 0; i < this.curSweeperObjects.Count; i++)
		{
			this.curSweeperObjects[i].ForceEnd();
		}
		GameManager.TimeSlinger.FireTimer(0.15f, new Action(this.ForceFinishGame), 0);
	}

	private void FireNextSweeper()
	{
		this.curSweeperIndex++;
		if (this.curSweeperIndex < this.curSweeperObjects.Count)
		{
			for (int i = this.curSweeperIndex; i < this.curSweeperObjects.Count; i++)
			{
				this.curSweeperObjects[i].MoveMeUp();
			}
			DOTween.To(() => this.termBlockRT.sizeDelta, delegate(Vector2 x)
			{
				this.termBlockRT.sizeDelta = x;
			}, new Vector2(0f, -(this.sweeperObjectRT.sizeDelta.y + 10f)), 0.15f).SetEase(Ease.Linear).SetRelative(true).SetDelay(0.15f).OnComplete(delegate
			{
				this.curSweeperObjects[this.curSweeperIndex].FireMe();
				this.gameIsActive = true;
			});
		}
		else
		{
			GameManager.HackerManager.HackingTimer.KillHackerTimer();
			DOTween.To(() => this.termBlockRT.sizeDelta, delegate(Vector2 x)
			{
				this.termBlockRT.sizeDelta = x;
			}, new Vector2(0f, -(this.sweeperObjectRT.sizeDelta.y + 10f)), 0.15f).SetEase(Ease.Linear).SetRelative(true).SetDelay(0.15f).OnComplete(delegate
			{
				this.FinishGame();
			});
		}
	}

	private void ForceFinishGame()
	{
		for (int i = 0; i < this.curSweeperObjects.Count; i++)
		{
			this.curSweeperObjects[i].IWasDismissed -= this.fireNextSweeperAction;
			this.curSweeperObjects[i].DestroyMe();
			this.sweeperObjectPool.Push(this.curSweeperObjects[i]);
		}
		this.curSweeperObjects.Clear();
		this.ClearTerm();
		GameManager.HackerManager.HackingTimer.KillHackerTimer();
		this.termLine1.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		this.termLine2.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		this.termLine3.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		GameManager.TimeSlinger.FireTimer<int>(0.4f, new Action<int>(this.tallyPlayerPoints), 0, 0);
	}

	private void FinishGame()
	{
		int num = 0;
		for (int i = 0; i < this.curSweeperObjects.Count; i++)
		{
			num += this.curSweeperObjects[i].MyScore;
			this.curSweeperObjects[i].IWasDismissed -= this.fireNextSweeperAction;
			this.curSweeperObjects[i].DestroyMe();
			this.sweeperObjectPool.Push(this.curSweeperObjects[i]);
		}
		this.curSweeperObjects.Clear();
		this.ClearTerm();
		GameManager.HackerManager.HackingTimer.KillHackerTimer();
		this.termLine1.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		this.termLine2.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		this.termLine3.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		GameManager.TimeSlinger.FireTimer<int>(0.4f, new Action<int>(this.tallyPlayerPoints), num, 0);
	}

	private void ClearTerm()
	{
		GameManager.AudioSlinger.PlaySound(this.SweeperEnd);
		this.termBlockDismiss1.Restart(true, -1f);
		this.termBlockDismiss2.Restart(true, -1f);
	}

	private void tallyPlayerPoints(int scoreCount)
	{
		GameManager.HackerManager.HackingTerminal.TerminalHelper.ClearTerminal();
		if (!this.isTestMode)
		{
			int num = 5 * this.SweeperLevels[this.curLevelIndex].NumOfSweepers;
			HACK_SWEEPER_SKILL_TIER setTier;
			if (scoreCount == 0)
			{
				this.myData.SkillPoints = this.myData.SkillPoints - this.SweeperLevels[this.curLevelIndex].PointsDeducted;
				setTier = HACK_SWEEPER_SKILL_TIER.TIER5;
			}
			else if ((float)scoreCount >= (float)num * 0.9f)
			{
				this.myData.SkillPoints = this.myData.SkillPoints + this.SweeperLevels[this.curLevelIndex].PointsRewaredTier1;
				if ((float)scoreCount >= (float)num * 0.95f)
				{
					setTier = HACK_SWEEPER_SKILL_TIER.INSTABLOCK;
				}
				else
				{
					setTier = HACK_SWEEPER_SKILL_TIER.TIER1;
				}
			}
			else if ((float)scoreCount >= (float)num * 0.75f && (float)scoreCount < (float)num * 0.9f)
			{
				this.myData.SkillPoints = this.myData.SkillPoints + this.SweeperLevels[this.curLevelIndex].PointsRewaredTier1;
				setTier = HACK_SWEEPER_SKILL_TIER.TIER2;
			}
			else if ((float)scoreCount >= (float)num * 0.5f && (float)scoreCount < (float)num * 0.75f)
			{
				this.myData.SkillPoints = this.myData.SkillPoints + this.SweeperLevels[this.curLevelIndex].PointsRewaredTier2;
				setTier = HACK_SWEEPER_SKILL_TIER.TIER3;
			}
			else
			{
				this.myData.SkillPoints = this.myData.SkillPoints + this.SweeperLevels[this.curLevelIndex].PointsRewaredTier2;
				setTier = HACK_SWEEPER_SKILL_TIER.TIER4;
			}
			DataManager.Save<SweeperHackData>(this.myData);
			GameManager.HackerManager.ProcessSweepAttack(setTier);
		}
		else
		{
			GameManager.HackerManager.ProcessSweepAttack(HACK_SWEEPER_SKILL_TIER.TIER4);
		}
		this.isTestMode = false;
	}

	private void setCurrentLevelIndex()
	{
		for (int i = 0; i < this.SweeperLevels.Count; i++)
		{
			if (this.myData.SkillPoints >= this.SweeperLevels[i].SkillPointsRequired)
			{
				this.curLevelIndex = i;
			}
			else
			{
				i = this.SweeperLevels.Count;
			}
		}
		if (this.curLevelIndex < 6)
		{
			this.curLevelIndex = 6;
		}
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<SweeperHackData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new SweeperHackData(this.myID);
			this.myData.SkillPoints = 0;
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		this.myID = base.transform.position.GetHashCode();
		this.termBlockCG = this.TermBlock.GetComponent<CanvasGroup>();
		this.sweeperHackOverlayCG = this.SweeperHackOverlay.GetComponent<CanvasGroup>();
		this.termBlockRT = this.TermBlock.GetComponent<RectTransform>();
		this.sweeperObjectRT = this.SweeperObject.GetComponent<RectTransform>();
		this.sweeperObjectHolderRT = this.SweeperObjectsHolder.GetComponent<RectTransform>();
		this.DefaultTermBlockSize = this.termBlockRT.sizeDelta;
		this.termBlockPresent1 = DOTween.To(() => this.termBlockCG.alpha, delegate(float x)
		{
			this.termBlockCG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear);
		this.termBlockPresent1.Pause<Tweener>();
		this.termBlockPresent1.SetAutoKill(false);
		this.termBlockDismiss1 = DOTween.To(() => this.termBlockRT.sizeDelta, delegate(Vector2 x)
		{
			this.termBlockRT.sizeDelta = x;
		}, this.DefaultTermBlockSize, 0.25f).SetEase(Ease.Linear);
		this.termBlockDismiss1.Pause<Tweener>();
		this.termBlockDismiss1.SetAutoKill(false);
		this.termBlockDismiss2 = DOTween.To(() => this.termBlockCG.alpha, delegate(float x)
		{
			this.termBlockCG.alpha = x;
		}, 0f, 0.3f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.termBlockRT.pivot = new Vector2(0.5f, 0.5f);
			this.termBlockRT.anchoredPosition = Vector2.zero;
		});
		this.termBlockDismiss2.Pause<Tweener>();
		this.termBlockDismiss2.SetAutoKill(false);
		this.SweeperHackOverlay.GetComponent<RectTransform>().sizeDelta = new Vector2((float)Screen.width, (float)Screen.height);
		this.SweeperHackOverlay.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		this.fireNextSweeperAction = new SweeperObject.VoidActions(this.FireNextSweeper);
		this.sweeperObjectPool = new PooledStack<SweeperObject>(() => UnityEngine.Object.Instantiate<GameObject>(this.SweeperObject, this.sweeperObjectHolderRT).GetComponent<SweeperObject>(), this.SWEEPER_OBJ_POOL_COUNT);
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void Update()
	{
		if (this.gameIsActive && StateManager.GameState == GAME_STATE.LIVE && computerController.Ins.Active && CrossPlatformInputManager.GetButtonDown("LeftClick") && this.curSweeperObjects.Count > this.curSweeperIndex && this.curSweeperObjects[this.curSweeperIndex] != null)
		{
			this.gameIsActive = false;
			this.curSweeperObjects[this.curSweeperIndex].PlayerHasDecided();
		}
	}

	public int SWEEPER_OBJ_POOL_COUNT = 10;

	public GameObject SweeperHackOverlay;

	public GameObject SweeperObject;

	public GameObject SweeperObjectsHolder;

	public GameObject TermBlock;

	public AudioFileDefinition TermBlockShowSFX;

	public AudioFileDefinition SweeperEnd;

	public List<SweeperObjectDefinition> SweeperLevels;

	private const float SLIDER_SPACING = 10f;

	private const float DOT_WIDTH = 11f;

	private const float TERM_BLOCK_BUFFER_WIDTH = 50f;

	private const float TERM_BLOCK_BUFFER_HEIGHT = 20f;

	private const float TERM_BLOCK_SLIDE_UP_TIME = 0.15f;

	private SweeperObject.VoidActions fireNextSweeperAction;

	private int curLevelIndex;

	private int curSweeperIndex;

	private bool gameIsActive;

	private bool isTestMode;

	private Vector2 DefaultTermBlockSize = Vector2.zero;

	private Vector2 termBlockTargetWidth = Vector2.zero;

	private Vector2 termBlockTargetHeight = Vector2.zero;

	private Vector2 sweeperObjectPOS = Vector2.zero;

	private PooledStack<SweeperObject> sweeperObjectPool;

	private List<SweeperObject> curSweeperObjects = new List<SweeperObject>(10);

	private TerminalLineObject termLine1;

	private TerminalLineObject termLine2;

	private TerminalLineObject termLine3;

	private CanvasGroup termBlockCG;

	private CanvasGroup sweeperHackOverlayCG;

	private RectTransform termBlockRT;

	private RectTransform sweeperObjectRT;

	private RectTransform sweeperObjectHolderRT;

	private Tweener termBlockPresent1;

	private Tweener termBlockDismiss1;

	private Tweener termBlockDismiss2;

	private Timer testTimer;

	private int myID;

	private SweeperHackData myData;
}
