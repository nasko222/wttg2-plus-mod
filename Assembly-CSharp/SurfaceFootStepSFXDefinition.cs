using System;
using System.Collections.Generic;

[Serializable]
public class SurfaceFootStepSFXDefinition : Definition
{
	public List<AudioFileDefinition> WalkFootStepSFXS = new List<AudioFileDefinition>();

	public List<AudioFileDefinition> DuckFootStepSFXS = new List<AudioFileDefinition>();

	public List<AudioFileDefinition> RunFootStepSFXS = new List<AudioFileDefinition>();
}
