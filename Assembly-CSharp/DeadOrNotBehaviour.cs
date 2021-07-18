using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class DeadOrNotBehaviour : MonoBehaviour
{
	public void StartViewers()
	{
		this.currentViewerCount = UnityEngine.Random.Range(666, 999);
		this.viewersText.text = this.currentViewerCount.ToString();
		this.viewerCountTimeStamp = Time.time;
		this.viewerCountWindow = UnityEngine.Random.Range(1f, 3f);
		this.viewerCountActive = true;
		this.prepareTween = DOTween.To(() => this.prepareText.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.prepareText.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		this.prepareTween.Play<Tweener>();
	}

	public void StartFirstVid()
	{
		this.prepareTween.Kill(false);
		this.prepareText.SetActive(false);
		this.pleaseWaitTween = DOTween.To(() => this.pleaseWaitCG.alpha, delegate(float x)
		{
			this.pleaseWaitCG.alpha = x;
		}, 1f, 0.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		this.pleaseWaitTween.Play<Tweener>();
		this.currentDeathVoteCount = 0;
		this.currentLifeVoteCount = 0;
		this.deathVoteCountText.text = "0";
		this.lifeVoteCountText.text = "0";
		this.presentVoteSeq = DOTween.Sequence().OnComplete(delegate
		{
			this.votingTimeStamp = Time.time;
			this.votingWindow = UnityEngine.Random.Range(0.5f, 3.5f);
			this.votingActive = true;
		});
		this.presentVoteSeq.Insert(0f, DOTween.To(() => this.castYourVotesTextCG.alpha, delegate(float x)
		{
			this.castYourVotesTextCG.alpha = x;
		}, 1f, 0.75f).SetEase(Ease.Linear));
		this.presentVoteSeq.Insert(2.75f, DOTween.To(() => this.castYourVotesTextCG.alpha, delegate(float x)
		{
			this.castYourVotesTextCG.alpha = x;
		}, 0f, 0.5f).SetEase(Ease.Linear));
		this.presentVoteSeq.Insert(3.25f, DOTween.To(() => this.castYourVotesHolderRT.anchoredPosition, delegate(Vector2 x)
		{
			this.castYourVotesHolderRT.anchoredPosition = x;
		}, new Vector2(0f, 15f), 0.75f).SetEase(Ease.OutQuad));
		this.presentVoteSeq.Play<Sequence>();
		this.presentChatTween = DOTween.To(() => this.chatHolderRT.anchoredPosition, delegate(Vector2 x)
		{
			this.chatHolderRT.anchoredPosition = x;
		}, Vector2.zero, 0.75f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.chatWindow = UnityEngine.Random.Range(1.5f, 6f);
			this.chatTimeStamp = Time.time;
			this.chatActive = true;
		});
		this.presentChatTween.Play<Tweener>();
	}

	public void ClearOut()
	{
		this.prepareTween.Kill(false);
		this.pleaseWaitTween.Kill(false);
		this.presentVoteSeq.Kill(false);
		this.presentChatTween.Kill(false);
		this.viewerCountActive = false;
		this.votingActive = false;
		this.chatActive = false;
		for (int i = 0; i < this.currentChatLineObjects.Count; i++)
		{
			this.chatLineObjectPool.Push(this.currentChatLineObjects[i]);
		}
		foreach (DeadOrNotChatObject deadOrNotChatObject in this.chatLineObjectPool)
		{
			if (deadOrNotChatObject != null)
			{
				UnityEngine.Object.Destroy(deadOrNotChatObject.gameObject);
			}
		}
		this.chatLineObjectPool.Clear();
	}

	private void addChat()
	{
		if (this.chatLines.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, this.chatLines.Count);
			string chatText = this.chatLines[index];
			this.chatLines.RemoveAt(index);
			DeadOrNotChatObject deadOrNotChatObject = this.chatLineObjectPool.Pop();
			deadOrNotChatObject.Build(chatText);
			this.currentChatLineObjects.Add(deadOrNotChatObject);
			float num = 0f;
			for (int i = this.currentChatLineObjects.Count - 1; i >= 0; i--)
			{
				this.currentChatLineObjects[i].MoveUp(num);
				num = num + this.currentChatLineObjects[i].Height + 8f;
			}
		}
		else
		{
			this.chatActive = false;
		}
	}

	private void Awake()
	{
		DeadOrNotBehaviour.Ins = this;
		for (int i = 0; i < this.staticChatLines.Length; i++)
		{
			this.chatLines.Add(this.staticChatLines[i]);
		}
		this.chatLineObjectPool = new PooledStack<DeadOrNotChatObject>(delegate()
		{
			DeadOrNotChatObject component = UnityEngine.Object.Instantiate<GameObject>(this.chatLineObject, this.chatContentHolderRT).GetComponent<DeadOrNotChatObject>();
			component.SoftBuild();
			return component;
		}, this.CHAT_LINE_OBJECT_POOL_COUNT);
	}

	private void Update()
	{
		if (this.viewerCountActive && Time.time - this.viewerCountTimeStamp >= this.viewerCountWindow)
		{
			this.viewerCountTimeStamp = Time.time;
			this.viewerCountWindow = UnityEngine.Random.Range(1f, 4f);
			this.currentViewerCount += UnityEngine.Random.Range(1, 9);
			this.viewersText.text = this.currentViewerCount.ToString();
		}
		if (this.votingActive && Time.time - this.votingTimeStamp >= this.votingWindow)
		{
			this.votingTimeStamp = Time.time;
			this.votingWindow = UnityEngine.Random.Range(0.5f, 3.5f);
			this.currentDeathVoteCount += UnityEngine.Random.Range(2, 10);
			this.currentDeathVoteCount = Mathf.Min(this.currentDeathVoteCount, 666);
			int num = UnityEngine.Random.Range(0, 10);
			if (num < 3)
			{
				this.currentLifeVoteCount++;
				this.currentLifeVoteCount = Mathf.Min(this.currentLifeVoteCount, 15);
			}
			this.deathVoteCountText.text = this.currentDeathVoteCount.ToString();
			this.lifeVoteCountText.text = this.currentLifeVoteCount.ToString();
		}
		if (this.chatActive && Time.time - this.chatTimeStamp >= this.chatWindow)
		{
			this.chatTimeStamp = Time.time;
			this.chatWindow = UnityEngine.Random.Range(1.25f, 6.5f);
			this.addChat();
		}
	}

	public static DeadOrNotBehaviour Ins;

	public int CHAT_LINE_OBJECT_POOL_COUNT = 60;

	[SerializeField]
	private Text viewersText;

	[SerializeField]
	private GameObject prepareText;

	[SerializeField]
	private CanvasGroup pleaseWaitCG;

	[SerializeField]
	private CanvasGroup castYourVotesTextCG;

	[SerializeField]
	private RectTransform castYourVotesHolderRT;

	[SerializeField]
	private Text deathVoteCountText;

	[SerializeField]
	private Text lifeVoteCountText;

	[SerializeField]
	private RectTransform chatHolderRT;

	[SerializeField]
	private RectTransform chatContentHolderRT;

	[SerializeField]
	private GameObject chatLineObject;

	[SerializeField]
	private string[] staticChatLines = new string[0];

	private Tweener prepareTween;

	private Tweener pleaseWaitTween;

	private Tweener presentChatTween;

	private PooledStack<DeadOrNotChatObject> chatLineObjectPool;

	private List<DeadOrNotChatObject> currentChatLineObjects = new List<DeadOrNotChatObject>(60);

	private List<string> chatLines = new List<string>(60);

	private Sequence presentVoteSeq;

	private bool viewerCountActive;

	private bool votingActive;

	private bool chatActive;

	private int currentViewerCount;

	private int currentDeathVoteCount;

	private int currentLifeVoteCount;

	private float viewerCountTimeStamp;

	private float viewerCountWindow;

	private float votingTimeStamp;

	private float votingWindow;

	private float chatTimeStamp;

	private float chatWindow;
}
