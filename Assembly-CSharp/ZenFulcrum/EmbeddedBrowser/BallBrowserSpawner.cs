using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	[RequireComponent(typeof(Browser))]
	public class BallBrowserSpawner : MonoBehaviour, INewWindowHandler
	{
		public void Start()
		{
			base.GetComponent<Browser>().NewWindowHandler = this;
		}

		public Browser CreateBrowser(Browser parent)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.AddComponent<Rigidbody>();
			gameObject.transform.localScale = new Vector3(this.size, this.size, this.size);
			gameObject.transform.position = this.spawnPosition.position + Vector3.one * UnityEngine.Random.value * 0.01f;
			Browser browser = gameObject.AddComponent<Browser>();
			browser.UIHandler = null;
			browser.Resize(110, 110);
			return browser;
		}

		public Transform spawnPosition;

		public float size;
	}
}
