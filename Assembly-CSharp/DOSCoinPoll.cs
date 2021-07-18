using System;
using System.Collections.Generic;

public class DOSCoinPoll
{
	public void BeginVote()
	{
		this.currentVotes = new Dictionary<string, string>();
		this.myDOSTwitch.myTwitchIRC.SendMsg("DOS Coin Poll");
		this.randomDOS = 50;
		this.myDOSTwitch.myTwitchIRC.SendMsg("Give or take " + this.randomDOS.ToString() + " DOS coins from the player");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!GIVE");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!TAKE");
		this.voteIsLive = true;
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd), 0);
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "GIVE" || text == "TAKE"))
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
		this.myDOSTwitch.myTwitchIRC.SendMsg("The DOSCoin Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "GIVE")
			{
				num++;
			}
			else if (keyValuePair.Value == "TAKE")
			{
				num2++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("GIVE: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("TAKE: " + num2.ToString());
		if (num == 0 && num2 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted, Story of my life!");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		if (num2 > num)
		{
			if (CurrencyManager.CurrentCurrency < (float)this.randomDOS)
			{
				DOSCoinPoll.moneyLoan += this.randomDOS - (int)CurrencyManager.CurrentCurrency;
				this.randomDOS = (int)CurrencyManager.CurrentCurrency;
				this.myDOSTwitch.myTwitchIRC.SendMsg("Taking " + this.randomDOS.ToString() + " DOSCoins from the player!");
				this.myDOSTwitch.myTwitchIRC.SendMsg("The player now owes " + DOSCoinPoll.moneyLoan.ToString() + " DOSCoins!");
				GameManager.TimeSlinger.FireTimer(300f, new Action(this.ReturnLoan), 0);
			}
			else
			{
				this.myDOSTwitch.myTwitchIRC.SendMsg("Taking " + this.randomDOS.ToString() + " DOSCoins from the player!");
			}
			CurrencyManager.RemoveCurrency((float)this.randomDOS);
			GameManager.HackerManager.BlackHatSound();
		}
		else if (num > num2)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("Giving " + this.randomDOS.ToString() + " DOSCoins to the player!");
			CurrencyManager.AddCurrency((float)this.randomDOS);
			if (DOSCoinPoll.moneyLoan > 0 && CurrencyManager.CurrentCurrency >= (float)DOSCoinPoll.moneyLoan)
			{
				GameManager.TimeSlinger.FireTimer(10f, new Action(this.ReturnLoan), 0);
			}
			GameManager.HackerManager.WhiteHatSound();
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

	public void ReturnLoan()
	{
		if ((int)CurrencyManager.CurrentCurrency < DOSCoinPoll.moneyLoan)
		{
			GameManager.TimeSlinger.FireTimer(300f, new Action(this.ReturnLoan), 0);
			return;
		}
		if (DOSCoinPoll.moneyLoan == 0)
		{
			return;
		}
		CurrencyManager.RemoveCurrency((float)DOSCoinPoll.moneyLoan);
		this.myDOSTwitch.myTwitchIRC.SendMsg("The loan of " + DOSCoinPoll.moneyLoan + " DOSCoins was returned to twitch chat.");
		DOSCoinPoll.moneyLoan = 0;
	}

	public DOSTwitch myDOSTwitch;

	private Dictionary<string, string> currentVotes;

	private bool voteIsLive;

	private int randomDOS;

	public static int moneyLoan;
}
