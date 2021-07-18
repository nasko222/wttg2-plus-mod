using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[AddComponentMenu("UBER/Deferred Params")]
[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
[ExecuteInEditMode]
public class UBER_DeferredParams : MonoBehaviour
{
	private void Start()
	{
		this.SetupTranslucencyValues();
	}

	public void OnValidate()
	{
		this.SetupTranslucencyValues();
	}

	public void SetupTranslucencyValues()
	{
		if (this.TranslucencyPropsTex == null)
		{
			this.TranslucencyPropsTex = new Texture2D(4, 3, TextureFormat.RGBAFloat, false, true);
			this.TranslucencyPropsTex.anisoLevel = 0;
			this.TranslucencyPropsTex.filterMode = FilterMode.Point;
			this.TranslucencyPropsTex.wrapMode = TextureWrapMode.Clamp;
			this.TranslucencyPropsTex.hideFlags = HideFlags.HideAndDontSave;
		}
		Shader.SetGlobalTexture("_UBERTranslucencySetup", this.TranslucencyPropsTex);
		byte[] array = new byte[192];
		this.EncodeRGBAFloatTo16Bytes(this.TranslucencyColor1.r, this.TranslucencyColor1.g, this.TranslucencyColor1.b, this.Strength1, array, 0, 0);
		this.EncodeRGBAFloatTo16Bytes(this.PointLightsDirectionality1, this.Constant1, this.Scattering1, this.SpotExponent1, array, 0, 1);
		this.EncodeRGBAFloatTo16Bytes(this.SuppressShadows1, this.NdotLReduction1, 1f, 1f, array, 0, 2);
		this.EncodeRGBAFloatTo16Bytes(this.TranslucencyColor2.r, this.TranslucencyColor2.g, this.TranslucencyColor2.b, this.Strength2, array, 1, 0);
		this.EncodeRGBAFloatTo16Bytes(this.PointLightsDirectionality2, this.Constant2, this.Scattering2, this.SpotExponent2, array, 1, 1);
		this.EncodeRGBAFloatTo16Bytes(this.SuppressShadows2, this.NdotLReduction2, 1f, 1f, array, 1, 2);
		this.EncodeRGBAFloatTo16Bytes(this.TranslucencyColor3.r, this.TranslucencyColor3.g, this.TranslucencyColor3.b, this.Strength3, array, 2, 0);
		this.EncodeRGBAFloatTo16Bytes(this.PointLightsDirectionality3, this.Constant3, this.Scattering3, this.SpotExponent3, array, 2, 1);
		this.EncodeRGBAFloatTo16Bytes(this.SuppressShadows3, this.NdotLReduction3, 1f, 1f, array, 2, 2);
		this.EncodeRGBAFloatTo16Bytes(this.TranslucencyColor4.r, this.TranslucencyColor4.g, this.TranslucencyColor4.b, this.Strength4, array, 3, 0);
		this.EncodeRGBAFloatTo16Bytes(this.PointLightsDirectionality4, this.Constant4, this.Scattering4, this.SpotExponent4, array, 3, 1);
		this.EncodeRGBAFloatTo16Bytes(this.SuppressShadows4, this.NdotLReduction4, 1f, 1f, array, 3, 2);
		this.TranslucencyPropsTex.LoadRawTextureData(array);
		this.TranslucencyPropsTex.Apply();
	}

	private void EncodeRGBAFloatTo16Bytes(float r, float g, float b, float a, byte[] rawTexdata, int idx_u, int idx_v)
	{
		int num = idx_v * 4 * 16 + idx_u * 16;
		UBER_RGBA_ByteArray uber_RGBA_ByteArray = default(UBER_RGBA_ByteArray);
		uber_RGBA_ByteArray.R = r;
		uber_RGBA_ByteArray.G = g;
		uber_RGBA_ByteArray.B = b;
		uber_RGBA_ByteArray.A = a;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte0;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte1;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte2;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte3;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte4;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte5;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte6;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte7;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte8;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte9;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte10;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte11;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte12;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte13;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte14;
		rawTexdata[num++] = uber_RGBA_ByteArray.Byte15;
	}

	public void OnEnable()
	{
		this.SetupTranslucencyValues();
		if (this.NotifyDecals())
		{
			return;
		}
		if (this.mycam == null)
		{
			this.mycam = base.GetComponent<Camera>();
			if (this.mycam == null)
			{
				return;
			}
		}
		this.Initialize();
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.SetupCam));
	}

	public void OnDisable()
	{
		this.NotifyDecals();
		this.Cleanup();
	}

	public void OnDestroy()
	{
		this.NotifyDecals();
		this.Cleanup();
	}

	private bool NotifyDecals()
	{
		Type type = Type.GetType("UBERDecalSystem.DecalManager");
		if (type != null)
		{
			bool flag = UnityEngine.Object.FindObjectOfType(type) != null && UnityEngine.Object.FindObjectOfType(type) is MonoBehaviour && (UnityEngine.Object.FindObjectOfType(type) as MonoBehaviour).enabled;
			if (flag)
			{
				(UnityEngine.Object.FindObjectOfType(type) as MonoBehaviour).Invoke("OnDisable", 0f);
				(UnityEngine.Object.FindObjectOfType(type) as MonoBehaviour).Invoke("OnEnable", 0f);
				return true;
			}
		}
		return false;
	}

	private void Cleanup()
	{
		if (this.TranslucencyPropsTex)
		{
			UnityEngine.Object.DestroyImmediate(this.TranslucencyPropsTex);
			this.TranslucencyPropsTex = null;
		}
		if (this.combufPreLight != null)
		{
			if (this.mycam)
			{
				this.mycam.RemoveCommandBuffer(CameraEvent.BeforeReflections, this.combufPreLight);
				this.mycam.RemoveCommandBuffer(CameraEvent.AfterLighting, this.combufPostLight);
			}
			foreach (Camera camera in this.sceneCamsWithBuffer)
			{
				if (camera)
				{
					camera.RemoveCommandBuffer(CameraEvent.BeforeReflections, this.combufPreLight);
					camera.RemoveCommandBuffer(CameraEvent.AfterLighting, this.combufPostLight);
				}
			}
		}
		this.sceneCamsWithBuffer.Clear();
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(this.SetupCam));
	}

	private void SetupCam(Camera cam)
	{
		bool flag = false;
		if (cam == this.mycam || flag)
		{
			this.RefreshComBufs(cam, flag);
		}
	}

	public void RefreshComBufs(Camera cam, bool isSceneCam)
	{
		if (cam && this.combufPreLight != null && this.combufPostLight != null)
		{
			CommandBuffer[] commandBuffers = cam.GetCommandBuffers(CameraEvent.BeforeReflections);
			bool flag = false;
			foreach (CommandBuffer commandBuffer in commandBuffers)
			{
				if (commandBuffer.name == this.combufPreLight.name)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				cam.AddCommandBuffer(CameraEvent.BeforeReflections, this.combufPreLight);
				cam.AddCommandBuffer(CameraEvent.AfterLighting, this.combufPostLight);
				if (isSceneCam)
				{
					this.sceneCamsWithBuffer.Add(cam);
				}
			}
		}
	}

	public void Initialize()
	{
		if (this.combufPreLight == null)
		{
			int nameID = Shader.PropertyToID("_UBERPropsBuffer");
			if (this.CopyPropsMat == null)
			{
				if (this.CopyPropsMat != null)
				{
					UnityEngine.Object.DestroyImmediate(this.CopyPropsMat);
				}
				this.CopyPropsMat = new Material(Shader.Find("Hidden/UBER_CopyPropsTexture"));
				this.CopyPropsMat.hideFlags = HideFlags.DontSave;
			}
			this.combufPreLight = new CommandBuffer();
			this.combufPreLight.name = "UBERPropsPrelight";
			this.combufPreLight.GetTemporaryRT(nameID, -1, -1, 0, FilterMode.Point, RenderTextureFormat.RHalf);
			this.combufPreLight.Blit(BuiltinRenderTextureType.CameraTarget, nameID, this.CopyPropsMat);
			this.combufPostLight = new CommandBuffer();
			this.combufPostLight.name = "UBERPropsPostlight";
			this.combufPostLight.ReleaseTemporaryRT(nameID);
		}
	}

	[Header("Translucency setup 1")]
	[ColorUsage(false)]
	public Color TranslucencyColor1 = new Color(1f, 1f, 1f, 1f);

	[Tooltip("You can control strength per light using its color alpha (first enable in UBER config file)")]
	public float Strength1 = 4f;

	[Range(0f, 1f)]
	public float PointLightsDirectionality1 = 0.7f;

	[Range(0f, 0.5f)]
	public float Constant1 = 0.1f;

	[Range(0f, 0.3f)]
	public float Scattering1 = 0.05f;

	[Range(0f, 100f)]
	public float SpotExponent1 = 30f;

	[Range(0f, 20f)]
	public float SuppressShadows1 = 0.5f;

	[Range(0f, 1f)]
	public float NdotLReduction1;

	[Space]
	[Header("Translucency setup 2")]
	[ColorUsage(false)]
	public Color TranslucencyColor2 = new Color(1f, 1f, 1f, 1f);

	[Tooltip("You can control strength per light using its color alpha (first enable in UBER config file)")]
	public float Strength2 = 4f;

	[Range(0f, 1f)]
	public float PointLightsDirectionality2 = 0.7f;

	[Range(0f, 0.5f)]
	public float Constant2 = 0.1f;

	[Range(0f, 0.3f)]
	public float Scattering2 = 0.05f;

	[Range(0f, 100f)]
	public float SpotExponent2 = 30f;

	[Range(0f, 20f)]
	public float SuppressShadows2 = 0.5f;

	[Range(0f, 1f)]
	public float NdotLReduction2;

	[Space]
	[Header("Translucency setup 3")]
	[ColorUsage(false)]
	public Color TranslucencyColor3 = new Color(1f, 1f, 1f, 1f);

	[Tooltip("You can control strength per light using its color alpha (first enable in UBER config file)")]
	public float Strength3 = 4f;

	[Range(0f, 1f)]
	public float PointLightsDirectionality3 = 0.7f;

	[Range(0f, 0.5f)]
	public float Constant3 = 0.1f;

	[Range(0f, 0.3f)]
	public float Scattering3 = 0.05f;

	[Range(0f, 100f)]
	public float SpotExponent3 = 30f;

	[Range(0f, 20f)]
	public float SuppressShadows3 = 0.5f;

	[Range(0f, 1f)]
	public float NdotLReduction3;

	[Space]
	[Header("Translucency setup 4")]
	[ColorUsage(false)]
	public Color TranslucencyColor4 = new Color(1f, 1f, 1f, 1f);

	[Tooltip("You can control strength per light using its color alpha (first enable in UBER config file)")]
	public float Strength4 = 4f;

	[Range(0f, 1f)]
	public float PointLightsDirectionality4 = 0.7f;

	[Range(0f, 0.5f)]
	public float Constant4 = 0.1f;

	[Range(0f, 0.3f)]
	public float Scattering4 = 0.05f;

	[Range(0f, 100f)]
	public float SpotExponent4 = 30f;

	[Range(0f, 20f)]
	public float SuppressShadows4 = 0.5f;

	[Range(0f, 1f)]
	public float NdotLReduction4;

	private Camera mycam;

	private CommandBuffer combufPreLight;

	private CommandBuffer combufPostLight;

	private Material CopyPropsMat;

	private bool UBERPresenceChecked;

	private bool UBERPresent;

	[HideInInspector]
	public Texture2D TranslucencyPropsTex;

	private HashSet<Camera> sceneCamsWithBuffer = new HashSet<Camera>();
}
