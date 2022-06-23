using System;
using UnityEngine;

public class PoliceScannerManager : MonoBehaviour
{
	private void productWasPickedUp(ShadowMarketProductDefinition TheProduct)
	{
		HARDWARE_PRODUCTS productID = TheProduct.productID;
		if (productID == HARDWARE_PRODUCTS.POLICE_SCANNER)
		{
			this.spawnPoliceScanner();
		}
		if (productID == HARDWARE_PRODUCTS.ROUTER)
		{
			RouterBehaviour.Ins.MoveMe(new Vector3(-5.66f, 39.1f, -1.93f), new Vector3(0f, -86f, 0f), new Vector3(0.45f, 0.45f, 0.45f));
		}
		if (productID == HARDWARE_PRODUCTS.TAROT_CARDS)
		{
			TarotCardsBehaviour.Ins.MoveMe(new Vector3(1.393f, 40.68f, 2.489f), new Vector3(0f, -20f, 180f), new Vector3(0.3f, 0.3f, 0.3f));
		}
	}

	private void spawnPoliceScanner()
	{
		this.policeScanerIns.MoveMe(this.spawnPOS, this.spawnROT);
	}

	private void Awake()
	{
		GameManager.ManagerSlinger.PoliceScanerManager = this;
		GameManager.ManagerSlinger.ProductsManager.ShadowMarketProductWasActivated.Event += this.productWasPickedUp;
		this.policeScanerIns = UnityEngine.Object.Instantiate<GameObject>(this.policeScannerObject, this.policeScanerParent).GetComponent<PoliceScannerBehaviour>();
		this.policeScanerIns.SoftBuild();
		UnityEngine.Object.Instantiate<GameObject>(CustomObjectLookUp.Router).GetComponent<RouterBehaviour>().SoftBuild();
		UnityEngine.Object.Instantiate<GameObject>(CustomObjectLookUp.TarotCards).GetComponent<TarotCardsBehaviour>().SoftBuild();
	}

	private void OnDestroy()
	{
	}

	[SerializeField]
	private Transform policeScanerParent;

	[SerializeField]
	private GameObject policeScannerObject;

	[SerializeField]
	private Vector3 spawnPOS;

	[SerializeField]
	private Vector3 spawnROT;

	private PoliceScannerBehaviour policeScanerIns;
}
