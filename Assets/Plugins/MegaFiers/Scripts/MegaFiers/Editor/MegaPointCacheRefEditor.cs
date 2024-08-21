using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaPointCacheRef))]
	public class MegaPointCacheRefEditor : MegaModifierEditor
	{
		public override void OnInspectorGUI()
		{
			MegaPointCacheRef am = (MegaPointCacheRef)target;

			am.showModParams = EditorGUILayout.Foldout(am.showModParams, "Modifier Common Params");

			if ( am.showModParams )
				CommonModParamsBasic(am);

			MegaEditorGUILayout.PointCache(target, "Source", ref am.source, true);
			MegaEditorGUILayout.Float(target, "Time", ref am.time);
			MegaEditorGUILayout.Float(target, "Loop Time", ref am.maxtime);
			MegaEditorGUILayout.Toggle(target, "Animated", ref am.animated);
			MegaEditorGUILayout.Float(target, "Speed", ref am.speed);
			MegaEditorGUILayout.RepeatMode(target, "Loop Mode", ref am.LoopMode);
			MegaEditorGUILayout.InterpMethod(target, "Interp Method", ref am.interpMethod);
			MegaEditorGUILayout.BlendMode(target, "Blend Mode", ref am.blendMode);
			if ( am.blendMode == MegaBlendAnimMode.Additive )
				MegaEditorGUILayout.Float(target, "Weight", ref am.weight);

			if ( GUI.changed )
				EditorUtility.SetDirty(target);
		}
	}
}