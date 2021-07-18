using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class CoinPickup : MonoBehaviour
	{
		public void Start()
		{
			this.coinVis = base.transform.Find("Vis");
		}

		public void Update()
		{
			this.coinVis.transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * this.spinSpeed, Vector3.up);
		}

		public void OnTriggerEnter(Collider other)
		{
			PlayerInventory component = other.GetComponent<PlayerInventory>();
			if (!component)
			{
				return;
			}
			if (this.isMassive)
			{
				HUDManager.Instance.LoadBrowseLevel(false);
			}
			else
			{
				component.AddCoin();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private Transform coinVis;

		public float spinSpeed = 20f;

		public bool isMassive;
	}
}
