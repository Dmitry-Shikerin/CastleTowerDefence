using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaPoke))]
	public class MegaPokeEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Poke Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaPoke mod = (MegaPoke)target;

			MegaEditorGUILayout.Transform(mod, "From Obj", ref mod.fromObj, true);
			MegaEditorGUILayout.Transform(mod, "To Obj", ref mod.toObj, true);
			MegaEditorGUILayout.Toggle(target, "Preserve Vol", ref mod.preserveVol);
			MegaEditorGUILayout.Float(target, "Strength", ref mod.strength);
			MegaEditorGUILayout.Falloff(target, "Falloff Type", ref mod.falloffType);
			MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
			MegaEditorGUILayout.Curve(target, "Falloff Curve", ref mod.curve);
			return false;
		}

		public override void DrawSceneGUI()
		{
			MegaPoke mod = (MegaPoke)target;

			if ( mod.fromObj )
			{
				mod.fromObj.position = Handles.PositionHandle(mod.fromObj.position, Quaternion.identity);
				Handles.Label(mod.fromObj.position, "From");
			}

			if ( mod.toObj )
			{
				mod.toObj.position = Handles.PositionHandle(mod.toObj.position, Quaternion.identity);
				Handles.Label(mod.toObj.position, "To");
			}

			if ( mod.fromObj && mod.toObj )
			{
				Vector3[] pos = new Vector3[2];
				pos[0] = mod.fromObj.position;
				pos[1] = mod.toObj.position;
				Handles.DrawAAPolyLine(pos);
			}

			if ( mod.fromObj )
			{
				Handles.DrawWireArc(mod.fromObj.position, Vector3.up, Vector3.forward, 360.0f, mod.radius);
				//Handles.DrawWireArc(mod.fromObj.position, Vector3.right, Vector3.forward, 360.0f, mod.radius);
			}
		}
	}
}