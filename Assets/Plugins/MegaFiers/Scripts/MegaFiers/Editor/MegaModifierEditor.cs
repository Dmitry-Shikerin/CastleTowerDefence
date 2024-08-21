using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaModifier))]
	public class MegaModifierEditor : Editor
	{
		public bool				showhelp = false;
		public virtual string	GetHelpString()	{ return "Modifier by Chris West"; }
		public virtual bool		Inspector()	{ return true; }
		public virtual bool		DisplayCommon() { return true; }
		private MegaModifier	src;

		public virtual void Enable()
		{
		}

		private void OnEnable()
		{
			src = target as MegaModifier;
			Enable();
		}

		void OnDestroy()
		{
#if UNITY_2022_2_OR_NEWER
			MegaModifyObject[] con = GameObject.FindObjectsByType<MegaModifyObject>(FindObjectsSortMode.None);
#else
			MegaModifyObject[] con = GameObject.FindObjectsOfType<MegaModifyObject>();
#endif

			for ( int i = 0; i < con.Length; i++ )
			{
				con[i].BuildList();
			}
		}

		public void CommonModParamsBasic(MegaModifier mod)
		{
			MegaEditorGUILayout.Text(target, "Label", ref mod.Label);
			MegaEditorGUILayout.Int(target, "MaxLOD", ref mod.MaxLOD);
			MegaEditorGUILayout.Toggle(target, "Mod Enabled", ref mod.ModEnabled);
			MegaEditorGUILayout.Toggle(target, "Display Gizmo", ref mod.DisplayGizmo);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Gizmo Col");
			MegaEditorGUILayout.Color(target, ref mod.gizCol1);
			MegaEditorGUILayout.Color(target, ref mod.gizCol2);
			EditorGUILayout.EndHorizontal();

			MegaEditorGUILayout.Int(target, "Gizmo Detail", ref mod.steps);
			if ( mod.steps < 1 )
				mod.steps = 1;

			MegaEditorGUILayout.Toggle(target, "Always Changing", ref mod.alwaysChanging);
		}

		public void CommonModParams(MegaModifier mod)
		{
			//mod.showModParams = EditorGUILayout.Foldout(mod.showModParams, "Modifier Common Params");
			mod.showModParams = EditorGUILayout.BeginFoldoutHeaderGroup(mod.showModParams, "Modifier Common Params");

			if ( mod.showModParams )
			{
				EditorGUILayout.BeginHorizontal();

				if ( GUILayout.Button("Rst Off") )
				{
					Undo.RecordObject(target, "Reset Off");
					mod.Offset = Vector3.zero;
					EditorUtility.SetDirty(target);
				}

				if ( GUILayout.Button("Rst Pos") )
				{
					Undo.RecordObject(target, "Reset Pos");
					mod.gizmoPos = Vector3.zero;
					EditorUtility.SetDirty(target);
				}

				if ( GUILayout.Button("Rst Rot") )
				{
					Undo.RecordObject(target, "Reset Rot");
					mod.gizmoRot = Vector3.zero;
					EditorUtility.SetDirty(target);
				}

				if ( GUILayout.Button("Rst Scl") )
				{
					Undo.RecordObject(target, "Reset Scale");
					mod.gizmoScale = Vector3.one;
					EditorUtility.SetDirty(target);
				}
				EditorGUILayout.EndHorizontal();

				MegaEditorGUILayout.Vector3(target, "Offset", ref mod.Offset);
				MegaEditorGUILayout.Vector3(target, "Gizmo Pos", ref mod.gizmoPos);
				MegaEditorGUILayout.Vector3(target, "Gizmo Rot", ref mod.gizmoRot);
				MegaEditorGUILayout.Vector3(target, "Gizmo Scale", ref mod.gizmoScale);
				CommonModParamsBasic(mod);
			}

			EditorGUILayout.EndFoldoutHeaderGroup();
		}

		public virtual void DrawGUI()
		{
			MegaModifier mod = (MegaModifier)target;
			MegaModifyObject context = mod.GetComponent<MegaModifyObject>();
			if ( context == null )
			{
				EditorGUILayout.LabelField("You need to Add a Mega Modify Object Component");
				return;
			}

			if ( DisplayCommon() )
			{
				EditorGUILayout.BeginVertical("box");
				CommonModParams((MegaModifier)target);
				EditorGUILayout.EndVertical();
			}

			if ( GUI.changed )
				EditorUtility.SetDirty(target);

			if ( Inspector() )
				DrawDefaultInspector();
		}

		void ArrowCap(int id, Vector3 pos, Quaternion rot, float size)
		{
			Handles.ArrowHandleCap(id, pos, rot, size, EventType.Repaint);
		}

		public virtual void DrawSceneGUI()
		{
			MegaModifier mod = (MegaModifier)target;

			if ( mod.ModEnabled && mod.DisplayGizmo && MegaModifyObject.GlobalDisplay && mod.showModParams )
			{
				MegaModifyObject context = mod.GetComponent<MegaModifyObject>();

				if ( context != null && context.Enabled && context.DrawGizmos )
				{
					float a = mod.gizCol1.a;
					Color col = Color.white;

					Quaternion rot = mod.transform.localRotation;
					Handles.matrix = mod.transform.localToWorldMatrix;

					if ( mod.Offset != Vector3.zero )
					{
						Vector3 pos = -mod.Offset;
						Handles.Label(pos, mod.ModName() + " Offset\n" + mod.Offset.ToString("0.000"));
						col = Color.blue;
						col.a = a;
						Handles.color = col;
						ArrowCap(0, pos, rot * Quaternion.Euler(180.0f, 0.0f, 0.0f), mod.GizmoSize());
						col = Color.green;
						col.a = a;
						Handles.color = col;
						ArrowCap(0, pos, rot * Quaternion.Euler(90.0f, 0.0f, 0.0f), mod.GizmoSize());
						col = Color.red;
						col.a = a;
						Handles.color = col;
						ArrowCap(0, pos, rot * Quaternion.Euler(0.0f, -90.0f, 0.0f), mod.GizmoSize());
					}

					// gizmopos
					if ( mod.gizmoPos != Vector3.zero )
					{
						Vector3 pos = -mod.gizmoPos;
						Handles.Label(pos, mod.ModName() + " Pos\n" + mod.gizmoPos.ToString("0.000"));
						col = Color.blue;
						col.a = a;
						Handles.color = col;
						ArrowCap(0, pos, rot * Quaternion.Euler(180.0f, 0.0f, 0.0f), mod.GizmoSize());
						col = Color.green;
						col.a = a;
						Handles.color = col;
						ArrowCap(0, pos, rot * Quaternion.Euler(90.0f, 0.0f, 0.0f), mod.GizmoSize());
						col = Color.red;
						col.a = a;
						Handles.color = col;
						ArrowCap(0, pos, rot * Quaternion.Euler(0.0f, -90.0f, 0.0f), mod.GizmoSize());
					}
					Handles.matrix = Matrix4x4.identity;
				}
			}
		}

		public override void OnInspectorGUI()
		{
			MegaModifier mod = (MegaModifier)target;

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