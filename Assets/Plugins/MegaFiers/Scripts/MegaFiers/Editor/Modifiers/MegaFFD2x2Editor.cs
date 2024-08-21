using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaFFD2x2))]
	public class MegaFFD2x2Editor : MegaFFDEditor
	{
		public override bool Inspector()
		{
			MegaFFD mod = (MegaFFD)target;

			MegaEditorGUILayout.Float(target, "Knot Size", ref mod.KnotSize);
			MegaEditorGUILayout.Toggle(target, "In Vol", ref mod.inVol);
			MegaEditorGUILayout.Toggle(target, "Handles", ref handles);
			MegaEditorGUILayout.Slider(target, "Size", ref handleSize, 0.0f, 1.0f);
			MegaEditorGUILayout.Toggle(target, "Mirror X", ref mirrorX);
			MegaEditorGUILayout.Toggle(target, "Mirror Y", ref mirrorY);

			showpoints = EditorGUILayout.Foldout(showpoints, "Points");

			if ( showpoints )
			{
				int gs = mod.GridSize();

				for ( int x = 0; x < gs; x++ )
				{
					for ( int y = 0; y < gs; y++ )
					{
						for ( int z = 0; z < 1; z++ )
						{
							int i = (x * gs * gs) + (y * gs) + z;
							Vector2 p2 = mod.pt[i];
							MegaEditorGUILayout.Vector2(target, "p[" + x + "," + y + "]", ref p2);
							mod.pt[i] = p2;
						}
					}
				}
			}
			return false;
		}

		Vector3 CircleCap(int id, Vector3 pos, Quaternion rot, float size)
		{
			return MegaEditorGUILayout.FreeHandle(target, pos, rot, size, Vector3.zero, Handles.CircleHandleCap);
		}

		public override void DrawSceneGUI()
		{
			MegaFFD ffd = (MegaFFD)target;

			bool snapshot = false;

			if ( ffd.DisplayGizmo && ffd.ModEnabled )
			{
				MegaModifyObject context = ffd.GetComponent<MegaModifyObject>();

				if ( context && context.DrawGizmos )
				{
					Vector3 size = ffd.lsize;
					Vector3 osize = ffd.lsize;
					osize.x = 1.0f / size.x;
					osize.y = 1.0f / size.y;
					osize.z = 1.0f / size.z;

					Matrix4x4 tm1 = Matrix4x4.identity;
					Quaternion rot = Quaternion.Euler(ffd.gizmoRot);
					tm1.SetTRS(-(ffd.gizmoPos + ffd.Offset), rot, ffd.gizmoScale);

					Matrix4x4 tm = Matrix4x4.identity;
					Handles.matrix = Matrix4x4.identity;

					if ( context != null && context.sourceObj != null )
						tm = context.sourceObj.transform.localToWorldMatrix * tm1;
					else
						tm = ffd.transform.localToWorldMatrix * tm1;

					DrawGizmos2D(ffd, tm);

					Handles.color = Color.yellow;
					int gs = ffd.GridSize();

					Vector3 ttp = Vector3.zero;

					for ( int z = 0; z < 1; z++ )
					{
						for ( int y = 0; y < gs; y++ )
						{
							for ( int x = 0; x < gs; x++ )
							{
								int index = ffd.GridIndex(x, y, z);

								Vector3 lp = ffd.GetPoint(index);
								Vector3 p = lp;

								Vector3 tp = tm.MultiplyPoint(p);
								if ( handles )
									ttp = PositionHandle2D(target, tp, Quaternion.identity, handleSize, ffd.gizCol1.a, index + 2000);
								else
									ttp = CircleCap(0, tp, Quaternion.identity, ffd.KnotSize * 0.1f);

								if ( ttp != tp )
								{
									if ( !snapshot )
										snapshot = true;
								}

								pm = tm.inverse.MultiplyPoint(ttp);
								Vector3 delta = pm - p;

								pm -= ffd.bcenter;
								p = Vector3.Scale(pm, osize);
								p.x += 0.5f;
								p.y += 0.5f;
								p.z = 0.0f;

								ffd.pt[index] = p;

								if ( mirrorX )
								{
									int y1 = y - (gs - 1);
									if ( y1 < 0 )
										y1 = -y1;

									if ( y != y1 )
									{
										Vector3 p1 = ffd.GetPoint(ffd.GridIndex(x, y1, z));

										delta.y = -delta.y;
										p1 += delta;
										p1 -= ffd.bcenter;
										p = Vector3.Scale(p1, osize);
										p.x += 0.5f;
										p.y += 0.5f;
										p.z = 0.0f;	//+= 0.5f;

										ffd.pt[ffd.GridIndex(x, y1, z)] = p;
									}
								}

								if ( mirrorY )
								{
									int z1 = z - (gs - 1);
									if ( z1 < 0 )
										z1 = -z1;

									if ( z != z1 )
									{
										Vector3 p1 = ffd.GetPoint(ffd.GridIndex(x, y, z1));

										delta.z = -delta.z;
										p1 += delta;
										p1 -= ffd.bcenter;
										p = Vector3.Scale(p1, osize);
										p.x += 0.5f;
										p.y += 0.5f;
										p.z = 0.0f;

										ffd.pt[ffd.GridIndex(x, y, z1)] = p;
									}
								}
#if false
								if ( mirrorZ )
								{
									int x1 = x - (gs - 1);
									if ( x1 < 0 )
										x1 = -x1;

									if ( x != x1 )
									{
										Vector3 p1 = ffd.GetPoint(ffd.GridIndex(x1, y, z));

										delta.x = -delta.x;
										p1 += delta;
										p1 -= ffd.bcenter;
										p = Vector3.Scale(p1, osize);
										p.x += 0.5f;
										p.y += 0.5f;
										p.z += 0.5f;

										ffd.pt[ffd.GridIndex(x1, y, z)] = p;
									}
								}
#endif
							}
						}
					}

					Handles.matrix = Matrix4x4.identity;

					if ( GUI.changed && snapshot )
						EditorUtility.SetDirty(target);
				}
			}
		}

		public override string GetHelpString() { return "FFD2x2 Modifier by Chris West"; }

		public void DrawGizmos2D(MegaFFD ffd, Matrix4x4 tm)
		{
			Handles.color = ffd.gizCol1;

			int pc = ffd.GridSize();

			for ( int i = 0; i < pc; i++ )
			{
				for ( int j = 0; j < pc; j++ )
				{
					for ( int k = 0; k < 1; k++ )
					{
						pp3[0] = tm.MultiplyPoint(ffd.GetPoint(i, j, k));

						if ( i < pc - 1 )
						{
							pp3[1] = tm.MultiplyPoint(ffd.GetPoint(i + 1, j, k));
							Handles.DrawLine(pp3[0], pp3[1]);
						}

						if ( j < pc - 1 )
						{
							pp3[1] = tm.MultiplyPoint(ffd.GetPoint(i, j + 1, k));
							Handles.DrawLine(pp3[0], pp3[1]);
						}

						if ( k < pc - 1 )
						{
							pp3[1] = tm.MultiplyPoint(ffd.GetPoint(i, j, k + 1));
							Handles.DrawLine(pp3[0], pp3[1]);
						}
					}
				}
			}
		}
	}
}