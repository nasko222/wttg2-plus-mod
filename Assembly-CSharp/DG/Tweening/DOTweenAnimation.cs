using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DG.Tweening
{
	[AddComponentMenu("DOTween/DOTween Animation")]
	public class DOTweenAnimation : ABSAnimationComponent
	{
		private void Awake()
		{
			if (!this.isActive || !this.isValid)
			{
				return;
			}
			if (this.animationType != DOTweenAnimationType.Move || !this.useTargetAsV3)
			{
				this.CreateTween();
				this._tweenCreated = true;
			}
		}

		private void Start()
		{
			if (this._tweenCreated || !this.isActive || !this.isValid)
			{
				return;
			}
			this.CreateTween();
			this._tweenCreated = true;
		}

		private void OnDestroy()
		{
			if (this.tween != null && this.tween.IsActive())
			{
				this.tween.Kill(false);
			}
			this.tween = null;
		}

		public void CreateTween()
		{
			if (this.target == null)
			{
				Debug.LogWarning(string.Format("{0} :: This tween's target is NULL, because the animation was created with a DOTween Pro version older than 0.9.255. To fix this, exit Play mode then simply select this object, and it will update automatically", base.gameObject.name), base.gameObject);
				return;
			}
			if (this.forcedTargetType != TargetType.Unset)
			{
				this.targetType = this.forcedTargetType;
			}
			if (this.targetType == TargetType.Unset)
			{
				this.targetType = DOTweenAnimation.TypeToDOTargetType(this.target.GetType());
			}
			switch (this.animationType)
			{
			case DOTweenAnimationType.Move:
				if (this.useTargetAsV3)
				{
					this.isRelative = false;
					if (this.endValueTransform == null)
					{
						Debug.LogWarning(string.Format("{0} :: This tween's TO target is NULL, a Vector3 of (0,0,0) will be used instead", base.gameObject.name), base.gameObject);
						this.endValueV3 = Vector3.zero;
					}
					else if (this.targetType == TargetType.RectTransform)
					{
						RectTransform rectTransform = this.endValueTransform as RectTransform;
						if (rectTransform == null)
						{
							Debug.LogWarning(string.Format("{0} :: This tween's TO target should be a RectTransform, a Vector3 of (0,0,0) will be used instead", base.gameObject.name), base.gameObject);
							this.endValueV3 = Vector3.zero;
						}
						else
						{
							RectTransform rectTransform2 = this.target as RectTransform;
							if (rectTransform2 == null)
							{
								Debug.LogWarning(string.Format("{0} :: This tween's target and TO target are not of the same type. Please reassign the values", base.gameObject.name), base.gameObject);
							}
							else
							{
								this.endValueV3 = DOTweenUtils46.SwitchToRectTransform(rectTransform, rectTransform2);
							}
						}
					}
					else
					{
						this.endValueV3 = this.endValueTransform.position;
					}
				}
				switch (this.targetType)
				{
				case TargetType.RectTransform:
					this.tween = ((RectTransform)this.target).DOAnchorPos3D(this.endValueV3, this.duration, this.optionalBool0);
					break;
				case TargetType.Rigidbody:
					this.tween = ((Rigidbody)this.target).DOMove(this.endValueV3, this.duration, this.optionalBool0);
					break;
				case TargetType.Rigidbody2D:
					this.tween = ((Rigidbody2D)this.target).DOMove(this.endValueV3, this.duration, this.optionalBool0);
					break;
				case TargetType.Transform:
					this.tween = ((Transform)this.target).DOMove(this.endValueV3, this.duration, this.optionalBool0);
					break;
				}
				break;
			case DOTweenAnimationType.LocalMove:
				this.tween = base.transform.DOLocalMove(this.endValueV3, this.duration, this.optionalBool0);
				break;
			case DOTweenAnimationType.Rotate:
			{
				TargetType targetType = this.targetType;
				if (targetType != TargetType.Transform)
				{
					if (targetType != TargetType.Rigidbody2D)
					{
						if (targetType == TargetType.Rigidbody)
						{
							this.tween = ((Rigidbody)this.target).DORotate(this.endValueV3, this.duration, this.optionalRotationMode);
						}
					}
					else
					{
						this.tween = ((Rigidbody2D)this.target).DORotate(this.endValueFloat, this.duration);
					}
				}
				else
				{
					this.tween = ((Transform)this.target).DORotate(this.endValueV3, this.duration, this.optionalRotationMode);
				}
				break;
			}
			case DOTweenAnimationType.LocalRotate:
				this.tween = base.transform.DOLocalRotate(this.endValueV3, this.duration, this.optionalRotationMode);
				break;
			case DOTweenAnimationType.Scale:
				this.tween = base.transform.DOScale((!this.optionalBool0) ? this.endValueV3 : new Vector3(this.endValueFloat, this.endValueFloat, this.endValueFloat), this.duration);
				break;
			case DOTweenAnimationType.Color:
				this.isRelative = false;
				switch (this.targetType)
				{
				case TargetType.Image:
					this.tween = ((Image)this.target).DOColor(this.endValueColor, this.duration);
					break;
				case TargetType.Light:
					this.tween = ((Light)this.target).DOColor(this.endValueColor, this.duration);
					break;
				case TargetType.Renderer:
					this.tween = ((Renderer)this.target).material.DOColor(this.endValueColor, this.duration);
					break;
				case TargetType.SpriteRenderer:
					this.tween = ((SpriteRenderer)this.target).DOColor(this.endValueColor, this.duration);
					break;
				case TargetType.Text:
					this.tween = ((Text)this.target).DOColor(this.endValueColor, this.duration);
					break;
				}
				break;
			case DOTweenAnimationType.Fade:
				this.isRelative = false;
				switch (this.targetType)
				{
				case TargetType.CanvasGroup:
					this.tween = ((CanvasGroup)this.target).DOFade(this.endValueFloat, this.duration);
					break;
				case TargetType.Image:
					this.tween = ((Image)this.target).DOFade(this.endValueFloat, this.duration);
					break;
				case TargetType.Light:
					this.tween = ((Light)this.target).DOIntensity(this.endValueFloat, this.duration);
					break;
				case TargetType.Renderer:
					this.tween = ((Renderer)this.target).material.DOFade(this.endValueFloat, this.duration);
					break;
				case TargetType.SpriteRenderer:
					this.tween = ((SpriteRenderer)this.target).DOFade(this.endValueFloat, this.duration);
					break;
				case TargetType.Text:
					this.tween = ((Text)this.target).DOFade(this.endValueFloat, this.duration);
					break;
				}
				break;
			case DOTweenAnimationType.Text:
			{
				TargetType targetType2 = this.targetType;
				if (targetType2 == TargetType.Text)
				{
					this.tween = ((Text)this.target).DOText(this.endValueString, this.duration, this.optionalBool0, this.optionalScrambleMode, this.optionalString);
				}
				break;
			}
			case DOTweenAnimationType.PunchPosition:
			{
				TargetType targetType3 = this.targetType;
				if (targetType3 != TargetType.RectTransform)
				{
					if (targetType3 == TargetType.Transform)
					{
						this.tween = ((Transform)this.target).DOPunchPosition(this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0, this.optionalBool0);
					}
				}
				else
				{
					this.tween = ((RectTransform)this.target).DOPunchAnchorPos(this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0, this.optionalBool0);
				}
				break;
			}
			case DOTweenAnimationType.PunchRotation:
				this.tween = base.transform.DOPunchRotation(this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0);
				break;
			case DOTweenAnimationType.PunchScale:
				this.tween = base.transform.DOPunchScale(this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0);
				break;
			case DOTweenAnimationType.ShakePosition:
			{
				TargetType targetType4 = this.targetType;
				if (targetType4 != TargetType.RectTransform)
				{
					if (targetType4 == TargetType.Transform)
					{
						this.tween = ((Transform)this.target).DOShakePosition(this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, this.optionalBool0, true);
					}
				}
				else
				{
					this.tween = ((RectTransform)this.target).DOShakeAnchorPos(this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, this.optionalBool0, true);
				}
				break;
			}
			case DOTweenAnimationType.ShakeRotation:
				this.tween = base.transform.DOShakeRotation(this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, true);
				break;
			case DOTweenAnimationType.ShakeScale:
				this.tween = base.transform.DOShakeScale(this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, true);
				break;
			case DOTweenAnimationType.CameraAspect:
				this.tween = ((Camera)this.target).DOAspect(this.endValueFloat, this.duration);
				break;
			case DOTweenAnimationType.CameraBackgroundColor:
				this.tween = ((Camera)this.target).DOColor(this.endValueColor, this.duration);
				break;
			case DOTweenAnimationType.CameraFieldOfView:
				this.tween = ((Camera)this.target).DOFieldOfView(this.endValueFloat, this.duration);
				break;
			case DOTweenAnimationType.CameraOrthoSize:
				this.tween = ((Camera)this.target).DOOrthoSize(this.endValueFloat, this.duration);
				break;
			case DOTweenAnimationType.CameraPixelRect:
				this.tween = ((Camera)this.target).DOPixelRect(this.endValueRect, this.duration);
				break;
			case DOTweenAnimationType.CameraRect:
				this.tween = ((Camera)this.target).DORect(this.endValueRect, this.duration);
				break;
			case DOTweenAnimationType.UIWidthHeight:
				this.tween = ((RectTransform)this.target).DOSizeDelta((!this.optionalBool0) ? this.endValueV2 : new Vector2(this.endValueFloat, this.endValueFloat), this.duration, false);
				break;
			}
			if (this.tween == null)
			{
				return;
			}
			if (this.isFrom)
			{
				((Tweener)this.tween).From(this.isRelative);
			}
			else
			{
				this.tween.SetRelative(this.isRelative);
			}
			this.tween.SetTarget(base.gameObject).SetDelay(this.delay).SetLoops(this.loops, this.loopType).SetAutoKill(this.autoKill).OnKill(delegate
			{
				this.tween = null;
			});
			if (this.isSpeedBased)
			{
				this.tween.SetSpeedBased<Tween>();
			}
			if (this.easeType == Ease.INTERNAL_Custom)
			{
				this.tween.SetEase(this.easeCurve);
			}
			else
			{
				this.tween.SetEase(this.easeType);
			}
			if (!string.IsNullOrEmpty(this.id))
			{
				this.tween.SetId(this.id);
			}
			this.tween.SetUpdate(this.isIndependentUpdate);
			if (this.hasOnStart)
			{
				if (this.onStart != null)
				{
					this.tween.OnStart(new TweenCallback(this.onStart.Invoke));
				}
			}
			else
			{
				this.onStart = null;
			}
			if (this.hasOnPlay)
			{
				if (this.onPlay != null)
				{
					this.tween.OnPlay(new TweenCallback(this.onPlay.Invoke));
				}
			}
			else
			{
				this.onPlay = null;
			}
			if (this.hasOnUpdate)
			{
				if (this.onUpdate != null)
				{
					this.tween.OnUpdate(new TweenCallback(this.onUpdate.Invoke));
				}
			}
			else
			{
				this.onUpdate = null;
			}
			if (this.hasOnStepComplete)
			{
				if (this.onStepComplete != null)
				{
					this.tween.OnStepComplete(new TweenCallback(this.onStepComplete.Invoke));
				}
			}
			else
			{
				this.onStepComplete = null;
			}
			if (this.hasOnComplete)
			{
				if (this.onComplete != null)
				{
					this.tween.OnComplete(new TweenCallback(this.onComplete.Invoke));
				}
			}
			else
			{
				this.onComplete = null;
			}
			if (this.hasOnRewind)
			{
				if (this.onRewind != null)
				{
					this.tween.OnRewind(new TweenCallback(this.onRewind.Invoke));
				}
			}
			else
			{
				this.onRewind = null;
			}
			if (this.autoPlay)
			{
				this.tween.Play<Tween>();
			}
			else
			{
				this.tween.Pause<Tween>();
			}
			if (this.hasOnTweenCreated && this.onTweenCreated != null)
			{
				this.onTweenCreated.Invoke();
			}
		}

		public override void DOPlay()
		{
			DOTween.Play(base.gameObject);
		}

		public override void DOPlayBackwards()
		{
			DOTween.PlayBackwards(base.gameObject);
		}

		public override void DOPlayForward()
		{
			DOTween.PlayForward(base.gameObject);
		}

		public override void DOPause()
		{
			DOTween.Pause(base.gameObject);
		}

		public override void DOTogglePause()
		{
			DOTween.TogglePause(base.gameObject);
		}

		public override void DORewind()
		{
			this._playCount = -1;
			DOTweenAnimation[] components = base.gameObject.GetComponents<DOTweenAnimation>();
			for (int i = components.Length - 1; i > -1; i--)
			{
				Tween tween = components[i].tween;
				if (tween != null && tween.IsInitialized())
				{
					components[i].tween.Rewind(true);
				}
			}
		}

		public override void DORestart(bool fromHere = false)
		{
			this._playCount = -1;
			if (this.tween == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(this.tween);
				}
				return;
			}
			if (fromHere && this.isRelative)
			{
				this.ReEvaluateRelativeTween();
			}
			DOTween.Restart(base.gameObject, true, -1f);
		}

		public override void DOComplete()
		{
			DOTween.Complete(base.gameObject, false);
		}

		public override void DOKill()
		{
			DOTween.Kill(base.gameObject, false);
			this.tween = null;
		}

		public void DOPlayById(string id)
		{
			DOTween.Play(base.gameObject, id);
		}

		public void DOPlayAllById(string id)
		{
			DOTween.Play(id);
		}

		public void DOPauseAllById(string id)
		{
			DOTween.Pause(id);
		}

		public void DOPlayBackwardsById(string id)
		{
			DOTween.PlayBackwards(base.gameObject, id);
		}

		public void DOPlayBackwardsAllById(string id)
		{
			DOTween.PlayBackwards(id);
		}

		public void DOPlayForwardById(string id)
		{
			DOTween.PlayForward(base.gameObject, id);
		}

		public void DOPlayForwardAllById(string id)
		{
			DOTween.PlayForward(id);
		}

		public void DOPlayNext()
		{
			DOTweenAnimation[] components = base.GetComponents<DOTweenAnimation>();
			while (this._playCount < components.Length - 1)
			{
				this._playCount++;
				DOTweenAnimation dotweenAnimation = components[this._playCount];
				if (dotweenAnimation != null && dotweenAnimation.tween != null && !dotweenAnimation.tween.IsPlaying() && !dotweenAnimation.tween.IsComplete())
				{
					dotweenAnimation.tween.Play<Tween>();
					break;
				}
			}
		}

		public void DORewindAndPlayNext()
		{
			this._playCount = -1;
			DOTween.Rewind(base.gameObject, true);
			this.DOPlayNext();
		}

		public void DORestartById(string id)
		{
			this._playCount = -1;
			DOTween.Restart(base.gameObject, id, true, -1f);
		}

		public void DORestartAllById(string id)
		{
			this._playCount = -1;
			DOTween.Restart(id, true, -1f);
		}

		public List<Tween> GetTweens()
		{
			List<Tween> list = new List<Tween>();
			DOTweenAnimation[] components = base.GetComponents<DOTweenAnimation>();
			foreach (DOTweenAnimation dotweenAnimation in components)
			{
				list.Add(dotweenAnimation.tween);
			}
			return list;
		}

		public static TargetType TypeToDOTargetType(Type t)
		{
			string text = t.ToString();
			int num = text.LastIndexOf(".");
			if (num != -1)
			{
				text = text.Substring(num + 1);
			}
			if (text.IndexOf("Renderer") != -1 && text != "SpriteRenderer")
			{
				text = "Renderer";
			}
			return (TargetType)Enum.Parse(typeof(TargetType), text);
		}

		private void ReEvaluateRelativeTween()
		{
			if (this.animationType == DOTweenAnimationType.Move)
			{
				((Tweener)this.tween).ChangeEndValue(base.transform.position + this.endValueV3, true);
			}
			else if (this.animationType == DOTweenAnimationType.LocalMove)
			{
				((Tweener)this.tween).ChangeEndValue(base.transform.localPosition + this.endValueV3, true);
			}
		}

		public float delay;

		public float duration = 1f;

		public Ease easeType = Ease.OutQuad;

		public AnimationCurve easeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		public LoopType loopType;

		public int loops = 1;

		public string id = string.Empty;

		public bool isRelative;

		public bool isFrom;

		public bool isIndependentUpdate;

		public bool autoKill = true;

		public bool isActive = true;

		public bool isValid;

		public Component target;

		public DOTweenAnimationType animationType;

		public TargetType targetType;

		public TargetType forcedTargetType;

		public bool autoPlay = true;

		public bool useTargetAsV3;

		public float endValueFloat;

		public Vector3 endValueV3;

		public Vector2 endValueV2;

		public Color endValueColor = new Color(1f, 1f, 1f, 1f);

		public string endValueString = string.Empty;

		public Rect endValueRect = new Rect(0f, 0f, 0f, 0f);

		public Transform endValueTransform;

		public bool optionalBool0;

		public float optionalFloat0;

		public int optionalInt0;

		public RotateMode optionalRotationMode;

		public ScrambleMode optionalScrambleMode;

		public string optionalString;

		private bool _tweenCreated;

		private int _playCount = -1;
	}
}
