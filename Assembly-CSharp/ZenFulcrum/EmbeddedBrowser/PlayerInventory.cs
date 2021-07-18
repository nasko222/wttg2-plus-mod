using System;
using System.Diagnostics;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class PlayerInventory : MonoBehaviour
	{
		public static PlayerInventory Instance { get; private set; }

		public int NumCoins { get; private set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> coinCollected = delegate(int coins)
		{
		};

		public void Awake()
		{
			PlayerInventory.Instance = this;
		}

		public void AddCoin()
		{
			this.NumCoins++;
			this.coinCollected(this.NumCoins);
		}

		public HUDManager hud;
	}
}
