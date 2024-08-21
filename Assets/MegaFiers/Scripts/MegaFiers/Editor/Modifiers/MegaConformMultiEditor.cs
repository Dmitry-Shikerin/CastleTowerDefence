using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaConformMulti))]
	public class MegaConformMultiEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Multi Conform Modifier by Chris West"; }

		public override bool DisplayCommon()
		{
			return false;
		}

		public override bool Inspector()
		{
			MegaConformMulti mod = (MegaConformMulti)target;

			CommonModParamsBasic(mod);

			MegaEditorGUILayout.Slider(target, "Conform Amount", ref mod.conformAmount, 0.0f, 1.0f);
			MegaEditorGUILayout.Float(target, "Ray Start Off", ref mod.raystartoff);
			MegaEditorGUILayout.Float(target, "Ray Dist", ref mod.raydist);
			MegaEditorGUILayout.Float(target, "Offset", ref mod.offset);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);

			if ( GUILayout.Button("Add Target") )
			{
				Undo.RecordObject(target, "Changed Add Target");
				MegaConformTarget targ = new MegaConformTarget();
				mod.targets.Add(targ);
				GUI.changed = true;
			}

			for ( int i = 0; i < mod.targets.Count; i++ )
			{
				MegaEditorGUILayout.GameObject(target, "Object", ref mod.targets[i].target, true);
				MegaEditorGUILayout.Toggle(target, "Include Children", ref mod.targets[i].children);

				if ( GUILayout.Button("Delete") )
				{
					Undo.RecordObject(target, "Changed Delet Target");
					mod.targets.RemoveAt(i);
					GUI.changed = true;
				}
			}

			if ( GUI.changed )
				mod.BuildColliderList();

			return false;
		}
	}
}