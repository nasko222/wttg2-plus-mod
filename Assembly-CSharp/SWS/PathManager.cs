using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SWS
{
	public class PathManager : MonoBehaviour
	{
		private void Awake()
		{
			WaypointManager.AddPath(base.gameObject);
		}

		public void Create(Transform parent = null)
		{
			if (parent == null)
			{
				parent = base.transform;
			}
			List<Transform> list = new List<Transform>();
			IEnumerator enumerator = parent.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform item = (Transform)obj;
					list.Add(item);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.Create(list.ToArray(), false);
		}

		public virtual void Create(Transform[] waypoints, bool makeChildren = false)
		{
			if (waypoints.Length < 2)
			{
				Debug.LogWarning("Not enough waypoints placed - minimum is 2. Cancelling.");
				return;
			}
			if (makeChildren)
			{
				for (int i = 0; i < waypoints.Length; i++)
				{
					waypoints[i].parent = base.transform;
				}
			}
			this.waypoints = waypoints;
		}

		private void OnDrawGizmos()
		{
			if (this.waypoints.Length <= 0)
			{
				return;
			}
			Vector3[] pathPoints = this.GetPathPoints(false);
			Vector3 vector = pathPoints[0];
			Vector3 vector2 = pathPoints[pathPoints.Length - 1];
			Gizmos.color = this.color1;
			Gizmos.DrawWireCube(vector, this.size * this.GetHandleSize(vector) * 1.5f);
			Gizmos.DrawWireCube(vector2, this.size * this.GetHandleSize(vector2) * 1.5f);
			Gizmos.color = this.color2;
			for (int i = 1; i < pathPoints.Length - 1; i++)
			{
				Gizmos.DrawWireSphere(pathPoints[i], this.radius * this.GetHandleSize(pathPoints[i]));
			}
			if (this.drawCurved && pathPoints.Length >= 2)
			{
				WaypointManager.DrawCurved(pathPoints);
			}
			else
			{
				WaypointManager.DrawStraight(pathPoints);
			}
		}

		public virtual float GetHandleSize(Vector3 pos)
		{
			return 1f;
		}

		public virtual Vector3[] GetPathPoints(bool local = false)
		{
			Vector3[] array = new Vector3[this.waypoints.Length];
			if (local)
			{
				for (int i = 0; i < this.waypoints.Length; i++)
				{
					array[i] = this.waypoints[i].localPosition;
				}
			}
			else
			{
				for (int j = 0; j < this.waypoints.Length; j++)
				{
					array[j] = this.waypoints[j].position;
				}
			}
			return array;
		}

		public virtual Transform GetWaypoint(int index)
		{
			return this.waypoints[index];
		}

		public virtual int GetWaypointIndex(int point)
		{
			return point;
		}

		public virtual int GetWaypointCount()
		{
			return this.waypoints.Length;
		}

		public Transform[] waypoints = new Transform[0];

		public bool drawCurved = true;

		public bool drawDirection;

		public Color color1 = new Color(1f, 0f, 1f, 0.5f);

		public Color color2 = new Color(1f, 0.921568632f, 0.0156862754f, 0.5f);

		public Vector3 size = new Vector3(0.7f, 0.7f, 0.7f);

		public float radius = 0.4f;

		public bool skipCustomNames = true;

		public GameObject replaceObject;
	}
}
