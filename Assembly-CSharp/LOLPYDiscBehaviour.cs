using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class LOLPYDiscBehaviour : MonoBehaviour
{
	public void SoftBuild()
	{
		this.myMeshRenderer = base.GetComponent<MeshRenderer>();
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.myMeshRenderer.enabled = false;
		this.myInteractionHook.LeftClickAction += this.leftClickAction;
		this.insertTween = DOTween.To(() => new Vector3(3.818f, 0.6124f, -23.1333f), delegate(Vector3 x)
		{
			base.transform.position = x;
		}, new Vector3(3.818f, 0.6124f, -23.2183f), 1f).SetEase(Ease.OutSine).OnComplete(delegate
		{
			GameManager.ManagerSlinger.TenantTrackManager.UnLockSystem();
		});
		this.insertTween.Pause<Tweener>();
		this.insertTween.SetAutoKill(false);
	}

	public void MoveMe(Vector3 SetPOS, Vector3 SetROT)
	{
		this.myMeshRenderer.enabled = true;
		base.transform.position = SetPOS;
		base.transform.rotation = Quaternion.Euler(SetROT);
	}

	public void InsertMe()
	{
		GameManager.ManagerSlinger.LOLPYDiscManager.LOLPYDiscWasInserted();
		this.myMeshRenderer.enabled = true;
		base.transform.position = new Vector3(3.818f, 0.6124f, -23.1333f);
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, -90f, 90f));
		this.insertTween.Restart(true, -1f);
	}

	public void HardInsert()
	{
		this.myMeshRenderer.enabled = false;
		this.myInteractionHook.ForceLock = true;
		GameManager.ManagerSlinger.TenantTrackManager.UnLockSystem();
	}

	private void leftClickAction()
	{
		this.myMeshRenderer.enabled = false;
		this.myInteractionHook.ForceLock = true;
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPickUp2);
		GameManager.ManagerSlinger.LOLPYDiscManager.LOLPYDiscWasPickedUp();
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.leftClickAction;
	}

	private MeshRenderer myMeshRenderer;

	private InteractionHook myInteractionHook;

	private Tweener insertTween;
}
