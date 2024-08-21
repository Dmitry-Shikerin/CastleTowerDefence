using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaWarp))]
	public class MegaWarpEditor : Editor
	{
		public bool				showhelp		= false;
		public bool				showmodparams	= true;
		public virtual string	GetHelpString()	{ return "Warp Modifer by Chris West"; }
		public virtual bool		Inspector()		{ return true; }
		public virtual bool		DisplayCommon()	{ return true; }
		private MegaWarp		src;

		static public void CreateWarp(string type, System.Type classtype)
		{
			Vector3 pos = Vector3.zero;

			if ( UnityEditor.SceneView.lastActiveSceneView != null )
				pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

			GameObject go = new GameObject(type + " Warp");

			go.AddComponent(classtype);

			go.transform.position = pos;
			Selection.activeObject = go;
		}

		[DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Pickable | GizmoType.InSelectionHierarchy)]
		static void RenderGizmo(MegaWarp warp, GizmoType gizmoType)
		{
			if ( MegaModifyObject.GlobalDisplay && warp.DisplayGizmo )
			{
				Color col = Color.white;

				if ( (gizmoType & GizmoType.NotInSelectionHierarchy) != 0 )
					col.a = 0.5f;
				else
				{
					if ( (gizmoType & GizmoType.Active) != 0 )
					{
						if ( warp.Enabled )
							col.a = 1.0f;
						else
							col.a = 0.75f;
					}
					else
					{
						if ( warp.Enabled )
							col.a = 0.5f;
						else
							col.a = 0.25f;
					}
				}
				warp.DrawGizmo(col);
				Gizmos.DrawIcon(warp.transform.position, warp.GetIcon(), false);
			}
		}

		private void OnEnable()
		{
			src = target as MegaWarp;
		}

		public void CommonModParamsBasic(MegaWarp mod)
		{
			MegaEditorGUILayout.Toggle(target, "Enabled", ref mod.Enabled);
			MegaEditorGUILayout.Toggle(target, "Display Gizmo", ref mod.DisplayGizmo);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Gizmo Col");
			MegaEditorGUILayout.Color(target, ref mod.GizCol1);
			MegaEditorGUILayout.Color(target, ref mod.GizCol2);
			EditorGUILayout.EndHorizontal();
		}

		public void CommonModParams(MegaWarp mod)
		{
			showmodparams = EditorGUILayout.Foldout(showmodparams, "Warp Common Params");

			if ( showmodparams )
			{
				EditorGUILayout.BeginVertical("Box");
				CommonModParamsBasic(mod);
				MegaEditorGUILayout.Float(target, "Width", ref mod.Width);
				MegaEditorGUILayout.Float(target, "Height", ref mod.Height);
				MegaEditorGUILayout.Float(target, "Length", ref mod.Length);
				MegaEditorGUILayout.Float(target, "Decay", ref mod.Decay);
				EditorGUILayout.EndVertical();
			}
		}

		public virtual void DrawGUI()
		{
			MegaWarp mod = (MegaWarp)target;

			if ( DisplayCommon() )
				CommonModParams(mod);

			if ( GUI.changed )
				EditorUtility.SetDirty(target);

			if ( Inspector() )
				DrawDefaultInspector();
		}

		public virtual void DrawSceneGUI()
		{
			MegaWarp mod = (MegaWarp)target;

			if ( mod.Enabled && mod.DisplayGizmo && showmodparams )
			{
			}
		}

		public override void OnInspectorGUI()
		{
			DrawGUI();

			if ( GUI.changed )
				EditorUtility.SetDirty(target);
		}

		public void OnSceneGUI()
		{
			DrawSceneGUI();
		}
	}
}