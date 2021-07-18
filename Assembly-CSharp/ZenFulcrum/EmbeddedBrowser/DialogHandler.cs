using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	[RequireComponent(typeof(Browser))]
	public class DialogHandler : MonoBehaviour
	{
		public static DialogHandler Create(Browser parent, DialogHandler.DialogCallback dialogCallback, DialogHandler.MenuCallback contextCallback)
		{
			if (DialogHandler.dialogPage == null)
			{
				DialogHandler.dialogPage = Resources.Load<TextAsset>("Browser/Dialogs").text;
			}
			GameObject gameObject = new GameObject("Browser Dialog for " + parent.name);
			DialogHandler handler = gameObject.AddComponent<DialogHandler>();
			handler.parentBrowser = parent;
			handler.dialogCallback = dialogCallback;
			Browser browser = handler.dialogBrowser = handler.GetComponent<Browser>();
			browser.UIHandler = parent.UIHandler;
			browser.EnableRendering = false;
			browser.EnableInput = false;
			browser.allowContextMenuOn = BrowserNative.ContextMenuOrigin.Editable;
			browser.Resize(parent.Texture);
			browser.LoadHTML(DialogHandler.dialogPage, "about:dialog");
			browser.UIHandler = parent.UIHandler;
			browser.RegisterFunction("reportDialogResult", delegate(JSONNode args)
			{
				dialogCallback(args[0], args[1], args[3]);
				handler.Hide();
			});
			browser.RegisterFunction("reportContextMenuResult", delegate(JSONNode args)
			{
				contextCallback(args[0]);
				handler.Hide();
			});
			return handler;
		}

		public void HandleDialog(BrowserNative.DialogType type, string text, string promptDefault = null)
		{
			if (type == BrowserNative.DialogType.DLT_HIDE)
			{
				this.Hide();
				return;
			}
			this.Show();
			switch (type)
			{
			case BrowserNative.DialogType.DLT_ALERT:
				this.dialogBrowser.CallFunction("showAlert", new JSONNode[]
				{
					text
				});
				break;
			case BrowserNative.DialogType.DLT_CONFIRM:
				this.dialogBrowser.CallFunction("showConfirm", new JSONNode[]
				{
					text
				});
				break;
			case BrowserNative.DialogType.DLT_PROMPT:
				this.dialogBrowser.CallFunction("showPrompt", new JSONNode[]
				{
					text,
					promptDefault
				});
				break;
			case BrowserNative.DialogType.DLT_PAGE_UNLOAD:
				this.dialogBrowser.CallFunction("showConfirmNav", new JSONNode[]
				{
					text
				});
				break;
			case BrowserNative.DialogType.DLT_PAGE_RELOAD:
				this.dialogBrowser.CallFunction("showConfirmReload", new JSONNode[]
				{
					text
				});
				break;
			case BrowserNative.DialogType.DLT_GET_AUTH:
				this.dialogBrowser.CallFunction("showAuthPrompt", new JSONNode[]
				{
					text
				});
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
		}

		public void Show()
		{
			this.parentBrowser.SetOverlay(this.dialogBrowser);
			this.parentBrowser.EnableInput = false;
			this.dialogBrowser.EnableInput = true;
			this.dialogBrowser.UpdateCursor();
		}

		public void Hide()
		{
			this.parentBrowser.SetOverlay(null);
			this.parentBrowser.EnableInput = true;
			this.dialogBrowser.EnableInput = false;
			this.parentBrowser.UpdateCursor();
			if (this.dialogBrowser.IsLoaded)
			{
				this.dialogBrowser.CallFunction("reset", new JSONNode[0]);
			}
		}

		public void HandleContextMenu(string menuJSON, int x, int y)
		{
			if (menuJSON == null)
			{
				this.Hide();
				return;
			}
			this.Show();
			this.dialogBrowser.CallFunction("showContextMenu", new JSONNode[]
			{
				menuJSON,
				x,
				y
			});
		}

		protected static string dialogPage;

		protected Browser parentBrowser;

		protected Browser dialogBrowser;

		protected DialogHandler.DialogCallback dialogCallback;

		protected DialogHandler.MenuCallback contextCallback;

		public delegate void DialogCallback(bool affirm, string text1, string text2);

		public delegate void MenuCallback(int commandId);
	}
}
