using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	[RequireComponent(typeof(Browser))]
	[RequireComponent(typeof(MeshCollider))]
	public class FPSBrowserUI : ClickMeshBrowserUI
	{
		public void Start()
		{
			FPSCursorRenderer.SetUpBrowserInput(base.GetComponent<Browser>(), base.GetComponent<MeshCollider>());
		}

		public static FPSBrowserUI Create(MeshCollider meshCollider, Transform worldPointer, FPSCursorRenderer cursorRenderer)
		{
			FPSBrowserUI fpsbrowserUI = meshCollider.gameObject.GetComponent<FPSBrowserUI>();
			if (!fpsbrowserUI)
			{
				fpsbrowserUI = meshCollider.gameObject.AddComponent<FPSBrowserUI>();
			}
			fpsbrowserUI.meshCollider = meshCollider;
			fpsbrowserUI.worldPointer = worldPointer;
			fpsbrowserUI.cursorRenderer = cursorRenderer;
			return fpsbrowserUI;
		}

		protected override Ray LookRay
		{
			get
			{
				return new Ray(this.worldPointer.position, this.worldPointer.forward);
			}
		}

		protected override void SetCursor(BrowserCursor newCursor)
		{
			if (newCursor != null && !base.MouseHasFocus)
			{
				return;
			}
			this.cursorRenderer.SetCursor(newCursor, this);
		}

		public override void InputUpdate()
		{
			if (!this.cursorRenderer.EnableInput)
			{
				base.MouseHasFocus = false;
				base.KeyboardHasFocus = false;
				return;
			}
			base.InputUpdate();
		}

		protected Transform worldPointer;

		protected FPSCursorRenderer cursorRenderer;
	}
}
