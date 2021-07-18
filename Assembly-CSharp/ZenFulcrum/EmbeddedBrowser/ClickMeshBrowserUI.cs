using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class ClickMeshBrowserUI : MonoBehaviour, IBrowserUI
	{
		public static ClickMeshBrowserUI Create(MeshCollider meshCollider)
		{
			ClickMeshBrowserUI clickMeshBrowserUI = meshCollider.gameObject.AddComponent<ClickMeshBrowserUI>();
			clickMeshBrowserUI.meshCollider = meshCollider;
			return clickMeshBrowserUI;
		}

		public void Awake()
		{
			this.BrowserCursor = new BrowserCursor();
			this.BrowserCursor.cursorChange += this.CursorUpdated;
			this.InputSettings = new BrowserInputSettings();
		}

		protected virtual Ray LookRay
		{
			get
			{
				return Camera.main.ScreenPointToRay(Input.mousePosition);
			}
		}

		public virtual void InputUpdate()
		{
			List<Event> list = this.keyEvents;
			this.keyEvents = this.keyEventsLast;
			this.keyEventsLast = list;
			this.keyEvents.Clear();
			Ray lookRay = this.LookRay;
			RaycastHit raycastHit;
			Physics.Raycast(lookRay, out raycastHit, this.maxDistance);
			if (raycastHit.transform != this.meshCollider.transform)
			{
				this.MousePosition = new Vector3(0f, 0f);
				this.MouseButtons = (MouseButton)0;
				this.MouseScroll = new Vector2(0f, 0f);
				this.MouseHasFocus = false;
				this.KeyboardHasFocus = false;
				this.LookOff();
				return;
			}
			this.LookOn();
			this.MouseHasFocus = true;
			this.KeyboardHasFocus = true;
			Vector2 textureCoord = raycastHit.textureCoord;
			this.MousePosition = textureCoord;
			MouseButton mouseButton = (MouseButton)0;
			if (Input.GetMouseButton(0))
			{
				mouseButton |= MouseButton.Left;
			}
			if (Input.GetMouseButton(1))
			{
				mouseButton |= MouseButton.Right;
			}
			if (Input.GetMouseButton(2))
			{
				mouseButton |= MouseButton.Middle;
			}
			this.MouseButtons = mouseButton;
			this.MouseScroll = Input.mouseScrollDelta;
			for (int i = 0; i < ClickMeshBrowserUI.keysToCheck.Length; i++)
			{
				if (Input.GetKeyDown(ClickMeshBrowserUI.keysToCheck[i]))
				{
					this.keyEventsLast.Insert(0, new Event
					{
						type = EventType.KeyDown,
						keyCode = ClickMeshBrowserUI.keysToCheck[i]
					});
				}
				else if (Input.GetKeyUp(ClickMeshBrowserUI.keysToCheck[i]))
				{
					this.keyEventsLast.Add(new Event
					{
						type = EventType.KeyUp,
						keyCode = ClickMeshBrowserUI.keysToCheck[i]
					});
				}
			}
		}

		public void OnGUI()
		{
			Event current = Event.current;
			if (current.type != EventType.KeyDown && current.type != EventType.KeyUp)
			{
				return;
			}
			this.keyEvents.Add(new Event(current));
		}

		protected void LookOn()
		{
			if (this.BrowserCursor != null)
			{
				this.CursorUpdated();
			}
			this.mouseWasOver = true;
		}

		protected void LookOff()
		{
			if (this.BrowserCursor != null && this.mouseWasOver)
			{
				this.SetCursor(null);
			}
			this.mouseWasOver = false;
		}

		protected void CursorUpdated()
		{
			this.SetCursor(this.BrowserCursor);
		}

		protected virtual void SetCursor(BrowserCursor newCursor)
		{
			if (!this.MouseHasFocus && newCursor != null)
			{
				return;
			}
			if (newCursor == null)
			{
				Cursor.visible = true;
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
			else if (newCursor.Texture != null)
			{
				Cursor.visible = true;
				Cursor.SetCursor(newCursor.Texture, newCursor.Hotspot, CursorMode.Auto);
			}
			else
			{
				Cursor.visible = false;
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
		}

		public bool MouseHasFocus { get; protected set; }

		public Vector2 MousePosition { get; protected set; }

		public MouseButton MouseButtons { get; protected set; }

		public Vector2 MouseScroll { get; protected set; }

		public bool KeyboardHasFocus { get; protected set; }

		public List<Event> KeyEvents
		{
			get
			{
				return this.keyEventsLast;
			}
		}

		public BrowserCursor BrowserCursor { get; protected set; }

		public BrowserInputSettings InputSettings { get; protected set; }

		protected MeshCollider meshCollider;

		[HideInInspector]
		public float maxDistance = float.PositiveInfinity;

		protected List<Event> keyEvents = new List<Event>();

		protected List<Event> keyEventsLast = new List<Event>();

		private static readonly KeyCode[] keysToCheck = new KeyCode[]
		{
			KeyCode.LeftShift,
			KeyCode.RightShift
		};

		protected bool mouseWasOver;
	}
}
