using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class Browser : MonoBehaviour
	{
		public static string LocalUrlPrefix
		{
			get
			{
				return BrowserNative.LocalUrlPrefix;
			}
		}

		public IBrowserUI UIHandler
		{
			get
			{
				return this._uiHandler;
			}
			set
			{
				this.uiHandlerAssigned = true;
				this._uiHandler = value;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string, string> onConsoleMessage = delegate(string s, string s1)
		{
		};

		public INewWindowHandler NewWindowHandler { get; set; }

		public bool EnableRendering { get; set; }

		public bool EnableInput { get; set; }

		public CookieManager CookieManager { get; private set; }

		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Texture2D> afterResize = delegate(Texture2D t)
		{
		};

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event BrowserNative.ReadyFunc onNativeReady;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<JSONNode> onLoad = delegate(JSONNode loadData)
		{
		};

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<JSONNode> onFetch = delegate(JSONNode loadData)
		{
		};

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<JSONNode> onFetchError = delegate(JSONNode errCode)
		{
		};

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<JSONNode> onCertError = delegate(JSONNode errInfo)
		{
		};

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action onSadTab = delegate()
		{
		};

		protected void Awake()
		{
			this.EnableRendering = true;
			this.EnableInput = true;
			this.CookieManager = new CookieManager(this);
			this.browserInput = new BrowserInput(this);
			this.onNativeReady += delegate(int id)
			{
				if (!this.uiHandlerAssigned)
				{
					MeshCollider component = base.GetComponent<MeshCollider>();
					if (component)
					{
						this.UIHandler = ClickMeshBrowserUI.Create(component);
					}
				}
				this.Resize(this._width, this._height);
				this.Zoom = this._zoom;
				if (!string.IsNullOrEmpty(this._url))
				{
					this.Url = this._url;
				}
			};
			this.onConsoleMessage += delegate(string message, string source)
			{
				string message2 = source + ": " + message;
				UnityEngine.Debug.Log(message2, this);
			};
			this.onFetchError += delegate(JSONNode err)
			{
				if (err["error"] == "ERR_ABORTED")
				{
					return;
				}
				this.QueuePageReplacer(delegate
				{
					this.LoadHTML(Resources.Load<TextAsset>("Browser/Errors").text, this.Url);
					this.CallFunction("setErrorInfo", new JSONNode[]
					{
						err
					});
				}, -1000f);
			};
			this.onCertError += delegate(JSONNode err)
			{
				this.QueuePageReplacer(delegate
				{
					this.LoadHTML(Resources.Load<TextAsset>("Browser/Errors").text, this.Url);
					this.CallFunction("setErrorInfo", new JSONNode[]
					{
						err
					});
				}, -900f);
			};
			this.onSadTab += delegate()
			{
				this.QueuePageReplacer(delegate
				{
					this.LoadHTML(Resources.Load<TextAsset>("Browser/Errors").text, this.Url);
					this.CallFunction("showCrash", new JSONNode[0]);
				}, -1000f);
			};
		}

		public bool IsReady
		{
			get
			{
				return this.browserId != 0;
			}
		}

		public void WhenReady(Action callback)
		{
			if (this.IsReady)
			{
				object obj = this.thingsToDo;
				lock (obj)
				{
					this.thingsToDo.Add(callback);
				}
			}
			else
			{
				BrowserNative.ReadyFunc func = null;
				func = delegate(int id)
				{
					callback();
					this.onNativeReady -= func;
				};
				this.onNativeReady += func;
			}
		}

		public void RunOnMainThread(Action callback)
		{
			object obj = this.thingsToDo;
			lock (obj)
			{
				this.thingsToDo.Add(callback);
			}
		}

		public void WhenLoaded(Action callback)
		{
			this.onloadActions.Add(callback);
		}

		private void RequestNativeBrowser(int newBrowserId = 0)
		{
			Browser.<RequestNativeBrowser>c__AnonStoreyA <RequestNativeBrowser>c__AnonStoreyA = new Browser.<RequestNativeBrowser>c__AnonStoreyA();
			<RequestNativeBrowser>c__AnonStoreyA.$this = this;
			if (this.browserId != 0 || this.browserIdRequested)
			{
				return;
			}
			this.browserIdRequested = true;
			try
			{
				BrowserNative.LoadNative();
			}
			catch
			{
				base.gameObject.SetActive(false);
				throw;
			}
			if (newBrowserId == 0)
			{
				BrowserNative.ZFBSettings settings = new BrowserNative.ZFBSettings
				{
					bgR = (int)this.backgroundColor.r,
					bgG = (int)this.backgroundColor.g,
					bgB = (int)this.backgroundColor.b,
					bgA = (int)this.backgroundColor.a,
					offscreen = 1
				};
				<RequestNativeBrowser>c__AnonStoreyA.newId = BrowserNative.zfb_createBrowser(settings);
			}
			else
			{
				<RequestNativeBrowser>c__AnonStoreyA.newId = newBrowserId;
			}
			this.unsafeBrowserId = <RequestNativeBrowser>c__AnonStoreyA.newId;
			object obj = Browser.allThingsToRemember;
			lock (obj)
			{
				Browser.allThingsToRemember[<RequestNativeBrowser>c__AnonStoreyA.newId] = this.thingsToRemember;
			}
			BrowserNative.ForwardJSCallFunc forwardJSCallFunc = delegate(int bId, int id, string data, int size)
			{
				object obj2 = <RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo;
				lock (obj2)
				{
					<RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo.Add(delegate
					{
						Browser.JSResultFunc jsresultFunc;
						if (!<RequestNativeBrowser>c__AnonStoreyA.registeredCallbacks.TryGetValue(id, out jsresultFunc))
						{
							UnityEngine.Debug.LogWarning("Got a JS callback for event " + id + ", but no such event is registered.");
							return;
						}
						bool isError = false;
						if (data.StartsWith("fail-"))
						{
							isError = true;
							data = data.Substring(5);
						}
						JSONNode value;
						try
						{
							value = JSONNode.Parse(data);
						}
						catch (SerializationException)
						{
							UnityEngine.Debug.LogWarning("Invalid JSON sent from browser: " + data);
							return;
						}
						try
						{
							jsresultFunc(value, isError);
						}
						catch (Exception exception)
						{
							UnityEngine.Debug.LogException(exception);
						}
					});
				}
			};
			this.thingsToRemember.Add(forwardJSCallFunc);
			BrowserNative.zfb_registerJSCallback(<RequestNativeBrowser>c__AnonStoreyA.newId, forwardJSCallFunc);
			BrowserNative.ChangeFunc changeFunc = delegate(int id, BrowserNative.ChangeType type, string arg1)
			{
				if (type == BrowserNative.ChangeType.CHT_BROWSER_CLOSE)
				{
					if (<RequestNativeBrowser>c__AnonStoreyA.$this)
					{
						object obj2 = <RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo;
						lock (obj2)
						{
							<RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo.Add(delegate
							{
								UnityEngine.Object.Destroy(<RequestNativeBrowser>c__AnonStoreyA.gameObject);
							});
						}
					}
					object obj3 = Browser.allThingsToRemember;
					lock (obj3)
					{
						Browser.allThingsToRemember.Remove(<RequestNativeBrowser>c__AnonStoreyA.$this.unsafeBrowserId);
					}
					<RequestNativeBrowser>c__AnonStoreyA.$this.browserId = 0;
				}
				else if (<RequestNativeBrowser>c__AnonStoreyA.$this)
				{
					object obj4 = <RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo;
					lock (obj4)
					{
						<RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo.Add(delegate
						{
							<RequestNativeBrowser>c__AnonStoreyA.OnItemChange(type, arg1);
						});
					}
				}
			};
			this.thingsToRemember.Add(changeFunc);
			BrowserNative.zfb_registerChangeCallback(<RequestNativeBrowser>c__AnonStoreyA.newId, changeFunc);
			BrowserNative.DisplayDialogFunc displayDialogFunc = delegate(int id, BrowserNative.DialogType type, string text, string promptText, string url)
			{
				object obj2 = <RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo;
				lock (obj2)
				{
					<RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo.Add(delegate
					{
						<RequestNativeBrowser>c__AnonStoreyA.CreateDialogHandler();
						<RequestNativeBrowser>c__AnonStoreyA.dialogHandler.HandleDialog(type, text, promptText);
					});
				}
			};
			this.thingsToRemember.Add(displayDialogFunc);
			BrowserNative.zfb_registerDialogCallback(<RequestNativeBrowser>c__AnonStoreyA.newId, displayDialogFunc);
			BrowserNative.ShowContextMenuFunc showContextMenuFunc = delegate(int id, string json, int x, int y, BrowserNative.ContextMenuOrigin origin)
			{
				if (json != null && (<RequestNativeBrowser>c__AnonStoreyA.$this.allowContextMenuOn & origin) == (BrowserNative.ContextMenuOrigin)0)
				{
					BrowserNative.zfb_sendContextMenuResults(<RequestNativeBrowser>c__AnonStoreyA.$this.browserId, -1);
					return;
				}
				object obj2 = <RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo;
				lock (obj2)
				{
					<RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo.Add(delegate
					{
						if (json != null)
						{
							<RequestNativeBrowser>c__AnonStoreyA.CreateDialogHandler();
						}
						if (<RequestNativeBrowser>c__AnonStoreyA.dialogHandler != null)
						{
							<RequestNativeBrowser>c__AnonStoreyA.dialogHandler.HandleContextMenu(json, x, y);
						}
					});
				}
			};
			this.thingsToRemember.Add(showContextMenuFunc);
			BrowserNative.zfb_registerContextMenuCallback(<RequestNativeBrowser>c__AnonStoreyA.newId, showContextMenuFunc);
			BrowserNative.NewWindowFunc newWindowFunc = delegate(int id, IntPtr urlPtr, bool userInvoked, int possibleId, ref BrowserNative.ZFBSettings possibleSettings)
			{
				if (!userInvoked)
				{
					return BrowserNative.NewWindowAction.NWA_IGNORE;
				}
				switch (<RequestNativeBrowser>c__AnonStoreyA.$this.newWindowAction)
				{
				default:
					return BrowserNative.NewWindowAction.NWA_IGNORE;
				case Browser.NewWindowAction.Redirect:
					return BrowserNative.NewWindowAction.NWA_REDIRECT;
				case Browser.NewWindowAction.NewBrowser:
					if (<RequestNativeBrowser>c__AnonStoreyA.$this.NewWindowHandler != null)
					{
						possibleSettings.bgR = (int)<RequestNativeBrowser>c__AnonStoreyA.$this.backgroundColor.r;
						possibleSettings.bgG = (int)<RequestNativeBrowser>c__AnonStoreyA.$this.backgroundColor.g;
						possibleSettings.bgB = (int)<RequestNativeBrowser>c__AnonStoreyA.$this.backgroundColor.b;
						possibleSettings.bgA = (int)<RequestNativeBrowser>c__AnonStoreyA.$this.backgroundColor.a;
						object obj2 = <RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo;
						lock (obj2)
						{
							<RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo.Add(delegate
							{
								Browser browser = <RequestNativeBrowser>c__AnonStoreyA.NewWindowHandler.CreateBrowser(<RequestNativeBrowser>c__AnonStoreyA.$this);
								browser.RequestNativeBrowser(possibleId);
							});
							return BrowserNative.NewWindowAction.NWA_NEW_BROWSER;
						}
					}
					UnityEngine.Debug.LogError("Missing NewWindowHandler, can't open new window", <RequestNativeBrowser>c__AnonStoreyA.$this);
					return BrowserNative.NewWindowAction.NWA_IGNORE;
				case Browser.NewWindowAction.NewWindow:
					return BrowserNative.NewWindowAction.NWA_NEW_WINDOW;
				}
			};
			this.thingsToRemember.Add(newWindowFunc);
			BrowserNative.zfb_registerPopupCallback(<RequestNativeBrowser>c__AnonStoreyA.newId, newWindowFunc);
			BrowserNative.ConsoleFunc consoleFunc = delegate(int id, string message, string source, int line)
			{
				object obj2 = <RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo;
				lock (obj2)
				{
					<RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo.Add(delegate
					{
						<RequestNativeBrowser>c__AnonStoreyA.onConsoleMessage(message, source + ":" + line);
					});
				}
			};
			this.thingsToRemember.Add(consoleFunc);
			BrowserNative.zfb_registerConsoleCallback(<RequestNativeBrowser>c__AnonStoreyA.newId, consoleFunc);
			BrowserNative.ReadyFunc readyFunc = delegate(int id)
			{
				object obj2 = <RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo;
				lock (obj2)
				{
					<RequestNativeBrowser>c__AnonStoreyA.$this.thingsToDo.Add(delegate
					{
						<RequestNativeBrowser>c__AnonStoreyA.$this.browserId = <RequestNativeBrowser>c__AnonStoreyA.newId;
						<RequestNativeBrowser>c__AnonStoreyA.$this.onNativeReady(<RequestNativeBrowser>c__AnonStoreyA.$this.browserId);
					});
				}
			};
			this.thingsToRemember.Add(readyFunc);
			BrowserNative.zfb_setReadyCallback(<RequestNativeBrowser>c__AnonStoreyA.newId, readyFunc);
		}

		protected void OnItemChange(BrowserNative.ChangeType type, string arg1)
		{
			switch (type)
			{
			case BrowserNative.ChangeType.CHT_CURSOR:
				this.UpdateCursor();
				break;
			case BrowserNative.ChangeType.CHT_FETCH_FINISHED:
				this.onFetch(JSONNode.Parse(arg1));
				break;
			case BrowserNative.ChangeType.CHT_FETCH_FAILED:
				this.onFetchError(JSONNode.Parse(arg1));
				break;
			case BrowserNative.ChangeType.CHT_LOAD_FINISHED:
				if (this.skipNextLoad)
				{
					this.skipNextLoad = false;
					return;
				}
				this.loadPending = false;
				if (this.onloadActions.Count != 0)
				{
					foreach (Action action in this.onloadActions)
					{
						action();
					}
					this.onloadActions.Clear();
				}
				this.onLoad(JSONNode.Parse(arg1));
				break;
			case BrowserNative.ChangeType.CHT_CERT_ERROR:
				this.onCertError(JSONNode.Parse(arg1));
				break;
			case BrowserNative.ChangeType.CHT_SAD_TAB:
				this.onSadTab();
				break;
			}
		}

		protected void CreateDialogHandler()
		{
			if (this.dialogHandler != null)
			{
				return;
			}
			DialogHandler.DialogCallback dialogCallback = delegate(bool affirm, string text1, string text2)
			{
				this.CheckSanity();
				BrowserNative.zfb_sendDialogResults(this.browserId, affirm, text1, text2);
			};
			DialogHandler.MenuCallback contextCallback = delegate(int commandId)
			{
				this.CheckSanity();
				BrowserNative.zfb_sendContextMenuResults(this.browserId, commandId);
			};
			this.dialogHandler = DialogHandler.Create(this, dialogCallback, contextCallback);
		}

		protected void CheckSanity()
		{
			if (this.browserId == 0)
			{
				throw new InvalidOperationException("No native browser allocated");
			}
		}

		internal bool DeferUnready(Action ifNotReady)
		{
			if (this.browserId == 0)
			{
				this.WhenReady(ifNotReady);
				return true;
			}
			this.CheckSanity();
			return false;
		}

		protected void OnDisable()
		{
		}

		protected void OnDestroy()
		{
			if (this.browserId == 0)
			{
				return;
			}
			if (this.dialogHandler)
			{
				UnityEngine.Object.DestroyImmediate(this.dialogHandler.gameObject);
			}
			this.dialogHandler = null;
			BrowserNative.zfb_destoryBrowser(this.browserId);
			if (this.textureIsOurs)
			{
				UnityEngine.Object.Destroy(this.texture);
			}
			this.browserId = 0;
			this.texture = null;
		}

		protected void OnApplicationQuit()
		{
			this.OnDestroy();
			if (BrowserNative.zfb_numBrowsers() == 0)
			{
				for (int i = 0; i < 10; i++)
				{
					BrowserNative.zfb_tick();
					Thread.Sleep(10);
				}
				BrowserNative.UnloadNative();
			}
		}

		public string Url
		{
			get
			{
				if (!this.IsReady)
				{
					return string.Empty;
				}
				this.CheckSanity();
				IntPtr intPtr = BrowserNative.zfb_getURL(this.browserId);
				string result = Marshal.PtrToStringAnsi(intPtr);
				BrowserNative.zfb_free(intPtr);
				return result;
			}
			set
			{
				this.LoadURL(value, true);
			}
		}

		public void LoadURL(string url, bool force)
		{
			if (this.DeferUnready(delegate
			{
				this.LoadURL(url, force);
			}))
			{
				return;
			}
			if (url.StartsWith("localGame://"))
			{
				url = Browser.LocalUrlPrefix + url.Substring("localGame://".Length);
			}
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentException("URL must be non-empty", "value");
			}
			this.loadPending = true;
			BrowserNative.zfb_goToURL(this.browserId, url, force);
		}

		public void LoadHTML(string html, string url = null)
		{
			if (this.DeferUnready(delegate
			{
				this.LoadHTML(html, url);
			}))
			{
				return;
			}
			this.loadPending = true;
			if (string.IsNullOrEmpty(url))
			{
				url = Browser.LocalUrlPrefix + "custom";
			}
			if (string.IsNullOrEmpty(this.Url))
			{
				this.Url = "about:blank";
				this.skipNextLoad = true;
			}
			BrowserNative.zfb_goToHTML(this.browserId, html, url);
		}

		public void SendFrameCommand(BrowserNative.FrameCommand command)
		{
			if (this.DeferUnready(delegate
			{
				this.SendFrameCommand(command);
			}))
			{
				return;
			}
			BrowserNative.zfb_sendCommandToFocusedFrame(this.browserId, command);
		}

		public void QueuePageReplacer(Action replacePage, float priority)
		{
			if (this.pageReplacer == null || priority >= this.pageReplacerPriority)
			{
				this.pageReplacer = replacePage;
				this.pageReplacerPriority = priority;
			}
		}

		public bool CanGoBack
		{
			get
			{
				if (!this.IsReady)
				{
					return false;
				}
				this.CheckSanity();
				return BrowserNative.zfb_canNav(this.browserId, -1);
			}
		}

		public void GoBack()
		{
			if (!this.IsReady)
			{
				return;
			}
			this.CheckSanity();
			BrowserNative.zfb_doNav(this.browserId, -1);
		}

		public bool CanGoForward
		{
			get
			{
				if (!this.IsReady)
				{
					return false;
				}
				this.CheckSanity();
				return BrowserNative.zfb_canNav(this.browserId, 1);
			}
		}

		public void GoForward()
		{
			if (!this.IsReady)
			{
				return;
			}
			this.CheckSanity();
			BrowserNative.zfb_doNav(this.browserId, 1);
		}

		public bool IsLoadingRaw
		{
			get
			{
				return this.IsReady && BrowserNative.zfb_isLoading(this.browserId);
			}
		}

		public bool IsLoaded
		{
			get
			{
				if (!this.IsReady || this.loadPending)
				{
					return false;
				}
				if (BrowserNative.zfb_isLoading(this.browserId))
				{
					return false;
				}
				string url = this.Url;
				bool flag = string.IsNullOrEmpty(url) || url == "about:blank";
				return !flag;
			}
		}

		public void Stop()
		{
			if (!this.IsReady)
			{
				return;
			}
			this.CheckSanity();
			BrowserNative.zfb_changeLoading(this.browserId, BrowserNative.LoadChange.LC_STOP);
		}

		public void Reload(bool force = false)
		{
			if (!this.IsReady)
			{
				return;
			}
			this.CheckSanity();
			if (force)
			{
				BrowserNative.zfb_changeLoading(this.browserId, BrowserNative.LoadChange.LC_FORCE_RELOAD);
			}
			else
			{
				BrowserNative.zfb_changeLoading(this.browserId, BrowserNative.LoadChange.LC_RELOAD);
			}
		}

		public void ShowDevTools(bool show = true)
		{
			if (this.DeferUnready(delegate
			{
				this.ShowDevTools(show);
			}))
			{
				return;
			}
			BrowserNative.zfb_showDevTools(this.browserId, show);
		}

		public Vector2 Size
		{
			get
			{
				return new Vector2((float)this._width, (float)this._height);
			}
		}

		protected void _Resize(Texture2D newTexture, bool newTextureIsOurs)
		{
			int width = newTexture.width;
			int height = newTexture.height;
			if (this.textureIsOurs && this.texture && newTexture != this.texture)
			{
				UnityEngine.Object.Destroy(this.texture);
			}
			this._width = width;
			this._height = height;
			if (this.IsReady)
			{
				BrowserNative.zfb_resize(this.browserId, width, height);
			}
			else
			{
				this.WhenReady(delegate
				{
					BrowserNative.zfb_resize(this.browserId, width, height);
				});
			}
			this.texture = newTexture;
			this.textureIsOurs = newTextureIsOurs;
			Renderer component = base.GetComponent<Renderer>();
			if (component)
			{
				component.material.mainTexture = this.texture;
			}
			this.afterResize(this.texture);
			if (this.overlay)
			{
				this.overlay.Resize(this.Texture);
			}
		}

		public void Resize(int width, int height)
		{
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, this.generateMipmap);
			if (this.generateMipmap)
			{
				texture2D.filterMode = FilterMode.Trilinear;
			}
			texture2D.wrapMode = TextureWrapMode.Clamp;
			this._Resize(texture2D, true);
		}

		public void Resize(Texture2D newTexture)
		{
			this._Resize(newTexture, false);
		}

		public float Zoom
		{
			get
			{
				return this._zoom;
			}
			set
			{
				if (this.DeferUnready(delegate
				{
					this.Zoom = value;
				}))
				{
					return;
				}
				BrowserNative.zfb_setZoom(this.browserId, (double)value);
				this._zoom = value;
			}
		}

		public IPromise<JSONNode> EvalJS(string script, string scriptURL = "scripted command")
		{
			Promise<JSONNode> promise = new Promise<JSONNode>();
			int id = this.nextCallbackId++;
			string asJSON = new JSONNode(script).AsJSON;
			string resultJS = string.Concat(new object[]
			{
				"try {_zfb_event(",
				id,
				", JSON.stringify(eval(",
				asJSON,
				" )) || 'null');} catch(ex) {_zfb_event(",
				id,
				", 'fail-' + (JSON.stringify(ex.stack) || 'null'));}"
			});
			this.registeredCallbacks.Add(id, delegate(JSONNode val, bool isError)
			{
				this.registeredCallbacks.Remove(id);
				if (isError)
				{
					promise.Reject(new JSException(val));
				}
				else
				{
					promise.Resolve(val);
				}
			});
			if (!this.IsLoaded)
			{
				this.WhenLoaded(delegate
				{
					this._EvalJS(resultJS, scriptURL);
				});
			}
			else
			{
				this._EvalJS(resultJS, scriptURL);
			}
			return promise;
		}

		protected void _EvalJS(string script, string scriptURL)
		{
			BrowserNative.zfb_evalJS(this.browserId, script, scriptURL);
		}

		public IPromise<JSONNode> CallFunction(string name, params JSONNode[] arguments)
		{
			string text = name + "(";
			string str = string.Empty;
			foreach (JSONNode jsonnode in arguments)
			{
				text = text + str + jsonnode.AsJSON;
				str = ", ";
			}
			text += ");";
			return this.EvalJS(text, "scripted command");
		}

		public void RegisterFunction(string name, Browser.JSCallback callback)
		{
			int num = this.nextCallbackId++;
			this.registeredCallbacks.Add(num, delegate(JSONNode value, bool error)
			{
				callback(value);
			});
			string script = string.Concat(new object[]
			{
				name,
				" = function() { _zfb_event(",
				num,
				", JSON.stringify(Array.prototype.slice.call(arguments))); };"
			});
			this.EvalJS(script, "scripted command");
		}

		protected void ProcessCallbacks()
		{
			while (this.thingsToDo.Count != 0)
			{
				object obj = this.thingsToDo;
				lock (obj)
				{
					this.thingsToDoClone.AddRange(this.thingsToDo);
					this.thingsToDo.Clear();
				}
				foreach (Action action in this.thingsToDoClone)
				{
					action();
				}
				this.thingsToDoClone.Clear();
			}
		}

		protected void Update()
		{
			this.ProcessCallbacks();
			if (this.browserId == 0)
			{
				this.RequestNativeBrowser(0);
				return;
			}
			this.CheckSanity();
			this.HandleInput();
		}

		protected void LateUpdate()
		{
			if (Browser.lastUpdateFrame != Time.frameCount)
			{
				BrowserNative.zfb_tick();
				Browser.lastUpdateFrame = Time.frameCount;
			}
			this.ProcessCallbacks();
			if (this.pageReplacer != null)
			{
				this.pageReplacer();
				this.pageReplacer = null;
			}
			if (this.browserId == 0)
			{
				return;
			}
			if (this.EnableRendering)
			{
				this.Render();
			}
		}

		protected void Render()
		{
			this.CheckSanity();
			BrowserNative.RenderData renderData = BrowserNative.zfb_getImage(this.browserId, this.forceNextRender);
			this.forceNextRender = false;
			if (renderData.pixels == IntPtr.Zero)
			{
				return;
			}
			if (renderData.w != this.texture.width || renderData.h != this.texture.height)
			{
				return;
			}
			if (this.pixelData == null || this.pixelData.Length != renderData.w * renderData.h)
			{
				this.pixelData = new Color32[renderData.w * renderData.h];
			}
			GCHandle gchandle = GCHandle.Alloc(this.pixelData, GCHandleType.Pinned);
			BrowserNative.zfb_memcpy(gchandle.AddrOfPinnedObject(), renderData.pixels, this.pixelData.Length * 4);
			gchandle.Free();
			this.texture.SetPixels32(this.pixelData);
			this.texture.Apply(true);
		}

		public void SetOverlay(Browser overlayBrowser)
		{
			if (this.DeferUnready(delegate
			{
				this.SetOverlay(overlayBrowser);
			}))
			{
				return;
			}
			if (overlayBrowser && overlayBrowser.DeferUnready(delegate
			{
				this.SetOverlay(overlayBrowser);
			}))
			{
				return;
			}
			BrowserNative.zfb_setOverlay(this.browserId, (!overlayBrowser) ? 0 : overlayBrowser.browserId);
			this.overlay = overlayBrowser;
			if (!this.overlay)
			{
				return;
			}
			if (!this.overlay.Texture || this.overlay.Texture.width != this.Texture.width || this.overlay.Texture.height != this.Texture.height)
			{
				this.overlay.Resize(this.Texture);
			}
		}

		protected void HandleInput()
		{
			if (this._uiHandler == null || !this.EnableInput)
			{
				return;
			}
			this.CheckSanity();
			this.browserInput.HandleInput();
		}

		public void UpdateCursor()
		{
			if (this.UIHandler == null)
			{
				return;
			}
			if (this.DeferUnready(new Action(this.UpdateCursor)))
			{
				return;
			}
			int num;
			int num2;
			BrowserNative.CursorType cursorType = BrowserNative.zfb_getMouseCursor(this.browserId, out num, out num2);
			if (cursorType != BrowserNative.CursorType.Custom)
			{
				this.UIHandler.BrowserCursor.SetActiveCursor(cursorType);
			}
			else
			{
				if (num == 0 && num2 == 0)
				{
					this.UIHandler.BrowserCursor.SetActiveCursor(BrowserNative.CursorType.None);
					return;
				}
				Color32[] array = new Color32[num * num2];
				GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				int num3;
				int num4;
				BrowserNative.zfb_getMouseCustomCursor(this.browserId, gchandle.AddrOfPinnedObject(), num, num2, out num3, out num4);
				gchandle.Free();
				Texture2D texture2D = new Texture2D(num, num2, TextureFormat.ARGB32, false);
				texture2D.SetPixels32(array);
				this.UIHandler.BrowserCursor.SetCustomCursor(texture2D, new Vector2((float)num3, (float)num4));
				UnityEngine.Object.DestroyImmediate(texture2D);
			}
		}

		private static int lastUpdateFrame;

		protected IBrowserUI _uiHandler;

		protected bool uiHandlerAssigned;

		[Tooltip("Initial URL to load.\n\nTo change at runtime use browser.Url to load a page.")]
		[SerializeField]
		private string _url = string.Empty;

		[Tooltip("Initial size.\n\nTo change at runtime use browser.Resize.")]
		[SerializeField]
		private int _width = 512;

		[Tooltip("Initial size.\n\nTo change at runtime use browser.Resize.")]
		[SerializeField]
		private int _height = 512;

		[Tooltip("Generate mipmaps?\r\n\r\nGenerating mipmaps tends to be somewhat expensive, especially when updating a large texture every frame. Instead of\r\ngenerating mipmaps, try using one of the \"emulate mipmap\" shader variants.\r\n\r\nTo change at runtime modify this value and call browser.Resize.")]
		public bool generateMipmap;

		[Tooltip("Initial background color to use for pages.\r\nYou may pick any opaque color. As a special case, if alpha == 0 the entire background will be rendered transparent.\r\n(Be sure to use a transparent-capable shader to see it.)\r\n\r\nThis value cannot be changed after the first InputUpdate() tick, create a new browser if you need a different option.")]
		public Color32 backgroundColor = new Color32(0, 0, 0, 0);

		[Tooltip("Initial browser \"zoom level\". Negative numbers are smaller, zero is normal, positive numbers are larger.\r\nThe size roughly doubles/halves for every four units added/removed.\r\nNote that zoom level is shared by all pages on the some domain.\r\nAlso note that this zoom level may be persisted across runs.\r\n\r\nTo change at runtime use browser.Zoom.")]
		[SerializeField]
		private float _zoom;

		[Tooltip("Allow right-clicking to show a context menu on what parts of the page?\r\n\r\nMay be changed at any time.\r\n")]
		[FlagsField]
		public BrowserNative.ContextMenuOrigin allowContextMenuOn = BrowserNative.ContextMenuOrigin.Editable;

		[Tooltip("What should we do when a user/the page tries to open a new window?\r\n\r\nFor \"New Browser\" to work, you need to assign NewWindowHandler to a handler of your creation.\r\n")]
		public Browser.NewWindowAction newWindowAction = Browser.NewWindowAction.Redirect;

		protected internal int browserId;

		private int unsafeBrowserId;

		protected bool browserIdRequested;

		protected Texture2D texture;

		protected bool textureIsOurs;

		protected bool forceNextRender = true;

		protected List<Action> thingsToDo = new List<Action>();

		protected List<Action> onloadActions = new List<Action>();

		protected List<object> thingsToRemember = new List<object>();

		protected static Dictionary<int, List<object>> allThingsToRemember = new Dictionary<int, List<object>>();

		private int nextCallbackId = 1;

		protected Dictionary<int, Browser.JSResultFunc> registeredCallbacks = new Dictionary<int, Browser.JSResultFunc>();

		private BrowserInput browserInput;

		private Browser overlay;

		private bool skipNextLoad;

		private bool loadPending;

		protected DialogHandler dialogHandler;

		private Action pageReplacer;

		private float pageReplacerPriority;

		protected List<Action> thingsToDoClone = new List<Action>();

		private Color32[] pixelData;

		[Flags]
		public enum NewWindowAction
		{
			Ignore = 1,
			Redirect = 2,
			NewBrowser = 3,
			NewWindow = 4
		}

		public delegate void JSCallback(JSONNode args);

		protected delegate void JSResultFunc(JSONNode value, bool isError);
	}
}
