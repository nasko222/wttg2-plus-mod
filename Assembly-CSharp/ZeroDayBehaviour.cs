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
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
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
