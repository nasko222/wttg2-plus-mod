using System;

public static class UIDialogManager
{
	public static NetworkDialog NetworkDialog
	{
		get
		{
			return UIDialogManager._networkDialog;
		}
		set
		{
			UIDialogManager._networkDialog = value;
		}
	}

	public static VWipeDialog VWipeDialog
	{
		get
		{
			return UIDialogManager._vWipeDialog;
		}
		set
		{
			UIDialogManager._vWipeDialog = value;
		}
	}

	private static NetworkDialog _networkDialog;

	private static VWipeDialog _vWipeDialog;
}
