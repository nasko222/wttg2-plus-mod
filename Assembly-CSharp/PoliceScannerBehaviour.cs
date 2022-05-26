using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioHubObject))]
[RequireComponent(typeof(InteractionHook))]
public class PoliceScannerBehaviour : MonoBehaviour
{
	public bool IsOn
	{
		get
		{
			return this.scannerIsActive;
		}
	}

	public void SoftBuild()
	{
		PoliceScannerBehaviour.Ins = this;
		this.myMeshRenderer = base.GetComponent<MeshRenderer>();
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myAudioHub = base.GetComponent<AudioHubObject>();
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.myMeshRenderer.enabled = false;
		this.myInteractionHook.LeftClickAction += this.leftClickAction;
		GameManager.PauseManager.GamePaused += this.playerHitPause;
		GameManager.PauseManager.GameUnPaused += this.playerHitUnPause;
		this.myAudioHub.DonePlaying += this.donePlayingClip;
	}

	public void MoveMe(Vector3 SetPOS, Vector3 SetROT)
	{
		this.owned = true;
		this.myMeshRenderer.enabled = true;
		this.scannerIsActive = true;
		base.transform.position = SetPOS;
		base.transform.rotation = Quaternion.Euler(SetROT);
		this.generateFireWindow();
		this.scannerIsActive = true;
		this.emsMat.EnableKeyword("_EMISSION");
		EnemyManager.PoliceManager.FireWarning += this.TriggerWarning;
	}

	public void TriggerWarning()
	{
		if (!this.scannerIsActive)
		{
			return;
		}
		for (int i = 0; i < this.currentPlayingClips.Count; i++)
		{
			GameManager.AudioSlinger.UniKillSound(this.currentPlayingClips[i]);
			this.myAudioHub.KillSound(this.currentPlayingClips[i].AudioClip);
		}
		int num = UnityEngine.Random.Range(1, this.warningSFXS.Length);
		this.myAudioHub.PlaySound(this.warningSFXS[num]);
		BlueWhisperManager.Ins.ProcessSound(this.warningSFXS[num]);
		this.currentPlayingClips.Add(this.warningSFXS[num]);
		AudioFileDefinition audioFileDefinition = this.warningSFXS[num];
		this.warningSFXS[num] = this.warningSFXS[0];
		this.warningSFXS[0] = audioFileDefinition;
	}

	private void playerHitPause()
	{
		this.freezeTimeStamp = Time.time;
		this.gameIsPaused = true;
	}

	private void playerHitUnPause()
	{
		this.freezeAddTime += Time.time - this.freezeTimeStamp;
		this.gameIsPaused = false;
	}

	private void leftClickAction()
	{
		this.toggleScanner();
	}

	private void toggleScanner()
	{
		if (this.scannerIsActive)
		{
			this.scannerIsActive = false;
			this.emsMat.DisableKeyword("_EMISSION");
			for (int i = 0; i < this.currentPlayingClips.Count; i++)
			{
				GameManager.AudioSlinger.UniKillSound(this.currentPlayingClips[i]);
				this.myAudioHub.KillSound(this.currentPlayingClips[i].AudioClip);
			}
			this.myAudioHub.PlaySound(this.offSFX);
			return;
		}
		this.myAudioHub.PlaySound(this.onSFX);
		this.generateFireWindow();
		this.scannerIsActive = true;
		this.emsMat.EnableKeyword("_EMISSION");
	}

	private void generateFireWindow()
	{
		this.fireTimeWindow = UnityEngine.Random.Range(this.fireWindowMin, this.fireWindowMax);
		this.fireTimeStamp = Time.time;
	}

	private void triggerPoliceBanter()
	{
		if (ModsManager.PoliceScannerMod)
		{
			return;
		}
		int num = UnityEngine.Random.Range(1, this.policeBanterSFXS.Length);
		this.myAudioHub.PlaySound(this.policeBanterSFXS[num]);
		BlueWhisperManager.Ins.ProcessSound(this.policeBanterSFXS[num]);
		this.currentPlayingClips.Add(this.policeBanterSFXS[num]);
		AudioFileDefinition audioFileDefinition = this.policeBanterSFXS[num];
		this.policeBanterSFXS[num] = this.policeBanterSFXS[0];
		this.policeBanterSFXS[0] = audioFileDefinition;
	}

	private void donePlayingClip(AudioFileDefinition TheClip)
	{
		this.currentPlayingClips.Remove(TheClip);
	}

	private void Update()
	{
		if (!this.gameIsPaused && this.scannerIsActive && Time.time - this.freezeAddTime - this.fireTimeStamp >= this.fireTimeWindow)
		{
			this.generateFireWindow();
			this.triggerPoliceBanter();
		}
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.leftClickAction;
		GameManager.PauseManager.GamePaused -= this.playerHitPause;
		GameManager.PauseManager.GameUnPaused -= this.playerHitUnPause;
		this.myAudioHub.DonePlaying -= this.donePlayingClip;
	}

	public bool ownPoliceScanner
	{
		get
		{
			return this.owned;
		}
	}

	public static PoliceScannerBehaviour Ins;

	public AudioFileDefinition onSFX;

	public AudioFileDefinition offSFX;

	[SerializeField]
	private AudioFileDefinition[] policeBanterSFXS;

	[SerializeField]
	private AudioFileDefinition[] warningSFXS;

	[SerializeField]
	private float fireWindowMin = 10f;

	[SerializeField]
	private float fireWindowMax = 20f;

	[SerializeField]
	private Material emsMat;

	private MeshRenderer myMeshRenderer;

	private InteractionHook myInteractionHook;

	private AudioHubObject myAudioHub;

	private List<AudioFileDefinition> currentPlayingClips = new List<AudioFileDefinition>(3);

	private bool scannerIsActive;

	private bool gameIsPaused;

	private bool owned;

	private float fireTimeWindow;

	private float fireTimeStamp;

	private float freezeTimeStamp;

	private float freezeAddTime;
}
