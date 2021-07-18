using System;
using System.Collections.Generic;
using UnityEngine;

public class MotionSensorMenuBehaviour : MonoBehaviour
{
	private void rebuildMenu()
	{
		float y = 0f;
		if (this.currentActiveMotionSensors.Count > 0)
		{
			y = 4f + ((float)this.currentActiveMotionSensors.Count * 24f + (float)this.currentActiveMotionSensors.Count * 4f) + 4f;
			this.nonActiveCG.alpha = 0f;
		}
		else
		{
			y = 32f;
			this.nonActiveCG.alpha = 1f;
		}
		int num = 0;
		foreach (KeyValuePair<MotionSensorObject, MotionSensorMenuOptionObject> keyValuePair in this.currentActiveMotionSensors)
		{
			keyValuePair.Value.PutMe(num);
			num++;
		}
		Vector2 sizeDelta = new Vector2(this.myRT.sizeDelta.x, y);
		Vector2 anchoredPosition = new Vector2(this.myRT.anchoredPosition.x, y);
		if (this.myRT.anchoredPosition.y == -41f)
		{
			anchoredPosition.y = -41f;
		}
		this.myRT.sizeDelta = sizeDelta;
		this.myRT.anchoredPosition = anchoredPosition;
	}

	private void clearMotionSensorFromMenu(MotionSensorObject TheMotionSensor)
	{
		MotionSensorMenuOptionObject motionSensorMenuOptionObject;
		if (this.currentActiveMotionSensors.TryGetValue(TheMotionSensor, out motionSensorMenuOptionObject))
		{
			motionSensorMenuOptionObject.ClearMe();
			this.currentActiveMotionSensors.Remove(TheMotionSensor);
			this.motionSensorMenuOptionsPool.Push(motionSensorMenuOptionObject);
			this.rebuildMenu();
		}
	}

	private void addMotionSensorToMenu(MotionSensorObject TheMotionSensor)
	{
		if (TheMotionSensor.Placed)
		{
			if (!this.currentActiveMotionSensors.ContainsKey(TheMotionSensor))
			{
				MotionSensorMenuOptionObject motionSensorMenuOptionObject = this.motionSensorMenuOptionsPool.Pop();
				motionSensorMenuOptionObject.BuildMe(TheMotionSensor);
				this.currentActiveMotionSensors.Add(TheMotionSensor, motionSensorMenuOptionObject);
			}
			this.rebuildMenu();
		}
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.rebuildMenu();
		GameManager.ManagerSlinger.MotionSensorManager.EnteredPlacementMode += this.clearMotionSensorFromMenu;
		GameManager.ManagerSlinger.MotionSensorManager.MotionSensorPlaced += this.addMotionSensorToMenu;
		GameManager.ManagerSlinger.MotionSensorManager.MotionSensorWasReturned += this.clearMotionSensorFromMenu;
	}

	private void Awake()
	{
		this.myRT = base.GetComponent<RectTransform>();
		this.motionSensorMenuOptionsPool = new PooledStack<MotionSensorMenuOptionObject>(delegate()
		{
			MotionSensorMenuOptionObject component = UnityEngine.Object.Instantiate<GameObject>(this.motionSensorMenuObject, this.myRT).GetComponent<MotionSensorMenuOptionObject>();
			component.SoftBuild();
			return component;
		}, this.MOTION_SENSOR_MENU_POOL_COUNT);
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void OnDestroy()
	{
	}

	[SerializeField]
	private int MOTION_SENSOR_MENU_POOL_COUNT = 6;

	[SerializeField]
	private CanvasGroup nonActiveCG;

	[SerializeField]
	private GameObject motionSensorMenuObject;

	private const float OPT_SPACING = 4f;

	private const float BOT_SPACING = 4f;

	private RectTransform myRT;

	private PooledStack<MotionSensorMenuOptionObject> motionSensorMenuOptionsPool;

	private Dictionary<MotionSensorObject, MotionSensorMenuOptionObject> currentActiveMotionSensors = new Dictionary<MotionSensorObject, MotionSensorMenuOptionObject>(6);
}
