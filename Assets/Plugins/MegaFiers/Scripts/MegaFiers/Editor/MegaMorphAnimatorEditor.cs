using UnityEngine;
using UnityEditor;

#if false
namespace MegaFiers
{
	[CustomEditor(typeof(MegaMorphAnimator))]
	public class MegaMorphAnimatorEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			MegaMorphAnimator anim = (MegaMorphAnimator)target;

			string[] clips = anim.GetClipNames();

			MegaEditorGUILayout.BeginToggle(target, "Use Frames", ref anim.useFrames);
			MegaEditorGUILayout.Int(target, "Source FPS", ref anim.sourceFPS);
			MegaEditorGUILayout.EndToggle();

			MegaEditorGUILayout.Toggle(target, "Multiple Morphs", ref anim.MultipleMorphs);
			MegaEditorGUILayout.Toggle(target, "Linked Update", ref anim.LinkedUpdate);
			MegaEditorGUILayout.Toggle(target, "Play On Start", ref anim.PlayOnStart);

			int current = anim.current;
			MegaEditorGUILayout.Popup(target, "Playing Clip", ref current, clips);
			if ( current != anim.current )
				anim.PlayClip(current);

			if ( GUILayout.Button("Add Clip") )
			{
				Undo.RecordObject(target, "Add Clip");
				anim.AddClip("Clip " + anim.clips.Count, 0.0f, 1.0f, MegaRepeatMode.Loop);
			}

			EditorGUILayout.BeginVertical();
			for ( int i = 0; i < anim.clips.Count; i++ )
			{
				EditorGUILayout.BeginHorizontal();

				anim.clips[i].name = EditorGUILayout.TextField(anim.clips[i].name);

				if ( anim.useFrames )
				{
					anim.clips[i].start = MegaEditorGUILayout.Float(target, (anim.clips[i].start * anim.sourceFPS), GUILayout.Width(40)) / (float)anim.sourceFPS;
					anim.clips[i].end = MegaEditorGUILayout.Float(target, (anim.clips[i].end * anim.sourceFPS), GUILayout.Width(40)) / (float)anim.sourceFPS;
					anim.clips[i].speed = MegaEditorGUILayout.Float(target, anim.clips[i].speed, GUILayout.Width(40));
					anim.clips[i].loop = (MegaRepeatMode)EditorGUILayout.EnumPopup(anim.clips[i].loop);
				}
				else
				{
					anim.clips[i].start = MegaEditorGUILayout.Float(target, anim.clips[i].start, GUILayout.Width(40));
					anim.clips[i].end = MegaEditorGUILayout.Float(target, anim.clips[i].end, GUILayout.Width(40));
					anim.clips[i].speed = MegaEditorGUILayout.Float(target, anim.clips[i].speed, GUILayout.Width(40));
					anim.clips[i].loop = (MegaRepeatMode)EditorGUILayout.EnumPopup(anim.clips[i].loop);
				}

				if ( GUILayout.Button("-") )
				{
					Undo.RecordObject(target, "Delete Clip");
					anim.clips.Remove(anim.clips[i]);
				}

				EditorGUILayout.EndHorizontal();
			}
		}
	}
}
#endif