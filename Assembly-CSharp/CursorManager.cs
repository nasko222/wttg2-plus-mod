using System;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
	public void SetOverwrite(bool setValue)
	{
		this.overwrite = setValue;
		if (this.overwrite)
		{
			this.EnableCursor();
		}
		else
		{
			this.DisableCursor();
		}
	}

	public void EnableCursor()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		this.cursorIsDisabled = false;
	}

	public void DisableCursor()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		this.cursorIsDisabled = true;
	}

	public void SwitchToDefaultCursor()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}

	public void SwitchToCustomCursor()
	{
		if (!this.hackerCursorActive)
		{
			Cursor.SetCursor(this.defaultCursor, new Vector2(12f, 12f), CursorMode.Auto);
		}
		else
		{
			this.SwitchToHackerCursor();
		}
	}

	public void SwitchToHackerCursor()
	{
		this.hackerCursorActive = true;
		Cursor.SetCursor(this.hackerCursor, new Vector2(16f, 16f), CursorMode.Auto);
	}

	public void ClearHackerCursor()
	{
		this.hackerCursorActive = false;
		this.SwitchToCustomCursor();
	}

	public void ResizeCursorState(bool active)
	{
		if (active)
		{
			Cursor.SetCursor(this.resizeCursor, new Vector2(12f, 12f), CursorMode.Auto);
		}
		else
		{
			this.SwitchToCustomCursor();
		}
	}

	public void PointerCursorState(bool active)
	{
		if (active)
		{
			Cursor.SetCursor(this.pointerCursor, new Vector2(12f, 12f), CursorMode.Auto);
		}
		else
		{
			this.SwitchToCustomCursor();
		}
	}

	private void Awake()
	{
		CursorManager.Ins = this;
		if (!this.skipGameManager)
		{
			GameManager.ManagerSlinger.CursorManager = this;
		}
		if (this.hideCursorOnStart)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			this.cursorIsDisabled = true;
			this.overwrite = false;
		}
		else
		{
			this.SetOverwrite(true);
			this.EnableCursor();
		}
	}

	private void Update()
	{
		if (!this.overwrite && !this.skipGameManager)
		{
			if (StateManager.GameState == GAME_STATE.PAUSED)
			{
				this.EnableCursor();
			}
			else
			{
				this.DisableCursor();
			}
		}
	}

	private void OnDestroy()
	{
		CursorManager.Ins = null;
	}

	[SerializeField]
	private Texture2D defaultCursor;

	[SerializeField]
	private Texture2D resizeCursor;

	[SerializeField]
	private Texture2D pointerCursor;

	[SerializeField]
	private Texture2D hackerCursor;

	[SerializeField]
	private bool skipGameManager;

	public static CursorManager Ins;

	public bool hideCursorOnStart;

	public bool cursorIsDisabled;

	public bool overwrite;

	public bool hackerCursorActive;
}
