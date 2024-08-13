using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AlmostEngine.Shadows
{
    /// <summary>
    /// Shadow manager sort the light by priority and enable only the most important ones.
    /// </summary>
    public class ShadowManager : Singleton<ShadowManager>
    {

        protected override void OnSingletonAwake()
        {
            if (GameObject.FindObjectOfType<LightManager>() == null)
            {
                gameObject.AddComponent<LightManager>();
            }
        }

        void OnEnable()
        {
            LightManager.onLightRegistered += RegisterLight;
            LightManager.onLightUnregistered += UnregisterLight;

            foreach (GameLight l in LightManager.m_Instance.m_RegisteredLights.Values)
            {
                RegisterLight(l);
            }
        }

        void OnDisable()
        {
            LightManager.onLightRegistered -= RegisterLight;
            LightManager.onLightUnregistered -= UnregisterLight;

            RestoreShadows();
        }

        #region SETTINGS

        [Tooltip("The fading time in seconds of the shadow strenght when the shadow is enabled.")]
        public float m_FadeInTime = 1f;
        [Tooltip("The fading time in seconds of the shadow strenght when the shadow is disabled.")]
        public float m_FadeOutTime = 1f;

        [Tooltip("The fading time in seconds of the light ranges when the shadow is enabled.")]
        public float m_RangeFadeInTime = 2f;
        [Tooltip("The fading time in seconds of the light ranges when the shadow is disabled.")]
        public float m_RangeFadeOutTime = 1f;

        [Tooltip("The fading time in seconds of the light intensity when the shadow is enabled.")]
        public float m_IntensityFadeInTime = 2f;
        [Tooltip("The fading time in seconds of the light intensity when the shadow is disabled.")]
        public float m_IntensityFadeOutTime = 1f;



        public enum DefaultImportanceMode
        {
            AUTO,
            DEFAULT_VALUE
        }

        ;

        public DefaultImportanceMode m_DefaultImportanceMode;
        public ShadowController.ImportanceMode m_DefaultImportance = ShadowController.ImportanceMode.MEDIUM;
        public float m_MediumMinLightRange = 12f;
        public float m_MediumMinLightIntensity = 2f;
        public float m_HighMinLightRange = 20f;
        public float m_HighMinLightIntensity = 3f;

        public ShadowManagerConfig m_Config;

        #endregion

        #region INGAME STATES

        public int m_CurrentActiveShadows = 0;
        public bool m_DebugView = true;

        [HideInInspector]
        public List<ComparableLight> m_RegisteredLights = new List<ComparableLight>();
        public List<ComparableLight> m_LightsPriorityList = new List<ComparableLight>();

        #endregion

        #region LIGHTS MANAGEMENT

        void RegisterLight(GameLight gamelight)
        {
            // Ignore already registered lights
            if (m_RegisteredLights.Find(x => x.m_GameLight == gamelight) != null)
            {
                return;
            }

            // Ignore lights not requesting shadows
            if (gamelight.m_RequestShadow != true)
                return;

            // Ignore baked lights
            //			#if UNITY_5_6_OR_NEWER
            //			if (gamelight.m_Light.lightmapBakeType != LightmapBakeType.Realtime)
            //				return;
            //			#else 
            //			if (gamelight.m_Light.isBaked)
            //				return;
            //			#endif

            ComparableLight lightItem = new ComparableLight();
            lightItem.m_GameLight = gamelight;

            // Create the shadow controller component
            lightItem.m_ShadowController = gamelight.GetComponent<ShadowController>();
            if (lightItem.m_ShadowController == null)
            {
                lightItem.m_ShadowController = gamelight.gameObject.AddComponent<ShadowController>();

                // Default value
                if (m_DefaultImportanceMode == DefaultImportanceMode.DEFAULT_VALUE)
                {
                    lightItem.m_ShadowController.m_Importance = m_DefaultImportance;
                }
                else
                {
                    if (gamelight.m_Light.type == LightType.Directional)
                    {
                        lightItem.m_ShadowController.m_Importance = ShadowController.ImportanceMode.UNRESTRICTED;
                    }
                    else if (gamelight.m_Intensity >= m_HighMinLightIntensity && gamelight.m_Range >= m_HighMinLightRange)
                    {
                        lightItem.m_ShadowController.m_Importance = ShadowController.ImportanceMode.HIGH;
                    }
                    else if (gamelight.m_Intensity >= m_MediumMinLightIntensity && gamelight.m_Range >= m_MediumMinLightRange)
                    {
                        lightItem.m_ShadowController.m_Importance = ShadowController.ImportanceMode.MEDIUM;
                    }
                    else
                    {
                        lightItem.m_ShadowController.m_Importance = ShadowController.ImportanceMode.LOW;
                    }
                }
            }

            m_RegisteredLights.Add(lightItem);
        }

        void UnregisterLight(GameLight gamelight)
        {
            if (gamelight != null)
            {
                m_RegisteredLights.RemoveAll(x => x.m_GameLight == gamelight);
            }
        }

        #endregion

        #region SHADOWS MANAGEMENT

        void Update()
        {
            if (LightManager.m_Instance == null)
            {
                return;
            }

            SortLights();
            ActivateShadows();
        }

        void SortLights()
        {
            // Construct the lights priority list
            m_LightsPriorityList.Clear();
            foreach (ComparableLight item in m_RegisteredLights)
            {

                // Ignore disabled game objects
                if (item.m_GameLight.gameObject.activeInHierarchy == false)
                    continue;

                // Ignore disabled lights
                if (item.m_GameLight.m_Light.enabled == false)
                    continue;

                // Ignore lights set as ignored
                if (item.m_ShadowController.m_Importance == ShadowController.ImportanceMode.IGNORE)
                    continue;

                // Update distance & settings
                item.UpdateDistance(transform.position);

                m_LightsPriorityList.Add(item);
            }

            // Sort the priority list
            m_LightsPriorityList.Sort();


        }

        void ActivateShadows()
        {
            // Enable the n first lights shadows
            // and diable the others
            m_CurrentActiveShadows = 0;
            foreach (ComparableLight item in m_LightsPriorityList)
            {

                if (item.m_ShadowController.m_Importance != ShadowController.ImportanceMode.UNRESTRICTED)
                {
                    if (item.m_GameLight.m_Light.enabled == true && item.m_GameLight.gameObject.activeInHierarchy && m_CurrentActiveShadows < m_Config.m_MaxActiveShadows && item.m_IsInRange)
                    {
                        // Enable shadows
                        if (Time.timeSinceLevelLoad < 0.5f)
                        {
                            item.m_ShadowController.ForceEnabled(true);
                        }
                        else
                        {
                            item.m_ShadowController.SetEnabled(true);
                        }

                        // Reduce Shadow Resolution
                        if (m_Config.m_ReduceInactiveLightIntensity && m_CurrentActiveShadows >= m_Config.m_MaxActiveShadows - m_Config.m_ReduceResolutionOfLast)
                        {
                            item.m_ShadowController.SetResolutionQualityReduced(true);
                        }
                        else
                        {
                            item.m_ShadowController.SetResolutionQualityReduced(false);
                        }
                    }
                    else
                    {
                        // Disable shadows
                        if (Time.timeSinceLevelLoad < 0.5f)
                        {
                            item.m_ShadowController.ForceEnabled(false);
                        }
                        else
                        {
                            item.m_ShadowController.SetEnabled(false);
                        }
                    }
                }

                // only count active ones (to let them the time to be enabled/disabled)
                if (item.m_GameLight.m_Light.enabled == true && item.m_GameLight.gameObject.activeInHierarchy && item.m_GameLight.m_Light.shadows != LightShadows.None)
                {
                    m_CurrentActiveShadows++;
                }
            }
        }

        void RestoreShadows()
        {
            foreach (ComparableLight item in m_RegisteredLights)
            {
                item.m_ShadowController.ForceEnabled(true);
            }
        }

        #endregion


        #region SORTING ALGORITHM

        [System.Serializable]
        public class ComparableLight : System.IComparable<ComparableLight>
        {

            public float m_DistanceToPlayer;
            public bool m_IsInRange;

            public ShadowController m_ShadowController;
            public GameLight m_GameLight;

            public void UpdateDistance(Vector3 pos)
            {

                m_DistanceToPlayer = (m_GameLight.transform.position - pos).magnitude;
                m_IsInRange = InRange();
            }

            bool InRange()
            {
                if (m_ShadowController.m_Importance == ShadowController.ImportanceMode.UNRESTRICTED)
                    return true;
                if (m_ShadowController.GetShadowRange() > m_DistanceToPlayer)
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Compares to.
            /// -1 is better than 1
            /// </summary>
            public int CompareTo(ComparableLight lightItem)
            {
                // Priority to enabled game objects
                //				if (m_GameLight.gameObject.activeInHierarchy == true
                //				    && lightItem.m_GameLight.gameObject.activeInHierarchy == false) {
                //					return -1;
                //				} else if (m_GameLight.gameObject.activeInHierarchy == false
                //				           && lightItem.m_GameLight.gameObject.activeInHierarchy == true) {
                //					return 1;
                //				}

                // Priority to enabled lights
                //				if (m_GameLight.enabled == true
                //				    && lightItem.m_GameLight.enabled == false) {
                //					return -1;
                //				} else if (m_GameLight.enabled == false
                //				           && lightItem.m_GameLight.enabled == true) {
                //					return 1;
                //				}

                // Priority to In Range lights
                if (m_IsInRange && !lightItem.m_IsInRange)
                {
                    return -1;
                }
                if (!m_IsInRange && lightItem.m_IsInRange)
                {
                    return 1;
                }

                // Priority to light with a higher importance
                if (m_ShadowController.m_Importance > lightItem.m_ShadowController.m_Importance)
                {
                    return -1;
                }
                else if (m_ShadowController.m_Importance < lightItem.m_ShadowController.m_Importance)
                {
                    return 1;
                }

                // Priority to light with a higher priority
                if (m_ShadowController.m_Priority > lightItem.m_ShadowController.m_Priority)
                {
                    return -1;
                }
                else if (m_ShadowController.m_Priority < lightItem.m_ShadowController.m_Priority)
                {
                    return 1;
                }

                // Priority to the nearest light
                return m_DistanceToPlayer.CompareTo(lightItem.m_DistanceToPlayer);

            }
        }

        #endregion

#if UNITY_EDITOR
        Color m_UnrestrictedColor = Color.blue;
        Color m_HighColor = Color.white;
        Color m_MediumColor = Color.yellow;
        Color m_LowColor = new Color(0.9f, 0.6f, 0f);

        void OnDrawGizmosSelected()
        {
            if (!m_DebugView)
                return;

            for (int i = 0; i < m_LightsPriorityList.Count; ++i)
            {
                ComparableLight light = m_LightsPriorityList[i];

                // only count active ones (to let them the time to be enabled/disabled)
                if (light.m_GameLight.gameObject.activeInHierarchy && light.m_GameLight.m_Light.shadows != LightShadows.None)
                {
                    if (light.m_ShadowController.m_Importance == ShadowController.ImportanceMode.UNRESTRICTED)
                    {
                        Debug.DrawLine(transform.position, light.m_GameLight.transform.position, m_UnrestrictedColor);
                    }
                    else if (light.m_ShadowController.m_Importance == ShadowController.ImportanceMode.HIGH)
                    {
                        Debug.DrawLine(transform.position, light.m_GameLight.transform.position, m_HighColor);
                    }
                    else if (light.m_ShadowController.m_Importance == ShadowController.ImportanceMode.MEDIUM)
                    {
                        Debug.DrawLine(transform.position, light.m_GameLight.transform.position, m_MediumColor);
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, light.m_GameLight.transform.position, m_LowColor);
                    }

                }
            }
        }
        public static bool HasNGSSSupportEnabled()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(target);
            return UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Contains("HASS_NGSS");
        }

        public static void SetSupportNGSS(bool support)
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(target);
            if (support)
            {
                UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(group, UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(group) + ";HASS_NGSS");
            }
            else
            {
                UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(group, UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Replace("HASS_NGSS", ""));
            }
        }


#endif

    }

}