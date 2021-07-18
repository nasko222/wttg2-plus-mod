using System;
using UnityEngine;

[AddComponentMenu("UBER/Global Params")]
[ExecuteInEditMode]
public class UBER_GlobalParams : MonoBehaviour
{
	private void Update()
	{
		this.AdvanceTime(Time.deltaTime);
	}

	private void Start()
	{
		this.SetupIt();
	}

	public void SetupIt()
	{
		Shader.SetGlobalFloat("_UBER_GlobalDry", 1f - this.WaterLevel);
		Shader.SetGlobalFloat("_UBER_GlobalDryConst", 1f - this.WetnessAmount);
		Shader.SetGlobalFloat("_UBER_GlobalRainDamp", 1f - this.RainIntensity);
		Shader.SetGlobalFloat("_UBER_RippleStrength", this.FlowBumpStrength);
		Shader.SetGlobalFloat("_UBER_GlobalSnowDamp", 1f - this.SnowLevel);
		Shader.SetGlobalFloat("_UBER_Frost", 1f - this.Frost);
		Shader.SetGlobalFloat("_UBER_GlobalSnowDissolve", this.SnowDissolve);
		Shader.SetGlobalFloat("_UBER_GlobalSnowBumpMicro", this.SnowBumpMicro);
		Shader.SetGlobalColor("_UBER_GlobalSnowSpecGloss", this.SnowSpecGloss);
		Shader.SetGlobalColor("_UBER_GlobalSnowGlitterColor", this.SnowGlitterColor);
	}

	public void AdvanceTime(float amount)
	{
		this.SimulateDynamicWeather(amount * this.weatherTimeScale);
		amount *= this.flowTimeScale;
		this.__Time.x = this.__Time.x + amount / 20f;
		this.__Time.y = this.__Time.y + amount;
		this.__Time.z = this.__Time.z + amount * 2f;
		this.__Time.w = this.__Time.w + amount * 3f;
		Shader.SetGlobalVector("UBER_Time", this.__Time);
	}

