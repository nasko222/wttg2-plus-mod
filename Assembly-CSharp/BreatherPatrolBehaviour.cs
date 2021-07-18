using System;
using UnityEngine;
using UnityEngine.AI;

public class BreatherPatrolBehaviour : MonoBehaviour
{
	public void PatrolSpawn()
	{
		BreatherBehaviour.Ins.SoftSpawn();
		base.transform.position = this.patrolSpawnPOS;
		base.transform.rotation = Quaternion.Euler(this.patrolSpawnROT);
		this.PatrolTo(this.patrolPoints[0]);
	}

	public void KillPatrol()
	{
		this.myNavMeshAgent.enabled = false;
		this.destInProgress = false;
		this.hadPathPreviousFrame = false;
		this.myAC.SetFloat("walking", 0f);
	}

	public void LeftDoorPatrol()
	{
		this.PatrolTo(this.patrolPoints[1]);
	}

	public void PatrolTo(PatrolPointDefinition Point)
	{
		this.currentPatrolPoint = Point;
		this.destInProgress = true;
		this.myNavMeshAgent.enabled = true;
		this.myNavMeshAgent.speed = 1f;
		this.myNavMeshAgent.angularSpeed = 240f;
		this.myNavMeshAgent.acceleration = 2f;
		this.myNavMeshAgent.SetDestination(Point.Position);
	}

	private void reachedEndPoint()
	{
		this.destInProgress = false;
		this.myNavMeshAgent.enabled = false;
		if (this.currentPatrolPoint != null)
		{
			this.currentPatrolPoint.InvokeEvents();
		}
		GameManager.TweenSlinger.FireDOSTweenLiner(this.myNavMeshAgent.velocity.magnitude, 0f, 1f, delegate(float value)
		{
			this.myAC.SetFloat("walking", value);
		});
	}

	private void Awake()
	{
		BreatherPatrolBehaviour.Ins = this;
		this.myAC = base.GetComponent<Animator>();
		this.myNavMeshAgent = base.GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (this.destInProgress && this.myNavMeshAgent.enabled)
		{
			if (this.myNavMeshAgent.hasPath)
			{
				this.myAC.SetFloat("walking", this.myNavMeshAgent.velocity.magnitude);
				this.hadPathPreviousFrame = true;
			}
			else if (this.hadPathPreviousFrame)
			{
				this.hadPathPreviousFrame = false;
				this.reachedEndPoint();
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		for (int i = 0; i < this.patrolPoints.Length; i++)
		{
			Gizmos.DrawWireCube(this.patrolPoints[i].Position, new Vector3(0.2f, 0.2f, 0.2f));
		}
	}

	public static BreatherPatrolBehaviour Ins;

	[SerializeField]
	private Vector3 patrolSpawnPOS = Vector3.zero;

	[SerializeField]
	private Vector3 patrolSpawnROT = Vector3.zero;

	[SerializeField]
	private PatrolPointDefinition[] patrolPoints = new PatrolPointDefinition[0];

	private Animator myAC;

	private NavMeshAgent myNavMeshAgent;

	private PatrolPointDefinition currentPatrolPoint;

	private bool destInProgress;

	private bool hadPathPreviousFrame;
}
