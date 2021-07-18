using System;
using System.Diagnostics;
using UnityEngine;

public class LOLPYDiscManager : MonoBehaviour
{
	public LOLPYDiscBehaviour LOLPYDiscBeh
	{
		get
		{
			return this.lolpyDiscIns;
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event LOLPYDiscManager.LOLPYDiscActions DiscWasPickedUp;

	public void LOLPYDiscWasPickedUp()
	{
		if (this.DiscWasPickedUp != null)
		{
			this.DiscWasPickedUp();
		}
	}

	public void LOLPYDiscWasInserted()
	{
		this.myData.WasInserted = true;
		DataManager.Save<LOLPYDiscData>(this.myData);
	}

	private void productWasPickedUp(ShadowMarketProductDefinition TheProduct)
	{
		HARDWARE_PRODUCTS productID = TheProduct.productID;
		if (productID == HARDWARE_PRODUCTS.FLOPPY_DISK)
		{
			this.spawnLOLPYDisc();
		}
	}

	private void spawnLOLPYDisc()
	{
		this.lolpyDiscIns.MoveMe(this.spawnPOS, this.spawnROT);
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<LOLPYDiscData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new LOLPYDiscData(this.myID);
			this.myData.WasInserted = false;
		}
		if (this.myData.WasInserted)
		{
			this.lolpyDiscIns.HardInsert();
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		this.myID = base.transform.position.GetHashCode();
		GameManager.ManagerSlinger.LOLPYDiscManager = this;
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.ManagerSlinger.ProductsManager.ShadowMarketProductWasActivated.Event += this.productWasPickedUp;
		this.lolpyDiscIns = UnityEngine.Object.Instantiate<GameObject>(this.lolpyDiscObject, this.lolpyDiscParent).GetComponent<LOLPYDiscBehaviour>();
		this.lolpyDiscIns.SoftBuild();
	}

	private void OnDestroy()
	{
	}

	[SerializeField]
	private Transform lolpyDiscParent;

	[SerializeField]
	private GameObject lolpyDiscObject;

	[SerializeField]
	private Vector3 spawnPOS;

	[SerializeField]
	private Vector3 spawnROT;

	private LOLPYDiscBehaviour lolpyDiscIns;

	private int myID;

	private LOLPYDiscData myData;

	public delegate void LOLPYDiscActions();
}
