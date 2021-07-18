using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentLinkMover : MonoBehaviour
{
	private IEnumerator Start()
	{
		NavMeshAgent agent = base.GetComponent<NavMeshAgent>();
		agent.autoTraverseOffMeshLink = false;
		for (;;)
		{
			if (agent.isOnOffMeshLink)
			{
				yield return base.StartCoroutine(this.NormalSpeed(agent));
				if (agent != null && agent.isOnOffMeshLink)
				{
					agent.CompleteOffMeshLink();
				}
			}
			yield return null;
		}
		yield break;
	}

	private IEnumerator NormalSpeed(NavMeshAgent agent)
	{
		agent.updateRotation = false;
		OffMeshLinkData data = agent.currentOffMeshLinkData;
		Vector3 startPOS = data.startPos + Vector3.up * agent.baseOffset;
		Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
		Vector3 dir = data.endPos - data.startPos;
		dir.y = 0f;
		Quaternion endROT = Quaternion.LookRotation(dir);
		while (agent.transform.position != startPOS)
		{
			agent.transform.position = Vector3.MoveTowards(agent.transform.position, startPOS, agent.speed * Time.deltaTime);
			agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, endROT, agent.angularSpeed * this.FirstRotationSpeed * Time.deltaTime);
			yield return null;
		}
		while (Quaternion.Angle(agent.transform.rotation, endROT) >= 5f)
		{
			agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, endROT, agent.angularSpeed * this.FirstRotationSpeed * Time.deltaTime);
			yield return null;
		}
		while (agent.transform.position != endPos)
		{
			agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * this.CrossSpeed * Time.deltaTime);
			yield return null;
		}
		agent.updateRotation = true;
		yield break;
	}

	public float FirstRotationSpeed = 1f;

	[Range(0f, 1f)]
	public float CrossSpeed = 1f;
}
