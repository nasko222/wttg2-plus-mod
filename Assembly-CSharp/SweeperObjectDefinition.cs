using System;

[Serializable]
public class SweeperObjectDefinition : Definition
{
	public int SkillPointsRequired;

	public int NumOfSweepers;

	public int NumOfDotsPerSweeper;

	public int HotZoneDotSizeMin;

	public int HotZoneDotSizeMax;

	public float ScrollSpeedMin;

	public float ScrollSpeedMax;

	public int HotBarWidthMin;

	public int HotBarWidthMax;

	public int WarmUpTime;

	public int NumOfSweepsPerSweeper;

	public float BufferTime;

	public int PointsRewaredTier1;

	public int PointsRewaredTier2;

	public int PointsRewaredTier3;

	public int PointsRewaredTier4;

	public int PointsDeducted;
}
