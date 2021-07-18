using System;

public class ActionStorage<A, B, C, D> : ActionSlinger
{
	public ActionStorage(Action<A, B, C, D> setCallBack, A setACallBackValue, B setBCallBackValue, C setCCallBackValue, D setDCallBackValue)
	{
		this.ACallBackValue = setACallBackValue;
		this.BCallBackValue = setBCallBackValue;
		this.CCallBackValue = setCCallBackValue;
		this.DCallBackValue = setDCallBackValue;
		this.myCallBack = setCallBack;
	}

	protected override void OnFire()
	{
		if (this.myCallBack != null)
		{
			this.myCallBack(this.ACallBackValue, this.BCallBackValue, this.CCallBackValue, this.DCallBackValue);
		}
	}

	private Action<A, B, C, D> myCallBack;

	private A ACallBackValue;

	private B BCallBackValue;

	private C CCallBackValue;

	private D DCallBackValue;
}
