using System;
using UnityEngine;
using UnityEngine.UI;

public class ShadowProductObject : MonoBehaviour
{
	public void RefreshMe()
	{
		if (!this.iAmBusy)
		{
			this.stockValue.text = "Stock: " + (this.myProduct.productMaxPurchaseAmount - InventoryManager.GetProductCount(this.myProduct.productID));
			if (this.myProduct.productOwned)
			{
				this.productsBTN.SetToOwned();
				return;
			}
			this.productsBTN.SetToBuy();
			if (this.myProduct.productIsShipped)
			{
				this.productsBTN.SetToShipped();
			}
			if (this.myProduct.productRequiresOtherProduct && !this.myProduct.productToOwn.productOwned)
			{
				this.productsBTN.SetToDisabled();
			}
		}
	}

	public void BuildMe(ShadowMarketProductDefinition SetProduct)
	{
		this.myProduct = SetProduct;
		this.productTitle.text = this.myProduct.productName;
		this.productPrice.text = this.myProduct.productPrice.ToString();
		this.productDesc.text = this.myProduct.productDesc;
		this.stockValue.text = "Stock: " + (this.myProduct.productMaxPurchaseAmount - InventoryManager.GetProductCount(this.myProduct.productID));
		this.productsBTN.SetDefaults();
		if (this.myProduct.productSprite != null)
		{
			this.productIMG.sprite = this.myProduct.productSprite;
		}
		if (this.myProduct.productOwned)
		{
			this.productsBTN.SetToOwned();
			return;
		}
		if (this.myProduct.productIsPending)
		{
			this.shipItem();
		}
		else if (this.myProduct.productIsShipped)
		{
			this.productsBTN.SetToShipped();
		}
		if (this.myProduct.productRequiresOtherProduct && !this.myProduct.productToOwn.productOwned)
		{
			this.productsBTN.SetToDisabled();
		}
		this.productsBTN.BuyItem += this.buyItem;
		this.productsBTN.CantBuy += this.cantBuyItem;
		this.productsBTN.ShipItem += this.shipItem;
	}

	private bool buyItem()
	{
		return CurrencyManager.PurchaseItem(this.myProduct);
	}

	private void cantBuyItem()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.CantBuyItem);
		this.productPrice.color = this.tooMuchPriceColor;
		GameManager.TimeSlinger.FireTimer(3f, delegate()
		{
			this.productPrice.color = this.defaultPriceColor;
		}, 0);
	}

	public void shipItem()
	{
		float num = UnityEngine.Random.Range(this.myProduct.deliveryTimeMin, this.myProduct.deliveryTimeMax);
		this.iAmBusy = true;
		this.myProduct.productIsPending = true;
		this.productsBTN.ShipAni(num);
		GameManager.ManagerSlinger.ProductsManager.MarkShadowProductAsPending(this.myProduct);
		GameManager.TimeSlinger.FireTimer(num, delegate()
		{
			this.iAmBusy = false;
			this.myProduct.productIsPending = false;
			this.myProduct.productIsShipped = true;
			this.productsBTN.SetToShipped();
			GameManager.ManagerSlinger.ProductsManager.ShipProduct(this.myProduct);
		}, 0);
	}

	private void OnDestroy()
	{
		this.productsBTN.BuyItem -= this.buyItem;
		this.productsBTN.CantBuy -= this.cantBuyItem;
		this.productsBTN.ShipItem -= this.shipItem;
	}

	public void DiscountMe()
	{
		this.myProduct.productPrice *= 0.75f;
		this.productPrice.GetComponent<Text>().text = this.myProduct.productPrice.ToString();
		this.productPrice.GetComponent<Text>().color = Color.blue;
		this.defaultPriceColor = Color.blue;
		this.myProduct.isDiscounted = true;
		ShadowProductObject.isDiscountOn = true;
	}

	private void Awake()
	{
		this.myProduct.myProductObject = this;
	}

	public void UnDiscountMe()
	{
		this.myProduct.productPrice /= 0.75f;
		this.productPrice.GetComponent<Text>().text = this.myProduct.productPrice.ToString();
		this.productPrice.GetComponent<Text>().color = Color.black;
		this.defaultPriceColor = Color.black;
		this.myProduct.isDiscounted = false;
		ShadowProductObject.isDiscountOn = false;
	}

	[SerializeField]
	private Text productTitle;

	[SerializeField]
	private Text productPrice;

	[SerializeField]
	private Text productDesc;

	[SerializeField]
	private Text stockValue;

	[SerializeField]
	private Image productIMG;

	[SerializeField]
	private ShadowMarketProductsBTNBehaviour productsBTN;

	[SerializeField]
	private Color defaultPriceColor;

	[SerializeField]
	private Color tooMuchPriceColor;

	private ShadowMarketProductDefinition myProduct;

	private bool iAmBusy;

	public static bool isDiscountOn;
}
