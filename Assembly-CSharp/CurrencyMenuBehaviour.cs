using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyMenuBehaviour : MonoBehaviour
{
	private void rebuildMenu()
	{
		float y = 0f;
		if (this.currentActiveRemoteVPNS.Count > 0)
		{
			y = 4f + ((float)this.currentActiveRemoteVPNS.Count * 22f + (float)this.currentActiveRemoteVPNS.Count * 4f) + 4f;
			this.noActiveRemoteVPNSCG.alpha = 0f;
		}
		else
		{
			y = 30f;
			this.noActiveRemoteVPNSCG.alpha = 1f;
		}
		int num = 0;
		foreach (KeyValuePair<RemoteVPNObject, RemoteVPNMenuObject> keyValuePair in this.currentActiveRemoteVPNS)
		{
			keyValuePair.Value.PutMe(num);
			num++;
		}
		Vector2 sizeDelta = new Vector2(this.myRT.sizeDelta.x, y);
		Vector2 anchoredPosition = new Vector2(this.myRT.anchoredPosition.x, y);
		this.myRT.sizeDelta = sizeDelta;
		this.myRT.anchoredPosition = anchoredPosition;
	}

	private void removeRemoteVPNFromMenu(RemoteVPNObject TheRemoteVPN)
	{
		RemoteVPNMenuObject remoteVPNMenuObject;
		if (this.currentActiveRemoteVPNS.TryGetValue(TheRemoteVPN, out remoteVPNMenuObject))
		{
			remoteVPNMenuObject.ClearMe();
			this.currentActiveRemoteVPNS.Remove(TheRemoteVPN);
			this.remoteVPNMenuObjectPool.Push(remoteVPNMenuObject);
			this.rebuildMenu();
		}
	}

	private void addRemoteVPNToMenu(RemoteVPNObject TheRemoteVPN)
	{
		if (TheRemoteVPN.Placed)
		{
			if (!this.currentActiveRemoteVPNS.ContainsKey(TheRemoteVPN))
			{
				RemoteVPNMenuObject remoteVPNMenuObject = this.remoteVPNMenuObjectPool.Pop();
				remoteVPNMenuObject.BuildMe(TheRemoteVPN);
				this.currentActiveRemoteVPNS.Add(TheRemoteVPN, remoteVPNMenuObject);
			}
			this.rebuildMenu();
		}
	}

	private void stageMe()
	{
		this.rebuildMenu();
		GameManager.StageManager.Stage -= this.stageMe;
		GameManager.ManagerSlinger.RemoteVPNManager.EnteredPlacementMode += this.removeRemoteVPNFromMenu;
		GameManager.ManagerSlinger.RemoteVPNManager.RemoteVPNWasReturned += this.removeRemoteVPNFromMenu;
		GameManager.ManagerSlinger.RemoteVPNManager.RemoteVPNWasPlaced += this.addRemoteVPNToMenu;
	}

	private void Awake()
	{
		this.myRT = base.GetComponent<RectTransform>();
		int index = this.REMOTE_VPN_MENU_POOL_COUNT + 1;
		this.remoteVPNMenuObjectPool = new PooledStack<RemoteVPNMenuObject>(delegate()
		{
			index--;
			RemoteVPNMenuObject component = UnityEngine.Object.Instantiate<GameObject>(this.remoteVPNMenuObject, this.myRT).GetComponent<RemoteVPNMenuObject>();
			component.SoftBuild(index);
			return component;
		}, this.REMOTE_VPN_MENU_POOL_COUNT);
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void OnDestroy()
	{
	}

	[SerializeField]
	private int REMOTE_VPN_MENU_POOL_COUNT = 6;

	[SerializeField]
	private CanvasGroup noActiveRemoteVPNSCG;

	[SerializeField]
	private GameObject remoteVPNMenuObject;

	private const float OPT_SPACING = 4f;

	private const float BOT_SPACING = 4f;

	private RectTransform myRT;

	private PooledStack<RemoteVPNMenuObject> remoteVPNMenuObjectPool;

	private Dictionary<RemoteVPNObject, RemoteVPNMenuObject> currentActiveRemoteVPNS = new Dictionary<RemoteVPNObject, RemoteVPNMenuObject>(6);
}
