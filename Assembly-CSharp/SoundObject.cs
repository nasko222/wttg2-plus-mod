using System;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event SoundObject.SoundObjectDone DonePlaying;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private event SoundObject.pauseActions soundUnPaused;

	public void AttachAudioHubObject(AudioHubObject attachedAudioHub)
	{
		this.myAudioHubObject = attachedAudioHub;
	}

	public void Build()
	{
		this.myAS = base.gameObject.AddComponent<AudioSource>();
		this.myAS.playOnAwake = false;
		if (this.myAudioHubObject != null)
		{
			this.myAudioHubObject.MuffleSound += this.audioHubMuffleAction;
			this.myAudioHubObject.UnMuffleSound += this.audioHubMuffleAction;
			this.myAudioHubObject.TriggerPause += this.audioHubPauseAction;
			this.myAudioHubObject.IAmDead += this.audioHubDestroyMeAction;
			this.myAudioHubObject.TriggerMute += this.audioHubMuteAction;
		}
	}

	public void Fire(AudioFileDefinition AudioFile)
	{
		this.MyAudioFile = AudioFile;
		if (GameManager.AudioSlinger != null)
		{
			GameManager.AudioSlinger.AddAudioLayer(this.MyAudioFile.MyAudioLayer, out this.myAudioLayerObject);
		}
		else
		{
			AudioSlingerHook.Ins.AddAudioLayer(this.MyAudioFile.MyAudioLayer, out this.myAudioLayerObject);
		}
		if (this.myAudioLayerObject != null)
		{
			this.myAudioLayerObject.MuffleSound.Event += this.audioLayerMuffleAction;
			this.myAudioLayerObject.UnMuffleSound.Event += this.audioLayerMuffleAction;
			this.myAudioLayerObject.TriggerPause.Event += this.audioLayerPauseAction;
			this.myAudioLayerObject.TriggerMute.Event += this.audioLayerMuteAction;
			this.myAudioLayerObject.IWasDestroyed.Event += this.audioLayerDestroyedAction;
		}
		this.stageAudioSource();
		this.triggerPause();
		this.triggerMute();
		this.stageAudioFile();
		if (this.iAmPaused)
		{
			this.soundUnPaused += this.playAudioFile;
		}
		else
		{
			this.playAudioFile();
		}
	}

	public void KillMe()
	{
		this.destroyMe();
	}

	public void SetCustomPitch(float PitchLevel)
	{
		this.myAS.pitch = PitchLevel;
	}

	private void stageAudioSource()
	{
		if (this.MyAudioFile != null)
		{
			if (this.MyAudioFile.MyAudioSourceDefinition.GoCustom)
			{
				this.myAS.panStereo = this.MyAudioFile.MyAudioSourceDefinition.CustomAudioSource.panStereo;
				this.myAS.dopplerLevel = this.MyAudioFile.MyAudioSourceDefinition.CustomAudioSource.dopplerLevel;
				this.myAS.spatialBlend = this.MyAudioFile.MyAudioSourceDefinition.CustomAudioSource.spatialBlend;
				this.myAS.reverbZoneMix = this.MyAudioFile.MyAudioSourceDefinition.CustomAudioSource.reverbZoneMix;
				this.myAS.spread = this.MyAudioFile.MyAudioSourceDefinition.CustomAudioSource.spread;
				this.myAS.rolloffMode = AudioRolloffMode.Custom;
				this.myAS.SetCustomCurve(AudioSourceCurveType.CustomRolloff, this.MyAudioFile.MyAudioSourceDefinition.CustomAudioSource.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
				this.myAS.minDistance = this.MyAudioFile.MyAudioSourceDefinition.CustomAudioSource.minDistance;
				this.myAS.maxDistance = this.MyAudioFile.MyAudioSourceDefinition.CustomAudioSource.maxDistance;
			}
			else
			{
				this.myAS.panStereo = this.MyAudioFile.MyAudioSourceDefinition.PanStero;
				this.myAS.spatialBlend = this.MyAudioFile.MyAudioSourceDefinition.SpatialBlend;
				this.myAS.reverbZoneMix = this.MyAudioFile.MyAudioSourceDefinition.ReverbZoneMix;
				this.myAS.dopplerLevel = this.MyAudioFile.MyAudioSourceDefinition.DopplerLevel;
				if (this.MyAudioFile.MyAudioSourceDefinition.IsLiner)
				{
					this.myAS.rolloffMode = AudioRolloffMode.Linear;
				}
				else
				{
					this.myAS.rolloffMode = AudioRolloffMode.Logarithmic;
					this.myAS.spread = this.MyAudioFile.MyAudioSourceDefinition.Spread;
				}
				this.myAS.minDistance = this.MyAudioFile.MyAudioSourceDefinition.MinDistance;
				this.myAS.maxDistance = this.MyAudioFile.MyAudioSourceDefinition.MaxDistance;
			}
		}
	}

	private void stageAudioFile()
	{
		if (this.MyAudioFile != null)
		{
			float num = this.MyAudioFile.Volume;
			this.myAS.clip = this.MyAudioFile.AudioClip;
			if (this.myAudioHubObject != null && this.myAudioHubObject.HubMuffled)
			{
				num *= this.myAudioHubObject.MuffledPerc;
			}
			if (this.myAudioLayerObject != null && this.myAudioLayerObject.IAmMuffled)
			{
				num *= this.myAudioLayerObject.MuffledAMT;
			}
			this.myAS.volume = num;
		}
	}

	private void playAudioFile()
	{
		if (this.MyAudioFile != null)
		{
			if (this.MyAudioFile.Loop)
			{
				if (this.MyAudioFile.LoopCount == -1)
				{
					this.myAS.loop = true;
				}
				else
				{
					this.curLoopCount = this.MyAudioFile.LoopCount;
				}
			}
			else
			{
				this.myAS.loop = false;
			}
			if (this.MyAudioFile.Delay)
			{
				this.delayTimeStamp = Time.time;
				this.delayIsActive = true;
				this.myAS.PlayDelayed(this.MyAudioFile.DelayAmount);
			}
			else
			{
				this.myAS.Play();
				this.audioFileWasPlayed = true;
			}
		}
	}

	private void mufflePass(float FadeTime)
	{
		if (this.MyAudioFile != null)
		{
			float num = this.MyAudioFile.Volume;
			if (this.myAudioHubObject != null && this.myAudioHubObject.HubMuffled)
			{
				num *= this.myAudioHubObject.MuffledPerc;
			}
			if (this.myAudioLayerObject != null && this.myAudioLayerObject.IAmMuffled)
			{
				num *= this.myAudioLayerObject.MuffledAMT;
			}
			if (FadeTime == 0f)
			{
				this.myAS.volume = num;
			}
			else
			{
				DOTween.To(() => this.myAS.volume, delegate(float x)
				{
					this.myAS.volume = x;
				}, num, FadeTime).SetEase(Ease.Linear);
			}
		}
	}

	private void triggerPause()
	{
		bool flag = false;
		if (this.myAudioHubObject != null)
		{
			flag = this.myAudioHubObject.HubPaused;
		}
		if (this.myAudioLayerObject != null && this.myAudioLayerObject.IAmPaused)
		{
			flag = true;
		}
		this.iAmPaused = flag;
		if (flag)
		{
			this.myAS.Pause();
		}
		else
		{
			this.myAS.UnPause();
			if (this.soundUnPaused != null)
			{
				this.soundUnPaused();
				this.soundUnPaused -= this.playAudioFile;
			}
		}
	}

	private void triggerMute()
	{
		bool mute = false;
		if (this.myAudioHubObject != null)
		{
			mute = this.myAudioHubObject.HubMuted;
		}
		if (this.myAudioLayerObject != null && this.myAudioLayerObject.IAmMuted)
		{
			mute = true;
		}
		this.myAS.mute = mute;
	}

	private void reLoop()
	{
		this.curLoopCount--;
		if (this.curLoopCount > 0)
		{
			this.myAS.Play();
			this.audioFileWasPlayed = true;
		}
		else
		{
			this.destroyMe();
		}
	}

	private void destroyMe()
	{
		base.enabled = false;
		this.audioFileWasPlayed = false;
		this.myAS.Stop();
		this.myAS.pitch = 1f;
		this.wasFrozen = false;
		this.freezeAddTime = 0f;
		this.freezeTimeStamp = 0f;
		this.delayIsActive = false;
		this.delayTimeStamp = 0f;
		this.curLoopCount = 0;
		if (this.myAudioHubObject != null)
		{
		}
		if (this.myAudioLayerObject != null)
		{
			this.myAudioLayerObject.MuffleSound.Event -= this.audioLayerMuffleAction;
			this.myAudioLayerObject.UnMuffleSound.Event -= this.audioLayerMuffleAction;
			this.myAudioLayerObject.TriggerPause.Event -= this.audioLayerPauseAction;
			this.myAudioLayerObject.TriggerMute.Event -= this.audioLayerMuteAction;
			this.myAudioLayerObject.IWasDestroyed.Event -= this.audioLayerDestroyedAction;
			this.myAudioLayerObject = null;
		}
		if (this.DonePlaying != null)
		{
			this.DonePlaying(this);
		}
	}

	private void PlayerHitPause()
	{
		if (base.enabled)
		{
			this.timeIsFrozen = true;
			this.myAS.Pause();
		}
	}

	private void PlayerHitUnPause()
	{
		if (base.enabled)
		{
			this.timeIsFrozen = false;
			this.triggerPause();
		}
	}

	private void Update()
	{
		if (this.timeIsFrozen)
		{
			if (!this.wasFrozen)
			{
				this.wasFrozen = true;
				this.freezeTimeStamp = Time.time;
			}
		}
		else
		{
			if (this.wasFrozen)
			{
				this.wasFrozen = false;
				this.freezeAddTime += Time.time - this.freezeTimeStamp;
			}
			if (this.delayIsActive && Time.time - this.freezeAddTime - this.delayTimeStamp >= this.MyAudioFile.DelayAmount)
			{
				this.delayIsActive = false;
				this.audioFileWasPlayed = true;
			}
			if (this.audioFileWasPlayed && !this.myAS.isPlaying && !this.iAmPaused)
			{
				this.audioFileWasPlayed = false;
				if (this.MyAudioFile.Loop)
				{
					this.reLoop();
				}
				else
				{
					this.destroyMe();
				}
			}
		}
	}

	private void Awake()
	{
		this.myAudioHubObject = null;
		this.audioHubMuffleAction = new AudioHubObject.AudioHubMuffleActions(this.mufflePass);
		this.audioHubPauseAction = new AudioHubObject.AudioHubVoidActions(this.triggerPause);
		this.audioHubDestroyMeAction = new AudioHubObject.AudioHubVoidActions(this.destroyMe);
		this.audioHubMuteAction = new AudioHubObject.AudioHubVoidActions(this.triggerMute);
		this.audioLayerMuffleAction = new Action<float>(this.mufflePass);
		this.audioLayerPauseAction = new Action(this.triggerPause);
		this.audioLayerMuteAction = new Action(this.triggerMute);
		this.audioLayerDestroyedAction = new Action(this.destroyMe);
		if (GameManager.PauseManager != null)
		{
			GameManager.PauseManager.GamePaused += this.PlayerHitPause;
			GameManager.PauseManager.GameUnPaused += this.PlayerHitUnPause;
		}
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		if (this.myAudioHubObject != null)
		{
			this.myAudioHubObject.MuffleSound -= this.audioHubMuffleAction;
			this.myAudioHubObject.UnMuffleSound -= this.audioHubMuffleAction;
			this.myAudioHubObject.TriggerPause -= this.audioHubPauseAction;
			this.myAudioHubObject.IAmDead -= this.audioHubDestroyMeAction;
			this.myAudioHubObject.TriggerMute -= this.audioHubMuteAction;
		}
		this.destroyMe();
		if (GameManager.PauseManager != null)
		{
			GameManager.PauseManager.GamePaused -= this.PlayerHitPause;
			GameManager.PauseManager.GameUnPaused -= this.PlayerHitUnPause;
		}
	}

	public AudioFileDefinition MyAudioFile;

	private AudioSource myAS;

	private AudioHubObject myAudioHubObject;

	private AudioLayerObject myAudioLayerObject;

	private bool delayIsActive;

	private bool audioFileWasPlayed;

	private bool iAmPaused;

	private bool timeIsFrozen;

	private bool wasFrozen;

	private float delayTimeStamp;

	private float freezeTimeStamp;

	private float freezeAddTime;

	private int curLoopCount;

	private AudioHubObject.AudioHubMuffleActions audioHubMuffleAction;

	private AudioHubObject.AudioHubVoidActions audioHubPauseAction;

	private AudioHubObject.AudioHubVoidActions audioHubDestroyMeAction;

	private AudioHubObject.AudioHubVoidActions audioHubMuteAction;

	private Action<float> audioLayerMuffleAction;

	private Action audioLayerPauseAction;

	private Action audioLayerMuteAction;

	private Action audioLayerDestroyedAction;

	public delegate void SoundObjectDone(SoundObject soundObject);

	private delegate void pauseActions();
}
