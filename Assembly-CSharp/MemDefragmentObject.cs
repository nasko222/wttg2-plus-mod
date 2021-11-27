using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MemDefragmentObject : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MemDefragmentObject.DefragmentActions PuzzleSolved;

	public void BuildMe(MemDefragLevelDefinition SetCurrentLevel)
	{
		if (SetCurrentLevel.MemoryCellCount > 4)
		{
			SetCurrentLevel.MemoryCellCount = 4;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = UnityEngine.Random.Range(1, 5);
		if (this.PauseGameTime != null)
		{
			this.PauseGameTime();
		}
		this.myCurrentLevel = SetCurrentLevel;
		this.curKeyIndex = 0;
		this.iWasSovled = false;
		this.curFragCount = UnityEngine.Random.Range(this.myCurrentLevel.MemoryFragmentCountMin, this.myCurrentLevel.MemoryFragmentCountMax);
		this.GenerateKey(this.curFragCount);
		List<string> list = new List<string>(this.curKey);
		this.curCoreObject.BuildMe(this.curKey);
		for (int i = 0; i < this.curFragCount; i++)
		{
			if (i != 0 && i % 4 == 0)
			{
				num3++;
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			this.fragmentObjetsData.Add(new MemDefragmentObject.FragmentData(num4, num3, list[index]));
			list.RemoveAt(index);
			if (num4 == 1 || num4 == 3)
			{
				num2++;
			}
			else
			{
				num++;
			}
			num4++;
			if (num4 > 4)
			{
				num4 = 1;
			}
		}
		list.Clear();
		num = Mathf.CeilToInt((float)num / 2f);
		num2 = Mathf.CeilToInt((float)num2 / 2f);
		this.myRT.sizeDelta = new Vector2(100f + (float)num * 55f * 2f + (float)num * 20f * 2f, 100f + (float)num2 * 55f * 2f + (float)num2 * 20f * 2f);
		if (this.myRT.sizeDelta.y >= 650f && Screen.height <= 800)
		{
			this.myRT.localScale = new Vector3(0.75f, 0.75f, 1f);
		}
		for (int j = this.fragmentObjetsData.Count - 1; j >= 0; j--)
		{
			MemFragmentObject memFragmentObject = this.memFragmentObjectPool.Pop();
			memFragmentObject.BuildMe(this.fragmentObjetsData[j].ClockPOS, this.fragmentObjetsData[j].LayerIndex, this.fragmentObjetsData[j].KeyPart);
			this.memFragmentObjects.Add(memFragmentObject);
		}
		this.fragmentObjetsData.Clear();
	}

	public void ForceEnd()
	{
		this.iWasSovled = false;
		GameManager.TimeSlinger.KillTimer(this.unlockFragsTimer);
		this.unlockFragsTimer = null;
		this.curCoreObject.DismissRefreshIcon();
		this.DestroyMe();
	}

	private void KeysWereReFlashed()
	{
		this.LockFragments();
		GameManager.TimeSlinger.FireHardTimer(out this.unlockFragsTimer, (float)this.curFragCount * 0.5f, delegate()
		{
			this.UnLockFragments();
		}, 0);
	}

	private void PresentKey()
	{
		if (this.PauseGameTime != null)
		{
			this.PauseGameTime();
		}
		this.curCoreObject.PresentKey();
	}

	private void BeginGame()
	{
		this.curKeyIndex = 0;
		this.PresentFragments();
		GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.RunGame), 0);
	}

	private void RunGame()
	{
		if (this.ResumeGameTime != null)
		{
			this.ResumeGameTime();
		}
	}

	private void GameFail()
	{
		if (this.PauseGameTime != null)
		{
			this.PauseGameTime();
		}
		this.curCoreObject.DismissRefreshIcon();
		this.RedFragments();
		GameManager.TimeSlinger.FireTimer(0.2f, delegate()
		{
			this.DismissFragments();
			GameManager.TimeSlinger.FireTimer(1.5f, new Action(this.GameReRoll), 0);
		}, 0);
	}

	private void GameReRoll()
	{
		this.GenerateKey(this.curFragCount);
		this.ResetFragments();
		this.curCoreObject.UpdateKey(this.curKey);
		this.PresentKey();
	}

	private void PresentFragments()
	{
		GameManager.AudioSlinger.PlaySound(this.MemFragmentsShowSFX);
		if (this.memFragmentObjects != null)
		{
			for (int i = 0; i < this.memFragmentObjects.Count; i++)
			{
				if (this.memFragmentObjects[i] != null && this.memFragmentObjects[i] != null)
				{
					this.memFragmentObjects[i].ExpandMe();
				}
			}
		}
	}

	private void DismissFragments()
	{
		GameManager.AudioSlinger.PlaySound(this.MemFragmentsHideSFX);
		if (this.memFragmentObjects != null)
		{
			for (int i = 0; i < this.memFragmentObjects.Count; i++)
			{
				if (this.memFragmentObjects[i] != null && this.memFragmentObjects[i] != null)
				{
					this.memFragmentObjects[i].CollapseMe();
				}
			}
		}
	}

	private void RedFragments()
	{
		if (this.memFragmentObjects != null)
		{
			for (int i = 0; i < this.memFragmentObjects.Count; i++)
			{
				if (this.memFragmentObjects[i] != null && this.memFragmentObjects[i] != null)
				{
					this.memFragmentObjects[i].GoRed();
				}
			}
		}
	}

	private void LockFragments()
	{
		if (this.memFragmentObjects != null)
		{
			for (int i = 0; i < this.memFragmentObjects.Count; i++)
			{
				if (this.memFragmentObjects[i] != null && this.memFragmentObjects[i] != null)
				{
					this.memFragmentObjects[i].LockMe();
				}
			}
		}
	}

	private void UnLockFragments()
	{
		if (this.memFragmentObjects != null)
		{
			for (int i = 0; i < this.memFragmentObjects.Count; i++)
			{
				if (this.memFragmentObjects[i] != null && this.memFragmentObjects[i] != null)
				{
					this.memFragmentObjects[i].UnLockMe();
				}
			}
		}
	}

	private void ResetFragments()
	{
		List<string> list = new List<string>(this.curKey);
		for (int i = 0; i < this.memFragmentObjects.Count; i++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			this.memFragmentObjects[i].ResetMe();
			this.memFragmentObjects[i].UpdateMyKey(list[index]);
			list.RemoveAt(index);
		}
		list.Clear();
	}

	private bool PlayerHasPickedAFrag(string theFragKey)
	{
		bool result = false;
		if (theFragKey == this.curKey[this.curKeyIndex])
		{
			GameManager.AudioSlinger.PlaySound(this.CorrectMemFragmentSelectedSFX);
			result = true;
			this.curKeyIndex++;
			if (this.curKeyIndex >= this.curKey.Count)
			{
				if (this.PauseGameTime != null)
				{
					this.PauseGameTime();
				}
				this.iWasSovled = true;
				this.DestroyMe();
			}
		}
		else
		{
			GameManager.AudioSlinger.PlaySound(this.IncorrectMemFragmentSelectedSFX);
			this.GameFail();
		}
		return result;
	}

	private void BuildKeyPool()
	{
		HACK_MEM_DEFRAG_KEY_TYPE memoryFragmentType = this.myCurrentLevel.MemoryFragmentType;
		if (memoryFragmentType != HACK_MEM_DEFRAG_KEY_TYPE.ALPHA)
		{
			if (memoryFragmentType != HACK_MEM_DEFRAG_KEY_TYPE.NUMERIC)
			{
				if (memoryFragmentType == HACK_MEM_DEFRAG_KEY_TYPE.BOTH)
				{
					for (int i = 0; i < this.alpha.Length; i++)
					{
						this.keyPool.Add(this.alpha[i]);
					}
					for (int j = 0; j < 10; j++)
					{
						this.keyPool.Add(this.num[j]);
					}
				}
			}
			else
			{
				for (int k = 0; k < 10; k++)
				{
					this.keyPool.Add(this.num[k]);
				}
			}
		}
		else
		{
			for (int l = 0; l < this.alpha.Length; l++)
			{
				this.keyPool.Add(this.alpha[l]);
			}
		}
	}

	private void GenerateKey(int setLength)
	{
		this.BuildKeyPool();
		this.curKey.Clear();
		for (int i = 0; i < setLength; i++)
		{
			int index = UnityEngine.Random.Range(0, this.keyPool.Count);
			this.curKey.Add(this.keyPool[index]);
			this.keyPool.RemoveAt(index);
		}
		this.keyPool.Clear();
	}

	private void DestroyMe()
	{
		this.DismissFragments();
		this.curCoreObject.SetLock(true);
		GameManager.TimeSlinger.FireTimer(0.5f, delegate()
		{
			for (int i = 0; i < this.memFragmentObjects.Count; i++)
			{
				this.memFragmentObjects[i].ResetMe();
				this.memFragmentObjectPool.Push(this.memFragmentObjects[i]);
			}
			this.memFragmentObjects.Clear();
			this.curCoreObject.DismissMe();
			GameManager.TimeSlinger.FireTimer(0.3f, delegate()
			{
				if (this.myRT.localScale != Vector3.one)
				{
					this.myRT.localScale = Vector3.one;
				}
				if (this.iWasSovled)
				{
					if (this.PauseGameTime != null)
					{
						this.PauseGameTime();
					}
					if (this.PuzzleSolved != null)
					{
						this.PuzzleSolved();
					}
				}
				else if (this.GameOver != null)
				{
					this.GameOver();
				}
			}, 0);
		}, 0);
	}

	private void clearPool()
	{
		foreach (MemFragmentObject memFragmentObject in this.memFragmentObjectPool)
		{
			memFragmentObject.IWasChosen -= this.PlayerHasPickedAFrag;
		}
		this.memFragmentObjectPool.Clear();
		this.memFragmentObjectPool = null;
	}

	private void Awake()
	{
		this.myRT = base.GetComponent<RectTransform>();
		this.curCoreObject = UnityEngine.Object.Instantiate<GameObject>(this.CoreObject, this.myRT).GetComponent<MemCoreObject>();
		this.curCoreObject.CoreWasShown += this.PresentKey;
		this.curCoreObject.KeyWasPresented += this.BeginGame;
		this.curCoreObject.KeysWereFlashed += this.KeysWereReFlashed;
		this.memFragmentObjectPool = new PooledStack<MemFragmentObject>(delegate()
		{
			MemFragmentObject component = UnityEngine.Object.Instantiate<GameObject>(this.FragmentObject, this.FragmentsHolder.GetComponent<RectTransform>()).GetComponent<MemFragmentObject>();
			component.IWasChosen += this.PlayerHasPickedAFrag;
			return component;
		}, 16);
	}

	private void OnDestroy()
	{
		this.clearPool();
		this.curCoreObject.CoreWasShown -= this.PresentKey;
		this.curCoreObject.KeyWasPresented -= this.BeginGame;
		this.curCoreObject.KeysWereFlashed -= this.KeysWereReFlashed;
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MemDefragmentObject.DefragmentActions GameOver;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MemDefragmentObject.DefragmentActions PauseGameTime;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MemDefragmentObject.DefragmentActions ResumeGameTime;

	public GameObject CoreObject;

	public GameObject FragmentObject;

	public GameObject FragmentsHolder;

	public AudioFileDefinition MemFragmentsShowSFX;

	public AudioFileDefinition MemFragmentsHideSFX;

	public AudioFileDefinition CorrectMemFragmentSelectedSFX;

	public AudioFileDefinition IncorrectMemFragmentSelectedSFX;

	private const float CORE_WIDTH_HEIGHT = 100f;

	private const float CORE_KEY_DELAY = 1f;

	private const float CORE_DISMISS_TIME = 0.3f;

	private const float FRAGMENT_SPACING = 20f;

	private const float FRAGMENT_WIDTH_HEIGHT = 55f;

	private const float FRAGMENT_SLIDE_TIME = 0.5f;

	private const float FRAGMENT_IMG_FADE_TIME = 0.2f;

	private string[] alpha = new string[]
	{
		"A",
		"B",
		"C",
		"D",
		"E",
		"F",
		"G",
		"H",
		"I",
		"J",
		"K",
		"L",
		"N",
		"O",
		"P",
		"Q",
		"R",
		"S",
		"T",
		"U",
		"V",
		"W",
		"X",
		"Y",
		"Z"
	};

	private string[] num = new string[]
	{
		"0",
		"1",
		"2",
		"3",
		"4",
		"5",
		"6",
		"7",
		"8",
		"9"
	};

	private MemCoreObject curCoreObject;

	private PooledStack<MemFragmentObject> memFragmentObjectPool;

	private List<MemFragmentObject> memFragmentObjects = new List<MemFragmentObject>(16);

	private List<MemDefragmentObject.FragmentData> fragmentObjetsData = new List<MemDefragmentObject.FragmentData>(36);

	private List<string> keyPool = new List<string>(35);

	private List<string> curKey = new List<string>(16);

	private int curKeyIndex;

	private int curFragCount;

	private bool iWasSovled;

	private RectTransform myRT;

	private Timer unlockFragsTimer;

	private MemDefragLevelDefinition myCurrentLevel;

	public delegate void DefragmentActions();

	private struct FragmentData
	{
		public FragmentData(int setClockPOS, int setLayerIndex, string setKeyPart)
		{
			this.ClockPOS = setClockPOS;
			this.LayerIndex = setLayerIndex;
			this.KeyPart = setKeyPart;
		}

		public int ClockPOS;

		public int LayerIndex;

		public string KeyPart;
	}
}
