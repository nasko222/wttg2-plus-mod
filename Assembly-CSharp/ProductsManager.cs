using System;
using System.Collections.Generic;
using UnityEngine;

public class ProductsManager : MonoBehaviour
{
	public List<ZeroDayProductDefinition> ZeroDayProducts
	{
		get
		{
			return this.zeroDayProducts;
		}
	}

	public List<ShadowMarketProductDefinition> ShadowMarketProducts
	{
		get
		{
			return this.shadowMarketProducts;
		}
	}

	public Transform ShippedProductParent
	{
		get
		{
			return this.shippedProductsParent;
		}
	}

	public void ActivateZeroDayProduct(ZeroDayProductDefinition TheProduct)
	{
		if (SteamSlinger.Ins != null)
		{
			SteamSlinger.Ins.AddPurchasedProduct();
		}
		ZeroDayProductData zeroDayProductData = DataManager.Load<ZeroDayProductData>((int)TheProduct.productID);
		TheProduct.productIsInstalling = false;
		if (!TheProduct.unlimtedUse)
		{
			TheProduct.productOwned = true;
			if (zeroDayProductData != null)
			{
				zeroDayProductData.Owned = true;
			}
		}
		else
		{
			TheProduct.productInventory++;
			if (zeroDayProductData != null)
			{
				zeroDayProductData.InventoryCount++;
			}
		}
		if (zeroDayProductData != null)
		{
			zeroDayProductData.Installing = false;
			DataManager.Save<ZeroDayProductData>(zeroDayProductData);
		}
		SteamSlinger.Ins.ActivateZeroDayProduct(TheProduct.GetHashCode());
		GameManager.ManagerSlinger.AppManager.ActivateApp(TheProduct);
	}

	public void ActivateShadowMarketProduct(ShadowMarketProductDefinition ProductToActivate)
	{
		if (SteamSlinger.Ins != null)
		{
			SteamSlinger.Ins.AddPurchasedProduct();
		}
		ShadowMarketProductData shadowMarketProductData = DataManager.Load<ShadowMarketProductData>((int)ProductToActivate.productID);
		ProductToActivate.productCurrentInventoryCount++;
		ProductToActivate.productIsShipped = false;
		ProductToActivate.productIsPending = false;
		InventoryManager.AddProduct(ProductToActivate);
		if (ProductToActivate.productHasLimitPurchases && InventoryManager.GetProductCount(ProductToActivate.productID) >= ProductToActivate.productMaxPurchaseAmount)
		{
			ProductToActivate.productOwned = true;
		}
		if (shadowMarketProductData != null)
		{
			shadowMarketProductData.Owned = ProductToActivate.productOwned;
			shadowMarketProductData.InventoryCount = ProductToActivate.productCurrentInventoryCount;
			shadowMarketProductData.Pending = ProductToActivate.productIsPending;
			shadowMarketProductData.Shipped = ProductToActivate.productIsShipped;
			DataManager.Save<ShadowMarketProductData>(shadowMarketProductData);
		}
		SteamSlinger.Ins.ActivateShadowMarketProduct(ProductToActivate.GetHashCode());
		this.ShadowMarketProductWasActivated.Execute(ProductToActivate);
	}

	public void ShipProduct(ShadowMarketProductDefinition TheProduct)
	{
		ShippedProductObject shippedProductObject = this.shippedProductPool.Pop();
		shippedProductObject.ProductPickUp.Event += this.productWasPickedUp;
		shippedProductObject.DroneDeliver(TheProduct);
		this.currentShippedProducts.Add(shippedProductObject);
		ShadowMarketProductData shadowMarketProductData = DataManager.Load<ShadowMarketProductData>((int)TheProduct.productID);
		if (shadowMarketProductData != null)
		{
			shadowMarketProductData.Pending = TheProduct.productIsPending;
			shadowMarketProductData.Shipped = TheProduct.productIsShipped;
			DataManager.Save<ShadowMarketProductData>(shadowMarketProductData);
		}
	}

