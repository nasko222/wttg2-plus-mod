using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class backdoorTextHook : MonoBehaviour
{
	private void softwareProductWasAdded(SOFTWARE_PRODUCTS ProductID)
	{
		this.myText.text = InventoryManager.GetProductCount(SOFTWARE_PRODUCTS.BACKDOOR).ToString();
	}

	private void softwareProductWasRemoved(SOFTWARE_PRODUCTS ProductID)
	{
		this.myText.text = InventoryManager.GetProductCount(SOFTWARE_PRODUCTS.BACKDOOR).ToString();
	}

	private void gameLive()
	{
		this.myText.text = InventoryManager.GetProductCount(SOFTWARE_PRODUCTS.BACKDOOR).ToString();
		GameManager.StageManager.TheGameIsLive -= this.gameLive;
	}

	private void Awake()
	{
		this.myText = base.GetComponent<Text>();
		GameManager.StageManager.TheGameIsLive += this.gameLive;
		InventoryManager.AddedSoftwareProduct.Event += this.softwareProductWasAdded;
		InventoryManager.RemovedSoftwareProduct.Event += this.softwareProductWasRemoved;
	}

	private void OnDestroy()
	{
		InventoryManager.AddedSoftwareProduct.Event -= this.softwareProductWasAdded;
		InventoryManager.RemovedSoftwareProduct.Event -= this.softwareProductWasRemoved;
	}

	private Text myText;
}
