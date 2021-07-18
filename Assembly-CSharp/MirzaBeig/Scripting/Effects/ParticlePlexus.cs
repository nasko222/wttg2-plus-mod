using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects
{
	[RequireComponent(typeof(ParticleSystem))]
	[AddComponentMenu("Effects/Particle Plexus")]
	public class ParticlePlexus : MonoBehaviour
	{
		private void Start()
		{
			this.particleSystem = base.GetComponent<ParticleSystem>();
			this.particleSystemMainModule = this.particleSystem.main;
			this._transform = base.transform;
		}

		private void OnDisable()
		{
			for (int i = 0; i < this.lineRenderers.Count; i++)
			{
				this.lineRenderers[i].enabled = false;
			}
		}

		private void OnBecameVisible()
		{
			this.visible = true;
		}

		private void OnBecameInvisible()
		{
			this.visible = false;
		}

		private void LateUpdate()
		{
			int num = this.lineRenderers.Count;
			if (num > this.maxLineRenderers)
			{
				for (int i = this.maxLineRenderers; i < num; i++)
				{
					UnityEngine.Object.Destroy(this.lineRenderers[i].gameObject);
				}
				this.lineRenderers.RemoveRange(this.maxLineRenderers, num - this.maxLineRenderers);
				num -= num - this.maxLineRenderers;
			}
			if (this.alwaysUpdate || this.visible)
			{
				int maxParticles = this.particleSystemMainModule.maxParticles;
				if (this.particles == null || this.particles.Length < maxParticles)
				{
					this.particles = new ParticleSystem.Particle[maxParticles];
					this.particlePositions = new Vector3[maxParticles];
					this.particleColours = new Color[maxParticles];
					this.particleSizes = new float[maxParticles];
				}
				this.timer += Time.deltaTime;
				if (this.timer >= this.delay)
				{
					this.timer = 0f;
					int num2 = 0;
					if (this.maxConnections > 0 && this.maxLineRenderers > 0)
					{
						this.particleSystem.GetParticles(this.particles);
						int particleCount = this.particleSystem.particleCount;
						float num3 = this.maxDistance * this.maxDistance;
						ParticleSystemSimulationSpace simulationSpace = this.particleSystemMainModule.simulationSpace;
						ParticleSystemScalingMode scalingMode = this.particleSystemMainModule.scalingMode;
						Transform customSimulationSpace = this.particleSystemMainModule.customSimulationSpace;
						Color startColor = this.lineRendererTemplate.startColor;
						Color endColor = this.lineRendererTemplate.endColor;
						float a = this.lineRendererTemplate.startWidth * this.lineRendererTemplate.widthMultiplier;
						float a2 = this.lineRendererTemplate.endWidth * this.lineRendererTemplate.widthMultiplier;
						for (int j = 0; j < particleCount; j++)
						{
							this.particlePositions[j] = this.particles[j].position;
							this.particleColours[j] = this.particles[j].GetCurrentColor(this.particleSystem);
							this.particleSizes[j] = this.particles[j].GetCurrentSize(this.particleSystem);
						}
						if (simulationSpace == ParticleSystemSimulationSpace.World)
						{
							for (int k = 0; k < particleCount; k++)
							{
								if (num2 == this.maxLineRenderers)
								{
									break;
								}
								Color b = this.particleColours[k];
								Color startColor2 = Color.LerpUnclamped(startColor, b, this.colourFromParticle);
								startColor2.a = Mathf.LerpUnclamped(startColor.a, b.a, this.alphaFromParticle);
								float startWidth = Mathf.LerpUnclamped(a, this.particleSizes[k], this.widthFromParticle);
								int num4 = 0;
								for (int l = k + 1; l < particleCount; l++)
								{
									Vector3 vector;
									vector.x = this.particlePositions[k].x - this.particlePositions[l].x;
									vector.y = this.particlePositions[k].y - this.particlePositions[l].y;
									vector.z = this.particlePositions[k].z - this.particlePositions[l].z;
									float num5 = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
									if (num5 <= num3)
									{
										LineRenderer lineRenderer;
										if (num2 == num)
										{
											lineRenderer = UnityEngine.Object.Instantiate<LineRenderer>(this.lineRendererTemplate, this._transform, false);
											this.lineRenderers.Add(lineRenderer);
											num++;
										}
										lineRenderer = this.lineRenderers[num2];
										lineRenderer.enabled = true;
										lineRenderer.SetPosition(0, this.particlePositions[k]);
										lineRenderer.SetPosition(1, this.particlePositions[l]);
										lineRenderer.startColor = startColor2;
										b = this.particleColours[l];
										Color endColor2 = Color.LerpUnclamped(endColor, b, this.colourFromParticle);
										endColor2.a = Mathf.LerpUnclamped(endColor.a, b.a, this.alphaFromParticle);
										lineRenderer.endColor = endColor2;
										lineRenderer.startWidth = startWidth;
										lineRenderer.endWidth = Mathf.LerpUnclamped(a2, this.particleSizes[l], this.widthFromParticle);
										num2++;
										num4++;
										if (num4 == this.maxConnections || num2 == this.maxLineRenderers)
										{
											break;
										}
									}
								}
							}
						}
						else
						{
							Vector3 vector2 = Vector3.zero;
							Quaternion rotation = Quaternion.identity;
							Vector3 vector3 = Vector3.one;
							Transform transform = this._transform;
							if (simulationSpace != ParticleSystemSimulationSpace.Local)
							{
								if (simulationSpace != ParticleSystemSimulationSpace.Custom)
								{
									throw new NotSupportedException(string.Format("Unsupported scaling mode '{0}'.", simulationSpace));
								}
								transform = customSimulationSpace;
								vector2 = transform.position;
								rotation = transform.rotation;
								vector3 = transform.localScale;
							}
							else
							{
								vector2 = transform.position;
								rotation = transform.rotation;
								vector3 = transform.localScale;
							}
							Vector3 vector4 = Vector3.zero;
							Vector3 vector5 = Vector3.zero;
							for (int m = 0; m < particleCount; m++)
							{
								if (num2 == this.maxLineRenderers)
								{
									break;
								}
								if (simulationSpace == ParticleSystemSimulationSpace.Local || simulationSpace == ParticleSystemSimulationSpace.Custom)
								{
									switch (scalingMode)
									{
									case ParticleSystemScalingMode.Hierarchy:
										vector4 = transform.TransformPoint(this.particlePositions[m]);
										break;
									case ParticleSystemScalingMode.Local:
										vector4.x = this.particlePositions[m].x * vector3.x;
										vector4.y = this.particlePositions[m].y * vector3.y;
										vector4.z = this.particlePositions[m].z * vector3.z;
										vector4 = rotation * vector4;
										vector4.x += vector2.x;
										vector4.y += vector2.y;
										vector4.z += vector2.z;
										break;
									case ParticleSystemScalingMode.Shape:
										vector4 = rotation * this.particlePositions[m];
										vector4.x += vector2.x;
										vector4.y += vector2.y;
										vector4.z += vector2.z;
										break;
									default:
										throw new NotSupportedException(string.Format("Unsupported scaling mode '{0}'.", scalingMode));
									}
								}
								Color b2 = this.particleColours[m];
								Color startColor3 = Color.LerpUnclamped(startColor, b2, this.colourFromParticle);
								startColor3.a = Mathf.LerpUnclamped(startColor.a, b2.a, this.alphaFromParticle);
								float startWidth2 = Mathf.LerpUnclamped(a, this.particleSizes[m], this.widthFromParticle);
								int num6 = 0;
								for (int n = m + 1; n < particleCount; n++)
								{
									if (simulationSpace == ParticleSystemSimulationSpace.Local || simulationSpace == ParticleSystemSimulationSpace.Custom)
									{
										switch (scalingMode)
										{
										case ParticleSystemScalingMode.Hierarchy:
											vector5 = transform.TransformPoint(this.particlePositions[n]);
											break;
										case ParticleSystemScalingMode.Local:
											vector5.x = this.particlePositions[n].x * vector3.x;
											vector5.y = this.particlePositions[n].y * vector3.y;
											vector5.z = this.particlePositions[n].z * vector3.z;
											vector5 = rotation * vector5;
											vector5.x += vector2.x;
											vector5.y += vector2.y;
											vector5.z += vector2.z;
											break;
										case ParticleSystemScalingMode.Shape:
											vector5 = rotation * this.particlePositions[n];
											vector5.x += vector2.x;
											vector5.y += vector2.y;
											vector5.z += vector2.z;
											break;
										default:
											throw new NotSupportedException(string.Format("Unsupported scaling mode '{0}'.", scalingMode));
										}
									}
									Vector3 vector;
									vector.x = this.particlePositions[m].x - this.particlePositions[n].x;
									vector.y = this.particlePositions[m].y - this.particlePositions[n].y;
									vector.z = this.particlePositions[m].z - this.particlePositions[n].z;
									float num7 = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
									if (num7 <= num3)
									{
										LineRenderer lineRenderer2;
										if (num2 == num)
										{
											lineRenderer2 = UnityEngine.Object.Instantiate<LineRenderer>(this.lineRendererTemplate, this._transform, false);
											this.lineRenderers.Add(lineRenderer2);
											num++;
										}
										lineRenderer2 = this.lineRenderers[num2];
										lineRenderer2.enabled = true;
										lineRenderer2.SetPosition(0, vector4);
										lineRenderer2.SetPosition(1, vector5);
										lineRenderer2.startColor = startColor3;
										b2 = this.particleColours[n];
										Color endColor3 = Color.LerpUnclamped(endColor, b2, this.colourFromParticle);
										endColor3.a = Mathf.LerpUnclamped(endColor.a, b2.a, this.alphaFromParticle);
										lineRenderer2.endColor = endColor3;
										lineRenderer2.startWidth = startWidth2;
										lineRenderer2.endWidth = Mathf.LerpUnclamped(a2, this.particleSizes[n], this.widthFromParticle);
										num2++;
										num6++;
										if (num6 == this.maxConnections || num2 == this.maxLineRenderers)
										{
											break;
										}
									}
								}
							}
						}
					}
					for (int num8 = num2; num8 < num; num8++)
					{
						if (this.lineRenderers[num8].enabled)
						{
							this.lineRenderers[num8].enabled = false;
						}
					}
				}
			}
		}

		public float maxDistance = 1f;

		public int maxConnections = 5;

		public int maxLineRenderers = 100;

		[Range(0f, 1f)]
		public float widthFromParticle = 0.125f;

		[Range(0f, 1f)]
		public float colourFromParticle = 1f;

		[Range(0f, 1f)]
		public float alphaFromParticle = 1f;

		private ParticleSystem particleSystem;

		private ParticleSystem.Particle[] particles;

		private Vector3[] particlePositions;

		private Color[] particleColours;

		private float[] particleSizes;

		private ParticleSystem.MainModule particleSystemMainModule;

		public LineRenderer lineRendererTemplate;

		private List<LineRenderer> lineRenderers = new List<LineRenderer>();

		private Transform _transform;

		[Header("General Performance Settings")]
		[Range(0f, 1f)]
		public float delay;

		private float timer;

		public bool alwaysUpdate;

		private bool visible;
	}
}
