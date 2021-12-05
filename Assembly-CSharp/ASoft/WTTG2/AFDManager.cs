using System;
using System.Collections.Generic;
using UnityEngine;

namespace ASoft.WTTG2
{
	public class AFDManager : MonoBehaviour
	{
		public AFDManager()
		{
			this.EnemyASD = ScriptableObject.CreateInstance<AudioSourceDefinition>();
			this.EnemyASD.id = 1;
			this.EnemyASD.PanStero = 0f;
			this.EnemyASD.SpatialBlend = 1f;
			this.EnemyASD.ReverbZoneMix = 1f;
			this.EnemyASD.DopplerLevel = 1f;
			this.EnemyASD.IsLiner = false;
			this.EnemyASD.Spread = 0f;
			this.EnemyASD.MinDistance = 2f;
			this.EnemyASD.MaxDistance = 8f;
		}

		public AudioFileDefinition AddEnemyAFD(string AFDName, AudioClip AFDClip)
		{
			AudioFileDefinition audioFileDefinition = UnityEngine.Object.Instantiate<AudioFileDefinition>(LookUp.SoundLookUp.JumpHit3);
			Definition definition = audioFileDefinition;
			int num = this.dynamicId;
			this.dynamicId = num + 1;
			definition.id = num;
			audioFileDefinition.MyAudioHub = AUDIO_HUB.UNIVERSAL;
			audioFileDefinition.MyAudioLayer = AUDIO_LAYER.ENEMY;
			audioFileDefinition.MyAudioSourceDefinition = this.EnemyASD;
			audioFileDefinition.AudioClip = AFDClip;
			if (this.currentAFDs.ContainsKey(AFDName))
			{
				this.currentAFDs[AFDName] = audioFileDefinition;
			}
			else
			{
				this.currentAFDs.Add(AFDName, audioFileDefinition);
			}
			return audioFileDefinition;
		}

		public void RemoveAFD(string AFDName)
		{
			this.currentAFDs.Remove(AFDName);
		}

		public AudioFileDefinition GetAFD(string AFDName)
		{
			if (!this.currentAFDs.ContainsKey(AFDName))
			{
				return null;
			}
			return this.currentAFDs[AFDName];
		}

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
			AFDManager.Ins = this;
			Debug.Log("AFDManager is loading...");
		}

		private void OnDestroy()
		{
			AFDManager.Ins = null;
			Debug.Log("AFDManager was unloaded!");
		}

		public AudioFileDefinition AddSwanAudio(string AFDName, AudioClip AFDClip)
		{
			AudioFileDefinition audioFileDefinition = UnityEngine.Object.Instantiate<AudioFileDefinition>(LookUp.SoundLookUp.LoudDoorBang);
			Definition definition = audioFileDefinition;
			int num = this.dynamicId;
			this.dynamicId = num + 1;
			definition.id = num;
			audioFileDefinition.AudioClip = AFDClip;
			audioFileDefinition.MyAudioHub = AUDIO_HUB.COMPUTER_HUB;
			audioFileDefinition.Volume = 0.5f;
			if (this.currentAFDs.ContainsKey(AFDName))
			{
				this.currentAFDs[AFDName] = audioFileDefinition;
			}
			else
			{
				this.currentAFDs.Add(AFDName, audioFileDefinition);
			}
			return audioFileDefinition;
		}

		public AudioFileDefinition AddPlayerAFD(string AFDName, AudioClip AFDClip, float volume)
		{
			AudioFileDefinition audioFileDefinition = UnityEngine.Object.Instantiate<AudioFileDefinition>(LookUp.SoundLookUp.JumpHit3);
			Definition definition = audioFileDefinition;
			int num = this.dynamicId;
			this.dynamicId = num + 1;
			definition.id = num;
			audioFileDefinition.AudioClip = AFDClip;
			audioFileDefinition.Volume = volume;
			if (this.currentAFDs.ContainsKey(AFDName))
			{
				this.currentAFDs[AFDName] = audioFileDefinition;
			}
			else
			{
				this.currentAFDs.Add(AFDName, audioFileDefinition);
			}
			return audioFileDefinition;
		}

		public AudioFileDefinition AddLoopAFD(string AFDName, AudioClip AFDClip)
		{
			AudioFileDefinition audioFileDefinition = UnityEngine.Object.Instantiate<AudioFileDefinition>(LookUp.SoundLookUp.JumpHit3);
			Definition definition = audioFileDefinition;
			int num = this.dynamicId;
			this.dynamicId = num + 1;
			definition.id = num;
			audioFileDefinition.AudioClip = AFDClip;
			audioFileDefinition.Volume = 1f;
			audioFileDefinition.Loop = true;
			audioFileDefinition.LoopCount = -1;
			if (this.currentAFDs.ContainsKey(AFDName))
			{
				this.currentAFDs[AFDName] = audioFileDefinition;
			}
			else
			{
				this.currentAFDs.Add(AFDName, audioFileDefinition);
			}
			return audioFileDefinition;
		}

		public AudioFileDefinition AddWebsiteAFD(string AFDName, AudioClip AFDClip, bool loop)
		{
			AudioFileDefinition audioFileDefinition = UnityEngine.Object.Instantiate<AudioFileDefinition>(LookUp.SoundLookUp.PuzzleSolved);
			Definition definition = audioFileDefinition;
			int num = this.dynamicId;
			this.dynamicId = num + 1;
			definition.id = num;
			audioFileDefinition.MyAudioLayer = AUDIO_LAYER.WEBSITE;
			audioFileDefinition.AudioClip = AFDClip;
			audioFileDefinition.Loop = loop;
			audioFileDefinition.LoopCount = -1;
			if (this.currentAFDs.ContainsKey(AFDName))
			{
				this.currentAFDs[AFDName] = audioFileDefinition;
			}
			else
			{
				this.currentAFDs.Add(AFDName, audioFileDefinition);
			}
			return audioFileDefinition;
		}

		public static AFDManager Ins;

		private Dictionary<string, AudioFileDefinition> currentAFDs = new Dictionary<string, AudioFileDefinition>();

		private AudioSourceDefinition EnemyASD;

		private int dynamicId = 354;
	}
}
