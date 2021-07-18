using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	internal class BrowserInput
	{
		public BrowserInput(Browser browser)
		{
			this.browser = browser;
		}

		public void HandleInput()
		{
			this.browser.UIHandler.InputUpdate();
			if (this.browser.UIHandler.MouseHasFocus || this.mouseWasFocused)
			{
				this.HandleMouseInput();
			}
			this.mouseWasFocused = this.browser.UIHandler.MouseHasFocus;
			if (this.browser.UIHandler.KeyboardHasFocus)
			{
				if (!this.kbWasFocused)
				{
					BrowserNative.zfb_setFocused(this.browser.browserId, this.kbWasFocused = true);
				}
				this.HandleKeyInput();
			}
			else if (this.kbWasFocused)
			{
				BrowserNative.zfb_setFocused(this.browser.browserId, this.kbWasFocused = false);
			}
		}

		private void HandleMouseInput()
		{
			IBrowserUI uihandler = this.browser.UIHandler;
			Vector2 mousePosition = uihandler.MousePosition;
			MouseButton mouseButtons = uihandler.MouseButtons;
			Vector2 mouseScroll = uihandler.MouseScroll;
			if (mousePosition != this.prevPos)
			{
				BrowserNative.zfb_mouseMove(this.browser.browserId, mousePosition.x, 1f - mousePosition.y);
			}
			if (mouseScroll.sqrMagnitude != 0f)
			{
				BrowserNative.zfb_mouseScroll(this.browser.browserId, (int)mouseScroll.x * uihandler.InputSettings.scrollSpeed, (int)mouseScroll.y * uihandler.InputSettings.scrollSpeed);
			}
			bool flag = (this.prevButtons & MouseButton.Left) != (mouseButtons & MouseButton.Left);
			bool flag2 = (mouseButtons & MouseButton.Left) == MouseButton.Left;
			bool flag3 = (this.prevButtons & MouseButton.Middle) != (mouseButtons & MouseButton.Middle);
			bool down = (mouseButtons & MouseButton.Middle) == MouseButton.Middle;
			bool flag4 = (this.prevButtons & MouseButton.Right) != (mouseButtons & MouseButton.Right);
			bool down2 = (mouseButtons & MouseButton.Right) == MouseButton.Right;
			if (flag)
			{
				if (flag2)
				{
					this.leftClickHistory.ButtonPress(mousePosition, uihandler, this.browser.Size);
				}
				BrowserNative.zfb_mouseButton(this.browser.browserId, BrowserNative.MouseButton.MBT_LEFT, flag2, (!flag2) ? 0 : this.leftClickHistory.repeatCount);
			}
			if (flag3)
			{
				BrowserNative.zfb_mouseButton(this.browser.browserId, BrowserNative.MouseButton.MBT_MIDDLE, down, 1);
			}
			if (flag4)
			{
				BrowserNative.zfb_mouseButton(this.browser.browserId, BrowserNative.MouseButton.MBT_RIGHT, down2, 1);
			}
			this.prevPos = mousePosition;
			this.prevButtons = mouseButtons;
		}

		private void HandleKeyInput()
		{
			List<Event> keyEvents = this.browser.UIHandler.KeyEvents;
			if (keyEvents.Count == 0)
			{
				return;
			}
			foreach (Event @event in keyEvents)
			{
				int windowsKeyCode = KeyMappings.GetWindowsKeyCode(@event);
				if (@event.character == '\n')
				{
					@event.character = '\r';
				}
				if (@event.character != '\0' && @event.type == EventType.KeyDown)
				{
					BrowserNative.zfb_characterEvent(this.browser.browserId, (int)@event.character, windowsKeyCode);
				}
				else
				{
					BrowserNative.zfb_keyEvent(this.browser.browserId, @event.type == EventType.KeyDown, windowsKeyCode);
				}
			}
		}

		private readonly Browser browser;

		private bool kbWasFocused;

		private bool mouseWasFocused;

		private MouseButton prevButtons;

		private Vector2 prevPos;

		private readonly BrowserInput.ButtonHistory leftClickHistory = new BrowserInput.ButtonHistory();

		private class ButtonHistory
		{
			public void ButtonPress(Vector3 mousePos, IBrowserUI uiHandler, Vector2 browserSize)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (realtimeSinceStartup - this.lastPressTime > uiHandler.InputSettings.multiclickSpeed)
				{
					this.repeatCount = 0;
				}
				if (this.repeatCount > 0)
				{
					Vector2 a = Vector2.Scale(mousePos, browserSize);
					Vector2 b = Vector2.Scale(this.lastPosition, browserSize);
					if (Vector2.Distance(a, b) > uiHandler.InputSettings.multiclickTolerance)
					{
						this.repeatCount = 0;
					}
				}
				this.repeatCount++;
				this.lastPressTime = realtimeSinceStartup;
				this.lastPosition = mousePos;
			}

			public float lastPressTime;

			public int repeatCount;

			public Vector3 lastPosition;
		}
	}
}
