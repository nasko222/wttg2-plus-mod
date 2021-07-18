using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public interface IBrowserUI
	{
		void InputUpdate();

		bool MouseHasFocus { get; }

		Vector2 MousePosition { get; }

		MouseButton MouseButtons { get; }

		Vector2 MouseScroll { get; }

		bool KeyboardHasFocus { get; }

		List<Event> KeyEvents { get; }

		BrowserCursor BrowserCursor { get; }

		BrowserInputSettings InputSettings { get; }
	}
}
