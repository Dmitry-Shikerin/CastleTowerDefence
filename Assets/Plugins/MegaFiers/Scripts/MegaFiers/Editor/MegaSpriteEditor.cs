using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaSprite))]
	public class MegaSpriteEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			MegaSprite mod = (MegaSprite)target;

			DrawDefaultInspector();

			if ( mod.meshRenderer == null )
			{
				EditorGUILayout.HelpBox("Click the Update Sprite button to set the sprite mesh up. You add the modifiers to the child mesh object that is created.", MessageType.Info);
			}

			if ( GUILayout.Button("Update Sprite") )
			{
				mod.UpdateSprite();
				if ( mod.spriteMeshObj )
				{
					MegaModifyObject modobj = mod.spriteMeshObj.GetComponent<MegaModifyObject>();
					if ( modobj )
					{
						for ( int i = 0; i < modobj.mods.Length; i++ )
						{
							if ( modobj.mods[i].GetType().IsSubclassOf(typeof(MegaFFD)) )
							{
								MegaFFD mffd = (MegaFFD)modobj.mods[i];
								mffd.SetUpdate();
							}
						}

						modobj.SetUpdate();
						modobj.ForceUpdate();
						EditorUtility.SetDirty(modobj);
					}
				}
			}

			if ( GUI.changed )
				EditorUtility.SetDirty(mod);
		}

		public void OnSceneGUI()
		{
			MegaSprite mod = (MegaSprite)target;

			if ( mod )
			{
				Handles.matrix = mod.transform.localToWorldMatrix;
				mod.pivot = MegaEditorGUILayout.PositionHandle(target, mod.pivot, Quaternion.identity);
				Handles.Label(mod.pivot, "Pivot");
			}
		}
	}
}