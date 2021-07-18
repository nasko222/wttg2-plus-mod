using System;

public abstract class ActionSlinger
{
	public void Fire()
	{
		this.OnFire();
	}

	protected abstract void OnFire();
}
