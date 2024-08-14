using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace AlmostEngine.Shadows.Example
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class WanderAround : MonoBehaviour
	{
		public float m_WanderMinDistance = 5f;
		public float m_WanderMaxDistance = 15f;

		NavMeshAgent m_Agent;
		Vector3 m_OriginalPos;

		void Start()
		{
			m_Agent = GetComponent<NavMeshAgent>();
			m_OriginalPos = transform.position;

			StartCoroutine(WanderAroundCoroutine());
		}

		Vector3 GetNextLocation()
		{
			Vector3 target = m_OriginalPos + RandomUtils.Vector3(-1f * m_WanderMaxDistance, m_WanderMaxDistance);

			UnityEngine.AI.NavMeshHit hit;
			target.y = transform.position.y;
			UnityEngine.AI.NavMesh.SamplePosition(target, out hit, 10f, ~0);
			if (!hit.hit)
				return m_OriginalPos;				
			target = hit.position;

			return target;
		}


		public IEnumerator WanderAroundCoroutine()
		{
			while (true) {

				// Set new target
				m_Agent.destination = GetNextLocation();

				// Wait for the path to be updated
				yield return null;

				// Wait to reach target
				while (m_Agent.remainingDistance > 0.5f)
					yield return null;

				// Wait
				yield return new WaitForSeconds(RandomUtils.Float(1f, 2f));
			}
		}


	}
}