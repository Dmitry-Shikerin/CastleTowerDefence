using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaModifyObject))]
	public class MegaModifyObjectEditor : Editor
	{
		string[]		modifierNames;
		static GUIStyle	foldoutStyle;

		string[]		blendNames;

		public static GUIStyle FoldoutStyle
		{
			get
			{
				if ( foldoutStyle == null )
				{
					foldoutStyle = new GUIStyle(EditorStyles.foldout);
					//foldoutStyle.font = new GUIStyle("Label").font;
					foldoutStyle.fontSize = 14;
					//foldoutStyle.border = new RectOffset(15, 7, 4, 4);
					//foldoutStyle.fixedHeight = 22;
					//foldoutStyle.contentOffset = new Vector2(20.0f, -2.0f);
				}

				return foldoutStyle;
			}
		}

		System.Type GetComponentType(string name)
		{
			string n = name.Replace(" ", "");

			string tname = "MegaFiers" + ".Mega" + n + "," + "Assembly-CSharp";

			System.Type rval = System.Type.GetType(tname, false, true);

			if ( rval != null )
				return rval;

			return null;
		}

		GUIStyle	dropdownStyle;

		void GetBlendNames()
		{
			MegaModifyObject mod = (MegaModifyObject)target;

			if ( mod.mesh )
			{
				if ( mod.mesh.blendShapeCount > 0 )
				{
					blendNames = new string[mod.mesh.blendShapeCount];
						
					for ( int i = 0; i < mod.mesh.blendShapeCount; i++ )
						blendNames[i] = mod.mesh.GetBlendShapeName(i);
				}
				else
				{
					blendNames = null;
				}
			}
		}

		public override void OnInspectorGUI()
		{
			MegaModifyObject mod = (MegaModifyObject)target;

			if ( dropdownStyle == null )
			{
				dropdownStyle = new GUIStyle(EditorStyles.popup);
				dropdownStyle.fontSize = 14;
				dropdownStyle.fixedHeight = 20;
			}

			if ( modifierNames == null || modifierNames.Length == 0 )
			{
				List<string>	mods = new List<string>();
				mods.Add("Select Modifier To Add");

				Type[] types = Assembly.GetAssembly(typeof(MegaModifier)).GetTypes();

				for ( int i = 0; i < types.Length; i++ )
				{
					if ( types[i].IsSubclassOf(typeof(MegaModifier)) )
					{
						string n = types[i].Name.Replace("Mega", "");

						string str = string.Concat(n.Select(x => System.Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

						mods.Add(str);
					}
				}

				modifierNames = mods.ToArray();
			}

			if ( mod.IsSkinned() )
			{
				if ( blendNames == null || blendNames.Length == 0 )
					GetBlendNames();
			}

			if ( mod.originalMesh && !mod.originalMesh.isReadable )    //mod.mesh && !mod.mesh.isReadable )
			{
				EditorGUILayout.BeginVertical("box");
				EditorGUILayout.HelpBox("Mesh is not readable\nPlease change the Read/Write Import Setting for this Object.\n", MessageType.Warning);
				EditorGUILayout.EndVertical();
			}
			else
			{
				EditorGUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("Add Modifier");
				int addmod = EditorGUILayout.Popup(0, modifierNames, dropdownStyle);
				if ( addmod > 0 )
				{
					Type mtype = GetComponentType(modifierNames[addmod]);
					if ( mtype != null )
					{
						MegaModifier newmod = (MegaModifier)mod.gameObject.AddComponent(mtype);
						mod.BuildList();
						if ( mod.tmproObj )
							mod.dynamicMesh = true;

						ApplyModsToGroup(mod);
					}
				}
				EditorGUILayout.EndVertical();

				mod.quickEdit = EditorGUILayout.Foldout(mod.quickEdit, "Quick Edit (Other Params Below)", FoldoutStyle);
				if ( mod.quickEdit )
				{
					EditorGUILayout.BeginVertical(EditorStyles.helpBox);
					mod.scrollpos = EditorGUILayout.BeginScrollView(mod.scrollpos, GUILayout.Height(mod.quickEditSize));

					for ( int i = 0; i < mod.mods.Length; i++ )
					{
						EditorGUILayout.BeginHorizontal();

						mod.mods[i].ModEnabled = EditorGUILayout.Toggle("", mod.mods[i].ModEnabled, GUILayout.Width(36));
						if ( mod.mods[i].Label.Length > 0 )
							mod.mods[i].showParams = EditorGUILayout.BeginFoldoutHeaderGroup(mod.mods[i].showParams, mod.mods[i].ModName() + " [" + mod.mods[i].Label + "]");
						else
							mod.mods[i].showParams = EditorGUILayout.BeginFoldoutHeaderGroup(mod.mods[i].showParams, mod.mods[i].ModName());

						if ( GUILayout.Button("-", GUILayout.Width(20)) )
						{
							MegaModifier m = mod.mods[i];
							mod.mods[i] = null;
							DestroyImmediate(m);
							mod.BuildList();
							ApplyModsToGroup(mod);
						}

						EditorGUILayout.EndHorizontal();

						if ( i < mod.mods.Length && mod.mods[i].showParams )
						{
							foreach ( FieldInfo field in mod.mods[i].GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default) )
							{
								Adjust[] attribs = (Adjust[])field.GetCustomAttributes(typeof(Adjust), true);

								if ( attribs.Length > 0 )
								{
									string na = string.Concat(field.Name.Select(x => System.Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

									//string na = field.Name;

									na = char.ToUpper(na[0]) + na.Substring(1);

									if ( field.FieldType == typeof(float) )
									{
										float ov = (float)field.GetValue(mod.mods[i]);
										float v;

										if ( attribs[0].min != attribs[0].max )
											v = EditorGUILayout.Slider("\t" + na, ov, attribs[0].min, attribs[0].max);
										else
											v = EditorGUILayout.FloatField("\t" + na, ov);

										if ( v != ov )
										{
											Undo.RecordObject(mod.mods[i], "Changed " + name);
											field.SetValue(mod.mods[i], v);
										}
									}

									if ( field.FieldType == typeof(bool) )
									{
										bool ov = (bool)field.GetValue(mod.mods[i]);
										bool v = EditorGUILayout.Toggle("\t" + na, ov);
										if ( v != ov )
										{
											Undo.RecordObject(mod.mods[i], "Changed " + name);
											field.SetValue(mod.mods[i], v);
										}
									}

									if ( field.FieldType == typeof(Vector3) )
									{
										Vector3 ov = (Vector3)field.GetValue(mod.mods[i]);
										Vector3 v = EditorGUILayout.Vector3Field("\t" + na, ov);
										if ( v != ov )
										{
											Undo.RecordObject(mod.mods[i], "Changed " + name);
											field.SetValue(mod.mods[i], v);
										}
									}

									if ( field.FieldType == typeof(Vector2) )
									{
										Vector2 ov = (Vector3)field.GetValue(mod.mods[i]);
										Vector2 v = EditorGUILayout.Vector2Field("\t" + na, ov);
										if ( v != ov )
										{
											Undo.RecordObject(mod.mods[i], "Changed " + name);
											field.SetValue(mod.mods[i], v);
										}
									}

									if ( field.FieldType == typeof(MegaAxis) )
									{
										MegaAxis ov = (MegaAxis)field.GetValue(mod.mods[i]);
										MegaAxis v = (MegaAxis)EditorGUILayout.EnumPopup("\t" + na, ov);
										if ( v != ov )
										{
											Undo.RecordObject(mod.mods[i], "Changed " + name);
											field.SetValue(mod.mods[i], v);
										}
									}

									if ( field.FieldType == typeof(Falloff) )
									{
										Falloff ov = (Falloff)field.GetValue(mod.mods[i]);
										Falloff v = (Falloff)EditorGUILayout.EnumPopup("\t" + na, ov);
										if ( v != ov )
										{
											Undo.RecordObject(mod.mods[i], "Changed " + name);
											field.SetValue(mod.mods[i], v);
										}
									}

									if ( field.FieldType == typeof(AnimationCurve) )
									{
										EditorGUI.BeginChangeCheck();
										AnimationCurve newcrv = EditorGUILayout.CurveField("\t" + na, (AnimationCurve)field.GetValue(mod.mods[i]));
										if ( EditorGUI.EndChangeCheck() )
										{
											Undo.RecordObject(target, "Changed " + name);
											field.SetValue(mod.mods[i], newcrv);
										}
									}

									if ( field.FieldType == typeof(GameObject) )
									{
										if ( attribs[0].name == "Warp" )
										{
											MegaWarp warp = null;
											GameObject go = (GameObject)field.GetValue(mod.mods[i]);
											if ( go )
												warp = go.GetComponent<MegaWarp>();

											MegaWarp ngo = (MegaWarp)EditorGUILayout.ObjectField("\t" + na, warp, typeof(MegaWarp), true);

											if ( warp != ngo )
											{
												Undo.RecordObject(mod.mods[i], "Changed " + name);

												if ( ngo )
													field.SetValue(mod.mods[i], ngo.gameObject);
												else
													field.SetValue(mod.mods[i], null);
											}
										}
										else
										{
											GameObject go = (GameObject)field.GetValue(mod.mods[i]);
											GameObject ngo = (GameObject)EditorGUILayout.ObjectField("\t" + na, go, typeof(GameObject), true);
											if ( go != ngo )
											{
												Undo.RecordObject(mod.mods[i], "Changed " + name);
												field.SetValue(mod.mods[i], ngo);
											}
										}
									}

									if ( field.FieldType == typeof(Transform) )
									{
										Transform ov = (Transform)field.GetValue(mod.mods[i]);
										Transform v = (Transform)EditorGUILayout.ObjectField("\t" + na, ov, typeof(Transform), true);
										if ( v != ov )
										{
											Undo.RecordObject(mod.mods[i], "Changed " + name);
											field.SetValue(mod.mods[i], v);
										}
									}
								}
							}
						}
						EditorGUILayout.EndFoldoutHeaderGroup();
					}

					EditorGUILayout.EndScrollView();
					EditorGUILayout.EndVertical();
				}

				mod.showParams = EditorGUILayout.BeginFoldoutHeaderGroup(mod.showParams, "Settings");
				if ( mod.showParams )
				{
					EditorGUILayout.BeginVertical("box");
					MegaEditorGUILayout.Toggle(target, "GlobalDisplayGizmos", ref MegaModifyObject.GlobalDisplay);
					MegaEditorGUILayout.Toggle(target, "Enabled", ref mod.Enabled);
					MegaEditorGUILayout.Toggle(target, "Auto Disable", ref mod.autoDisable);
					MegaEditorGUILayout.Toggle(target, "No Jobs", ref mod.noJobs);
					MegaNormalMethod method = mod.NormalMethod;
					MegaEditorGUILayout.NormalMethod(target, "Normal Method", ref mod.NormalMethod);
					MegaEditorGUILayout.TangentMethod(target, "Tangents Method", ref mod.TangentMethod);
					MegaEditorGUILayout.Toggle(target, "Recalc Bounds", ref mod.recalcbounds);
					MegaEditorGUILayout.Toggle(target, "Recalc Collider", ref mod.recalcCollider);
					mod.UpdateMode		= (MegaUpdateMode)EditorGUILayout.EnumPopup("Update Mode", mod.UpdateMode);
					MegaEditorGUILayout.Toggle(target, "Invisible Update", ref mod.InvisibleUpdate);
					bool dynamicMesh = mod.dynamicMesh;
					MegaEditorGUILayout.Toggle(target, "Dynamic Mesh", ref mod.dynamicMesh);
					if ( dynamicMesh != mod.dynamicMesh )
					{
						mod.GetMesh(true);
					}

					MegaEditorGUILayout.Toggle(mod, "Use Color Selection", ref mod.useColSection);
					MegaColor lcol = mod.selectChannel;
					MegaEditorGUILayout.Channel(mod, "Channel", ref mod.selectChannel);
					if ( lcol != mod.selectChannel && mod.mask )
					{
						mod.CalcMaskWeights();
					}
					Texture2D lmask = mod.mask;
					MegaEditorGUILayout.Texture2D(mod, "Mask", ref mod.mask, false);
					if ( mod.mask )
					{
						if ( !mod.mask.isReadable )
						{
							EditorUtility.DisplayDialog("Texture Not Readable", "The selected texture is not readable, please change its Import Settings", "Ok");
							mod.mask = null;
						}
					}
					if ( mod.mask && mod.mask != lmask )
						mod.CalcMaskWeights();

					MegaEditorGUILayout.Slider(mod, "Selection Weight", ref mod.selectionWeight, 0.0f, 1.0f);

					mod.quickEditSize = EditorGUILayout.IntField("Quick Edit Size", mod.quickEditSize);
					if ( mod.quickEditSize < 100 )
						mod.quickEditSize = 100;
					if ( mod.quickEditSize > 400 )
						mod.quickEditSize = 400;

					MegaEditorGUILayout.Toggle(target, "Draw Gizmos", ref mod.DrawGizmos);
					EditorGUILayout.EndVertical();

					if ( mod.NormalMethod != method && mod.NormalMethod == MegaNormalMethod.Mega )
						mod.BuildNormalMappingNew(mod.mesh, false);

					if ( GUILayout.Button("Bake Deformation To Prefab") )
						CreatePrefab(mod);
				}
				EditorGUILayout.EndFoldoutHeaderGroup();

				//mod.showGroup = EditorGUILayout.Foldout(mod.showGroup, "Group/Collider Objects");
				mod.showGroup = EditorGUILayout.BeginFoldoutHeaderGroup(mod.showGroup, "Group/Collider Objects");
				if ( mod.showGroup )
				{
					EditorGUILayout.BeginVertical("box");

					MegaEditorGUILayout.Toggle(target, "Group Enabled", ref mod.groupEnabled);
					for ( int i = 0; i < mod.group.Count; i++ )
					{
						EditorGUILayout.BeginHorizontal();

						GameObject oldgobj = null;

						MegaModifyObject mobj = mod.group[i];
						//MegaModifyObject newobj = (MegaModifyObject)EditorGUILayout.ObjectField("", mod.group[i], typeof(MegaModifyObject), true);

						if ( mobj )
							mobj.Enabled = EditorGUILayout.Toggle("", mobj.Enabled, GUILayout.Width(20));
						else
							EditorGUILayout.Toggle("", false, GUILayout.Width(20));

						GameObject newgobj = null;

						if ( mobj )
						{
							oldgobj = mobj.gameObject;
							newgobj = (GameObject)EditorGUILayout.ObjectField("", oldgobj, typeof(GameObject), true);
						}
						else
							newgobj = (GameObject)EditorGUILayout.ObjectField("", null, typeof(GameObject), true);

						if ( newgobj != oldgobj )
						{
							if ( newgobj )
							{
								MegaModifyObject newobj = (MegaModifyObject)newgobj.GetComponent<MegaModifyObject>();

								if ( !newobj )
									newobj = newgobj.AddComponent<MegaModifyObject>();

								newobj.hideFlags = HideFlags.HideInInspector;

								mod.group[i] = newobj;

								if ( newobj )
								{
									ApplyModsToObject(mod, newobj);

									//Vector3 offset = mod.transform.InverseTransformPoint(newobj.transform.position);
									//if ( newobj.mods != null )
									//{
										//for ( int m = 0; m < newobj.mods.Length; m++ )
											//newobj.mods[m].Offset = offset;
									//}
								}
							}
							else
							{
								if ( oldgobj )
								{
									MegaModifyObject mo = oldgobj.GetComponent<MegaModifyObject>();
									if ( mo )
									{
										mo.hideFlags = HideFlags.None;

										for ( int m = 0; m < mo.mods.Length; m++ )
											mo.mods[m].hideFlags = HideFlags.None;
									}
								}
								mod.group[i] = null;
							}
						}

						if ( GUILayout.Button("-", GUILayout.Width(20)) )
						{
							MegaModifyObject mo = mod.group[i];
							if ( mo )
							{
								mo.hideFlags = HideFlags.None;

								for ( int m = 0; m < mo.mods.Length; m++ )
									mo.mods[m].hideFlags = HideFlags.None;
							}
							mod.group.RemoveAt(i);
						}

						EditorGUILayout.EndHorizontal();
					}

					EditorGUILayout.BeginHorizontal();
					if ( GUILayout.Button("Add Group Slot") )
						mod.group.Add(null);

					if ( GUILayout.Button("Add All Child Meshes") )
						AddChildObjects(mod);

					EditorGUILayout.EndHorizontal();

					GameObject oldcolobj = null;
					GameObject newcolobj = null;
					MegaModifyObject colobj = mod.colliderObject;

					if ( colobj )
						oldcolobj = colobj.gameObject;

					newcolobj = (GameObject)EditorGUILayout.ObjectField("Collider Object", oldcolobj, typeof(GameObject), true);
					if ( newcolobj != oldcolobj )
					{
						if ( newcolobj )
						{
							MegaModifyObject newobj = (MegaModifyObject)newcolobj.GetComponent<MegaModifyObject>();

							if ( !newobj )
							{
								newobj = newcolobj.AddComponent<MegaModifyObject>();
								newobj.recalcbounds = false;
								newobj.recalcCollider = false;
								newobj.NormalMethod = MegaNormalMethod.None;
								newobj.TangentMethod = MegaTangentMethod.None;
							}

							newobj.hideFlags = HideFlags.HideInInspector;
							mod.colliderObject = newobj;

							if ( newobj )
								ApplyModsToObject(mod, newobj);
						}
						else
						{
							if ( oldcolobj )
							{
								MegaModifyObject mo = oldcolobj.GetComponent<MegaModifyObject>();
								if ( mo )
								{
									mo.hideFlags = HideFlags.None;

									for ( int m = 0; m < mo.mods.Length; m++ )
										mo.mods[m].hideFlags = HideFlags.None;
								}
							}
							mod.colliderObject = null;
						}
					}

					if ( GUILayout.Button("Apply Modifers to Group/Collider") )
						ApplyModsToGroup(mod);

					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndFoldoutHeaderGroup();


				if ( mod.IsSkinned() )
				{
					if ( blendNames == null || blendNames.Length != mod.mesh.blendShapeCount )
						GetBlendNames();

					MegaBlendShapeHolder holder = mod.GetComponent<MegaBlendShapeHolder>();
					if ( !holder )
					{
						holder = mod.gameObject.AddComponent<MegaBlendShapeHolder>();
						//holder.hideFlags = HideFlags.HideInInspector;
						holder.GrabBlendShapes(mod);
					}
					else
					{
						if ( mod.mesh.blendShapeCount != holder.channels.Count )
							holder.SetBlendShapes(mod);
					}

					bool grabshapes = false;

					mod.showBlendShapes = EditorGUILayout.BeginFoldoutHeaderGroup(mod.showBlendShapes, "Blend Shapes Workshop");

					if ( mod.showBlendShapes )
					{
						if ( blendNames != null && blendNames.Length > 0 )
						{
							mod.blendIndex = Mathf.Clamp(mod.blendIndex, 0, mod.mesh.blendShapeCount);
							mod.blendIndex = EditorGUILayout.Popup(mod.blendIndex, blendNames);

							EditorGUILayout.LabelField("Frames");
							for ( int i = 0; i < mod.mesh.GetBlendShapeFrameCount(mod.blendIndex); i++ )
							{
								EditorGUILayout.BeginHorizontal();
							
								float nweight = EditorGUILayout.Slider(i + " Weight ", mod.mesh.GetBlendShapeFrameWeight(mod.blendIndex, i), 0.0f, 100.0f);
								if ( nweight != mod.mesh.GetBlendShapeFrameWeight(mod.blendIndex, i) )
								{
									MegaBlendshapeWorkshop.ChangeFrameWeight(mod, blendNames[mod.blendIndex], i, nweight);
									EditorUtility.SetDirty(target);
									EditorUtility.SetDirty(mod.originalMesh);
									grabshapes = true;
								}

								if ( GUILayout.Button("-", GUILayout.Width(20)) )
								{
									MegaBlendshapeWorkshop.RemoveBlendShapeFrame(mod, blendNames[mod.blendIndex], i);
									EditorUtility.SetDirty(target);
									EditorUtility.SetDirty(mod.originalMesh);
									grabshapes = true;
								}

								EditorGUILayout.EndHorizontal();
							}

							EditorGUILayout.BeginHorizontal();
							mod.blendShapeWeight = EditorGUILayout.Slider("Add Blend Frame Weight", mod.blendShapeWeight, 0.0f, 100.0f);

							if ( GUILayout.Button("+", GUILayout.Width(20)) )
							{
								MegaBlendshapeWorkshop.AddBlendShapeFrame(mod, blendNames[mod.blendIndex], mod.blendShapeWeight);
								EditorUtility.SetDirty(target);
								EditorUtility.SetDirty(mod.originalMesh);
								grabshapes = true;
							}
							EditorGUILayout.EndHorizontal();

							if ( GUILayout.Button("Remove Blend") )
							{
								MegaBlendshapeWorkshop.RemoveChannel(mod, blendNames[mod.blendIndex]);
								GetBlendNames();
								EditorUtility.SetDirty(target);
								EditorUtility.SetDirty(mod.originalMesh);
								grabshapes = true;
							}
						}
						else
						{
							//mod.blendShapeWeight = EditorGUILayout.Slider("Add Blend Frame Weight", mod.blendShapeWeight, 0.0f, 100.0f);
						}

						mod.blendShapeName = EditorGUILayout.TextField("New Blend Name", mod.blendShapeName);
						if ( GUILayout.Button("Add New Blend") )
						{
							MegaBlendshapeWorkshop.AddBlendShapeFrame(mod, mod.blendShapeName, 100.0f);	//mod.blendShapeWeight);
							GetBlendNames();
							EditorUtility.SetDirty(target);
							EditorUtility.SetDirty(mod.originalMesh);
							EditorUtility.SetDirty(mod.mesh);
							EditorUtil.SetDirty(target);
							grabshapes = true;
						}

						EditorGUILayout.EndFoldoutHeaderGroup();

						if ( grabshapes )
						{
							holder.GrabBlendShapes(mod);
						}
					}
				}

				EditorGUILayout.BeginHorizontal();
				if ( GUILayout.Button("Attach Children") )
					mod.AttachChildren();

				if ( GUILayout.Button("Detach Children") )
					mod.DetachChildren();
				EditorGUILayout.EndHorizontal();
			}

			if ( GUI.changed )
				EditorUtility.SetDirty(target);
		}

		[MenuItem("MegaFiers/Convert to Skinned")]
		static public void ConvertToSkinned()
		{
			GameObject[] objs = Selection.gameObjects;

			for ( int i = 0; i < objs.Length; i++ )
			{
				SkinnedMeshRenderer sr = objs[i].GetComponent<SkinnedMeshRenderer>();

				if ( !sr )
				{
					MeshFilter mf = objs[i].GetComponent<MeshFilter>();
					MeshRenderer mr = objs[i].GetComponent<MeshRenderer>();

					if ( mf && mr )
					{
						Mesh mesh = mf.sharedMesh;
						Material[] mats = mr.sharedMaterials;

						DestroyImmediate(mr);
						DestroyImmediate(mf);

						sr = objs[i].AddComponent<SkinnedMeshRenderer>();

						sr.sharedMesh = mesh;
						sr.sharedMaterials = mats;
					}
				}
			}
		}

		[MenuItem("MegaFiers/Deform Object")]
		static public void DeformObject()
		{
			GameObject[] objs = Selection.gameObjects;

			for ( int i = 0; i < objs.Length; i++ )
			{
				MegaModifyObject modobj = objs[i].GetComponent<MegaModifyObject>();

				if ( !modobj )
				{
					Renderer r = objs[i].GetComponent<Renderer>();
					if ( r )
						objs[i].AddComponent<MegaModifyObject>();
				}
			}
		}

		[MenuItem("MegaFiers/Create Deformable Skin")]
		static public void CreateDeformableSkin()
		{
			MegaModifyObject.CreateDeformableSkin(Selection.activeGameObject);
		}

#if false	// set to flase if Probuilder not in your project
		[MenuItem("MegaFiers/Remove Probuilder")]
		static public void RemoveProBuilder()
		{
			GameObject[] objs = Selection.gameObjects;

			for ( int i = 0; i < objs.Length; i++ )
			{
				UnityEngine.ProBuilder.ProBuilderMesh promesh = objs[i].GetComponent<UnityEngine.ProBuilder.ProBuilderMesh>();

				if ( promesh )
				{
					MeshFilter mf = objs[i].GetComponent<MeshFilter>();
					if ( mf )
					{
						Mesh m = MegaUtils.DupMesh(mf.sharedMesh, "");

						if ( m )
						{
							DestroyImmediate(promesh);
							mf.sharedMesh = m;
							MegaModifyObject modobj = objs[i].GetComponent<MegaModifyObject>();
							if ( modobj )
								modobj.dynamicMesh = true;

							if ( !System.IO.Directory.Exists("Assets/MegaPrefabs") )
								AssetDatabase.CreateFolder("Assets", "MegaPrefabs");

							string meshpath = "Assets/MegaPrefabs/" + m.name + ".asset";
							AssetDatabase.CreateAsset(m, meshpath);
							AssetDatabase.SaveAssets();
							AssetDatabase.Refresh();
						}
					}
				}
			}
		}
#endif

		bool InGroup(MegaModifyObject modobj, GameObject obj)
		{
			for ( int i = 0; i < modobj.group.Count; i++ )
			{
				if ( modobj.group[i].gameObject == obj )
					return true;
			}

			return false;
		}

		void AddChildObjects(MegaModifyObject modobj)
		{
			modobj.bbox = modobj.originalMesh.bounds;

			Renderer[] rends = modobj.GetComponentsInChildren<Renderer>();

			for ( int i = 0; i < rends.Length; i++ )
			{
				if ( rends[i] is MeshRenderer || rends[i] is SkinnedMeshRenderer )
				{
					Mesh ms = null;

					if ( rends[i] is MeshRenderer )
						ms = rends[i].GetComponent<MeshFilter>().sharedMesh;
					else
						ms = ((SkinnedMeshRenderer)(rends[i])).sharedMesh;

					if ( !InGroup(modobj, rends[i].gameObject) )
					{
						// Add obj
						MegaModifyObject newobj = (MegaModifyObject)rends[i].GetComponent<MegaModifyObject>();

						if ( !newobj )
						{
							newobj = rends[i].gameObject.AddComponent<MegaModifyObject>();
							newobj.hideFlags = HideFlags.HideInInspector;
							newobj.recalcbounds		= false;
							newobj.recalcCollider	= false;
							newobj.NormalMethod		= MegaNormalMethod.None;
							newobj.TangentMethod	= MegaTangentMethod.None;

							modobj.group.Add(newobj);
						}
						else
						{
							// Should destoy all mega stuff
							newobj.hideFlags = HideFlags.HideInInspector;
						}
					}
				}
			}

			ApplyModsToGroup(modobj);
		}

		void ApplyModsToGroup(MegaModifyObject modobj)
		{
			for ( int g = 0; g < modobj.group.Count; g++ )
			{
				MegaModifyObject tobj = modobj.group[g];
				ApplyModsToObject(modobj, tobj);
			}

			if ( modobj.colliderObject )
			{
				ApplyModsToObject(modobj, modobj.colliderObject);
			}
		}

		void ApplyModsToObject(MegaModifyObject modobj, MegaModifyObject tobj)
		{
			if ( tobj )
			{
				//Matrix4x4 mat = Matrix4x4.TRS(modobj.transform.position, modobj.transform.rotation, Vector3.one);
				//Vector3 offset = mat.inverse.MultiplyPoint3x4(tobj.transform.position);

				Vector3 offset = modobj.transform.InverseTransformPoint(tobj.transform.position);

				tobj.inGroup = true;

				// Not ideal as may have added extras?
				MegaModifier[] mods = tobj.GetComponents<MegaModifier>();
				for ( int i = 0; i < mods.Length; i++ )
					DestroyImmediate(mods[i]);

				for ( int i = 0; i < modobj.mods.Length; i++ )
				{
					MegaModifier mod = modobj.mods[i];
					MegaModifier m = (MegaModifier)tobj.GetComponent(mod.GetType());

					if ( !m )
						m = (MegaModifier)tobj.gameObject.AddComponent(mod.GetType());

					if ( m )
					{
						m.hideFlags = HideFlags.HideInInspector;
						EditorUtility.CopySerialized(mod, m);
						m.Offset = offset;
					}
				}

				if ( tobj.tmproObj )
					tobj.dynamicMesh = true;
			}
		}

		static public GameObject CreatePrefab(MegaModifyObject root)
		{
			GameObject newobj = GameObject.Instantiate(root.gameObject);

			newobj.name = newobj.name.Replace("(Clone)", "");

			if ( !System.IO.Directory.Exists("Assets/MegaPrefabs") )
				AssetDatabase.CreateFolder("Assets", "MegaPrefabs");

			MegaModifyObject[] modobjs = newobj.GetComponents<MegaModifyObject>();

			for ( int i = 0; i < modobjs.Length; i++ )
			{
				Mesh mesh = modobjs[i].mesh;

				string mname = mesh.name;
				int ix = mname.IndexOf("Instance");
				if ( ix != -1 )
					mname = mname.Remove(ix);

				string meshpath = "Assets/MegaPrefabs/" + mname + ".asset";
				meshpath = AssetDatabase.GenerateUniqueAssetPath(meshpath);
				AssetDatabase.CreateAsset(mesh, meshpath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			GameObject prefab = PrefabUtility.SaveAsPrefabAsset(newobj, "Assets/MegaPrefabs/" + newobj.name + ".prefab");

			modobjs = prefab.GetComponents<MegaModifyObject>();

			for ( int i = 0; i < modobjs.Length; i++ )
			{
				MegaModifier[] mods = modobjs[i].GetComponents<MegaModifier>();

				for ( int j = 0; j < mods.Length; j++ )
				{
					DestroyImmediate(mods[j], true);
					mods[j] = null;
				}

				modobjs[i].restorekeep = 1;
				DestroyImmediate(modobjs[i], true);
			}

			return prefab;
		}
	}
}