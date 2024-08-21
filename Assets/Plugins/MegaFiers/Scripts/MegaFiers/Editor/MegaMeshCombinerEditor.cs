using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaMeshCombiner))]
	public class MegaMeshCombinerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			MegaMeshCombiner mod = (MegaMeshCombiner)target;

			if ( GUILayout.Button("Combine") )
				mod.Combine();
		}

		[MenuItem("MegaFiers/Combine Meshes")]
		static public void CombineMeshes()
		{
			GameObject[] objs = Selection.gameObjects;

			for ( int i = 0; i < objs.Length; i++ )
				MegaMeshCombiner.Combine(objs[i]);
		}
	}
}