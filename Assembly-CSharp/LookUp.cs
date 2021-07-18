using System;

public static class LookUp
{
	public static DesktopUI DesktopUI
	{
		get
		{
			return LookUp._desktopUI;
		}
		set
		{
			LookUp._desktopUI = value;
		}
	}

	public static PlayerUI PlayerUI
	{
		get
		{
			return LookUp._playerUI;
		}
		set
		{
			LookUp._playerUI = value;
		}
	}

	public static SoftwareProducts SoftwareProducts
	{
		get
		{
			return LookUp._softwareProducts;
		}
		set
		{
			LookUp._softwareProducts = value;
		}
	}

	public static SoundLookUp SoundLookUp
	{
		get
		{
			return LookUp._soundLookUp;
		}
		set
		{
			LookUp._soundLookUp = value;
		}
	}

	public static Doors Doors
	{
		get
		{
			return LookUp._doors;
		}
		set
		{
			LookUp._doors = value;
		}
	}

	private static DesktopUI _desktopUI;

	private static PlayerUI _playerUI;

	private static SoftwareProducts _softwareProducts;

	private static SoundLookUp _soundLookUp;

	private static Doors _doors;
}
