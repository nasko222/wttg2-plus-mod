using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeHexerHack : MonoBehaviour
{
	public MatrixStack<NodeHexObject> CurrentMatrix
	{
		get
		{
			return this.currentMatrix;
		}
	}

	public bool IsGameWon()
	{
		this.currentTagPieces--;
		if (this.currentTagPieces <= 0)
		{
			GameManager.AudioSlinger.PlaySound(this.GameWonSFX);
			this.gameWon = true;
			this.clearGame();
			return true;
		}
		return false;
	}

	public void ForceGameOver()
	{
		GameManager.AudioSlinger.PlaySound(this.ForceGameOverSFX);
		this.gameWon = false;
		this.clearGame();
	}

	public void PrepNodeHexAttack(HACK_SWEEPER_SKILL_TIER SetTier)
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
			this.currentLevelIndex = this.NodeHexerLevels.Count - 1;
			break;
		}
		if (this.currentLevelIndex > this.NodeHexerLevels.Count - 1)
		{
			this.currentLevelIndex = this.NodeHexerLevels.Count - 1;
		}
		if (GameManager.HackerManager.InTestMode)
		{
			this.currentLevelIndex = 0;
		}
		this.MyRayCaster.enabled = true;
		this.NodeHexerContentCG.interactable = true;
		this.NodeHexerContentCG.blocksRaycasts = true;
		this.NodeHexerContentCG.ignoreParentGroups = true;
		GameManager.AudioSlinger.PlaySound(GameManager.HackerManager.HackingTypeSFX);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine1, TERMINAL_LINE_TYPE.TYPE, "> ./N0D3H3X3R", 0.2f, 0f);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine2, TERMINAL_LINE_TYPE.TYPE, "  Loading N0D3H3X3R v6.13.70", 0.2f, 0.2f);
		GameManager.HackerManager.HackingTerminal.TerminalHelper.AddLine(out this.termLine3, TERMINAL_LINE_TYPE.TYPE, "  Initialzing...", 0.2f, 0.4f);
		GameManager.TimeSlinger.FireTimer(0.6f, new Action(this.buildNodeHexAttack), 0);
	}

	public void AddTimeBoost(MatrixStackCord TagCord)
	{
		if (this.nodesThatNeedToBeTagged.ContainsKey(TagCord))
		{
			this.nodesThatNeedToBeTagged.Remove(TagCord);
			if (GameManager.HackerManager.HackingTimer.CurrentHackerTimerObject.HackerTimeTimer != null)
			{
				GameManager.AudioSlinger.PlaySound(this.TimeBoostSFX);
				float setDur = GameManager.HackerManager.HackingTimer.CurrentHackerTimerObject.GetTimeLeft() + this.timeBoost;
				GameManager.HackerManager.HackingTimer.CurrentHackerTimerObject.ResetMe(setDur, new Action(this.firePath));
			}
		}
	}

	private void buildNodeHexAttack()
	{
		int matrixSize = this.NodeHexerLevels[this.currentLevelIndex].MatrixSize;
		int tagPieces = this.NodeHexerLevels[this.currentLevelIndex].TagPieces;
		this.timeBoost = this.NodeHexerLevels[this.currentLevelIndex].TimeBoost;
		this.TotalMoves = 0;
		this.nodeHolderSize.x = (float)matrixSize * 50f + (float)(matrixSize - 1) * 10f;
		this.nodeHolderSize.y = (float)matrixSize * 50f + (float)(matrixSize - 1) * 10f;
		this.NodeHolderRT.sizeDelta = this.nodeHolderSize;
		this.NodeHexerContentRT.sizeDelta = this.nodeHolderSize;
		if (this.NodeHexerContentRT.sizeDelta.y >= 650f && Screen.height <= 800)
		{
			this.NodeHexerContentRT.localScale = new Vector3(0.72f, 0.72f, 1f);
		}
		this.NodeHexerContentCG.alpha = 1f;
		this.currentMatrix.SetMatrixSize(matrixSize);
		int num = matrixSize * matrixSize / 2;
		int num2 = matrixSize * matrixSize / 2;
		int i = 0;
		int j = 0;
		this.stackCords.Clear();
		this.nodesThatNeedToBeTagged.Clear();
		if (matrixSize * matrixSize - (num + num2) != 0)
		{
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				num++;
			}
			else
			{
				num2++;
			}
		}
		for (int k = 0; k < matrixSize * matrixSize; k++)
		{
			NodeHexObject nodeHexObject = this.nodeHexObjectPool.Pop();
			this.currentMatrix.Push(nodeHexObject);
			nodeHexObject.SoftBuild(this.currentMatrix.Pointer, HACK_NODE_HEXER_NODE_TYPE.BETA);
			this.stackCords.Add(this.currentMatrix.Pointer);
		}
		bool flag = true;
		int num3 = 0;
		int num4 = UnityEngine.Random.Range(0, this.stackCords.Count);
		int num5 = UnityEngine.Random.Range(2, matrixSize - 1);
		while (flag)
		{
			while (i < num)
			{
				NodeHexObject nodeHexObject2;
				if (this.currentMatrix.TryAndGetValue(out nodeHexObject2, this.stackCords[num4]))
				{
					if (nodeHexObject2.Type != HACK_NODE_HEXER_NODE_TYPE.ALPHA)
					{
						nodeHexObject2.Type = HACK_NODE_HEXER_NODE_TYPE.ALPHA;
						if (this.currentMatrix.Set(nodeHexObject2, this.stackCords[num4]))
						{
							i++;
						}
					}
					else if (num5 == 2)
					{
						num5++;
					}
					else
					{
						num5--;
					}
				}
				num4 += num5;
				if (num4 >= this.stackCords.Count)
				{
					num4 -= this.stackCords.Count;
				}
			}
			foreach (NodeHexObject sourceNode in this.currentMatrix.GetAll())
			{
				if (this.noWayOutCheck(sourceNode))
				{
					num3++;
				}
			}
			if (num3 > 0)
			{
				num4 = UnityEngine.Random.Range(0, this.stackCords.Count);
				num5 = UnityEngine.Random.Range(2, matrixSize - 1);
				i = 0;
				num3 = 0;
				foreach (NodeHexObject nodeHexObject3 in this.currentMatrix.GetAll())
				{
					nodeHexObject3.Type = HACK_NODE_HEXER_NODE_TYPE.BETA;
				}
			}
			else
			{
				num3 = 0;
				flag = false;
			}
		}
		this.hexTypePicker.X = 0;
		this.hexTypePicker.Y = UnityEngine.Random.Range(1, matrixSize);
		this.startNode = this.currentMatrix.Get(this.hexTypePicker);
		this.currentTagPieces = tagPieces;
		while (j < tagPieces)
		{
			this.hexTypePicker.X = ((j != tagPieces - 1) ? UnityEngine.Random.Range(1, matrixSize - 1) : (matrixSize - 1));
			this.hexTypePicker.Y = UnityEngine.Random.Range(0, matrixSize);
			if (!this.nodesThatNeedToBeTagged.ContainsKey(this.hexTypePicker))
			{
				this.nodesThatNeedToBeTagged.Add(this.hexTypePicker, true);
				j++;
			}
		}
		int colIndex = 0;
		int num6 = 0;
		GameManager.AudioSlinger.PlaySound(this.ShowRowSFX);
		foreach (NodeHexObject nodeHexObject4 in this.currentMatrix.GetAll())
		{
			bool needsToBeTagged = this.nodesThatNeedToBeTagged.ContainsKey(nodeHexObject4.MyCord);
			nodeHexObject4.Build(matrixSize, colIndex, needsToBeTagged);
			colIndex++;
			if (colIndex >= matrixSize)
			{
				num6++;
				colIndex = 0;
				if (num6 < matrixSize)
				{
					GameManager.TimeSlinger.FireTimer((float)num6 * 0.2f, delegate()
					{
						GameManager.AudioSlinger.PlaySound(this.ShowRowSFX);
					}, 0);
				}
			}
		}
		colIndex = 0;
		GameManager.TimeSlinger.FireTimer((float)matrixSize * 0.2f, delegate()
		{
			this.StartArrow.Present(this.startNode.GetComponent<RectTransform>());
			foreach (KeyValuePair<MatrixStackCord, bool> keyValuePair in this.nodesThatNeedToBeTagged)
			{
				NodeHexObject nodeHexObject5;
				if (this.currentMatrix.TryAndGetValue(out nodeHexObject5, keyValuePair.Key))
				{
					nodeHexObject5.ActivateNeedsToBeTagged(colIndex);
					colIndex++;
				}
			}
			GameManager.HackerManager.HackingTimer.FireWarmUpTimer(3);
			GameManager.TimeSlinger.FireTimer(3f, new Action(this.beginGame), 0);
		}, 0);
	}

	private void beginGame()
	{
		this.gameWon = false;
		this.startNode.SetPlayable();
		GameManager.HackerManager.HackingTimer.FireHackingTimer(this.NodeHexerLevels[this.currentLevelIndex].StartTime, new Action(this.firePath));
	}

	private void clearGame()
	{
		foreach (NodeHexObject nodeHexObject in this.currentMatrix.GetAll())
		{
			nodeHexObject.Clear();
			this.nodeHexObjectPool.Push(nodeHexObject);
		}
		this.StartArrow.Clear();
		this.currentMatrix.Clear();
		this.termLine1.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		this.termLine2.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		this.termLine3.AniHardClear(TERMINAL_LINE_TYPE.TYPE, 0.25f);
		GameManager.TimeSlinger.FireTimer(0.4f, delegate()
		{
			this.MyRayCaster.enabled = false;
			this.NodeHexerContentCG.alpha = 0f;
			this.NodeHexerContentCG.interactable = false;
			this.NodeHexerContentCG.blocksRaycasts = false;
			this.NodeHexerContentCG.ignoreParentGroups = false;
			if (this.NodeHexerContentRT.localScale != Vector3.one)
			{
				this.NodeHexerContentRT.localScale = Vector3.one;
			}
			GameManager.HackerManager.HackingTerminal.TerminalHelper.ClearTerminal();
			if (this.gameWon)
			{
				if (!GameManager.HackerManager.InTestMode)
				{
					if (!DataManager.LeetMode)
					{
						if (this.TotalMoves <= this.NodeHexerLevels[this.currentLevelIndex].MatrixSize * this.NodeHexerLevels[this.currentLevelIndex].MatrixSize / 2)
						{
							this.myData.SkillPoints = this.myData.SkillPoints + this.NodeHexerLevels[this.currentLevelIndex].PointsRewaredTier1;
						}
						else
						{
							this.myData.SkillPoints = this.myData.SkillPoints + this.NodeHexerLevels[this.currentLevelIndex].PointsRewaredTier2;
						}
						DataManager.Save<NodeHexerHackData>(this.myData);
					}
					if (this.currentLevelIndex == this.NodeHexerLevels.Count - 1)
					{
						SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.HOOKUPMASTER);
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
						this.myData.SkillPoints = this.myData.SkillPoints - this.NodeHexerLevels[this.currentLevelIndex].PointsDeducted;
						DataManager.Save<NodeHexerHackData>(this.myData);
					}
					if (this.currentLevelIndex == 0)
					{
						SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.IDONTNODE);
					}
				}
				GameManager.HackerManager.PlayerLost();
			}
		}, 0);
	}

	private void firePath()
	{
		this.startNode.Activate(HACK_NODE_HEXER_NODE_TYPE.DEAD);
	}

	public bool noWayOutCheck(NodeHexObject SourceNode)
	{
		int num = 0;
		NodeHexObject nodeHexObject;
		if (this.currentMatrix.TryAndGetValueByClock(out nodeHexObject, SourceNode.MyCord, MATRIX_STACK_CLOCK_POSITION.NOON))
		{
			if (nodeHexObject.Type == SourceNode.Type)
			{
				num++;
			}
		}
		else
		{
			num++;
		}
		if (this.currentMatrix.TryAndGetValueByClock(out nodeHexObject, SourceNode.MyCord, MATRIX_STACK_CLOCK_POSITION.ONE))
		{
			if (nodeHexObject.Type == SourceNode.Type)
			{
				num++;
			}
		}
		else
		{
			num++;
		}
		if (this.currentMatrix.TryAndGetValueByClock(out nodeHexObject, SourceNode.MyCord, MATRIX_STACK_CLOCK_POSITION.THREE))
		{
			if (nodeHexObject.Type == SourceNode.Type)
			{
				num++;
			}
		}
		else
		{
			num++;
		}
		if (this.currentMatrix.TryAndGetValueByClock(out nodeHexObject, SourceNode.MyCord, MATRIX_STACK_CLOCK_POSITION.FOUR))
		{
			if (nodeHexObject.Type == SourceNode.Type)
			{
				num++;
			}
		}
		else
		{
			num++;
		}
		if (this.currentMatrix.TryAndGetValueByClock(out nodeHexObject, SourceNode.MyCord, MATRIX_STACK_CLOCK_POSITION.SIX))
		{
			if (nodeHexObject.Type == SourceNode.Type)
			{
				num++;
			}
		}
		else
		{
			num++;
		}
		if (this.currentMatrix.TryAndGetValueByClock(out nodeHexObject, SourceNode.MyCord, MATRIX_STACK_CLOCK_POSITION.SEVEN))
		{
			if (nodeHexObject.Type == SourceNode.Type)
			{
				num++;
			}
		}
		else
		{
			num++;
		}
		if (this.currentMatrix.TryAndGetValueByClock(out nodeHexObject, SourceNode.MyCord, MATRIX_STACK_CLOCK_POSITION.NINE))
		{
			if (nodeHexObject.Type == SourceNode.Type)
			{
				num++;
			}
		}
		else
		{
			num++;
		}
		if (this.currentMatrix.TryAndGetValueByClock(out nodeHexObject, SourceNode.MyCord, MATRIX_STACK_CLOCK_POSITION.TEN))
		{
			if (nodeHexObject.Type == SourceNode.Type)
			{
				num++;
			}
		}
		else
		{
			num++;
		}
		return num >= 7;
	}

	private void setCurrentLevelIndex()
	{
		for (int i = 0; i < this.NodeHexerLevels.Count; i++)
		{
			if (this.myData.SkillPoints >= this.NodeHexerLevels[i].SkillPointsRequired)
			{
				this.currentLevelIndex = i;
			}
			else
			{
				i = this.NodeHexerLevels.Count;
			}
		}
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<NodeHexerHackData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new NodeHexerHackData(this.myID);
			this.myData.SkillPoints = 0;
		}
		if (DataManager.LeetMode)
		{
			this.myData.SkillPoints = this.NodeHexerLevels[this.NodeHexerLevels.Count - 1].SkillPointsRequired;
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		this.nodeHexObjectPool = new PooledStack<NodeHexObject>(delegate()
		{
			NodeHexObject component = UnityEngine.Object.Instantiate<GameObject>(this.NodeHexObject, this.NodeHolderRT).GetComponent<NodeHexObject>();
			component.PoolBuild(this);
			return component;
		}, this.START_NODE_HEX_OBJECT_POOL_COUNT);
		GameManager.StageManager.Stage += this.stageMe;
	}

	public int START_NODE_HEX_OBJECT_POOL_COUNT = 36;

	public int TotalMoves;

	public GraphicRaycaster MyRayCaster;

	public GameObject NodeHexObject;

	public NodeHexStartArrowObject StartArrow;

	public CanvasGroup NodeHexerContentCG;

	public RectTransform NodeHexerContentRT;

	public RectTransform NodeHolderRT;

	public List<NodeHexerLevelDefinition> NodeHexerLevels;

	public AudioFileDefinition ShowRowSFX;

	public AudioFileDefinition TimeBoostSFX;

	public AudioFileDefinition ForceGameOverSFX;

	public AudioFileDefinition GameWonSFX;

	private PooledStack<NodeHexObject> nodeHexObjectPool;

	private MatrixStack<NodeHexObject> currentMatrix = new MatrixStack<NodeHexObject>();

	private Dictionary<MatrixStackCord, bool> nodesThatNeedToBeTagged = new Dictionary<MatrixStackCord, bool>(MatrixStackCordCompare.Ins);

	private List<MatrixStackCord> stackCords = new List<MatrixStackCord>();

	private Vector2 nodeHolderSize = Vector2.zero;

	private MatrixStackCord hexTypePicker = new MatrixStackCord(0, 0);

	private NodeHexObject startNode;

	private TerminalLineObject termLine1;

	private TerminalLineObject termLine2;

	private TerminalLineObject termLine3;

	private int currentTagPieces;

	private int currentLevelIndex;

	private float timeBoost;

	private bool gameWon;

	private int myID;

	private NodeHexerHackData myData;
}
