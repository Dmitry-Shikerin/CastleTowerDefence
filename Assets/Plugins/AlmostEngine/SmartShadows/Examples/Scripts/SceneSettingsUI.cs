using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlmostEngine.Shadows.Example
{
	public class SceneSettingsUI : MonoBehaviour
	{
		public Text m_Button;
		public Text m_NbShadows;
		public Text m_MaxShadows;
		public Slider m_MaxShadowsSlider;
		public Text m_MaxDistance;
		public Slider m_MaxDistanceSlider;

		public void Toggle ()
		{
			ShadowManager.m_Instance.enabled = !ShadowManager.m_Instance.enabled;
		}

		void Start ()
		{
			m_MaxShadowsSlider.value = ShadowManager.m_Instance.m_Config.m_MaxActiveShadows;
			m_MaxDistanceSlider.value = ShadowManager.m_Instance.m_Config.m_DefaultMaxShadowDistance;
		}

		void Update ()
		{
			m_Button.text = ShadowManager.m_Instance.enabled ? "SmartShadows is ENABLED" : "SmartShadows is DISABLED";

			int nb = 0;
			foreach (var light in LightManager.m_Instance.m_RegisteredLights.Values) {
				if (light.m_Light != null && light.m_Light.shadows != LightShadows.None) {
					nb++;
				}
			}
			m_NbShadows.text = "Active shadows: " + nb;

			ShadowManager.m_Instance.m_Config.m_MaxActiveShadows = (int)m_MaxShadowsSlider.value;
			m_MaxShadows.text = "Max shadows: " + ShadowManager.m_Instance.m_Config.m_MaxActiveShadows;
			ShadowManager.m_Instance.m_Config.m_DefaultMaxShadowDistance = m_MaxDistanceSlider.value;
			m_MaxDistance.text = "Max distance: " + ShadowManager.m_Instance.m_Config.m_DefaultMaxShadowDistance;
		}

	}
}