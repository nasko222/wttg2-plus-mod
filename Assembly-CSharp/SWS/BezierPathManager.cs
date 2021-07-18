using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SWS
{
	public class BezierPathManager : PathManager
	{
		private void Awake()
		{
			WaypointManager.AddPath(base.gameObject);
			if (this.bPoints == null || this.bPoints.Count == 0)
			{
				return;
			}
			this.CalculatePath();
		}

		public override void Create(Transform[] waypoints, bool makeChildren = false)
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
			this.bPoints.Clear();
			for (int j = 0; j < waypoints.Length; j++)
			{
				BezierPoint bezierPoint = new BezierPoint();
				bezierPoint.wp = waypoints[j];
				bezierPoint.cp = new Transform[2];
				bezierPoint.cp[0] = bezierPoint.wp.GetChild(0);
				bezierPoint.cp[1] = bezierPoint.wp.GetChild(1);
				this.bPoints.Add(bezierPoint);
			}
			this.CalculatePath();
		}

		private void OnDrawGizmos()
		{
			if (this.bPoints.Count <= 0)
			{
				return;
			}
			Vector3 position = this.bPoints[0].wp.position;
			Vector3 position2 = this.bPoints[this.bPoints.Count - 1].wp.position;
			Gizmos.color = this.color1;
			Gizmos.DrawWireCube(position, this.size * this.GetHandleSize(position) * 1.5f);
			Gizmos.DrawWireCube(position2, this.size * this.GetHandleSize(position2) * 1.5f);
			Gizmos.color = this.color2;
			for (int i = 1; i < this.bPoints.Count - 1; i++)
			{
				Gizmos.DrawWireSphere(this.bPoints[i].wp.position, this.radius * this.GetHandleSize(this.bPoints[i].wp.position));
			}
			if (this.drawCurved && this.bPoints.Count >= 2)
			{
				WaypointManager.DrawCurved(this.pathPoints);
			}
			else
			{
				WaypointManager.DrawStraight(this.pathPoints);
			}
		}

		public override Vector3[] GetPathPoints(bool local = false)
		{
			if (local)
			{
				Vector3[] array = new Vector3[this.pathPoints.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = base.transform.InverseTransformPoint(this.pathPoints[i]);
				}
				return array;
			}
			return this.pathPoints;
		}

		public override int GetWaypointCount()
		{
			return this.bPoints.Count;
		}

		public override Transform GetWaypoint(int index)
		{
			return this.bPoints[index].wp;
		}

		public override int GetWaypointIndex(int point)
		{
			int result = -1;
			int num = 0;
			int num2 = 10;
			for (int i = 0; i < this.segmentDetail.Count; i++)
			{
				if (point == num)
				{
					result = i;
					break;
				}
				if (this.customDetail)
				{
					num += Mathf.CeilToInt(this.segmentDetail[i] * (float)num2);
				}
				else
				{
					num += Mathf.CeilToInt(this.pathDetail * (float)num2);
				}
			}
			return result;
		}

		public void CalculatePath()
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < this.bPoints.Count - 1; i++)
			{
				BezierPoint bezierPoint = this.bPoints[i];
				float detail = this.pathDetail;
				if (this.customDetail)
				{
					detail = this.segmentDetail[i];
				}
				list.AddRange(this.GetPoints(bezierPoint.wp.position, bezierPoint.cp[1].position, this.bPoints[i + 1].cp[0].position, this.bPoints[i + 1].wp.position, detail));
			}
			this.pathPoints = list.Distinct<Vector3>().ToArray<Vector3>();
		}

		private List<Vector3> GetPoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float detail)
		{
			List<Vector3> list = new List<Vector3>();
			float num = detail * 10f;
			int num2 = 0;
			while ((float)num2 <= num)
			{
				float num3 = (float)num2 / num;
				float d = 1f - num3;
				Vector3 vector = Vector3.zero;
				vector += p0 * d * d * d;
				vector += p1 * num3 * 3f * d * d;
				vector += p2 * 3f * num3 * num3 * d;
				vector += p3 * num3 * num3 * num3;
				list.Add(vector);
				num2++;
			}
			return list;
		}

		public Vector3[] pathPoints = new Vector3[0];

		public List<BezierPoint> bPoints = new List<BezierPoint>();

		public bool showHandles = true;

		public bool connectHandles = true;

		public Color color3 = new Color(0.423529416f, 0.5921569f, 1f, 1f);

		public float pathDetail = 1f;

		public bool customDetail;

		public List<float> segmentDetail = new List<float>();
	}
}
