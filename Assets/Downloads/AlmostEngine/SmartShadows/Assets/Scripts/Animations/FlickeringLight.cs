using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlmostEngine.Examples
{
	
	[RequireComponent(typeof(Light))]
	public class FlickeringLight : MonoBehaviour
	{
		
		public float m_MinFlickeringTime = 0.01f;
		public float m_MaxFlickeringTime = 0.1f;

		public float m_MinIntensity = 0f;
		public float m_MaxIntensity = 1f;
		public float m_IntensitySmoothing = 0.1f;

		Light m_Light;
		float m_TargetIntensity;

		void OnEnable()
		{
			m_Light = GetComponent<Light>();
			StartCoroutine(FlickerCoroutine());
		}


		IEnumerator FlickerCoroutine()
		{
			while (true) {
				m_TargetIntensity = RandomUtils.Float(m_MinIntensity, m_MaxIntensity);
				yield return new WaitForSeconds(RandomUtils.Float(m_MinFlickeringTime, m_MaxFlickeringTime));
			}
		}

		void Update()
		{
			m_Light.intensity = Mathf.Lerp(m_Light.intensity, m_TargetIntensity, Time.deltaTime / m_IntensitySmoothing);
		}
	}
}