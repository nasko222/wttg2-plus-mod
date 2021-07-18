using System;
using UnityEngine;
using UnityEngine.UI;

public class ZeroDayProductObject : MonoBehaviour
{
	public event ZeroDayProductObject.VoidActions RefreshProducts;

	public void BuildMe(ZeroDayProductDefinition myProduct)
	{
		this.myProduct = myProduct;
		this.ProductTitle.GetComponent<Text>().text = myProduct.productName;
		this.ProductPrice.GetComponent<Text>().text = myProduct.productPrice.ToString();
		this.ProductDesc.GetComponent<Text>().text = myProduct.productDesc;
		if (myProduct.productSprite != null)
		{
			this.ProductIMG.GetComponent<Image>().sprite = myProduct.productSprite;
		}
		if (myProduct.productOwned)
		{
			this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().SetToOwned();
		}
		else
		{
			if (myProduct.productRequiresOtherProduct && !myProduct.productToOwn.productOwned)
			{
				this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().SetToDisabled();
			}
			this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().BuyItem += this.BuyItem;
			this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().InstallItem += this.InstallItem;
			this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().CantBuy += this.CantBuyItem;
		}
		if (myProduct.productIsInstalling)
		{
			this.InstallItem();
		}
	}

	public void RefreshMe(ZeroDayProductDefinition updatedProduct)
	{
		this.myProduct = updatedProduct;
		if (!this.iAmBusy)
		{
			this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().SetToBuy();
			if (this.myProduct.productOwned)
			{
				this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().SetToOwned();
				return;
			}
			if (this.myProduct.productRequiresOtherProduct && !this.myProduct.productToOwn.productOwned)
			{
				this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().SetToDisabled();
			}
		}
	}

	private bool BuyItem()
	{
		return CurrencyManager.PurchaseItem(this.myProduct);
	}

	private void CantBuyItem()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.CantBuyItem);
		this.ProductPrice.GetComponent<Text>().color = this.tooMuchPriceColor;
		GameManager.TimeSlinger.FireTimer(3f, new Action(this.ResetPriceColor), 0);
	}

	private void ResetPriceColor()
	{
		this.ProductPrice.GetComponent<Text>().color = this.defaultPriceColor;
	}

	private void InstallItem()
	{
		this.iAmBusy = true;
		this.myProduct.productIsInstalling = true;
		this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().InstallAni(this.myProduct.installTime);
		GameManager.ManagerSlinger.ProductsManager.MarkZeroDayProductAsInstalling(this.myProduct);
		GameManager.TimeSlinger.FireTimer<ZeroDayProductDefinition>(this.myProduct.installTime - 0.25f, new Action<ZeroDayProductDefinition>(GameManager.ManagerSlinger.ProductsManager.ActivateZeroDayProduct), this.myProduct, 0);
		GameManager.TimeSlinger.FireTimer(this.myProduct.installTime - 0.25f, delegate()
		{
			this.iAmBusy = false;
		}, 0);
		GameManager.TimeSlinger.FireTimer(this.myProduct.installTime, delegate()
		{
			if (this.RefreshProducts != null)
			{
				this.RefreshProducts();
			}
		}, 0);
	}

	private void OnDestroy()
	{
		this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().BuyItem -= this.BuyItem;
		this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().InstallItem -= this.InstallItem;
		this.ProductBTN.GetComponent<ZeroDayProductBTNBehaviour>().CantBuy -= this.CantBuyItem;
	}

	public void DiscountMe()
	{
		this.myProduct.productPrice *= 0.75f;
		this.ProductPrice.GetComponent<Text>().text = this.myProduct.productPrice.ToString();
		this.ProductPrice.GetComponent<Text>().color = Color.blue;
		this.defaultPriceColor = Color.blue;
		this.myProduct.isDiscounted = true;
		ZeroDayProductObject.isDiscountOn = true;
	}

	private void Awake()
	{
		this.myProduct.myProductObject = this;
	}

	public void UnDiscountMe()
	{
		this.myProduct.productPrice /= 0.75f;
		this.ProductPrice.GetComponent<Text>().text = this.myProduct.productPrice.ToString();
		this.ProductPrice.GetComponent<Text>().color = Color.black;
		this.defaultPriceColor = Color.black;
		this.myProduct.isDiscounted = false;
		ZeroDayProductObject.isDiscountOn = false;
	}

	public GameObject ProductTitle;

	public GameObject ProductPrice;

	public GameObject ProductDesc;

	public GameObject ProductIMG;

	public GameObject ProductBTN;

	public Color defaultPriceColor;

	public Color tooMuchPriceColor;

	private ZeroDayProductDefinition myProduct;

	private bool iAmBusy;

	public static bool isDiscountOn;

	public delegate void VoidActions();
}
