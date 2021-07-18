using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MicManager : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MicManager.MicActions MicFound;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MicManager.MicFloatOut GetMicDB;

	public void ListenToPlayer()
	{
		this.listenToPlayer = true;
	}

	public void StopListeningToPlayer()
	{
		this.listenToPlayer = false;
	}

	private void testMic()
	{
		if (this.micIndex >= this.micDevs.Count)
		{
			this.micCheckCount++;
			if (this.micCheckCount >= this.maxMicCheckCount)
			{
				if (this.NoMic != null)
				{
					this.NoMic();
				}
			}
			else
			{
				this.micDevs.Clear();
				this.micDevs = new List<string>(Microphone.devices);
				this.micIndex = 0;
				this.testMic();
			}
		}
		else
		{
			bool flag = false;
			if (this.micIndex >= 0 && this.micIndex < this.micDevs.Count)
			{
				List<string> list = new List<string>(Microphone.devices);
				if (list.Contains(this.micDevs[this.micIndex]))
				{
					flag = true;
				}
				list.Clear();
			}
			if (flag)
			{
				this.micDevToTest = this.micDevs[this.micIndex];
				this.startRec();
				this.micIndex++;
			}
			else
			{
				this.micIndex++;
				this.micDevs = new List<string>(Microphone.devices);
			}
		}
	}

	private void playerListen()
	{
		bool flag = false;
		List<string> list = new List<string>(Microphone.devices);
		if (list.Contains(this.defaultMic))
		{
			flag = true;
		}
		list.Clear();
		if (flag)
		{
			this.listenToPlayer = true;
			this.myAS.loop = true;
			this.myAS.clip = Microphone.Start(this.defaultMic, true, 1, AudioSettings.outputSampleRate);
			while (Microphone.GetPosition(this.defaultMic) <= 0)
			{
			}
			this.myAS.Play();
			GameManager.TimeSlinger.FireTimer(10f, delegate()
			{
				this.checkForSubDBLevel = true;
				if (this.MicFound != null)
				{
					this.MicFound();
				}
			}, 0);
		}
		else
		{
			this.listenToPlayer = false;
			if (this.NoMic != null)
			{
				this.NoMic();
			}
			this.micDevs.Clear();
			this.defaultMic = string.Empty;
			this.micDevToTest = string.Empty;
			this.micDevs = new List<string>(Microphone.devices);
			if (this.micDevs.Count > 0)
			{
				this.micIndex = 0;
				this.testMic();
			}
		}
	}

	private void startRec()
	{
		bool flag = false;
		List<string> list = new List<string>(Microphone.devices);
		if (list.Contains(this.micDevToTest))
		{
			flag = true;
		}
		list.Clear();
		if (flag)
		{
			this.micTimeStamp = Time.time;
			this.checkForMic = true;
			this.myAS.loop = true;
			this.myAS.clip = Microphone.Start(this.defaultMic, true, 1, AudioSettings.outputSampleRate);
			while (Microphone.GetPosition(this.defaultMic) <= 0)
			{
			}
			this.myAS.Play();
		}
		else
		{
			this.testMic();
		}
	}

	private void prepMics()
	{
		this.listenToPlayer = false;
		this.checkForSubDBLevel = false;
		this.ignoreAvg = false;
		this.avgCheckCount = 0;
		this.myAS = base.gameObject.AddComponent<AudioSource>();
		this.myAS.spatialBlend = 0f;
		this.myAS.bypassReverbZones = true;
		this.myAS.minDistance = 0.1f;
		this.myAS.maxDistance = 0.2f;
		this.micCheckCount = 0;
		this.micIndex = -1;
		this.micDevs = new List<string>(Microphone.devices);
		if (this.micDevs.Count > 0)
		{
			this.micIndex = 0;
			this.testMic();
		}
	}

	private void gameLive()
	{
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
		if (OptionDataHook.Ins.Options.Mic)
		{
			this.prepMics();
		}
	}

	private void Awake()
	{
		GameManager.MicManager = this;
		GameManager.StageManager.TheGameIsLive += this.gameLive;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.checkForMic)
		{
			if (Time.time - this.micTimeStamp >= this.micWarmCheckTime)
			{
				this.checkForMic = false;
				this.testMic();
			}
			else if (this.Decibels > -155f)
			{
				this.checkForMic = false;
				this.defaultMic = this.micDevToTest;
				this.playerListen();
			}
		}
		if (this.listenToPlayer && this.GetMicDB != null)
		{
			this.GetMicDB(this.currentDBLevel);
		}
	}

	private void OnAudioFilterRead(float[] data, int channels)
	{
		this.Sum = 0f;
		for (int i = 0; i < data.Length; i++)
		{
			this.Sum += data[i] * data[i];
		}
		this.RMS = Mathf.Sqrt(this.Sum / this.SampleSize);
		this.Decibels = 20f * Mathf.Log10(this.RMS / this.DecibelRef);
		if (this.Decibels < -160f)
		{
			this.Decibels = -160f;
		}
		if (!this.ignoreAvg)
		{
			this.avgDecibels = Mathf.Lerp(this.avgDecibels, this.Decibels, 0.01f);
		}
		if (this.checkForSubDBLevel)
		{
			int num = (int)this.avgDecibels;
			if (Mathf.Abs(this.subDBAvg - num) < 5)
			{
				this.avgCheckCount++;
				if (this.avgCheckCount > 250)
				{
					this.ignoreAvg = true;
					this.checkForSubDBLevel = false;
				}
			}
			else
			{
				this.subDBAvg = (int)this.avgDecibels;
				this.avgCheckCount--;
				if (this.avgCheckCount <= 0)
				{
					this.avgCheckCount = 1;
				}
			}
		}
		this.currentDBLevel = (int)Mathf.Clamp(this.Decibels - this.avgDecibels, 0f, 100f);
		Array.Clear(data, 0, data.Length);
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MicManager.MicActions NoMic;

	[Range(0.5f, 5.5f)]
	public float micWarmCheckTime = 3f;

	[Range(1f, 10f)]
	public int maxMicCheckCount = 5;

	public int currentDBLevel;

	private AudioSource myAS;

	private List<string> micDevs;

	private int micIndex;

	private int micCheckCount;

	private int subDBAvg;

	private int avgCheckCount;

	private bool checkForMic;

	private bool listenToPlayer;

	private bool checkForSubDBLevel;

	private bool ignoreAvg;

	private float micTimeStamp;

	private string defaultMic;

	private string micDevToTest;

	private float DecibelRef = 0.1f;

	private float RMS;

	private float Decibels = -165f;

	private float avgDecibels = 100f;

	private float SampleSize = 1024f;

	private float Sum;

	public delegate void MicActions();

	public delegate void MicFloatOut(int intValue);
}
