using System;

public class ActionStorage : ActionSlinger
{
	public ActionStorage(Action setCallBackAction)
	{
		this.myCallBack = setCallBackAction;
	}

	protected override void OnFire()
	{
		this.myCallBack();
	}

	private Action myCallBack;
}
