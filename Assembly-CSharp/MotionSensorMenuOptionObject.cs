using System;
using UnityEngine;
using UnityEngine.UI;

public class MotionSensorMenuOptionObject : MonoBehaviour
{
	public void ClearMe()
	{
		this.myRT.anchoredPosition = this.spawnPOS;
		this.locationText.text = string.Empty;
		this.myMotionSensorObject.IWasTripped -= this.motionTriggerWasTripped;
		this.myMotionSensorObject = null;
	}

	public void BuildMe(MotionSensorObject TheMotionSensor)
	{
		this.locationText.text = MagicSlinger.GetFriendlyLocationName(TheMotionSensor.Location);
		this.myMotionSensorObject = TheMotionSensor;
		this.myMotionSensorObject.IWasTripped += this.motionTriggerWasTripped;
	}

	public void SoftBuild()
	{
		this.myRT = base.GetComponent<RectTransform>();
		this.myRT.anchoredPosition = this.spawnPOS;
	}

	public void PutMe(int SetIndex)
	{
		float y = -(4f + ((float)SetIndex * 4f + (float)SetIndex * 24f));
		this.myRT.anchoredPosition = new Vector2(4f, y);
	}

	private void motionTriggerWasTripped(MotionSensorObject TheMotionSensor)
	{
		if (!this.triggerWarningActive)
		{
			this.triggerWarningTimeStamp = Time.time;
			this.triggerWarningActive = true;
		}
	}

	private void Update()
	{
		if (this.triggerWarningActive)
		{
			this.activeCG.alpha = Mathf.Lerp(1f, 0f, (Time.time - this.triggerWarningTimeStamp) / this.triggerWarningDisplayTime);
			if (Time.time - this.triggerWarningTimeStamp >= this.triggerWarningDisplayTime)
			{
				this.triggerWarningActive = false;
			}
		}
	}

	[SerializeField]
	private Text locationText;

	[SerializeField]
	private Image stateImage;

	[SerializeField]
	private CanvasGroup activeCG;

	[SerializeField]
	private float triggerWarningDisplayTime = 0.2f;

	private const float OPT_SPACING = 4f;

	private const float MENU_OPT_X = 4f;

	private RectTransform myRT;

	private Vector2 spawnPOS = new Vector2(0f, 24f);

	private MotionSensorObject myMotionSensorObject;

	private bool triggerWarningActive;

	private float triggerWarningTimeStamp;
}
