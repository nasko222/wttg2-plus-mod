using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class currencyTextHook : MonoBehaviour
{
	private void currencyWasAdded(float AddAMT)
	{
		if (this.myData != null)
		{
			this.myData.CurrentCurrency = this.myData.CurrentCurrency + AddAMT;
			DataManager.Save<CurrencyData>(this.myData);
		}
	}

	private void currencyWasRemoved(float RemoveAMT)
	{
		if (this.myData != null)
		{
			this.myData.CurrentCurrency = this.myData.CurrentCurrency - RemoveAMT;
			if (this.myData.CurrentCurrency <= 0f)
			{
				this.myData.CurrentCurrency = 0f;
			}
			DataManager.Save<CurrencyData>(this.myData);
		}
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<CurrencyData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new CurrencyData(this.myID);
			if (DataManager.LeetMode)
			{
				this.myData.CurrentCurrency = 1f;
			}
			else if (ModsManager.EasyModeActive)
			{
				this.myData.CurrentCurrency = 20f;
			}
			else
			{
				this.myData.CurrentCurrency = 10f;
			}
		}
		CurrencyManager.SetCurrency(this.myData.CurrentCurrency);
		this.myText.text = CurrencyManager.CurrentCurrency.ToString();
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		this.myID = 1738;
		this.myText = base.GetComponent<Text>();
		this.myTextRoller = base.gameObject.AddComponent<TextRollerObject>();
		CurrencyManager.CurrencyWasAdded.Event += this.currencyWasAdded;
		CurrencyManager.CurrencyWasRemoved.Event += this.currencyWasRemoved;
		CurrencyManager.SetCurrencyTextRoller(this.myTextRoller);
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void OnDestroy()
	{
		CurrencyManager.CurrencyWasAdded.Event -= this.currencyWasAdded;
		CurrencyManager.CurrencyWasRemoved.Event -= this.currencyWasRemoved;
	}

	private Text myText;

	private TextRollerObject myTextRoller;

	private int myID;

	private CurrencyData myData;
}
