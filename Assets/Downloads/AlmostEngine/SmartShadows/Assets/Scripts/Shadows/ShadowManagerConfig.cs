using UnityEngine;
using System.Collections;

namespace AlmostEngine.Shadows
{
	[System.Serializable]
	public class ShadowManagerConfig
	{
		[Tooltip ("Defines the maximum simultaneous active shadows.")]
		public int m_MaxActiveShadows = 10;

		[Tooltip ("Defines the default maximum distance from the shadow origin for which a shadow can be activated.")]
		public float m_DefaultMaxShadowDistance = 25f;

		[Tooltip ("When a shadow is disabled, its light range can be reduced.")]
		public bool m_ReduceInactiveLightRange = false;
		[Tooltip ("Defines the default range reduction coefficient of the lights.")]
		public float m_DefaultRangeReductionCoeff = 1f;

		[Tooltip ("When a shadow is disabled, its light intensity can be reduced.")]
		public bool m_ReduceInactiveLightIntensity = false;
		[Tooltip ("Defines the default intensity reduction coefficient of the lights.")]
		public float m_DefaultIntensityReductionCoeff = 1f;

		[Tooltip ("Reduces the shadow resolution of the lowest priority shadows.")]
		public bool m_ReduceShadowResolutionOfLeastPriority = false;
		[Tooltip ("If N shadows are enabled and you set this value to M < N, all shadows from M to N will have their resolution reduced by one quality level.")]
		public int m_ReduceResolutionOfLast = 0;
	};
}

