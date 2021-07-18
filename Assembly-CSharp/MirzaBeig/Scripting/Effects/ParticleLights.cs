using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects
{
	[RequireComponent(typeof(ParticleSystem))]
	[Serializable]
	public class ParticleLights : MonoBehaviour
	{
		private void Awake()
		{
		}

		private void Start()
		{
			this.ps = base.GetComponent<ParticleSystem>();
			this.template = new GameObject();
			this.template.transform.SetParent(base.transform);
			this.template.name = "Template";
		}

		private void Update()
		{
		}

		private void LateUpdate()
		{
			ParticleSystem.Particle[] array = new ParticleSystem.Particle[this.ps.particleCount];
			int particles = this.ps.GetParticles(array);
			if (this.lights.Count != particles)
			{
				for (int i = 0; i < this.lights.Count; i++)
				{
					UnityEngine.Object.Destroy(this.lights[i].gameObject);
				}
				this.lights.Clear();
				for (int j = 0; j < particles; j++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.template, base.transform);
					gameObject.name = "- " + (j + 1).ToString();
					this.lights.Add(gameObject.AddComponent<Light>());
				}
			}
			bool flag = this.ps.main.simulationSpace == ParticleSystemSimulationSpace.World;
			for (int k = 0; k < particles; k++)
			{
				ParticleSystem.Particle particle = array[k];
				Light light = this.lights[k];
				light.range = particle.GetCurrentSize(this.ps) * this.scale;
				light.color = Color.Lerp(this.colour, particle.GetCurrentColor(this.ps), this.colourFromParticle);
				light.intensity = this.intensity;
				light.shadows = this.shadows;
				light.transform.position = ((!flag) ? this.ps.transform.TransformPoint(particle.position) : particle.position);
			}
		}

		private ParticleSystem ps;

		private List<Light> lights = new List<Light>();

		public float scale = 2f;

		[Range(0f, 8f)]
		public float intensity = 8f;

		public Color colour = Color.white;

		[Range(0f, 1f)]
		public float colourFromParticle = 1f;

		public LightShadows shadows;

		private GameObject template;
	}
}
