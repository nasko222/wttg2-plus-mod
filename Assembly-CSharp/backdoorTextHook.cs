using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class backdoorTextHook : MonoBehaviour
{
	private void softwareProductWasAdded(SOFTWARE_PRODUCTS ProductID)
	{
		this.myText.text = InventoryManager.GetProductCount(SOFTWARE_PRODUCTS.BACKDOOR).ToString();
		if (ProductID == SOFTWARE_PRODUCTS.SPEED_POWERUP)
		{
			SpeedPoll.DevEnableManipulator(TWITCH_NET_SPEED.FAST);
			return;
		}
		if (ProductID == SOFTWARE_PRODUCTS.KEY_POWERUP)
		{
			KeyPoll.DevEnableManipulator(KEY_CUE_MODE.ENABLED);
		}
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
