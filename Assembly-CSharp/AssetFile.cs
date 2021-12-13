using System;

public struct AssetFile
{
	public AssetFile(long size, string fileName, string location, string externalURL)
	{
		this.size = size;
		this.fileName = fileName;
		this.location = location;
		this.externalURL = externalURL;
		this.path = location + fileName;
	}

	public long size;

	public string fileName;

	public string location;

	public string externalURL;

	public string path;
}
