using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AlmostEngine.Examples
{

	[RequireComponent(typeof(Light))]
	public class FireLight : MonoBehaviour
	{


		public float m_MinFlickeringTime = 0.01f;
		public float m_MaxFlickeringTime = 0.2f;

		public float m_MinIntensity = 0f;
		public float m_MaxIntensity = 1f;
		public float m_IntensitySmoothing = 0.1f;

		public float m_MaxPositionOffset = 0.1f;
		public float m_PositionSmoothing = 0.1f;


		Light m_Light;

		float m_TargetIntensity;

		Vector3 m_DefaultPos;
		Vector3 m_TargetPos;

		void Start()
		{
			m_Light = GetComponent<Light>();

			m_DefaultPos = transform.position;

			StartCoroutine(FlickerCoroutine());
		}

		IEnumerator FlickerCoroutine()
		{
			while (true) {
				m_TargetIntensity = RandomUtils.Float(m_MinIntensity, m_MaxIntensity);
				m_TargetPos = m_DefaultPos + RandomUtils.Vector3(-m_MaxPositionOffset, m_MaxPositionOffset);

				yield return new WaitForSeconds(RandomUtils.Float(m_MinFlickeringTime, m_MaxFlickeringTime));
			}
		}


		public void Update()
		{
			m_Light.intensity = Mathf.Lerp(m_Light.intensity, m_TargetIntensity, Time.deltaTime / m_IntensitySmoothing);
			transform.position = Vector3.Lerp(transform.position, m_TargetPos, Time.deltaTime / m_PositionSmoothing);
		}

	}

}