using System;
using UnityEngine;

public class PlayerAudioHubHook : MonoBehaviour
{
	private void gameIsLive()
	{
		for (int i = 0; i < this.gameLiveSFXS.Length; i++)
		{
			GameManager.AudioSlinger.PlaySound(this.gameLiveSFXS[i]);
		}
		GameManager.StageManager.TheGameIsLive -= this.gameIsLive;
	}

	private void Awake()
	{
		PlayerAudioHubHook.Ins = this;
		GameManager.StageManager.TheGameIsLive += this.gameIsLive;
	}

	public static PlayerAudioHubHook Ins;

	[SerializeField]
	private AudioFileDefinition[] gameLiveSFXS = new AudioFileDefinition[0];
}
