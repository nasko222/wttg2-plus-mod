using System;
using System.Collections.Generic;

public class DiscountPoll
{
	public void BeginVote()
	{
		this.currentVotes = new Dictionary<string, string>();
		this.myDOSTwitch.myTwitchIRC.SendMsg("Market Discount Poll");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Shall the player get 25% discount for every item in zeroDay market? or Shadow market? or no discounts?");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!ZERODAY");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!SHADOW");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!NO");
		this.voteIsLive = true;
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd), 0);
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "ZERODAY" || text == "SHADOW" || text == "NO"))
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
		bool flag = false;
		this.voteIsLive = false;
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Discount Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "ZERODAY")
			{
				num++;
			}
			else if (keyValuePair.Value == "SHADOW")
			{
				num2++;
			}
			else if (keyValuePair.Value == "NO")
			{
				num3++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("ZERODAY: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("SHADOW: " + num2.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("NO: " + num3.ToString());
		if (num == 0 && num2 == 0 && num3 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted, Story of my life!");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		if (num > num2 && num > num3)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("All items on zeroDay market are now 25% off.");
			WindowManager.Get(SOFTWARE_PRODUCTS.ZERODAY).Launch();
			for (int i = 0; i < GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts.Count; i++)
			{
				GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[i].myProductObject.DiscountMe();
			}
		}
		else if (num2 > num && num2 > num3)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("All items on Shadow market are now 25% off.");
			WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
			for (int j = 0; j < GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count; j++)
			{
				GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[j].myProductObject.DiscountMe();
			}
		}
		else if (num3 > num && num3 > num2)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No discounts were made!");
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

	public void BeginVoteNightmare()
	{
		this.currentVotes = new Dictionary<string, string>();
		this.myDOSTwitch.myTwitchIRC.SendMsg("Market Discount Poll");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Shall the player get 25% discount for every item in both markets?");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!YES");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!NO");
		this.voteIsLive = true;
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEndNightmare), 0);
	}

	public void CastVoteNightmare(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "YES" || text == "NO"))
			{
				this.currentVotes.Add(userName, text);
			}
		}
	}

	private void PollEndNightmare()
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		this.voteIsLive = false;
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Discount Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "YES")
			{
				num++;
			}
			else if (keyValuePair.Value == "NO")
			{
				num2++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("YES: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("NO: " + num2.ToString());
		if (num == 0 && num2 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted, Story of my life!");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		if (num > num2)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("All items are now 25% off.");
			WindowManager.Get(SOFTWARE_PRODUCTS.ZERODAY).Launch();
			for (int i = 0; i < GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts.Count; i++)
			{
				GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[i].myProductObject.DiscountMe();
			}
			WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
			for (int j = 0; j < GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count; j++)
			{
				GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[j].myProductObject.DiscountMe();
			}
		}
		else if (num2 > num)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No discounts were made!");
		}
		else
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("There is a tie! RE-VOTE!");
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.BeginVoteNightmare), 0);
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
