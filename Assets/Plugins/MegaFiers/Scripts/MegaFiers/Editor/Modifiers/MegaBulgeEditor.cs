using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaBulge))]
	public class MegaBulgeEditor : MegaModifierEditor
	{

		public override bool Inspector()
		{
			MegaBulge mod = (MegaBulge)target;

			MegaEditorGUILayout.Vector3(target, "Radius", ref mod.Amount);
			MegaEditorGUILayout.Vector3(target, "Falloff", ref mod.FallOff);
			MegaEditorGUILayout.Toggle(target, "Link Falloff", ref mod.LinkFallOff);
			return false;
		}
	}
}