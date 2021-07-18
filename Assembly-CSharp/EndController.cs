using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class EndController : mouseableController
{
	public bool AllowPlayerResponseSelection
	{
		set
		{
			this.allowPlayerResponseSelection = value;
		}
	}

	public void PrepareForDeath()
	{
		base.SetMasterLock(true);
		DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -180f, 0f), 0.5f).SetEase(Ease.Linear).SetOptions(true).OnComplete(delegate
		{
			Sequence sequence = DOTween.Sequence().OnComplete(delegate
			{
				CultFemaleEndingDeath.Ins.TriggerDeath();
			});
			GameManager.TimeSlinger.FireTimer(3.5f, new Action(AdamBehaviour.Ins.LetHerGo), 0);
			sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, Vector3.zero, 0.25f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Insert(0.25f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, new Vector3(-15f, 0f, 0f), 1f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Insert(1.25f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, Vector3.zero, 4f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Insert(1.25f, DOTween.To(() => this.chairTrans.localRotation, delegate(Quaternion x)
			{
				this.chairTrans.localRotation = x;
			}, new Vector3(0f, -180f, 0f), 4f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Insert(5.25f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.MyCamera.transform.localRotation = x;
			}, Vector3.zero, 1f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Play<Sequence>();
		});
	}

	public void PrepareForLife()
	{
		base.SetMasterLock(true);
		CamWalkOut.Ins.WalkOut();
	}

	private void getInput()
	{
		if (!this.lockControl)
		{
			float axis = CrossPlatformInputManager.GetAxis("HorizontalDesk");
			if (axis >= 1f && !this.achAlreadySent)
			{
				this.achAlreadySent = true;
				SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.WHOSTHATLADY);
			}
			float y = -180f + axis * 75f;
			Vector3 euler = new Vector3(0f, y, 0f);
			base.transform.rotation = Quaternion.Euler(euler);
			if (this.allowPlayerResponseSelection)
			{
				if (CrossPlatformInputManager.GetButtonDown("AlphaOne"))
				{
					this.allowPlayerResponseSelection = false;
					this.PlayerSelectedOptionOne.Execute();
				}
				else if (CrossPlatformInputManager.GetButtonDown("AlphaTwo"))
				{
					this.allowPlayerResponseSelection = false;
					this.PlayerSelectedOptionTwo.Execute();
				}
			}
		}
	}

	protected new void Awake()
	{
		EndController.Ins = this;
		base.Awake();
		ControllerManager.Add(this);
	}

	protected new void Start()
	{
		base.Start();
		this.MyCamera.transform.SetParent(this.MouseRotatingObject.transform);
		this.MyCamera.transform.localPosition = Vector3.zero;
		this.MyCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
		if (!this.MouseCaptureInit)
		{
			base.Init();
		}
		this.Active = true;
		this.MyState = GAME_CONTROLLER_STATE.IDLE;
		this.playerEndingAudioHub.PlaySound(this.endingBGMusic);
	}

	protected new void Update()
	{
		base.Update();
		this.getInput();
	}

	protected new void OnDestroy()
	{
		ControllerManager.Remove(this.Controller);
		base.OnDestroy();
	}

	public static EndController Ins;

	public CustomEvent PlayerSelectedOptionOne = new CustomEvent(2);

	public CustomEvent PlayerSelectedOptionTwo = new CustomEvent(2);

	[SerializeField]
	private AudioHubObject playerEndingAudioHub;

	[SerializeField]
	private AudioFileDefinition endingBGMusic;

	[SerializeField]
	private Transform chairTrans;

	private bool allowPlayerResponseSelection;

	private bool achAlreadySent;
}
