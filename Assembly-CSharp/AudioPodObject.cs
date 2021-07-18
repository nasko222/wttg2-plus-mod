using System;
using UnityEngine;

[RequireComponent(typeof(AudioHubObject))]
public class AudioPodObject : MonoBehaviour
{
	private void gameIsStaging()
	{
		for (int i = 0; i < this.gameIsStagingSFXS.Length; i++)
		{
			if (this.isUniversal)
			{
				this.myAudioHubObject.PlaySound(this.gameIsStagingSFXS[i]);
			}
			else
			{
				GameManager.AudioSlinger.PlaySound(this.gameIsStagingSFXS[i]);
			}
		}
		GameManager.StageManager.TheGameIsStageing -= this.gameIsStaging;
	}

	private void gameIsLive()
	{
		for (int i = 0; i < this.gameIsLiveSFXS.Length; i++)
		{
			if (this.isUniversal)
			{
				this.myAudioHubObject.PlaySound(this.gameIsLiveSFXS[i]);
			}
			else
			{
				GameManager.AudioSlinger.PlaySound(this.gameIsLiveSFXS[i]);
			}
		}
		if (this.magicFire)
		{
			this.generateMagicFire();
		}
		GameManager.StageManager.TheGameIsLive -= this.gameIsLive;
	}

	private void generateMagicFire()
	{
		this.magicFireWindow = UnityEngine.Random.Range(this.magicFireWindowMin, this.magicFireWindowMax);
		this.magicFireTimeStamp = Time.time;
		this.magicFireActive = true;
	}

	private void playerPausedGame()
	{
		if (this.magicFire)
		{
			this.freezeTimeStamp = Time.time;
			this.gameIsPaused = true;
		}
	}

	private void playerUnPausedGame()
	{
		if (this.magicFire)
		{
			this.freezeAddTime += Time.time - this.freezeTimeStamp;
			this.gameIsPaused = false;
		}
	}

	private void Awake()
	{
		this.myAudioHubObject = base.GetComponent<AudioHubObject>();
		if (this.myAudioHubObject.MyAudioHub == AUDIO_HUB.UNIVERSAL)
		{
			this.isUniversal = true;
		}
		GameManager.StageManager.TheGameIsStageing += this.gameIsStaging;
		GameManager.StageManager.TheGameIsLive += this.gameIsLive;
		GameManager.PauseManager.GamePaused += this.playerPausedGame;
		GameManager.PauseManager.GameUnPaused += this.playerUnPausedGame;
	}

	private void Update()
	{
		if (this.magicFireActive && !this.gameIsPaused && Time.time - this.freezeAddTime - this.magicFireTimeStamp >= this.magicFireWindow)
		{
			this.magicFireActive = false;
			this.freezeTimeStamp = 0f;
			this.freezeAddTime = 0f;
			if (this.isUniversal)
			{
				this.myAudioHubObject.PlaySound(this.magicFireSFXS[UnityEngine.Random.Range(0, this.magicFireSFXS.Length)]);
			}
			else
			{
				GameManager.AudioSlinger.PlaySound(this.magicFireSFXS[UnityEngine.Random.Range(0, this.magicFireSFXS.Length)]);
			}
			this.generateMagicFire();
		}
	}

	private void OnDestroy()
	{
		GameManager.PauseManager.GamePaused -= this.playerPausedGame;
		GameManager.PauseManager.GameUnPaused -= this.playerUnPausedGame;
	}

	[SerializeField]
	private AudioFileDefinition[] gameIsStagingSFXS = new AudioFileDefinition[0];

	[SerializeField]
	private AudioFileDefinition[] gameIsLiveSFXS = new AudioFileDefinition[0];

	[SerializeField]
	private bool magicFire;

	[SerializeField]
	private float magicFireWindowMin = 5f;

	[SerializeField]
	private float magicFireWindowMax = 30f;

	[SerializeField]
	private AudioFileDefinition[] magicFireSFXS = new AudioFileDefinition[0];

	private AudioHubObject myAudioHubObject;

	private bool magicFireActive;

	private bool gameIsPaused;

	private bool isUniversal;

	private float magicFireTimeStamp;

	private float magicFireWindow;

	private float freezeTimeStamp;

	private float freezeAddTime;
}
