using System;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class RemoteVPNObject : MonoBehaviour
{
	public Vector3 SpawnLocation
	{
		get
		{
			return this.spawnLocation;
		}
	}

	public Vector3 SpawnRotation
	{
		get
		{
			return this.spawnRotation;
		}
	}

	public Vector3 CurrentLocation
	{
		get
		{
			return base.transform.position;
		}
	}

	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	public bool Placed
	{
		get
		{
			return this.amPlaced;
		}
	}

	public string DOSCoinValue
	{
		get
		{
			return this.myCurrency.GenerateDOSCoinValue.ToString();
		}
	}

	public string TimeValue
	{
		get
		{
			return this.myCurrency.GenerateTime.ToString();
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event RemoteVPNObject.RemoteVPNPlacement EnteredPlacementMode;

	public void SoftBuild()
	{
		this.myMeshRenderer = base.GetComponent<MeshRenderer>();
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.LeftClickAction += this.enterPlacementMode;
		this.myMeshRenderer.enabled = false;
		this.myInteractionHook.ForceLock = true;
		RemoteVPNObject.ObjectBuilt = true;
		RemoteVPNObject.defaultMaterialColor = this.myMeshRenderer.material.color;
		GameManager.PauseManager.GamePaused += this.playerHitPause;
		GameManager.PauseManager.GameUnPaused += this.playerHitUnPause;
	}

	public void SpawnMe(Vector3 SetPosition, Vector3 SetRotation)
	{
		this.amPlaced = false;
		base.transform.position = SetPosition;
		base.transform.rotation = Quaternion.Euler(SetRotation);
		this.spawnLocation = SetPosition;
		this.spawnRotation = SetRotation;
		this.myMeshRenderer.enabled = true;
		this.myInteractionHook.ForceLock = false;
	}

	public void PlaceMe(Vector3 SetPosition, Vector3 SetRotation)
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPutDown1);
		UIInventoryManager.HideRemoteVPN();
		this.amPlaced = true;
		base.transform.position = SetPosition;
		base.transform.rotation = Quaternion.Euler(SetRotation);
		this.myMeshRenderer.enabled = true;
		this.myInteractionHook.ForceLock = false;
		GameManager.WorldManager.SetVPNValues(this);
		if (this.IWasPlaced != null)
		{
			this.IWasPlaced(this);
		}
		this.generateCurrencyActive = true;
	}

	public void SetPlaceMe(SerTrans WhereTo)
	{
		this.amPlaced = true;
		base.transform.position = WhereTo.PositionToVector3;
		base.transform.rotation = Quaternion.Euler(WhereTo.RotationToVector3);
		this.myMeshRenderer.enabled = true;
		this.myInteractionHook.ForceLock = false;
		GameManager.WorldManager.SetVPNValues(this);
		this.generateCurrencyActive = true;
	}

	public void SetCurrency(VPNCurrencyData SetValue)
	{
		this.myCurrency = SetValue;
	}

	public void SetCurrency(VPNCurrencyDefinition SetValue)
	{
		this.myCurrency = new VPNCurrencyData(SetValue.GenerateTime, SetValue.GenerateDOSCoinValue);
	}

	private void enterPlacementMode()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPickUp1);
		UIInventoryManager.ShowRemoteVPN();
		this.amPlaced = false;
		this.myMeshRenderer.enabled = false;
		this.myInteractionHook.ForceLock = true;
		if (this.EnteredPlacementMode != null)
		{
			this.EnteredPlacementMode(this);
		}
	}

	private void playerHitPause()
	{
		this.generateCurrencyActive = false;
	}

	private void playerHitUnPause()
	{
		this.generateCurrencyActive = true;
	}

	private void Update()
	{
		if (this.amPlaced && this.generateCurrencyActive)
		{
			int remoteVPNLevel = RemoteVPNObject.RemoteVPNLevel;
			if (remoteVPNLevel != 2)
			{
				if (remoteVPNLevel != 3)
				{
					CurrencyManager.AddPendingCurrency(Mathf.Lerp(0f, this.myCurrency.GenerateDOSCoinValue, Time.deltaTime / this.myCurrency.GenerateTime));
				}
				else
				{
					CurrencyManager.AddPendingCurrency(Mathf.Lerp(0f, this.myCurrency.GenerateDOSCoinValue * 3f, Time.deltaTime / this.myCurrency.GenerateTime));
				}
			}
			else
			{
				CurrencyManager.AddPendingCurrency(Mathf.Lerp(0f, this.myCurrency.GenerateDOSCoinValue * 2f, Time.deltaTime / this.myCurrency.GenerateTime));
			}
			this.UpdateMaterial();
		}
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.enterPlacementMode;
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event RemoteVPNObject.RemoteVPNPlacement IWasPlaced;

	private void UpdateMaterial()
	{
		int remoteVPNLevel = RemoteVPNObject.RemoteVPNLevel;
		if (remoteVPNLevel == 2)
		{
			this.myMeshRenderer.material.color = new Color(139f, 0f, 139f);
			return;
		}
		if (remoteVPNLevel != 3)
		{
			this.myMeshRenderer.material.color = RemoteVPNObject.defaultMaterialColor;
			return;
		}
		this.myMeshRenderer.material.color = new Color(0f, 106f, 209f);
	}

	private void Awake()
	{
	}

	private VPNCurrencyData myCurrency;

	private InteractionHook myInteractionHook;

	private MeshRenderer myMeshRenderer;

	private Vector3 spawnLocation = Vector3.zero;

	private Vector3 spawnRotation = Vector3.zero;

	private bool amPlaced;

	private bool generateCurrencyActive;

	public static int RemoteVPNLevel;

	private static Color defaultMaterialColor;

	public static bool ObjectBuilt;

	public delegate void RemoteVPNPlacement(RemoteVPNObject TheRemoteVPN);
}
