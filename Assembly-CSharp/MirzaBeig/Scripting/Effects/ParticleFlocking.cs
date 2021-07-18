using System;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects
{
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleFlocking : MonoBehaviour
	{
		private void Start()
		{
			this.particleSystem = base.GetComponent<ParticleSystem>();
			this.particleSystemMainModule = this.particleSystem.main;
		}

		private void OnBecameVisible()
		{
			this.visible = true;
		}

		private void OnBecameInvisible()
		{
			this.visible = false;
		}

		private void buildVoxelGrid()
		{
			int num = this.voxelsPerAxis * this.voxelsPerAxis * this.voxelsPerAxis;
			this.voxels = new ParticleFlocking.Voxel[num];
			float num2 = this.voxelVolume / (float)this.voxelsPerAxis;
			float num3 = num2 / 2f;
			float num4 = this.voxelVolume / 2f;
			Vector3 position = base.transform.position;
			int num5 = 0;
			for (int i = 0; i < this.voxelsPerAxis; i++)
			{
				float x = -num4 + num3 + (float)i * num2;
				for (int j = 0; j < this.voxelsPerAxis; j++)
				{
					float y = -num4 + num3 + (float)j * num2;
					for (int k = 0; k < this.voxelsPerAxis; k++)
					{
						float z = -num4 + num3 + (float)k * num2;
						this.voxels[num5].particleCount = 0;
						this.voxels[num5].bounds = new Bounds(position + new Vector3(x, y, z), Vector3.one * num2);
						num5++;
					}
				}
			}
		}

		private void LateUpdate()
		{
			if (this.alwaysUpdate || this.visible)
			{
				if (this.useVoxels)
				{
					int num = this.voxelsPerAxis * this.voxelsPerAxis * this.voxelsPerAxis;
					if (this.voxels == null || this.voxels.Length < num)
					{
						this.buildVoxelGrid();
					}
				}
				int maxParticles = this.particleSystemMainModule.maxParticles;
				if (this.particles == null || this.particles.Length < maxParticles)
				{
					this.particles = new ParticleSystem.Particle[maxParticles];
					this.particlePositions = new Vector3[maxParticles];
					if (this.useVoxels)
					{
						for (int i = 0; i < this.voxels.Length; i++)
						{
							this.voxels[i].particles = new int[maxParticles];
						}
					}
				}
				this.timer += Time.deltaTime;
				if (this.timer >= this.delay)
				{
					float num2 = this.timer;
					this.timer = 0f;
					this.particleSystem.GetParticles(this.particles);
					int particleCount = this.particleSystem.particleCount;
					float d = this.cohesion * num2;
					float num3 = this.separation * num2;
					for (int j = 0; j < particleCount; j++)
					{
						this.particlePositions[j] = this.particles[j].position;
					}
					if (this.useVoxels)
					{
						int num4 = this.voxels.Length;
						float num5 = this.voxelVolume / (float)this.voxelsPerAxis;
						for (int k = 0; k < particleCount; k++)
						{
							for (int l = 0; l < num4; l++)
							{
								if (this.voxels[l].bounds.Contains(this.particlePositions[k]))
								{
									this.voxels[l].particles[this.voxels[l].particleCount] = k;
									ParticleFlocking.Voxel[] array = this.voxels;
									int num6 = l;
									array[num6].particleCount = array[num6].particleCount + 1;
									break;
								}
							}
						}
						for (int m = 0; m < num4; m++)
						{
							if (this.voxels[m].particleCount > 1)
							{
								for (int n = 0; n < this.voxels[m].particleCount; n++)
								{
									Vector3 a = this.particlePositions[this.voxels[m].particles[n]];
									Vector3 a2;
									if (this.voxelLocalCenterFromBounds)
									{
										a2 = this.voxels[m].bounds.center - this.particlePositions[this.voxels[m].particles[n]];
									}
									else
									{
										for (int num7 = 0; num7 < this.voxels[m].particleCount; num7++)
										{
											if (num7 != n)
											{
												a += this.particlePositions[this.voxels[m].particles[num7]];
											}
										}
										a /= (float)this.voxels[m].particleCount;
										a2 = a - this.particlePositions[this.voxels[m].particles[n]];
									}
									float sqrMagnitude = a2.sqrMagnitude;
									a2.Normalize();
									Vector3 a3 = Vector3.zero;
									a3 += a2 * d;
									a3 -= a2 * ((1f - sqrMagnitude / num5) * num3);
									Vector3 velocity = this.particles[this.voxels[m].particles[n]].velocity;
									velocity.x += a3.x;
									velocity.y += a3.y;
									velocity.z += a3.z;
									this.particles[this.voxels[m].particles[n]].velocity = velocity;
								}
								this.voxels[m].particleCount = 0;
							}
						}
					}
					else
					{
						float num8 = this.maxDistance * this.maxDistance;
						for (int num9 = 0; num9 < particleCount; num9++)
						{
							int num10 = 1;
							Vector3 a4 = this.particlePositions[num9];
							for (int num11 = 0; num11 < particleCount; num11++)
							{
								if (num11 != num9)
								{
									Vector3 vector;
									vector.x = this.particlePositions[num9].x - this.particlePositions[num11].x;
									vector.y = this.particlePositions[num9].y - this.particlePositions[num11].y;
									vector.z = this.particlePositions[num9].z - this.particlePositions[num11].z;
									float num12 = Vector3.SqrMagnitude(vector);
									if (num12 <= num8)
									{
										num10++;
										a4 += this.particlePositions[num11];
									}
								}
							}
							if (num10 != 1)
							{
								a4 /= (float)num10;
								Vector3 a5 = a4 - this.particlePositions[num9];
								float sqrMagnitude2 = a5.sqrMagnitude;
								a5.Normalize();
								Vector3 a6 = Vector3.zero;
								a6 += a5 * d;
								a6 -= a5 * ((1f - sqrMagnitude2 / num8) * num3);
								Vector3 velocity2 = this.particles[num9].velocity;
								velocity2.x += a6.x;
								velocity2.y += a6.y;
								velocity2.z += a6.z;
								this.particles[num9].velocity = velocity2;
							}
						}
					}
					this.particleSystem.SetParticles(this.particles, particleCount);
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
			float num = this.voxelVolume / (float)this.voxelsPerAxis;
			float num2 = num / 2f;
			float num3 = this.voxelVolume / 2f;
			Vector3 position = base.transform.position;
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(position, Vector3.one * this.voxelVolume);
			Gizmos.color = Color.white;
			for (int i = 0; i < this.voxelsPerAxis; i++)
			{
				float x = -num3 + num2 + (float)i * num;
				for (int j = 0; j < this.voxelsPerAxis; j++)
				{
					float y = -num3 + num2 + (float)j * num;
					for (int k = 0; k < this.voxelsPerAxis; k++)
					{
						float z = -num3 + num2 + (float)k * num;
						Gizmos.DrawWireCube(position + new Vector3(x, y, z), Vector3.one * num);
					}
				}
			}
		}

		[Header("N^2 Mode Settings")]
		public float maxDistance = 0.5f;

		[Header("Forces")]
		public float cohesion = 0.5f;

		public float separation = 0.25f;

		[Header("Voxel Mode Settings")]
		public bool useVoxels = true;

		public bool voxelLocalCenterFromBounds = true;

		public float voxelVolume = 8f;

		public int voxelsPerAxis = 5;

		private int previousVoxelsPerAxisValue;

		private ParticleFlocking.Voxel[] voxels;

		private ParticleSystem particleSystem;

		private ParticleSystem.Particle[] particles;

		private Vector3[] particlePositions;

		private ParticleSystem.MainModule particleSystemMainModule;

		[Header("General Performance Settings")]
		[Range(0f, 1f)]
		public float delay;

		private float timer;

		public bool alwaysUpdate;

		private bool visible;

		public struct Voxel
		{
			public Bounds bounds;

			public int[] particles;

			public int particleCount;
		}
	}
}
