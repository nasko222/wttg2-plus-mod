using System;
using UnityEngine;

public static class CurrencyManager
{
	public static float CurrentCurrency
	{
		get
		{
			return CurrencyManager._currentCurrenyAMT;
		}
	}

	public static void SetCurrency(float SetAMT)
	{
		CurrencyManager._currentCurrenyAMT = SetAMT;
	}

	public static void AddCurrency(float SetAMT)
	{
		CurrencyManager.updateCurrencyText(CurrencyManager._currentCurrenyAMT + SetAMT);
		CurrencyManager._currentCurrenyAMT += SetAMT;
		CurrencyManager.CurrencyWasAdded.Execute(SetAMT);
	}

	public static void AddPendingCurrency(float SetAMT)
	{
		CurrencyManager._pendingCurrencyAMT += SetAMT;
	}

	public static void RemoveCurrency(float SetAMT)
	{
		float num = CurrencyManager._currentCurrenyAMT - SetAMT;
		if (num <= 0f)
		{
			num = 0f;
		}
		CurrencyManager.updateCurrencyText(num);
		CurrencyManager._currentCurrenyAMT = num;
		CurrencyManager.CurrencyWasRemoved.Execute(SetAMT);
	}

	public static bool PurchaseItem(ZeroDayProductDefinition ItemToPurchase)
	{
		if (CurrencyManager._currentCurrenyAMT >= ItemToPurchase.productPrice)
		{
			CurrencyManager.RemoveCurrency(ItemToPurchase.productPrice);
			return true;
		}
		return false;
	}

	public static bool PurchaseItem(ShadowMarketProductDefinition ItemToPurchase)
	{
		if (CurrencyManager._currentCurrenyAMT >= ItemToPurchase.productPrice)
		{
			CurrencyManager.RemoveCurrency(ItemToPurchase.productPrice);
			return true;
		}
		return false;
	}

	public static void SetCurrencyTextRoller(TextRollerObject SetTextRollerObject)
	{
		CurrencyManager._currencyTextRoller = SetTextRollerObject;
	}

	public static void Tick()
	{
		if (Time.time - CurrencyManager._pendingCurrencyTimeStamp >= 5f)
		{
			CurrencyManager._pendingCurrencyTimeStamp = Time.time;
			CurrencyManager.processPendingCurrency();
		}
	}

	private static void updateCurrencyText(float ToValue)
	{
		CurrencyManager._currencyTextRoller.ProcessLinerRequest(CurrencyManager._currentCurrenyAMT, ToValue, 1f);
	}

	private static void processPendingCurrency()
	{
		if (CurrencyManager._pendingCurrencyAMT > 0f)
		{
			CurrencyManager.CurrencyWasAdded.Execute(CurrencyManager._pendingCurrencyAMT);
			CurrencyManager.updateCurrencyText(CurrencyManager._currentCurrenyAMT + CurrencyManager._pendingCurrencyAMT);
			CurrencyManager._currentCurrenyAMT += CurrencyManager._pendingCurrencyAMT;
			CurrencyManager._pendingCurrencyAMT = 0f;
		}
	}

	public static CustomEvent<float> CurrencyWasAdded = new CustomEvent<float>(2);

	public static CustomEvent<float> CurrencyWasRemoved = new CustomEvent<float>(2);

	private static TextRollerObject _currencyTextRoller;

	private static float _currentCurrenyAMT = 100f;

	private static float _pendingCurrencyAMT = 0f;

	private static float _pendingCurrencyTimeStamp = 0f;
}
