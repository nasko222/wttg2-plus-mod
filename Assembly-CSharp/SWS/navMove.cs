using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace SWS
{
	[RequireComponent(typeof(NavMeshAgent))]
	[AddComponentMenu("Simple Waypoint System/navMove")]
	public class navMove : MonoBehaviour
	{
		private void Awake()
		{
			this.agent = base.GetComponent<NavMeshAgent>();
		}

		private void Start()
		{
			if (this.onStart)
			{
				this.StartMove();
			}
		}

		public void StartMove()
		{
			if (this.pathContainer == null)
			{
				Debug.LogWarning(base.gameObject.name + " has no path! Please set Path Container.");
				return;
			}
			this.waypoints = new Transform[this.pathContainer.waypoints.Length];
			Array.Copy(this.pathContainer.waypoints, this.waypoints, this.pathContainer.waypoints.Length);
			this.startPoint = Mathf.Clamp(this.startPoint, 0, this.waypoints.Length - 1);
			int num = this.startPoint;
			if (this.reverse)
			{
				Array.Reverse(this.waypoints);
				num = this.waypoints.Length - 1 - num;
			}
			this.currentPoint = num;
			for (int i = this.events.Count; i <= this.waypoints.Length - 1; i++)
			{
				this.events.Add(new UnityEvent());
			}
			this.Stop();
			base.StartCoroutine(this.Move());
		}

		private IEnumerator Move()
		{
			this.agent.isStopped = false;
			this.agent.updateRotation = this.updateRotation;
			if (this.moveToPath)
			{
				this.agent.SetDestination(this.waypoints[this.currentPoint].position);
				yield return base.StartCoroutine(this.WaitForDestination());
			}
			if (this.loopType == navMove.LoopType.random)
			{
				base.StartCoroutine(this.ReachedEnd());
				yield break;
			}
			if (this.moveToPath)
			{
				base.StartCoroutine(this.NextWaypoint());
			}
			else
			{
				this.GoToWaypoint(this.startPoint);
			}
			this.moveToPath = false;
			yield break;
		}

		private IEnumerator NextWaypoint()
		{
			this.OnWaypointChange(this.currentPoint);
			yield return new WaitForEndOfFrame();
			while (this.waiting)
			{
				yield return null;
			}
			Transform next = null;
			if (this.loopType == navMove.LoopType.pingPong && this.repeat)
			{
				this.currentPoint--;
			}
			else if (this.loopType == navMove.LoopType.random)
			{
				this.rndIndex++;
				this.currentPoint = int.Parse(this.waypoints[this.rndIndex].name.Replace("Waypoint ", string.Empty));
				next = this.waypoints[this.rndIndex];
			}
			else
			{
				this.currentPoint++;
			}
			this.currentPoint = Mathf.Clamp(this.currentPoint, 0, this.waypoints.Length - 1);
			if (next == null)
			{
				next = this.waypoints[this.currentPoint];
			}
			this.agent.SetDestination(next.position);
			yield return base.StartCoroutine(this.WaitForDestination());
			if ((this.loopType != navMove.LoopType.random && this.currentPoint == this.waypoints.Length - 1) || this.rndIndex == this.waypoints.Length - 1 || (this.repeat && this.currentPoint == 0))
			{
				base.StartCoroutine(this.ReachedEnd());
			}
			else
			{
				base.StartCoroutine(this.NextWaypoint());
			}
			yield break;
		}

		private IEnumerator WaitForDestination()
		{
			yield return new WaitForEndOfFrame();
			while (this.agent.pathPending)
			{
				yield return null;
			}
			yield return new WaitForEndOfFrame();
			float remain = this.agent.remainingDistance;
			while (remain == float.PositiveInfinity || remain - this.agent.stoppingDistance > 1.401298E-45f || this.agent.pathStatus != NavMeshPathStatus.PathComplete)
			{
				remain = this.agent.remainingDistance;
				yield return null;
			}
			yield break;
		}

		private void OnWaypointChange(int index)
		{
			if (this.reverse)
			{
				index = this.waypoints.Length - 1 - index;
			}
			if (this.events == null || this.events.Count - 1 < index || this.events[index] == null)
			{
				return;
			}
			this.events[index].Invoke();
		}

		private IEnumerator ReachedEnd()
		{
			switch (this.loopType)
			{
			case navMove.LoopType.none:
				this.OnWaypointChange(this.waypoints.Length - 1);
				yield break;
			case navMove.LoopType.loop:
				this.OnWaypointChange(this.waypoints.Length - 1);
				if (this.closeLoop)
				{
					this.agent.SetDestination(this.waypoints[0].position);
					yield return base.StartCoroutine(this.WaitForDestination());
				}
				else
				{
					this.agent.Warp(this.waypoints[0].position);
				}
				this.currentPoint = 0;
				break;
			case navMove.LoopType.pingPong:
				this.repeat = !this.repeat;
				break;
			case navMove.LoopType.random:
				this.RandomizeWaypoints();
				break;
			}
			base.StartCoroutine(this.NextWaypoint());
			yield break;
		}

		private void RandomizeWaypoints()
		{
			Array.Copy(this.pathContainer.waypoints, this.waypoints, this.pathContainer.waypoints.Length);
			int i = this.waypoints.Length;
			while (i > 1)
			{
				int num = this.rand.Next(i--);
				Transform transform = this.waypoints[i];
				this.waypoints[i] = this.waypoints[num];
				this.waypoints[num] = transform;
			}
			Transform y = this.pathContainer.waypoints[this.currentPoint];
			for (int j = 0; j < this.waypoints.Length; j++)
			{
				if (this.waypoints[j] == y)
				{
					Transform transform2 = this.waypoints[0];
					this.waypoints[0] = this.waypoints[j];
					this.waypoints[j] = transform2;
					break;
				}
			}
			this.rndIndex = 0;
		}

		public void GoToWaypoint(int index)
		{
			if (this.reverse)
			{
				index = this.waypoints.Length - 1 - index;
			}
			this.Stop();
			this.currentPoint = index;
			this.agent.Warp(this.waypoints[index].position);
			base.StartCoroutine(this.NextWaypoint());
		}

		public void Pause(float seconds = 0f)
		{
			base.StopCoroutine(this.Wait(0f));
			this.waiting = true;
			this.agent.isStopped = true;
			if (seconds > 0f)
			{
				base.StartCoroutine(this.Wait(seconds));
			}
		}

		private IEnumerator Wait(float secs = 0f)
		{
			yield return new WaitForSeconds(secs);
			this.Resume();
			yield break;
		}

		public void Resume()
		{
			base.StopCoroutine(this.Wait(0f));
			this.waiting = false;
			this.agent.isStopped = false;
		}

		public void Reverse()
		{
			this.reverse = !this.reverse;
			if (this.reverse)
			{
				this.startPoint = this.currentPoint - 1;
			}
			else
			{
				Array.Reverse(this.waypoints);
				this.startPoint = this.waypoints.Length - this.currentPoint;
			}
			this.moveToPath = true;
			this.StartMove();
		}

		public void SetPath(PathManager newPath)
		{
			this.Stop();
			this.pathContainer = newPath;
			this.StartMove();
		}

		public void Stop()
		{
			base.StopAllCoroutines();
			if (this.agent.enabled)
			{
				this.agent.isStopped = true;
			}
		}

		public void ResetToStart()
		{
			this.Stop();
			this.currentPoint = 0;
			if (this.pathContainer)
			{
				this.agent.Warp(this.pathContainer.waypoints[this.currentPoint].position);
			}
		}

		public void ChangeSpeed(float value)
		{
			this.agent.speed = value;
		}

		public PathManager pathContainer;

		public bool onStart;

		public bool moveToPath;

		public bool reverse;

		public int startPoint;

		[HideInInspector]
		public int currentPoint;

		public bool closeLoop;

		public bool updateRotation = true;

		[HideInInspector]
		public List<UnityEvent> events = new List<UnityEvent>();

		public navMove.LoopType loopType;

		[HideInInspector]
		public Transform[] waypoints;

		private bool repeat;

		private NavMeshAgent agent;

		private System.Random rand = new System.Random();

		private int rndIndex;

		private bool waiting;

		public enum LoopType
		{
			none,
			loop,
			pingPong,
			random
		}
	}
}
