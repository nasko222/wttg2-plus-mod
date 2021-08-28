using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class TenantTrackManager : MonoBehaviour
{
	public void UnLockSystem()
	{
		GameManager.AudioSlinger.PlaySound(this.UnLockSystemSFX);
		this.myLobbyComputerCameraManager.TriggerGlitch();
		GameManager.TimeSlinger.FireTimer(3f, delegate()
		{
			this.myState = TENANT_TRACK_STATE.FLOOR_SELECT;
			this.systemIsLocked = false;
			this.lockInput = false;
			this.systemLockedImage.enabled = false;
			this.showBorderBoxTween.Restart(true, -1f);
		}, 0);
	}

	public bool CheckIfFemaleTenant(int UnitNumber)
	{
		return UnitNumber != 0 && this.tenantLookUp[UnitNumber].canBeTagged;
	}

	private void takeInput()
	{
		if (StateManager.GameState == GAME_STATE.LIVE && !this.systemIsLocked && !this.lockInput && ControllerManager.Get<lobbyComputerController>(GAME_CONTROLLER.LOBBY_COMPUTER).Active)
		{
			TENANT_TRACK_STATE tenant_TRACK_STATE = this.myState;
			if (tenant_TRACK_STATE != TENANT_TRACK_STATE.FLOOR_SELECT)
			{
				if (tenant_TRACK_STATE == TENANT_TRACK_STATE.TENANT_SELECT)
				{
					this.tenantSelectInput();
				}
			}
			else
			{
				this.floorSelectInput();
			}
		}
	}

	private void processKeyboardSFX()
	{
		int num = UnityEngine.Random.Range(1, this.KeyboardSFXS.Length);
		AudioFileDefinition audioFileDefinition = this.KeyboardSFXS[num];
		GameManager.AudioSlinger.PlaySound(audioFileDefinition);
		this.KeyboardSFXS[num] = this.KeyboardSFXS[0];
		this.KeyboardSFXS[0] = audioFileDefinition;
	}

	private void floorSelectInput()
	{
		if (CrossPlatformInputManager.GetButtonDown("Up"))
		{
			this.processKeyboardSFX();
			this.currentFloorIndex--;
			if (this.currentFloorIndex < 0)
			{
				this.currentFloorIndex = 0;
			}
			this.floorSelectObjects[this.currentFloorIndex].ActivateMe();
			this.floorSelectObjects[this.currentFloorIndex + 1].DeActivateMe();
		}
		else if (CrossPlatformInputManager.GetButtonDown("Down"))
		{
			this.processKeyboardSFX();
			this.currentFloorIndex++;
			if (this.currentFloorIndex >= this.floorSelectObjects.Length - 1)
			{
				this.currentFloorIndex = this.floorSelectObjects.Length - 1;
			}
			this.floorSelectObjects[this.currentFloorIndex].ActivateMe();
			this.floorSelectObjects[this.currentFloorIndex - 1].DeActivateMe();
		}
		else if (CrossPlatformInputManager.GetButtonDown("Return"))
		{
			this.processKeyboardSFX();
			this.currentTenantIndex = 0;
			this.presentTenantInfoWindow();
		}
	}

	private void tenantSelectInput()
	{
		if (CrossPlatformInputManager.GetButtonDown("Left"))
		{
			this.processKeyboardSFX();
			this.tenantOptionIndex--;
			if (this.tenantOptionIndex < 0)
			{
				this.tenantOptionIndex = 0;
			}
		}
		else if (CrossPlatformInputManager.GetButtonDown("Right"))
		{
			this.processKeyboardSFX();
			this.tenantOptionIndex++;
			if (this.tenantOptionIndex > 2)
			{
				this.tenantOptionIndex = 2;
			}
		}
		else if (CrossPlatformInputManager.GetButtonDown("Return"))
		{
			this.processKeyboardSFX();
			int num = this.tenantOptionIndex;
			if (num != 0)
			{
				if (num != 1)
				{
					if (num == 2)
					{
						this.currentTenantIndex++;
						if (this.currentTenantIndex > this.tenantData[this.floorSelectObjects[this.currentFloorIndex].FloorNumber].Count - 1)
						{
							this.currentTenantIndex = this.tenantData[this.floorSelectObjects[this.currentFloorIndex].FloorNumber].Count - 1;
						}
						this.updateTenantData();
					}
				}
				else
				{
					this.dismissTenantInfoWindow();
				}
			}
			else
			{
				this.currentTenantIndex--;
				if (this.currentTenantIndex < 0)
				{
					this.currentTenantIndex = 0;
				}
				this.updateTenantData();
			}
		}
		int num2 = this.tenantOptionIndex;
		if (num2 != 0)
		{
			if (num2 != 1)
			{
				if (num2 == 2)
				{
					this.tenantLeftButton.sprite = this.inactiveTenantLeftButtonSprite;
					this.tenantBackButton.sprite = this.inactiveTenantBackButtonSprite;
					this.tenantRightButton.sprite = this.activeTenantRightButtonSprite;
				}
			}
			else
			{
				this.tenantLeftButton.sprite = this.inactiveTenantLeftButtonSprite;
				this.tenantBackButton.sprite = this.activeTenantBackButtonSprite;
				this.tenantRightButton.sprite = this.inactiveTenantRightButtonSprite;
			}
		}
		else
		{
			this.tenantLeftButton.sprite = this.activeTenantLeftButtonSprite;
			this.tenantBackButton.sprite = this.inactiveTenantBackButtonSprite;
			this.tenantRightButton.sprite = this.inactiveTenantRightButtonSprite;
		}
	}

	private void presentTenantInfoWindow()
	{
		this.updateTenantData();
		this.lockInput = true;
		this.myState = TENANT_TRACK_STATE.TENANT_SELECT;
		this.tenantWindowInfoCG.alpha = 1f;
		this.showTenantWindow.Restart(true, -1f);
	}

	private void dismissTenantInfoWindow()
	{
		this.lockInput = true;
		this.myState = TENANT_TRACK_STATE.FLOOR_SELECT;
		this.hideTenantWindow.Restart(true, -1f);
	}

	private void updateTenantData()
	{
		if (this.tenantData.ContainsKey(this.floorSelectObjects[this.currentFloorIndex].FloorNumber) && this.tenantData[this.floorSelectObjects[this.currentFloorIndex].FloorNumber][this.currentTenantIndex] != null)
		{
			this.tenantFloorTitle.text = "FLOOR " + this.floorSelectObjects[this.currentFloorIndex].FloorNumber.ToString();
			this.tenantUnitValue.text = this.tenantData[this.floorSelectObjects[this.currentFloorIndex].FloorNumber][this.currentTenantIndex].tenantUnit.ToString();
			this.tenantNameValue.text = this.tenantData[this.floorSelectObjects[this.currentFloorIndex].FloorNumber][this.currentTenantIndex].tenantName;
			this.tenantAgeValue.text = this.tenantData[this.floorSelectObjects[this.currentFloorIndex].FloorNumber][this.currentTenantIndex].tenantAge.ToString();
			this.tenantNotesValue.text = this.tenantData[this.floorSelectObjects[this.currentFloorIndex].FloorNumber][this.currentTenantIndex].tenantNotes;
		}
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.systemIsLocked = true;
		this.lockInput = true;
		this.currentFloorIndex = 0;
		this.floorSelectObjects[this.currentFloorIndex].ActivateMe();
		this.tenantOptionIndex = 1;
		this.tenantLeftButton.sprite = this.inactiveTenantLeftButtonSprite;
		this.tenantBackButton.sprite = this.activeTenantBackButtonSprite;
		this.tenantRightButton.sprite = this.inactiveTenantRightButtonSprite;
		this.myData = DataManager.Load<TenantTrackData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new TenantTrackData(this.myID);
			this.myData.TenantData = new Dictionary<int, List<TenantData>>();
			for (int i = 0; i < this.tenants.Length; i++)
			{
				this.avaibleTenants.Add(this.tenants[i]);
			}
			for (int j = 0; j < this.floors.Length; j++)
			{
				int k = 0;
				List<TenantData> list = new List<TenantData>();
				while (k < 6)
				{
					if (this.floors[j] == 8 && k == 2)
					{
						TenantData item = new TenantData(this.clintData);
						list.Add(item);
					}
					else if (this.floors[j] != 1 || k != 2)
					{
						int num = UnityEngine.Random.Range(0, this.avaibleTenants.Count);
						TenantDefinition tenantDefinition = this.avaibleTenants[num];
						tenantDefinition.tenantUnit = this.floors[j] * 100 + (k + 1);
						list.Add(new TenantData(tenantDefinition)
						{
							tenantIndex = num
						});
						this.avaibleTenants.RemoveAt(num);
					}
					k++;
				}
				this.myData.TenantData.Add(this.floors[j], list);
			}
			DataManager.Save<TenantTrackData>(this.myData);
		}
		foreach (KeyValuePair<int, List<TenantData>> keyValuePair in this.myData.TenantData)
		{
			for (int l = 0; l < keyValuePair.Value.Count; l++)
			{
				this.tenantLookUp.Add(keyValuePair.Value[l].tenantUnit, keyValuePair.Value[l]);
				this.tenants[keyValuePair.Value[l].tenantIndex].tenantUnit = keyValuePair.Value[l].tenantUnit;
				if (keyValuePair.Value[l].tenantSex == 1 || keyValuePair.Value[l].canBeTagged)
				{
				}
			}
			this.tenantData.Add(keyValuePair.Key, keyValuePair.Value);
		}
		Camera camera;
		CameraManager.Get(CAMERA_ID.LOBBY_COMPUTER, out camera);
		if (camera != null)
		{
			this.myLobbyComputerCameraManager = camera.gameObject.GetComponent<LobbyComputerCameraManager>();
		}
	}

	private void Awake()
	{
		this.myID = base.transform.position.GetHashCode();
		this.lockInput = true;
		GameManager.ManagerSlinger.TenantTrackManager = this;
		GameManager.StageManager.Stage += this.stageMe;
		this.myState = TENANT_TRACK_STATE.LOCKED;
		this.showBorderBoxTween = DOTween.To(() => new Vector3(0f, 0f, 1f), delegate(Vector3 x)
		{
			this.borderBoxRT.localScale = x;
		}, Vector3.one, 0.4f).SetEase(Ease.Linear);
		this.showBorderBoxTween.Pause<Tweener>();
		this.showBorderBoxTween.SetAutoKill(false);
		this.showTenantWindow = DOTween.To(() => new Vector2(800f, 0f), delegate(Vector2 x)
		{
			this.tenantWindowInfoRT.sizeDelta = x;
		}, new Vector2(800f, 500f), 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.lockInput = false;
		});
		this.showTenantWindow.Pause<Tweener>();
		this.showTenantWindow.SetAutoKill(false);
		this.hideTenantWindow = DOTween.To(() => new Vector2(800f, 500f), delegate(Vector2 x)
		{
			this.tenantWindowInfoRT.sizeDelta = x;
		}, new Vector2(800f, 0f), 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.lockInput = false;
			this.tenantWindowInfoCG.alpha = 0f;
		});
		this.hideTenantWindow.Pause<Tweener>();
		this.hideTenantWindow.SetAutoKill(false);
	}

	private void Update()
	{
		this.takeInput();
	}

	public TenantDefinition[] Tenants
	{
		get
		{
			return this.tenants;
		}
	}

	public float CheckDollMakerPrice(int UnitNumber)
	{
		if (this.tenantLookUp[UnitNumber].tenantSex == 0 || this.tenantLookUp[UnitNumber].tenantAge < 18 || this.tenantLookUp[UnitNumber].tenantName == "Kylie Hogan")
		{
			return 135f;
		}
		if ((this.tenantLookUp[UnitNumber].tenantAge >= 18 && this.tenantLookUp[UnitNumber].tenantAge < 30) || this.tenantLookUp[UnitNumber].tenantName == "Carissa Whitehead")
		{
			return 65f;
		}
		return 35f;
	}

	[SerializeField]
	private Image systemLockedImage;

	[SerializeField]
	private RectTransform borderBoxRT;

	[SerializeField]
	private RectTransform tenantWindowInfoRT;

	[SerializeField]
	private CanvasGroup tenantWindowInfoCG;

	[SerializeField]
	private Image tenantLeftButton;

	[SerializeField]
	private Image tenantBackButton;

	[SerializeField]
	private Image tenantRightButton;

	[SerializeField]
	private Sprite inactiveTenantLeftButtonSprite;

	[SerializeField]
	private Sprite inactiveTenantBackButtonSprite;

	[SerializeField]
	private Sprite inactiveTenantRightButtonSprite;

	[SerializeField]
	private Sprite activeTenantLeftButtonSprite;

	[SerializeField]
	private Sprite activeTenantBackButtonSprite;

	[SerializeField]
	private Sprite activeTenantRightButtonSprite;

	[SerializeField]
	private Text tenantFloorTitle;

	[SerializeField]
	private Text tenantUnitValue;

	[SerializeField]
	private Text tenantNameValue;

	[SerializeField]
	private Text tenantAgeValue;

	[SerializeField]
	private Text tenantNotesValue;

	[SerializeField]
	private TenantDefinition clintData;

	[SerializeField]
	private AudioFileDefinition UnLockSystemSFX;

	[SerializeField]
	private AudioFileDefinition[] KeyboardSFXS;

	[SerializeField]
	private TennantTrackFloorSelectObject[] floorSelectObjects = new TennantTrackFloorSelectObject[0];

	[SerializeField]
	private TenantDefinition[] tenants = new TenantDefinition[0];

	private TENANT_TRACK_STATE myState;

	private int currentFloorIndex;

	private int currentTenantIndex;

	private int tenantOptionIndex;

	private bool systemIsLocked;

	private bool lockInput;

	private LobbyComputerCameraManager myLobbyComputerCameraManager;

	private Tweener showBorderBoxTween;

	private Tweener showTenantWindow;

	private Tweener hideTenantWindow;

	private int[] floors = new int[]
	{
		10,
		8,
		6,
		5,
		3,
		1
	};

	private List<TenantDefinition> avaibleTenants = new List<TenantDefinition>();

	private Dictionary<int, List<TenantData>> tenantData = new Dictionary<int, List<TenantData>>();

	private Dictionary<int, TenantData> tenantLookUp = new Dictionary<int, TenantData>();

	private int myID;

	private TenantTrackData myData;
}