	public void SimulateDynamicWeather(float dt)
	{
		if (dt == 0f || !this.Simulate)
		{
			return;
		}
		float rainIntensity = this.RainIntensity;
		float propA = this.temperature;
		float propA2 = this.flowTimeScale;
		float flowBumpStrength = this.FlowBumpStrength;
		float waterLevel = this.WaterLevel;
		float wetnessAmount = this.WetnessAmount;
		float snowLevel = this.SnowLevel;
		float snowDissolve = this.SnowDissolve;
		float snowBumpMicro = this.SnowBumpMicro;
		Color snowSpecGloss = this.SnowSpecGloss;
		Color snowGlitterColor = this.SnowGlitterColor;
		float num = this.wind * 4f + 1f;
		float num2 = (!this.FreezeWetWhenSnowPresent) ? 1f : Mathf.Clamp01((0.05f - this.SnowLevel) / 0.05f);
		if (this.temperature > 0f)
		{
			float num3 = this.temperature + 10f;
			this.RainIntensity = this.fallIntensity * num2;
			this.flowTimeScale += dt * num3 * 0.3f * num2;
			if (this.flowTimeScale > 1f)
			{
				this.flowTimeScale = 1f;
			}
			this.FlowBumpStrength += dt * num3 * 0.3f * num2;
			if (this.FlowBumpStrength > 1f)
			{
				this.FlowBumpStrength = 1f;
			}
			this.WaterLevel += this.RainIntensity * dt * 2f * num2;
			if (this.WaterLevel > 1f)
			{
				this.WaterLevel = 1f;
			}
			this.WetnessAmount += this.RainIntensity * dt * 3f * num2;
			if (this.WetnessAmount > 1f)
			{
				this.WetnessAmount = 1f;
			}
			float num4 = Mathf.Abs(dt * num3 * 0.03f + dt * this.RainIntensity * 0.05f);
			this.SnowDissolve = this.TargetValue(this.SnowDissolve, this.SnowDissolveMelt, num4 * 2f);
			this.SnowBumpMicro = this.TargetValue(this.SnowBumpMicro, this.SnowBumpMicroMelt, num4 * 0.1f);
			this.SnowSpecGloss.r = this.TargetValue(this.SnowSpecGloss.r, this.SnowSpecGlossMelt.r, num4);
			this.SnowSpecGloss.g = this.TargetValue(this.SnowSpecGloss.g, this.SnowSpecGlossMelt.g, num4);
			this.SnowSpecGloss.b = this.TargetValue(this.SnowSpecGloss.b, this.SnowSpecGlossMelt.b, num4);
			this.SnowSpecGloss.a = this.TargetValue(this.SnowSpecGloss.a, this.SnowSpecGlossMelt.a, num4);
			this.SnowGlitterColor.r = this.TargetValue(this.SnowGlitterColor.r, this.SnowGlitterColorMelt.r, num4);
			this.SnowGlitterColor.g = this.TargetValue(this.SnowGlitterColor.g, this.SnowGlitterColorMelt.g, num4);
			this.SnowGlitterColor.b = this.TargetValue(this.SnowGlitterColor.b, this.SnowGlitterColorMelt.b, num4);
			this.SnowGlitterColor.a = this.TargetValue(this.SnowGlitterColor.a, this.SnowGlitterColorMelt.a, num4);
			this.Frost -= dt * num3 * 0.3f * num2;
			if (this.Frost < 0f)
			{
				this.Frost = 0f;
			}
			this.SnowLevel -= dt * num3 * 0.01f;
			if (this.SnowLevel < 0f)
			{
				this.SnowLevel = 0f;
			}
		}
		else
		{
			float num5 = this.temperature - 10f;
			this.RainIntensity += dt * num5 * 0.2f;
			if (this.RainIntensity < 0f)
			{
				this.RainIntensity = 0f;
			}
			this.flowTimeScale += dt * num5 * 0.3f * num;
			if (this.flowTimeScale < 0f)
			{
				this.flowTimeScale = 0f;
			}
			if (this.FlowBumpStrength > 0.1f)
			{
				this.FlowBumpStrength += dt * num5 * 0.5f * this.flowTimeScale;
				if (this.FlowBumpStrength < 0.1f)
				{
					this.FlowBumpStrength = 0.1f;
				}
			}
			float num6 = Mathf.Abs(dt * num5 * 0.05f) * this.fallIntensity;
			this.SnowDissolve = this.TargetValue(this.SnowDissolve, this.SnowDissolveCover, num6 * 2f);
			this.SnowBumpMicro = this.TargetValue(this.SnowBumpMicro, this.SnowBumpMicroCover, num6 * 0.1f);
			this.SnowSpecGloss.r = this.TargetValue(this.SnowSpecGloss.r, this.SnowSpecGlossCover.r, num6);
			this.SnowSpecGloss.g = this.TargetValue(this.SnowSpecGloss.g, this.SnowSpecGlossCover.g, num6);
			this.SnowSpecGloss.b = this.TargetValue(this.SnowSpecGloss.b, this.SnowSpecGlossCover.b, num6);
			this.SnowSpecGloss.a = this.TargetValue(this.SnowSpecGloss.a, this.SnowSpecGlossCover.a, num6);
			this.SnowGlitterColor.r = this.TargetValue(this.SnowGlitterColor.r, this.SnowGlitterColorCover.r, num6);
			this.SnowGlitterColor.g = this.TargetValue(this.SnowGlitterColor.g, this.SnowGlitterColorCover.g, num6);
			this.SnowGlitterColor.b = this.TargetValue(this.SnowGlitterColor.b, this.SnowGlitterColorCover.b, num6);
			this.SnowGlitterColor.a = this.TargetValue(this.SnowGlitterColor.a, this.SnowGlitterColorCover.a, num6);
			this.Frost -= dt * num5 * 0.3f;
			if (this.Frost > 1f)
			{
				this.Frost = 1f;
			}
			this.SnowLevel -= this.fallIntensity * (dt * num5 * 0.01f);
			if (this.SnowLevel > 1f)
			{
				this.SnowLevel = 1f;
			}
			if (this.AddWetWhenSnowPresent && this.WaterLevel < this.SnowLevel)
			{
				this.WaterLevel = this.SnowLevel;
			}
		}
		this.WaterLevel -= num * (this.temperature + 273f) * 0.001f * this.flowTimeScale * dt * num2;
		if (this.WaterLevel < 0f)
		{
			this.WaterLevel = 0f;
		}
		this.WetnessAmount -= num * (this.temperature + 273f) * 0.0003f * this.flowTimeScale * dt * num2;
		if (this.WetnessAmount < 0f)
		{
			this.WetnessAmount = 0f;
		}
		this.RefreshParticleSystem();
		bool flag = false;
		if (this.compareDelta(rainIntensity, this.RainIntensity))
		{
			flag = true;
		}
		else if (this.compareDelta(propA, this.temperature))
		{
			flag = true;
		}
		else if (this.compareDelta(propA2, this.flowTimeScale))
		{
			flag = true;
		}
		else if (this.compareDelta(flowBumpStrength, this.FlowBumpStrength))
		{
			flag = true;
		}
		else if (this.compareDelta(waterLevel, this.WaterLevel))
		{
			flag = true;
		}
		else if (this.compareDelta(wetnessAmount, this.WetnessAmount))
		{
			flag = true;
		}
		else if (this.compareDelta(snowLevel, this.SnowLevel))
		{
			flag = true;
		}
		else if (this.compareDelta(snowDissolve, this.SnowDissolve))
		{
			flag = true;
		}
		else if (this.compareDelta(snowBumpMicro, this.SnowBumpMicro))
		{
			flag = true;
		}
		else if (this.compareDelta(snowSpecGloss, this.SnowSpecGloss))
		{
			flag = true;
		}
		else if (this.compareDelta(snowGlitterColor, this.SnowGlitterColor))
		{
			flag = true;
		}
		if (flag)
		{
			this.SetupIt();
		}
	}

