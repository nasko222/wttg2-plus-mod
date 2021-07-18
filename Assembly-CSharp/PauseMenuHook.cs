using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class PauseMenuHook : MonoBehaviour
{
	private void playerHitPause()
	{
		this.pauseTween.Restart(true, -1f);
		this.myCG.blocksRaycasts = true;
		this.myCG.interactable = true;
	}

	private void playerHitUnPause()
	{
		this.unPauseTween.Restart(true, -1f);
		this.myCG.blocksRaycasts = false;
		this.myCG.interactable = false;
	}

	private void playerChangedMouseSens(float SetValue)
	{
		int num = Mathf.RoundToInt(SetValue);
		this.mouseSensSlider.value = (float)num;
		this.mouseSensValue.SetText(num.ToString());
		this.UpdatedMouseSens.Execute(num);
		OptionDataHook.Ins.Options.MouseSens = num;
		OptionDataHook.Ins.SaveOptionData();
	}

	private void quitGame()
	{
		Application.Quit();
	}

	private void exitToMainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(1);
	}

	private void Awake()
	{
		PauseMenuHook.Ins = this;
		this.myCG = base.GetComponent<CanvasGroup>();
		this.pauseTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.2f).SetEase(Ease.Linear).SetUpdate(true);
		this.pauseTween.Pause<Tweener>();
		this.pauseTween.SetAutoKill(false);
		this.unPauseTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear).SetUpdate(true);
		this.unPauseTween.Pause<Tweener>();
		this.unPauseTween.SetAutoKill(false);
		this.mouseSensSlider.onValueChanged.AddListener(new UnityAction<float>(this.playerChangedMouseSens));
		this.mouseSensSlider.value = (float)OptionDataHook.Ins.Options.MouseSens;
		this.mouseSensValue.SetText(OptionDataHook.Ins.Options.MouseSens.ToString());
		GameManager.PauseManager.GamePaused += this.playerHitPause;
		GameManager.PauseManager.GameUnPaused += this.playerHitUnPause;
		this.quitGameBTN.MyAction.Event += this.quitGame;
		this.exitToMainMenuBTN.MyAction.Event += this.exitToMainMenu;
	}

	private void OnDestroy()
	{
		PauseMenuHook.Ins = null;
		this.mouseSensSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.playerChangedMouseSens));
		GameManager.PauseManager.GamePaused -= this.playerHitPause;
		GameManager.PauseManager.GameUnPaused -= this.playerHitUnPause;
		this.quitGameBTN.MyAction.Event -= this.quitGame;
		this.exitToMainMenuBTN.MyAction.Event -= this.exitToMainMenu;
	}

	public static PauseMenuHook Ins;

	public CustomEvent<int> UpdatedMouseSens = new CustomEvent<int>(5);

	[SerializeField]
	private Slider mouseSensSlider;

	[SerializeField]
	private TextMeshProUGUI mouseSensValue;

	[SerializeField]
	private TitleMenuBTN quitGameBTN;

	[SerializeField]
	private TitleMenuBTN exitToMainMenuBTN;

	private CanvasGroup myCG;

	private Tweener pauseTween;

	private Tweener unPauseTween;
}
