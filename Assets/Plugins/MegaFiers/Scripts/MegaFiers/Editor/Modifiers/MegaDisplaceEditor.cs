using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaDisplace))]
	public class MegaDisplaceEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Displace Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaDisplace mod = (MegaDisplace)target;

			MegaEditorGUILayout.Texture2D(target, "Map", ref mod.map);
			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Vector2(target, "Offset", ref mod.offset);
			MegaEditorGUILayout.Float(target, "Vertical", ref mod.vertical);
			MegaEditorGUILayout.Vector2(target, "Scale", ref mod.scale);
			MegaEditorGUILayout.Channel(target, "Channel", ref mod.channel);
			MegaEditorGUILayout.Toggle(target, "Cent Lum", ref mod.CentLum);
			MegaEditorGUILayout.Float(target, "Cent Val", ref mod.CentVal);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.Decay);
			return false;
		}
	}
}