	private bool compareDelta(float propA, float propB)
	{
		return Mathf.Abs(propA - propB) > 1E-06f;
	}

	private bool compareDelta(Color propA, Color propB)
	{
		return Mathf.Abs(propA.r - propB.r) > 1E-06f || Mathf.Abs(propA.g - propB.g) > 1E-06f || Mathf.Abs(propA.b - propB.b) > 1E-06f || Mathf.Abs(propA.a - propB.a) > 1E-06f;
	}

	private float TargetValue(float val, float target_val, float delta)
	{
		if (val < target_val)
		{
			val += delta;
			if (val > target_val)
			{
				val = target_val;
			}
		}
		else if (val > target_val)
		{
			val -= delta;
			if (val < target_val)
			{
				val = target_val;
			}
		}
		return val;
	}

	public void RefreshParticleSystem()
	{
		if (this.paricleSystemActive != this.UseParticleSystem)
		{
			if (this.rainGameObject)
			{
				this.rainGameObject.SetActive(this.UseParticleSystem);
			}
			if (this.snowGameObject)
			{
				this.snowGameObject.SetActive(this.UseParticleSystem);
			}
			this.paricleSystemActive = this.UseParticleSystem;
		}
		if (this.UseParticleSystem)
		{
			if (this.rainGameObject != null)
			{
				this.rainGameObject.transform.position = base.transform.position + Vector3.up * 3f;
				if (this.psRain == null)
				{
					this.psRain = this.rainGameObject.GetComponent<ParticleSystem>();
				}
			}
			if (this.snowGameObject != null)
			{
				this.snowGameObject.transform.position = base.transform.position + Vector3.up * 3f;
				if (this.psSnow == null)
				{
					this.psSnow = this.snowGameObject.GetComponent<ParticleSystem>();
				}
			}
			if (this.psRain != null)
			{
				ParticleSystem.EmissionModule emission = this.psRain.emission;
				ParticleSystem.MinMaxCurve rateOverTime = new ParticleSystem.MinMaxCurve(this.fallIntensity * 3000f * Mathf.Clamp01(this.temperature + 1f));
				emission.rateOverTime = rateOverTime;
			}
			if (this.psSnow != null)
			{
				ParticleSystem.EmissionModule emission2 = this.psSnow.emission;
				ParticleSystem.MinMaxCurve rateOverTime2 = new ParticleSystem.MinMaxCurve(this.fallIntensity * 3000f * Mathf.Clamp01(1f - this.temperature));
				emission2.rateOverTime = rateOverTime2;
			}
		}
	}

	public const float DEFROST_RATE = 0.3f;

	public const float RAIN_DAMP_ON_FREEZE_RATE = 0.2f;

	public const float FROZEN_FLOW_BUMP_STRENGTH = 0.1f;

	public const float FROST_RATE = 0.3f;

	public const float FROST_RATE_BUMP = 0.5f;

	public const float RAIN_TO_WATER_LEVEL_RATE = 2f;

	public const float RAIN_TO_WET_AMOUNT_RATE = 3f;

	public const float WATER_EVAPORATION_RATE = 0.001f;

	public const float WET_EVAPORATION_RATE = 0.0003f;

	public const float SNOW_FREEZE_RATE = 0.05f;

	public const float SNOW_INCREASE_RATE = 0.01f;

	public const float SNOW_MELT_RATE = 0.03f;

