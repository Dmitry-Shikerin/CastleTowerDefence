using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlmostEngine.Examples
{
	
	[RequireComponent(typeof(Light))]
	public class PeriodicLight : MonoBehaviour
	{

		Light m_Light;

		public float m_MinIntensity = 0f;
		public float m_MaxIntensity = 3f;
		public float m_Period = 0.1f;

		public float m_MaxTimeOffset = 1f;

		float m_Offset;

		void OnEnable()
		{
			m_Light = GetComponent<Light>();
			m_Offset = m_MaxTimeOffset * Random.value;
		}

		void Update()
		{
			m_Light.intensity = m_MinIntensity + (m_MaxIntensity - m_MinIntensity) * Mathf.Sin((m_Offset + Time.time)  * 2f * Mathf.PI * m_Period);
		}
	}
}