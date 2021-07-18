using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ClockHook : MonoBehaviour
{
	private void updateClock(string ClockValue)
	{
		this.myText.text = ClockValue;
	}

	private void Awake()
	{
		this.myText = base.GetComponent<Text>();
		GameManager.TimeKeeper.UpdateClockEvents.Event += this.updateClock;
	}

	private Text myText;
}
