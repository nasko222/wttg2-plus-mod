using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class FPSCursorRenderer : MonoBehaviour
	{
		public static FPSCursorRenderer Instance
		{
			get
			{
				if (!FPSCursorRenderer._instance)
				{
					FPSCursorRenderer._instance = UnityEngine.Object.FindObjectOfType<FPSCursorRenderer>();
					if (!FPSCursorRenderer._instance)
					{
						GameObject gameObject = new GameObject("Cursor Crosshair");
						FPSCursorRenderer._instance = gameObject.AddComponent<FPSCursorRenderer>();
					}
				}
				return FPSCursorRenderer._instance;
			}
		}

		public bool EnableInput { get; set; }

		public static void SetUpBrowserInput(Browser browser, MeshCollider mesh)
		{
			FPSCursorRenderer instance = FPSCursorRenderer.Instance;
			Transform transform = instance.pointer;
			if (!transform)
			{
				transform = Camera.main.transform;
			}
			FPSBrowserUI fpsbrowserUI = FPSBrowserUI.Create(mesh, transform, instance);
			fpsbrowserUI.maxDistance = instance.maxDistance;
			browser.UIHandler = fpsbrowserUI;
		}

		public void Start()
		{
			this.EnableInput = true;
			this.baseCursor = new BrowserCursor();
			this.baseCursor.SetActiveCursor(BrowserNative.CursorType.Cross);
		}

		public void OnGUI()
		{
			if (!this.EnableInput)
			{
				return;
			}
			BrowserCursor browserCursor = this.currentCursor ?? this.baseCursor;
			Texture2D texture = browserCursor.Texture;
			if (texture == null)
			{
				return;
			}
			Rect position = new Rect((float)Screen.width / 2f, (float)Screen.height / 2f, (float)texture.width * this.scale, (float)texture.height * this.scale);
			position.x -= browserCursor.Hotspot.x * this.scale;
			position.y -= browserCursor.Hotspot.y * this.scale;
			GUI.DrawTexture(position, texture);
		}

		public void SetCursor(BrowserCursor newCursor, FPSBrowserUI ui)
		{
			this.currentCursor = newCursor;
		}

		private static FPSCursorRenderer _instance;

		[Tooltip("How large should we render the cursor?")]
		public float scale = 0.5f;

		[Tooltip("How far can we reach to push buttons and such?")]
		public float maxDistance = 7f;

		[Tooltip("What are we using to point at things? Leave as null to use Camera.main")]
		public Transform pointer;

		protected BrowserCursor baseCursor;

		protected BrowserCursor currentCursor;
	}
}
