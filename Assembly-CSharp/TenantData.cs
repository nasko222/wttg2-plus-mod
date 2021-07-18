using System;

[Serializable]
public class TenantData
{
	public TenantData()
	{
	}

	public TenantData(TenantDefinition Source)
	{
		this.tenantUnit = Source.tenantUnit;
		this.tenantName = Source.tenantName;
		this.tenantAge = Source.tenantAge;
		this.tenantNotes = Source.tenantNotes;
		this.tenantSex = (int)Source.tenantSex;
		this.canBeTagged = Source.canBeTagged;
	}

	public int tenantUnit;

	public string tenantName;

	public int tenantAge;

	public string tenantNotes;

	public int tenantSex;

	public bool canBeTagged;

	public int tenantIndex;
}
