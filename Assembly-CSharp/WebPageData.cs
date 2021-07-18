using System;

[Serializable]
public class WebPageData
{
	public int KeyDiscoveryMode { get; set; }

	public bool IsTapped { get; set; }

	public int HashIndex { get; set; }

	public string HashValue { get; set; }
}
