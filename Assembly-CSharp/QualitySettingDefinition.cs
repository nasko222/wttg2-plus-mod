using System;

[Serializable]
public class QualitySettingDefinition : Definition
{
	public bool vSync;

	public bool shadowsOn;

	public bool shadowsSoftOn;

	public short shadowResolution;

	public short textureQuality;

	public bool antiAliasing;

	public bool ssaa;

	public short ssaaSampling;

	public bool ultraSSAA;

	public bool volumetricLighting;

	public bool realTimeReflections;

	public bool postFXSSAO;

	public bool postFXBloom;

	public bool postFXFastVintage;

	public bool postFXLoFi;

	public bool postFXTonemapping;
}
