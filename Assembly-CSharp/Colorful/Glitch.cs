using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/glitch.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Glitch")]
	public class Glitch : BaseEffect
	{
		public bool IsActive
		{
			get
			{
				return this.m_Activated;
			}
		}

		protected override void Start()
		{
			base.Start();
			this.m_DurationTimerEnd = UnityEngine.Random.Range(this.RandomDuration.x, this.RandomDuration.y);
		}

		protected virtual void Update()
		{
			if (!this.RandomActivation)
			{
				return;
			}
			if (this.m_Activated)
			{
				this.m_DurationTimer += Time.deltaTime;
				if (this.m_DurationTimer >= this.m_DurationTimerEnd)
				{
					this.m_DurationTimer = 0f;
					this.m_Activated = false;
					this.m_EveryTimerEnd = UnityEngine.Random.Range(this.RandomEvery.x, this.RandomEvery.y);
				}
			}
			else
			{
				this.m_EveryTimer += Time.deltaTime;
				if (this.m_EveryTimer >= this.m_EveryTimerEnd)
				{
					this.m_EveryTimer = 0f;
					this.m_Activated = true;
					this.m_DurationTimerEnd = UnityEngine.Random.Range(this.RandomDuration.x, this.RandomDuration.y);
				}
			}
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.m_Activated)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.Mode == Glitch.GlitchingMode.Interferences)
			{
				this.DoInterferences(source, destination, this.SettingsInterferences);
			}
			else if (this.Mode == Glitch.GlitchingMode.Tearing)
			{
				this.DoTearing(source, destination, this.SettingsTearing);
			}
			else
			{
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.width, 0, RenderTextureFormat.ARGB32);
				this.DoTearing(source, temporary, this.SettingsTearing);
				this.DoInterferences(temporary, destination, this.SettingsInterferences);
				temporary.Release();
			}
		}

		protected virtual void DoInterferences(RenderTexture source, RenderTexture destination, Glitch.InterferenceSettings settings)
		{
			base.Material.SetVector("_Params", new Vector3(settings.Speed, settings.Density, settings.MaxDisplacement));
			Graphics.Blit(source, destination, base.Material, 0);
		}

		protected virtual void DoTearing(RenderTexture source, RenderTexture destination, Glitch.TearingSettings settings)
		{
			base.Material.SetVector("_Params", new Vector4(settings.Speed, settings.Intensity, settings.MaxDisplacement, settings.YuvOffset));
			int pass = 1;
			if (settings.AllowFlipping && settings.YuvColorBleeding)
			{
				pass = 4;
			}
			else if (settings.AllowFlipping)
			{
				pass = 2;
			}
			else if (settings.YuvColorBleeding)
			{
				pass = 3;
			}
			Graphics.Blit(source, destination, base.Material, pass);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Glitch";
		}

		[Tooltip("Automatically activate/deactivate the effect randomly.")]
		public bool RandomActivation;

		public Vector2 RandomEvery = new Vector2(1f, 2f);

		public Vector2 RandomDuration = new Vector2(1f, 2f);

		[Tooltip("Glitch type.")]
		public Glitch.GlitchingMode Mode;

		public Glitch.InterferenceSettings SettingsInterferences = new Glitch.InterferenceSettings();

		public Glitch.TearingSettings SettingsTearing = new Glitch.TearingSettings();

		protected bool m_Activated = true;

		protected float m_EveryTimer;

		protected float m_EveryTimerEnd;

		protected float m_DurationTimer;

		protected float m_DurationTimerEnd;

		public enum GlitchingMode
		{
			Interferences,
			Tearing,
			Complete
		}

		[Serializable]
		public class InterferenceSettings
		{
			public float Speed = 10f;

			public float Density = 8f;

			public float MaxDisplacement = 2f;
		}

		[Serializable]
		public class TearingSettings
		{
			public float Speed = 1f;

			[Range(0f, 1f)]
			public float Intensity = 0.25f;

			[Range(0f, 0.5f)]
			public float MaxDisplacement = 0.05f;

			public bool AllowFlipping;

			public bool YuvColorBleeding = true;

			[Range(-2f, 2f)]
			public float YuvOffset = 0.5f;
		}
	}
}
