using System;

public class ActionStorage<A> : ActionSlinger
{
	public ActionStorage(Action<A> setCallBack, A setCallBackValue)
	{
		this.ACallBackValue = setCallBackValue;
		this.myCallBack = setCallBack;
	}

	protected override void OnFire()
	{
		if (this.myCallBack != null)
		{
			this.myCallBack(this.ACallBackValue);
		}
	}

	private Action<A> myCallBack;

	private A ACallBackValue;
}
