using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SWS
{
	[RequireComponent(typeof(LineRenderer))]
	public class PathRenderer : MonoBehaviour
	{
		private void Start()
		{
			this.line = base.GetComponent<LineRenderer>();
			this.path = base.GetComponent<PathManager>();
			if (this.path)
			{
				base.StartCoroutine("StartRenderer");
			}
		}

		private IEnumerator StartRenderer()
		{
			this.Render();
			if (!this.onUpdate)
			{
				yield break;
			}
			for (;;)
			{
				yield return null;
				this.Render();
			}
		}

		private void Render()
		{
			this.spacing = Mathf.Clamp01(this.spacing);
			if (this.spacing == 0f)
			{
				this.spacing = 0.05f;
			}
			List<Vector3> list = new List<Vector3>();
			list.AddRange(this.path.GetPathPoints(false));
			if (this.path.drawCurved)
			{
				list.Insert(0, list[0]);
				list.Add(list[list.Count - 1]);
				this.points = list.ToArray();
				this.DrawCurved();
			}
			else
			{
				this.points = list.ToArray();
				this.DrawLinear();
			}
		}

		private void DrawCurved()
		{
			int num = Mathf.RoundToInt(1f / this.spacing) + 1;
			this.line.positionCount = num;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				this.line.SetPosition(i, WaypointManager.GetPoint(this.points, num2));
				num2 += this.spacing;
			}
		}

		private void DrawLinear()
		{
			this.line.positionCount = this.points.Length;
			float num = 0f;
			for (int i = 0; i < this.points.Length; i++)
			{
				this.line.SetPosition(i, this.points[i]);
				num += this.spacing;
			}
		}

		public bool onUpdate;

		public float spacing = 0.05f;

		private PathManager path;

		private LineRenderer line;

		private Vector3[] points;
	}
}
