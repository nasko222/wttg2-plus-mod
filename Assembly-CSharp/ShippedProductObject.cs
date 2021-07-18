using System;
using UnityEngine;

public class ShippedProductObject : MonoBehaviour
{
	public void SoftBuild()
	{
		this.myMeshRender.enabled = false;
		this.myMeshCollider.enabled = false;
		this.myRigidBody.isKinematic = true;
		this.myInteractionHook.ForceLock = true;
	}

	public void ShipMe(ShadowMarketProductDefinition TheProduct, Vector3 TargetLocation, Vector3 TargetRotation)
	{
		this.myProduct = TheProduct;
		this.myMeshRender.enabled = true;
		this.myMeshCollider.enabled = true;
		this.myInteractionHook.ForceLock = false;
		base.transform.position = TargetLocation;
		base.transform.rotation = Quaternion.Euler(TargetRotation);
		this.myRigidBody.isKinematic = false;
	}

	public void DroneDeliver(ShadowMarketProductDefinition TheProduct)
	{
		this.myProduct = TheProduct;
		this.myMeshRender.enabled = true;
		this.myMeshCollider.enabled = true;
		this.myInteractionHook.ForceLock = false;
		GameManager.BehaviourManager.DroneBehaviour.DeliverPackage(this);
	}

	public void DroneDrop()
	{
		base.transform.SetParent(GameManager.ManagerSlinger.ProductsManager.ShippedProductParent);
		this.myRigidBody.isKinematic = false;
	}

	private void pickUpPackage()
	{
		this.myAudioHub.PlaySound(this.pickUpPackageSFX);
		this.myMeshRender.enabled = false;
		this.myMeshCollider.enabled = false;
		this.myRigidBody.isKinematic = true;
		this.myInteractionHook.ForceLock = true;
		GameManager.TimeSlinger.FireTimer(1f, delegate()
		{
			base.transform.position = Vector3.zero;
			base.transform.rotation = Quaternion.Euler(Vector3.zero);
		}, 0);
		GameManager.ManagerSlinger.ProductsManager.ActivateShadowMarketProduct(this.myProduct);
		this.ProductPickUp.Execute(this);
	}

	private void Awake()
	{
		this.myMeshRender = base.GetComponent<MeshRenderer>();
		this.myMeshCollider = base.GetComponent<MeshCollider>();
		this.myRigidBody = base.GetComponent<Rigidbody>();
		this.myAudioHub = base.GetComponent<AudioHubObject>();
		this.myInteractionHook.LeftClickAction += this.pickUpPackage;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.pickUpPackage;
	}

	public CustomEvent<ShippedProductObject> ProductPickUp = new CustomEvent<ShippedProductObject>(2);

	[SerializeField]
	private InteractionHook myInteractionHook;

	[SerializeField]
	private AudioFileDefinition pickUpPackageSFX;

	private MeshRenderer myMeshRender;

	private MeshCollider myMeshCollider;

	private Rigidbody myRigidBody;

	private ShadowMarketProductDefinition myProduct;

	private AudioHubObject myAudioHub;
}
