using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Collections;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaWrap))]
	public class MegaWrapEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			MegaWrap mod = (MegaWrap)target;

			if ( mod.target )
			{
				SkinnedMeshRenderer sr = mod.target.GetComponent<SkinnedMeshRenderer>();
				if ( sr )
				{
					if ( mod.GetComponent<SkinnedMeshRenderer>() )
					{
						EditorGUILayout.BeginVertical("box");
						EditorGUILayout.HelpBox("Both Target and Wrapping Mesh are Skinned, this is not supported.\nPress Convert to change this Object to Non Skinned.\n", MessageType.Warning);

						if ( GUILayout.Button("Convert") )
						{
							SkinnedMeshRenderer srend = mod.GetComponent<SkinnedMeshRenderer>();
							Mesh mesh = srend.sharedMesh;
							Material[] mats = srend.sharedMaterials;

							DestroyImmediate(srend);
							MeshFilter mf = mod.gameObject.AddComponent<MeshFilter>();
							mf.sharedMesh = mesh;
							MeshRenderer mr = mod.gameObject.AddComponent<MeshRenderer>();
							mr.sharedMaterials = mats;
						}
						EditorGUILayout.EndVertical();
						return;
					}
				}
			}
			MegaEditorGUILayout.Toggle(target, "Enabled", ref mod.WrapEnabled);

			mod.target = (MegaModifyObject)EditorGUILayout.ObjectField("Target", mod.target, typeof(MegaModifyObject), true);

			if ( mod.target.mods == null || mod.target.mods.Length == 0 )
			{
				EditorGUILayout.HelpBox("Wrap system requires a Mesh with at least one modifier applied. Please add a Modifier to the target, you can always disable it", MessageType.Info);
			}
			else
			{
				if ( mod.target )
				{
					float max = 1.0f;
					if ( mod.target )
						max = mod.target.bbox.size.magnitude;

					MegaEditorGUILayout.IntSlider(target, "Voxel Resolution", ref mod.voxelRes, 1, 64);

					MegaEditorGUILayout.Toggle(target, "Use Max Dist", ref mod.useMaxDist);
					MegaEditorGUILayout.Slider(target, "Max Dist", ref mod.maxdist, 0.0f, max);

					MegaEditorGUILayout.Int(target, "Max Points", ref mod.maxpoints);
					if ( mod.maxpoints < 1 )
						mod.maxpoints = 1;

					Color col = GUI.backgroundColor;
					EditorGUILayout.BeginHorizontal();
					if ( mod.weight == null || mod.weight.Length == 0 )
					{
						GUI.backgroundColor = Color.red;
						if ( GUILayout.Button("Map") )
						{
							Undo.RecordObject(target, "Attach");
							Attach(mod.target);
							mod.forceUpdate = true;
						}
					}
					else
					{
						GUI.backgroundColor = Color.green;
						if ( GUILayout.Button("ReMap") )
						{
							Undo.RecordObject(target, "Remap");
							Attach(mod.target);
							mod.forceUpdate = true;
						}
					}

					GUI.backgroundColor = col;
					if ( GUILayout.Button("Reset") )
					{
						Undo.RecordObject(target, "Reset");
						mod.ResetMesh();
					}

					EditorGUILayout.EndHorizontal();

					if ( GUI.changed )
						EditorUtility.SetDirty(mod);

					MegaEditorGUILayout.Float(target, "Gap", ref mod.gap);
					MegaEditorGUILayout.Slider(target, "Shrink", ref mod.shrink, 0.0f, 1.0f);
					MegaEditorGUILayout.Slider(target, "Size", ref mod.size, 0.001f, 0.04f);
					if ( mod.weight != null && mod.weight.Length > 0 )
						MegaEditorGUILayout.Int(target, "Vert Index", ref mod.vertindex, 0, mod.weight.Length - 1);
					MegaEditorGUILayout.Vector3(target, "Offset", ref mod.offset);

					MegaEditorGUILayout.NormalMethod(target, "Normal Method", ref mod.NormalMethod);
					MegaEditorGUILayout.Toggle(target, "Use Baked Mesh", ref mod.UseBakedMesh);
					MegaEditorGUILayout.Toggle(target, "Recalc Bounds", ref mod.recalcBounds);
					MegaEditorGUILayout.Toggle(target, "Local Space", ref mod.localSpace);
					MegaEditorGUILayout.Toggle(target, "Auto Disable", ref mod.autoDisable);

					if ( mod.weight == null || mod.weight.Length == 0 || mod.target == null )
						EditorGUILayout.LabelField("Object not wrapped");
					else
						EditorGUILayout.LabelField("UnMapped", mod.nomapcount.ToString());

					if ( GUILayout.Button("Attach Children") )
						mod.AttachChildren();

					if ( GUILayout.Button("Detach Children") )
						mod.DetachChildren();
				}
			}

			if ( GUI.changed )
			{
				mod.forceUpdate = true;
				EditorUtility.SetDirty(mod);
			}
		}

		public void OnSceneGUI()
		{
			DisplayDebug();
		}

		void DisplayDebug()
		{
			MegaWrap mod = (MegaWrap)target;
			if ( mod.target )
			{
				if ( mod.weight != null && mod.weight.Length > 0 )
				{
					if ( mod.targetIsSkin && !mod.sourceIsSkin )
					{
						if ( mod.skinnedVerts.IsCreated )
						{
							Color col = Color.black;
							Handles.matrix = Matrix4x4.identity;

							if ( mod.UseBakedMesh )
								Handles.matrix = mod.target.transform.localToWorldMatrix;

							int c = mod.indexes[mod.vertindex * 2];
							if ( c > 0 )
							{
								int ix = mod.indexes[(mod.vertindex * 2) + 1];
								int ixx = ix * 3;

								for ( int i = 0; i < c; i++ )
								{
									float w = mod.weights[ix + i] / mod.weight[mod.vertindex];

									if ( w > 0.5f )
										col = Color.Lerp(Color.green, Color.red, (w - 0.5f) * 2.0f);
									else
										col = Color.Lerp(Color.blue, Color.green, w * 2.0f);
									Handles.color = col;

									int i0 = mod.ixs[ixx + (i * 3)];
									int i1 = mod.ixs[ixx + (i * 3) + 1];
									int i2 = mod.ixs[ixx + (i * 3) + 2];

									Vector3 p = (mod.skinnedVerts[i0] + mod.skinnedVerts[i1] + mod.skinnedVerts[i2]) / 3.0f;
									MegaHandles.DotCap(i, p, Quaternion.identity, mod.size);

									Vector3 p0 = mod.skinnedVerts[i0];
									Vector3 p1 = mod.skinnedVerts[i1];
									Vector3 p2 = mod.skinnedVerts[i2];

									Vector3 cp = MegaPlane.GetCoordMine(p0, p1, p2, mod.barys[ix + i]);
									Handles.color = Color.gray;
									Handles.DrawLine(p, cp);

									Vector3 norm = MegaPlane.FaceNormal(p0, p1, p2);
									Vector3 cp1 = cp + (((mod.dist[ix + i] * mod.shrink) + mod.gap) * norm.normalized);
									Handles.color = Color.green;
									Handles.DrawLine(cp, cp1);
								}
							}
						}
					}
					else
					{
						if ( mod.target.jsverts.IsCreated )
						{
							Color col = Color.black;
							Matrix4x4 tm = mod.target.transform.localToWorldMatrix;
							Handles.matrix = tm;

							int c = mod.indexes[mod.vertindex * 2];
							if ( c > 0 )
							{
								int ix = mod.indexes[(mod.vertindex * 2) + 1];
								int ixx = ix * 3;

								for ( int i = 0; i < c; i++ )
								{
									float w = mod.weights[ix + i] / mod.weight[mod.vertindex];

									if ( w > 0.5f )
										col = Color.Lerp(Color.green, Color.red, (w - 0.5f) * 2.0f);
									else
										col = Color.Lerp(Color.blue, Color.green, w * 2.0f);
									Handles.color = col;

									int i0 = mod.ixs[ixx + (i * 3)];
									int i1 = mod.ixs[ixx + (i * 3) + 1];
									int i2 = mod.ixs[ixx + (i * 3) + 2];

									Vector3 p = (mod.target.jsverts[i0] + mod.target.jsverts[i1] + mod.target.jsverts[i2]) / 3.0f;
									MegaHandles.DotCap(i, p, Quaternion.identity, mod.size);

									Vector3 p0 = mod.target.jsverts[i0];
									Vector3 p1 = mod.target.jsverts[i1];
									Vector3 p2 = mod.target.jsverts[i2];

									Vector3 cp = MegaPlane.GetCoordMine(p0, p1, p2, mod.barys[ix + i]);
									Handles.color = Color.gray;
									Handles.DrawLine(p, cp);

									Vector3 norm = MegaPlane.FaceNormal(p0, p1, p2);
									Vector3 cp1 = cp + (((mod.dist[ix + i] * mod.shrink) + mod.gap) * norm.normalized);
									Handles.color = Color.green;
									Handles.DrawLine(cp, cp1);
								}
							}
						}
					}

					Handles.color = Color.yellow;
					for ( int i = 0; i < mod.weight.Length; i++ )
					{
						if ( mod.weight[i] == 0.0f )
						{
							Vector3 pv1 = mod.freeverts[i];
							MegaHandles.DotCap(0, pv1, Quaternion.identity, mod.size);
						}
					}
				}

				if ( mod.verts != null && mod.verts.Length > mod.vertindex )
				{
					Handles.color = Color.red;
					Handles.matrix = mod.transform.localToWorldMatrix;
					Vector3 pv = mod.verts[mod.vertindex];
					MegaHandles.DotCap(0, pv, Quaternion.identity, mod.size);
				}
			}
		}

		void Attach(MegaModifyObject modobj)
		{
			MegaWrap mod = (MegaWrap)target;

			mod.targetIsSkin = false;
			mod.sourceIsSkin = false;

			if ( mod.mesh && mod.startverts != null )
				mod.mesh.vertices = mod.startverts;

			mod.DisposeArrays();
			if ( modobj == null )
				return;

			mod.nomapcount = 0;

			if ( mod.mesh )
				mod.mesh.vertices = mod.startverts;

			MeshFilter mf = mod.GetComponent<MeshFilter>();
			Mesh srcmesh = null;

			if ( mf != null )
				srcmesh = mf.sharedMesh;
			else
			{
				SkinnedMeshRenderer smesh = (SkinnedMeshRenderer)mod.GetComponent(typeof(SkinnedMeshRenderer));

				if ( smesh != null )
				{
					srcmesh = smesh.sharedMesh;
					mod.sourceIsSkin = true;
				}
			}

			if ( srcmesh == null )
			{
				Debug.LogWarning("No Mesh found on the target object, make sure target has a mesh and MegaFiers modifier attached!");
				return;
			}

			if ( mod.mesh == null )
				mod.mesh = MegaUtils.DupMesh(srcmesh, "");

			if ( mf )
				mf.mesh = mod.mesh;
			else
			{
				SkinnedMeshRenderer smesh = (SkinnedMeshRenderer)mod.GetComponent(typeof(SkinnedMeshRenderer));
				smesh.sharedMesh = mod.mesh;
			}

			if ( mod.sourceIsSkin == false )
			{
				mod.tmesh = (SkinnedMeshRenderer)modobj.GetComponent(typeof(SkinnedMeshRenderer));
				if ( mod.tmesh != null )
				{
					mod.targetIsSkin = true;

					if ( !mod.sourceIsSkin )
					{
						Mesh sm = mod.tmesh.sharedMesh;
						mod.bindposes = sm.bindposes;
						mod.boneweights = sm.boneWeights;
						mod.bones = mod.tmesh.bones;
						//mod.skinnedVerts = sm.vertices;
						mod.targetVertexCount = sm.vertexCount;

						mod.blendShapeCount = sm.blendShapeCount;
						mod.lastWeights = new float[sm.blendShapeCount];
					}
				}
			}
			else
			{
				mod.tmesh = (SkinnedMeshRenderer)modobj.GetComponent(typeof(SkinnedMeshRenderer));
				if ( mod.tmesh != null )
				{
					mod.targetIsSkin = true;

					if ( !mod.sourceIsSkin )
					{
						Mesh sm = mod.tmesh.sharedMesh;
						mod.bindposes = sm.bindposes;
						mod.boneweights = sm.boneWeights;
						mod.bones = mod.tmesh.bones;
						//mod.skinnedVerts = sm.vertices;
						mod.targetVertexCount = sm.vertexCount;

						mod.blendShapeCount = sm.blendShapeCount;
						mod.lastWeights = new float[sm.blendShapeCount];
					}
				}
			}

			if ( mod.targetIsSkin )
			{
				//if ( mod.boneweights == null || mod.boneweights.Length == 0 )
					//mod.targetIsSkin = false;
			}

			mod.GetTargetVerts();
			mod.neededVerts.Clear();

			mod.verts = new NativeArray<Vector3>(mod.mesh.vertices, Allocator.Persistent);
			mod.freeverts = new Vector3[mod.verts.Length];
			NativeArray<Vector3> baseverts = modobj.startverts; // jverts
			int[] basefaces = modobj.Tris;

			mod.weight = new float[mod.verts.Length];
			mod.indexes = new int[mod.verts.Length * 2];

			Matrix4x4 tm = Matrix4x4.identity;

			if ( !mod.localSpace )
				tm = mod.transform.localToWorldMatrix * modobj.transform.worldToLocalMatrix;

			List<MegaCloseFace> closefaces = new List<MegaCloseFace>();

			Vector3 p0 = Vector3.zero;
			Vector3 p1 = Vector3.zero;
			Vector3 p2 = Vector3.zero;

			Vector3[] tverts = new Vector3[mod.target.startverts.Length];   // not sure these are right

			for ( int i = 0; i < tverts.Length; i++ )
			{
				if ( mod.targetIsSkin && !mod.sourceIsSkin )
					tverts[i] = baseverts[i];   //modobj.transform.InverseTransformPoint(mod.GetSkinPos(i));
				else
					tverts[i] = baseverts[i];
			}

			float unit = 0.0f;
			mod.target.mesh.RecalculateBounds();
			MegaVoxel.Voxelize(tverts, basefaces, mod.target.mesh.bounds, mod.voxelRes, out unit);

			List<Vector3> barys = new List<Vector3>();
			List<float> weights = new List<float>();
			List<float> dist = new List<float>();
			List<int> ix = new List<int>();

			int minx = 0;
			int miny = 0;
			int minz = 0;

			int maxx = 0;
			int maxy = 0;
			int maxz = 0;

			Vector3 p = Vector3.zero;

			int gridx = MegaVoxel.volume.GetLength(0) - 1;
			int gridy = MegaVoxel.volume.GetLength(1) - 1;
			int gridz = MegaVoxel.volume.GetLength(2) - 1;

			for ( int i = 0; i < mod.verts.Length; i++ )
			{
				p = mod.verts[i];
				if ( !mod.localSpace )
				{
					p = mod.transform.TransformPoint(p);
					p = modobj.transform.InverseTransformPoint(p);
				}

				mod.freeverts[i] = p;

				closefaces.Clear();

				MegaVoxel.GetGridIndex(p - new Vector3(mod.maxdist, mod.maxdist, mod.maxdist), out minx, out miny, out minz, unit);
				MegaVoxel.GetGridIndex(p + new Vector3(mod.maxdist, mod.maxdist, mod.maxdist), out maxx, out maxy, out maxz, unit);

				minx = Mathf.Clamp(minx, 0, gridx);
				miny = Mathf.Clamp(miny, 0, gridy);
				minz = Mathf.Clamp(minz, 0, gridz);

				maxx = Mathf.Clamp(maxx, 0, gridx);
				maxy = Mathf.Clamp(maxy, 0, gridy);
				maxz = Mathf.Clamp(maxz, 0, gridz);

				for ( int x = minx; x <= maxx; x++ )
				{
					for ( int y = miny; y <= maxy; y++ )
					{
						for ( int z = minz; z <= maxz; z++ )
						{
							MegaVoxel.Voxel_t voxel = MegaVoxel.volume[x, y, z];

							List<MegaTriangle> tris = voxel.tris;

							for ( int t = 0; t < tris.Count; t++ )
							{
								float dist1 = MegaPlane.GetDistance(p, tris[t].a, tris[t].b, tris[t].c);

								if ( !mod.useMaxDist || Mathf.Abs(dist1) < mod.maxdist )
								{
									MegaCloseFace cf = new MegaCloseFace();
									cf.dist = Mathf.Abs(dist1);
									cf.face = tris[t].t;

									bool inserted = false;
									for ( int k = 0; k < closefaces.Count; k++ )
									{
										if ( cf.dist < closefaces[k].dist )
										{
											closefaces.Insert(k, cf);
											inserted = true;
											break;
										}
									}

									if ( !inserted )
										closefaces.Add(cf);
								}
							}
						}
					}
				}

				float tweight = 0.0f;
				int maxp = mod.maxpoints;
				if ( maxp == 0 )
					maxp = closefaces.Count;

				int index = weights.Count;
				int faceindex = ix.Count;

				for ( int j = 0; j < maxp; j++ )
				{
					if ( j < closefaces.Count )
					{
						int t = closefaces[j].face;

						p0 = tverts[basefaces[t]];
						p1 = tverts[basefaces[t + 1]];
						p2 = tverts[basefaces[t + 2]];

						Vector3 normal = MegaPlane.FaceNormal(p0, p1, p2);

						float dist1 = closefaces[j].dist;

						float dst = MegaPlane.GetPlaneDistance(p, p0, p1, p2);
						float weight = 1.0f / (1.0f + dist1);
						dist.Add(dst);
						weights.Add(weight);
						barys.Add(MegaPlane.CalcBary(p, p0, p1, p2));

						ix.Add(basefaces[t]);
						ix.Add(basefaces[t + 1]);
						ix.Add(basefaces[t + 2]);

						tweight += weight;
					}
				}

				if ( !mod.sourceIsSkin && mod.targetIsSkin )
				{
					for ( int fi = faceindex; fi < ix.Count; fi++ )
					{
						if ( !mod.neededVerts.Contains(ix[fi]) )
							mod.neededVerts.Add(ix[fi]);    // this is so we dont have to skin the whole mesh
					}
				}

				if ( tweight == 0.0f )
					mod.nomapcount++;

				mod.weight[i] = tweight;
				mod.indexes[(i * 2)] = maxp;
				mod.indexes[(i * 2) + 1] = index;
			}

			mod.barys = barys.ToArray();
			mod.weights = weights.ToArray();
			mod.dist = dist.ToArray();
			mod.ixs = ix.ToArray();
		}
	}
}