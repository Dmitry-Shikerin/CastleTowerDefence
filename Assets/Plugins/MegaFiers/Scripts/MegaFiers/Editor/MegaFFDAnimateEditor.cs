using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaFFDAnimate))]
	public class MegaFFDAnimateEditor : Editor
	{
		bool showpoints = false;

		public override void OnInspectorGUI()
		{
			MegaFFDAnimate ffd = (MegaFFDAnimate)target;

			MegaEditorGUILayout.Toggle(target, "Enabled", ref ffd.Enabled);

			bool rec = ffd.GetRecord();
			MegaEditorGUILayout.Toggle(target, "Record", ref rec);
			ffd.SetRecord(rec);

			MegaFFD mod = ffd.GetFFD();

			showpoints = EditorGUILayout.Foldout(showpoints, "Points");

			if ( mod && showpoints )
			{
				int size = mod.GridSize();
				size = size * size * size;
				for ( int i = 0; i < size; i++ )
				{
					Vector3 p = ffd.GetPoint(i);
					MegaEditorGUILayout.Vector3(target, "p" + i, ref p);
					ffd.SetPoint(i, p);
				}
			}

			if ( GUI.changed )
				EditorUtility.SetDirty(target);
		}
	}
}