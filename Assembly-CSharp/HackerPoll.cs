using System;
using System.Collections.Generic;
using UnityEngine;

public class HackerPoll
{
	public void BeginVote()
	{
		this.currentVotes = new Dictionary<string, string>();
		this.myDOSTwitch.myTwitchIRC.SendMsg("HACKERMANS Poll");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Who will win, black hats or white hats?");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!WHITEHAT");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!BLACKHAT");
		this.voteIsLive = true;
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd), 0);
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "WHITEHAT" || text == "BLACKHAT"))
			{
				this.currentVotes.Add(userName, text);
			}
		}
	}

	private void PollEnd()
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		this.voteIsLive = false;
		this.myDOSTwitch.myTwitchIRC.SendMsg("The HACKERMANS Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "WHITEHAT")
			{
				num++;
			}
			else if (keyValuePair.Value == "BLACKHAT")
			{
				num2++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("WHITEHAT: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("BLACKHAT: " + num2.ToString());
		if (num == 0 && num2 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted, Story of my life!");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		if (num > num2)
		{
			if (InventoryManager.GetProductCount(SOFTWARE_PRODUCTS.BACKDOOR) <= 0)
			{
				this.myDOSTwitch.myTwitchIRC.SendMsg("Whitehats won! The player does not have a backdoor hack, no DOS Coins earned.");
			}
			else if (UnityEngine.Random.Range(0, 100) > ((DataManager.LeetMode || ModsManager.Nightmare) ? 90 : 75))
			{
				InventoryManager.RemoveProduct(SOFTWARE_PRODUCTS.BACKDOOR);
				if (!ProductsManager.ownsWhitehatScanner && !PoliceScannerBehaviour.Ins.ownPoliceScanner && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 7].myProductObject.myProduct.productIsPending && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 7].myProductObject.myProduct.productIsShipped)
				{
					WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
					ProductsManager.ownsWhitehatScanner = true;
					GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 7].myProductObject.shipItem();
					this.myDOSTwitch.myTwitchIRC.SendMsg("Whitehats won! The player just unlocked POLICE SCANNER.");
				}
				else if (!ProductsManager.ownsWhitehatRemoteVPN2 && RemoteVPNObject.RemoteVPNLevel == 1 && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 4].myProductObject.myProduct.productIsPending && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 4].myProductObject.myProduct.productIsShipped)
				{
					WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
					ProductsManager.ownsWhitehatRemoteVPN2 = true;
					GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 4].myProductObject.shipItem();
					this.myDOSTwitch.myTwitchIRC.SendMsg("Whitehats won! The player just unlocked REMOTE VPN LEVEL 2.");
				}
				else if (!ProductsManager.ownsWhitehatRouter && !RouterBehaviour.Ins.Owned && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 1].myProductObject.myProduct.productIsPending && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 1].myProductObject.myProduct.productIsShipped)
				{
					WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
					ProductsManager.ownsWhitehatRouter = true;
					GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 1].myProductObject.shipItem();
					this.myDOSTwitch.myTwitchIRC.SendMsg("Whitehats won! The player just unlocked ROUTER.");
				}
				else if (!ProductsManager.ownsWhitehatDongle2 && InventoryManager.WifiDongleLevel == WIFI_DONGLE_LEVEL.LEVEL1 && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 11].myProductObject.myProduct.productIsPending && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 11].myProductObject.myProduct.productIsShipped)
				{
					WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
					ProductsManager.ownsWhitehatDongle2 = true;
					GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 11].myProductObject.shipItem();
					this.myDOSTwitch.myTwitchIRC.SendMsg("Whitehats won! The player just unlocked WIFI DONGLE LEVEL 2.");
				}
				else
				{
					this.myDOSTwitch.myTwitchIRC.SendMsg("Whitehats won! The player got 2.5 DOS Coins from that hack.");
					GameManager.HackerManager.WhiteHatSound();
					CurrencyManager.AddCurrency(2.5f);
				}
			}
			else
			{
				InventoryManager.RemoveProduct(SOFTWARE_PRODUCTS.BACKDOOR);
				float setAMT;
				if (UnityEngine.Random.Range(0, 10) > 7)
				{
					setAMT = UnityEngine.Random.Range(3.5f, 133.7f);
				}
				else if (UnityEngine.Random.Range(0, 100) > 90)
				{
					setAMT = 3.5f;
				}
				else
				{
					setAMT = UnityEngine.Random.Range(3.5f, 33.7f);
				}
				this.myDOSTwitch.myTwitchIRC.SendMsg("Whitehats won! The player got " + setAMT.ToString() + " DOS Coins from that hack.");
				GameManager.HackerManager.WhiteHatSound();
				CurrencyManager.AddCurrency(setAMT);
			}
		}
		else if (num2 > num)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("The BLACKHATS Have Won!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Launching 1337 Difficulty hack! HA HA HA HA!!!!");
			GameManager.HackerManager.ForceTwitchHack();
		}
		else
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("There is a tie! RE-VOTE!");
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.BeginVote), 0);
			flag = true;
		}
		if (!flag)
		{
			this.myDOSTwitch.setPollInactive();
		}
	}

	public DOSTwitch myDOSTwitch;

	private Dictionary<string, string> currentVotes;

	private bool voteIsLive;
}
