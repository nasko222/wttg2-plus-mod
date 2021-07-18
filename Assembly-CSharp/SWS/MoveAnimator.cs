using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace SWS
{
	public class MoveAnimator : MonoBehaviour
	{
		private void Start()
		{
			this.animator = base.GetComponentInChildren<Animator>();
			this.sMove = base.GetComponent<splineMove>();
			if (!this.sMove)
			{
				this.nAgent = base.GetComponent<NavMeshAgent>();
			}
		}

		private void OnAnimatorMove()
		{
			float value;
			float value2;
			if (this.sMove)
			{
				value = ((this.sMove.tween != null && this.sMove.tween.IsPlaying()) ? this.sMove.speed : 0f);
				value2 = (base.transform.eulerAngles.y - this.lastRotY) * 10f;
				this.lastRotY = base.transform.eulerAngles.y;
			}
			else
			{
				value = this.nAgent.velocity.magnitude;
				Vector3 vector = Quaternion.Inverse(base.transform.rotation) * this.nAgent.desiredVelocity;
				value2 = Mathf.Atan2(vector.x, vector.z) * 180f / 3.14159f;
			}
			this.animator.SetFloat("Speed", value);
			this.animator.SetFloat("Direction", value2, 0.15f, Time.deltaTime);
		}

		private splineMove sMove;

		private NavMeshAgent nAgent;

		private Animator animator;

		private float lastRotY;
	}
}
