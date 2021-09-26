using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public abstract class WindowBehaviour : MonoBehaviour
{
	protected abstract void OnLaunch();

	protected abstract void OnClose();

	protected abstract void OnMin();

	protected abstract void OnUnMin();

	protected abstract void OnMax();

	protected abstract void OnUnMax();

	protected abstract void OnResized();

	public void Launch()
	{
		this.OnLaunch();
		if (!this.Window.activeSelf)
		{
			this.Window.SetActive(true);
			this.Window.GetComponent<BringWindowToFrontBehaviour>().forceTap();
		}
		else if (this.IAmMinned)
		{
			if (this.Product == SOFTWARE_PRODUCTS.UNIVERSAL)
			{
				GameManager.ManagerSlinger.AppManager.ForceUnMinApp(this.UniProductData);
			}
			else
			{
				GameManager.ManagerSlinger.AppManager.ForceUnMinApp(this.Product);
			}
		}
		else
		{
			this.Window.GetComponent<BringWindowToFrontBehaviour>().forceTap();
		}
		if (this.myData != null)
		{
			this.myData.Opened = true;
			DataManager.Save<WindowData>(this.myData);
		}
		if (this.UniProductData != null)
		{
			if (this.UniProductData.ProductTitle == "memD3FR4G3R")
			{
				this.UniProductData.MinProductTitle = "Hacks";
				this.SetWindowTitle("Hacks");
			}
			if (this.UniProductData.ProductTitle == "stackPUSHER")
			{
				this.UniProductData.MinProductTitle = "Viruses";
				this.SetWindowTitle("Viruses");
			}
		}
	}

	public void Close()
	{
		this.OnClose();
		if (this.myData != null)
		{
			this.myData.Opened = false;
			DataManager.Save<WindowData>(this.myData);
		}
	}

	public void Max()
	{
		this.Window.GetComponent<BringWindowToFrontBehaviour>().forceTap();
		this.IAmMaxxed = true;
		this.preMaxPOS = this.Window.GetComponent<RectTransform>().anchoredPosition;
		this.preMaxSize = this.Window.GetComponent<RectTransform>().sizeDelta;
		Sequence sequence = DOTween.Sequence().OnComplete(new TweenCallback(this.OnMax));
		sequence.Insert(0f, DOTween.To(() => this.Window.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			this.Window.GetComponent<RectTransform>().anchoredPosition = x;
		}, this.maxedPOS, 0.2f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.Window.GetComponent<RectTransform>().sizeDelta, delegate(Vector2 x)
		{
			this.Window.GetComponent<RectTransform>().sizeDelta = x;
		}, this.maxedSize, 0.2f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
		if (this.myData != null)
		{
			this.myData.Maxed = true;
			DataManager.Save<WindowData>(this.myData);
		}
	}

	public void UnMax()
	{
		this.Window.GetComponent<BringWindowToFrontBehaviour>().forceTap();
		this.IAmMaxxed = false;
		Sequence sequence = DOTween.Sequence().OnComplete(new TweenCallback(this.OnUnMax));
		sequence.Insert(0f, DOTween.To(() => this.Window.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			this.Window.GetComponent<RectTransform>().anchoredPosition = x;
		}, this.preMaxPOS, 0.2f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.Window.GetComponent<RectTransform>().sizeDelta, delegate(Vector2 x)
		{
			this.Window.GetComponent<RectTransform>().sizeDelta = x;
		}, this.preMaxSize, 0.2f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
		if (this.myData != null)
		{
			this.myData.Maxed = false;
			DataManager.Save<WindowData>(this.myData);
		}
	}

	public void Min()
	{
		this.OnMin();
		this.IAmMinned = true;
		this.Window.GetComponent<BringWindowToFrontBehaviour>().forceTap();
		this.preMinWindowPOS = this.Window.GetComponent<RectTransform>().anchoredPosition;
		this.preMinWindowSize = this.Window.GetComponent<RectTransform>().sizeDelta;
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			if (this.Product == SOFTWARE_PRODUCTS.UNIVERSAL)
			{
				GameManager.ManagerSlinger.AppManager.DoMinApp(this.UniProductData);
			}
			else
			{
				GameManager.ManagerSlinger.AppManager.DoMinApp(this.Product);
			}
		});
		sequence.Insert(0f, DOTween.To(() => this.Window.GetComponent<RectTransform>().sizeDelta, delegate(Vector2 x)
		{
			this.Window.GetComponent<RectTransform>().sizeDelta = x;
		}, this.minAppWindowSize, 0.15f).SetEase(Ease.Linear));
		sequence.Insert(0.15f, DOTween.To(() => this.Window.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			this.Window.GetComponent<RectTransform>().anchoredPosition = x;
		}, new Vector2(this.Window.GetComponent<RectTransform>().localPosition.x, (float)(-(float)Screen.height)), 0.25f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
		if (this.myData != null)
		{
			this.myData.Minned = true;
			DataManager.Save<WindowData>(this.myData);
		}
	}

	public void UnMin()
	{
		this.OnUnMin();
		this.IAmMinned = false;
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.Window.GetComponent<RectTransform>().anchoredPosition, delegate(Vector2 x)
		{
			this.Window.GetComponent<RectTransform>().anchoredPosition = x;
		}, this.preMinWindowPOS, 0.25f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.Window.GetComponent<RectTransform>().sizeDelta, delegate(Vector2 x)
		{
			this.Window.GetComponent<RectTransform>().sizeDelta = x;
		}, this.preMinWindowSize, 0.15f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
		if (this.myData != null)
		{
			this.myData.Minned = false;
			DataManager.Save<WindowData>(this.myData);
		}
	}

	public void Resized()
	{
		if (this.myData != null)
		{
			this.myData.WindowSize = this.myRT.sizeDelta.ToVect2();
			DataManager.Save<WindowData>(this.myData);
		}
		this.OnResized();
	}

	public void MoveMe(Vector2 NewPOS)
	{
		if (this.myData != null)
		{
			this.myData.WindowPosition = this.myRT.anchoredPosition.ToVect2();
			DataManager.Save<WindowData>(this.myData);
		}
	}

	private void stageMe()
	{
		this.myData = DataManager.Load<WindowData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new WindowData(this.myID);
			this.myData.Opened = false;
			this.myData.Maxed = false;
			this.myData.Minned = false;
			this.myData.WindowSize = this.myRT.sizeDelta.ToVect2();
			if (this.Product == SOFTWARE_PRODUCTS.ZERODAY)
			{
				this.myData.WindowPosition = new Vect2(525f, -75f);
			}
			else
			{
				this.myData.WindowPosition = this.myRT.anchoredPosition.ToVect2();
			}
		}
		this.myRT.sizeDelta = this.myData.WindowSize.ToVector2;
		this.myRT.anchoredPosition = this.myData.WindowPosition.ToVector2;
		if (this.myData.Opened)
		{
			this.Launch();
		}
		if (this.canBeMax && this.myData.Maxed)
		{
			this.Max();
			this.maxBTN.HardMax();
		}
		if (this.canBeMin && this.myData.Minned)
		{
			this.Min();
		}
		GameManager.StageManager.Stage -= this.stageMe;
	}

	protected virtual void Awake()
	{
		if (this.Product == SOFTWARE_PRODUCTS.UNIVERSAL)
		{
			this.myID = this.UniProductData.GetHashCode();
		}
		else
		{
			this.myID = (int)this.Product;
		}
		this.myRT = this.Window.GetComponent<RectTransform>();
		WindowManager.Add(this);
		GameManager.StageManager.Stage += this.stageMe;
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	public void SetWindowTitle(string title)
	{
		foreach (object obj in this.Window.transform.Find("TopBar").Find("Title").transform)
		{
			((Transform)obj).GetComponent<Text>().text = title;
		}
	}

	public SOFTWARE_PRODUCTS Product;

	public SoftwareProductDefinition UniProductData;

	public GameObject Window;

	[SerializeField]
	private bool canBeMax;

	[SerializeField]
	private MaxWindowBehaviour maxBTN;

	[SerializeField]
	private bool canBeMin;

	protected bool IAmMinned;

	protected bool IAmMaxxed;

	private Vector2 maxedPOS = new Vector2(0f, -41f);

	private Vector2 maxedSize = new Vector2((float)Screen.width, (float)Screen.height - 41f - 40f);

	private Vector2 minAppWindowSize = new Vector2(335f, 75f);

	private Vector2 preMinWindowPOS;

	private Vector2 preMinWindowSize;

	private Vector2 preMaxPOS;

	private Vector2 preMaxSize;

	private int myID;

	private WindowData myData;

	private RectTransform myRT;
}