	public void MarkShadowProductAsPending(ShadowMarketProductDefinition TheProduct)
	{
		ShadowMarketProductData shadowMarketProductData = DataManager.Load<ShadowMarketProductData>((int)TheProduct.productID);
		if (shadowMarketProductData != null)
		{
			shadowMarketProductData.Pending = true;
			DataManager.Save<ShadowMarketProductData>(shadowMarketProductData);
		}
	}

	public void MarkZeroDayProductAsInstalling(ZeroDayProductDefinition TheProduct)
	{
		ZeroDayProductData zeroDayProductData = DataManager.Load<ZeroDayProductData>((int)TheProduct.productID);
		if (zeroDayProductData != null)
		{
			zeroDayProductData.Installing = true;
			DataManager.Save<ZeroDayProductData>(zeroDayProductData);
		}
	}

	private void directShipProduct(ShadowMarketProductDefinition TheProduct)
	{
		ShippedProductObject shippedProductObject = this.shippedProductPool.Pop();
		shippedProductObject.ProductPickUp.Event += this.productWasPickedUp;
		shippedProductObject.ShipMe(TheProduct, this.shippedProductPOS, this.shippedProductROT);
		this.currentShippedProducts.Add(shippedProductObject);
	}

	private void productWasPickedUp(ShippedProductObject TheProduct)
	{
		TheProduct.ProductPickUp.Event -= this.productWasPickedUp;
		if (this.currentShippedProducts.Count >= 2)
		{
			SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.DOUBLEUP);
		}
		this.ProductWasPickedUp.Execute();
		this.currentShippedProducts.Remove(TheProduct);
		this.shippedProductPool.Push(TheProduct);
	}

	private void softwareProductWasRemoved(SOFTWARE_PRODUCTS ProductID)
	{
		if (ProductID == SOFTWARE_PRODUCTS.BACKDOOR)
		{
			int index = 0;
			for (int i = 0; i < this.zeroDayProducts.Count; i++)
			{
				if (this.zeroDayProducts[i].productID == SOFTWARE_PRODUCTS.BACKDOOR)
				{
					index = i;
					i = this.zeroDayProducts.Count;
				}
			}
			this.zeroDayProducts[index].productInventory = this.zeroDayProducts[index].productInventory - 1;
			ZeroDayProductData zeroDayProductData = DataManager.Load<ZeroDayProductData>(7);
			if (zeroDayProductData != null)
			{
				zeroDayProductData.InventoryCount--;
				DataManager.Save<ZeroDayProductData>(zeroDayProductData);
			}
		}
	}

	private void stageMe()
	{
		for (int i = 0; i < this.shadowMarketProducts.Count; i++)
		{
			ShadowMarketProductData shadowMarketProductData = DataManager.Load<ShadowMarketProductData>((int)this.shadowMarketProducts[i].productID);
			if (shadowMarketProductData == null)
			{
				shadowMarketProductData = new ShadowMarketProductData((int)this.shadowMarketProducts[i].productID);
				shadowMarketProductData.Owned = false;
				shadowMarketProductData.InventoryCount = 0;
				shadowMarketProductData.Pending = false;
				shadowMarketProductData.Shipped = false;
			}
			this.shadowMarketProducts[i].productOwned = shadowMarketProductData.Owned;
			this.shadowMarketProducts[i].productCurrentInventoryCount = shadowMarketProductData.InventoryCount;
			this.shadowMarketProducts[i].productIsPending = shadowMarketProductData.Pending;
			this.shadowMarketProducts[i].productIsShipped = shadowMarketProductData.Shipped;
			if (shadowMarketProductData.InventoryCount > 0)
			{
				InventoryManager.AddProduct(this.shadowMarketProducts[i]);
				for (int j = 0; j < shadowMarketProductData.InventoryCount; j++)
				{
					this.ShadowMarketProductWasActivated.Execute(this.shadowMarketProducts[i]);
				}
			}
			if (shadowMarketProductData.Shipped)
			{
				this.directShipProduct(this.shadowMarketProducts[i]);
			}
			DataManager.Save<ShadowMarketProductData>(shadowMarketProductData);
		}
		for (int k = 0; k < this.zeroDayProducts.Count; k++)
		{
			ZeroDayProductData zeroDayProductData = DataManager.Load<ZeroDayProductData>((int)this.zeroDayProducts[k].productID);
			if (zeroDayProductData == null)
			{
				zeroDayProductData = new ZeroDayProductData((int)this.zeroDayProducts[k].productID);
				zeroDayProductData.InventoryCount = 0;
				zeroDayProductData.Owned = false;
				zeroDayProductData.Installing = false;
			}
			this.zeroDayProducts[k].productOwned = zeroDayProductData.Owned;
			this.zeroDayProducts[k].productIsInstalling = zeroDayProductData.Installing;
			this.zeroDayProducts[k].productInventory = zeroDayProductData.InventoryCount;
			if (this.zeroDayProducts[k].unlimtedUse)
			{
				if (zeroDayProductData.InventoryCount > 0)
				{
					InventoryManager.AddProduct(this.zeroDayProducts[k]);
				}
			}
			else if (this.zeroDayProducts[k].productOwned)
			{
				GameManager.ManagerSlinger.AppManager.ActivateApp(this.zeroDayProducts[k]);
			}
			DataManager.Save<ZeroDayProductData>(zeroDayProductData);
		}
		if (DataManager.LeetMode)
		{
			this.ActivateZeroDayProduct(this.zeroDayProducts[3]);
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		GameManager.ManagerSlinger.ProductsManager = this;
		this.shippedProductPool = new PooledStack<ShippedProductObject>(delegate()
		{
			ShippedProductObject component = UnityEngine.Object.Instantiate<GameObject>(this.shippedProductObject, this.shippedProductsParent).GetComponent<ShippedProductObject>();
			component.SoftBuild();
			return component;
		}, this.SHIPPED_PRODUCT_POOL_COUNT);
		for (int i = 0; i < this.zeroDayProducts.Count; i++)
		{
			SteamSlinger.Ins.AddZeroDayProduct(this.zeroDayProducts[i].GetHashCode());
		}
		for (int j = 0; j < this.shadowMarketProducts.Count; j++)
		{
			SteamSlinger.Ins.AddShadowMarketProduct(this.shadowMarketProducts[j].GetHashCode());
		}
		GameManager.StageManager.Stage += this.stageMe;
		InventoryManager.RemovedSoftwareProduct.Event += this.softwareProductWasRemoved;
	}

	private void OnDestroy()
	{
		InventoryManager.RemovedSoftwareProduct.Event -= this.softwareProductWasRemoved;
	}

	public CustomEvent<ShadowMarketProductDefinition> ShadowMarketProductWasActivated = new CustomEvent<ShadowMarketProductDefinition>(10);

	public CustomEvent ProductWasPickedUp = new CustomEvent(2);

	[SerializeField]
	private int SHIPPED_PRODUCT_POOL_COUNT = 5;

	[SerializeField]
	private Transform shippedProductsParent;

	[SerializeField]
	private GameObject shippedProductObject;

	[SerializeField]
	private Vector3 shippedProductPOS;

	[SerializeField]
	private Vector3 shippedProductROT;

	[SerializeField]
	private List<ZeroDayProductDefinition> zeroDayProducts;

	[SerializeField]
	private List<ShadowMarketProductDefinition> shadowMarketProducts;

	private PooledStack<ShippedProductObject> shippedProductPool;

	private List<ShippedProductObject> currentShippedProducts = new List<ShippedProductObject>(10);

	public static bool ownsWhitehatScanner;

	public static bool ownsWhitehatDongle2;

	public static bool ownsWhitehatDongle3;

	public static bool ownsWhitehatRemoteVPN2;

	public static bool ownsWhitehatRemoteVPN3;

	public static bool ownsWhitehatRouter;
}
