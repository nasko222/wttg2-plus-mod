using System;
using UnityEngine;

namespace JBrothers.PreIntegratedSkinShader2.Demo
{
	public class DemoController : MonoBehaviour
	{
		private void Start()
		{
			this._MainTex = Shader.PropertyToID("_MainTex");
			this._ScatteringPower = Shader.PropertyToID("_ScatteringPower");
			if (!this.skyboxSphereShader)
			{
				Debug.LogWarning("no skybox preview shader");
				base.enabled = false;
				return;
			}
			this.skyboxSphereMaterial = new Material(this.skyboxSphereShader);
			if (!this.profileSphereShader)
			{
				Debug.LogWarning("no profile preview shader");
				base.enabled = false;
				return;
			}
			this.profileSphereMaterial = new Material(this.profileSphereShader);
			this.profileSphereMaterial.SetTexture("_LookupDirectSM2", Resources.Load<Texture2D>("PSSLookupDirectSM2"));
			if (!this.meshRenderer)
			{
				Debug.LogWarning("no mesh renderer");
				base.enabled = false;
				return;
			}
			this.materialOrig = this.meshRenderer.sharedMaterial;
			this.materialCopy = this.meshRenderer.material;
			this.scattering = this.materialCopy.GetFloat(this._ScatteringPower);
			this.profileSphereMaterial.SetFloat(this._ScatteringPower, 0.4f);
			this.skyboxSpheres = new DemoController.SkyboxSphere[this.skyboxes.Length];
			for (int i = 0; i < this.skyboxes.Length; i++)
			{
				Material material = this.skyboxes[i];
				DemoController.SkyboxSphere skyboxSphere = new DemoController.SkyboxSphere();
				if (!material)
				{
					Debug.LogWarning("no skybox material specified");
					base.enabled = false;
					return;
				}
				skyboxSphere.skybox = material;
				skyboxSphere.cube = this.bakeSkyboxMaterialToCube(this.cubemapResolution, material);
				this.skyboxSpheres[i] = skyboxSphere;
			}
			this.SelectSkybox(this.skyboxSpheres[0]);
			this.UpdateRelfectionProbeIfNecessary();
		}

		private void OnDestroy()
		{
			if (this.skyboxSpheres != null)
			{
				foreach (DemoController.SkyboxSphere skyboxSphere in this.skyboxSpheres)
				{
					UnityEngine.Object.Destroy(skyboxSphere.cube);
				}
			}
			if (this.skyboxSphereMaterial)
			{
				UnityEngine.Object.Destroy(this.skyboxSphereMaterial);
			}
			if (this.materialCopy)
			{
				UnityEngine.Object.Destroy(this.materialCopy);
			}
		}

		private void SelectSkybox(DemoController.SkyboxSphere sb)
		{
			this.selectedSkybox = sb;
			RenderSettings.skybox = sb.skybox;
		}

		private void Update()
		{
			this.UpdateRelfectionProbeIfNecessary();
		}

