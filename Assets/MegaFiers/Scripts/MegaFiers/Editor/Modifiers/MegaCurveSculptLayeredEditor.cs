using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaCurveSculptLayered))]
	public class MegaCurveSculptLayeredEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Mega Curve Sculpt New Modifier by Chris West"; }

		void CurveGUI(MegaSculptCurve crv)
		{
			MegaEditorGUILayout.BeginToggle(target, "Enabled", ref crv.enabled);
			MegaEditorGUILayout.Text(target, "Name", ref crv.name);
			MegaEditorGUILayout.Axis(target, "Axis", ref crv.axis);
			MegaEditorGUILayout.Curve(target, "Curve", ref crv.curve);
			MegaEditorGUILayout.Slider(target, "Weight", ref crv.weight, 0.0f, 1.0f);

			MegaEditorGUILayout.AffectPopup(target, "Affect Off", ref crv.affectOffset);
			if ( crv.affectOffset != MegaAffect.None )
				MegaEditorGUILayout.Vector3(target, "Offset", ref crv.offamount);

			MegaEditorGUILayout.AffectPopup(target, "Affect Scl", ref crv.affectScale);
			if ( crv.affectScale != MegaAffect.None )
				MegaEditorGUILayout.Vector3(target, "Scale", ref crv.sclamount);

			MegaEditorGUILayout.BeginToggle(target, "Limits", ref crv.uselimits);
			MegaEditorGUILayout.Color(target, "Col", ref crv.regcol);
			MegaEditorGUILayout.Vector3(target, "Origin", ref crv.origin);
			MegaEditorGUILayout.Vector3(target, "Boxsize", ref crv.boxsize);
			EditorGUILayout.EndToggleGroup();
			EditorGUILayout.EndToggleGroup();
		}

		void SwapCurves(MegaCurveSculptLayered mod, int t1, int t2)
		{
			if ( t1 >= 0 && t1 < mod.curves.Count && t2 >= 0 && t2 < mod.curves.Count && t1 != t2 )
			{
				MegaSculptCurve mt1 = mod.curves[t1];
				mod.curves.RemoveAt(t1);
				mod.curves.Insert(t2, mt1);
				EditorUtility.SetDirty(target);
			}
		}

		public override bool Inspector()
		{
			MegaCurveSculptLayered mod = (MegaCurveSculptLayered)target;

			if ( GUILayout.Button("Add Curve") )
			{
				Undo.RecordObject(target, "Add Curve");
				mod.curves.Add(MegaSculptCurve.Create());
			}

			for ( int i = 0; i < mod.curves.Count; i++ )
			{
				CurveGUI(mod.curves[i]);

				EditorGUILayout.BeginHorizontal();

				if ( GUILayout.Button("Up") )
				{
					if ( i > 0 )
					{
						Undo.RecordObject(target, "Swap Curves");
						SwapCurves(mod, i, i - 1);
					}
				}

				if ( GUILayout.Button("Down") )
				{
					if ( i < mod.curves.Count - 1 )
					{
						Undo.RecordObject(target, "Swap Curve");
						SwapCurves(mod, i, i + 1);
					}
				}

				if ( GUILayout.Button("Delete") )
				{
					Undo.RecordObject(target, "Delete Curve");
					mod.curves.RemoveAt(i);
					i--;
				}
				EditorGUILayout.EndHorizontal();
			}

			return false;
		}

		public override void DrawSceneGUI()
		{
			MegaCurveSculptLayered mod = (MegaCurveSculptLayered)target;

			for ( int i = 0; i < mod.curves.Count; i++ )
			{
				if ( mod.curves[i].enabled && mod.curves[i].uselimits )
				{
					Vector3 pos = mod.transform.TransformPoint(mod.curves[i].origin);
					Vector3 newpos = Handles.PositionHandle(pos, Quaternion.identity);

					if ( newpos != pos )
					{
						mod.curves[i].origin = mod.transform.worldToLocalMatrix.MultiplyPoint(newpos);
						EditorUtility.SetDirty(target);
					}
				}
			}
		}

	}
}