using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects
{
	public abstract class ParticleForceField : MonoBehaviour
	{
		public float scaledRadius
		{
			get
			{
				return this.radius * base.transform.lossyScale.x;
			}
		}

		protected virtual void Awake()
		{
		}

		protected virtual void Start()
		{
			this.particleSystem = base.GetComponent<ParticleSystem>();
		}

		protected virtual void PerParticleSystemSetup()
		{
		}

		protected virtual Vector3 GetForce()
		{
			return Vector3.zero;
		}

		protected virtual void Update()
		{
		}

		public void AddParticleSystem(ParticleSystem particleSystem)
		{
			this._particleSystems.Add(particleSystem);
		}

		public void RemoveParticleSystem(ParticleSystem particleSystem)
		{
			this._particleSystems.Remove(particleSystem);
		}

		protected virtual void LateUpdate()
		{
			this._radius = this.scaledRadius;
			this.radiusSqr = this._radius * this._radius;
			this.forceDeltaTime = this.force * Time.deltaTime;
			this.transformPosition = base.transform.position + this.center;
			if (this._particleSystems.Count != 0)
			{
				if (this.particleSystems.Count != this._particleSystems.Count)
				{
					this.particleSystems.Clear();
					this.particleSystems.AddRange(this._particleSystems);
				}
				else
				{
					for (int i = 0; i < this._particleSystems.Count; i++)
					{
						this.particleSystems[i] = this._particleSystems[i];
					}
				}
			}
			else if (this.particleSystem)
			{
				if (this.particleSystems.Count == 1)
				{
					this.particleSystems[0] = this.particleSystem;
				}
				else
				{
					this.particleSystems.Clear();
					this.particleSystems.Add(this.particleSystem);
				}
			}
			else
			{
				this.particleSystems.Clear();
				this.particleSystems.AddRange(UnityEngine.Object.FindObjectsOfType<ParticleSystem>());
			}
			this.parameters = default(ParticleForceField.GetForceParameters);
			this.particleSystemsCount = this.particleSystems.Count;
			if (this.particleSystemParticles == null || this.particleSystemParticles.Length < this.particleSystemsCount)
			{
				this.particleSystemParticles = new ParticleSystem.Particle[this.particleSystemsCount][];
				this.particleSystemMainModules = new ParticleSystem.MainModule[this.particleSystemsCount];
				this.particleSystemRenderers = new Renderer[this.particleSystemsCount];
				this.particleSystemExternalForcesMultipliers = new float[this.particleSystemsCount];
				for (int j = 0; j < this.particleSystemsCount; j++)
				{
					this.particleSystemMainModules[j] = this.particleSystems[j].main;
					this.particleSystemRenderers[j] = this.particleSystems[j].GetComponent<Renderer>();
					this.particleSystemExternalForcesMultipliers[j] = this.particleSystems[j].externalForces.multiplier;
				}
			}
			for (int k = 0; k < this.particleSystemsCount; k++)
			{
				if (this.particleSystemRenderers[k].isVisible || this.alwaysUpdate)
				{
					int maxParticles = this.particleSystemMainModules[k].maxParticles;
					if (this.particleSystemParticles[k] == null || this.particleSystemParticles[k].Length < maxParticles)
					{
						this.particleSystemParticles[k] = new ParticleSystem.Particle[maxParticles];
					}
					this.currentParticleSystem = this.particleSystems[k];
					this.PerParticleSystemSetup();
					int particles = this.currentParticleSystem.GetParticles(this.particleSystemParticles[k]);
					ParticleSystemSimulationSpace simulationSpace = this.particleSystemMainModules[k].simulationSpace;
					ParticleSystemScalingMode scalingMode = this.particleSystemMainModules[k].scalingMode;
					Transform transform = this.currentParticleSystem.transform;
					Transform customSimulationSpace = this.particleSystemMainModules[k].customSimulationSpace;
					if (simulationSpace == ParticleSystemSimulationSpace.World)
					{
						for (int l = 0; l < particles; l++)
						{
							this.parameters.particlePosition = this.particleSystemParticles[k][l].position;
							this.parameters.scaledDirectionToForceFieldCenter.x = this.transformPosition.x - this.parameters.particlePosition.x;
							this.parameters.scaledDirectionToForceFieldCenter.y = this.transformPosition.y - this.parameters.particlePosition.y;
							this.parameters.scaledDirectionToForceFieldCenter.z = this.transformPosition.z - this.parameters.particlePosition.z;
							this.parameters.distanceToForceFieldCenterSqr = this.parameters.scaledDirectionToForceFieldCenter.sqrMagnitude;
							if (this.parameters.distanceToForceFieldCenterSqr < this.radiusSqr)
							{
								float time = this.parameters.distanceToForceFieldCenterSqr / this.radiusSqr;
								float num = this.forceOverDistance.Evaluate(time);
								Vector3 vector = this.GetForce();
								float num2 = this.forceDeltaTime * num * this.particleSystemExternalForcesMultipliers[k];
								vector.x *= num2;
								vector.y *= num2;
								vector.z *= num2;
								Vector3 velocity = this.particleSystemParticles[k][l].velocity;
								velocity.x += vector.x;
								velocity.y += vector.y;
								velocity.z += vector.z;
								this.particleSystemParticles[k][l].velocity = velocity;
							}
						}
					}
					else
					{
						Vector3 b = Vector3.zero;
						Quaternion rotation = Quaternion.identity;
						Vector3 b2 = Vector3.one;
						Transform transform2 = transform;
						if (simulationSpace != ParticleSystemSimulationSpace.Local)
						{
							if (simulationSpace != ParticleSystemSimulationSpace.Custom)
							{
								throw new NotSupportedException(string.Format("Unsupported scaling mode '{0}'.", simulationSpace));
							}
							transform2 = customSimulationSpace;
							b = transform2.position;
							rotation = transform2.rotation;
							b2 = transform2.localScale;
						}
						else
						{
							b = transform2.position;
							rotation = transform2.rotation;
							b2 = transform2.localScale;
						}
						for (int m = 0; m < particles; m++)
						{
							this.parameters.particlePosition = this.particleSystemParticles[k][m].position;
							if (simulationSpace == ParticleSystemSimulationSpace.Local || simulationSpace == ParticleSystemSimulationSpace.Custom)
							{
								switch (scalingMode)
								{
								case ParticleSystemScalingMode.Hierarchy:
									this.parameters.particlePosition = transform2.TransformPoint(this.particleSystemParticles[k][m].position);
									break;
								case ParticleSystemScalingMode.Local:
									this.parameters.particlePosition = Vector3.Scale(this.parameters.particlePosition, b2);
									this.parameters.particlePosition = rotation * this.parameters.particlePosition;
									this.parameters.particlePosition = this.parameters.particlePosition + b;
									break;
								case ParticleSystemScalingMode.Shape:
									this.parameters.particlePosition = rotation * this.parameters.particlePosition;
									this.parameters.particlePosition = this.parameters.particlePosition + b;
									break;
								default:
									throw new NotSupportedException(string.Format("Unsupported scaling mode '{0}'.", scalingMode));
								}
							}
							this.parameters.scaledDirectionToForceFieldCenter.x = this.transformPosition.x - this.parameters.particlePosition.x;
							this.parameters.scaledDirectionToForceFieldCenter.y = this.transformPosition.y - this.parameters.particlePosition.y;
							this.parameters.scaledDirectionToForceFieldCenter.z = this.transformPosition.z - this.parameters.particlePosition.z;
							this.parameters.distanceToForceFieldCenterSqr = this.parameters.scaledDirectionToForceFieldCenter.sqrMagnitude;
							if (this.parameters.distanceToForceFieldCenterSqr < this.radiusSqr)
							{
								float time2 = this.parameters.distanceToForceFieldCenterSqr / this.radiusSqr;
								float num3 = this.forceOverDistance.Evaluate(time2);
								Vector3 vector2 = this.GetForce();
								float num4 = this.forceDeltaTime * num3 * this.particleSystemExternalForcesMultipliers[k];
								vector2.x *= num4;
								vector2.y *= num4;
								vector2.z *= num4;
								if (simulationSpace == ParticleSystemSimulationSpace.Local || simulationSpace == ParticleSystemSimulationSpace.Custom)
								{
									switch (scalingMode)
									{
									case ParticleSystemScalingMode.Hierarchy:
										vector2 = transform2.InverseTransformVector(vector2);
										break;
									case ParticleSystemScalingMode.Local:
										vector2 = Quaternion.Inverse(rotation) * vector2;
										vector2 = Vector3.Scale(vector2, new Vector3(1f / b2.x, 1f / b2.y, 1f / b2.z));
										break;
									case ParticleSystemScalingMode.Shape:
										vector2 = Quaternion.Inverse(rotation) * vector2;
										break;
									default:
										throw new NotSupportedException(string.Format("Unsupported scaling mode '{0}'.", scalingMode));
									}
								}
								Vector3 velocity2 = this.particleSystemParticles[k][m].velocity;
								velocity2.x += vector2.x;
								velocity2.y += vector2.y;
								velocity2.z += vector2.z;
								this.particleSystemParticles[k][m].velocity = velocity2;
							}
						}
					}
					this.currentParticleSystem.SetParticles(this.particleSystemParticles[k], particles);
				}
			}
		}

		private void OnApplicationQuit()
		{
		}

		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(base.transform.position + this.center, this.scaledRadius);
		}

		[Header("Common Controls")]
		[Tooltip("Force field spherical range.")]
		public float radius = float.PositiveInfinity;

		[Tooltip("Maximum baseline force.")]
		public float force = 5f;

		[Tooltip("Internal force field position offset.")]
		public Vector3 center = Vector3.zero;

		private float _radius;

		private float radiusSqr;

		private float forceDeltaTime;

		private Vector3 transformPosition;

		private float[] particleSystemExternalForcesMultipliers;

		[Tooltip("Force scale as determined by distance to individual particles.")]
		public AnimationCurve forceOverDistance = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 1f)
		});

		private ParticleSystem particleSystem;

		[Tooltip("If nothing no particle systems are assigned, this force field will operate globally on ALL particle systems in the scene (NOT recommended).\n\nIf attached to a particle system, the force field will operate only on that system.\n\nIf specific particle systems are assigned, then the force field will operate on those systems only, even if attached to a particle system.")]
		public List<ParticleSystem> _particleSystems;

		private int particleSystemsCount;

		private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

		private ParticleSystem.Particle[][] particleSystemParticles;

		private ParticleSystem.MainModule[] particleSystemMainModules;

		private Renderer[] particleSystemRenderers;

		protected ParticleSystem currentParticleSystem;

		protected ParticleForceField.GetForceParameters parameters;

		[Tooltip("If TRUE, update even if target particle system(s) are invisible/offscreen.\n\nIf FALSE, update only if particles of the target system(s) are visible/onscreen.")]
		public bool alwaysUpdate;

		protected struct GetForceParameters
		{
			public float distanceToForceFieldCenterSqr;

			public Vector3 scaledDirectionToForceFieldCenter;

			public Vector3 particlePosition;
		}
	}
}
