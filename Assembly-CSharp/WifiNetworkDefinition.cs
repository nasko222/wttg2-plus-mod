using System;

[Serializable]
public class WifiNetworkDefinition : Definition
{
	public bool networkIsOffline;

	public float networkCoolOffTime;

	public string networkName;

	public short networkStrength;

	public WIFI_SECURITY networkSecurity;

	public string networkPassword;

	public float networkTrackRate;

	public float networkTrackProbability;

	public string networkBSSID;

	public short networkChannel;

	public short networkPower;

	public WIFI_SIGNAL_TYPE networkSignal;

	public short networkOpenPort;

	public short networkRandPortStart;

	public short networkRandPortEnd;

	public short networkInjectionAmount;

	public short networkInjectionRandStart;

	public short networkInjectionRandEnd;

	public short networkMaxInjectionAmount;

	public float networkInjectionCoolOffTime;

	public bool affectedByDosDrainer;
}
