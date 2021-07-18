using System;

public class ManagerSlinger
{
	public CursorManager CursorManager
	{
		get
		{
			return this.cursorManager;
		}
		set
		{
			this.cursorManager = value;
		}
	}

	public AppManager AppManager
	{
		get
		{
			return this.appManager;
		}
		set
		{
			this.appManager = value;
		}
	}

	public ProductsManager ProductsManager
	{
		get
		{
			return this.productsManager;
		}
		set
		{
			this.productsManager = value;
		}
	}

	public WifiManager WifiManager
	{
		get
		{
			return this.wifiManager;
		}
		set
		{
			this.wifiManager = value;
		}
	}

	public MotionSensorManager MotionSensorManager
	{
		get
		{
			return this.motionSensorManager;
		}
		set
		{
			this.motionSensorManager = value;
		}
	}

	public RemoteVPNManager RemoteVPNManager
	{
		get
		{
			return this.remoteVPNManager;
		}
		set
		{
			this.remoteVPNManager = value;
		}
	}

	public TenantTrackManager TenantTrackManager
	{
		get
		{
			return this.tenantTrackManager;
		}
		set
		{
			this.tenantTrackManager = value;
		}
	}

	public PoliceScannerManager PoliceScanerManager
	{
		get
		{
			return this.policeScanerManager;
		}
		set
		{
			this.policeScanerManager = value;
		}
	}

	public LOLPYDiscManager LOLPYDiscManager
	{
		get
		{
			return this.lolpyDiscManager;
		}
		set
		{
			this.lolpyDiscManager = value;
		}
	}

	public VPNManager VPNManager
	{
		get
		{
			return this.vpnManager;
		}
		set
		{
			this.vpnManager = value;
		}
	}

	public TextDocManager TextDocManager
	{
		get
		{
			return this.textDocManager;
		}
		set
		{
			this.textDocManager = value;
		}
	}

	private CursorManager cursorManager;

	private AppManager appManager;

	private ProductsManager productsManager;

	private WifiManager wifiManager;

	private MotionSensorManager motionSensorManager;

	private RemoteVPNManager remoteVPNManager;

	private TenantTrackManager tenantTrackManager;

	private PoliceScannerManager policeScanerManager;

	private LOLPYDiscManager lolpyDiscManager;

	private VPNManager vpnManager;

	private TextDocManager textDocManager;
}
