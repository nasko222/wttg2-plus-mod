using System;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects
{
	public class TurbulenceParticleAffector : ParticleAffector
	{
		protected override void Awake()
		{
			base.Awake();
		}

		protected override void Start()
		{
			base.Start();
			this.randomX = UnityEngine.Random.Range(-32f, 32f);
			this.randomY = UnityEngine.Random.Range(-32f, 32f);
			this.randomZ = UnityEngine.Random.Range(-32f, 32f);
		}

		protected override void Update()
		{
			this.time = Time.time;
			base.Update();
		}

		protected override void LateUpdate()
		{
			this.offsetX = this.time * this.speed + this.randomX;
			this.offsetY = this.time * this.speed + this.randomY;
			this.offsetZ = this.time * this.speed + this.randomZ;
			base.LateUpdate();
		}

		protected override Vector3 GetForce()
		{
			float num = this.parameters.particlePosition.x + this.offsetX;
			float num2 = this.parameters.particlePosition.y + this.offsetX;
			float num3 = this.parameters.particlePosition.z + this.offsetX;
			float num4 = this.parameters.particlePosition.x + this.offsetY;
			float num5 = this.parameters.particlePosition.y + this.offsetY;
			float num6 = this.parameters.particlePosition.z + this.offsetY;
			float num7 = this.parameters.particlePosition.x + this.offsetZ;
			float num8 = this.parameters.particlePosition.y + this.offsetZ;
			float num9 = this.parameters.particlePosition.z + this.offsetZ;
			Vector3 result;
			switch (this.noiseType)
			{
			case TurbulenceParticleAffector.NoiseType.PseudoPerlin:
			{
				float num10 = Mathf.PerlinNoise(num * this.frequency, num5 * this.frequency);
				float num11 = Mathf.PerlinNoise(num * this.frequency, num6 * this.frequency);
				float num12 = Mathf.PerlinNoise(num * this.frequency, num4 * this.frequency);
				num10 = Mathf.Lerp(-1f, 1f, num10);
				num11 = Mathf.Lerp(-1f, 1f, num11);
				num12 = Mathf.Lerp(-1f, 1f, num12);
				Vector3 a = Vector3.right * num10;
				Vector3 b = Vector3.up * num11;
				Vector3 b2 = Vector3.forward * num12;
				return a + b + b2;
			}
			case TurbulenceParticleAffector.NoiseType.Simplex:
				result.x = Noise.simplex(num * this.frequency, num2 * this.frequency, num3 * this.frequency);
				result.y = Noise.simplex(num4 * this.frequency, num5 * this.frequency, num6 * this.frequency);
				result.z = Noise.simplex(num7 * this.frequency, num8 * this.frequency, num9 * this.frequency);
				return result;
			case TurbulenceParticleAffector.NoiseType.OctavePerlin:
				result.x = Noise.octavePerlin(num, num2, num3, this.frequency, this.octaves, this.lacunarity, this.persistence);
				result.y = Noise.octavePerlin(num4, num5, num6, this.frequency, this.octaves, this.lacunarity, this.persistence);
				result.z = Noise.octavePerlin(num7, num8, num9, this.frequency, this.octaves, this.lacunarity, this.persistence);
				return result;
			case TurbulenceParticleAffector.NoiseType.OctaveSimplex:
				result.x = Noise.octaveSimplex(num, num2, num3, this.frequency, this.octaves, this.lacunarity, this.persistence);
				result.y = Noise.octaveSimplex(num4, num5, num6, this.frequency, this.octaves, this.lacunarity, this.persistence);
				result.z = Noise.octaveSimplex(num7, num8, num9, this.frequency, this.octaves, this.lacunarity, this.persistence);
				return result;
			}
			result.x = Noise.perlin(num * this.frequency, num2 * this.frequency, num3 * this.frequency);
			result.y = Noise.perlin(num4 * this.frequency, num5 * this.frequency, num6 * this.frequency);
			result.z = Noise.perlin(num7 * this.frequency, num8 * this.frequency, num9 * this.frequency);
			return result;
		}

		protected override void OnDrawGizmosSelected()
		{
			if (base.enabled)
			{
				base.OnDrawGizmosSelected();
			}
		}

		[Header("Affector Controls")]
		public float speed = 1f;

		[Range(0f, 8f)]
		public float frequency = 1f;

		public TurbulenceParticleAffector.NoiseType noiseType = TurbulenceParticleAffector.NoiseType.Perlin;

		[Header("Octave Variant-Only Controls")]
		[Range(1f, 8f)]
		public int octaves = 1;

		[Range(0f, 4f)]
		public float lacunarity = 2f;

		[Range(0f, 1f)]
		public float persistence = 0.5f;

		private float time;

		private float randomX;

		private float randomY;

		private float randomZ;

		private float offsetX;

		private float offsetY;

		private float offsetZ;

		public enum NoiseType
		{
			PseudoPerlin,
			Perlin,
			Simplex,
			OctavePerlin,
			OctaveSimplex
		}
	}
}
