using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class MemDeFragHack : MonoBehaviour
{
	public void PrepAttack(HACK_SWEEPER_SKILL_TIER SetTier)
	{
		this.timeAvg = 0f;
		this.setCurrentLevelIndex();
		this.MyRayCaster.enabled = true;
		this.myCG.alpha = 1f;
		this.myCG.interactable = true;
		this.myCG.blocksRaycasts = true;
		this.myCG.ignoreParentGroups = true;
		switch (SetTier)
		{
		case HACK_SWEEPER_SKILL_TIER.TIER1:
			this.currentLevelIndex = ((this.currentLevelIndex != 0) ? (this.currentLevelIndex - 1) : 0);
			break;
		case HACK_SWEEPER_SKILL_TIER.TIER3:
			this.currentLevelIndex++;
			break;
		case HACK_SWEEPER_SKILL_TIER.TIER4:
			this.currentLevelIndex += 2;
			break;
		case HACK_SWEEPER_SKILL_TIER.TIER5:
			this.currentLevelIndex += 3;
			break;
		case HACK_SWEEPER_SKILL_TIER.GOD_TIER:
			this.currentLevelIndex = this.MemDefragLevels.Count - 1;
			break;
		}
		if (this.currentLevelIndex > this.MemDefragLevels.Count - 1)
		{
			this.currentLevelIndex = this.MemDefragLevels.Count - 1;
		}
		if (GameManager.HackerManager.InTestMode)
		{
			this.currentLevelIndex = 0;
		}
		GameManager.AudioSlinger.PlaySound(GameManager.HackerManager.HackingTypeSFX);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine1, TERMINAL_LINE_TYPE.TYPE, "> ./memD3FR4G3R", 0.2f, 0f);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine2, TERMINAL_LINE_TYPE.TYPE, "  Loading memD3FR4G3R v0.45.570", 0.2f, 0.2f);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine3, TERMINAL_LINE_TYPE.TYPE, "  Initialzing...", 0.2f, 0.4f);
		GameManager.TimeSlinger.FireTimer(0.6f, new Action(this.BuildAttack), 0);
	}

	public void BuildAttack()
	{
		GameManager.HackerManager.HackingTimer.FireWarmUpTimer(this.MemDefragLevels[this.currentLevelIndex].WarmUpTime);
		GameManager.TimeSlinger.FireTimer((float)this.MemDefragLevels[this.currentLevelIndex].WarmUpTime, new Action(this.beginGame), 0);
		this.curMemoryCellCount = this.MemDefragLevels[this.currentLevelIndex].MemoryCellCount;
		this.memoryCellObjectHolderSize.x = 100f * (float)this.curMemoryCellCount + 40f * (float)(this.curMemoryCellCount - 1);
		this.memoryCellObjectHolderSize.y = this.memoryCellObjectHolderRT.sizeDelta.y;
		this.memoryCellObjectHolderRT.sizeDelta = this.memoryCellObjectHolderSize;
		this.memoryCellObjectHolderRT.anchoredPosition = this.startingMemoryCellObjectHolderPOS;
		for (int i = 0; i < this.curMemoryCellCount; i++)
		{
			MemCellObject memCellObject = this.memoryCellObjectPool.Pop();
			memCellObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)i * 140f, 0f);
			memCellObject.BuildMe(i, this.curMemoryCellCount);
			this.curMemoryCellObjects.Add(memCellObject);
		}
		this.activeMemoryCellIndex = 0;
		this.slideUpMemoryCellObjectHolder.Restart(true, -1f);
		this.fadeInMemoryCellObjectHolder.Restart(true, -1f);
	}

	private void beginGame()
	{
		this.triggerMemoryCell();
	}

	private void triggerMemoryCell()
	{
		if (this.activeMemoryCellIndex >= this.curMemoryCellCount)
		{
			this.dismissMemoryCells();
			GameManager.HackerManager.HackingTimer.CurrentHackerTimerObject.ForceKillMe();
			this.termLine1.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
			this.termLine2.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
			this.termLine3.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
			GameManager.TimeSlinger.FireTimer(0.4f, delegate()
			{
				GameManager.HackerManager.HackingTerminal.TerminalHelper.ClearTerminal();
				float num = (float)(this.MemDefragLevels[this.currentLevelIndex].MemoryFragmentCountMax - 1) * this.MemDefragLevels[this.currentLevelIndex].TimePerFragment * (float)this.MemDefragLevels[this.currentLevelIndex].MemoryCellCount;
				float num2 = this.timeAvg / num;
				if (!GameManager.HackerManager.InTestMode)
				{
					if (!DataManager.LeetMode)
					{
						if (num2 >= 0.45f)
						{
							this.myData.SkillPoints = this.myData.SkillPoints + this.MemDefragLevels[this.currentLevelIndex].PointsRewaredTier1;
						}
						else if (num2 >= 0.35f && num2 < 0.45f)
						{
							this.myData.SkillPoints = this.myData.SkillPoints + this.MemDefragLevels[this.currentLevelIndex].PointsRewaredTier1;
						}
						else if (num2 >= 0.2f && num2 < 0.35f)
						{
							this.myData.SkillPoints = this.myData.SkillPoints + this.MemDefragLevels[this.currentLevelIndex].PointsRewaredTier2;
						}
						else
						{
							this.myData.SkillPoints = this.myData.SkillPoints + this.MemDefragLevels[this.currentLevelIndex].PointsRewaredTier2;
						}
						DataManager.Save<MemDeFragHackData>(this.myData);
					}
					if (this.currentLevelIndex == this.MemDefragLevels.Count - 1)
					{
						SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.STEELTRAP);
					}
				}
				GameManager.HackerManager.PlayerWon(this.currentLevelIndex);
				this.MyRayCaster.enabled = false;
				this.myCG.alpha = 0f;
				this.myCG.interactable = false;
				this.myCG.blocksRaycasts = false;
				this.myCG.ignoreParentGroups = false;
			}, 0);
		}
		else
		{
			float setDur = (float)(this.MemDefragLevels[this.currentLevelIndex].MemoryFragmentCountMax - 1) * this.MemDefragLevels[this.currentLevelIndex].TimePerFragment;
			GameManager.HackerManager.HackingTimer.FireHackingTimer(setDur, new Action(this.timesUp));
			GameManager.AudioSlinger.PlaySound(this.MemCellPresentSFX);
			this.curMemoryDerfagObject.BuildMe(this.MemDefragLevels[this.currentLevelIndex]);
			this.curMemoryCellObjects[this.activeMemoryCellIndex].ActivateInactiveCellIMG();
		}
	}

	private void memoryDerfagPuzzleSolved()
	{
		this.timeAvg += GameManager.HackerManager.HackingTimer.CurrentHackerTimerObject.GetTimeLeft();
		this.curMemoryCellObjects[this.activeMemoryCellIndex].ActivateActiveCellIMG();
		this.activeMemoryCellIndex++;
	}

	private void memoryDefragGameOver()
	{
		this.dismissMemoryCells();
		GameManager.HackerManager.HackingTimer.CurrentHackerTimerObject.ForceKillMe();
		this.termLine1.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		this.termLine2.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		this.termLine3.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		GameManager.TimeSlinger.FireTimer(0.4f, delegate()
		{
			GameManager.HackerManager.HackingTerminal.TerminalHelper.ClearTerminal();
			if (!GameManager.HackerManager.InTestMode)
			{
				if (!DataManager.LeetMode)
				{
					this.myData.SkillPoints = this.myData.SkillPoints - this.MemDefragLevels[this.currentLevelIndex].PointsDeducted;
					DataManager.Save<MemDeFragHackData>(this.myData);
				}
				if (this.currentLevelIndex == 0)
				{
					SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.GOLDFISHMEMORY);
				}
			}
			GameManager.HackerManager.PlayerLost();
			this.MyRayCaster.enabled = false;
			this.myCG.alpha = 0f;
			this.myCG.interactable = false;
			this.myCG.blocksRaycasts = false;
			this.myCG.ignoreParentGroups = false;
		}, 0);
	}

	private void dismissMemoryCells()
	{
		DOTween.To(() => this.MemoryCellObjectHolder.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			this.MemoryCellObjectHolder.GetComponent<RectTransform>().anchoredPosition = x;
		}, new Vector2(0f, -150f), 0.2f).SetEase(Ease.Linear).SetRelative(true);
		DOTween.To(() => this.MemoryCellObjectHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.MemoryCellObjectHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.destroyMemoryCells));
	}

	private void destroyMemoryCells()
	{
		for (int i = 0; i < this.curMemoryCellObjects.Count; i++)
		{
			this.curMemoryCellObjects[i].ResetMe();
			this.memoryCellObjectPool.Push(this.curMemoryCellObjects[i]);
		}
		this.curMemoryCellObjects.Clear();
	}

	private void pauseGameTime()
	{
		GameManager.HackerManager.HackingTimer.CurrentHackerTimerObject.Pause();
	}

	private void resumeGameTime()
	{
		GameManager.HackerManager.HackingTimer.CurrentHackerTimerObject.UnPause();
	}

	private void timesUp()
	{
		this.curMemoryDerfagObject.GetComponent<MemDefragmentObject>().ForceEnd();
	}

	private void setCurrentLevelIndex()
	{
		for (int i = 0; i < this.MemDefragLevels.Count; i++)
		{
			if (this.myData.SkillPoints >= this.MemDefragLevels[i].SkillPointsRequired)
			{
				this.currentLevelIndex = i;
			}
			else
			{
				i = this.MemDefragLevels.Count;
			}
		}
	}

	private void clearPool()
	{
		foreach (MemCellObject memCellObject in this.memoryCellObjectPool)
		{
			memCellObject.IWasActivated -= this.triggerMemoryCell;
		}
		this.memoryCellObjectPool.Clear();
		this.memoryCellObjectPool = null;
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<MemDeFragHackData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new MemDeFragHackData(this.myID);
			this.myData.SkillPoints = 0;
		}
		GameManager.StageManager.Stage -= this.stageMe;
		if (DataManager.LeetMode)
		{
			this.myData.SkillPoints = this.MemDefragLevels[this.MemDefragLevels.Count - 1].SkillPointsRequired;
		}
		for (int i = 0; i < this.MemDefragLevels.Count; i++)
		{
			if (this.MemDefragLevels[i].MemoryCellCount > 4)
			{
				this.MemDefragLevels[i].MemoryCellCount = 4;
				Debug.Log("Nerfed memory hack");
			}
		}
	}

	private void Awake()
	{
		this.myID = base.transform.position.GetHashCode();
		this.myCG = this.MemoryDefragContentHolder.GetComponent<CanvasGroup>();
		this.memoryCellObjectHolderCG = this.MemoryCellObjectHolder.GetComponent<CanvasGroup>();
		this.memoryCellObjectHolderRT = this.MemoryCellObjectHolder.GetComponent<RectTransform>();
		this.slideUpMemoryCellObjectHolder = DOTween.To(() => this.memoryCellObjectHolderRT.anchoredPosition, delegate(Vector2 x)
		{
			this.memoryCellObjectHolderRT.anchoredPosition = x;
		}, new Vector2(0f, 150f), 0.4f).SetEase(Ease.Linear).SetRelative(true);
		this.slideUpMemoryCellObjectHolder.Pause<Tweener>();
		this.slideUpMemoryCellObjectHolder.SetAutoKill(false);
		this.fadeInMemoryCellObjectHolder = DOTween.To(() => this.memoryCellObjectHolderCG.alpha, delegate(float x)
		{
			this.memoryCellObjectHolderCG.alpha = x;
		}, 1f, 0.4f).SetEase(Ease.Linear);
		this.fadeInMemoryCellObjectHolder.Pause<Tweener>();
		this.fadeInMemoryCellObjectHolder.SetAutoKill(false);
		this.memoryCellObjectPool = new PooledStack<MemCellObject>(delegate()
		{
			MemCellObject component = UnityEngine.Object.Instantiate<GameObject>(this.MemoryCellObject, this.memoryCellObjectHolderRT).GetComponent<MemCellObject>();
			component.IWasActivated += this.triggerMemoryCell;
			return component;
		}, this.MEMORY_CELL_START_POOL_COUNT);
		this.curMemoryDerfagObject = UnityEngine.Object.Instantiate<GameObject>(this.MemoryDefragObject, this.MemoryDefragContentHolder.GetComponent<RectTransform>()).GetComponent<MemDefragmentObject>();
		this.curMemoryDerfagObject.PuzzleSolved += this.memoryDerfagPuzzleSolved;
		this.curMemoryDerfagObject.PauseGameTime += this.pauseGameTime;
		this.curMemoryDerfagObject.ResumeGameTime += this.resumeGameTime;
		this.curMemoryDerfagObject.GameOver += this.memoryDefragGameOver;
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void OnDestroy()
	{
		this.clearPool();
		this.curMemoryDerfagObject.PuzzleSolved -= this.memoryDerfagPuzzleSolved;
		this.curMemoryDerfagObject.PauseGameTime -= this.pauseGameTime;
		this.curMemoryDerfagObject.ResumeGameTime -= this.resumeGameTime;
		this.curMemoryDerfagObject.GameOver -= this.memoryDefragGameOver;
	}

	public int MEMORY_CELL_START_POOL_COUNT = 6;

	public GraphicRaycaster MyRayCaster;

	public GameObject MemoryDefragContentHolder;

	public GameObject MemoryCellObjectHolder;

	public GameObject MemoryCellObject;

	public GameObject MemoryDefragObject;

	public AudioFileDefinition MemCellPresentSFX;

	public List<MemDefragLevelDefinition> MemDefragLevels;

	private const float MEMORY_CELL_WIDTH = 100f;

	private const float MEMORY_CELL_CON_WIDTH = 40f;

	private PooledStack<MemCellObject> memoryCellObjectPool;

	private List<MemCellObject> curMemoryCellObjects = new List<MemCellObject>(6);

	private MemDefragmentObject curMemoryDerfagObject;

	private TerminalLineObject termLine1;

	private TerminalLineObject termLine2;

	private TerminalLineObject termLine3;

	private Vector2 startingMemoryCellObjectHolderPOS = new Vector2(0f, -75f);

	private Vector2 memoryCellObjectHolderSize = Vector2.zero;

	private int curMemoryCellCount;

	private int activeMemoryCellIndex;

	private int currentLevelIndex;

	private float timeAvg;

	private CanvasGroup myCG;

	private CanvasGroup memoryCellObjectHolderCG;

	private RectTransform memoryCellObjectHolderRT;

	private Tweener slideUpMemoryCellObjectHolder;

	private Tweener fadeInMemoryCellObjectHolder;

	private int myID;

	private MemDeFragHackData myData;
}
