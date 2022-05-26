using System;

public class BombMakerDataDefinition
{
	public BombMakerDataDefinition()
	{
		this.SulphurCoolTimeMin = 930f;
		this.SulphurCoolTimeMax = 1290f;
		this.maxSulphurReq = 6;
	}

	public readonly float SulphurCoolTimeMin;

	public readonly float SulphurCoolTimeMax;

	public readonly int maxSulphurReq;
}
