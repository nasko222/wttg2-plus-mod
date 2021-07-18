using System;

[Serializable]
public class MemDefragLevelDefinition : Definition
{
	public int SkillPointsRequired;

	public int WarmUpTime;

	public float TimePerFragment;

	public int MemoryCellCount;

	public int MemoryFragmentCountMin;

	public int MemoryFragmentCountMax;

	public HACK_MEM_DEFRAG_KEY_TYPE MemoryFragmentType;

	public int PointsRewaredTier1;

	public int PointsRewaredTier2;

	public int PointsRewaredTier3;

	public int PointsRewaredTier4;

	public int PointsDeducted;
}
