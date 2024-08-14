using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


namespace AlmostEngine.Shadows
{
	[CustomEditor(typeof(ShadowController))]
	[CanEditMultipleObjects]
	public class ShadowControllerInspector : Editor
	{

		ShadowController m_Target;

		void OnEnable()
		{
			m_Target = (ShadowController)target;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Importance"));

			if (m_Target.m_Importance != ShadowController.ImportanceMode.IGNORE && m_Target.m_Importance != ShadowController.ImportanceMode.UNRESTRICTED) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Priority"));

				EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DistanceMode"));
				if (m_Target.m_DistanceMode == ShadowController.DistanceMode.CUSTOM) {
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_CustomMaxShadowDistance"));
					EditorGUI.indentLevel--;
				}
				if (m_Target.m_DistanceMode == ShadowController.DistanceMode.PROPORTIONAL_TO_LIGHT_RANGE) {
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LightRangeShadowDistanceCoeff"));
					EditorGUI.indentLevel--;
				}

				EditorGUILayout.PropertyField(serializedObject.FindProperty("m_IntensityReductionMode"));
				if (m_Target.m_IntensityReductionMode == ShadowController.IntensityReductionMode.CUSTOM) {
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_CustomIntensityReductionCoeff"));
					EditorGUI.indentLevel--;
				}

				EditorGUILayout.PropertyField(serializedObject.FindProperty("m_RangeReductionMode"));
				if (m_Target.m_RangeReductionMode == ShadowController.RangeReductionMode.CUSTOM) {
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_CustomRangeReductionCoeff"));
					EditorGUI.indentLevel--;
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

	}
}