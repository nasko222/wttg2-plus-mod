using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace SWS
{
	public class WaypointManager : MonoBehaviour
	{
		private void Awake()
		{
			DOTween.Init(null, null, null);
		}

		public static void AddPath(GameObject path)
		{
			string text = path.name;
			if (text.Contains("(Clone)"))
			{
				text = text.Replace("(Clone)", string.Empty);
			}
			PathManager componentInChildren = path.GetComponentInChildren<PathManager>();
			if (componentInChildren == null)
			{
				Debug.LogWarning("Called AddPath() but GameObject " + text + " has no PathManager attached.");
				return;
			}
			WaypointManager.CleanUp();
			if (WaypointManager.Paths.ContainsKey(text))
			{
				int num = 1;
				while (WaypointManager.Paths.ContainsKey(text + "#" + num))
				{
					num++;
				}
				text = text + "#" + num;
				Debug.Log(string.Concat(new string[]
				{
					"Renamed ",
					path.name,
					" to ",
					text,
					" because a path with the same name was found."
				}));
			}
			path.name = text;
			WaypointManager.Paths.Add(text, componentInChildren);
		}

		public static void CleanUp()
		{
			string[] array = (from p in WaypointManager.Paths
			where p.Value == null
			select p.Key).ToArray<string>();
			for (int i = 0; i < array.Length; i++)
			{
				WaypointManager.Paths.Remove(array[i]);
			}
		}

		private void OnDestroy()
		{
			WaypointManager.Paths.Clear();
		}

		public static void DrawStraight(Vector3[] waypoints)
		{
			for (int i = 0; i < waypoints.Length - 1; i++)
			{
				Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
			}
		}

		public static void DrawCurved(Vector3[] pathPoints)
		{
			pathPoints = WaypointManager.GetCurved(pathPoints);
			Vector3 to = pathPoints[0];
			for (int i = 1; i < pathPoints.Length; i++)
			{
				Vector3 vector = pathPoints[i];
				Gizmos.DrawLine(vector, to);
				to = vector;
			}
		}

		public static Vector3[] GetCurved(Vector3[] waypoints)
		{
			Vector3[] array = new Vector3[waypoints.Length + 2];
			waypoints.CopyTo(array, 1);
			array[0] = waypoints[1];
			array[array.Length - 1] = array[array.Length - 2];
			int num = array.Length * 10;
			Vector3[] array2 = new Vector3[num + 1];
			for (int i = 0; i <= num; i++)
			{
				float t = (float)i / (float)num;
				Vector3 point = WaypointManager.GetPoint(array, t);
				array2[i] = point;
			}
			return array2;
		}

		public static Vector3 GetPoint(Vector3[] gizmoPoints, float t)
		{
			int num = gizmoPoints.Length - 3;
			int num2 = (int)Mathf.Floor(t * (float)num);
			int num3 = num - 1;
			if (num3 > num2)
			{
				num3 = num2;
			}
			float num4 = t * (float)num - (float)num3;
			Vector3 a = gizmoPoints[num3];
			Vector3 a2 = gizmoPoints[num3 + 1];
			Vector3 vector = gizmoPoints[num3 + 2];
			Vector3 b = gizmoPoints[num3 + 3];
			return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num4 * num4 * num4) + (2f * a - 5f * a2 + 4f * vector - b) * (num4 * num4) + (-a + vector) * num4 + 2f * a2);
		}

		public static float GetPathLength(Vector3[] waypoints)
		{
			float num = 0f;
			for (int i = 0; i < waypoints.Length - 1; i++)
			{
				num += Vector3.Distance(waypoints[i], waypoints[i + 1]);
			}
			return num;
		}

		public static List<Vector3> SmoothCurve(List<Vector3> pathToCurve, int interpolations)
		{
			if (interpolations < 1)
			{
				interpolations = 1;
			}
			int count = pathToCurve.Count;
			int num = count * Mathf.RoundToInt((float)interpolations) - 1;
			List<Vector3> list = new List<Vector3>(num);
			for (int i = 0; i < num + 1; i++)
			{
				float num2 = Mathf.InverseLerp(0f, (float)num, (float)i);
				List<Vector3> list2 = new List<Vector3>(pathToCurve);
				for (int j = count - 1; j > 0; j--)
				{
					for (int k = 0; k < j; k++)
					{
						list2[k] = (1f - num2) * list2[k] + num2 * list2[k + 1];
					}
				}
				list.Add(list2[0]);
			}
			return list;
		}

		public KeyCode placementKey = KeyCode.P;

		public KeyCode viewPlacementKey = KeyCode.C;

		public static readonly Dictionary<string, PathManager> Paths = new Dictionary<string, PathManager>();
	}
}
