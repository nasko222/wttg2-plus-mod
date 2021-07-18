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
