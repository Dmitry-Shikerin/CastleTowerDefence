using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MegaFiers
{
	public class MegaFFDEditor : MegaModifierEditor
	{
		protected Vector3	pm			= new Vector3();
		protected bool		showpoints	= false;
		protected Vector3[]	pp3			= new Vector3[3];
		static public float	handleSize	= 0.5f;
		static public bool	handles		= true;
		static public bool	mirrorX		= false;
		static public bool	mirrorY		= false;
		static public bool	mirrorZ		= false;

		public override bool Inspector()
		{
			MegaFFD mod = (MegaFFD)target;

			MegaEditorGUILayout.Float(target, "Knot Size", ref mod.KnotSize);
			MegaEditorGUILayout.Toggle(target, "In Vol", ref mod.inVol);
			MegaEditorGUILayout.Toggle(target, "Handles", ref handles);
			mod.showIndex = (MegaFFD.ShowIndex)EditorGUILayout.EnumPopup("Show Index", mod.showIndex);
			MegaEditorGUILayout.Slider(target, "Size", ref handleSize, 0.0f, 1.0f);
			MegaEditorGUILayout.Toggle(target, "Mirror X", ref mirrorX);
			MegaEditorGUILayout.Toggle(target, "Mirror Y", ref mirrorY);
			MegaEditorGUILayout.Toggle(target, "Mirror Z", ref mirrorZ);

			showpoints = EditorGUILayout.Foldout(showpoints, "Points");

			if ( showpoints )
			{
				int gs = mod.GridSize();

				for ( int x = 0; x < gs; x++ )
				{
					for ( int y = 0; y < gs; y++ )
					{
						for ( int z = 0; z < gs; z++ )
						{
							int i = (x * gs * gs) + (y * gs) + z;
							MegaEditorGUILayout.Vector3(target, "p[" + x + "," + y + "," + z + "]", ref mod.pt[i]);
						}
					}
				}
			}
			return false;
		}

		Vector3 CircleCap(int id, Vector3 pos, Quaternion rot, float size)
		{
			return MegaEditorGUILayout.FreeHandle(target, pos, rot, size, Vector3.zero, Handles.CircleHandleCap, id);
		}

		Vector3 SphereCap(int id, Vector3 pos, Quaternion rot, float size)
		{
			return MegaEditorGUILayout.FreeHandle(target, pos, rot, size, Vector3.zero, Handles.SphereHandleCap, id);
		}

		List<int> dragids = new List<int>();

		public override void DrawSceneGUI()
		{
			MegaFFD ffd = (MegaFFD)target;

			switch ( Event.current.type )
			{
				case EventType.MouseDown:
					break;

				case EventType.MouseUp:
					int id = GUIUtility.hotControl;
					if ( id >= 2000 && id < 5064 )
					{
						id = id % 1000;

						if ( id < 64 )
						{
							if ( Event.current.control )
							{
								if ( !dragids.Contains(id) )
									dragids.Add(id);
								else
									dragids.Remove(id);
							}
							else
							{
								if ( dragids.Contains(id) )
								{
								}
								else
								{
									dragids.Clear();
									dragids.Add(id);
								}
							}
						}
					}
					break;
			}

			bool snapshot = false;

			if ( ffd && ffd.DisplayGizmo && ffd.ModEnabled )
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

					DrawGizmos(ffd, tm);

					Handles.color = Color.yellow;
					int gs = ffd.GridSize();

					Vector3 ttp = Vector3.zero;

					bool showall = false;

					for ( int z = 0; z < gs; z++ )
					{
						for ( int y = 0; y < gs; y++ )
						{
							for ( int x = 0; x < gs; x++ )
							{
								bool changes = false;

								int index = ffd.GridIndex(x, y, z);

								if ( dragids.Contains(index) )
								{
									showall = true;
								}
								else
									showall = false;

								Vector3 lp = ffd.GetPoint(index);
								Vector3 p = lp;

								Vector3 tp = tm.MultiplyPoint(p);

								Handles.color = Color.green;
								if ( dragids.Contains(index) )
								{
									Handles.SphereHandleCap(0, tp, Quaternion.identity, ffd.KnotSize * 0.1f, EventType.Repaint);
								}

								if ( showall )
								{
									if ( handles )
										ttp = PositionHandle(target, tp, Quaternion.identity, handleSize, ffd.gizCol1.a, index + 2000);
									else
										ttp = CircleCap(index + 2000, tp, Quaternion.identity, ffd.KnotSize * 0.1f);
								}
								else
								{
									Handles.color = ffd.gizCol2;
									//Handles.SphereHandleCap(index + 2000, ttp, Quaternion.identity, ffd.KnotSize * 0.1f, EventType.Repaint);
									ttp = SphereCap(index + 2000, tp, Quaternion.identity, ffd.KnotSize * 0.1f);
								}

								if ( ttp != tp )
								{
									changes = true;
									if ( !snapshot )
										snapshot = true;
								}

								if ( ffd.showIndex == MegaFFD.ShowIndex.Index )
									Handles.Label(tp, index.ToString());
								else
								{
									if ( ffd.showIndex == MegaFFD.ShowIndex.XYZ )
										Handles.Label(tp, "[" + x + " " + y + " " + z + "]");
								}

								if ( changes )
								{
									pm = tm.inverse.MultiplyPoint(ttp);
									Vector3 delta = pm - p;
									Vector3 deltasave = delta;

									pm -= ffd.bcenter;
									p = Vector3.Scale(pm, osize);
									p.x += 0.5f;
									p.y += 0.5f;
									p.z += 0.5f;

									Vector3 grpdelta = p - ffd.pt[index];

									ffd.pt[index] = p;

									DoMirror(ffd, x, y, z, delta, osize);

									for ( int i = 0; i < dragids.Count; i++ )
									{
										int did = dragids[i];

										if ( did != index )
										{
											ffd.pt[did] += grpdelta;

											int x1 = 0;
											int y1 = 0;
											int z1 = 0;
											ffd.GridXYZ(did, out x1, out y1, out z1);

											DoMirror(ffd, x1, y1, z1, delta, osize);
										}
									}
								}
							}
						}
					}

					MegaFFDAnimate ffdanim = ffd.GetComponent<MegaFFDAnimate>();
					if ( ffdanim && snapshot )
					{
						ffdanim.GetPoints();
						Undo.RecordObject(ffdanim, "Lattice Changed");

						EditorUtility.SetDirty(ffdanim);
					}

					Handles.matrix = Matrix4x4.identity;

					if ( GUI.changed && snapshot )
						EditorUtility.SetDirty(target);
				}
			}
		}

		public void DoMirror(MegaFFD ffd, int x, int y, int z, Vector3 delta, Vector3 osize)
		{
			int gs = ffd.GridSize();

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
					Vector3 p = Vector3.Scale(p1, osize);
					p.x += 0.5f;
					p.y += 0.5f;
					p.z += 0.5f;

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
					Vector3 p = Vector3.Scale(p1, osize);
					p.x += 0.5f;
					p.y += 0.5f;
					p.z += 0.5f;

					ffd.pt[ffd.GridIndex(x, y, z1)] = p;
				}
			}

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
					Vector3 p = Vector3.Scale(p1, osize);
					p.x += 0.5f;
					p.y += 0.5f;
					p.z += 0.5f;

					ffd.pt[ffd.GridIndex(x1, y, z)] = p;
				}
			}
		}

		public static Vector3 PositionHandle(Object target, Vector3 position, Quaternion rotation, float size, float alpha, int id = 20)
		{
			float handlesize = HandleUtility.GetHandleSize(position) * size;
			Color color = Handles.color;
			Color col = Color.red;
			col.a = alpha;
			Handles.color = col;
			position = MegaEditorGUILayout.SliderHandle(target, position, rotation * Vector3.right, handlesize, Handles.ArrowHandleCap, 0.0f, id);
			col = Color.green;
			col.a = alpha;
			Handles.color = col;
			position = MegaEditorGUILayout.SliderHandle(target, position, rotation * Vector3.up, handlesize, Handles.ArrowHandleCap, 0.0f, id + 1000);
			col = Color.blue;
			col.a = alpha;
			Handles.color = col;
			position = MegaEditorGUILayout.SliderHandle(target, position, rotation * Vector3.forward, handlesize, Handles.ArrowHandleCap, 0.0f, id + 2000);
			col = Color.yellow;
			col.a = alpha;
			Handles.color = col;
			position = MegaEditorGUILayout.FreeHandle(target, position, rotation, handlesize * 0.15f, Vector3.zero, Handles.RectangleHandleCap, id + 3000);
			Handles.color = color;
			return position;
		}

		public static Vector3 PositionHandle2D(Object target, Vector3 position, Quaternion rotation, float size, float alpha, int id = 20)
		{
			float handlesize = HandleUtility.GetHandleSize(position) * size;
			Color color = Handles.color;
			Color col = Color.red;
			col.a = alpha;
			Handles.color = col;
			position = MegaEditorGUILayout.SliderHandle(target, position, rotation * Vector3.right, handlesize, Handles.ArrowHandleCap, 0.0f, id);
			col = Color.green;
			col.a = alpha;
			Handles.color = col;
			position = MegaEditorGUILayout.SliderHandle(target, position, rotation * Vector3.up, handlesize, Handles.ArrowHandleCap, 0.0f, id + 1000);
			col = Color.yellow;
			col.a = alpha;
			Handles.color = col;
			position = MegaEditorGUILayout.FreeHandle(target, position, rotation, handlesize * 0.15f, Vector3.zero, Handles.RectangleHandleCap, id + 3000);
			position.z = 0.0f;
			Handles.color = color;
			return position;
		}

		public void DrawGizmos(MegaFFD ffd, Matrix4x4 tm)
		{
			Handles.color = ffd.gizCol1;

			int pc = ffd.GridSize();

			for ( int  i = 0; i < pc; i++ )
			{
				for ( int j = 0; j < pc; j++ )
				{
					for ( int k = 0; k < pc; k++ )
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