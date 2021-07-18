using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VPNMenuBehaviour : MonoBehaviour, IPointerExitHandler, IEventSystemHandler
{
	private void rebuildMenu()
	{
		float num;
		if (this.currentVPNMenuObjects.Count > 0)
		{
			num = 4f + ((float)this.currentVPNMenuObjects.Count * 24f + (float)this.currentVPNMenuObjects.Count * 4f);
			if (GameManager.ManagerSlinger.VPNManager.CurrentVPN != VPN_LEVELS.LEVEL0)
			{
				this.sepRT.anchoredPosition = new Vector2(10f, -num);
				this.disRT.anchoredPosition = new Vector2(10f, -(num + 3f));
				num += 4f + this.disRT.sizeDelta.y;
			}
			num += 10f;
			this.noVPNSOwnedCG.alpha = 0f;
		}
		else
		{
			num = 38f;
			this.noVPNSOwnedCG.alpha = 1f;
		}
		Vector2 sizeDelta = new Vector2(this.myRT.sizeDelta.x, num);
		Vector2 anchoredPosition = new Vector2(this.myRT.anchoredPosition.x, num);
		this.myRT.sizeDelta = sizeDelta;
		this.myRT.anchoredPosition = anchoredPosition;
	}

	private void userAddedSoftwareProduct(SOFTWARE_PRODUCTS ProductID)
	{
		switch (ProductID)
		{
		case SOFTWARE_PRODUCTS.VPN_1:
			this.addVPNMenuObject(VPN_LEVELS.LEVEL1);
			break;
		case SOFTWARE_PRODUCTS.VPN_2:
			this.addVPNMenuObject(VPN_LEVELS.LEVEL2);
			break;
		case SOFTWARE_PRODUCTS.VPN_3:
			this.addVPNMenuObject(VPN_LEVELS.LEVEL3);
			break;
		case SOFTWARE_PRODUCTS.VPN_4:
			this.addVPNMenuObject(VPN_LEVELS.LEVEL4);
			break;
		case SOFTWARE_PRODUCTS.VPN_5:
			this.addVPNMenuObject(VPN_LEVELS.LEVEL5);
			break;
		}
	}

	private void addVPNMenuObject(VPN_LEVELS SetLevel)
	{
		VPNMenuObject vpnmenuObject = this.vpnMenuObjectPool.Pop();
		vpnmenuObject.BuildMe(SetLevel, this.currentVPNMenuObjects.Count);
		this.currentVPNMenuObjects.Add(vpnmenuObject);
		this.rebuildMenu();
	}

	private void connectToVPN(VPN_LEVELS SetLevel)
	{
		for (int i = 0; i < this.currentVPNMenuObjects.Count; i++)
		{
			this.currentVPNMenuObjects[i].SetMeConnected(SetLevel);
		}
		if (this.myData != null)
		{
			this.myData.CurrentActiveVPN = (int)SetLevel;
			DataManager.Save<VPNMenuData>(this.myData);
		}
		GameManager.ManagerSlinger.VPNManager.ConnectToVPN(SetLevel);
		this.rebuildMenu();
	}

	private void clearMenuOptions()
	{
		for (int i = 0; i < this.currentVPNMenuObjects.Count; i++)
		{
			this.currentVPNMenuObjects[i].IWasPressed -= this.connectToVPN;
			this.vpnMenuObjectPool.Push(this.currentVPNMenuObjects[i]);
		}
		this.currentVPNMenuObjects.Clear();
		this.vpnMenuObjectPool.Clear();
	}

	private void disconnectedFromVPN()
	{
		for (int i = 0; i < this.currentVPNMenuObjects.Count; i++)
		{
			this.currentVPNMenuObjects[i].SetMeConnected(VPN_LEVELS.LEVEL0);
		}
		this.sepRT.anchoredPosition = new Vector2(0f, 1f);
		this.disRT.anchoredPosition = new Vector2(0f, 24f);
		this.rebuildMenu();
	}

	private void stageMe()
	{
		this.rebuildMenu();
		this.myData = DataManager.Load<VPNMenuData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new VPNMenuData(this.myID);
			this.myData.CurrentActiveVPN = 0;
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void gameLive()
	{
		if (this.myData.CurrentActiveVPN != 0)
		{
			this.connectToVPN((VPN_LEVELS)this.myData.CurrentActiveVPN);
		}
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
	}

	private void Awake()
	{
		this.myID = 52085;
		this.myRT = base.GetComponent<RectTransform>();
		this.vpnMenuObjectPool = new PooledStack<VPNMenuObject>(delegate()
		{
			VPNMenuObject component = UnityEngine.Object.Instantiate<GameObject>(this.VPNMenuObjectPrefab, this.myRT).GetComponent<VPNMenuObject>();
			component.SoftBuild();
			component.IWasPressed += this.connectToVPN;
			return component;
		}, this.VPN_MENU_OBJ_POOL_COUNT);
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.TheGameIsLive += this.gameLive;
		InventoryManager.AddedSoftwareProduct.Event += this.userAddedSoftwareProduct;
		this.disObject.IWasPressed += this.disconnectedFromVPN;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameManager.ManagerSlinger.VPNManager.TriggerVPNMenu();
	}

	private void OnDestroy()
	{
		this.clearMenuOptions();
		InventoryManager.AddedSoftwareProduct.Event -= this.userAddedSoftwareProduct;
		this.disObject.IWasPressed -= this.disconnectedFromVPN;
	}

	[SerializeField]
	private int VPN_MENU_OBJ_POOL_COUNT = 5;

	[SerializeField]
	private GameObject VPNMenuObjectPrefab;

	[SerializeField]
	private CanvasGroup noVPNSOwnedCG;

	[SerializeField]
	private RectTransform sepRT;

	[SerializeField]
	private RectTransform disRT;

	[SerializeField]
	private VPNMenuDisObject disObject;

	private const float OPT_SPACING = 4f;

	private const float BOT_SPACING = 10f;

	private PooledStack<VPNMenuObject> vpnMenuObjectPool;

	private List<VPNMenuObject> currentVPNMenuObjects = new List<VPNMenuObject>(5);

	private RectTransform myRT;

	private int myID;

	private VPNMenuData myData;
}
