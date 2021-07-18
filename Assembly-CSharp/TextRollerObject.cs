using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextRollerObject : MonoBehaviour
{
	public void ProcessLinerRequest(float FromValue, float ToValue, float SetDuration)
	{
		if (this.curTweens.Count > 0)
		{
			this.AddLinerTweenToQue(FromValue, ToValue, SetDuration);
		}
		else
		{
			this.FireLiner(FromValue, ToValue, SetDuration);
		}
	}

	public void AddLinerTweenToQue(float fromValue, float toValue, float setDuration)
	{
		this.curTweens.Add(new TextRollerObject.textTween(false, fromValue, toValue, setDuration, 0f, 0f));
	}

	public void AddStepTweenToQue(float fromValue, float toValue, float delayPerUnit, float maxDuration)
	{
		this.curTweens.Add(new TextRollerObject.textTween(true, fromValue, toValue, 0f, delayPerUnit, maxDuration));
	}

	public void FireLiner(float fromValue, float toValue, float setDuration)
	{
		this.toValue = toValue;
		GameManager.TweenSlinger.FireDOSTweenLiner(fromValue, toValue, setDuration, new Action<float>(this.UpdateText));
	}

	public void FireLiner(float fromValue, float toValue, float setDuration, string setID)
	{
		this.toValue = toValue;
		GameManager.TweenSlinger.FireDOSTweenLiner(fromValue, toValue, setDuration, new Action<float>(this.UpdateText));
	}

	public void FireStep(float fromValue, float toValue, float delayPerUnit, float maxDuration)
	{
		this.toValue = toValue;
		GameManager.TweenSlinger.FireDOSTweenStep(fromValue, toValue, delayPerUnit, maxDuration, new Action<float>(this.UpdateText));
	}

	public void FireStep(float fromValue, float toValue, float delayPerUnit, float maxDuration, string setID)
	{
		this.toValue = toValue;
		GameManager.TweenSlinger.FireDOSTweenStep(fromValue, toValue, delayPerUnit, maxDuration, new Action<float>(this.UpdateText));
	}

	private void UpdateText(float newValue)
	{
		if (this.myText != null)
		{
			this.myText.text = newValue.ToString();
			if (newValue == this.toValue && this.curTweens.Count > 0)
			{
				if (this.curTweens[0].isStep)
				{
					this.toValue = this.curTweens[0].toValue;
					GameManager.TweenSlinger.FireDOSTweenStep(this.curTweens[0].fromValue, this.curTweens[0].toValue, this.curTweens[0].delayPerUnit, this.curTweens[0].maxDuration, new Action<float>(this.UpdateText));
				}
				else
				{
					this.toValue = this.curTweens[0].toValue;
					GameManager.TweenSlinger.FireDOSTweenLiner(this.curTweens[0].fromValue, this.curTweens[0].toValue, this.curTweens[0].duration, new Action<float>(this.UpdateText));
				}
				this.curTweens.RemoveAt(0);
			}
		}
	}

	private void Awake()
	{
		this.myText = base.GetComponent<Text>();
	}

	private float toValue;

	private List<TextRollerObject.textTween> curTweens = new List<TextRollerObject.textTween>();

	private Text myText;

	public struct textTween
	{
		public textTween(bool setStep, float setFromValue, float setToValue, float setDuration, float setDelayPerUnit, float setMaxDuration)
		{
			this.isStep = setStep;
			this.fromValue = setFromValue;
			this.toValue = setToValue;
			this.duration = setDuration;
			this.delayPerUnit = setDelayPerUnit;
			this.maxDuration = setMaxDuration;
		}

		public bool isStep;

		public float fromValue;

		public float toValue;

		public float duration;

		public float delayPerUnit;

		public float maxDuration;
	}
}
