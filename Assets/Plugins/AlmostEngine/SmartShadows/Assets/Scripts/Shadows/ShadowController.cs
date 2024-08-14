using UnityEngine;
using System.Collections;


namespace AlmostEngine.Shadows
{
    /// <summary>
    /// Shadow controller is used to enable and disable the light's shadow, but also its range and intensity.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public class ShadowController : MonoBehaviour
    {
        Light m_Light;
        GameLight m_GameLight;

        void Start()
        {
            m_Light = GetComponent<Light>();
            m_GameLight = GetComponent<GameLight>();
            if (m_GameLight == null)
            {
                m_GameLight = gameObject.AddComponent<GameLight>();
            }
            m_IsShadowEnabled = m_Light.shadows != LightShadows.None;
        }

        #region PRIORITY

        public enum ImportanceMode
        {
            LOW,
            MEDIUM,
            HIGH,
            UNRESTRICTED,
            IGNORE
        }
        ;

        [Tooltip("Defines the coarse priority of the shadow. " +
        "All shadows of a given importance will have priority over shadows of a lower importance. " +
        "Unrestricted will be counted as an active shadow but not affected by the manager. " +
        "Ignore will not be counted as an active shadow and not affected by the manager.")]
        public ImportanceMode m_Importance = ImportanceMode.MEDIUM;
        [Tooltip("Defines the fine priority of the shadow. " +
        "Within the same importance level, all shadows of a given priority value will have priority over shadows of a lower priority value. ")]
        public int m_Priority = 0;

        #endregion

        #region DISTANCE

        public enum DistanceMode
        {
            DEFAULT,
            PROPORTIONAL_TO_LIGHT_RANGE,
            CUSTOM
        }
        ;

        [Tooltip("Defines for what distance to the manager this shadow can be enabled. " +
        "Default uses the manager default value." +
        "You can set the distance to be proportional to the light range or custom.")]
        public DistanceMode m_DistanceMode = DistanceMode.DEFAULT;
        public float m_LightRangeShadowDistanceCoeff = 2f;
        public float m_CustomMaxShadowDistance = 100f;


        public float GetShadowRange()
        {
            if (m_DistanceMode == ShadowController.DistanceMode.DEFAULT)
                return ShadowManager.m_Instance.m_Config.m_DefaultMaxShadowDistance;
            else if (m_DistanceMode == ShadowController.DistanceMode.PROPORTIONAL_TO_LIGHT_RANGE)
                return m_GameLight.m_Range * m_LightRangeShadowDistanceCoeff;
            else
                return m_CustomMaxShadowDistance;
        }

        #endregion



        #region INTENSITY

        public enum IntensityReductionMode
        {
            IGNORE,
            DEFAULT,
            CUSTOM
        }
        ;

        [Tooltip("Defines how the light intensity is reduced when its shadow is disabled. " +
        "Default uses the manager default value. " +
        "Ignore will not modify the intensity. " +
        "You can set the intensity reduction to a custom coefficient.")]
        public IntensityReductionMode m_IntensityReductionMode = IntensityReductionMode.DEFAULT;
        public float m_CustomIntensityReductionCoeff = 1f;

        public float GetIntensityReductionCoeff()
        {
            if (m_IntensityReductionMode == IntensityReductionMode.CUSTOM)
                return m_CustomIntensityReductionCoeff;
            return ShadowManager.m_Instance.m_Config.m_DefaultIntensityReductionCoeff;
        }

        #endregion

        #region RANGE

        public enum RangeReductionMode
        {
            IGNORE,
            DEFAULT,
            CUSTOM
        }
        ;

        [Tooltip("Defines how the light range is reduced when its shadow is disabled. " +
        "Default uses the manager default value. " +
        "Ignore will not modify the range. " +
        "You can set the range reduction to a custom coefficient.")]
        public RangeReductionMode m_RangeReductionMode = RangeReductionMode.DEFAULT;
        public float m_CustomRangeReductionCoeff = 1f;

        public float GetRangeReductionCoeff()
        {
            if (m_RangeReductionMode == RangeReductionMode.CUSTOM)
                return m_CustomRangeReductionCoeff;
            return ShadowManager.m_Instance.m_Config.m_DefaultRangeReductionCoeff;
        }


        #endregion

        bool m_IsShadowEnabled = false;
        bool m_IsResolutionReduced = false;



        #region ENABLE/DISABLE


        public void ForceEnabled(bool enabled)
        {
            if (m_GameLight == null)
                return;

            if (!m_GameLight.m_RequestShadow)
                return;

            StopAllCoroutines();
            m_IsShadowEnabled = enabled;
            if (m_IsShadowEnabled)
            {
                m_Light.shadows = m_GameLight.m_ShadowMode;
                m_Light.range = m_GameLight.m_Range;
                m_Light.intensity = m_GameLight.m_Intensity;
                m_Light.shadowStrength = m_GameLight.m_ShadowStrength;
#if HASS_NGSS
                if (m_Light.GetComponent<NGSS_Local>() != null)
                {
                    m_Light.GetComponent<NGSS_Local>().NGSS_SHADOWS_SOFTNESS = m_GameLight.m_ShadowStrength;
                }
#endif
            }
            else
            {
                m_Light.shadows = LightShadows.None;
                m_Light.shadowStrength = 0f;
#if HASS_NGSS
                if (m_Light.GetComponent<NGSS_Local>() != null)
                {
                    m_Light.GetComponent<NGSS_Local>().NGSS_SHADOWS_SOFTNESS = 0f;
                }
#endif

                if (ShadowManager.m_Instance.m_Config.m_ReduceInactiveLightRange && m_RangeReductionMode != RangeReductionMode.IGNORE)
                {
                    m_Light.range = m_GameLight.m_Range * GetRangeReductionCoeff();
                }
                if (ShadowManager.m_Instance.m_Config.m_ReduceInactiveLightIntensity && m_IntensityReductionMode != IntensityReductionMode.IGNORE)
                {
                    m_Light.intensity = m_GameLight.m_Intensity * GetIntensityReductionCoeff();
                }
            }
        }

        public void SetEnabled(bool enabled)
        {
            if (m_GameLight == null)
                return;

            if (!m_GameLight.m_RequestShadow)
                return;

            if (enabled == m_IsShadowEnabled)
                return;

            if (m_Light.enabled == false
                || this.enabled == false
                || gameObject.activeInHierarchy == false)
            {
                return;
            }

            m_IsShadowEnabled = enabled;

            StopAllCoroutines();
            if (m_IsShadowEnabled)
            {
                StartCoroutine(EnableLightShadowCoroutine());
                if (ShadowManager.m_Instance.m_Config.m_ReduceInactiveLightRange && m_RangeReductionMode != RangeReductionMode.IGNORE)
                {
                    StartCoroutine(RestoreRangeCoroutine());
                }
                if (ShadowManager.m_Instance.m_Config.m_ReduceInactiveLightIntensity && m_IntensityReductionMode != IntensityReductionMode.IGNORE)
                {
                    StartCoroutine(RestoreIntensityCoroutine());
                }
            }
            else
            {
                StartCoroutine(DisableLightShadowCoroutine());
                if (ShadowManager.m_Instance.m_Config.m_ReduceInactiveLightRange && m_RangeReductionMode != RangeReductionMode.IGNORE)
                {
                    StartCoroutine(ReduceRangeCoroutine());
                }
                if (ShadowManager.m_Instance.m_Config.m_ReduceInactiveLightIntensity && m_IntensityReductionMode != IntensityReductionMode.IGNORE)
                {
                    StartCoroutine(ReduceIntensityCoroutine());
                }
            }
        }

        public void SetResolutionQualityReduced(bool reduced)
        {
            if (m_GameLight == null)
                return;

            m_IsResolutionReduced = reduced;

            int targetResolution;
            if (m_GameLight.m_ShadowResolution == UnityEngine.Rendering.LightShadowResolution.FromQualitySettings)
            {
                targetResolution = (int)QualitySettings.shadowResolution;
            }
            else
            {
                targetResolution = (int)m_GameLight.m_ShadowResolution;
            }
            if (m_IsResolutionReduced)
            {
                // Reduce only if > LOW == 1
                targetResolution = (targetResolution > 1) ? targetResolution - 1 : 1;
            }

            m_Light.shadowResolution = (UnityEngine.Rendering.LightShadowResolution)(targetResolution);
        }

        #endregion

        #region Coroutines

        IEnumerator DisableLightShadowCoroutine()
        {
            while (m_Light.shadowStrength > 0f)
            {
                m_Light.shadowStrength = Mathf.Clamp(m_Light.shadowStrength - Time.deltaTime / ShadowManager.m_Instance.m_FadeOutTime * m_GameLight.m_ShadowStrength, 0, 1);
#if HASS_NGSS
                if (m_Light.GetComponent<NGSS_Local>())
                {
                    m_Light.GetComponent<NGSS_Local>().NGSS_SHADOWS_SOFTNESS = Mathf.Clamp(m_Light.GetComponent<NGSS_Local>().NGSS_SHADOWS_SOFTNESS - Time.deltaTime / ShadowManager.m_Instance.m_FadeOutTime * m_GameLight.m_ShadowStrength, 0, 1);
                }
#endif

                yield return null;
            }
            m_Light.shadows = LightShadows.None;
        }

        IEnumerator EnableLightShadowCoroutine()
        {
            m_Light.shadows = m_GameLight.m_ShadowMode;
            while (m_Light.shadowStrength < m_GameLight.m_ShadowStrength)
            {
                m_Light.shadowStrength = Mathf.Clamp(m_Light.shadowStrength + Time.deltaTime / ShadowManager.m_Instance.m_FadeInTime * m_GameLight.m_ShadowStrength, 0, 1);
#if HASS_NGSS
                if (m_Light.GetComponent<NGSS_Local>())
                {
                    m_Light.GetComponent<NGSS_Local>().NGSS_SHADOWS_SOFTNESS = Mathf.Clamp(m_Light.GetComponent<NGSS_Local>().NGSS_SHADOWS_SOFTNESS + Time.deltaTime / ShadowManager.m_Instance.m_FadeInTime * m_GameLight.m_ShadowStrength, 0, 1);
                }
#endif
                yield return null;
            }
        }


        IEnumerator ReduceRangeCoroutine()
        {
            while (m_Light.range >= m_GameLight.m_Range * GetRangeReductionCoeff())
            {

                m_Light.range = m_Light.range - Time.deltaTime / ShadowManager.m_Instance.m_RangeFadeInTime * m_GameLight.m_Range;
                yield return null;
            }
        }

        IEnumerator RestoreRangeCoroutine()
        {
            while (m_Light.range <= m_GameLight.m_Range)
            {
                m_Light.range = m_Light.range + Time.deltaTime / ShadowManager.m_Instance.m_RangeFadeOutTime * m_GameLight.m_Range;
                yield return null;
            }
        }


        IEnumerator ReduceIntensityCoroutine()
        {
            while (m_Light.intensity >= m_GameLight.m_Intensity * GetIntensityReductionCoeff())
            {
                m_Light.intensity = m_Light.intensity - Time.deltaTime / ShadowManager.m_Instance.m_IntensityFadeInTime * m_GameLight.m_Intensity;
                yield return null;
            }
        }

        IEnumerator RestoreIntensityCoroutine()
        {
            while (m_Light.intensity <= m_GameLight.m_Intensity)
            {
                m_Light.intensity = m_Light.intensity + Time.deltaTime / ShadowManager.m_Instance.m_IntensityFadeOutTime * m_GameLight.m_Intensity;
                yield return null;
            }
        }

        #endregion
    }

}