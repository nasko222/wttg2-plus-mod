using System;
using System.Collections.Generic;
using UnityEngine;

public class EarlyGamePoll
{
	public void BeginVote()
	{
		this.currentVotes = new Dictionary<string, string>();
		this.myDOSTwitch.myTwitchIRC.SendMsg("Escalation Poll");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Shall the player get free 100 DOSCoins, or release Lucas, or release The Doll Maker, or release The Bomb Maker?");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!DOSCOINS");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!LUCAS");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!DOLLMAKER");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!BOMBMAKER");
		this.voteIsLive = true;
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd), 0);
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "DOSCOINS" || text == "LUCAS" || text == "DOLLMAKER" || text == "BOMBMAKER"))
			{
				this.currentVotes.Add(userName, text);
			}
		}
	}

	private void PollEnd()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		bool flag = false;
		this.voteIsLive = false;
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Escalation Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "DOSCOINS")
			{
				num++;
			}
			else if (keyValuePair.Value == "LUCAS")
			{
				num2++;
			}
			else if (keyValuePair.Value == "DOLLMAKER")
			{
				num3++;
			}
			else if (keyValuePair.Value == "BOMBMAKER")
			{
				num4++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("DOSCOINS: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("LUCAS: " + num2.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("DOLLMAKER: " + num3.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("BOMBMAKER: " + num4.ToString());
		if (num == 0 && num2 == 0 && num3 == 0 && num4 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted, Story of my life!");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		if (num > num2 && num > num3 && num > num4)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("The player got 100 DOS Coins.");
			GameManager.TheCloud.ForceKeyDiscover();
			GameManager.TheCloud.ForceKeyDiscover();
			CurrencyManager.AddCurrency(100f);
			GameManager.HackerManager.WhiteHatSound();
		}
		else if (num2 > num && num2 > num3 && num2 > num4)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("Lucas has been set free!");
			GameManager.TheCloud.ForceKeyDiscover();
			GameManager.TheCloud.ForceKeyDiscover();
			GameManager.TheCloud.ForceKeyDiscover();
			if ((float)(num + num2 + num3 + num4) * 0.75f <= (float)num2 && num2 >= 4)
			{
				this.myDOSTwitch.myTwitchIRC.SendMsg("More than 75% of twitch chat voted for Lucas, giving out free motion sensor.");
				WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
				GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 9].myProductObject.shipItem();
			}
		}
		else if (num3 > num && num3 > num2 && num3 > num4)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("The Doll Maker is on the way!");
			EnemyManager.DollMakerManager.ReleaseTheDollMaker();
			this.myDOSTwitch.myTwitchIRC.SendMsg("Shipping the LOLPY disc...");
			WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 6].myProductObject.shipItem();
			GameManager.TheCloud.ForceKeyDiscover();
			if ((float)(num + num2 + num3 + num4) * 0.75f <= (float)num3 && num3 >= 4)
			{
				this.myDOSTwitch.myTwitchIRC.SendMsg("More than 75% of twitch chat voted for The Doll Maker, giving out 3 random tenants to the player.");
				this.DropTenant();
				GameManager.TimeSlinger.FireTimer(5f, new Action(this.DropTenant), 0);
				GameManager.TimeSlinger.FireTimer(10f, new Action(this.DropTenant), 0);
			}
		}
		else if (num4 > num && num4 > num2 && num4 > num3)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("The Bomb Maker has been released!");
			EnemyManager.BombMakerManager.ReleaseTheBombMaker();
			this.myDOSTwitch.myTwitchIRC.SendMsg("Shipping some sulphur...");
			WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
			GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 2].myProductObject.shipItem();
			if ((float)(num + num2 + num3 + num4) * 0.75f <= (float)num4 && num4 >= 4)
			{
				if (ModsManager.EasyModeActive)
				{
					this.myDOSTwitch.myTwitchIRC.SendMsg("More than 75% of twitch chat voted for The Bomb Maker, giving out 15 DOSCoins for one more sulphur package.");
					CurrencyManager.AddCurrency(15f);
				}
				else
				{
					this.myDOSTwitch.myTwitchIRC.SendMsg("More than 75% of twitch chat voted for The Bomb Maker, giving out 40 DOSCoins for one more sulphur package.");
					CurrencyManager.AddCurrency(40f);
				}
				GameManager.HackerManager.WhiteHatSound();
			}
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

	private void DropTenant()
	{
		TenantData tenantData;
		do
		{
			int num = UnityEngine.Random.Range(0, GameManager.ManagerSlinger.TenantTrackManager.TenantDatas.Length);
			tenantData = GameManager.ManagerSlinger.TenantTrackManager.TenantDatas[num];
		}
		while (tenantData.tenantUnit == 0);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
		GameManager.ManagerSlinger.TextDocManager.CreateTextDoc(tenantData.tenantUnit.ToString(), string.Concat(new object[]
		{
			tenantData.tenantName,
			Environment.NewLine,
			Environment.NewLine,
			"Age: ",
			tenantData.tenantAge,
			Environment.NewLine,
			Environment.NewLine,
			tenantData.tenantNotes
		}));
	}

	public DOSTwitch myDOSTwitch;

	private Dictionary<string, string> currentVotes;

	private bool voteIsLive;
}
