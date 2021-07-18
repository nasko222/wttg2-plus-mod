using System;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;

public class StackPusherGridBlockObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event StackPusherGridBlockObject.SelfPassActions Kill;

	public void SoftBuild(StackPusherHack SetStackPusher)
	{
		this.myStackPusherHack = SetStackPusher;
		this.myCG.alpha = 0f;
		this.myRT.anchoredPosition = this.myStartPOS;
		this.State = HACK_STACK_PUSHER_GRID_BLOCK_STATE.INACTIVE;
	}

	public void Build(MatrixStackCord SetCord, HACK_STACK_PUSHER_GRID_BLOCK_STATE SetState)
	{
		this.State = SetState;
		this.myCord = SetCord;
		this.myPOS.x = (float)this.myCord.X * 40f;
		this.myPOS.y = -((float)this.myCord.Y * 40f);
		this.myRT.anchoredPosition = this.myPOS;
		this.showBlockTween.Rewind(true);
		this.showBlockTween.Play<Tweener>();
	}

	public void ClearStackState()
	{
		this.canBePushed = false;
		if (this.State == HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_LOCKED || this.State == HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_READY)
		{
			this.State = HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_LOCKED;
			this.showStackLockedTween.Restart(true, -1f);
			this.hideStackReadyTween.Restart(true, -1f);
		}
		if (this.State == HACK_STACK_PUSHER_GRID_BLOCK_STATE.POPER)
		{
			this.myStackPusherHack.GridPoper.SetInactive();
		}
	}

	public void SetStackStateReady()
	{
		if (this.State == HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_LOCKED)
		{
			this.State = HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_READY;
			this.showStackReadyTween.Restart(true, -1f);
			this.hideStackLockedTween.Restart(true, -1f);
		}
		if (this.State == HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE)
		{
			this.canBePushed = true;
		}
		if (this.State == HACK_STACK_PUSHER_GRID_BLOCK_STATE.POPER)
		{
			this.canBePushed = true;
			this.myStackPusherHack.GridPoper.SetActive();
		}
	}

	public void ClearMyStack()
	{
		this.State = HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE;
		this.canBePushed = true;
		this.stackReadyCG.alpha = 0f;
		this.stackLockedCG.alpha = 0f;
		this.clearMyStackSeq.Restart(true, -1f);
	}

	public void CancelPushingStack()
	{
		this.hideStackActivated.Restart(true, -1f);
	}

	public void Pop()
	{
		this.State = HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE;
		this.pushStackCG.alpha = 1f;
		this.stackReadyCG.alpha = 0f;
		this.stackLockedCG.alpha = 0f;
		this.stackActivatedCG.alpha = 0f;
		this.popSeq.Restart(true, -1f);
	}

	public void Destroy()
	{
		this.State = HACK_STACK_PUSHER_GRID_BLOCK_STATE.INACTIVE;
		this.canBePushed = false;
		this.stackLockedCG.alpha = 0f;
		this.stackReadyCG.alpha = 0f;
		this.stackActiveCG.alpha = 0f;
		this.stackActivatedCG.alpha = 0f;
		this.pushCrossCG.alpha = 0f;
		this.pushStackCG.alpha = 0f;
		this.skullBlockCG.alpha = 0f;
		this.hideBlockTween.Rewind(true);
		this.hideBlockTween.Play<Tweener>();
	}

	private void nowHasStack()
	{
		this.State = HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_READY;
		this.stackReadyRT.localScale = this.myMinScale;
		this.presentStackReadySeq.Rewind(true);
		this.hidePushStackTween.Rewind(true);
		this.presentStackReadySeq.Play<Sequence>();
		this.hidePushStackTween.Play<Tweener>();
	}

	private void kill()
	{
		this.myRT.anchoredPosition = this.myStartPOS;
		this.stackLockedCG.alpha = 0f;
		this.stackReadyCG.alpha = 0f;
		this.stackActiveCG.alpha = 0f;
		this.stackActivatedCG.alpha = 0f;
		this.pushCrossCG.alpha = 0f;
		this.pushStackCG.alpha = 0f;
		this.skullBlockCG.alpha = 0f;
		this.skullLaughSeq.Pause<Sequence>();
		if (this.Kill != null)
		{
			this.Kill(this);
		}
	}

	private void subBuild()
	{
		switch (this.State)
		{
		case HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE_AND_PUSHABLE:
			this.State = HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE;
			this.canBePushed = true;
			return;
		case HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_LOCKED:
			this.showStackLockedTween.Restart(true, -1f);
			return;
		case HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_READY:
			this.showStackReadyTween.Restart(true, -1f);
			return;
		case HACK_STACK_PUSHER_GRID_BLOCK_STATE.PUSHER:
			this.myStackPusherHack.GridPusher.SetMyParent(this.myRT);
			this.myStackPusherHack.GridPusher.PresentMe();
			return;
		case HACK_STACK_PUSHER_GRID_BLOCK_STATE.POPER:
			this.myStackPusherHack.GridPoper.SetMyParent(this.myRT);
			this.myStackPusherHack.GridPoper.PresentInactive();
			return;
		case HACK_STACK_PUSHER_GRID_BLOCK_STATE.POPER_AND_ACTIVE:
			this.State = HACK_STACK_PUSHER_GRID_BLOCK_STATE.POPER;
			this.myStackPusherHack.GridPoper.SetMyParent(this.myRT);
			this.myStackPusherHack.GridPoper.PresentActive();
			this.canBePushed = true;
			return;
		case HACK_STACK_PUSHER_GRID_BLOCK_STATE.DEAD:
			this.showSkullTween.Restart(true, -1f);
			this.skullLaughSeq.Restart(true, -1f);
			return;
		}
		this.stackLockedCG.alpha = 0f;
		this.stackReadyCG.alpha = 0f;
		this.stackActiveCG.alpha = 0f;
		this.stackActivatedCG.alpha = 0f;
		this.pushCrossCG.alpha = 0f;
		this.pushStackCG.alpha = 0f;
		this.skullBlockCG.alpha = 0f;
	}

	private void Awake()
	{
		this.myRT = base.GetComponent<RectTransform>();
		this.stackActiveRT = this.StackActive.GetComponent<RectTransform>();
		this.stackReadyRT = this.StackReady.GetComponent<RectTransform>();
		this.pushStackRT = this.PushStack.GetComponent<RectTransform>();
		this.stackActivatedRT = this.StackActivated.GetComponent<RectTransform>();
		this.skullBotRT = this.SkullBot.GetComponent<RectTransform>();
		this.myCG = base.GetComponent<CanvasGroup>();
		this.stackLockedCG = this.StackLocked.GetComponent<CanvasGroup>();
		this.stackReadyCG = this.StackReady.GetComponent<CanvasGroup>();
		this.stackActiveCG = this.StackActive.GetComponent<CanvasGroup>();
		this.stackActivatedCG = this.StackActivated.GetComponent<CanvasGroup>();
		this.pushStackCG = this.PushStack.GetComponent<CanvasGroup>();
		this.pushCrossCG = this.PushCross.GetComponent<CanvasGroup>();
		this.skullBlockCG = this.SkullBlock.GetComponent<CanvasGroup>();
		this.showBlockTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.subBuild));
		this.showBlockTween.Pause<Tweener>();
		this.showBlockTween.SetAutoKill(false);
		this.showSkullTween = DOTween.To(() => this.skullBlockCG.alpha, delegate(float x)
		{
			this.skullBlockCG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear);
		this.showSkullTween.Pause<Tweener>();
		this.showSkullTween.SetAutoKill(false);
		this.hideBlockTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.kill));
		this.hideBlockTween.Pause<Tweener>();
		this.hideBlockTween.SetAutoKill(false);
		this.showStackLockedTween = DOTween.To(() => this.stackLockedCG.alpha, delegate(float x)
		{
			this.stackLockedCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear);
		this.showStackLockedTween.Pause<Tweener>();
		this.showStackLockedTween.SetAutoKill(false);
		this.hideStackLockedTween = DOTween.To(() => this.stackLockedCG.alpha, delegate(float x)
		{
			this.stackLockedCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear);
		this.hideStackLockedTween.Pause<Tweener>();
		this.hideStackLockedTween.SetAutoKill(false);
		this.showStackReadyTween = DOTween.To(() => this.stackReadyCG.alpha, delegate(float x)
		{
			this.stackReadyCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear);
		this.showStackReadyTween.Pause<Tweener>();
		this.showStackReadyTween.SetAutoKill(false);
		this.hideStackReadyTween = DOTween.To(() => this.stackReadyCG.alpha, delegate(float x)
		{
			this.stackReadyCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear);
		this.hideStackReadyTween.Pause<Tweener>();
		this.hideStackReadyTween.SetAutoKill(false);
		this.showStackActiveTween = DOTween.To(() => this.stackActiveCG.alpha, delegate(float x)
		{
			this.stackActiveCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear);
		this.showStackActiveTween.Pause<Tweener>();
		this.showStackActiveTween.SetAutoKill(false);
		this.hideStackActiveTween = DOTween.To(() => this.stackActiveCG.alpha, delegate(float x)
		{
			this.stackActiveCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear);
		this.hideStackActiveTween.Pause<Tweener>();
		this.hideStackActiveTween.SetAutoKill(false);
		this.showStackActivated = DOTween.To(() => this.stackActivatedCG.alpha, delegate(float x)
		{
			this.stackActivatedCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear);
		this.showStackActivated.Pause<Tweener>();
		this.showStackActivated.SetAutoKill(false);
		this.hideStackActivated = DOTween.To(() => this.stackActivatedCG.alpha, delegate(float x)
		{
			this.stackActivatedCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear);
		this.hideStackActivated.Pause<Tweener>();
		this.hideStackActivated.SetAutoKill(false);
		this.showPushStackTween = DOTween.To(() => this.pushStackCG.alpha, delegate(float x)
		{
			this.pushStackCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear);
		this.showPushStackTween.Pause<Tweener>();
		this.showPushStackTween.SetAutoKill(false);
		this.hidePushStackTween = DOTween.To(() => this.pushStackCG.alpha, delegate(float x)
		{
			this.pushStackCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear);
		this.hidePushStackTween.Pause<Tweener>();
		this.hidePushStackTween.SetAutoKill(false);
		this.clearMyStackSeq = DOTween.Sequence().OnComplete(delegate
		{
			this.stackActivatedRT.localScale = this.myMaxScale;
		});
		this.clearMyStackSeq.Insert(0f, DOTween.To(() => this.stackActivatedRT.localScale, delegate(Vector3 x)
		{
			this.stackActivatedRT.localScale = x;
		}, this.myMinScale, 0.15f).SetEase(Ease.Linear));
		this.clearMyStackSeq.Insert(0f, DOTween.To(() => this.stackActivatedCG.alpha, delegate(float x)
		{
			this.stackActivatedCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear));
		this.clearMyStackSeq.Pause<Sequence>();
		this.clearMyStackSeq.SetAutoKill(false);
		this.presentStackReadySeq = DOTween.Sequence();
		this.presentStackReadySeq.Insert(0.15f, DOTween.To(() => this.stackReadyRT.localScale, delegate(Vector3 x)
		{
			this.stackReadyRT.localScale = x;
		}, this.myMaxScale, 0.15f).SetEase(Ease.Linear));
		this.presentStackReadySeq.Insert(0.15f, DOTween.To(() => this.stackReadyCG.alpha, delegate(float x)
		{
			this.stackReadyCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear));
		this.presentStackReadySeq.Pause<Sequence>();
		this.presentStackReadySeq.SetAutoKill(false);
		this.popSeq = DOTween.Sequence().OnComplete(delegate
		{
			this.pushStackRT.localScale = this.myMidScale;
		});
		this.popSeq.Insert(0f, DOTween.To(() => this.stackActiveCG.alpha, delegate(float x)
		{
			this.stackActiveCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear));
		this.popSeq.Insert(0f, DOTween.To(() => this.pushStackRT.localScale, delegate(Vector3 x)
		{
			this.pushStackRT.localScale = x;
		}, this.myMaxScale, 0.25f).SetEase(Ease.Linear));
		this.popSeq.Insert(0f, DOTween.To(() => this.pushStackCG.alpha, delegate(float x)
		{
			this.pushStackCG.alpha = x;
		}, 0f, 0.3f).SetEase(Ease.Linear));
		this.popSeq.Pause<Sequence>();
		this.popSeq.SetAutoKill(false);
		this.skullLaughSeq = DOTween.Sequence();
		this.skullLaughSeq.Insert(0f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.skullLaughOpen, 0.2f).SetEase(Ease.Linear).SetRelative(true));
		this.skullLaughSeq.Insert(0.2f, DOTween.To(() => this.skullBotRT.anchoredPosition, delegate(Vector2 x)
		{
			this.skullBotRT.anchoredPosition = x;
		}, this.skullLaughClose, 0.2f).SetEase(Ease.Linear).SetRelative(true));
		this.skullLaughSeq.SetLoops(-1);
		this.skullLaughSeq.Pause<Sequence>();
		this.skullLaughSeq.SetAutoKill(false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		HACK_STACK_PUSHER_STATE state = this.myStackPusherHack.State;
		if (state != HACK_STACK_PUSHER_STATE.IDLE)
		{
			if (state != HACK_STACK_PUSHER_STATE.PUSHER_PLACEMENT)
			{
				if (state == HACK_STACK_PUSHER_STATE.PUSHING)
				{
					HACK_STACK_PUSHER_GRID_BLOCK_STATE state2 = this.State;
					if (state2 != HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE)
					{
						if (state2 == HACK_STACK_PUSHER_GRID_BLOCK_STATE.POPER)
						{
							if (this.canBePushed)
							{
								this.myStackPusherHack.GridPoper.PopMouseEnter();
							}
						}
					}
					else if (this.canBePushed)
					{
						this.showPushStackTween.Restart(true, -1f);
					}
				}
			}
			else
			{
				HACK_STACK_PUSHER_GRID_BLOCK_STATE state3 = this.State;
				if (state3 == HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE)
				{
					this.pushCrossCG.alpha = 1f;
				}
			}
		}
		else
		{
			HACK_STACK_PUSHER_GRID_BLOCK_STATE state4 = this.State;
			if (state4 != HACK_STACK_PUSHER_GRID_BLOCK_STATE.PUSHER)
			{
				if (state4 == HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_READY)
				{
					this.showStackActiveTween.Restart(true, -1f);
				}
			}
			else
			{
				this.myStackPusherHack.GridPusher.PointerEnter();
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		HACK_STACK_PUSHER_STATE state = this.myStackPusherHack.State;
		if (state != HACK_STACK_PUSHER_STATE.IDLE)
		{
			if (state != HACK_STACK_PUSHER_STATE.PUSHER_PLACEMENT)
			{
				if (state == HACK_STACK_PUSHER_STATE.PUSHING)
				{
					HACK_STACK_PUSHER_GRID_BLOCK_STATE state2 = this.State;
					if (state2 != HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE)
					{
						if (state2 == HACK_STACK_PUSHER_GRID_BLOCK_STATE.POPER)
						{
							if (this.canBePushed)
							{
								this.myStackPusherHack.GridPoper.PopMouseExit();
							}
						}
					}
					else if (this.canBePushed)
					{
						this.hidePushStackTween.Restart(true, -1f);
					}
				}
			}
			else
			{
				HACK_STACK_PUSHER_GRID_BLOCK_STATE state3 = this.State;
				if (state3 == HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE)
				{
					this.pushCrossCG.alpha = 0f;
				}
			}
		}
		else
		{
			HACK_STACK_PUSHER_GRID_BLOCK_STATE state4 = this.State;
			if (state4 != HACK_STACK_PUSHER_GRID_BLOCK_STATE.PUSHER)
			{
				if (state4 == HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_READY)
				{
					this.hideStackActiveTween.Restart(true, -1f);
				}
			}
			else
			{
				this.myStackPusherHack.GridPusher.PointerExit();
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		switch (this.myStackPusherHack.State)
		{
		case HACK_STACK_PUSHER_STATE.IDLE:
		{
			HACK_STACK_PUSHER_GRID_BLOCK_STATE state = this.State;
			if (state != HACK_STACK_PUSHER_GRID_BLOCK_STATE.PUSHER)
			{
				if (state != HACK_STACK_PUSHER_GRID_BLOCK_STATE.STACK_READY)
				{
					if (state == HACK_STACK_PUSHER_GRID_BLOCK_STATE.DEAD)
					{
						this.myStackPusherHack.Boom();
					}
				}
				else
				{
					GameManager.AudioSlinger.PlaySound(this.StackClick);
					this.myStackPusherHack.CurrentPushingStackBlock = this;
					this.myStackPusherHack.State = HACK_STACK_PUSHER_STATE.PUSHING;
					this.hideStackActiveTween.Restart(true, -1f);
					this.showStackActivated.Restart(true, -1f);
				}
			}
			else
			{
				GameManager.AudioSlinger.PlaySound(this.PusherClickSFX);
				this.myStackPusherHack.State = HACK_STACK_PUSHER_STATE.PUSHER_PLACEMENT;
				this.myStackPusherHack.GridPusher.SetActive();
			}
			break;
		}
		case HACK_STACK_PUSHER_STATE.PUSHER_PLACEMENT:
		{
			HACK_STACK_PUSHER_GRID_BLOCK_STATE state2 = this.State;
			if (state2 != HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE)
			{
				if (state2 != HACK_STACK_PUSHER_GRID_BLOCK_STATE.DEAD)
				{
					this.myStackPusherHack.State = HACK_STACK_PUSHER_STATE.IDLE;
					this.myStackPusherHack.GridPusher.SetInActive();
				}
				else
				{
					this.myStackPusherHack.Boom();
				}
			}
			else
			{
				GameManager.AudioSlinger.PlaySound(this.PusherPlaceSFX);
				this.pushCrossCG.alpha = 0f;
				this.myStackPusherHack.State = HACK_STACK_PUSHER_STATE.IDLE;
				this.myStackPusherHack.GridPusher.Move(this.myRT, this.myCord);
			}
			break;
		}
		case HACK_STACK_PUSHER_STATE.PUSHING:
		{
			HACK_STACK_PUSHER_GRID_BLOCK_STATE state3 = this.State;
			switch (state3)
			{
			case HACK_STACK_PUSHER_GRID_BLOCK_STATE.POPER:
				if (this.canBePushed)
				{
					GameManager.AudioSlinger.PlaySound(this.StackPop);
					this.myStackPusherHack.State = HACK_STACK_PUSHER_STATE.IDLE;
					this.myStackPusherHack.GridPoper.PopMouseExit();
					this.myStackPusherHack.PopStack();
				}
				break;
			default:
				if (state3 != HACK_STACK_PUSHER_GRID_BLOCK_STATE.ACTIVE)
				{
					this.myStackPusherHack.State = HACK_STACK_PUSHER_STATE.IDLE;
					this.myStackPusherHack.CurrentPushingStackBlock.CancelPushingStack();
					this.myStackPusherHack.CurrentPushingStackBlock = null;
				}
				else if (this.canBePushed)
				{
					GameManager.AudioSlinger.PlaySound(this.StackPlace);
					this.myStackPusherHack.State = HACK_STACK_PUSHER_STATE.IDLE;
					this.myStackPusherHack.CurrentPushingStackBlock.ClearMyStack();
					this.myStackPusherHack.CurrentPushingStackBlock = null;
					this.nowHasStack();
				}
				else
				{
					this.myStackPusherHack.State = HACK_STACK_PUSHER_STATE.IDLE;
					this.myStackPusherHack.CurrentPushingStackBlock.CancelPushingStack();
					this.myStackPusherHack.CurrentPushingStackBlock = null;
				}
				break;
			case HACK_STACK_PUSHER_GRID_BLOCK_STATE.DEAD:
				this.myStackPusherHack.Boom();
				break;
			}
			break;
		}
		default:
		{
			HACK_STACK_PUSHER_GRID_BLOCK_STATE state4 = this.State;
			if (state4 == HACK_STACK_PUSHER_GRID_BLOCK_STATE.DEAD)
			{
				this.myStackPusherHack.Boom();
			}
			break;
		}
		}
	}

	public HACK_STACK_PUSHER_GRID_BLOCK_STATE State;

	public GameObject SkullBlock;

	public GameObject SkullBot;

	public GameObject StackLocked;

	public GameObject StackReady;

	public GameObject StackActive;

	public GameObject StackActivated;

	public GameObject PushCross;

	public GameObject PushStack;

	public AudioFileDefinition PusherClickSFX;

	public AudioFileDefinition PusherPlaceSFX;

	public AudioFileDefinition StackClick;

	public AudioFileDefinition StackPlace;

	public AudioFileDefinition StackPop;

	private StackPusherHack myStackPusherHack;

	private MatrixStackCord myCord;

	private CanvasGroup myCG;

	private CanvasGroup stackLockedCG;

	private CanvasGroup stackReadyCG;

	private CanvasGroup stackActiveCG;

	private CanvasGroup pushCrossCG;

	private CanvasGroup pushStackCG;

	private CanvasGroup stackActivatedCG;

	private CanvasGroup skullBlockCG;

	private RectTransform myRT;

	private RectTransform stackActiveRT;

	private RectTransform stackReadyRT;

	private RectTransform pushStackRT;

	private RectTransform stackActivatedRT;

	private RectTransform skullBotRT;

	private Vector3 myMinScale = new Vector3(0.1f, 0.1f, 1f);

	private Vector3 myMidScale = new Vector3(0.67f, 0.68f, 1f);

	private Vector3 myMaxScale = Vector3.one;

	private Vector2 myStartPOS = new Vector2(-40f, 40f);

	private Vector2 skullLaughClose = new Vector2(0f, 3f);

	private Vector2 skullLaughOpen = new Vector2(0f, -3f);

	private Vector2 myPOS = Vector2.zero;

	private Sequence clearMyStackSeq;

	private Sequence presentStackReadySeq;

	private Sequence popSeq;

	private Sequence skullLaughSeq;

	private Tweener showStackActiveTween;

	private Tweener hideStackActiveTween;

	private Tweener showBlockTween;

	private Tweener hideBlockTween;

	private Tweener showStackLockedTween;

	private Tweener hideStackLockedTween;

	private Tweener showStackReadyTween;

	private Tweener hideStackReadyTween;

	private Tweener showPushStackTween;

	private Tweener hidePushStackTween;

	private Tweener showStackActivated;

	private Tweener hideStackActivated;

	private Tweener showSkullTween;

	private bool canBePushed;

	public delegate void SelfPassActions(StackPusherGridBlockObject SPGO);
}
