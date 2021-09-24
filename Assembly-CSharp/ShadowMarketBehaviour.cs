using System;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMarketBehaviour : WindowBehaviour
{
	protected override void OnLaunch()
	{
		if (!this.productsAreBuilt)
		{
			this.buildMyProducts();
		}
	}

	protected override void OnClose()
	{
	}

	protected override void OnMin()
	{
	}

	protected override void OnUnMin()
	{
	}

	protected override void OnMax()
	{
	}

	protected override void OnUnMax()
	{
	}

	protected override void OnResized()
	{
	}

	private void productWasPickedUp(ShadowMarketProductDefinition TheProduct)
	{
		for (int i = 0; i < this.currentProducts.Count; i++)
		{
			this.currentProducts[i].RefreshMe();
		}
	}

	private void buildMyProducts()
	{
		Vector2 sizeDelta = new Vector2(this.productsContentHolder.sizeDelta.x, (float)this.myProducts.Count * 126f);
		Vector2 zero = Vector2.zero;
		for (int i = 0; i < this.myProducts.Count; i++)
		{
			ShadowProductObject component = UnityEngine.Object.Instantiate<GameObject>(this.shadowMarketProductObject, this.productsContentHolder).GetComponent<ShadowProductObject>();
			component.BuildMe(this.myProducts[i]);
			zero.y = -((float)i * 126f);
			component.gameObject.GetComponent<RectTransform>().anchoredPosition = zero;
			this.currentProducts.Add(component);
		}
		this.productsContentHolder.sizeDelta = sizeDelta;
		this.productsAreBuilt = true;
	}

	private void setMeOffline()
	{
		this.offLineHolder.SetActive(true);
		this.productsHolder.SetActive(false);
	}

	private void setMeOnline()
	{
		this.offLineHolder.SetActive(false);
		this.productsHolder.SetActive(true);
	}

	protected new void Awake()
	{
		base.Awake();
		this.productsHolder = LookUp.DesktopUI.SHADOW_MARKET_PRODUCTS_HOLDER;
		this.productsContentHolder = LookUp.DesktopUI.SHADOW_MARKET_PRODUCTS_CONTENT_HOLDER;
		this.shadowMarketProductObject = LookUp.DesktopUI.SHADOW_MARKET_PRODUCT_OBJECT;
		this.offLineHolder = LookUp.DesktopUI.SHADOW_MARKET_OFF_LINE_HOLDER;
		this.myProducts = GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts;
		GameManager.ManagerSlinger.WifiManager.WentOffline += this.setMeOffline;
		GameManager.ManagerSlinger.WifiManager.WentOnline += this.setMeOnline;
		GameManager.ManagerSlinger.ProductsManager.ShadowMarketProductWasActivated.Event += this.productWasPickedUp;
	}

	protected new void Start()
	{
		base.Start();
		this.addRemoteVPNLevel2();
		this.addRemoteVPNLevel3();
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
	}

	private void addRemoteVPNLevel2()
	{
		ShadowMarketProductDefinition shadowMarketProductDefinition = new ShadowMarketProductDefinition();
		shadowMarketProductDefinition.deliveryTimeMin = 105f;
		shadowMarketProductDefinition.deliveryTimeMax = 115f;
		shadowMarketProductDefinition.id = 6301;
		shadowMarketProductDefinition.isDiscounted = false;
		shadowMarketProductDefinition.productDesc = "Looking to score even more DOS coins? We got you covered! This upgraded device will use an advanced script to accquire 2x DOS coins when placed. Does upgrade any previously purchased VPNs.";
		shadowMarketProductDefinition.productHasLimitPurchases = true;
		shadowMarketProductDefinition.productID = HARDWARE_PRODUCTS.REMOTE_VPN_LEVEL2;
		shadowMarketProductDefinition.productMaxPurchaseAmount = 1;
		shadowMarketProductDefinition.productName = "Remote VPN Level 2";
		shadowMarketProductDefinition.productRequiresOtherProduct = false;
		shadowMarketProductDefinition.productSprite = DownloadTIFiles.RemoteVPNLevel2;
		if (ModsManager.EasyModeActive)
		{
			shadowMarketProductDefinition.productPrice = 50f;
		}
		else
		{
			shadowMarketProductDefinition.productPrice = 75f;
		}
		ShadowMarketBehaviour.vpn2 = shadowMarketProductDefinition;
		this.myProducts.Add(shadowMarketProductDefinition);
	}

	private void addRemoteVPNLevel3()
	{
		ShadowMarketProductDefinition shadowMarketProductDefinition = new ShadowMarketProductDefinition();
		shadowMarketProductDefinition.deliveryTimeMin = 165f;
		shadowMarketProductDefinition.deliveryTimeMax = 195f;
		shadowMarketProductDefinition.id = 6302;
		shadowMarketProductDefinition.isDiscounted = false;
		shadowMarketProductDefinition.productDesc = "Are you even more desperate for that cash? Are you trying to never run out of money in these hard times while wanting to do absolutely nothing? Look no further with this top tier performance!";
		shadowMarketProductDefinition.productHasLimitPurchases = true;
		shadowMarketProductDefinition.productID = HARDWARE_PRODUCTS.REMOTE_VPN_LEVEL3;
		shadowMarketProductDefinition.productMaxPurchaseAmount = 1;
		shadowMarketProductDefinition.productName = "Remote VPN Level 3";
		shadowMarketProductDefinition.productRequiresOtherProduct = true;
		shadowMarketProductDefinition.productToOwn = ShadowMarketBehaviour.vpn2;
		shadowMarketProductDefinition.productSprite = DownloadTIFiles.RemoteVPNLevel3;
		if (ModsManager.EasyModeActive)
		{
			shadowMarketProductDefinition.productPrice = 80f;
		}
		else
		{
			shadowMarketProductDefinition.productPrice = 120f;
		}
		this.myProducts.Add(shadowMarketProductDefinition);
	}

	private GameObject productsHolder;

	private RectTransform productsContentHolder;

	private GameObject shadowMarketProductObject;

	private GameObject offLineHolder;

	private List<ShadowMarketProductDefinition> myProducts = new List<ShadowMarketProductDefinition>();

	private List<ShadowProductObject> currentProducts = new List<ShadowProductObject>();

	private bool productsAreBuilt;

	private static ShadowMarketProductDefinition vpn2;
}
