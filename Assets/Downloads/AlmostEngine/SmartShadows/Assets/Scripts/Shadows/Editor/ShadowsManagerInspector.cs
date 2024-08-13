using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


using UnityEditor;


namespace AlmostEngine.Shadows
{
    [CustomEditor(typeof(ShadowManager))]
    public class ShadowsManagerInspector : Editor
    {
        ShadowManager m_Target;

        void OnEnable()
        {
            m_Target = (ShadowManager)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            GUILayout.Label("Constraints", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Config.m_MaxActiveShadows"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Config.m_DefaultMaxShadowDistance"));
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();


            GUILayout.Label("Reductions", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Config.m_ReduceInactiveLightRange"));
            if (m_Target.m_Config.m_ReduceInactiveLightRange)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Config.m_DefaultRangeReductionCoeff"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Config.m_ReduceInactiveLightIntensity"));
            if (m_Target.m_Config.m_ReduceInactiveLightIntensity)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Config.m_DefaultIntensityReductionCoeff"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Config.m_ReduceShadowResolutionOfLeastPriority"));
            if (m_Target.m_Config.m_ReduceShadowResolutionOfLeastPriority)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Config.m_ReduceResolutionOfLast"));
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();


            GUILayout.Label("Animation", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FadeInTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FadeOutTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_RangeFadeInTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_RangeFadeOutTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_IntensityFadeInTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_IntensityFadeOutTime"));
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();


            GUILayout.Label("Importance", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DefaultImportanceMode"));
            if (m_Target.m_DefaultImportanceMode == ShadowManager.DefaultImportanceMode.AUTO)
            {

                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MediumMinLightRange"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MediumMinLightIntensity"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_HighMinLightRange"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_HighMinLightIntensity"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DefaultImportance"));
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();



            if (typeof(ShadowManager).Assembly.GetType("NGSS_Local") != null)
            {
                GUILayout.Label("NGSS support", EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                if (ShadowManager.HasNGSSSupportEnabled())
                {
                    EditorGUILayout.HelpBox("Next Gen Soft Shadows support is enabled for the current target platform.", MessageType.Info);
                    if (GUILayout.Button("Disable"))
                    {
                        ShadowManager.SetSupportNGSS(false);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Next Gen Soft Shadows support is disable for the current target platform. Set Enable to support smooth shadow transitions.", MessageType.Info);
                    if (GUILayout.Button("Enable"))
                    {
                        ShadowManager.SetSupportNGSS(true);
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }


            GUILayout.Label("Debug", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Current active shadows " + m_Target.m_CurrentActiveShadows);
            GUILayout.Label("Current registered shadows " + m_Target.m_RegisteredLights.Count);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DebugView"));
            EditorGUILayout.EndVertical();
			EditorGUILayout.Space();






            GUILayout.Label("Support", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(GUI.skin.box);

			Color cc = GUI.color;
			GUI.color = new Color (0.55f, 0.7f, 1f, 1.0f);

			if (GUILayout.Button ("More assets from Wild Mage Games")) {
				Application.OpenURL ("https://www.wildmagegames.com/unity/");
			}

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Contact support")) {
				Application.OpenURL ("mailto:support@wildmagegames.com");
			}

			GUI.color = new Color (0.6f, 1f, 0.6f, 1.0f);
			if (GUILayout.Button ("Leave a Review")) {
				Application.OpenURL ("https://www.assetstore.unity3d.com/#!/content/103152");
			}
			GUI.color = cc;
			EditorGUILayout.EndHorizontal ();
            EditorGUILayout.EndVertical();




            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField(SmartShadows.VERSION, UIStyle.centeredGreyTextStyle);
            EditorGUILayout.LabelField(SmartShadows.AUTHOR, UIStyle.centeredGreyTextStyle);


            serializedObject.ApplyModifiedProperties();


        }
    }

}