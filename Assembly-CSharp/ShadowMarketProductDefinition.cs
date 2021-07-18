using System;
using UnityEngine;

[Serializable]
public class ShadowMarketProductDefinition : Definition
{
	public HARDWARE_PRODUCTS productID;

	public string productName;

	public string productDesc;

	public float productPrice;

	public bool productIsPending;

	public bool productIsShipped;

	public bool productHasLimitPurchases;

	public int productCurrentInventoryCount;

	public int productMaxPurchaseAmount;

	public bool productOwned;

	public bool productRequiresOtherProduct;

	public ShadowMarketProductDefinition productToOwn;

	public Sprite productSprite;

	public float deliveryTimeMin;

	public float deliveryTimeMax;

	public ShadowProductObject myProductObject;

	public bool isDiscounted;
}
