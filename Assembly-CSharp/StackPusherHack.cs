using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class StackPusherHack : MonoBehaviour
{
	public void PopStack()
	{
		this.stackPopIndex--;
		if (this.CurrentPushingStackBlock != null)
		{
			this.CurrentPushingStackBlock.Pop();
			this.CurrentPushingStackBlock = null;
		}
		if (this.stackPopIndex <= 0)
		{
			this.gameWon = true;
			this.clearGame();
		}
	}

	public void Boom()
	{
		this.gameWon = false;
		this.clearGame();
	}

	public void PrepStackPusherAttack(HACK_SWEEPER_SKILL_TIER SetTier)
	{
		this.setCurrentLevelIndex();
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
			this.currentLevelIndex = this.StackPusherLevels.Count - 1;
			break;
		}
		if (this.currentLevelIndex > this.StackPusherLevels.Count - 1)
		{
			this.currentLevelIndex = this.StackPusherLevels.Count - 1;
		}
		if (GameManager.HackerManager.InTestMode)
		{
			this.currentLevelIndex = 0;
		}
		this.MyRayCaster.enabled = true;
		this.stackPusherContentCG.interactable = true;
		this.stackPusherContentCG.blocksRaycasts = true;
		this.stackPusherContentCG.ignoreParentGroups = true;
		GameManager.AudioSlinger.PlaySound(GameManager.HackerManager.HackingTypeSFX);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine1, TERMINAL_LINE_TYPE.TYPE, "> ./stackPUSHER", 0.2f, 0f);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine2, TERMINAL_LINE_TYPE.TYPE, "  Loading stackPUSHER v1.5.20", 0.2f, 0.2f);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine3, TERMINAL_LINE_TYPE.TYPE, "  Initialzing...", 0.2f, 0.4f);
		GameManager.TimeSlinger.FireTimer(0.6f, new Action(this.buildStackPusherAttack), 0);
	}

	private void buildStackPusherAttack()
	{
		int matrixSize = this.StackPusherLevels[this.currentLevelIndex].MatrixSize;
		int stackPeices = this.StackPusherLevels[this.currentLevelIndex].StackPeices;
		int deadPeices = this.StackPusherLevels[this.currentLevelIndex].DeadPeices;
		int num = Mathf.FloorToInt((float)(matrixSize / 2));
		float num2 = (float)matrixSize * 40f;
		int i = 0;
		int j = 0;
		bool flag = false;
		this.stackPopIndex = stackPeices;
		this.State = HACK_STACK_PUSHER_STATE.IDLE;
		this.currentCenterCord.X = num;
		this.currentCenterCord.Y = num;
		this.gridContentSize.x = num2;
		this.gridContentSize.y = num2;
		this.stackPusherContentSize.x = num2 + 2f;
		this.stackPusherContentSize.y = num2 + 2f;
		this.StackPusherContent.GetComponent<RectTransform>().sizeDelta = this.stackPusherContentSize;
		this.GridHolder.GetComponent<RectTransform>().sizeDelta = this.gridContentSize;
		this.currentMatrix.SetMatrixSize(matrixSize);
		this.specialBlockTypes.Clear();
		this.specialBlockTypes.Add(this.currentCenterCord, HACK_STACK_PUSHER_GRID_BLOCK_STATE.POPER);
		while (!flag)
		{
			this.specialPeicePicker.X = UnityEngine.Random.Range(0, matrixSize);
			this.specialPeicePicker.Y = UnityEngine.Random.Range(0, matrixSize);
			if (!this.specialBlockTypes.ContainsKey(this.specialPeicePicker))
			{
				this.specialBlockTypes.Add(this.specialPeicePicker, HACK_STACK_PUSHER_GRID_BLOCK_STATE.PUSHER);
				this.GridPusher.CurrentCord = this.specialPeicePicker;
				flag = true;
			}
		}
		if (this.GridPusher.AmNextTo(this.currentCenterCord))
		{
			this.specialBlockTypes[this.currentCenterCord] = HACK_STACK_PUSHER_GRID_BLOCK_STATE.POPER_AND_ACTIVE;
		}
		while (i < stackPeices)
		{
			this.specialPeicePicker.X = UnityEngine.Random.Range(0, matrixSize);
			this.specialPeicePicker.Y = UnityEngine.Random.Range(0, matrixSize);
			if (!this.specialBlockTypes.ContainsKey(this.specialPeicePicker))
			{
				if (this.GridPusher.AmNextTo(this.specialPeicePicker))
				{
					this.specialBlockTypes.Add(this.specialPeicePicker, HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_READY);
				}
				else
				{
					this.specialBlockTypes.Add(this.specialPeicePicker, HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_LOCKED);
				}
				i++;
			}
		}
		while (j < deadPeices)
		{
			this.specialPeicePicker.X = UnityEngine.Random.Range(0, matrixSize);
			this.specialPeicePicker.Y = UnityEngine.Random.Range(0, matrixSize);
			if (!this.specialBlockTypes.ContainsKey(this.specialPeicePicker))
			{
				this.specialBlockTypes.Add(this.specialPeicePicker, HACK_STACK_PUSHER_GRID_BLOCK_STATE.DEAD);
				j++;
			}
		}
		GameManager.AudioSlinger.PlaySound(this.PresentStackSFX);
		if (this.StackPusherContent.GetComponent<RectTransform>().sizeDelta.y >= 575f && Screen.height <= 800)
		{
			this.presentStackPusherSeqSmall.Restart(true, -1f);
		}
		else
		{
			this.presentStackPusherSeq.Restart(true, -1f);
		}
		GameManager.HackerManager.HackingTimer.FireWarmUpTimer(this.StackPusherLevels[this.currentLevelIndex].WarmUpTime);
		GameManager.TimeSlinger.FireTimer((float)this.StackPusherLevels[this.currentLevelIndex].WarmUpTime, new Action(this.beginGame), 0);
	}

	private void clearGridBlock(StackPusherGridBlockObject TheBlock)
	{
		TheBlock.Kill -= this.clearGridBlockAction;
		this.gridBlockPool.Push(TheBlock);
	}

	private void clearOldPushBlock(MatrixStackCord OldCord)
	{
		this.currentMatrix.Get(OldCord).State = HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE;
	}

	private void setNewPushBlock(MatrixStackCord NewCord)
	{
		this.currentMatrix.Get(NewCord).State = HACK_STACK_PUSHER_GRID_BLOCK_STATE.PUSHER;
	}

	private void resetOldStacks(Dictionary<MatrixStackCord, short> OldCords)
	{
		foreach (KeyValuePair<MatrixStackCord, short> keyValuePair in OldCords)
		{
			StackPusherGridBlockObject stackPusherGridBlockObject;
			if (this.currentMatrix.TryAndGetValue(out stackPusherGridBlockObject, keyValuePair.Key))
			{
				stackPusherGridBlockObject.ClearStackState();
			}
		}
	}

	private void setNewStacks(Dictionary<MatrixStackCord, short> NewCords)
	{
		foreach (KeyValuePair<MatrixStackCord, short> keyValuePair in NewCords)
		{
			StackPusherGridBlockObject stackPusherGridBlockObject;
			if (this.currentMatrix.TryAndGetValue(out stackPusherGridBlockObject, keyValuePair.Key))
			{
				stackPusherGridBlockObject.SetStackStateReady();
			}
		}
	}

	private void buildBlocks()
	{
		int matrixSize = this.StackPusherLevels[this.currentLevelIndex].MatrixSize;
		for (int i = 0; i < matrixSize * matrixSize; i++)
		{
			StackPusherGridBlockObject stackPusherGridBlockObject = this.gridBlockPool.Pop();
			this.currentMatrix.Push(stackPusherGridBlockObject);
			if (this.specialBlockTypes.ContainsKey(this.currentMatrix.Pointer))
			{
				stackPusherGridBlockObject.Build(this.currentMatrix.Pointer, this.specialBlockTypes[this.currentMatrix.Pointer]);
			}
			else if (this.GridPusher.AmNextTo(this.currentMatrix.Pointer))
			{
				stackPusherGridBlockObject.Build(this.currentMatrix.Pointer, HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE_AND_PUSHABLE);
			}
			else
			{
				stackPusherGridBlockObject.Build(this.currentMatrix.Pointer, HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE);
			}
			stackPusherGridBlockObject.Kill += this.clearGridBlockAction;
		}
	}

	private void beginGame()
	{
		this.gameTime = (float)(this.StackPusherLevels[this.currentLevelIndex].MatrixSize * this.StackPusherLevels[this.currentLevelIndex].MatrixSize) * this.StackPusherLevels[this.currentLevelIndex].TimePerPeice;
		GameManager.HackerManager.HackingTimer.FireHackingTimer(this.gameTime, new Action(this.timesUp));
		this.fadeinGridBlockHolderTween.Restart(true, -1f);
		this.gridBlockHolderCG.interactable = true;
		this.gridBlockHolderCG.ignoreParentGroups = true;
		this.gridBlockHolderCG.blocksRaycasts = true;
	}

	private void timesUp()
	{
		this.gameWon = false;
		this.clearGame();
	}

	private void clearGame()
	{
		GameManager.HackerManager.HackingTimer.CurrentHackerTimerObject.Pause();
		float timeLeft = GameManager.HackerManager.HackingTimer.CurrentHackerTimerObject.GetTimeLeft();
		this.MyRayCaster.enabled = false;
		this.stackPusherContentCG.interactable = false;
		this.stackPusherContentCG.blocksRaycasts = false;
		this.stackPusherContentCG.ignoreParentGroups = false;
		this.gridBlockHolderCG.interactable = false;
		this.gridBlockHolderCG.ignoreParentGroups = false;
		this.gridBlockHolderCG.blocksRaycasts = false;
		this.GridPoper.Clear();
		this.GridPusher.Clear();
		foreach (StackPusherGridBlockObject stackPusherGridBlockObject in this.currentMatrix.GetAll())
		{
			if (stackPusherGridBlockObject != null)
			{
				stackPusherGridBlockObject.Destroy();
			}
		}
		this.currentMatrix.Clear();
		GameManager.TimeSlinger.FireTimer(0.15f, delegate()
		{
			GameManager.AudioSlinger.PlaySound(this.DismissStackSFX);
			this.clearStackPusherSeq.Restart(true, -1f);
			GameManager.HackerManager.HackingTimer.KillHackerTimer();
			this.termLine1.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
			this.termLine2.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
			this.termLine3.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
			GameManager.TimeSlinger.FireTimer(0.4f, delegate()
			{
				this.stackPusherContentCG.alpha = 0f;
				this.gridBlockHolderCG.alpha = 0.95f;
				GameManager.HackerManager.HackingTerminal.TerminalHelper.ClearTerminal();
				if (this.gameWon)
				{
					if (!GameManager.HackerManager.InTestMode)
					{
						if (!DataManager.LeetMode)
						{
							float num = timeLeft / this.gameTime;
							if (num >= 0.65f)
							{
								this.myData.SkillPoints = this.myData.SkillPoints + this.StackPusherLevels[this.currentLevelIndex].PointsRewaredTier1;
							}
							else if (num >= 0.45f && num <= 0.65f)
							{
								this.myData.SkillPoints = this.myData.SkillPoints + this.StackPusherLevels[this.currentLevelIndex].PointsRewaredTier1;
							}
							else if (num >= 0.25f && num <= 0.45f)
							{
								this.myData.SkillPoints = this.myData.SkillPoints + this.StackPusherLevels[this.currentLevelIndex].PointsRewaredTier2;
							}
							else
							{
								this.myData.SkillPoints = this.myData.SkillPoints + this.StackPusherLevels[this.currentLevelIndex].PointsRewaredTier2;
							}
							DataManager.Save<StackPusherHackData>(this.myData);
						}
						if (this.currentLevelIndex == this.StackPusherLevels.Count - 1)
						{
							SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.POPPERPRO);
						}
					}
					GameManager.HackerManager.PlayerWon(this.currentLevelIndex);
				}
				else
				{
					if (!GameManager.HackerManager.InTestMode)
					{
						if (!DataManager.LeetMode)
						{
							this.myData.SkillPoints = this.myData.SkillPoints - this.StackPusherLevels[this.currentLevelIndex].PointsDeducted;
							DataManager.Save<StackPusherHackData>(this.myData);
						}
						if (this.currentLevelIndex == 0)
						{
							SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.STACKOVERLOAD);
						}
					}
					GameManager.HackerManager.PlayerLost();
				}
			}, 0);
		}, 0);
	}

	private void setCurrentLevelIndex()
	{
		for (int i = 0; i < this.StackPusherLevels.Count; i++)
		{
			if (this.myData.SkillPoints >= this.StackPusherLevels[i].SkillPointsRequired)
			{
				this.currentLevelIndex = i;
			}
			else
			{
				i = this.StackPusherLevels.Count;
			}
		}
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<StackPusherHackData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new StackPusherHackData(this.myID);
			this.myData.SkillPoints = 0;
		}
		if (DataManager.LeetMode)
		{
			this.myData.SkillPoints = this.StackPusherLevels[this.StackPusherLevels.Count - 1].SkillPointsRequired;
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		this.gridBlockPool = new PooledStack<StackPusherGridBlockObject>(delegate()
		{
			StackPusherGridBlockObject component = UnityEngine.Object.Instantiate<GameObject>(this.GridBlockObject, this.GridHolder.GetComponent<RectTransform>()).GetComponent<StackPusherGridBlockObject>();
			component.SoftBuild(this);
			return component;
		}, this.GRID_BLOCK_POOL_START_COUNT);
		this.stackPusherContentCG = this.StackPusherContent.GetComponent<CanvasGroup>();
		this.stackPusherContentRT = this.StackPusherContent.GetComponent<RectTransform>();
		this.gridBlockHolderCG = this.GridHolder.GetComponent<CanvasGroup>();
		this.clearGridBlockAction = new StackPusherGridBlockObject.SelfPassActions(this.clearGridBlock);
		this.GridPusher.ClearOldPushBlock += this.clearOldPushBlock;
		this.GridPusher.SetNewPushBlock += this.setNewPushBlock;
		this.GridPusher.ResetStacks += this.resetOldStacks;
		this.GridPusher.SetNewStacks += this.setNewStacks;
		this.fadeinGridBlockHolderTween = DOTween.To(() => this.gridBlockHolderCG.alpha, delegate(float x)
		{
			this.gridBlockHolderCG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear);
		this.fadeinGridBlockHolderTween.Pause<Tweener>();
		this.fadeinGridBlockHolderTween.SetAutoKill(false);
		this.presentStackPusherSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.buildBlocks));
		this.presentStackPusherSeq.Insert(0f, DOTween.To(() => this.stackPusherContentCG.alpha, delegate(float x)
		{
			this.stackPusherContentCG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		this.presentStackPusherSeq.Insert(0f, DOTween.To(() => this.stackPusherContentRT.localScale, delegate(Vector3 x)
		{
			this.stackPusherContentRT.localScale = x;
		}, this.stackPusherContentMaxScale, 0.25f).SetEase(Ease.InCirc));
		this.presentStackPusherSeq.Pause<Sequence>();
		this.presentStackPusherSeq.SetAutoKill(false);
		this.presentStackPusherSeqSmall = DOTween.Sequence().OnComplete(new TweenCallback(this.buildBlocks));
		this.presentStackPusherSeqSmall.Insert(0f, DOTween.To(() => this.stackPusherContentCG.alpha, delegate(float x)
		{
			this.stackPusherContentCG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		this.presentStackPusherSeqSmall.Insert(0f, DOTween.To(() => this.stackPusherContentRT.localScale, delegate(Vector3 x)
		{
			this.stackPusherContentRT.localScale = x;
		}, new Vector3(0.9f, 0.9f, 1f), 0.25f).SetEase(Ease.InCirc));
		this.presentStackPusherSeqSmall.Pause<Sequence>();
		this.presentStackPusherSeqSmall.SetAutoKill(false);
		this.clearStackPusherSeq = DOTween.Sequence();
		this.clearStackPusherSeq.Insert(0f, DOTween.To(() => this.stackPusherContentCG.alpha, delegate(float x)
		{
			this.stackPusherContentCG.alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear));
		this.clearStackPusherSeq.Insert(0f, DOTween.To(() => this.stackPusherContentRT.localScale, delegate(Vector3 x)
		{
			this.stackPusherContentRT.GetComponent<RectTransform>().localScale = x;
		}, this.stackPusherContentMinScale, 0.25f).SetEase(Ease.InCirc));
		this.clearStackPusherSeq.Pause<Sequence>();
		this.clearStackPusherSeq.SetAutoKill(false);
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void OnDestroy()
	{
		this.GridPusher.ClearOldPushBlock -= this.clearOldPushBlock;
		this.GridPusher.SetNewPushBlock -= this.setNewPushBlock;
		this.GridPusher.ResetStacks -= this.resetOldStacks;
		this.GridPusher.SetNewStacks -= this.setNewStacks;
	}

	public int GRID_BLOCK_POOL_START_COUNT = 169;

	public HACK_STACK_PUSHER_STATE State;

	public StackPusherGridBlockObject CurrentPushingStackBlock;

	public GraphicRaycaster MyRayCaster;

	public GameObject StackPusherContent;

	public GameObject GridHolder;

	public GameObject GridBlockObject;

	public StackPusherGridPoperObject GridPoper;

	public StackPusherGridPusherObject GridPusher;

	public AudioFileDefinition PresentStackSFX;

	public AudioFileDefinition DismissStackSFX;

	public List<StackPusherLevelDefinition> StackPusherLevels;

	private CanvasGroup stackPusherContentCG;

	private CanvasGroup gridBlockHolderCG;

	private RectTransform stackPusherContentRT;

	private Vector2 gridContentSize = Vector2.zero;

	private Vector2 stackPusherContentSize = Vector2.zero;

	private Vector3 stackPusherContentMinScale = new Vector3(0.1f, 0.1f, 1f);

	private Vector3 stackPusherContentMaxScale = Vector3.one;

	private PooledStack<StackPusherGridBlockObject> gridBlockPool;

	private MatrixStack<StackPusherGridBlockObject> currentMatrix = new MatrixStack<StackPusherGridBlockObject>();

	private Dictionary<MatrixStackCord, HACK_STACK_PUSHER_GRID_BLOCK_STATE> specialBlockTypes = new Dictionary<MatrixStackCord, HACK_STACK_PUSHER_GRID_BLOCK_STATE>(MatrixStackCordCompare.Ins);

	private MatrixStackCord specialPeicePicker = new MatrixStackCord(0, 0);

	private MatrixStackCord currentCenterCord = new MatrixStackCord(0, 0);

	private TerminalLineObject termLine1;

	private TerminalLineObject termLine2;

	private TerminalLineObject termLine3;

	private StackPusherGridBlockObject.SelfPassActions clearGridBlockAction;

	private Sequence presentStackPusherSeq;

	private Sequence presentStackPusherSeqSmall;

	private Sequence clearStackPusherSeq;

	private Tweener fadeinGridBlockHolderTween;

	private bool gameWon;

	private int stackPopIndex;

	private int currentLevelIndex;

	private float gameTime;

	private int myID;

	private StackPusherHackData myData;
}
