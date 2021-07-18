using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClickSoundScrub : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.MouseClick);
	}
}
