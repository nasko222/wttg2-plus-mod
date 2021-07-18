using System;

[Serializable]
public class TenantDefinition : Definition
{
	public int tenantUnit;

	public string tenantName;

	public int tenantAge;

	public string tenantNotes;

	public SEX tenantSex;

	public bool canBeTagged;
}
