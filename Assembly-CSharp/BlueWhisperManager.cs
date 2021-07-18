using System;
using UnityEngine;

public class BlueWhisperManager : MonoBehaviour
{
	public bool Owns
	{
		get
		{
			return this.myData.Owns;
		}
	}

	public void PickedUpBlueWhisper()
	{
		if (this.myData != null)
		{
			this.myData.Pending = false;
			this.myData.Owns = true;
			DataManager.Save<BlueWhisperData>(this.myData);
		}
	}

	public void ProcessSound(AudioFileDefinition TheSound)
	{
		if (this.myData.Owns)
		{
			AudioFileDefinition audioFileDefinition = new AudioFileDefinition(TheSound);
			audioFileDefinition.MyAudioSourceDefinition = this.blueWhisperAudioSource;
			audioFileDefinition.MyAudioHub = AUDIO_HUB.BLUE_WHISPER_HUB;
			audioFileDefinition.MyAudioLayer = AUDIO_LAYER.BLUE_WHISPER;
			audioFileDefinition.Volume = 0.05f;
			GameManager.AudioSlinger.PlaySound(audioFileDefinition);
		}
	}

	private void productWasPickedUp(ShadowMarketProductDefinition TheProduct)
	{
		HARDWARE_PRODUCTS productID = TheProduct.productID;
		if (productID == HARDWARE_PRODUCTS.BLUE_WHISPER)
		{
			if (this.myData != null && !this.myData.Owns)
			{
				this.myData.Pending = true;
				DataManager.Save<BlueWhisperData>(this.myData);
				BlueWhisperBehaviour.Ins.SpawnMe();
			}
		}
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<BlueWhisperData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new BlueWhisperData(this.myID);
			this.myData.Pending = false;
			this.myData.Owns = false;
		}
		GameManager.StageManager.Stage -= this.stageMe;
		GameManager.ManagerSlinger.ProductsManager.ShadowMarketProductWasActivated.Event += this.productWasPickedUp;
	}

	private void gameLive()
	{
		if (this.myData.Pending)
		{
			BlueWhisperBehaviour.Ins.SpawnMe();
		}
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
	}

	private void Awake()
	{
		BlueWhisperManager.Ins = this;
		this.myID = base.transform.position.GetHashCode();
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.TheGameIsLive += this.gameLive;
	}

	private void OnDestroy()
	{
	}

	public static BlueWhisperManager Ins;

	[SerializeField]
	private AudioSourceDefinition blueWhisperAudioSource;

	private int myID;

	private BlueWhisperData myData;
}
