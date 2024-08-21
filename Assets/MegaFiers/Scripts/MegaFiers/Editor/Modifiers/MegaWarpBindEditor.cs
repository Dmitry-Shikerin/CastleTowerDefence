using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaWarpBind))]
	public class MegaWarpBindEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Warp Bind Modifier by Chris West"; }

		public GameObject	SourceWarpObj;

		public override bool Inspector()
		{
			MegaWarpBind mod = (MegaWarpBind)target;

			GameObject go = mod.SourceWarpObj;
			MegaWarp warp = null;

			if ( go )
				warp = go.GetComponent<MegaWarp>();

			MegaEditorGUILayout.WarpObject(target, "Warp Object", ref warp, true);

			//MegaEditorGUILayout.GameObject(target, "Warp Object", ref go, true);
			if ( warp )
				go = warp.gameObject;
			else
				go = null;

			mod.SetTarget(go);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.decay);
			return false;
		}
	}
}