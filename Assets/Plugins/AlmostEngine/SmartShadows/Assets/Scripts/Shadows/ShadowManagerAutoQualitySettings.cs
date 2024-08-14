using UnityEngine;
using System.Collections.Generic;

namespace AlmostEngine.Shadows
{
	/// <summary>
	/// Overwrites the ShadowManager settings by the setting corresponding to the current Unity quality level.
	/// </summary>
	[RequireComponent(typeof(ShadowManager))]
	public class ShadowManagerAutoQualitySettings : MonoBehaviour
	{

		public List <ShadowManagerConfig> m_QualityLevels = new List<ShadowManagerConfig>{ new ShadowManagerConfig() };

		ShadowManager m_ShadowsOptimizer;

		void Awake()
		{
			m_ShadowsOptimizer = GetComponent<ShadowManager>();
		}

		void Update()
		{
			int level = QualitySettings.GetQualityLevel();
			if (level >= m_QualityLevels.Count) {
				m_ShadowsOptimizer.m_Config = m_QualityLevels[m_QualityLevels.Count - 1];
			} else {
				m_ShadowsOptimizer.m_Config = m_QualityLevels[level];
			}
		}
	}
}

