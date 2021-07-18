using System;

[Serializable]
public class PlayerData
{
	public PlayerData()
	{
		this.SweeperSkillPoints = 0;
		this.memDeFragSkillPoints = 0;
		this.stackPusherSkillPoints = 0;
		this.nodeHexerSkillPoints = 0;
	}

	public int SweeperSkillPoints
	{
		get
		{
			return this.sweeperSkillPoints;
		}
		set
		{
			this.sweeperSkillPoints = value;
			if (this.sweeperSkillPoints < 0)
			{
				this.sweeperSkillPoints = 0;
			}
		}
	}

	public int MemDeFragSkillPoints
	{
		get
		{
			return this.memDeFragSkillPoints;
		}
		set
		{
			this.memDeFragSkillPoints = value;
			if (this.memDeFragSkillPoints < 0)
			{
				this.memDeFragSkillPoints = 0;
			}
		}
	}

	public int StackPusherSkillPoints
	{
		get
		{
			return this.stackPusherSkillPoints;
		}
		set
		{
			this.stackPusherSkillPoints = value;
			if (this.stackPusherSkillPoints < 0)
			{
				this.stackPusherSkillPoints = 0;
			}
		}
	}

	public int NodeHexerSkillPoints
	{
		get
		{
			return this.nodeHexerSkillPoints;
		}
		set
		{
			this.nodeHexerSkillPoints = value;
			if (this.nodeHexerSkillPoints < 0)
			{
				this.nodeHexerSkillPoints = 0;
			}
		}
	}

	private int sweeperSkillPoints;

	private int memDeFragSkillPoints;

	private int stackPusherSkillPoints;

	private int nodeHexerSkillPoints;
}
