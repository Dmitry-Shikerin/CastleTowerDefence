using UnityEngine;
using System.Collections;


namespace AlmostEngine.Shadows
{
	/// <summary>
	/// Game light is used to save the default settings of light components.
	/// </summary>
	[RequireComponent(typeof(Light))]
	public class GameLight : MonoBehaviour
	{

		[HideInInspector]
		public Light m_Light;

		[HideInInspector]
		public float m_Range;
		[HideInInspector]
		public float m_Intensity;
		[HideInInspector]
		public Color m_Color;

		[HideInInspector]
		public bool m_RequestShadow = false;
		[HideInInspector]
		public float m_ShadowStrength;
		[HideInInspector]
		public LightShadows m_ShadowMode;
		[HideInInspector]
		public UnityEngine.Rendering.LightShadowResolution m_ShadowResolution;

		void Awake()
		{
			Init();
		}

		public void Init()
		{
			m_Light = GetComponent<Light>();

			m_Range = m_Light.range;
			m_Intensity = m_Light.intensity;
			m_Color = m_Light.color;

			m_RequestShadow = (m_Light.shadows != LightShadows.None) ? true : false;
			m_ShadowStrength = m_Light.shadowStrength;
			m_ShadowMode = m_Light.shadows;
			m_ShadowResolution = m_Light.shadowResolution;
		}
			

		void Start()
		{
			if (LightManager.m_Instance != null) {
				LightManager.m_Instance.RegisterLight(m_Light);
			}
		}

		void OnDestroy()
		{
			if (LightManager.m_Instance != null) {
				LightManager.m_Instance.UnregisterLight(m_Light);
			}
		}

	}
}