	public const float SNOW_MELT_RATE_BY_RAIN = 0.05f;

	public const float SNOW_DECREASE_RATE = 0.01f;

	[Header("Global Water & Rain")]
	[Tooltip("You can control global water level (multiplied by material value)")]
	[Range(0f, 1f)]
	public float WaterLevel = 1f;

	[Tooltip("You can control global wetness (multiplied by material value)")]
	[Range(0f, 1f)]
	public float WetnessAmount = 1f;

	[Tooltip("Time scale for flow animation")]
	public float flowTimeScale = 1f;

	[Tooltip("Multiplier of water flow ripple normalmap")]
	[Range(0f, 1f)]
	public float FlowBumpStrength = 1f;

	[Tooltip("You can control global rain intensity")]
	[Range(0f, 1f)]
	public float RainIntensity = 1f;

	[Header("Global Snow")]
	[Tooltip("You can control global snow")]
	[Range(0f, 1f)]
	public float SnowLevel = 1f;

	[Tooltip("You can control global frost")]
	[Range(0f, 1f)]
	public float Frost = 1f;

	[Tooltip("Global snow dissolve value")]
	[Range(0f, 4f)]
	public float SnowDissolve = 2f;

	[Tooltip("Global snow dissolve value")]
	[Range(0.001f, 0.2f)]
	public float SnowBumpMicro = 0.08f;

	[Tooltip("Global snow spec (RGB) & Gloss (A)")]
	public Color SnowSpecGloss = new Color(0.1f, 0.1f, 0.1f, 0.15f);

	[Tooltip("Global snow glitter color/spec boost")]
	public Color SnowGlitterColor = new Color(0.8f, 0.8f, 0.8f, 0.2f);

	[Header("Global Snow - cover state")]
	[HideInInspector]
	[Range(0f, 4f)]
	public float SnowDissolveCover = 2f;

	[Tooltip("Global snow dissolve value")]
	[HideInInspector]
	[Range(0.001f, 0.2f)]
	public float SnowBumpMicroCover = 0.08f;

	[Tooltip("Global snow spec (RGB) & Gloss (A)")]
	[HideInInspector]
	public Color SnowSpecGlossCover = new Color(0.1f, 0.1f, 0.1f, 0.15f);

	[Tooltip("Global snow glitter color/spec boost")]
	[HideInInspector]
	public Color SnowGlitterColorCover = new Color(0.8f, 0.8f, 0.8f, 0.2f);

	[Header("Global Snow - melt state")]
	[HideInInspector]
	[Range(0f, 4f)]
	public float SnowDissolveMelt = 0.3f;

	[Tooltip("Global snow dissolve value")]
	[HideInInspector]
	[Range(0.001f, 0.2f)]
	public float SnowBumpMicroMelt = 0.02f;

	[Tooltip("Global snow spec (RGB) & Gloss (A)")]
	[HideInInspector]
	public Color SnowSpecGlossMelt = new Color(0.15f, 0.15f, 0.15f, 0.6f);

	[Tooltip("Global snow glitter color/spec boost")]
	[HideInInspector]
	public Color SnowGlitterColorMelt = new Color(0.1f, 0.1f, 0.1f, 0.03f);

	[Header("Rainfall/snowfall controller")]
	public bool Simulate;

	[Range(0f, 1f)]
	public float fallIntensity;

	[Tooltip("Temperature (influences melt/freeze/evaporation speed) - 0 means water freeze")]
	[Range(-50f, 50f)]
	public float temperature = 20f;

	[Tooltip("Wind (1 means 4x faster evaporation and freeze rate)")]
	[Range(0f, 1f)]
	public float wind;

	[Tooltip("Speed of surface state change due to the weather dynamics")]
	[Range(0f, 1f)]
	public float weatherTimeScale = 1f;

	[Tooltip("We won't melt ice nor decrease water level while snow level is >5%")]
	public bool FreezeWetWhenSnowPresent = true;

	[Tooltip("Increase global Water level when snow appears")]
	public bool AddWetWhenSnowPresent = true;

	[Space(10f)]
	[Tooltip("Set to show and adjust below particle systems")]
	public bool UseParticleSystem = true;

	[Tooltip("GameObject with particle system attached controlling rain")]
	public GameObject rainGameObject;

	[Tooltip("GameObject with particle system attached controlling snow")]
	public GameObject snowGameObject;

	private Vector4 __Time;

	private float lTime;

	private bool paricleSystemActive;

	private ParticleSystem psRain;

	private ParticleSystem psSnow;
}
