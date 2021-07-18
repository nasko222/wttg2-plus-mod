using System;

public class ActionStorage<A, B, C> : ActionSlinger
{
	public ActionStorage(Action<A, B, C> setCallBack, A setACallBackValue, B setBCallBackValue, C setCCallBackValue)
	{
		this.ACallBackValue = setACallBackValue;
		this.BCallBackValue = setBCallBackValue;
		this.CCallBackValue = setCCallBackValue;
		this.myCallBack = setCallBack;
	}

	protected override void OnFire()
	{
		if (this.myCallBack != null)
		{
			this.myCallBack(this.ACallBackValue, this.BCallBackValue, this.CCallBackValue);
		}
	}

	private Action<A, B, C> myCallBack;

	private A ACallBackValue;

	private B BCallBackValue;

	private C CCallBackValue;
}
