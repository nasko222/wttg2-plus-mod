using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CultLooker : MonoBehaviour
{
	public bool CheckForVisible
	{
		get
		{
			return this.checkForVisible;
		}
		set
		{
			this.checkForVisible = value;
		}
	}

	public bool CheckForNotVisible
	{
		get
		{
			return this.checkForNotVisible;
		}
		set
		{
			this.checkForNotVisible = value;
		}
	}

	public Vector3 TargetLocation
	{
		get
		{
			return this.storedTargetPOS;
		}
		set
		{
			this.storedTargetPOS = value;
		}
	}

	public bool IsTargetVisible(Vector3 TargetPOS)
	{
		Vector3 vector = this.myCamera.WorldToViewportPoint(TargetPOS);
		int num = 0;
		if (vector.z > 0f)
		{
			num++;
		}
		if (vector.x >= 0f && vector.x <= 1f)
		{
			num++;
		}
		if (vector.y >= 0f && vector.y <= 1f)
		{
			num++;
		}
		return num >= 2 && vector.z >= 0f;
	}

	private void Awake()
	{
		CultLooker.Ins = this;
		this.myCamera = base.GetComponent<Camera>();
	}

	private void Update()
	{
		if (this.checkForVisible)
		{
			if (this.IsTargetVisible(this.storedTargetPOS))
			{
				this.checkForVisible = false;
				this.VisibleActions.Execute();
			}
		}
		else if (this.checkForNotVisible && !this.IsTargetVisible(this.storedTargetPOS))
		{
			this.checkForNotVisible = false;
			this.NotVisibleActions.Execute();
		}
	}

	public static CultLooker Ins;

	public CustomEvent VisibleActions = new CustomEvent(2);

	public CustomEvent NotVisibleActions = new CustomEvent(2);

	private Camera myCamera;

	private Vector3 storedTargetPOS = Vector3.zero;

	private bool checkForVisible;

	private bool checkForNotVisible;
}
