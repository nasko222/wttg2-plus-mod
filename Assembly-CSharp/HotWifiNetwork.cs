using System;

[Serializable]
public struct HotWifiNetwork
{
	public HotWifiNetwork(int SetHash, float SetHotTime, float SetTimeStamp, float SetTimeLeft)
	{
		this.Hash = SetHash;
		this.HotTime = SetHotTime;
		this.TimeStamp = SetTimeStamp;
		this.TimeLeft = SetTimeLeft;
	}

	public int Hash;

	public float HotTime;

	public float TimeStamp;

	public float TimeLeft;
}
