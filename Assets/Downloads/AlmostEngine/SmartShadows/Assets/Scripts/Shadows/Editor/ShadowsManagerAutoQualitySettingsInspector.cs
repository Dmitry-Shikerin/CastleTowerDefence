using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


using UnityEditor;


namespace AlmostEngine.Shadows
{
	[CustomEditor(typeof(ShadowManagerAutoQualitySettings))]
	public class ShadowsManagerAutoQualitySettingsInspector : Editor
	{
		ShadowManagerAutoQualitySettings m_Target;

		void OnEnable()
		{
			m_Target = (ShadowManagerAutoQualitySettings)target;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GUILayout.Label("Current quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);

			int nbQuality = QualitySettings.names.Length;

			while (m_Target.m_QualityLevels.Count < nbQuality) {
				m_Target.m_QualityLevels.Add(new ShadowManagerConfig());
			}

			SerializedProperty list = serializedObject.FindProperty("m_QualityLevels");
			for (int i = 0; i < nbQuality; ++i) {
				GUILayout.Label(QualitySettings.names[i].ToUpper(), EditorStyles.boldLabel);
				EditorGUI.indentLevel++;

				SerializedProperty element = list.GetArrayElementAtIndex(i);
				while (element.Next(true) && (i == nbQuality - 1 || element.propertyType != list.GetArrayElementAtIndex(i).propertyType)) {
					EditorGUILayout.PropertyField(element);
				}

				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}