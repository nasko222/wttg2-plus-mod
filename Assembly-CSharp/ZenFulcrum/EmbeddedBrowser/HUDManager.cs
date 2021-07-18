using System;
using System.Collections;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class HUDManager : MonoBehaviour
	{
		public static HUDManager Instance { get; private set; }

		public Browser HUDBrowser { get; private set; }

		public void Awake()
		{
			HUDManager.Instance = this;
		}

		public void Start()
		{
			this.HUDBrowser = this.hud.GetComponent<Browser>();
			this.HUDBrowser.RegisterFunction("unpause", delegate(JSONNode args)
			{
				this.Unpause();
			});
			this.HUDBrowser.RegisterFunction("browserMode", delegate(JSONNode args)
			{
				this.LoadBrowseLevel(true);
			});
			this.HUDBrowser.RegisterFunction("quit", delegate(JSONNode args)
			{
				Application.Quit();
			});
			this.Unpause();
			PlayerInventory.Instance.coinCollected += delegate(int count)
			{
				this.HUDBrowser.CallFunction("setCoinCount", new JSONNode[]
				{
					count
				});
			};
		}

		private IEnumerator Rehide()
		{
			while (Application.isShowingSplashScreen)
			{
				yield return null;
			}
			Cursor.visible = false;
			yield return new WaitForSeconds(0.2f);
			Cursor.visible = true;
			yield return new WaitForSeconds(0.2f);
			Cursor.visible = false;
			yield break;
		}

		public void Unpause()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			this.EnableUserControls(true);
			Time.timeScale = 1f;
			this.haveMouse = true;
			this.HUDBrowser.CallFunction("setPaused", new JSONNode[]
			{
				false
			});
		}

		public void Pause()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			this.haveMouse = false;
			Time.timeScale = 0f;
			this.EnableUserControls(false);
			this.HUDBrowser.CallFunction("setPaused", new JSONNode[]
			{
				true
			});
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (this.haveMouse)
				{
					this.Pause();
				}
				else
				{
					this.Unpause();
				}
			}
		}

		public void Say(string html, float dwellTime)
		{
			this.HUDBrowser.CallFunction("say", new JSONNode[]
			{
				html,
				dwellTime
			});
		}

		protected void EnableUserControls(bool enableIt)
		{
			FPSCursorRenderer.Instance.EnableInput = enableIt;
			foreach (MouseLook mouseLook in base.GetComponentsInChildren<MouseLook>())
			{
				mouseLook.enabled = enableIt;
			}
			foreach (MouseLook mouseLook2 in base.GetComponentsInChildren<MouseLook>())
			{
				mouseLook2.enabled = enableIt;
			}
			Behaviour behaviour = (Behaviour)base.GetComponentInChildren(Type.GetType("FPSInputController, Assembly-UnityScript"));
			behaviour.enabled = enableIt;
			this.hud.enableInput = !enableIt;
		}

		public void LoadBrowseLevel(bool force = false)
		{
			base.StartCoroutine(this.LoadLevel(force));
		}

		private IEnumerator LoadLevel(bool force = false)
		{
			if (!force)
			{
				yield return new WaitUntil(() => SayWordsOnTouch.ActiveSpeakers == 0);
			}
			this.Pause();
			Application.LoadLevel("SimpleBrowser");
			yield break;
		}

		private bool haveMouse;

		public GUIBrowserUI hud;
	}
}
