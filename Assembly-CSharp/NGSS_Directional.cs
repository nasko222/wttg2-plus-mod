using System;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Light))]
[ExecuteInEditMode]
public class NGSS_Directional : MonoBehaviour
{
	private Light dirLight
	{
		get
		{
			if (this._dirLight == null)
			{
				this._dirLight = base.GetComponent<Light>();
			}
			return this._dirLight;
		}
	}

	private void OnDisable()
	{
		this.isInitialized = false;
		if (this.NGSS_KEEP_ONDISABLE)
		{
			return;
		}
		if (this.isGraphicSet)
		{
			this.isGraphicSet = false;
			GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/Internal-ScreenSpaceShadows"));
			GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseBuiltin);
		}
	}

	private void OnEnable()
	{
		if (this.IsNotSupported())
		{
			Debug.LogWarning("Unsupported graphics API, NGSS requires at least SM3.0 or higher and DX9 is not supported.", this);
			base.enabled = false;
			return;
		}
		this.Init();
	}

	private void Init()
	{
		if (this.isInitialized)
		{
			return;
		}
		if (!this.isGraphicSet)
		{
			GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseCustom);
			GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/NGSS_Directional"));
			this.isGraphicSet = true;
		}
		this.isInitialized = true;
	}

	private bool IsNotSupported()
	{
		return SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.PlayStationVita || SystemInfo.graphicsDeviceType == GraphicsDeviceType.N3DS;
	}

	private void Update()
	{
		if (this.NGSS_HARD_SHADOWS)
		{
			Shader.EnableKeyword("NGSS_HARD_SHADOWS_DIR");
			this.dirLight.shadows = LightShadows.Hard;
			return;
		}
		Shader.DisableKeyword("NGSS_HARD_SHADOWS_DIR");
		this.dirLight.shadows = LightShadows.Soft;
		this.NGSS_TEST_SAMPLERS = Mathf.Clamp(this.NGSS_TEST_SAMPLERS, 4, this.NGSS_FILTER_SAMPLERS / 2);
		Shader.SetGlobalFloat("NGSS_TEST_SAMPLERS_DIR", (float)this.NGSS_TEST_SAMPLERS);
		Shader.SetGlobalFloat("NGSS_FILTER_SAMPLERS_DIR", (float)this.NGSS_FILTER_SAMPLERS);
		Shader.SetGlobalFloat("NGSS_GLOBAL_SOFTNESS", this.NGSS_SOFTNESS / (QualitySettings.shadowDistance * 0.66f) * ((QualitySettings.shadowCascades != 2) ? ((QualitySettings.shadowCascades != 4) ? 0.25f : 1f) : 1.5f));
		Shader.SetGlobalFloat("NGSS_BANDING_TO_NOISE_RATIO_DIR", this.NGSS_NOISE);
		if (this.NGSS_PCSS_ENABLED)
		{
			float num = this.NGSS_PCSS_SOFTNESS_MIN * 0.1f;
			float num2 = this.NGSS_PCSS_SOFTNESS_MAX * 0.25f;
			Shader.SetGlobalFloat("NGSS_PCSS_FILTER_DIR_MIN", (num <= num2) ? num : num2);
			Shader.SetGlobalFloat("NGSS_PCSS_FILTER_DIR_MAX", (num2 >= num) ? num2 : num);
			Shader.EnableKeyword("NGSS_PCSS_FILTER_DIR");
		}
		else
		{
			Shader.DisableKeyword("NGSS_PCSS_FILTER_DIR");
		}
		this.GLOBAL_CASCADES_COUNT = ((this.GLOBAL_CASCADES_COUNT != 3) ? this.GLOBAL_CASCADES_COUNT : 4);
		this.GLOBAL_SHADOWS_DISTANCE = Mathf.Clamp(this.GLOBAL_SHADOWS_DISTANCE, 0f, this.GLOBAL_SHADOWS_DISTANCE);
		if (this.GLOBAL_SETTINGS_OVERRIDE)
		{
			QualitySettings.shadowDistance = this.GLOBAL_SHADOWS_DISTANCE;
			QualitySettings.shadowProjection = this.GLOBAL_SHADOWS_PROJECTION;
			this.dirLight.shadowCustomResolution = (int)this.GLOBAL_SHADOWS_RESOLUTION;
			if (this.GLOBAL_CASCADES_COUNT > 1)
			{
				QualitySettings.shadowCascades = this.GLOBAL_CASCADES_COUNT;
				QualitySettings.shadowCascade4Split = new Vector3(this.GLOBAL_CASCADES_SPLIT_VALUE, this.GLOBAL_CASCADES_SPLIT_VALUE * 2f, this.GLOBAL_CASCADES_SPLIT_VALUE * 2f * 2f);
				QualitySettings.shadowCascade2Split = this.GLOBAL_CASCADES_SPLIT_VALUE * 2f;
			}
		}
		if (this.GLOBAL_CASCADES_COUNT > 1)
		{
			Shader.SetGlobalFloat("NGSS_CASCADES_SOFTNESS_NORMALIZATION", this.NGSS_CASCADES_SOFTNESS_NORMALIZATION);
			Shader.SetGlobalFloat("NGSS_CASCADES_COUNT", (float)QualitySettings.shadowCascades);
			Shader.SetGlobalVector("NGSS_CASCADES_SPLITS", (QualitySettings.shadowCascades != 2) ? new Vector4(QualitySettings.shadowCascade4Split.x, QualitySettings.shadowCascade4Split.y, QualitySettings.shadowCascade4Split.z, 1f) : new Vector4(QualitySettings.shadowCascade2Split, 1f, 1f, 1f));
		}
		if (this.NGSS_CASCADES_BLENDING && this.GLOBAL_CASCADES_COUNT > 1)
		{
			Shader.EnableKeyword("NGSS_USE_CASCADE_BLENDING");
			Shader.SetGlobalFloat("NGSS_CASCADE_BLEND_DISTANCE", this.NGSS_CASCADES_BLENDING_VALUE * 0.125f);
		}
		else
		{
			Shader.DisableKeyword("NGSS_USE_CASCADE_BLENDING");
		}
	}

	[Header("MAIN SETTINGS")]
	[Tooltip("If disabled, NGSS Directional shadows replacement will be removed from Graphics settings when OnDisable is called in this component.")]
	public bool NGSS_KEEP_ONDISABLE = true;

	[Tooltip("Useful if you want to fallback to hard shadows at runtime without having to disable the component.")]
	public bool NGSS_HARD_SHADOWS;

	[Header(" ")]
	[Tooltip("Used to test blocker search and early bail out algorithms.\nRecommended values: Mobile = 16, Consoles = 24, Desktop VR = 32, Desktop High = 64, Desktop Ultra 128")]
	[Range(8f, 128f)]
	public int NGSS_TEST_SAMPLERS = 16;

	[Tooltip("Used for the final filtering of shadows.\nRecommended values: Mobile = 16, Consoles = 24, Desktop VR = 32, Desktop High = 64, Desktop Ultra 128")]
	[Range(8f, 128f)]
	public int NGSS_FILTER_SAMPLERS = 48;

	[Header(" ")]
	[Tooltip("If zero = 100% banding.\nIf one = 100% noise.")]
	[Range(0f, 2f)]
	public float NGSS_NOISE = 1f;

	[Header(" ")]
	[Tooltip("Overall softness for all shadows.")]
	[Range(0f, 2f)]
	public float NGSS_SOFTNESS = 1f;

	[Header("PCSS")]
	[Tooltip("PCSS Requires inline sampling and SM3.5, only available in Unity 2017.\nIt provides Area Light like soft-shadows.\nDisable it if you are looking for PCF filtering (uniform soft-shadows) which runs with SM3.0.")]
	public bool NGSS_PCSS_ENABLED = true;

	[Tooltip("PCSS softness when shadows is close to caster.")]
	[Range(0f, 2f)]
	public float NGSS_PCSS_SOFTNESS_MIN = 1f;

	[Tooltip("PCSS softness when shadows is far from caster.")]
	[Range(0f, 2f)]
	public float NGSS_PCSS_SOFTNESS_MAX = 1f;

	[Header(" ")]
	[Header("GLOBAL SETTINGS")]
	[Tooltip("Enable it to let NGSS_Directional control global shadows settings through this component.\nDisable it if you want to manage shadows settings through Unity Quality & Graphics Settings panel.")]
	public bool GLOBAL_SETTINGS_OVERRIDE;

	[Tooltip("Shadows projection.\nRecommeded StableFit as it helps stabilizing shadows as camera moves.")]
	public ShadowProjection GLOBAL_SHADOWS_PROJECTION = ShadowProjection.StableFit;

	[Tooltip("Shadows resolution.\nLow = 1024, Med = 2048, High = 4096, Ultra = 8192, Mega = 16384.")]
	public NGSS_Directional.ShadowsResolution GLOBAL_SHADOWS_RESOLUTION = NGSS_Directional.ShadowsResolution.High;

	[Tooltip("Sets the maximum distance at wich shadows are visible from camera.\nThis option affects your shadow distance in Quality Settings.")]
	public float GLOBAL_SHADOWS_DISTANCE = 150f;

	[Range(1f, 4f)]
	[Tooltip("Number of cascades the shadowmap will have.\nThis value affects your cascade counts in Quality Settings.")]
	public int GLOBAL_CASCADES_COUNT = 4;

	[Range(0.01f, 0.25f)]
	[Tooltip("Used for the cascade stitching algorithm. Compute cascades splits distribution exponentially in a x*2^n form.\nThis value affects your cascade splits in Quality Settings.")]
	public float GLOBAL_CASCADES_SPLIT_VALUE = 0.1f;

	[Header(" ")]
	[Tooltip("Blends cascades at seams intersection.\nAdditional overhead required for this option.")]
	public bool NGSS_CASCADES_BLENDING = true;

	[Tooltip("Tweak this value to adjust the blending transition between cascades.")]
	[Range(0f, 2f)]
	public float NGSS_CASCADES_BLENDING_VALUE = 1f;

	[Range(0f, 1f)]
	[Tooltip("If one, softness across cascades will be matched using splits distribution, resulting in realistic soft-ness over distance.\nIf zero the softness distribution will be based on cascade index, resulting in blurrier shadows over distance thus less realistic.")]
	public float NGSS_CASCADES_SOFTNESS_NORMALIZATION = 1f;

	private bool isInitialized;

	private bool isGraphicSet;

	private Light _dirLight;

	public enum ShadowsResolution
	{
		Low = 1024,
		Med = 2048,
		High = 4096,
		Ultra = 8192,
		Mega = 16384
	}
}
