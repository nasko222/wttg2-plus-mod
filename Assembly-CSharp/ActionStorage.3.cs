using System;

public class ActionStorage<A, B> : ActionSlinger
{
	public ActionStorage(Action<A, B> setCallBack, A setACallBackValue, B setBCallBackValue)
	{
		this.ACallBackValue = setACallBackValue;
		this.BCallBackValue = setBCallBackValue;
		this.myCallBack = setCallBack;
	}

	protected override void OnFire()
	{
		if (this.myCallBack != null)
		{
			this.myCallBack(this.ACallBackValue, this.BCallBackValue);
		}
	}

	private Action<A, B> myCallBack;

	private A ACallBackValue;

	private B BCallBackValue;
}
