using System;

[Serializable]
public class StackPusherHackData : DataObject
{
	public StackPusherHackData(int SetID) : base(SetID)
	{
	}

	public int SkillPoints
	{
		get
		{
			return this.skillPoints;
		}
		set
		{
			this.skillPoints = value;
			if (this.skillPoints <= 0)
			{
				this.skillPoints = 0;
			}
		}
	}

	private int skillPoints;
}
