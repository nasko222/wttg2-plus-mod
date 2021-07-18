using System;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects
{
	public class VortexParticleAffector : ParticleAffector
	{
		protected override void Awake()
		{
			base.Awake();
		}

		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			base.Update();
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();
		}

		private void UpdateAxisOfRotation()
		{
			this.axisOfRotation = Quaternion.Euler(this.axisOfRotationOffset) * base.transform.up;
		}

		protected override void PerParticleSystemSetup()
		{
			this.UpdateAxisOfRotation();
		}

		protected override Vector3 GetForce()
		{
			return Vector3.Normalize(Vector3.Cross(this.axisOfRotation, this.parameters.scaledDirectionToAffectorCenter));
		}

		protected override void OnDrawGizmosSelected()
		{
			if (base.enabled)
			{
				base.OnDrawGizmosSelected();
				Gizmos.color = Color.red;
				Vector3 a;
				if (Application.isPlaying && base.enabled)
				{
					this.UpdateAxisOfRotation();
					a = this.axisOfRotation;
				}
				else
				{
					a = Quaternion.Euler(this.axisOfRotationOffset) * base.transform.up;
				}
				Gizmos.DrawLine(base.transform.position + this.offset, base.transform.position + this.offset + a * base.scaledRadius);
			}
		}

		private Vector3 axisOfRotation;

		[Header("Affector Controls")]
		public Vector3 axisOfRotationOffset = Vector3.zero;
	}
}
