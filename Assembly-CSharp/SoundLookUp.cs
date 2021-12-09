using System;
using UnityEngine;

public class SoundLookUp : MonoBehaviour
{
	private void Awake()
	{
		LookUp.SoundLookUp = this;
		AssetBundleManager.ProceedLoadingAFD();
		LookUp.SoundLookUp.JumpHit1.MyAudioLayer = AUDIO_LAYER.GAME_OVER;
	}

	public AudioFileDefinition MouseClick;

	public AudioFileDefinition[] KeyboardSounds;

	public AudioFileDefinition KeyboardSpaceReturnSound;

	public AudioFileDefinition CantBuyItem;

	public AudioFileDefinition MenuHover;

	public AudioFileDefinition MenuClick;

	public AudioFileDefinition ItemPickUp1;

	public AudioFileDefinition ItemPickUp2;

	public AudioFileDefinition ItemPutDown1;

	public AudioFileDefinition KeyFound;

	public AudioFileDefinition HolyClick;

	public AudioFileDefinition PuzzleGoodClick;

	public AudioFileDefinition PuzzleFailClick;

	public AudioFileDefinition PuzzleSolved;

	public AudioFileDefinition LoudDoorBang;

	public AudioFileDefinition SlamOpenDoor;

	public AudioFileDefinition SlamOpenDoor2;

	public AudioFileDefinition DoorKnobSFX;

	public AudioFileDefinition FlashBang;

	public AudioFileDefinition EarRing;

	public AudioFileDefinition BodyHit;

	public AudioFileDefinition FloorHit;

	public AudioFileDefinition GameOver;

	public AudioFileDefinition GameOverHit;

	public AudioFileDefinition HeadHit;

	public AudioFileDefinition FleshHit1;

	public AudioFileDefinition KnifeStab1;

	public AudioFileDefinition KnifeStab2;

	public AudioFileDefinition KnifeStab3;

	public AudioFileDefinition JumpHit1;

	public AudioFileDefinition JumpHit2;

	public AudioFileDefinition JumpHit3;

	public AudioFileDefinition JumpHit4;

	public AudioFileDefinition JumpHit5;

	public AudioFileDefinition JumpHit6;

	public AudioFileDefinition JumpHit7;

	public AudioFileDefinition vacationRinging;

	public AudioFileDefinition breatherLaugh;

	public AudioFileDefinition oneless;

	public AudioFileDefinition theHall;

	public AudioFileDefinition isEvil;

	public AudioFileDefinition babyTime;

	public AudioFileDefinition creepy;

	public AudioFileDefinition dollMusics;

	public AudioFileDefinition numberStationScare;
}
