using System;
using SWS;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(splineMove))]
public class HitmanProxyBehaviour : MonoBehaviour
{
	public void TriggerPath()
	{
		int num = UnityEngine.Random.Range(0, this.paths.Length);
		this.mySplineMove.PathIsCompleted += this.triggerPathCompleteEvents;
		this.mySplineMove.pathContainer = this.paths[num].ThePath;
		this.mySplineMove.SetPath(this.paths[num].ThePath);
		this.mySplineMove.speed = this.paths[num].PathTime;
		this.mySplineMove.StartMove();
	}

	private void triggerPathCompleteEvents()
	{
		this.mySplineMove.PathIsCompleted -= this.triggerPathCompleteEvents;
		base.transform.position = Vector3.zero;
		this.PathCompletedEvents.Invoke();
	}

	private void Awake()
	{
		this.mySplineMove = base.GetComponent<splineMove>();
	}

	public UnityEvent PathCompletedEvents = new UnityEvent();

	[SerializeField]
	private PathManagerDefinition[] paths = new PathManagerDefinition[0];

	private splineMove mySplineMove;
}