		private void OnGUI()
		{
			GUIStyle guistyle = new GUIStyle("label");
			guistyle.alignment = TextAnchor.MiddleCenter;
			guistyle.fontStyle = FontStyle.Bold;
			GUIStyle guistyle2 = new GUIStyle("label");
			guistyle2.alignment = TextAnchor.UpperLeft;
			guistyle2.fontStyle = FontStyle.Normal;
			int controlID = GUIUtility.GetControlID(FocusType.Passive);
			GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			});
			foreach (DemoController.SkyboxSphere skyboxSphere in this.skyboxSpheres)
			{
				Rect rect = GUILayoutUtility.GetRect(this.sphereSize, this.sphereSize, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				Rect rect2 = new Rect(rect.x, (float)Screen.height - rect.y - rect.height, rect.width, rect.height);
				bool flag = false;
				if (rect2.Contains(Input.mousePosition))
				{
					float num = rect.width * rect.height / 4f;
					float sqrMagnitude = (Input.mousePosition - rect2.center).sqrMagnitude;
					flag = (sqrMagnitude < num);
				}
				if (Event.current.type == EventType.Repaint)
				{
					float value = Mathf.Repeat(Time.time / 10f, 1f);
					this.skyboxSphereMaterial.SetFloat("_Alpha", (!flag) ? 0.5f : 1f);
					this.skyboxSphereMaterial.SetFloat("_Radius", (!flag) ? 0.4f : 0.5f);
					this.skyboxSphereMaterial.SetTexture("_Cube", skyboxSphere.cube);
					this.skyboxSphereMaterial.SetFloat("_Rotation", value);
					Graphics.DrawTexture(rect, Texture2D.whiteTexture, this.skyboxSphereMaterial);
				}
				if (flag)
				{
					GUI.Label(rect, skyboxSphere.skybox.name, guistyle);
				}
				if (flag && Input.GetMouseButtonDown(0))
				{
					this.SelectSkybox(skyboxSphere);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			});
			foreach (PreIntegratedSkinProfile preIntegratedSkinProfile in this.profiles)
			{
				Rect rect3 = GUILayoutUtility.GetRect(this.sphereSize, this.sphereSize, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				Rect rect4 = new Rect(rect3.x, (float)Screen.height - rect3.y - rect3.height, rect3.width, rect3.height);
				bool flag2 = false;
				if (rect4.Contains(Input.mousePosition))
				{
					float num2 = rect3.width * rect3.height / 4f;
					float sqrMagnitude2 = (Input.mousePosition - rect4.center).sqrMagnitude;
					flag2 = (sqrMagnitude2 < num2);
				}
				if (Event.current.type.Equals(EventType.Repaint))
				{
					float value2 = Mathf.Repeat(Time.time / 10f, 1f);
					this.profileSphereMaterial.SetFloat("_Alpha", (!flag2) ? 0.5f : 1f);
					this.profileSphereMaterial.SetFloat("_Radius", (!flag2) ? 0.4f : 0.5f);
					this.profileSphereMaterial.SetFloat("_Rotation", value2);
					preIntegratedSkinProfile.ApplyProfile(this.profileSphereMaterial);
					Graphics.DrawTexture(rect3, Texture2D.whiteTexture, this.profileSphereMaterial);
				}
				if (flag2)
				{
					GUI.Label(rect3, preIntegratedSkinProfile.name, guistyle);
				}
				if (flag2 && Input.GetMouseButtonDown(0))
				{
					preIntegratedSkinProfile.ApplyProfile(this.materialCopy);
				}
			}
			GUILayout.EndHorizontal();
			this.sun.enabled = GUILayout.Toggle(this.sun.enabled, "Direct light", new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label("Ambient intensity", guistyle2, new GUILayoutOption[0]);
			RenderSettings.ambientIntensity = GUILayout.HorizontalSlider(RenderSettings.ambientIntensity, 0f, 2f, new GUILayoutOption[0]);
			GUILayout.EndVertical();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label("Reflection intensity", guistyle2, new GUILayoutOption[0]);
			this.reflectionProbe.intensity = GUILayout.HorizontalSlider(this.reflectionProbe.intensity, 0f, 2f, new GUILayoutOption[0]);
			GUILayout.EndVertical();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label("Scattering", guistyle2, new GUILayoutOption[0]);
			float num3 = GUILayout.HorizontalSlider(this.scattering, 0f, 2f, new GUILayoutOption[0]);
			if (num3 != this.scattering)
			{
				this.scattering = num3;
				this.materialCopy.SetFloat(this._ScatteringPower, this.scattering);
			}
			GUILayout.EndVertical();
			bool flag3 = !object.ReferenceEquals(this.materialCopy.GetTexture(this._MainTex), Texture2D.whiteTexture);
			bool flag4 = GUILayout.Toggle(flag3, "Use diffuse texture", new GUILayoutOption[0]);
			if (flag4 != flag3)
			{
				if (flag4)
				{
					this.materialCopy.SetTexture(this._MainTex, this.materialOrig.GetTexture(this._MainTex));
				}
				else
				{
					this.materialCopy.SetTexture(this._MainTex, Texture2D.whiteTexture);
				}
			}
			GUILayout.EndVertical();
			Rect lastRect = GUILayoutUtility.GetLastRect();
			switch (Event.current.GetTypeForControl(controlID))
			{
			case EventType.MouseDown:
				if (lastRect.Contains(Event.current.mousePosition))
				{
					GUIUtility.hotControl = controlID;
					Event.current.Use();
				}
				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl == controlID)
				{
					GUIUtility.hotControl = 0;
					Event.current.Use();
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlID)
				{
					Event.current.Use();
				}
				break;
			case EventType.ScrollWheel:
				if (lastRect.Contains(Event.current.mousePosition))
				{
					Event.current.Use();
				}
				break;
			}
		}

		private void UpdateRelfectionProbeIfNecessary()
		{
			if (this.sun)
			{
				bool flag = false;
				if (!object.ReferenceEquals(this.probeBakedWithSkybox, this.selectedSkybox))
				{
					flag = true;
					this.probeBakedWithSkybox = this.selectedSkybox;
				}
				if (this.sun.transform.rotation != this.probeBakedWithSunRotation)
				{
					flag = true;
					this.probeBakedWithSunRotation = this.sun.transform.rotation;
				}
				if (flag && this.reflectionProbe.isActiveAndEnabled)
				{
					this.reflectionProbe.RenderProbe();
				}
			}
		}

		private Cubemap bakeSkyboxMaterialToCube(int size, Material skybox)
		{
			GameObject gameObject = new GameObject();
			Cubemap result;
			try
			{
				gameObject.SetActive(false);
				Skybox skybox2 = gameObject.AddComponent<Skybox>();
				skybox2.material = skybox;
				Cubemap cubemap = new Cubemap(size, TextureFormat.RGB24, false);
				Camera camera = gameObject.AddComponent<Camera>();
				camera.enabled = false;
				camera.clearFlags = CameraClearFlags.Skybox;
				camera.renderingPath = RenderingPath.Forward;
				camera.cullingMask = 0;
				camera.RenderToCubemap(cubemap);
				cubemap.Apply(false, true);
				result = cubemap;
			}
			finally
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			return result;
		}

		public Light sun;

		public ReflectionProbe reflectionProbe;

		public PreIntegratedSkinProfile[] profiles;

		public Material[] skyboxes;

		private DemoController.SkyboxSphere[] skyboxSpheres;

		private Quaternion probeBakedWithSunRotation = Quaternion.identity;

		private DemoController.SkyboxSphere probeBakedWithSkybox;

		private DemoController.SkyboxSphere selectedSkybox;

		public Shader skyboxSphereShader;

		private Material skyboxSphereMaterial;

		public Shader profileSphereShader;

		private Material profileSphereMaterial;

		public Renderer meshRenderer;

		private Material materialCopy;

		private Material materialOrig;

		private int _MainTex;

		public float sphereSize = 64f;

		public int cubemapResolution = 64;

		private float scattering;

		private int _ScatteringPower;

		[Serializable]
		private class SkyboxSphere
		{
			public Material skybox;

			public Cubemap cube;
		}
	}
}
