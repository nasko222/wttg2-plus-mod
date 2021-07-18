using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public static class BrowserNative
	{
		public static bool NativeLoaded { get; private set; }

		public static string LocalUrlPrefix
		{
			get
			{
				return "http://game.local/";
			}
		}

		private static void LogCallback(string message)
		{
			Debug.Log("ZFWeb: " + message);
		}

		public static void LoadNative()
		{
			if (BrowserNative.NativeLoaded)
			{
				return;
			}
			if (BrowserNative.webResources == null)
			{
				StandaloneWebResources standaloneWebResources = new StandaloneWebResources(Application.dataPath + "/Resources/browser_assets");
				standaloneWebResources.LoadIndex();
				BrowserNative.webResources = standaloneWebResources;
			}
			int debugPort = (!Debug.isDebugBuild) ? 0 : 9849;
			FileLocations.CEFDirs dirs = FileLocations.Dirs;
			string fullName = Directory.GetParent(Application.dataPath).FullName;
			string text = Environment.GetEnvironmentVariable("PATH");
			text = text + ";" + fullName;
			Environment.SetEnvironmentVariable("PATH", text);
			StandaloneShutdown.Create();
			try
			{
				BrowserNative.zfb_destroyAllBrowsers();
				if (BrowserNative.<>f__mg$cache0 == null)
				{
					BrowserNative.<>f__mg$cache0 = new BrowserNative.MessageFunc(BrowserNative.LogCallback);
				}
				BrowserNative.zfb_setDebugFunc(BrowserNative.<>f__mg$cache0);
				if (BrowserNative.<>f__mg$cache1 == null)
				{
					BrowserNative.<>f__mg$cache1 = new BrowserNative.GetRequestHeadersFunc(BrowserNative.HeaderCallback);
				}
				BrowserNative.GetRequestHeadersFunc headerFunc = BrowserNative.<>f__mg$cache1;
				if (BrowserNative.<>f__mg$cache2 == null)
				{
					BrowserNative.<>f__mg$cache2 = new BrowserNative.GetRequestDataFunc(BrowserNative.DataCallback);
				}
				BrowserNative.zfb_localRequestFuncs(headerFunc, BrowserNative.<>f__mg$cache2);
				BrowserNative.zfb_setCallbacksEnabled(true);
				BrowserNative.ZFBInitialSettings settings = new BrowserNative.ZFBInitialSettings
				{
					cefPath = dirs.resourcesPath,
					localePath = dirs.localesPath,
					subprocessFile = dirs.subprocessFile,
					userAgent = UserAgent.GetUserAgent(),
					logFile = dirs.logFile,
					debugPort = debugPort,
					multiThreadedMessageLoop = 1
				};
				foreach (string value in BrowserNative.commandLineSwitches)
				{
					BrowserNative.zfb_addCLISwitch(value);
				}
				if (!BrowserNative.zfb_init(settings))
				{
					throw new Exception("Failed to initialize browser system.");
				}
				BrowserNative.NativeLoaded = true;
			}
			finally
			{
			}
			AppDomain.CurrentDomain.DomainUnload += delegate(object sender, EventArgs args)
			{
				BrowserNative.zfb_destroyAllBrowsers();
				BrowserNative.zfb_setCallbacksEnabled(false);
			};
		}

		private static void FixProcessPermissions(FileLocations.CEFDirs dirs)
		{
			uint num = (uint)File.GetAttributes(dirs.subprocessFile);
			num |= 2147483648u;
			File.SetAttributes(dirs.subprocessFile, (FileAttributes)num);
		}

		private static int HeaderCallback(string url, IntPtr mimeTypeDest, out int size, out int responseCode)
		{
			if (url.SafeStartsWith(BrowserNative.LocalUrlPrefix))
			{
				url = "/" + url.Substring(BrowserNative.LocalUrlPrefix.Length);
			}
			WebResources.Response value;
			if (BrowserNative.webResources == null)
			{
				value = new WebResources.Response
				{
					data = Encoding.UTF8.GetBytes("No WebResources handler!"),
					mimeType = "text/plain",
					responseCode = 500
				};
			}
			else
			{
				value = BrowserNative.webResources[url];
			}
			byte[] array = Encoding.UTF8.GetBytes(value.mimeType);
			if (array.Length > 99)
			{
				Debug.LogWarning("mime type is too long " + value.mimeType);
				array = new byte[0];
			}
			Marshal.Copy(array, 0, mimeTypeDest, array.Length);
			Marshal.WriteByte(mimeTypeDest, array.Length, 0);
			responseCode = value.responseCode;
			size = value.data.Length;
			object obj = BrowserNative.requests;
			int num;
			lock (obj)
			{
				num = BrowserNative.nextRequestId++;
				BrowserNative.requests[num] = value;
			}
			return num;
		}

		private static void DataCallback(int reqId, IntPtr data, int size)
		{
			object obj = BrowserNative.requests;
			WebResources.Response response;
			lock (obj)
			{
				if (!BrowserNative.requests.TryGetValue(reqId, out response))
				{
					response = new WebResources.Response
					{
						data = Encoding.UTF8.GetBytes("No response for request!"),
						mimeType = "text/plain",
						responseCode = 500
					};
				}
				BrowserNative.requests.Remove(reqId);
			}
			if (size != 0)
			{
				Marshal.Copy(response.data, 0, data, size);
			}
		}

		public static void UnloadNative()
		{
			if (!BrowserNative.NativeLoaded)
			{
				return;
			}
			Debug.Log("Stop CEF");
			BrowserNative.zfb_setCallbacksEnabled(false);
			BrowserNative.zfb_shutdown();
			BrowserNative.NativeLoaded = false;
		}

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_noop();

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_free(IntPtr memory);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_memcpy(IntPtr dst, IntPtr src, int size);

		[DllImport("ZFEmbedWeb")]
		public static extern IntPtr zfb_getVersion();

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_setDebugFunc(BrowserNative.MessageFunc debugFunc);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_localRequestFuncs(BrowserNative.GetRequestHeadersFunc headerFunc, BrowserNative.GetRequestDataFunc dataFunc);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_setCallbacksEnabled(bool enabled);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_destroyAllBrowsers();

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_addCLISwitch(string value);

		[DllImport("ZFEmbedWeb")]
		public static extern bool zfb_init(BrowserNative.ZFBInitialSettings settings);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_shutdown();

		[DllImport("ZFEmbedWeb")]
		public static extern int zfb_createBrowser(BrowserNative.ZFBSettings settings);

		[DllImport("ZFEmbedWeb")]
		public static extern int zfb_numBrowsers();

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_destoryBrowser(int id);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_tick();

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_setReadyCallback(int id, BrowserNative.ReadyFunc cb);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_resize(int id, int w, int h);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_setOverlay(int browserId, int overlayBrowserId);

		[DllImport("ZFEmbedWeb")]
		public static extern BrowserNative.RenderData zfb_getImage(int id, bool forceDirty);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_goToURL(int id, string url, bool force);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_goToHTML(int id, string html, string url);

		[DllImport("ZFEmbedWeb")]
		public static extern IntPtr zfb_getURL(int id);

		[DllImport("ZFEmbedWeb")]
		public static extern bool zfb_canNav(int id, int direction);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_doNav(int id, int direction);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_setZoom(int id, double zoom);

		[DllImport("ZFEmbedWeb")]
		public static extern bool zfb_isLoading(int id);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_changeLoading(int id, BrowserNative.LoadChange what);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_showDevTools(int id, bool show);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_setFocused(int id, bool focused);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_mouseMove(int id, float x, float y);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_mouseButton(int id, BrowserNative.MouseButton button, bool down, int clickCount);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_mouseScroll(int id, int deltaX, int deltaY);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_keyEvent(int id, bool down, int windowsKeyCode);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_characterEvent(int id, int character, int windowsKeyCode);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_registerConsoleCallback(int id, BrowserNative.ConsoleFunc callback);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_evalJS(int id, string script, string scriptURL);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_registerJSCallback(int id, BrowserNative.ForwardJSCallFunc cb);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_registerChangeCallback(int id, BrowserNative.ChangeFunc cb);

		[DllImport("ZFEmbedWeb")]
		public static extern BrowserNative.CursorType zfb_getMouseCursor(int id, out int width, out int height);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_getMouseCustomCursor(int id, IntPtr buffer, int width, int height, out int hotX, out int hotY);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_registerDialogCallback(int id, BrowserNative.DisplayDialogFunc cb);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_sendDialogResults(int id, bool affirmed, string text1, string text2);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_registerPopupCallback(int id, BrowserNative.NewWindowFunc cb);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_registerContextMenuCallback(int id, BrowserNative.ShowContextMenuFunc cb);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_sendContextMenuResults(int id, int commandId);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_sendCommandToFocusedFrame(int id, BrowserNative.FrameCommand command);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_getCookies(int id, BrowserNative.GetCookieFunc cb);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_editCookie(int id, BrowserNative.NativeCookie cookie, BrowserNative.CookieAction action);

		[DllImport("ZFEmbedWeb")]
		public static extern void zfb_clearCookies(int id);

		public const int DebugPort = 9849;

		public static List<string> commandLineSwitches = new List<string>
		{
			"--enable-system-flash"
		};

		public static WebResources webResources;

		private static Dictionary<int, WebResources.Response> requests = new Dictionary<int, WebResources.Response>();

		private static int nextRequestId = 1;

		[CompilerGenerated]
		private static BrowserNative.MessageFunc <>f__mg$cache0;

		[CompilerGenerated]
		private static BrowserNative.GetRequestHeadersFunc <>f__mg$cache1;

		[CompilerGenerated]
		private static BrowserNative.GetRequestDataFunc <>f__mg$cache2;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void MessageFunc(string message);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int GetRequestHeadersFunc(string url, IntPtr mimeType, out int size, out int responseCode);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void GetRequestDataFunc(int reqId, IntPtr data, int size);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ReadyFunc(int browserId);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ConsoleFunc(int browserId, string message, string source, int line);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ForwardJSCallFunc(int browserId, int callbackId, string data, int size);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate BrowserNative.NewWindowAction NewWindowFunc(int browserId, IntPtr newURL, bool userInvoked, int possibleBrowserId, ref BrowserNative.ZFBSettings possibleSettings);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ChangeFunc(int browserId, BrowserNative.ChangeType changeType, string arg1);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void DisplayDialogFunc(int browserId, BrowserNative.DialogType dialogType, string dialogText, string initialPromptText, string sourceURL);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ShowContextMenuFunc(int browserId, string menuJSON, int x, int y, BrowserNative.ContextMenuOrigin origin);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void GetCookieFunc(BrowserNative.NativeCookie cookie);

		public enum LoadChange
		{
			LC_STOP = 1,
			LC_RELOAD,
			LC_FORCE_RELOAD
		}

		public enum MouseButton
		{
			MBT_LEFT,
			MBT_MIDDLE,
			MBT_RIGHT
		}

		public enum ChangeType
		{
			CHT_CURSOR,
			CHT_BROWSER_CLOSE,
			CHT_FETCH_FINISHED,
			CHT_FETCH_FAILED,
			CHT_LOAD_FINISHED,
			CHT_CERT_ERROR,
			CHT_SAD_TAB
		}

		public enum CursorType
		{
			Pointer,
			Cross,
			Hand,
			IBeam,
			Wait,
			Help,
			EastResize,
			NorthResize,
			NorthEastResize,
			NorthWestResize,
			SouthResize,
			SouthEastResize,
			SouthWestResize,
			WestResize,
			NorthSouthResize,
			EastWestResize,
			NorthEastSouthWestResize,
			NorthWestSouthEastResize,
			ColumnResize,
			RowResize,
			MiddlePanning,
			EastPanning,
			NorthPanning,
			NorthEastPanning,
			NorthWestPanning,
			SouthPanning,
			SouthEastPanning,
			SouthWestPanning,
			WestPanning,
			Move,
			VerticalText,
			Cell,
			ContextMenu,
			Alias,
			Progress,
			NoDrop,
			Copy,
			None,
			NotAllowed,
			ZoomIn,
			ZoomOut,
			Grab,
			Grabbing,
			Custom
		}

		public enum DialogType
		{
			DLT_HIDE,
			DLT_ALERT,
			DLT_CONFIRM,
			DLT_PROMPT,
			DLT_PAGE_UNLOAD,
			DLT_PAGE_RELOAD,
			DLT_GET_AUTH
		}

		public enum NewWindowAction
		{
			NWA_IGNORE = 1,
			NWA_REDIRECT,
			NWA_NEW_BROWSER,
			NWA_NEW_WINDOW
		}

		[Flags]
		public enum ContextMenuOrigin
		{
			Editable = 2,
			Image = 4,
			Selection = 8,
			Other = 1
		}

		public enum FrameCommand
		{
			Undo,
			Redo,
			Cut,
			Copy,
			Paste,
			Delete,
			SelectAll,
			ViewSource
		}

		public enum CookieAction
		{
			Delete,
			Create
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct ZFBInitialSettings
		{
			public string cefPath;

			public string localePath;

			public string subprocessFile;

			public string userAgent;

			public string logFile;

			public int debugPort;

			public int multiThreadedMessageLoop;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct ZFBSettings
		{
			public int bgR;

			public int bgG;

			public int bgB;

			public int bgA;

			public int offscreen;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RenderData
		{
			public IntPtr pixels;

			public int w;

			public int h;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class NativeCookie
		{
			public string name;

			public string value;

			public string domain;

			public string path;

			public string creation;

			public string lastAccess;

			public string expires;

			public byte secure;

			public byte httpOnly;
		}
	}
}
