using System;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
	public void LaunchApp(SOFTWARE_PRODUCTS AppToLaunch)
	{
		WindowManager.Get(AppToLaunch).Launch();
	}

	public void LaunchApp(SoftwareProductDefinition UniAppToLaunch)
	{
		WindowManager.Get(UniAppToLaunch).Launch();
	}

	public void CloseApp(SOFTWARE_PRODUCTS AppToClose)
	{
		WindowManager.Get(AppToClose).Close();
	}

	public void CloseApp(SoftwareProductDefinition UniAppToClose)
	{
		WindowManager.Get(UniAppToClose).Close();
	}

	public void MaxApp(SOFTWARE_PRODUCTS AppToMax)
	{
		WindowManager.Get(AppToMax).Max();
	}

	public void MaxApp(SoftwareProductDefinition AppToMax)
	{
		WindowManager.Get(AppToMax).Max();
	}

	public void UnMaxApp(SOFTWARE_PRODUCTS AppToUnMax)
	{
		WindowManager.Get(AppToUnMax).UnMax();
	}

	public void UnMaxApp(SoftwareProductDefinition AppToUnMax)
	{
		WindowManager.Get(AppToUnMax).UnMax();
	}

	public void MinApp(SOFTWARE_PRODUCTS AppToMin)
	{
		WindowManager.Get(AppToMin).Min();
	}

	public void MinApp(SoftwareProductDefinition AppToMin)
	{
		WindowManager.Get(AppToMin).Min();
	}

	public void ResizedApp(SOFTWARE_PRODUCTS AppToResized)
	{
		WindowManager.Get(AppToResized).Resized();
	}

	public void ResizedApp(SoftwareProductDefinition AppToResized)
	{
		WindowManager.Get(AppToResized).Resized();
	}

	public void UnMinApp(SOFTWARE_PRODUCTS AppToUnMin)
	{
		MinnedAppObject t;
		if (this.currentMinApps.TryGetValue((int)AppToUnMin, out t))
		{
			this.currentMinApps.Remove((int)AppToUnMin);
			this.minTabObjectPool.Push(t);
			WindowManager.Get(AppToUnMin).UnMin();
			this.minTabsCleanUp();
		}
	}

	public void UnMinApp(SoftwareProductDefinition AppToUnMin)
	{
		MinnedAppObject t;
		if (this.currentMinApps.TryGetValue(AppToUnMin.GetHashCode(), out t))
		{
			this.currentMinApps.Remove(AppToUnMin.GetHashCode());
			this.minTabObjectPool.Push(t);
			WindowManager.Get(AppToUnMin).UnMin();
			this.minTabsCleanUp();
		}
	}

	public void DoMinApp(SOFTWARE_PRODUCTS TheAppToMin)
	{
		if (!this.currentMinApps.ContainsKey((int)TheAppToMin))
		{
			MinnedAppObject minnedAppObject = this.minTabObjectPool.Pop();
			minnedAppObject.BuildMe(TheAppToMin, this.currentMinApps.Count);
			this.currentMinApps.Add((int)TheAppToMin, minnedAppObject);
		}
	}

	public void DoMinApp(SoftwareProductDefinition TheAppToMin)
	{
		if (!this.currentMinApps.ContainsKey(TheAppToMin.GetHashCode()))
		{
			MinnedAppObject minnedAppObject = this.minTabObjectPool.Pop();
			minnedAppObject.BuildMe(TheAppToMin, this.currentMinApps.Count);
			this.currentMinApps.Add(TheAppToMin.GetHashCode(), minnedAppObject);
		}
	}

	public void ForceUnMinApp(SOFTWARE_PRODUCTS AppToForceUnMin)
	{
		MinnedAppObject minnedAppObject;
		if (this.currentMinApps.TryGetValue((int)AppToForceUnMin, out minnedAppObject))
		{
			minnedAppObject.ForceDismissMe();
		}
	}

	public void ForceUnMinApp(SoftwareProductDefinition AppToForceUnMin)
	{
		MinnedAppObject minnedAppObject;
		if (this.currentMinApps.TryGetValue(AppToForceUnMin.GetHashCode(), out minnedAppObject))
		{
			minnedAppObject.ForceDismissMe();
		}
	}

	public void ActivateApp(ZeroDayProductDefinition appToActivate)
	{
		SOFTWARE_PRODUCTS productID = appToActivate.productID;
		switch (productID)
		{
		case SOFTWARE_PRODUCTS.KEY_CUE:
			InventoryManager.OwnsKeyCue = true;
			break;
		default:
			if (productID != SOFTWARE_PRODUCTS.SKYBREAK)
			{
				if (productID == SOFTWARE_PRODUCTS.MOTION_SENSOR_AUDIO_QUE)
				{
					InventoryManager.OwnsMotionSensorAudioCue = true;
				}
			}
			else
			{
				this.skyBreakIcon.ActivateMe();
			}
			break;
		case SOFTWARE_PRODUCTS.VWIPE:
			VirusManager.Ins.ClearVirus();
			break;
		}
		InventoryManager.AddProduct(appToActivate);
	}

	public void minTabsCleanUp()
	{
		int num = 0;
		foreach (KeyValuePair<int, MinnedAppObject> keyValuePair in this.currentMinApps)
		{
			keyValuePair.Value.RePOSMe(num);
			num++;
		}
	}

	private void Awake()
	{
		GameManager.ManagerSlinger.AppManager = this;
		this.minTabObjectPool = new PooledStack<MinnedAppObject>(delegate()
		{
			MinnedAppObject component = UnityEngine.Object.Instantiate<GameObject>(LookUp.DesktopUI.MIN_WINDOW_TAB_OBJECT, LookUp.DesktopUI.MIN_WINDOW_TAB_HOLDER.GetComponent<RectTransform>()).GetComponent<MinnedAppObject>();
			component.SoftBuild();
			return component;
		}, this.START_MIN_APP_POOL_COUNT);
	}

	public int START_MIN_APP_POOL_COUNT = 4;

	[SerializeField]
	private iconBehavior skyBreakIcon;

	private PooledStack<MinnedAppObject> minTabObjectPool;

	private Dictionary<int, MinnedAppObject> currentMinApps = new Dictionary<int, MinnedAppObject>();
}
