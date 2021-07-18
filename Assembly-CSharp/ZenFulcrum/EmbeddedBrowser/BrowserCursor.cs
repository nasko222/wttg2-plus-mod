using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class BrowserCursor
	{
		public BrowserCursor()
		{
			BrowserCursor.Load();
			this.normalTexture = new Texture2D(BrowserCursor.size, BrowserCursor.size, TextureFormat.ARGB32, false);
			this.SetActiveCursor(BrowserNative.CursorType.Pointer);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action cursorChange = delegate()
		{
		};

		private static void Load()
		{
			if (BrowserCursor.loaded)
			{
				return;
			}
			BrowserCursor.allCursors = Resources.Load<Texture2D>("Browser/Cursors");
			if (!BrowserCursor.allCursors)
			{
				throw new Exception("Failed to find browser allCursors");
			}
			BrowserCursor.size = BrowserCursor.allCursors.height;
			TextAsset textAsset = Resources.Load<TextAsset>("Browser/Cursors");
			foreach (string text in textAsset.text.Split(new char[]
			{
				'\n'
			}))
			{
				string[] array2 = text.Split(new char[]
				{
					','
				});
				BrowserNative.CursorType key = (BrowserNative.CursorType)Enum.Parse(typeof(BrowserNative.CursorType), array2[0]);
				BrowserCursor.CursorInfo value = new BrowserCursor.CursorInfo
				{
					atlasOffset = int.Parse(array2[1]),
					hotspot = new Vector2((float)int.Parse(array2[2]), (float)int.Parse(array2[3]))
				};
				BrowserCursor.mapping[key] = value;
			}
			BrowserCursor.loaded = true;
		}

		public virtual Texture2D Texture { get; protected set; }

		public virtual Vector2 Hotspot { get; protected set; }

		public virtual void SetActiveCursor(BrowserNative.CursorType type)
		{
			if (type == BrowserNative.CursorType.Custom)
			{
				throw new ArgumentException("Use SetCustomCursor to set custom cursors.", "type");
			}
			if (type == BrowserNative.CursorType.None)
			{
				this.Texture = null;
				this.cursorChange();
				return;
			}
			BrowserCursor.CursorInfo cursorInfo = BrowserCursor.mapping[type];
			Color[] pixels = BrowserCursor.allCursors.GetPixels(cursorInfo.atlasOffset * BrowserCursor.size, 0, BrowserCursor.size, BrowserCursor.size);
			this.Hotspot = cursorInfo.hotspot;
			this.normalTexture.SetPixels(pixels);
			this.normalTexture.Apply(true);
			this.Texture = this.normalTexture;
			this.cursorChange();
		}

		public virtual void SetCustomCursor(Texture2D cursor, Vector2 hotspot)
		{
			if (!this.customTexture || this.customTexture.width != cursor.width || this.customTexture.height != cursor.height)
			{
				UnityEngine.Object.Destroy(this.customTexture);
				this.customTexture = new Texture2D(cursor.width, cursor.height, TextureFormat.ARGB32, false);
			}
			this.customTexture.SetPixels32(cursor.GetPixels32());
			this.customTexture.Apply(true);
			this.Hotspot = hotspot;
			this.Texture = this.customTexture;
			this.cursorChange();
		}

		private static Dictionary<BrowserNative.CursorType, BrowserCursor.CursorInfo> mapping = new Dictionary<BrowserNative.CursorType, BrowserCursor.CursorInfo>();

		private static bool loaded = false;

		private static int size;

		private static Texture2D allCursors;

		protected Texture2D normalTexture;

		protected Texture2D customTexture;

		public class CursorInfo
		{
			public int atlasOffset;

			public Vector2 hotspot;
		}
	}
}
