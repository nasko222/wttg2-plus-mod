using System;
using System.Collections.Generic;
using UnityEngine;

public class EndingResponseManager : MonoBehaviour
{
	public void ProcessPlayerReponse(EndingStepDefinition TheStep)
	{
		this.currentEndingStep = TheStep;
		float num = 0f;
		float num2 = 45f * (float)(this.currentEndingStep.ResponseOptions.Count - 1);
		for (int i = 0; i < this.currentEndingStep.ResponseOptions.Count; i++)
		{
			EndingResponseObject endingResponseObject = this.endingResponseObjectPool.Pop();
			endingResponseObject.Build(this.currentEndingStep.ResponseOptions[i], i, num2, num);
			this.currentEndingResponses.Add(endingResponseObject);
			num += 1f;
			num2 -= 45f;
		}
		num = 1f * (float)this.currentEndingStep.ResponseOptions.Count;
		GameManager.TimeSlinger.FireTimer(num, delegate()
		{
			this.endController.PlayerSelectedOptionOne.Event += this.playerChoseOptionOne;
			this.endController.PlayerSelectedOptionTwo.Event += this.playerChoseOptionTwo;
			this.endController.AllowPlayerResponseSelection = true;
		}, 0);
	}

	private void playerChoseOptionOne()
	{
		this.endController.PlayerSelectedOptionOne.Event -= this.playerChoseOptionOne;
		this.endController.PlayerSelectedOptionTwo.Event -= this.playerChoseOptionTwo;
		this.currentEndingResponses[0].Dismiss(1.5f);
		this.currentEndingResponses[1].Dismiss(0f);
		this.clearCurrentResponses();
		GameManager.TimeSlinger.FireTimer(2f, delegate()
		{
			this.adamBehaviour.CallAniTrigger(this.currentEndingStep.ResponseOptions[0].ResponseAnimationTrigger);
			this.currentEndingStep = null;
		}, 0);
	}

	private void playerChoseOptionTwo()
	{
		this.endController.PlayerSelectedOptionOne.Event -= this.playerChoseOptionOne;
		this.endController.PlayerSelectedOptionTwo.Event -= this.playerChoseOptionTwo;
		this.currentEndingResponses[0].Dismiss(0f);
		this.currentEndingResponses[1].Dismiss(1.5f);
		this.clearCurrentResponses();
		GameManager.TimeSlinger.FireTimer(2f, delegate()
		{
			this.adamBehaviour.CallAniTrigger(this.currentEndingStep.ResponseOptions[1].ResponseAnimationTrigger);
			this.currentEndingStep = null;
		}, 0);
	}

	private void clearCurrentResponses()
	{
		for (int i = 0; i < this.currentEndingResponses.Count; i++)
		{
			this.endingResponseObjectPool.Push(this.currentEndingResponses[i]);
		}
		this.currentEndingResponses.Clear();
	}

	private void Awake()
	{
		this.endingResponseObjectPool = new PooledStack<EndingResponseObject>(delegate()
		{
			EndingResponseObject component = UnityEngine.Object.Instantiate<GameObject>(this.endingResponseObject, this.endingResponseObjectHolder).GetComponent<EndingResponseObject>();
			component.SoftBuild();
			return component;
		}, 2);
	}

	[SerializeField]
	private GameObject endingResponseObject;

	[SerializeField]
	private RectTransform endingResponseObjectHolder;

	[SerializeField]
	private EndController endController;

	[SerializeField]
	private AdamBehaviour adamBehaviour;

	private const float RESPONSE_SPACING = 45f;

	private const float RESPONSE_PRESENT_DELAY_TIME = 1f;

	private PooledStack<EndingResponseObject> endingResponseObjectPool;

	private List<EndingResponseObject> currentEndingResponses = new List<EndingResponseObject>(2);

	private EndingStepDefinition currentEndingStep;
}
