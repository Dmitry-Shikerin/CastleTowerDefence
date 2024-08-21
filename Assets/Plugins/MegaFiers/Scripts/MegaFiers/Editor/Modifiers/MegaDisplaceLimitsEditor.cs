using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaDisplaceLimits))]
	public class MegaDisplaceLimitsEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Displace Limits Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaDisplaceLimits mod = (MegaDisplaceLimits)target;

			MegaEditorGUILayout.Texture2D(target, "Map", ref mod.map);
			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Vector2(target, "Offset", ref mod.offset);
			MegaEditorGUILayout.Float(target, "Vertical", ref mod.vertical);
			MegaEditorGUILayout.Vector2(target, "Scale", ref mod.scale);
			MegaEditorGUILayout.Channel(target, "Channel", ref mod.channel);
			MegaEditorGUILayout.Toggle(target, "Cent Lum", ref mod.CentLum);
			MegaEditorGUILayout.Float(target, "Cent Val", ref mod.CentVal);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.Decay);
			MegaEditorGUILayout.Vector3(target, "Origin", ref mod.origin);
			MegaEditorGUILayout.Vector3(target, "Size", ref mod.size);
			return false;
		}

		public override void DrawSceneGUI()
		{
			MegaDisplaceLimits mod = (MegaDisplaceLimits)target;

			Vector3 pos = mod.transform.TransformPoint(mod.origin);
			Vector3 newpos = Handles.PositionHandle(pos, Quaternion.identity);

			if ( newpos != pos )
			{
				mod.origin = mod.transform.worldToLocalMatrix.MultiplyPoint(newpos);
				EditorUtility.SetDirty(target);
			}
		}
	}
}