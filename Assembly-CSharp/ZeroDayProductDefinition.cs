using System;
using UnityEngine;

[Serializable]
public class ZeroDayProductDefinition : Definition
{
	public SOFTWARE_PRODUCTS productID;

	public string productName;

	public string productDesc;

	public float productPrice;

	public bool productIsInstalling;

	public bool unlimtedUse;

	public int productInventory;

	public bool productOwned;

	public bool productRequiresOtherProduct;

	public ZeroDayProductDefinition productToOwn;

	public Sprite productSprite;

	public float installTime;

	public ZeroDayProductObject myProductObject;

	public bool isDiscounted;
}
