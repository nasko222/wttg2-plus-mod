using System;

[Serializable]
public struct VPNCurrencyData
{
	public VPNCurrencyData(float SetTime, float SetValue)
	{
		this.GenerateTime = SetTime;
		this.GenerateDOSCoinValue = SetValue;
	}

	public float GenerateTime;

	public float GenerateDOSCoinValue;
}
