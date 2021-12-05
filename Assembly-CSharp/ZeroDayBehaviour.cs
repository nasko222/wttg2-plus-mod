using System;
using System.Collections.Generic;
using UnityEngine;

public class ZeroDayBehaviour : WindowBehaviour
{
	public void RefreshProducts()
	{
		for (int i = 0; i < this.currentProducts.Count; i++)
		{
			this.currentProducts[i].GetComponent<ZeroDayProductObject>().RefreshMe(this.myProducts[i]);
		}
	}

	protected override void OnLaunch()
	{
		if (this.productIsInstalling)
		{
			this.productIsInstalling = false;
			return;
		}
		if (!this.productsAreBuilt)
		{
			this.buildMyProducts();
		}
	}

	protected override void OnClose()
	{
		bool flag = false;
		for (int i = 0; i < this.myProducts.Count; i++)
		{
			if (this.myProducts[i].productIsInstalling)
			{
				flag = true;
				i = this.myProducts.Count;
			}
		}
		if (flag)
		{
			this.productIsInstalling = true;
		}
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

	private void buildMyProducts()
	{
		this.productsContentHolderSize.x = this.productsContentHolder.sizeDelta.x;
		this.productsContentHolderSize.y = (float)this.myProducts.Count * 126f;
		Vector2 zero = Vector2.zero;
		for (int i = 0; i < this.myProducts.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.zeroDayProductObject, this.productsContentHolder.GetComponent<RectTransform>());
			gameObject.GetComponent<ZeroDayProductObject>().BuildMe(this.myProducts[i]);
			gameObject.GetComponent<ZeroDayProductObject>().RefreshProducts += this.RefreshProducts;
			zero.y = -((float)i * 126f);
			gameObject.GetComponent<RectTransform>().anchoredPosition = zero;
			this.currentProducts.Add(gameObject);
		}
		this.productsContentHolder.sizeDelta = this.productsContentHolderSize;
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
		this.productsHolder = LookUp.DesktopUI.ZERO_DAY_PRODUCTS_HOLDER;
		this.productsContentHolder = LookUp.DesktopUI.ZERO_DAY_PRODUCTS_CONTENT_HOLDER;
		this.zeroDayProductObject = LookUp.DesktopUI.ZERO_DAY_PRODUCT_OBJECT;
		this.offLineHolder = LookUp.DesktopUI.ZERO_DAY_OFF_LINE_HOLDER;
		this.myProducts = GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts;
		GameManager.ManagerSlinger.WifiManager.WentOffline += this.setMeOffline;
		GameManager.ManagerSlinger.WifiManager.WentOnline += this.setMeOnline;
	}

	protected new void Start()
	{
		base.Start();
		this.SpeedItem();
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
	}

	private void SpeedItem()
	{
		ZeroDayProductDefinition zeroDayProductDefinition = new ZeroDayProductDefinition();
		zeroDayProductDefinition.id = 6303;
		zeroDayProductDefinition.installTime = 8f;
		zeroDayProductDefinition.isDiscounted = false;
		zeroDayProductDefinition.productDesc = "Do you have really high ping? This script may help you boost up your internet speed by 3 times. Does not apply if you have active speed powerup.";
		zeroDayProductDefinition.productID = SOFTWARE_PRODUCTS.SPEED_POWERUP;
		zeroDayProductDefinition.productName = "P1NG_B005T.EXE";
		zeroDayProductDefinition.productSprite = CustomSpriteLookUp.speeditem;
		zeroDayProductDefinition.productRequiresOtherProduct = false;
		zeroDayProductDefinition.productToOwn = null;
		zeroDayProductDefinition.unlimtedUse = true;
		if (ModsManager.EasyModeActive)
		{
			zeroDayProductDefinition.productPrice = 30f;
			this.myProducts.Add(zeroDayProductDefinition);
			return;
		}
		if (ModsManager.DOSTwitchActive)
		{
			zeroDayProductDefinition.productRequiresOtherProduct = true;
			zeroDayProductDefinition.productToOwn = GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[3];
		}
		zeroDayProductDefinition.productPrice = 50f;
		this.myProducts.Add(zeroDayProductDefinition);
	}

	private void KeyItem()
	{
		ZeroDayProductDefinition zeroDayProductDefinition = new ZeroDayProductDefinition();
		zeroDayProductDefinition.id = 6304;
		zeroDayProductDefinition.installTime = 24f;
		zeroDayProductDefinition.isDiscounted = false;
		zeroDayProductDefinition.productDesc = "You want a key cue, but you don't wanna spend a lot money on? This is a demo version active for 10 minutes, to try in A.N.N. Does not apply if you have active key cue powerup.";
		zeroDayProductDefinition.productID = SOFTWARE_PRODUCTS.KEY_POWERUP;
		zeroDayProductDefinition.productName = "Temporary Key Cue";
		zeroDayProductDefinition.productSprite = CustomSpriteLookUp.keyitem;
		zeroDayProductDefinition.productRequiresOtherProduct = false;
		zeroDayProductDefinition.productToOwn = null;
		zeroDayProductDefinition.unlimtedUse = true;
		if (ModsManager.EasyModeActive)
		{
			zeroDayProductDefinition.productPrice = 80f;
			this.myProducts.Add(zeroDayProductDefinition);
			return;
		}
		if (ModsManager.DOSTwitchActive)
		{
			zeroDayProductDefinition.productRequiresOtherProduct = true;
			zeroDayProductDefinition.productToOwn = GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[3];
		}
		zeroDayProductDefinition.productPrice = 145f;
		this.myProducts.Add(zeroDayProductDefinition);
	}

	private GameObject productsHolder;

	private RectTransform productsContentHolder;

	private GameObject zeroDayProductObject;

	private GameObject offLineHolder;

	private List<ZeroDayProductDefinition> myProducts;

	private bool isMin;

	private bool productsAreBuilt;

	private bool productIsInstalling;

	private List<GameObject> currentProducts = new List<GameObject>();

	private Vector2 productsContentHolderSize = Vector2.zero;
}
