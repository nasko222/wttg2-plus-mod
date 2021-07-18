using System;
using System.Collections.Generic;
using DG.Tweening;
using SWS;
using UnityEngine;

[RequireComponent(typeof(AudioHubObject))]
[RequireComponent(typeof(splineMove))]
public class DroneBehaviour : MonoBehaviour
{
	public void DeliverPackage(ShippedProductObject ThePackage)
	{
		if (!this.currentlyBusy)
		{
			this.currentlyBusy = true;
			this.currentShippingProduct = ThePackage;
			this.currentShippingProduct.gameObject.transform.SetParent(base.transform);
			this.currentShippingProduct.gameObject.transform.localPosition = this.packageLocalPosition;
			this.currentShippingProduct.gameObject.transform.localRotation = Quaternion.Euler(this.packageLocalRotation);
			this.beginDeliverPackage();
		}
		else
		{
			this.pendingProducts.Add(ThePackage);
		}
	}

	private void fireUpDrone()
	{
		this.myAudioHub.PlaySound(this.droneNoise);
		for (int i = 0; i < this.bladeAnimations.Length; i++)
		{
			this.bladeAnimations[i].DORestart(false);
		}
	}

	private void shutDownDrone()
	{
		this.myAudioHub.KillSound(this.droneNoise.AudioClip);
		for (int i = 0; i < this.bladeAnimations.Length; i++)
		{
			this.bladeAnimations[i].DOPause();
		}
	}

	private void beginDeliverPackage()
	{
		this.fireUpDrone();
		base.transform.position = this.preDeliveryLocation;
		base.transform.rotation = Quaternion.Euler(this.preDeliveryRotation);
		this.mySplineMovement.PathIsCompleted += this.deliverPackage;
		this.mySplineMovement.pathContainer = this.pathAnimations[0];
		this.mySplineMovement.ResetToStart();
		this.mySplineMovement.StartMove();
	}

	private void deliverPackage()
	{
		this.mySplineMovement.PathIsCompleted -= this.deliverPackage;
		this.currentShippingProduct.DroneDrop();
		GameManager.TimeSlinger.FireTimer(3f, delegate()
		{
			this.mySplineMovement.PathIsCompleted += this.doneWithDelivery;
			this.mySplineMovement.pathContainer = this.pathAnimations[1];
			this.mySplineMovement.ResetToStart();
			this.mySplineMovement.StartMove();
		}, 0);
	}

	private void doneWithDelivery()
	{
		this.mySplineMovement.PathIsCompleted -= this.doneWithDelivery;
		this.currentlyBusy = false;
		if (this.pendingProducts.Count > 0)
		{
			ShippedProductObject thePackage = this.pendingProducts[0];
			this.pendingProducts.RemoveAt(0);
			this.DeliverPackage(thePackage);
		}
		else
		{
			this.deSpawn();
		}
	}

	private void deSpawn()
	{
		this.mySplineMovement.PathIsCompleted -= this.deSpawn;
		this.shutDownDrone();
		base.transform.position = this.deSpawnLocation;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
	}

	private void Awake()
	{
		GameManager.BehaviourManager.DroneBehaviour = this;
		this.myAudioHub = base.GetComponent<AudioHubObject>();
		this.mySplineMovement = base.GetComponent<splineMove>();
	}

	private void Start()
	{
		this.shutDownDrone();
		base.transform.position = this.deSpawnLocation;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
	}

	[SerializeField]
	private PathManager[] pathAnimations = new PathManager[0];

	[SerializeField]
	private DOTweenAnimation[] bladeAnimations = new DOTweenAnimation[0];

	[SerializeField]
	private Vector3 deSpawnLocation = Vector3.zero;

	[SerializeField]
	private Vector3 packageLocalPosition = Vector3.zero;

	[SerializeField]
	private Vector3 packageLocalRotation = Vector3.zero;

	[SerializeField]
	private Vector3 preDeliveryLocation = Vector3.zero;

	[SerializeField]
	private Vector3 preDeliveryRotation = Vector3.zero;

	[SerializeField]
	private AudioFileDefinition droneNoise;

	private AudioHubObject myAudioHub;

	private splineMove mySplineMovement;

	private ShippedProductObject currentShippingProduct;

	private List<ShippedProductObject> pendingProducts = new List<ShippedProductObject>();

	private bool currentlyBusy;
}
