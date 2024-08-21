using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	public class MegaFFDWarpEditor : MegaWarpEditor
	{
		Vector3				pm			= new Vector3();
		bool				showpoints	= false;
		Vector3[]			pp3			= new Vector3[3];
		static public float	handleSize	= 0.5f;
		static public bool	handles		= true;
		static public bool	mirrorX		= false;
		static public bool	mirrorY		= false;
		static public bool	mirrorZ		= false;

		public override string GetHelpString() { return "Bend Warp Modifier by Chris West"; }

		public static void CreateFFDWarp(string type, System.Type classtype)
		{
			Vector3 pos = Vector3.zero;

			if ( UnityEditor.SceneView.lastActiveSceneView != null )
				pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

			GameObject go = new GameObject(type + " Warp");

			MegaFFDWarp warp = (MegaFFDWarp)go.AddComponent(classtype);
			warp.Init();

			go.transform.position = pos;
			Selection.activeObject = go;
		}

		public override bool Inspector()
		{
			MegaFFDWarp mod = (MegaFFDWarp)target;

			MegaEditorGUILayout.Float(target, "Knot Size", ref mod.KnotSize);
			MegaEditorGUILayout.Toggle(target, "In Vol", ref mod.inVol);
			MegaEditorGUILayout.Toggle(target, "Handles", ref handles);
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

			if ( mod.bsize.x != mod.Width || mod.bsize.y != mod.Height || mod.bsize.z != mod.Length )
				mod.Init();

			return false;
		}

		Vector3 CircleCap(int id, Vector3 pos, Quaternion rot, float size)
		{
			return MegaEditorGUILayout.FreeHandle(target, pos, rot, size, Vector3.zero, Handles.CircleHandleCap);
		}

		public override void DrawSceneGUI()
		{
			MegaFFDWarp ffd = (MegaFFDWarp)target;

			bool snapshot = false;

			if ( ffd.DisplayGizmo )
			{
				{
					Vector3 size = ffd.lsize;
					Vector3 osize = ffd.lsize;
					osize.x = 1.0f / size.x;
					osize.y = 1.0f / size.y;
					osize.z = 1.0f / size.z;

					Matrix4x4 tm = Matrix4x4.identity;
					Handles.matrix = Matrix4x4.identity;

					tm = ffd.transform.localToWorldMatrix;

					DrawGizmos(ffd, tm);

					Handles.color = Color.yellow;
					int gs = ffd.GridSize();

					Vector3 ttp = Vector3.zero;

					for ( int z = 0; z < gs; z++ )
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
									ttp = PositionHandle(target, tp, Quaternion.identity, handleSize, ffd.GizCol1.a);
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
								p.z += 0.5f;

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
										p = Vector3.Scale(p1, osize);
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
										p = Vector3.Scale(p1, osize);
										p.x += 0.5f;
										p.y += 0.5f;
										p.z += 0.5f;

										ffd.pt[ffd.GridIndex(x1, y, z)] = p;
									}
								}
							}
						}
					}

					Handles.matrix = Matrix4x4.identity;

					if ( GUI.changed && snapshot )
						EditorUtility.SetDirty(ffd);
				}
			}
		}

		public static Vector3 PositionHandle(Object target, Vector3 position, Quaternion rotation, float size, float alpha)
		{
			float handlesize = HandleUtility.GetHandleSize(position) * size;
			Color color = Handles.color;
			Color col = Color.red;
			col.a = alpha;
			Handles.color = col;
			position = MegaEditorGUILayout.SliderHandle(target, position, rotation * Vector3.right, handlesize, Handles.ArrowHandleCap, 0.0f);
			col = Color.green;
			col.a = alpha;
			Handles.color = col;
			position = MegaEditorGUILayout.SliderHandle(target, position, rotation * Vector3.up, handlesize, Handles.ArrowHandleCap, 0.0f);
			col = Color.blue;
			col.a = alpha;
			Handles.color = col;
			position = MegaEditorGUILayout.SliderHandle(target, position, rotation * Vector3.forward, handlesize, Handles.ArrowHandleCap, 0.0f);
			col = Color.yellow;
			col.a = alpha;
			Handles.color = col;
			position = MegaEditorGUILayout.FreeHandle(target, position, rotation, handlesize * 0.15f, Vector3.zero, Handles.RectangleHandleCap);
			Handles.color = color;
			return position;
		}

		public void DrawGizmos(MegaFFDWarp ffd, Matrix4x4 tm)
		{
			Handles.color = Color.yellow;

			int pc = ffd.GridSize();

			for ( int i = 0; i < pc; i++ )
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