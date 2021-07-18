using System;
using UnityEngine;

namespace JBrothers.PreIntegratedSkinShader2.Demo
{
	[AddComponentMenu("Camera-Control/Configurable Mouse Orbit")]
	public class ConfigurableMouseOrbit : MonoBehaviour
	{
		private void Start()
		{
			if (!this.target)
			{
				return;
			}
			Vector3 eulerAngles = base.transform.eulerAngles;
			this.x = eulerAngles.y;
			this.y = eulerAngles.x;
			if (base.GetComponent<Rigidbody>())
			{
				base.GetComponent<Rigidbody>().freezeRotation = true;
			}
		}

		private bool CursorLocked
		{
			get
			{
				return Screen.lockCursor;
			}
			set
			{
				Screen.lockCursor = value;
			}
		}

		private bool isMouseOverGUI()
		{
			return GUIUtility.hotControl != 0;
		}

		private void Update()
		{
			if (!this.target)
			{
				return;
			}
			if (this.mouseButton == ConfigurableMouseOrbit.MouseButton.None || (Input.GetMouseButton((int)this.mouseButton) && !this.isMouseOverGUI()))
			{
				if (!this.CursorLocked && this.lockCursor)
				{
					this.CursorLocked = true;
				}
				this.x += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
				this.y -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			}
			else if (this.CursorLocked && this.lockCursor)
			{
				this.CursorLocked = false;
			}
			if (this.ScrollZoom)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis != 0f)
				{
					this.distance = Mathf.Clamp(this.distance * (1f - axis * this.zoomSpeed), this.distanceMin, this.distanceMax);
				}
				if (Input.touchCount == 2)
				{
					Touch touch = Input.GetTouch(0);
					Touch touch2 = Input.GetTouch(1);
					Vector2 a = touch.position - touch.deltaPosition;
					Vector2 b = touch2.position - touch2.deltaPosition;
					float magnitude = (a - b).magnitude;
					float magnitude2 = (touch.position - touch2.position).magnitude;
					float num = magnitude - magnitude2;
					this.distance = Mathf.Clamp(this.distance * (1f + num * this.zoomSpeed * 0.1f), this.distanceMin, this.distanceMax);
				}
			}
			this.y = ConfigurableMouseOrbit.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
			Quaternion quaternion = Quaternion.Euler(this.y, this.x, 0f);
			Vector3 vector = quaternion * new Vector3(0f, 0f, -this.distance) + this.target.position;
			if (this.centerToAABB && this.target.GetComponent<Renderer>())
			{
				vector += this.target.transform.InverseTransformPoint(this.target.GetComponent<Renderer>().bounds.center);
			}
			base.transform.localRotation = quaternion;
			base.transform.localPosition = vector;
		}

		private static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}

		public Transform target;

		public float distance = 3f;

		public float zoomSpeed = 1f;

		public float distanceMin = 0.2f;

		public float distanceMax = 10f;

		public float xSpeed = 250f;

		public float ySpeed = 120f;

		public float yMinLimit;

		public float yMaxLimit = 90f;

		private float x;

		private float y;

		public bool centerToAABB = true;

		public bool ScrollZoom = true;

		public ConfigurableMouseOrbit.MouseButton mouseButton;

		public bool lockCursor = true;

		public enum MouseButton
		{
			None = -1,
			Left,
			Middle = 2,
			Right = 1
		}
	}
}
