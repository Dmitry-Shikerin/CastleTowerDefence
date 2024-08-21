using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaHose))]
	public class MegaHoseEditor : Editor
	{
		[MenuItem("GameObject/Create Other/MegaShape/Hose New")]
		static void CreatePageMesh()
		{
			Vector3 pos = Vector3.zero;

			if ( UnityEditor.SceneView.lastActiveSceneView )
				pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

			GameObject go = new GameObject("Hose New");

			MeshFilter mf = go.AddComponent<MeshFilter>();
			mf.sharedMesh = new Mesh();
			MeshRenderer mr = go.AddComponent<MeshRenderer>();

			Material[] mats = new Material[3];

			mr.sharedMaterials = mats;
			MegaHose pm = go.AddComponent<MegaHose>();

			go.transform.position = pos;
			Selection.activeObject = go;
			pm.rebuildcross = true;
			pm.updatemesh = true;
		}

		public override void OnInspectorGUI()
		{
			MegaHose mod = (MegaHose)target;

			MegaEditorGUILayout.Toggle(target, "Do Late Update", ref mod.dolateupdate);
			MegaEditorGUILayout.Toggle(target, "Invisible Update", ref mod.InvisibleUpdate);
			MegaEditorGUILayout.HoseType(target, "Wire Type", ref mod.wiretype);
			MegaEditorGUILayout.Int(target, "Segments", ref mod.segments);
			MegaEditorGUILayout.Toggle(target, "Cap Ends", ref mod.capends);
			MegaEditorGUILayout.Toggle(target, "Calc Normals", ref mod.calcnormals);
			MegaEditorGUILayout.Toggle(target, "Calc Tangents", ref mod.calctangents);
			MegaEditorGUILayout.Toggle(target, "Calc Collider", ref mod.recalcCollider);

			switch ( mod.wiretype )
			{
				case MegaHoseType.Round:
					MegaEditorGUILayout.Float(target, "Diameter", ref mod.rnddia);
					MegaEditorGUILayout.Int(target, "Sides", ref mod.rndsides);
					MegaEditorGUILayout.Float(target, "Rotate", ref mod.rndrot);
					break;

				case MegaHoseType.Rectangle:
					MegaEditorGUILayout.Float(target, "Width", ref mod.rectwidth);
					MegaEditorGUILayout.Float(target, "Depth", ref mod.rectdepth);
					MegaEditorGUILayout.Float(target, "Fillet", ref mod.rectfillet);
					MegaEditorGUILayout.Int(target, "Fillet Sides", ref mod.rectfilletsides);
					MegaEditorGUILayout.Float(target, "Rotate", ref mod.rectrotangle);
					break;

				case MegaHoseType.DSection:
					MegaEditorGUILayout.Float(target, "Width", ref mod.dsecwidth);
					MegaEditorGUILayout.Float(target, "Depth", ref mod.dsecdepth);
					MegaEditorGUILayout.Int(target, "Rnd Sides", ref mod.dsecrndsides);
					MegaEditorGUILayout.Float(target, "Fillet", ref mod.dsecfillet);
					MegaEditorGUILayout.Int(target, "Fillet Sides", ref mod.dsecfilletsides);
					MegaEditorGUILayout.Float(target, "Rotate", ref mod.dsecrotangle);
					break;
			}

			MegaEditorGUILayout.Vector2(target, "UV Scale", ref mod.uvscale);

			if ( GUI.changed )
			{
				mod.updatemesh = true;
				mod.rebuildcross = true;
			}

			MegaEditorGUILayout.GameObject(target, "Start Object", ref mod.custnode, true);
			MegaEditorGUILayout.Vector3(target, "Offset", ref mod.offset);
			MegaEditorGUILayout.Vector3(target, "Rotate", ref mod.rotate);
			MegaEditorGUILayout.GameObject(target, "End Object", ref mod.custnode2, true);
			MegaEditorGUILayout.Vector3(target, "Offset", ref mod.offset1);
			MegaEditorGUILayout.Vector3(target, "Rotate", ref mod.rotate1);
			MegaEditorGUILayout.BeginToggle(target, "Flex On", ref mod.flexon);
			MegaEditorGUILayout.Slider(target, "Start", ref mod.flexstart, 0.0f, 1.0f);
			MegaEditorGUILayout.Slider(target, "Stop", ref mod.flexstop, 0.0f, 1.0f);

			if ( mod.flexstart > mod.flexstop )
				mod.flexstart = mod.flexstop;

			if ( mod.flexstop < mod.flexstart )
				mod.flexstop = mod.flexstart;

			MegaEditorGUILayout.Int(target, "Cycles", ref mod.flexcycles);
			MegaEditorGUILayout.Float(target, "Diameter", ref mod.flexdiameter);

			EditorGUILayout.EndToggleGroup();

			MegaEditorGUILayout.BeginToggle(target, "Use Bulge Curve", ref mod.usebulgecurve);
			MegaEditorGUILayout.Curve(target, "Bulge", ref mod.bulge);
			MegaEditorGUILayout.Float(target, "Bulge Amount", ref mod.bulgeamount);
			MegaEditorGUILayout.Float(target, "Bulge Offset", ref mod.bulgeoffset);
			MegaEditorGUILayout.BeginToggle(target, "Animate", ref mod.animatebulge);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.bulgespeed);
			MegaEditorGUILayout.Float(target, "Min", ref mod.minbulge);
			MegaEditorGUILayout.Float(target, "Max", ref mod.maxbulge);
			MegaEditorGUILayout.EndToggle();
			MegaEditorGUILayout.EndToggle();

			MegaEditorGUILayout.BeginToggle(target, "Use Size Curve", ref mod.usesizecurve);
			MegaEditorGUILayout.Curve(target, "Size", ref mod.size);
			MegaEditorGUILayout.EndToggle();

			MegaEditorGUILayout.Float(target, "Tension Start", ref mod.tension1);
			MegaEditorGUILayout.Float(target, "Tension End", ref mod.tension2);

			MegaEditorGUILayout.BeginToggle(target, "Free Create", ref mod.freecreate);
			MegaEditorGUILayout.Float(target, "Free Length", ref mod.noreflength);
			MegaEditorGUILayout.EndToggle();

			MegaEditorGUILayout.Vector3(target, "Up", ref mod.up);
			MegaEditorGUILayout.Toggle(target, "Display Spline", ref mod.displayspline);

			if ( GUI.changed )
			{
				mod.updatemesh = true;
				mod.Rebuild();
			}
		}

		[DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Pickable | GizmoType.InSelectionHierarchy)]
		static void RenderGizmo(MegaHose hose, GizmoType gizmoType)
		{
			if ( (gizmoType & GizmoType.Active) != 0 && Selection.activeObject == hose.gameObject )
			{
				if ( !hose.displayspline )
					return;

				if ( hose.custnode == null || hose.custnode2 == null )
					return;

				DrawGizmos(hose, new Color(1.0f, 1.0f, 1.0f, 1.0f));
				Color col = Color.yellow;
				col.a = 0.5f;
				Gizmos.color = col;

				Matrix4x4 RingTM = Matrix4x4.identity;
				hose.CalcMatrix(ref RingTM, 0.0f);
				RingTM = hose.transform.localToWorldMatrix * RingTM;

				float gsize = 0.0f;
				switch ( hose.wiretype )
				{
					case MegaHoseType.Round: gsize = hose.rnddia; break;
					case MegaHoseType.Rectangle: gsize = (hose.rectdepth + hose.rectwidth) * 0.5f; break;
					case MegaHoseType.DSection: gsize = (hose.dsecdepth + hose.dsecwidth) * 0.5f; break;
				}

				gsize *= 0.1f;

				for ( int p = 0; p < hose.hosespline.knots.Count; p++ )
				{
					Vector3 p1 = RingTM.MultiplyPoint(hose.hosespline.knots[p].p);
					Vector3 p2 = RingTM.MultiplyPoint(hose.hosespline.knots[p].invec);
					Vector3 p3 = RingTM.MultiplyPoint(hose.hosespline.knots[p].outvec);

					Gizmos.color = Color.black;
					Gizmos.DrawLine(p2, p1);
					Gizmos.DrawLine(p3, p1);

					Gizmos.color = Color.green;
					Gizmos.DrawSphere(p1, gsize);

					Gizmos.color = Color.red;
					Gizmos.DrawSphere(p2, gsize);
					Gizmos.DrawSphere(p3, gsize);
				}
			}
		}

		static void DrawGizmos(MegaHose hose, Color modcol1)
		{
			Matrix4x4 RingTM = Matrix4x4.identity;
			Matrix4x4 tm = hose.transform.localToWorldMatrix;

			float ldist = 1.0f * 0.1f;
			if ( ldist < 0.01f )
				ldist = 0.01f;

			Color modcol = modcol1;

			if ( hose.hosespline.length / ldist > 500.0f )
				ldist = hose.hosespline.length / 500.0f;

			float ds = hose.hosespline.length / (hose.hosespline.length / ldist);

			if ( ds > hose.hosespline.length )
				ds = hose.hosespline.length;

			int c = 0;
			int k = -1;
			int lk = -1;

			Vector3 first = hose.hosespline.Interpolate(0.0f, true, ref lk);

			hose.CalcMatrix(ref RingTM, 0.0f);
			RingTM = tm * RingTM;

			for ( float dist = ds; dist < hose.hosespline.length; dist += ds )
			{
				float alpha = dist / hose.hosespline.length;

				Vector3 pos = hose.hosespline.Interpolate(alpha, true, ref k);

				if ( (c & 1) == 1 )
					Gizmos.color = Color.black * modcol;
				else
					Gizmos.color = Color.yellow * modcol;

				if ( k != lk )
				{
					for ( lk = lk + 1; lk <= k; lk++ )
					{
						Gizmos.DrawLine(RingTM.MultiplyPoint(first), RingTM.MultiplyPoint(hose.hosespline.knots[lk].p));
						first = hose.hosespline.knots[lk].p;
					}
				}

				lk = k;

				Gizmos.DrawLine(RingTM.MultiplyPoint(first), RingTM.MultiplyPoint(pos));

				c++;

				first = pos;
			}

			if ( (c & 1) == 1 )
				Gizmos.color = Color.blue * modcol;
			else
				Gizmos.color = Color.yellow * modcol;

			Vector3 lastpos;
			if ( hose.hosespline.closed )
				lastpos = hose.hosespline.Interpolate(0.0f, true, ref k);
			else
				lastpos = hose.hosespline.Interpolate(1.0f, true, ref k);

			Gizmos.DrawLine(RingTM.MultiplyPoint(first), RingTM.MultiplyPoint(lastpos));
		}
	}
}