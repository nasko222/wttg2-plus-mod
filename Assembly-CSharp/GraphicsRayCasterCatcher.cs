using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class GraphicsRayCasterCatcher : MonoBehaviour
{
	private void gameWasPaused()
	{
		this.myLastState = this.myRayCaster.enabled;
		this.myRayCaster.enabled = false;
	}

	private void gameWasUnPaused()
	{
		this.myRayCaster.enabled = this.myLastState;
	}

	private void Awake()
	{
		this.myRayCaster = base.GetComponent<GraphicRaycaster>();
		GameManager.PauseManager.GamePaused += this.gameWasPaused;
		GameManager.PauseManager.GameUnPaused += this.gameWasUnPaused;
	}

	private void OnDestroy()
	{
		GameManager.PauseManager.GamePaused -= this.gameWasPaused;
		GameManager.PauseManager.GameUnPaused -= this.gameWasUnPaused;
	}

	private GraphicRaycaster myRayCaster;

	private bool myLastState;
}
