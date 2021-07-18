using System;
using System.Threading;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects
{
	[Serializable]
	public class ParticleAffectorMT : MonoBehaviour
	{
		private void Awake()
		{
		}

		private void Start()
		{
			this.particleSystem = base.GetComponent<ParticleSystem>();
			this.randomX = UnityEngine.Random.Range(-32f, 32f);
			this.randomY = UnityEngine.Random.Range(-32f, 32f);
			this.randomZ = UnityEngine.Random.Range(-32f, 32f);
			this.t = new Thread(new ThreadStart(this.process));
			this.t.Start();
			this.isDoneAssigning = true;
		}

		private void LateUpdate()
		{
			object obj = this.locker;
			lock (obj)
			{
				if (!this.processing && this.isDoneAssigning)
				{
					this.particles = new ParticleSystem.Particle[this.particleSystem.particleCount];
					this.particleSystem.GetParticles(this.particles);
					float time = Time.time;
					this.deltaTime = Time.deltaTime;
					this.offsetX = time * this.speed * this.randomX;
					this.offsetY = time * this.speed * this.randomY;
					this.offsetZ = time * this.speed * this.randomZ;
					this.processing = true;
					this.isDoneAssigning = false;
				}
			}
			if (this.t.ThreadState == ThreadState.Stopped)
			{
				this.t = new Thread(new ThreadStart(this.process));
				this.t.Start();
			}
			object obj2 = this.locker;
			lock (obj2)
			{
				if (!this.processing && !this.isDoneAssigning)
				{
					this.particleSystem.SetParticles(this.particles, this.particles.Length);
					this.isDoneAssigning = true;
				}
			}
		}

		private void process()
		{
			object obj = this.locker;
			lock (obj)
			{
				if (this.processing)
				{
					for (int i = 0; i < this.particles.Length; i++)
					{
						ParticleSystem.Particle particle = this.particles[i];
						Vector3 position = particle.position;
						Vector3 vector = new Vector3(Noise.perlin(this.offsetX + position.x, this.offsetX + position.y, this.offsetX + position.z), Noise.perlin(this.offsetY + position.x, this.offsetY + position.y, this.offsetY + position.z), Noise.perlin(this.offsetZ + position.x, this.offsetZ + position.y, this.offsetZ + position.z)) * this.force;
						vector *= this.deltaTime;
						particle.velocity += vector;
						this.particles[i] = particle;
					}
					this.processing = false;
				}
			}
		}

		private void OnDisable()
		{
		}

		private void OnApplicationQuit()
		{
		}

		public float force = 1f;

		public float speed = 1f;

		private ParticleSystem particleSystem;

		private ParticleSystem.Particle[] particles;

		private float randomX;

		private float randomY;

		private float randomZ;

		private float offsetX;

		private float offsetY;

		private float offsetZ;

		private float deltaTime;

		private Thread t;

		private readonly object locker = new object();

		private bool processing;

		private bool isDoneAssigning;
	}
}
